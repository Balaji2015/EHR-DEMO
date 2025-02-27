var tab = [];
var tab_to_disable;
$(document).ready(function () {
    localStorage.setItem("PrevSubTab", "");
    var now = new Date();
    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
    document.cookie = "VitalCurrentDate=" + utc;
    top.window.document.getElementById('ctl00_C5POBody_hdnIsSaveEnable').value = "false";
    HideLoading();
    var humanid = "";
    if (window.parent.parent.document.getElementsByName('lblPatientStrip')[0] != undefined &&
        window.parent.parent.document.getElementsByName('lblPatientStrip')[0].innerText.split('|').length > 3)
        humanid = window.parent.parent.document.getElementsByName('lblPatientStrip')[0].innerText.split('|')[4].split(':')[1].trim();
    $('#tbVitals').attr("data-src", "frmVitals.aspx?Openingfrom=Queue&HumanID=" + humanid);
    tab_to_disable = $("#pnlEncScroll li[disabled] a");
    for (var i = 0; i < tab_to_disable.length; i++) {
        tab.push(tab_to_disable[i].attributes.getNamedItem("href").value);
        $("#myTabs a[href*='" + tab[i] + "']").addClass("disableTab");
    }

    if ($('#hdnTab')[0].value != null && $('#hdnTab')[0].value != "") {
        if ($('#hdnTab')[0].value == 'tbSummary') {
            //$('#myTabs li:eq(12) a').tab('show');
            //Jira #CAP-492
            //$(top.window.document).find('#ctl00_C5POBody_EncounterContainer')[0].src = "frmSummaryNew.aspx?EncounterId=" + document.getElementById('hdnEncounterIDSummary').value
            $(top.window.document).find('#ctl00_C5POBody_EncounterContainer')[0].src = "frmSummaryNew.aspx?EncounterId=" + document.getElementById('hdnEncounterIDSummary').value + "&TabMode=true";;
        }
        else if ($('#hdnTab')[0].value == 'tbPlan') {
            $('#myTabs li:eq(10) a').tab('show');
        }
        else if ($('#hdnTab')[0].value == 'tbEandM') {
            $('#myTabs li:eq(11) a').tab('show');
        }
    }

    else if ($("ul#myTabs li.active").length != 0) {
        var target = $("ul#myTabs li.active")[0].childNodes[1];
        paneID = $(target).attr('href');
        src = $(paneID).attr('data-src');
        $(paneID).attr('data-src', src);
        $(paneID + " iframe").attr("src", src);

    }
    $('#myTabs li a').click(function () {
        var is_disabled = false;
        if (tab != undefined && tab.length > 0) {
            for (var t = 0; t < tab.length; t++) {
                if (this.hash == tab[t]) {
                    event.stopPropagation();
                    is_disabled = true;
                }
            }
            if (is_disabled == false) {
                localStorage.setItem("notification", "");
                if ($("ul#myTabs li.active a")[0].innerText != this.innerText) {
                    sessionStorage.setItem("Encounter_PrevTabRevert", "false");
                    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
                }
            }
        }
        else {
            localStorage.setItem("notification", "");
            if ($("ul#myTabs li.active a")[0].innerText != this.innerText) {
                sessionStorage.setItem("Encounter_PrevTabRevert", "false");
                { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
            }
        }
    });
    $(document).on("keydown", function (e) {

        if (e.which === 8 && e.currentTarget == document) {
            e.preventDefault();
        }
    });
    $("#btnHiddenTab").click(function () {
        event.preventDefault();
        var PrevTabtxt = sessionStorage.getItem('EncPrevTabText');
        $("a:contains('" + PrevTabtxt + "')").tab('show');
    });
});


function isSaveEnabledButton(sender, args) {
    document.getElementById("hdnCopyPreviousClick").value = "";
    if (window.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value == "true") {
        HideLoading();
        if (args != null || args != undefined) {

            var TabClick = document.getElementById('hdnTabClick');
            if (TabClick.value == "first") {
                if (window.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value == "true") {
                    TabClick.value = sender._uniqueID + "$#$";
                    args.set_cancel(true);
                    HideLoading();
                    DisplayErrorMessage('1100000', 'MoveToButtonsClick');
                    return;
                }
            } else {
                var splitvalue = TabClick.value.split('$#$');
                var clicked_tab = splitvalue[0];
                var switchcase = splitvalue[1];
                if (switchcase == "second,true") {
                    var IDs = document.getElementById('hdnSaveButtonID').value.split(',');
                    if (IDs.length == 1) {
                        var save_button = window.frames[0].frameElement.contentDocument.getElementById(IDs[0]);
                        if (save_button == null)
                            save_button = $find('pageContainer').get_selectedPageView().get_element().getElementsByTagName("iframe")[0].contentDocument.getElementById(IDs[0]);
                        if (save_button != undefined || save_button != null) {
                            args.set_cancel(true);
                            TabClick.value = clicked_tab + "$#$third";
                            save_button.click();
                            return;
                        }
                    } else if (IDs.length == 2) {
                        var childControlsofParentContainer = $find('pageContainer').get_selectedPageView().get_element().getElementsByTagName("iframe")[0].contentWindow.$telerik.radControls;
                        for (var count = (childControlsofParentContainer.length - 1) ; count >= 0; count--) {
                            if (childControlsofParentContainer[count]._element.id == IDs[1]) {
                                var MultiPage = childControlsofParentContainer[count];
                                break;
                            }
                        }
                        var childControlsofChildContainer = MultiPage.get_selectedPageView().get_element().getElementsByTagName("iframe")[0].contentWindow.$telerik.radControls;
                        for (var count = (childControlsofChildContainer.length - 1) ; count >= 0; count--) {
                            if (childControlsofChildContainer[count]._element.id == IDs[0]) {
                                var save_button = childControlsofChildContainer[count];
                                if (MultiPage.get_selectedPageView()._contentUrl.indexOf('frmOtherHistory.aspx') > -1)
                                    var add_button = MultiPage.get_selectedPageView().get_element().getElementsByTagName("iframe")[0].contentDocument.getElementById('btnSave');
                                if (save_button != undefined && save_button != null) {
                                    args.set_cancel(true);
                                    TabClick.value = clicked_tab + "$#$third";
                                    save_button.click();
                                    if (add_button != undefined && add_button != null) {
                                        if (add_button.control._enabled)
                                            add_button.click();
                                    }
                                    return;
                                }
                                break;
                            }
                        }
                    }
                } else if (switchcase == "second,false") {
                    window.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                    document.getElementById('hdnPurposeOfVisit_SaveRejected').value = true;
                } else if (switchcase == "second,cancel") {
                    args.set_cancel(true);
                }
                TabClick.value = "first";
            }
        }
    }
    else {
        window.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        document.getElementById('hdnTabClick').value = "first";
    }
    var now = new Date();
    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear();
    utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
    document.getElementById(GetClientId("hdnLocalTime")).value = utc;

    if (sender._text == "Move to Next Process" || sender._text == "Move to Provider") {
        //Added by Muthu on 5-Mar-2015
        var checkAutoSave = localStorage.getItem('AvutoSaveCheck');
        if (checkAutoSave != "true") {
            if (DisplayErrorMessage('180026', '', '')) {
                localStorage.setItem('AvutoSaveCheck', 'true');
                if (document.getElementById('btnMove_input') != null)
                    document.getElementById('btnMove_input').click();
                sender.set_autoPostBack(true);
            }
            else {
                HideLoading();
                sender.set_autoPostBack(false);
            }
        }
        else {
            localStorage.removeItem('AvutoSaveCheck');
            if (document.getElementById('btnMove_input') != null)
                document.getElementById('btnMove_input').click();
            sender.set_autoPostBack(true);
        }
    }

}

function OnClientCloseEncounter(oWindow, args) {
    var arg = args.get_argument();
    if (arg == true) {
        document.getElementById('hdnACOValidated').value = "true";
        var btnMoveClient = document.getElementById("btnMove");
        if (btnMoveClient != null) {
            btnMoveClient.click();
        }

    } else if (arg == false) {
        DisplayErrorMessage('180062');
    }

    oWindow.remove_close(OnClientCloseEncounter);
}

function OnClientCloseMA(oWindow, args) {
    var arg = args.get_argument();
    if (arg == true) {
        var btnMoveClient = document.getElementById("btnMoveToMA");
        if (btnMoveClient != null) {
            btnMoveClient.click();
        }

    } else if (arg == false) {
        DisplayErrorMessage('180062');
    }

    oWindow.remove_close(OnClientCloseMA);
}

function OnClientCloseForPrintDocument(oWindow, args) {
    var arg = args.get_argument();
    if (arg != null || undefined) {
        var btnMoveClient = document.getElementById("btnPhysiciancorrection");
        if (btnMoveClient != null) {
            document.getElementById('hdnACOValidated').value = "true";
            btnMoveClient.click();
        }

    } else if (arg == null || undefined) {
        DisplayErrorMessage('180062');
    }

    oWindow.remove_close(OnClientCloseForPrintDocument);
}

function GetUTCTime() {
    var now = new Date();
    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear();
    utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
    document.getElementById("hdnLocalTime").value = utc;
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
    if (oArg.result) {
        oWnd.close(oArg.result);
    } else {
        oWnd.close();
    }
}

function ClosePrintDocuments(oWindow, args) {
    if (args != null) {
        if (args._argument != null) {

            var btnMoveClient = document.getElementById("btnMove").value;
            if (btnMoveClient == "Move to Next Process") {
                window.top.location.href = "frmMyQueueNew.aspx";
            } else {
                var button2 = $find("btnPhysiciancorrection");
                button2.set_enabled(false);
                window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
            }

        }
    }
    oWindow.remove_close(ClosePrintDocuments);
}

function OnCheckoutMAprocess(oWindow, args) {
    if (args != null) {
        if (args._argument != null) {
            window.top.location.href = "frmMyQueueNew.aspx";

        }

    }
}

function OnCheckoutClosePrintDocuments(oWindow, args) {
    if (args != null) {
        if (args._argument != null) {
            if ($('#btnPhysiciancorrection') != undefined) {
                $('#btnPhysiciancorrection')[0].disabled = true;
                document.getElementById('hdnChkOut').value = "false";
                __doPostBack('btnPhysiciancorrection', 'OnClick');
            }
        } else {
            $('#btnPhysiciancorrection')[0].disabled = false;
            document.getElementById('hdnChkOut').value = "true";
        }
        localStorage.setItem("bSave", "true");
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    }
}


function OpenPrintDocuments(oWindow, args) {
    var arg = args.get_argument();
    if (arg != undefined && arg.UNAME != undefined) {
        var objPrintDocuments = new Array();
        objPrintDocuments.push("EID=" + arg.EID);
        objPrintDocuments.push("SPHYID=" + arg.SPHYID);
        objPrintDocuments.push("HID=" + arg.HID);
        objPrintDocuments.push("UNAME=" + arg.UNAME);
        objPrintDocuments.push("BUTTONNAME=" + arg.BUTTONNAME);
        objPrintDocuments.push("HNAME=" + arg.HNAME);
        objPrintDocuments.push("EPROID=" + arg.EPROID);
        objPrintDocuments.push("MTR=" + arg.MTR);
        objPrintDocuments.push("DS=" + arg.DS);
        var PrintDocumentsWindow = openModal("frmPrintDocuments.aspx", 730, 855, objPrintDocuments, 'MessageWindow');
        var objWindow = $find("MessageWindow");
        objWindow.add_close(ClosePrintDocuments);
        return false;
    }
    oWindow.remove_close(OpenPrintDocuments);
}

function ACOCheckOutBegining(oWindow, args) {
    var arg = args.get_argument();
    if (arg != undefined && arg.UNAME != undefined) {
        var btnphysicianCorreciton = document.getElementById("btnPhysiciancorrection");
        __doPostBack("btnphysicianCorreciton");

    }
    oWindow.remove_close(ACOCheckOutBegining);
}

function CheckOut_ClientClick(btnObj) {
    if (btnObj.value.toString() == "Move To Checkout") {
        if (document.getElementById("hdnIsACOValid").value == "True") {
            var objCheckOutParams = new Array();
            objCheckOutParams.push("IsPFSHVald=" + "Y");
            objCheckOutParams.push("btnName=" + btnObj.value);
            var objACOValidaion = openModal("frmACOValidation.aspx", 250, 550, objCheckOutParams, 'MessageWindow');
            var objWindow = $find("MessageWindow");
            objWindow.add_close(ACOCheckOutBegining);
            return false;
        } else {
            return true;
        }
    } else {
        return true;
    }
}

function OpenPFSHVerificationScreen() {
    var objPFSHVerification = new Array();
    var ResultPFSH = openModal("frmPFSHVerficationYesNo.aspx", 100, 200, objPFSHVerification, 'MessageWindow');
    if (ResultPFSH == true) {
        var btnMoveClient = document.getElementById("btnMove");
        if (btnMoveClient != null) {
            btnMoveClient.click();
        }
    }
}

function pageLoad() {
    var $ = $telerik.$;
    var height = $(window).height();
    var multiPage = $find("<%=pageContainer.ClientID %>");
    var totalHeight = height - 42;
    if (multiPage != null) {
        multiPage.get_element().style.height = totalHeight + "px";
    }
}

function RefreshERX(index) {
    GetUTCTime();
    window.parent.parent.location.href = "frmPatientChart.aspx?tabName=" + "ERX" + "&Index=" + index + "&hdnLocalTime=" + document.getElementById('hdnLocalTime').value;
}



function btnTemplate_Click() {
    var obj = new Array();

    obj.push("TemplateName=" + $find('txtTemplate')._text);

    var Result = openModal("frmTemplate.aspx", 470, 550, obj, "MessageWindow");
    var WindowName = $find('MessageWindow');
    WindowName.add_close(OnOpendPatientChartClick);
    return false;
}

function OnOpendPatientChartClick(oWindow, args) {
    var arg = args.get_argument();
    if (arg) {
        var Result = arg;
        if (Result != null) {
            document.getElementById('txtTemplate').value = Result.TemplateName;
            $find("txtTemplate").set_value(Result.TemplateName);
            document.getElementById('hdnTemaplteId').value = Result.TemplateId;
        }
    }
}

function pnlBarGroupTabs_ContextMenu(sender, args) {
    args._cancel = true;
}

function setRadWindowProperties(childWindow, height, width) {
    childWindow.SetModal(false);
    childWindow.set_visibleStatusbar(false);
    childWindow.setSize(width, height);
    childWindow.set_behaviors(Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Minimize);
    childWindow.set_iconUrl("Resources/16_16.ico");
    childWindow.set_keepInScreenBounds(true);
    childWindow.set_centerIfModal(true);
    childWindow.center();
}

function GetRadWindow() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    return oWindow;
}


function RefreshTab() {
    clickMenuButton("CC / HPI", false);
}


function ShowLoading() {
    document.getElementById("divLoading").style.display = "block";
    top.window.document.getElementById('ctl00_Loading').style.display = 'block';
}
function HideLoading() {
    document.getElementById("divLoading").style.display = 'none';
    if (top.window.document.getElementById('ctl00_Loading') != null) {
        top.window.document.getElementById('ctl00_Loading').style.display = 'none';
    }
}


function clickMenuButton(itemText, autoSaveFlag) {

    try {

        var tabStrip = $find("tabStripEncounter");
        var name = itemText.split("_");
        var tab = tabStrip.findTabByText(name[0]);
        if (tab) {
            tab.select();
            tab.click();
            tab.set_selected(true);

            var PageView = tab.get_pageView();
            PageView.set_selected(true);

            var domElement = PageView.get_element();
            var iFrame = domElement.getElementsByTagName("iframe");


            if (iFrame.length > 0) {
                iFrame[0].src = iFrame[0].src;
            }
            else {
                console.log(iFrame);
                console.log("Cant able to find the Iframe at : " + itemText);
            }

            if (autoSaveFlag == true)
                window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
            else
                window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;

        } else {
            alert("Tab with text '" + text + "' not found.");
        }
    } catch (err) {
        console.log(err.message);
    }
}


function OpenFollowUpEncounter() {
    var childWindow = window.radopen("frmFollowUpEncounter.aspx?OpeningFrom=CopyPreviousClick", "RadFollowUpEncounter");
    setRadWindowProperties(childWindow, 500, 560);
    childWindow.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Minimize);
    document.getElementById("hdnCopyPreviousClick").value = "true";
    document.getElementById("hdnCloseFS").value = "false";
}

function FollowUpWindowClientClose(oWindow, args) {
    oWindow.close();
    document.getElementById("hdnCopyPreviousClick").value = "false";
    document.getElementById("hdnCloseFS").value = "false";

    if ($find("RadWindowManager1") != null) {
        $find("RadWindowManager1").visible = false;
        $find("RadWindowManager1")._visibleOnPageLoad = false;

        $find("RadFollowUpEncounter").visible = false;
        $find("RadFollowUpEncounter")._visibleOnPageLoad = false;
    }
}

function FollowUpWindowClientClosePageLoad(oWindow, args) {
    oWindow.close();
    if ($find("RadWindowManager2") != null) {
        $find("RadWindowManager2").visible = false;
        $find("RadWindowManager2")._visibleOnPageLoad = false;

        $find("WindowMngr").visible = false;
        $find("WindowMngr")._visibleOnPageLoad = false;

        $find("RadWindow2").visible = false;
        $find("RadWindow2")._visibleOnPageLoad = false;
    }
    document.getElementById("hdnCloseFS").value = "false";
}

function chkShowAllPhysicians_CheckedChanged(checkStatus) {    
    //Jira #Cap-360 - Supervising Provider not listed for MA to select in the chart
    //if (checkStatus.firstChild.checked) {
    //    var usedNames = {};
    //    $("#cboPhysicianName > option").each(function () {
    //        if (usedNames[this.text]) {
    //            $(this).css('display', 'none');
    //        } else {
    //            usedNames[this.text] = this.value;
    //            $(this).css('display', 'block');
    //        }
    //    });
    //}
    //else {
    //    $("#cboPhysicianName > option").each(function () {
    //        var option = $(this);
    //        if (option.attr('default') == 'true' || option.attr('default') == '') { option.css('display', 'block'); } else { option.css('display', 'none'); }
    //    });
    //}
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    var DropdownList = document.getElementById("cboPhysicianName");
    var SelectedIndex = DropdownList.value;
    document.getElementById("hdnindex").value = SelectedIndex;
}

function getDropdownListSelectedText() {
    document.getElementById('btnmovetoscribe').style.display = "none";
    var DropdownList = document.getElementById("cboPhysicianName");
    var SelectedIndex = DropdownList.value;
    document.getElementById("hdnindex").value = SelectedIndex;
    
    //Old Code
    //document.getElementById("hdnLocalPhy").value = DropdownList.value + '~' + DropdownList.selectedOptions[0].textContent.split('-')[0];
    //Gitlab# 2485 - Physician Name Display Change
    $.ajax({
        type: "GET",
        url: "ConfigXML/User.json",
        dataType: "json",
        async: false,
        cache: false,
        success: function (json) {
            //$(json).find('User').each(function () {
            //    var cookies = document.cookie.split(';');
            //    var CLegalOrg = "";
            //    for (var l = 0; l < cookies.length; l++) {
            //        if (cookies[l].indexOf("CLegalOrg") > -1)
            //            CLegalOrg = cookies[l].split("=")[1];
            //    }
            //    if ($(this)[0].getAttribute("Physician_Library_ID") == DropdownList.value.toString() && $(this)[0].getAttribute("Legal_Org") == CLegalOrg) {
            //        document.getElementById("hdnLocalPhy").value = DropdownList.value + '~' + $(this)[0].getAttribute("User_Name");

            //    }
            //})
            var cookies = document.cookie.split(';');
            var CLegalOrg = "";
            for (var l = 0; l < cookies.length; l++) {
                if (cookies[l].indexOf("CLegalOrg") > -1)
                    CLegalOrg = cookies[l].split("=")[1];
            }
            $.each(json, function (index, user) {
                if (user.Physician_Library_ID == DropdownList.value.toString() && user.Legal_Org == CLegalOrg) {
                    document.getElementById("hdnLocalPhy").value = DropdownList.value + '~' + user.User_Name;
                }
            });
        }
    });
        
    if (document.getElementById("hdnEncounterProviderId").value == DropdownList.selectedOptions[0].value.toString()) {

        if (document.getElementById("chkProviderReview") != null && document.getElementById("chkProviderReview") != undefined)
            document.getElementById("chkProviderReview").checked = true;
    }
    //CAP-2547
    //if (document.getElementById('hdnUserRole').value.toUpperCase() == "MEDICAL ASSISTANT") {
    if (document.getElementById('hdnCurrentProcess').value != undefined && document.getElementById('hdnCurrentProcess').value != null && document.getElementById('hdnCurrentProcess').value.toUpperCase() == "MA_PROCESS") {

        //Jira CAP-2766
        //$.ajax({
        //    type: "GET",
        //    url: "ConfigXML/CapellaScribeLookupList.xml",
        //    dataType: "xml",
        //    async: false,
        //    cache: false,
        //    success: function (xml) {
        //        var cookies = document.cookie.split(';');
        //        var CLegalOrg = "";
        //        for (var l = 0; l < cookies.length; l++) {
        //            if (cookies[l].indexOf("CLegalOrg") > -1)
        //                CLegalOrg = cookies[l].split("=")[1];
        //        }
        //        $(xml).find('CapellaScribeLookup').each(function () {

        //            if ($(this)[0].getAttribute("ProviderID") == DropdownList.selectedOptions[0].value.toString() && $(this)[0].getAttribute("Legal_Org") == CLegalOrg) {
        //                document.getElementById('btnmovetoscribe').style.display = "block";

        //            }
        //        })
        //    }
        //});

        //Jira CAP-2766
        $.get("ConfigXML/CapellaScribeLookupList.json", {}, function (jsonobject) {
            var cookies = document.cookie.split(';');
            var CLegalOrg = "";
            var CapellaScribeLookupList = null;
            for (var l = 0; l < cookies.length; l++) {
                if (cookies[l].indexOf("CLegalOrg") > -1)
                    CLegalOrg = cookies[l].split("=")[1];
            }
            if (jsonobject != null) {
                CapellaScribeLookupList = jsonobject.CapellaScribeLookup.filter(g => g.ProviderID == DropdownList.selectedOptions[0].value.toString() && g.Legal_Org == CLegalOrg);
            }
            if ((CapellaScribeLookupList?.length ?? 0) > 0) {
                document.getElementById('btnmovetoscribe').style.display = "block";
            }
            else {
                document.getElementById('btnmovetoscribe').style.display = "none";
            }
        });
    }
}

function DuplicateWarningMessage() {
    if (DisplayErrorMessage('180044') == true) {
        document.getElementById("btnHiddenDuplicateCheck").click();
    }
}


function refreshSummaryBar(tabname) {
    window.parent.parent.location.href = "frmPatientChart.aspx?tabName=" + tabname + "";
}

function reloadSummary() {
    var enc_id = sessionStorage.getItem("EncId_PatSummaryBar");
    var enc_DOS = sessionStorage.getItem("Enc_DOS");
    //sessionStorage.removeItem("EncId_PatSummaryBar");
    //sessionStorage.removeItem("Enc_DOS");

    //CAP-2596
    var encounterId = parseInt(enc_id);
    if ((encounterId ?? 0) > 0) {
        $.ajax({
            type: "POST",
            url: "frmRCopiaToolbar.aspx/LoadPatientSummaryBar",
            // data: JSON.stringify({ EncID: "", Enc_DOS: "" }),
            data: JSON.stringify({ EncID: enc_id, Enc_DOS: enc_DOS }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: OnSuccessSummaryBar,
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

    RefreshNotification("Notify");
}

function OnSuccessSummaryBar(response) {
    var regex = /<BR\s*[\/]?>/gi;

    if (response != null) {

        top.window.document.getElementById("ctl00_C5POBody_lblAllergies").innerHTML = response.d[0];
        top.window.document.getElementById("ctl00_C5POBody_lblCheifComplaints").innerHTML = response.d[1];
        top.window.document.getElementById("ctl00_C5POBody_lblProblemList").innerHTML = response.d[2];
        top.window.document.getElementById("ctl00_C5POBody_lblVitals").innerHTML = response.d[3];
        top.window.document.getElementById("ctl00_C5POBody_lblMedication").innerHTML = response.d[4];
        if (response.d[5].replace("Allergies :<br/>", "").length != 0)
            top.window.document.getElementById("Allergies_tooltp").innerText = response.d[5].replace(regex, "\n") + "\n";
        else
            top.window.document.getElementById("Allergies_tooltp").innerText = "";
        if (response.d[6].replace("Chief Complaints :<br/><br/>", "").length != 0)
            top.window.document.getElementById("CheifComplaints_tooltp").innerText = response.d[6].replace(regex, "\n").split("&#xA;").join("\n") + "\n";
        else
            top.window.document.getElementById("CheifComplaints_tooltp").innerText = "";
        if (response.d[7].replace("Problem List :<br/>", "").length != 0)
            top.window.document.getElementById("ProblemList_tooltp").innerText = response.d[7].replace(regex, "\n") + "\n";
        else
            top.window.document.getElementById("ProblemList_tooltp").innerText = "";
        if (response.d[8].replace("Vitals :<br/>", "").length != 0)
            top.window.document.getElementById("Vitals_tooltp").innerText = response.d[8].replace(regex, "\n") + "\n";
        else
            top.window.document.getElementById("Vitals_tooltp").innerText = "";
        if (response.d[9].replace("Medication :<br/>", "").length != 0)
            top.window.document.getElementById("Medication_tooltp").innerText = response.d[9].replace(regex, "\n") + "\n";
        else
            top.window.document.getElementById("Medication_tooltp").innerText = "";
        RefreshOverallSummaryTooltip();

    }
    var sDtls = window.parent.parent.document.getElementsByName('lblPatientStrip')[0].innerText;
    document.cookie = "Human_Details=Last_Name:" + sDtls.split('|')[0].split(',')[0] + "|First_Name:" + sDtls.split('|')[0].split(',')[1].split(' ')[0] +
        "|Middle_Name:" + sDtls.split('|')[0].split(',')[1].split(' ')[1] + "|DOB:" + sDtls.split('|')[1] + "|Sex:" + sDtls.split('|')[3] + "|" +
        window.parent.document.getElementsByTagName('fieldset')[0].innerText.split('|')[1] + "|" + window.parent.parent.document.all("ctl00_C5POBody_lblVitals").innerText.split('\n')[1] + "|" +
        window.parent.parent.document.all("ctl00_C5POBody_lblVitals").innerText.split('\n')[2];

}


function SocialTabMantatoryField() {
    if (window.parent.parent.theForm.ctl00_C5POBody_hdnSocialHistoryMandatory.value == "true") {
        DisplayErrorMessage('180020');
        return false;
    }
}

function SocialTabMantatoryFieldButton(sender, args) {

    isSaveEnabledButton(sender, args);
    if (window.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value == "false" || document.getElementById('hdnTabClick').value.split('$#$')[1] == "third") {
        if (window.parent.parent.theForm.ctl00_C5POBody_hdnSocialHistoryMandatory.value == "true") {
            DisplayErrorMessage('180020');
            {
                if (sender != null || sender != undefined) {
                    sender.set_autoPostBack(false);
                }
            }
        }
    }
}

function PfshVerified() {
    DisplayErrorMessage('180046');
}

function OnClientCloseForCopyToProviderForReview(oWindow, args) {
    var CopyToProviderForReview = document.getElementById('hdnCopyToProviderForReview');
    if (args._argument == "Yes") {
        CopyToProviderForReview.value = "true";
        document.getElementById('btnMove').click();
    }
    else if (args._argument == "No") {
        CopyToProviderForReview.value = "false";
        document.getElementById('btnMove').click();
    }
    else if (args._argument == "Cancel")
        CopyToProviderForReview.value = "";
}

function OnClientPageLoadForCopyToProviderForReview() {
    var iFrames = $telerik.$('.RadWindow .rwWindowContent iframe');

    for (i = 0; i < iFrames.length; i++) {
        var n = iFrames[i].src.indexOf("frmValidationArea.aspx");
        if (n >= 0) {
            iFrames[i].style.height = "85px";
            iFrames[i].style.width = "275px";
        } else {
            iFrames[i].style.height = "100%";
            iFrames[i].style.width = "100%";
        }
    }
}

function ConfirmToProceed() {
    if (DisplayErrorMessage('180026', '', '')) {
        document.getElementById('btnMove_input').click();
    }
    else
        return false;
}

function MovedSuccessfully() {
    DisplayErrorMessage('180029', '', '');
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    top.window.setTimeout(function () {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        window.top.location.href = "frmMyQueueNew.aspx";
    }, 100);
}


function providerValidation() {
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    openModal("frmProviderValidation.aspx", 250, 800, null, 'RadWindow1');
}




function showConfirmRadWindow(sender) {
    HideLoading();
    var oWnd = radconfirm(sender, confirmResult, 450, 100, "confirm");
    oWnd.center();
    oWnd.set_behaviors(Telerik.Web.UI.WindowBehaviors.none);
    oWnd.set_iconUrl("Resources/16_16.ico");
    oWnd.SetTitle("Message");
}

function confirmResult(sender, args) {
    if (sender == true) {
        document.getElementById('btnhiddenConfirmOk').click();
    }

}

function SetCookie() {
    if (localStorage["phyIDList"] != undefined) {
        var phyIDList = JSON.parse(localStorage["phyIDList"]);
        var list = Object.keys(phyIDList).map(function (key) { return phyIDList[key] });
        var strList = "";
        for (var i = 0; i < list.length; i++) {
            strList += list[i] + "#";
        }
        strList = strList.substr(0, strList.length);
        document.cookie = "LocalStorage=" + strList + ";path=/;";
    }
    if (localStorage["FACILITY_LIST"] != undefined) {
        var Faclist = JSON.parse(localStorage["FACILITY_LIST"]);
        var list = Object.keys(Faclist).map(function (key) { return Faclist[key] });
        var strList = "";
        for (var i = 0; i < list.length; i++) {
            strList += list[i] + "!";
        }
        strList = strList.substr(0, strList.length - 1);
        document.cookie += "AllFacility=" + strList + ";path=/;";
    }
}
function FillData(UserRole, facility, EncProID) {
    if (localStorage["FACILITY_LIST"] != undefined) {
        var Faclist = JSON.parse(localStorage["FACILITY_LIST"]);
        var list = Object.keys(Faclist).map(function (key) { return Faclist[key] });
        var strList = "";
        for (var i = 0; i < list.length; i++) {
            strList += list[i] + "!";
        }
        strList = strList.substr(0, strList.length - 1);
        document.cookie += "AllFacility=" + strList + ";path=/;";
    }

    if (UserRole != "PHYSICIAN" && UserRole != "PHYSICIAN ASSISTANT" && UserRole != "CODER") {
        var setList = JSON.parse(localStorage.getItem("FACILITY_LIST"));
        var cboPhysician = document.getElementById("cboPhysicianName");
        if (cboPhysician != undefined) {
            var options = Object.keys(setList).map(function (key) { return setList[key] });
            if (document.getElementById('chkShowAllPhysicians').checked == false) {
                for (var i = 0; i < options.length; i++) {
                    var facList = options[i].split("|")[1].split("$")[1].split("*")[1].split("+");
                    for (var j = 0; j < facList.length; j++) {
                        if (facList[j] == facility) {
                            var optUser = options[i].split("|")[0];
                            var optName = options[i].split("|")[1].split("$")[0];
                            var optValue = options[i].split("|")[1].split("$")[1].split("*")[0];
                            var newOption = document.createElement("option");
                            newOption.textContent = optUser + ' - ' + optName;
                            newOption.value = optValue;
                            cboPhysician.appendChild(newOption);
                        }
                    }
                }
                var selectedID = document.getElementById("hdnLocalPhy").value;
                var setID = (selectedID != "" && EncProID != selectedID) ? selectedID : EncProID;
                for (var l = 0; l < cboPhysician.options.length; l++) {
                    if (cboPhysician.options[l].value == setID)
                        cboPhysician.selectedIndex = cboPhysician.options[l].index;
                }
                //Old Code
                //document.getElementById("hdnLocalPhy").value = setID;
                //Gitlab# 2485 - Physician Name Display Change
                $.ajax({
                    type: "GET",
                    url: "ConfigXML/User.json",
                    dataType: "json",
                    async: false,
                    cache: false,
                    success: function (json) {
                        //$(json).find('User').each(function () {
                        //    var cookies = document.cookie.split(';');
                        //    var CLegalOrg = "";
                        //    for (var l = 0; l < cookies.length; l++) {
                        //        if (cookies[l].indexOf("CLegalOrg") > -1)
                        //            CLegalOrg = cookies[l].split("=")[1];
                        //    }
                        //    if ($(this)[0].getAttribute("Physician_Library_ID") == setID && $(this)[0].getAttribute("Legal_Org") == CLegalOrg) {
                        //        document.getElementById("hdnLocalPhy").value = setID + '~' + $(this)[0].getAttribute("User_Name");

                        //    }
                        //})
                        var cookies = document.cookie.split(';');
                        var CLegalOrg = "";
                        for (var l = 0; l < cookies.length; l++) {
                            if (cookies[l].indexOf("CLegalOrg") > -1)
                                CLegalOrg = cookies[l].split("=")[1];
                        }
                        $.each(json, function (index, user) {
                            if (user.Physician_Library_ID == setID && user.Legal_Org == CLegalOrg) {
                                document.getElementById("hdnLocalPhy").value = setID + '~' + user.User_Name;
                            }
                        });
                    }
                });
            }
            else {
                for (var i = 0; i < options.length; i++) {
                    var optUser = options[i].split("|")[0];
                    var optName = options[i].split("|")[1].split("$")[0];
                    var optValue = options[i].split("|")[1].split("$")[1].split("*")[0];
                    var newOption = document.createElement("option");
                    newOption.textContent = optUser + ' - ' + optName;
                    newOption.value = optValue;
                    cboPhysician.appendChild(newOption);
                }
                var selectedID = document.getElementById("hdnLocalPhy").value;
                var setID = (selectedID != "" && EncProID != selectedID) ? selectedID : EncProID;
                for (var l = 0; l < cboPhysician.options.length; l++) {
                    if (cboPhysician.options[l].value == setID)
                        cboPhysician.selectedIndex = cboPhysician.options[l].index;
                }
                //Old Code
                //document.getElementById("hdnLocalPhy").value = setID;
                //Gitlab# 2485 - Physician Name Display Change
                $.ajax({
                    type: "GET",
                    url: "ConfigXML/User.json",
                    dataType: "json",
                    async: false,
                    cache: false,
                    success: function (json) {
                        //$(json).find('User').each(function () {
                        //    var cookies = document.cookie.split(';');
                        //    var CLegalOrg = "";
                        //    for (var l = 0; l < cookies.length; l++) {
                        //        if (cookies[l].indexOf("CLegalOrg") > -1)
                        //            CLegalOrg = cookies[l].split("=")[1];
                        //    }
                        //    if ($(this)[0].getAttribute("Physician_Library_ID") == setID && $(this)[0].getAttribute("Legal_Org") == CLegalOrg) {
                        //        document.getElementById("hdnLocalPhy").value = setID + '~' + $(this)[0].getAttribute("User_Name");

                        //    }
                        //})
                        var cookies = document.cookie.split(';');
                        var CLegalOrg = "";
                        for (var l = 0; l < cookies.length; l++) {
                            if (cookies[l].indexOf("CLegalOrg") > -1)
                                CLegalOrg = cookies[l].split("=")[1];
                        }
                        $.each(json, function (index, user) {
                            if (user.Physician_Library_ID == setID && user.Legal_Org == CLegalOrg) {
                                document.getElementById("hdnLocalPhy").value = setID + '~' + user.User_Name;
                            }
                        });
                    }
                });
            }
        }
    }
    else
        if (sUserRole == "PHYSICIAN ASSISTANT" && document.getElementById('chkShowAllPhysicians').checked == true) {
            var cboPhysician = document.getElementById("cboPhysicianName");
            var i;
            for (i = cboPhysician.options.length - 1; i >= 0; i--) {
                cboPhysician.remove(i);
            }
            FillData("PHYSICIAN ASSISTANT_TRUE", curr_Fac, encProID);
        }
}

function SetSummaryList(ProbLstText, toolTipText) {

    var dox = top.window.parent.window.parent.window.parent.window.document;
    var sAllergy = ProbLstText.split('^')[0];
    var sCC = ProbLstText.split('^')[1];
    var sProblemList = ProbLstText.split('^')[2];

    var pnl = dox.getElementById('ctl00_C5POBody_pnlProblemList');
    pnl.innerHTML = sProblemList.replace("ProblemList-", "");
    var regex = /<BR\s*[\/]?>/gi;
    sProblemList = sProblemList.replace(regex, "\n");
    regex = /<[\/]{0,1}(span|SPAN)[^><]*>/g;
    sProblemList = sProblemList.replace(regex, "");
    pnl.title = sProblemList;



    var CCText = dox.getElementById("ctl00_C5POBody_lblCheifComplaints");
    CCText.innerHTML = sCC.replace("CC-", "");
    var pnl = dox.getElementById('ctl00_C5POBody_pnlCheifComplaints');
    var regex = /<BR\s*[\/]?>/gi;
    pnl.title = sCC.replace(regex, "\n");



    var AllergyText = dox.getElementById("ctl00_C5POBody_lblAllergies");
    AllergyText.innerHTML = sAllergy.replace("Allergy-", "");

    var pnl = dox.getElementById('ctl00_C5POBody_pnlAllergies');
    var regex = /<BR\s*[\/]?>/gi;
    pnl.title = sAllergy.replace(regex, "\n");


    var overAllTitle = dox.all.ctl00_C5POBody_lblAllergies.title + "\n" + dox.all.ctl00_C5POBody_lblCheifComplaints.title + "\n"
    + dox.all.ctl00_C5POBody_lblProblemList.title + "\n" + dox.all.ctl00_C5POBody_lblVitals.title + "\n"
    + dox.all.ctl00_C5POBody_lblMedication.title;

    top.window.document.getElementById("ctl00_C5POBody_imgOverAllSummary").title = overAllTitle;

}

function btnCopyPrevious_ClientClick(sender, args) {

    //if ($("ul#myTabs li.active a")[0].innerText == "CC / HPI") {
    //    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnccAutosave.value == "false") {
    //        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
    //        localStorage.setItem("bSave", "true");
    //    }
    //}

    if (window.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value == "true" &&
        localStorage.getItem("bSave") == "false") {
        var CurrentTab = $("ul#myTabs li.active a");
        if (localStorage.getItem("PrevSubTab") != null && localStorage.getItem("PrevSubTab") != undefined && localStorage.getItem("PrevSubTab") != "") {
            sessionStorage.setItem("EncPrevTabText", CurrentTab[0].innerText);
        }
        tabAutoSave(CurrentTab, sender);
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnccAutosave.value == "false";
        return false;
    }
    else {

        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        var hdnAppointmentProviderId = document.getElementById("hdnAppointmentProviderId").value;
        var hdnEncounterProviderId = document.getElementById("hdnEncounterProviderId").value;

        if (hdnAppointmentProviderId != hdnEncounterProviderId) {
            ProviderValidationCheck();
        }
        else
            return true;
    }
}


function OpenNotification_Before_MovetoNextProcess() {
    Notification_Popup('MovetoNextProcess');
}

function IsSaveEnabled(sender) {
    localStorage.setItem("MovetofromEandM", "True");
    var bsave = localStorage.getItem("bSave");
    var val = localStorage.getItem("CCAndEandMAutosave");
    if (bsave == "false") {
        var CurrentTab = $("ul#myTabs li.active a");
        if (localStorage.getItem("PrevSubTab") != null && localStorage.getItem("PrevSubTab") != undefined && localStorage.getItem("PrevSubTab") != "") {
            sessionStorage.setItem("EncPrevTabText", CurrentTab[0].innerText);
        }
        if ((CurrentTab[0].innerText == "CC / HPI" || CurrentTab[0].innerText == "SERV./PROC. CODES") && (val != null && val != undefined && val != "")) {
            if (val != "true") {
                tabAutoSave(CurrentTab, sender);
            }
            else {
                disableAutoSave();//to prevent repeated enabling of autosave functionality - from Notification screen 
                { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
                return true;
            }
        }
        else {
            tabAutoSave(CurrentTab, sender);
        }
        return false;
    }
    else {
        disableAutoSave();//to prevent repeated enabling of autosave functionality - from Notification screen 
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        return true;

    }
}

var bCancel = false;
//function tabAutoSave(CurrentTab, sender) {
//    CurTab = CurrentTab;
//    localStorage.setItem('bCheck', 'false');
//    $(top.window.document).find("body").append("<div id='dvdialog' style='min-height: 65px !important; width: auto; max-height: none; height: auto; display: none;'>" +
//                            "<p style='font-family: Verdana,Arial,sans-serif; font-size: 13.5px;'>There are unsaved changes.Do you want to save them?</p></div>");
//    dvdialog = window.parent.parent.parent.parent.document.getElementsByTagName('div').namedItem('dvdialog');
//    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value == "true" && localStorage.getItem("bSave") == "false") {
//        event.preventDefault();
//        bCancel = true;
//        $(dvdialog).dialog({
//            modal: true,
//            title: "Capella -EHR",
//            position: {
//                my: 'left' + " " + 'center',
//                at: 'center' + " " + 'center + 100px'

//            },
//            buttons: {
//                "Yes": function () {
//                    sessionStorage.setItem("EncAutoSave", "true");
//                    if (CurTab[0].innerText == "CC / HPI") {
//                        $('.clsIframe').contents()[0].all.namedItem('btnAdd').click();
//                        $(dvdialog).dialog("close");
//                        enableAutoSave();
//                        CurTab.tab('show');
//                    }
//                    else if (CurTab[0].innerText == "SCREENING") {
//                        var subtab = localStorage.getItem("PrevSubTab");
//                        if (subtab == "General") {
//                            $('.clsIframe').contents()[1].all.namedItem('General').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "TB Risk Assessment") {
//                            $('.clsIframe').contents()[1].all.namedItem('TBRiskAssessment').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "Diabetic Foot Screening") {
//                            $('.clsIframe').contents()[1].all.namedItem('DiabeticFootScreening').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "Sleep Screening") {
//                            $('.clsIframe').contents()[1].all.namedItem('SleepScreening').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "Sleep") {
//                            $('.clsIframe').contents()[1].all.namedItem('Sleep').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "Pulmonary") {
//                            $('.clsIframe').contents()[1].all.namedItem('Pulmonary').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "Epworth Sleep Score") {
//                            $('.clsIframe').contents()[1].all.namedItem('EpworthSleepScore').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "Pulmonary/Sleep Exam") {
//                            $('.clsIframe').contents()[1].all.namedItem('PulmonarySleepExam').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "Dermatology Questionnaire") {
//                            $('.clsIframe').contents()[1].all.namedItem('DermatologyQuestionnaire').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "Asthma Control Test") {
//                            $('.clsIframe').contents()[1].all.namedItem('AsthmaControlTest').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "Depression") {
//                            $('.clsIframe').contents()[1].all.namedItem('Depression').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "Depression Test") {
//                            $('.clsIframe').contents()[1].all.namedItem('DepressionTest').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "Neck Disability Index") {
//                            $('.clsIframe').contents()[1].all.namedItem('NeckDisabilityIndex').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "Oswestry Disability Index") {
//                            $('.clsIframe').contents()[1].all.namedItem('OswestryDisabilityIndex').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "Development") {
//                            $('.clsIframe').contents()[1].all.namedItem('Development').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }

//                        else if (subtab == "Chronic Cough Scale") {
//                            $('.clsIframe').contents()[1].all.namedItem('ChronicCoughScale').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "Gynecological") {
//                            $('.clsIframe').contents()[1].all.namedItem('Gynecological').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "Pediatric Sleep Questionnaire") {
//                            $('.clsIframe').contents()[1].all.namedItem('PediatricSleepQuestionnaire').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "Sleep Short") {
//                            $('.clsIframe').contents()[1].all.namedItem('SleepShort').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "Sleep Tendency Scale") {
//                            $('.clsIframe').contents()[1].all.namedItem('SleepTendencyScale').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "Functional Assessment") {
//                            $('.clsIframe').contents()[1].all.namedItem('FunctionalAssessment').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "Karnofsky") {
//                            $('.clsIframe').contents()[1].all.namedItem('Karnofsky').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "Mini Mental") {
//                            $('.clsIframe').contents()[1].all.namedItem('MiniMental').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "Nutritional Screening") {
//                            $('.clsIframe').contents()[1].all.namedItem('NutritionalScreening').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "Safety Guidelines") {
//                            $('.clsIframe').contents()[1].all.namedItem('SafetyGuidelines').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "Support Needs") {
//                            $('.clsIframe').contents()[1].all.namedItem('SupportNeeds').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "Pain Assessment") {
//                            $('.clsIframe').contents()[1].all.namedItem('PainAssessment').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "CAT Screening") {
//                            $('.clsIframe').contents()[1].all.namedItem('CATScreening').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "COPD Breathe Well Program") {
//                            $('.clsIframe').contents()[1].all.namedItem('COPDBreatheWellProgram').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "Fall Risk Screening") {
//                            $('.clsIframe').contents()[1].all.namedItem('FallRiskScreening').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "Cognitive Screening") {
//                            $('.clsIframe').contents()[1].all.namedItem('CognitiveScreening').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "Pain Screening") {
//                            $('.clsIframe').contents()[1].all.namedItem('PainScreening').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "PHQ-9 Screening") {
//                            $('.clsIframe').contents()[1].all.namedItem('PHQ-9Screening').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "Katz Index Screening") {
//                            $('.clsIframe').contents()[1].all.namedItem('KatzIndexScreening').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "ADL Screening") {
//                            $('.clsIframe').contents()[1].all.namedItem('ADLScreening').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "Get Up and Go") {
//                            $('.clsIframe').contents()[1].all.namedItem('GetUpandGo').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "Lawton Screening") {
//                            $('.clsIframe').contents()[1].all.namedItem('LawtonScreening').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "Anxiety Screening") {
//                            $('.clsIframe').contents()[1].all.namedItem('AnxietyScreening').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "Spine Intake") {
//                            $('.clsIframe').contents()[1].all.namedItem('SpineIntake').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "Monofilament Foot Exam") {
//                            $('.clsIframe').contents()[1].all.namedItem('MonofilamentFootExam').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "Cervical Spine") {
//                            $('.clsIframe').contents()[1].all.namedItem('CervicalSpine').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "Lumbar Spine") {
//                            $('.clsIframe').contents()[1].all.namedItem('LumbarSpine').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "Urinalysis") {
//                            $('.clsIframe').contents()[1].all.namedItem('Urinalysis').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "Staying Healthy Assessment") {
//                            $('.clsIframe').contents()[1].all.namedItem('StayingHealthyAssessment').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        $(dvdialog).dialog("close");
//                        enableAutoSave();
//                        CurTab.tab('show');
//                    }
//                    else if (CurTab[0].innerText == "PFSH") {
//                        var subtab = localStorage.getItem("PrevSubTab");
//                        if (subtab == "Past Medical History") {
//                            $('.clsIframe').contents()[2].all.namedItem('pastmedHis').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "Surg./Proc.") {
//                            $('.clsIframe').contents()[2].all.namedItem('surgproc').children[0].contentDocument.all.namedItem('btnAdd').click();
//                        }
//                        else if (subtab == "Hospitalization History") {
//                            $('.clsIframe').contents()[2].all.namedItem('hospHis').children[0].contentDocument.all.namedItem('btnAdd').click();
//                        }
//                        else if (subtab == "Family History") {
//                            $('.clsIframe').contents()[2].all.namedItem('famHis').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "Social History") {
//                            $('.clsIframe').contents()[2].all.namedItem('socialHistory').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "Non Drug Allergy") {
//                            $('.clsIframe').contents()[2].all.namedItem('NDA').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "Immunization History") {
//                            $('.clsIframe').contents()[2].all.namedItem('immHis').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "AD") {
//                            $('.clsIframe').contents()[2].all.namedItem('ad').children[0].contentDocument.all.namedItem('btnPFSHAutoSave').click();
//                        }
//                        $(dvdialog).dialog("close");
//                        disableAutoSave();
//                        CurTab.tab('show');
//                    }
//                    else if (CurTab[0].innerText == "ROS") {
//                        $('.clsIframe').contents()[3].all.namedItem('btnSave').click();
//                        $(dvdialog).dialog("close");
//                        disableAutoSave();
//                        CurTab.tab('show');
//                    }
//                    else if (CurTab[0].innerText == "VITALS") {
//                        $('.clsIframe').contents()[4].all.namedItem('btnSaveVitals').click();
//                        $(dvdialog).dialog("close");
//                        CurTab.tab('show');
//                        var check = sessionStorage.getItem("MandatoryCheck");
//                        if (check != "true") {
//                            __doPostBack(sender.id, 'OnClick');
//                            sessionStorage.removeItem("MandatoryCheck");
//                        }


//                    }
//                    else if (CurTab[0].innerText == "EXAM") {
//                        var subtab = localStorage.getItem("PrevSubTab");
//                        if (subtab == "General With Specialty") {
//                            $('.clsIframe').contents()[5].all.namedItem('generalwithspeciality').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "Focused") {
//                            $('.clsIframe').contents()[5].all.namedItem('Focused').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "Upload & View Photos") {
//                            $('.clsIframe').contents()[5].all.namedItem('UploadViewPhotos').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "Body Image") {
//                            $('.clsIframe').contents()[5].all.namedItem('BodyImage').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }

//                        $(dvdialog).dialog("close");
//                        disableAutoSave();
//                        CurTab.tab('show');
//                    }
//                    else if (CurTab[0].innerText == "TEST") {
//                        $('.clsIframe').contents()[6].all.namedItem('btnSave').click();
//                        $(dvdialog).dialog("close");
//                        disableAutoSave();
//                        CurTab.tab('show');
//                    }
//                    else if (CurTab[0].innerText == "ASSESSMENT") {
//                        $('.clsIframe').contents()[7].all.namedItem('btnSave').click();
//                        $(dvdialog).dialog("close");
//                        enableAutoSave();
//                        CurTab.tab('show');
//                    }
//                    else if (CurTab[0].innerText == "ORDERS") {
//                        var subtab = localStorage.getItem("PrevSubTab");
//                        if (subtab == "Diagnostic Order") {
//                            $('.clsIframe').contents()[8].all.namedItem('DiagnosticOrder').children[0].contentDocument.all.namedItem('btnOrderSubmit').click();
//                        }
//                        else if (subtab == "Referral Order") {
//                            $('.clsIframe').contents()[8].all.namedItem('ReferralOrder').children[0].contentDocument.all.namedItem('btnAddRefOrder').click();
//                        }
//                        else if (subtab == "Immunization/Injection") {
//                            $('.clsIframe').contents()[8].all.namedItem('Immunization').children[0].contentDocument.all.namedItem('btnAdd').click();
//                        }
//                        else if (subtab == "Procedures") {
//                            $('.clsIframe').contents()[8].all.namedItem('InHouseProcedure').children[0].contentDocument.all.namedItem('btnAdd').click();
//                        }
//                        else if (subtab == "DME Order") {
//                            $('.clsIframe').contents()[8].all.namedItem('DMEOrder').children[0].contentDocument.all.namedItem('btnAdd').click();
//                        }
//                        $(dvdialog).dialog("close");
//                        disableAutoSave();
//                        CurTab.tab('show');
//                    }
//                    else if (CurTab[0].innerText == "eRx") {
//                        paneID = $(event.target).attr('href');
//                        src = $(paneID).attr('data-src');
//                        $(paneID + " iframe").attr("src", src);
//                    }
//                    else if (CurTab[0].innerText == "SERV./PROC. CODES") {
//                        $('.clsIframe').contents()[11].all.namedItem('btnSave').click();
//                        $(dvdialog).dialog("close");
//                        enableAutoSave();//BugID:52795
//                        CurTab.tab('show');
//                    }
//                    else if (CurTab[0].innerText == "PLAN") {
//                        var subtab = localStorage.getItem("PrevSubTab");
//                        if (subtab == "General Plan") {
//                            $('.clsIframe').contents()[10].all.namedItem('generalplan').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "Individualized CarePlan") {
//                            $('.clsIframe').contents()[10].all.namedItem('IndividualCarePlan').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        else if (subtab == "Preventive Screen Plan") {
//                            $('.clsIframe').contents()[10].all.namedItem('PreventiveScreen').children[0].contentDocument.all.namedItem('btnSave').click();
//                        }
//                        $(dvdialog).dialog("close");
//                        disableAutoSave();
//                        CurTab.tab('show');
//                    }
//                    else if (CurTab[0].innerText == "SUMMARY") {
//                        $('.clsIframe').contents()[12].all.namedItem('btnSave').click();
//                        $(dvdialog).dialog("close");
//                        disableAutoSave();
//                        CurTab.tab('show');
//                    }
//                    return;
//                },
//                "No": function () {
//                    $(dvdialog).dialog("close");
//                    $(dvdialog).remove();
//                    disableAutoSave();//BugID:48506
//                    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
//                    bCancel = true;
//                    CurTab.tab('show');
//                    __doPostBack(sender.id, 'OnClick');
//                },
//                "Cancel": function () {
//                    enableAutoSave();
//                    $(dvdialog).dialog("close");
//                    $(dvdialog).remove();
//                    CurTab.tab('show');
//                    return;
//                }
//            }
//        });
//    }
//    else {
//        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
//        if ($(".ui-dialog").is(":visible")) {
//            $(dvdialog).dialog("close");
//            $(dvdialog).remove();
//        }
//        if (bCancel) {
//            __doPostBack(sender.id, 'OnClick');
//            bCancel = false;
//        }
//    }
//}
function tabAutoSave(CurrentTab, sender) {
    CurTab = CurrentTab;     // active tab    
    localStorage.setItem('bCheck', 'false');
    sessionStorage.setItem("EncPrevTabText", CurTab[0].innerText);
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value == "true" && localStorage.getItem("bSave") == "false") {

        event.preventDefault();
        bCancel = true;
        sessionStorage.setItem("EncAutoSave", "true");
        //CAP-2678
        localStorage.setItem("IsSaveCompleted", false);
        autoSaveAndMoveToNextProcess(sender);
        if (CurTab[0].innerText == "CC / HPI") {
            $('.clsIframe').contents()[0].all.namedItem('btnAdd').click();
            //$(dvdialog).dialog("close");
            enableAutoSave();
            CurTab.tab('show');
        }

        else if (CurTab[0].innerText == "SCREENING") {
            var subtab = localStorage.getItem("PrevSubTab");
            if (subtab == "General") {
                $('.clsIframe').contents()[1].all.namedItem('General').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "TB Risk Assessment") {
                $('.clsIframe').contents()[1].all.namedItem('TBRiskAssessment').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "Diabetic Foot Screening") {
                $('.clsIframe').contents()[1].all.namedItem('DiabeticFootScreening').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "Sleep Screening") {
                $('.clsIframe').contents()[1].all.namedItem('SleepScreening').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "Sleep") {
                $('.clsIframe').contents()[1].all.namedItem('Sleep').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "Pulmonary") {
                $('.clsIframe').contents()[1].all.namedItem('Pulmonary').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "Epworth Sleep Score") {
                $('.clsIframe').contents()[1].all.namedItem('EpworthSleepScore').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "Pulmonary/Sleep Exam") {
                $('.clsIframe').contents()[1].all.namedItem('PulmonarySleepExam').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "Dermatology Questionnaire") {
                $('.clsIframe').contents()[1].all.namedItem('DermatologyQuestionnaire').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "Asthma Control Test") {
                $('.clsIframe').contents()[1].all.namedItem('AsthmaControlTest').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "Depression") {
                $('.clsIframe').contents()[1].all.namedItem('Depression').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "Depression Test") {
                $('.clsIframe').contents()[1].all.namedItem('DepressionTest').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "Neck Disability Index") {
                $('.clsIframe').contents()[1].all.namedItem('NeckDisabilityIndex').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "Oswestry Disability Index") {
                $('.clsIframe').contents()[1].all.namedItem('OswestryDisabilityIndex').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "Development") {
                $('.clsIframe').contents()[1].all.namedItem('Development').children[0].contentDocument.all.namedItem('btnSave').click();
            }

            else if (subtab == "Chronic Cough Scale") {
                $('.clsIframe').contents()[1].all.namedItem('ChronicCoughScale').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "Gynecological") {
                $('.clsIframe').contents()[1].all.namedItem('Gynecological').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "Pediatric Sleep Questionnaire") {
                $('.clsIframe').contents()[1].all.namedItem('PediatricSleepQuestionnaire').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "Sleep Short") {
                $('.clsIframe').contents()[1].all.namedItem('SleepShort').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "Sleep Tendency Scale") {
                $('.clsIframe').contents()[1].all.namedItem('SleepTendencyScale').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "Functional Assessment") {
                $('.clsIframe').contents()[1].all.namedItem('FunctionalAssessment').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "Karnofsky") {
                $('.clsIframe').contents()[1].all.namedItem('Karnofsky').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "Mini Mental") {
                $('.clsIframe').contents()[1].all.namedItem('MiniMental').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "Nutritional Screening") {
                $('.clsIframe').contents()[1].all.namedItem('NutritionalScreening').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "Safety Guidelines") {
                $('.clsIframe').contents()[1].all.namedItem('SafetyGuidelines').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "Support Needs") {
                $('.clsIframe').contents()[1].all.namedItem('SupportNeeds').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "Pain Assessment") {
                $('.clsIframe').contents()[1].all.namedItem('PainAssessment').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "CAT Screening") {
                $('.clsIframe').contents()[1].all.namedItem('CATScreening').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "COPD Breathe Well Program") {
                $('.clsIframe').contents()[1].all.namedItem('COPDBreatheWellProgram').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "Fall Risk Screening") {
                $('.clsIframe').contents()[1].all.namedItem('FallRiskScreening').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "Cognitive Screening") {
                $('.clsIframe').contents()[1].all.namedItem('CognitiveScreening').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "Pain Screening") {
                $('.clsIframe').contents()[1].all.namedItem('PainScreening').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "PHQ-9 Screening") {
                $('.clsIframe').contents()[1].all.namedItem('PHQ-9Screening').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "Katz Index Screening") {
                $('.clsIframe').contents()[1].all.namedItem('KatzIndexScreening').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "ADL Screening") {
                $('.clsIframe').contents()[1].all.namedItem('ADLScreening').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "Get Up and Go") {
                $('.clsIframe').contents()[1].all.namedItem('GetUpandGo').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "Lawton Screening") {
                $('.clsIframe').contents()[1].all.namedItem('LawtonScreening').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "Anxiety Screening") {
                $('.clsIframe').contents()[1].all.namedItem('AnxietyScreening').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "Spine Intake") {
                $('.clsIframe').contents()[1].all.namedItem('SpineIntake').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "Monofilament Foot Exam") {
                $('.clsIframe').contents()[1].all.namedItem('MonofilamentFootExam').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "Cervical Spine") {
                $('.clsIframe').contents()[1].all.namedItem('CervicalSpine').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "Lumbar Spine") {
                $('.clsIframe').contents()[1].all.namedItem('LumbarSpine').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "Urinalysis") {
                $('.clsIframe').contents()[1].all.namedItem('Urinalysis').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "Staying Healthy Assessment") {
                $('.clsIframe').contents()[1].all.namedItem('StayingHealthyAssessment').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "AUA BPH Symptom") {
                $('.clsIframe').contents()[1].all.namedItem('AUABPHSymptom').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "Home Safety") {
                $('.clsIframe').contents()[1].all.namedItem('HomeSafety').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            //$(dvdialog).dialog("close");
            enableAutoSave();
            CurTab.tab('show');
        }
        else if (CurTab[0].innerText == "PFSH") {
            var subtab = localStorage.getItem("PrevSubTab");
            if (subtab == "Past Medical History") {
                $('.clsIframe').contents()[2].all.namedItem('pastmedHis').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "Surg./Proc.") {
                $('.clsIframe').contents()[2].all.namedItem('surgproc').children[0].contentDocument.all.namedItem('btnAdd').click();
            }
            else if (subtab == "Hospitalization History") {
                $('.clsIframe').contents()[2].all.namedItem('hospHis').children[0].contentDocument.all.namedItem('btnAdd').click();
            }
            else if (subtab == "Family History") {
                $('.clsIframe').contents()[2].all.namedItem('famHis').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "Social History") {
                $('.clsIframe').contents()[2].all.namedItem('socialHistory').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "Non Drug Allergy") {
                $('.clsIframe').contents()[2].all.namedItem('NdDugAllergy').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "Immunization History") {
                $('.clsIframe').contents()[2].all.namedItem('immHis').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "AD") {
                $('.clsIframe').contents()[2].all.namedItem('ad').children[0].contentDocument.all.namedItem('btnPFSHAutoSave').click();
            }
            //$(dvdialog).dialog("close");
            disableAutoSave();
            CurTab.tab('show');
        }
        else if (CurTab[0].innerText == "ROS") {
            $('.clsIframe').contents()[3].all.namedItem('btnSave').click();
            //$(dvdialog).dialog("close");
            disableAutoSave();
            CurTab.tab('show');
        }
        else if (CurTab[0].innerText == "VITALS") {
            $('.clsIframe').contents()[4].all.namedItem('btnSaveVitals').click();
            //$(dvdialog).dialog("close");
            CurTab.tab('show');
            var check = sessionStorage.getItem("MandatoryCheck");
            if (check != "true") {
                __doPostBack(sender.id, 'OnClick');
                sessionStorage.removeItem("MandatoryCheck");
            }


        }
        else if (CurTab[0].innerText == "EXAM") {
            var subtab = localStorage.getItem("PrevSubTab");
            if (subtab == "General With Specialty") {
                $('.clsIframe').contents()[5].all.namedItem('generalwithspeciality').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "Focused") {
                $('.clsIframe').contents()[5].all.namedItem('Focused').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "Upload & View Photos") {
                $('.clsIframe').contents()[5].all.namedItem('UploadViewPhotos').children[0].contentDocument.all.namedItem('btnSave').click();
            }
            else if (subtab == "Body Image") {
                $('.clsIframe').contents()[5].all.namedItem('BodyImage').children[0].contentDocument.all.namedItem('btnSave').click();
            }

            //$(dvdialog).dialog("close");
            disableAutoSave();
            CurTab.tab('show');

        }
        else if (CurTab[0].innerText == "TEST") {
            $('.clsIframe').contents()[6].all.namedItem('btnSave').click();
            //$(dvdialog).dialog("close");
            disableAutoSave();
            CurTab.tab('show');
        }
        else if (CurTab[0].innerText == "ASSESSMENT") {
            $('.clsIframe').contents()[7].all.namedItem('btnSave').click();
            //$(dvdialog).dialog("close");
            enableAutoSave();
            CurTab.tab('show');
        }
        else if (CurTab[0].innerText == "ORDERS") {
            var subtab = localStorage.getItem("PrevSubTab");
            if (subtab == "Diagnostic Order") {
                $('.clsIframe').contents()[8].all.namedItem('DiagnosticOrder').children[0].contentDocument.all.namedItem('btnOrderSubmit').click();
            }
            else if (subtab == "Referral Order") {
                $('.clsIframe').contents()[8].all.namedItem('ReferralOrder').children[0].contentDocument.all.namedItem('btnAddRefOrder').click();
            }
            else if (subtab == "Immunization/Injection") {
                $('.clsIframe').contents()[8].all.namedItem('Immunization').children[0].contentDocument.all.namedItem('btnAdd').click();
            }
            else if (subtab == "Procedures") {
                $('.clsIframe').contents()[8].all.namedItem('InHouseProcedure').children[0].contentDocument.all.namedItem('btnAdd').click();
            }
            else if (subtab == "DME Order") {
                $('.clsIframe').contents()[8].all.namedItem('DMEOrder').children[0].contentDocument.all.namedItem('btnAdd').click();
            }
            //$(dvdialog).dialog("close");
            disableAutoSave();
            CurTab.tab('show');
        }
        else if (CurTab[0].innerText == "eRx") {
            paneID = $(event.target).attr('href');
            src = $(paneID).attr('data-src');
            $(paneID + " iframe").attr("src", src);
        }
        else if (CurTab[0].innerText == "SERV./PROC. CODES") {
            $('.clsIframe').contents()[11].all.namedItem('btnSave').click();
            //$(dvdialog).dialog("close");
            enableAutoSave();//BugID:52795
            CurTab.tab('show');
        }
        else if (CurTab[0].innerText == "PLAN") {
            var subtab = localStorage.getItem("PrevSubTab");
            //CAP-1463
            if (subtab == "General Plan") {
                $('.clsIframe').contents()[10]?.all?.namedItem('generalplan')?.children[0]?.contentDocument?.all?.namedItem('btnSave')?.click();
            }
            else if (subtab == "Individualized CarePlan") {
                $('.clsIframe').contents()[10]?.all?.namedItem('IndividualCarePlan')?.children[0]?.contentDocument?.all?.namedItem('btnSave')?.click();
            }
            else if (subtab == "Preventive Screen Plan") {
                $('.clsIframe').contents()[10]?.all?.namedItem('PreventiveScreen')?.children[0]?.contentDocument?.all?.namedItem('btnSave')?.click();
            }
            //$(dvdialog).dialog("close");
            disableAutoSave();
            CurTab.tab('show');

        }
        else if (CurTab[0].innerText == "SUMMARY") {
            $('.clsIframe').contents()[12].all.namedItem('btnSave').click();
            //$(dvdialog).dialog("close");
            disableAutoSave();
            CurTab.tab('show');
        }

        return;

        //event.preventDefault();
        //bCancel = true;
        //$(dvdialog).dialog({
        //    modal: true,
        //    title: "Capella -EHR",
        //    position: {
        //        my: 'left' + " " + 'center',
        //        at: 'center' + " " + 'center + 100px'

        //    },
        //    buttons: {
        //        "Yes": function () {
        //            sessionStorage.setItem("EncAutoSave", "true");
        //            if (CurTab[0].innerText == "CC / HPI") {
        //                $('.clsIframe').contents()[0].all.namedItem('btnAdd').click();
        //                $(dvdialog).dialog("close");
        //                enableAutoSave();
        //                CurTab.tab('show');
        //            }

        //            else if (CurTab[0].innerText == "SCREENING") {
        //                var subtab = localStorage.getItem("PrevSubTab");
        //                if (subtab == "General") {
        //                    $('.clsIframe').contents()[1].all.namedItem('General').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "TB Risk Assessment") {
        //                    $('.clsIframe').contents()[1].all.namedItem('TBRiskAssessment').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "Diabetic Foot Screening") {
        //                    $('.clsIframe').contents()[1].all.namedItem('DiabeticFootScreening').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "Sleep Screening") {
        //                    $('.clsIframe').contents()[1].all.namedItem('SleepScreening').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "Sleep") {
        //                    $('.clsIframe').contents()[1].all.namedItem('Sleep').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "Pulmonary") {
        //                    $('.clsIframe').contents()[1].all.namedItem('Pulmonary').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "Epworth Sleep Score") {
        //                    $('.clsIframe').contents()[1].all.namedItem('EpworthSleepScore').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "Pulmonary/Sleep Exam") {
        //                    $('.clsIframe').contents()[1].all.namedItem('PulmonarySleepExam').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "Dermatology Questionnaire") {
        //                    $('.clsIframe').contents()[1].all.namedItem('DermatologyQuestionnaire').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "Asthma Control Test") {
        //                    $('.clsIframe').contents()[1].all.namedItem('AsthmaControlTest').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "Depression") {
        //                    $('.clsIframe').contents()[1].all.namedItem('Depression').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "Depression Test") {
        //                    $('.clsIframe').contents()[1].all.namedItem('DepressionTest').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "Neck Disability Index") {
        //                    $('.clsIframe').contents()[1].all.namedItem('NeckDisabilityIndex').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "Oswestry Disability Index") {
        //                    $('.clsIframe').contents()[1].all.namedItem('OswestryDisabilityIndex').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "Development") {
        //                    $('.clsIframe').contents()[1].all.namedItem('Development').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }

        //                else if (subtab == "Chronic Cough Scale") {
        //                    $('.clsIframe').contents()[1].all.namedItem('ChronicCoughScale').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "Gynecological") {
        //                    $('.clsIframe').contents()[1].all.namedItem('Gynecological').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "Pediatric Sleep Questionnaire") {
        //                    $('.clsIframe').contents()[1].all.namedItem('PediatricSleepQuestionnaire').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "Sleep Short") {
        //                    $('.clsIframe').contents()[1].all.namedItem('SleepShort').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "Sleep Tendency Scale") {
        //                    $('.clsIframe').contents()[1].all.namedItem('SleepTendencyScale').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "Functional Assessment") {
        //                    $('.clsIframe').contents()[1].all.namedItem('FunctionalAssessment').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "Karnofsky") {
        //                    $('.clsIframe').contents()[1].all.namedItem('Karnofsky').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "Mini Mental") {
        //                    $('.clsIframe').contents()[1].all.namedItem('MiniMental').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "Nutritional Screening") {
        //                    $('.clsIframe').contents()[1].all.namedItem('NutritionalScreening').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "Safety Guidelines") {
        //                    $('.clsIframe').contents()[1].all.namedItem('SafetyGuidelines').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "Support Needs") {
        //                    $('.clsIframe').contents()[1].all.namedItem('SupportNeeds').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "Pain Assessment") {
        //                    $('.clsIframe').contents()[1].all.namedItem('PainAssessment').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "CAT Screening") {
        //                    $('.clsIframe').contents()[1].all.namedItem('CATScreening').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "COPD Breathe Well Program") {
        //                    $('.clsIframe').contents()[1].all.namedItem('COPDBreatheWellProgram').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "Fall Risk Screening") {
        //                    $('.clsIframe').contents()[1].all.namedItem('FallRiskScreening').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "Cognitive Screening") {
        //                    $('.clsIframe').contents()[1].all.namedItem('CognitiveScreening').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "Pain Screening") {
        //                    $('.clsIframe').contents()[1].all.namedItem('PainScreening').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "PHQ-9 Screening") {
        //                    $('.clsIframe').contents()[1].all.namedItem('PHQ-9Screening').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "Katz Index Screening") {
        //                    $('.clsIframe').contents()[1].all.namedItem('KatzIndexScreening').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "ADL Screening") {
        //                    $('.clsIframe').contents()[1].all.namedItem('ADLScreening').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "Get Up and Go") {
        //                    $('.clsIframe').contents()[1].all.namedItem('GetUpandGo').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "Lawton Screening") {
        //                    $('.clsIframe').contents()[1].all.namedItem('LawtonScreening').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "Anxiety Screening") {
        //                    $('.clsIframe').contents()[1].all.namedItem('AnxietyScreening').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "Spine Intake") {
        //                    $('.clsIframe').contents()[1].all.namedItem('SpineIntake').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "Monofilament Foot Exam") {
        //                    $('.clsIframe').contents()[1].all.namedItem('MonofilamentFootExam').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "Cervical Spine") {
        //                    $('.clsIframe').contents()[1].all.namedItem('CervicalSpine').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "Lumbar Spine") {
        //                    $('.clsIframe').contents()[1].all.namedItem('LumbarSpine').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "Urinalysis") {
        //                    $('.clsIframe').contents()[1].all.namedItem('Urinalysis').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "Staying Healthy Assessment") {
        //                    $('.clsIframe').contents()[1].all.namedItem('StayingHealthyAssessment').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "AUA BPH Symptom") {
        //                    $('.clsIframe').contents()[1].all.namedItem('AUABPHSymptom').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                $(dvdialog).dialog("close");
        //                enableAutoSave();
        //                CurTab.tab('show');
        //            }
        //            else if (CurTab[0].innerText == "PFSH") {
        //                var subtab = localStorage.getItem("PrevSubTab");
        //                if (subtab == "Past Medical History") {
        //                    $('.clsIframe').contents()[2].all.namedItem('pastmedHis').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "Surg./Proc.") {
        //                    $('.clsIframe').contents()[2].all.namedItem('surgproc').children[0].contentDocument.all.namedItem('btnAdd').click();
        //                }
        //                else if (subtab == "Hospitalization History") {
        //                    $('.clsIframe').contents()[2].all.namedItem('hospHis').children[0].contentDocument.all.namedItem('btnAdd').click();
        //                }
        //                else if (subtab == "Family History") {
        //                    $('.clsIframe').contents()[2].all.namedItem('famHis').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "Social History") {
        //                    $('.clsIframe').contents()[2].all.namedItem('socialHistory').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "Non Drug Allergy") {
        //                    $('.clsIframe').contents()[2].all.namedItem('NDA').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "Immunization History") {
        //                    $('.clsIframe').contents()[2].all.namedItem('immHis').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "AD") {
        //                    $('.clsIframe').contents()[2].all.namedItem('ad').children[0].contentDocument.all.namedItem('btnPFSHAutoSave').click();
        //                }
        //                $(dvdialog).dialog("close");
        //                disableAutoSave();
        //                CurTab.tab('show');
        //            }
        //            else if (CurTab[0].innerText == "ROS") {
        //                $('.clsIframe').contents()[3].all.namedItem('btnSave').click();
        //                $(dvdialog).dialog("close");
        //                disableAutoSave();
        //                CurTab.tab('show');
        //            }
        //            else if (CurTab[0].innerText == "VITALS") {
        //                $('.clsIframe').contents()[4].all.namedItem('btnSaveVitals').click();
        //                $(dvdialog).dialog("close");
        //                CurTab.tab('show');
        //                var check = sessionStorage.getItem("MandatoryCheck");
        //                if (check != "true") {
        //                    __doPostBack(sender.id, 'OnClick');
        //                    sessionStorage.removeItem("MandatoryCheck");
        //                }


        //            }
        //            else if (CurTab[0].innerText == "EXAM") {
        //                var subtab = localStorage.getItem("PrevSubTab");
        //                if (subtab == "General With Specialty") {
        //                    $('.clsIframe').contents()[5].all.namedItem('generalwithspeciality').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "Focused") {
        //                    $('.clsIframe').contents()[5].all.namedItem('Focused').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "Upload & View Photos") {
        //                    $('.clsIframe').contents()[5].all.namedItem('UploadViewPhotos').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "Body Image") {
        //                    $('.clsIframe').contents()[5].all.namedItem('BodyImage').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }

        //                $(dvdialog).dialog("close");
        //                disableAutoSave();
        //                CurTab.tab('show');

        //            }
        //            else if (CurTab[0].innerText == "TEST") {
        //                $('.clsIframe').contents()[6].all.namedItem('btnSave').click();
        //                $(dvdialog).dialog("close");
        //                disableAutoSave();
        //                CurTab.tab('show');
        //            }
        //            else if (CurTab[0].innerText == "ASSESSMENT") {
        //                $('.clsIframe').contents()[7].all.namedItem('btnSave').click();
        //                $(dvdialog).dialog("close");
        //                enableAutoSave();
        //                CurTab.tab('show');
        //            }
        //            else if (CurTab[0].innerText == "ORDERS") {
        //                var subtab = localStorage.getItem("PrevSubTab");
        //                if (subtab == "Diagnostic Order") {
        //                    $('.clsIframe').contents()[8].all.namedItem('DiagnosticOrder').children[0].contentDocument.all.namedItem('btnOrderSubmit').click();
        //                }
        //                else if (subtab == "Referral Order") {
        //                    $('.clsIframe').contents()[8].all.namedItem('ReferralOrder').children[0].contentDocument.all.namedItem('btnAddRefOrder').click();
        //                }
        //                else if (subtab == "Immunization/Injection") {
        //                    $('.clsIframe').contents()[8].all.namedItem('Immunization').children[0].contentDocument.all.namedItem('btnAdd').click();
        //                }
        //                else if (subtab == "Procedures") {
        //                    $('.clsIframe').contents()[8].all.namedItem('InHouseProcedure').children[0].contentDocument.all.namedItem('btnAdd').click();
        //                }
        //                else if (subtab == "DME Order") {
        //                    $('.clsIframe').contents()[8].all.namedItem('DMEOrder').children[0].contentDocument.all.namedItem('btnAdd').click();
        //                }
        //                $(dvdialog).dialog("close");
        //                disableAutoSave();
        //                CurTab.tab('show');
        //            }
        //            else if (CurTab[0].innerText == "eRx") {
        //                paneID = $(event.target).attr('href');
        //                src = $(paneID).attr('data-src');
        //                $(paneID + " iframe").attr("src", src);
        //            }
        //            else if (CurTab[0].innerText == "SERV./PROC. CODES") {
        //                $('.clsIframe').contents()[11].all.namedItem('btnSave').click();
        //                $(dvdialog).dialog("close");
        //                enableAutoSave();//BugID:52795
        //                CurTab.tab('show');
        //            }
        //            else if (CurTab[0].innerText == "PLAN") {
        //                var subtab = localStorage.getItem("PrevSubTab");
        //                if (subtab == "General Plan") {
        //                    $('.clsIframe').contents()[10].all.namedItem('generalplan').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "Individualized CarePlan") {
        //                    $('.clsIframe').contents()[10].all.namedItem('IndividualCarePlan').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                else if (subtab == "Preventive Screen Plan") {
        //                    $('.clsIframe').contents()[10].all.namedItem('PreventiveScreen').children[0].contentDocument.all.namedItem('btnSave').click();
        //                }
        //                $(dvdialog).dialog("close");
        //                disableAutoSave();
        //                CurTab.tab('show');

        //            }
        //            else if (CurTab[0].innerText == "SUMMARY") {
        //                $('.clsIframe').contents()[12].all.namedItem('btnSave').click();
        //                $(dvdialog).dialog("close");
        //                disableAutoSave();
        //                CurTab.tab('show');
        //            }

        //            return;
        //        },
        //        "No": function () {
        //            $(dvdialog).dialog("close");
        //            $(dvdialog).remove();
        //            disableAutoSave();//BugID:48506
        //            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        //            bCancel = true;
        //            CurTab.tab('show');
        //            __doPostBack(sender.id, 'OnClick');
        //        },
        //        "Cancel": function () {
        //            enableAutoSave();
        //            $(dvdialog).dialog("close");
        //            $(dvdialog).remove();
        //            CurTab.tab('show');
        //            return;
        //        }
        //    }
        //});
    }
    else {
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        if ($(".ui-dialog").is(":visible")) {
            $(dvdialog).dialog("close");
            $(dvdialog).remove();
        }
        if (bCancel) {
            __doPostBack(sender.id, 'OnClick');
            bCancel = false;
        }
    }
}

function SummaryXMlAlert() {
    document.getElementById("SummaryAlert").style.display = "block";
    document.getElementById("divEncounter").style.display = "none";
}

function disableAutoSave() {
    localStorage.setItem("bSave", "true");
    sessionStorage.setItem("bCCSave", "false");//BugID:48506
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
}
function enableAutoSave() {
    localStorage.setItem("bSave", "false");
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
}
function CreateCodingException() {
    $(top.window.document).find("#TabException").modal({ backdrop: "static", keyboard: false }, 'show');
    $(top.window.document).find("#TabModalExceptionTitle")[0].textContent = "Create Coding Exception";
    $(top.window.document).find("#TabmdldlgException")[0].style.width = "950px";
    $(top.window.document).find("#TabmdldlgException")[0].style.height = "680px";
    var sPath = ""
    var patientName = $(top.window.document).find('#ctl00_C5POBody_lblPatientStrip')[0].innerHTML.split('|')[0].trim();
    sPath = "frmException.aspx?formName=" + "Create Coding Exception" + "&PatientName=" + patientName;
    $(top.window.document).find("#TabExceptionFrame")[0].style.height = "605px";
    $(top.window.document).find("#TabExceptionFrame")[0].contentDocument.location.href = sPath;
    $(top.window.document).find("#TabException").one("hidden.bs.modal", function (e) {
    });
    return false;
}
function FeedbackCodingException(Addendumid) {
    //CAP-2202
    if (localStorage.getItem("OpenFeedbackCoding") != "NO") {
        $(top.window.document).find("#TabException").modal({ backdrop: "static", keyboard: false }, 'show');
        $(top.window.document).find("#TabModalExceptionTitle")[0].textContent = "Feedback for Coding Exception";
        $(top.window.document).find("#TabmdldlgException")[0].style.width = "950px";
        $(top.window.document).find("#TabmdldlgException")[0].style.height = "800px";
        var sPath = "";
        var patientName = "";
        if ($("[id*='lblPatientStrip']") != null && $("[id*='lblPatientStrip']") != undefined && $("[id*='lblPatientStrip']")[0] != undefined)
            patientName = $("[id*='lblPatientStrip']")[0].innerHTML.split('|')[0].trim();
        sPath = "frmException.aspx?formName=" + "Feedback for Coding Exception" + "&PatientName=" + patientName + "&AddendumID=" + Addendumid;
        $(top.window.document).find("#TabExceptionFrame")[0].style.height = "725px";
        $(top.window.document).find("#TabExceptionFrame")[0].contentDocument.location.href = sPath;
        $(top.window.document).find("#TabException").one("hidden.bs.modal", function (e) {
        });
    }
    return false;
}

function reloadSummaryBar(encounterId, dateOfService) {
    //CAP-2596
    var encounterId = parseInt(encounterId);
    if ((encounterId ?? 0) > 0) {
        $.ajax({
            type: "POST",
            url: "frmRCopiaToolbar.aspx/LoadPatientSummaryBar",
            data: JSON.stringify({ EncID: encounterId, Enc_DOS: dateOfService }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: OnSuccessSummaryBar,
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
    RefreshNotification("Notify");
}

function copyPreviousClick(messageText, encounterId) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }

    var Continue = DisplayErrorMessageCopyPrevious('210017', '', messageText);

    if (Continue == undefined) {
        document.getElementById('hdnPreviousEncounterId').value = encounterId;
        return;
    }

    if (Continue == true) {
        document.getElementById('btnhiddenConfirmOk').click();
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    }
    else {
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        document.getElementById('hdnSelectPhysicianId').value = "";
    }
}

function copyPreviousClickError(messageText) {
    DisplayErrorMessage('172506', '', messageText);
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
}

function PromptMessage(Message, encounterId, dateOfService) {
    DisplayErrorMessage('172505', '', Message);
    reloadSummaryBar(encounterId, dateOfService);
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
}

function ProviderValidationCheck() {

    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }

    var hdnAppointmentProviderId = document.getElementById("hdnAppointmentProviderId").value;
    var hdnEncounterProviderId = document.getElementById("hdnEncounterProviderId").value;

    if (hdnAppointmentProviderId != hdnEncounterProviderId) {

        var hdnAppointmentProviderName = document.getElementById("hdnAppointmentProviderName").value;
        var hdnEncounterProviderName = document.getElementById("hdnEncounterProviderName").value;

        OpenProviderValidation(hdnAppointmentProviderId, hdnAppointmentProviderName, hdnEncounterProviderId, hdnEncounterProviderName);
        return false;
    }
    else {
        return true;
    }
}

function OpenProviderValidation(appointmentProviderId, appointmentProviderName, encounterProviderId, encounterProviderName) {

    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }

    var Message = "The appointment was initially fixed with <br\><span style='color:#4285f4;font-size: medium;'>' " + appointmentProviderName + " ' </span><br\> kindly select an option for loading the  Previous  encounter data";

    var bCheckTrue = localStorage.getItem("ErrorCheck");

    if (bCheckTrue == "true") {
        localStorage.__proto__.localStorageSave = null;
        localStorage.setItem("ErrorCheck", "");
        $(top.window.document).find('#divProviderValidation').off('click');
        $(top.window.document).find('#btnEncounterId').off('click');
        $(top.window.document).find('#btnCancel').off('click');
        return true;
    }
    else if (bCheckTrue == "false") {

        localStorage.__proto__.localStorageSave = null;
        localStorage.setItem("ErrorCheck", "");
        $(top.window.document).find('#divProviderValidation').off('click');
        $(top.window.document).find('#btnEncounterId').off('click');
        $(top.window.document).find('#btnCancel').off('click');
        return false;
    }
    else if (bCheckTrue == " ") {

        localStorage.__proto__.localStorageSave = null;
        localStorage.setItem("ErrorCheck", "");
        $(top.window.document).find('#divProviderValidation').off('click');
        $(top.window.document).find('#btnEncounterId').off('click');
        $(top.window.document).find('#btnCancel').off('click');
        return false;
    }

    if ($(top.window.document).find('#divProviderValidation').modal == undefined ||
        $(top.window.document).find('#divProviderValidation').length == 0) {

        if (window.confirm(Message) == true) {
            var z = 1;
            return true;
        }
        else {
            var z = 2;
            return false;
        }
    }

    $(top.window.document).find('#pMsg').html(Message);
    $(top.window.document).find('#divProviderValidation').modal({ backdrop: 'static', keyboard: false }, 'show');

    $(top.window.document).find('#btnAppointmentId').text(appointmentProviderName);
    $(top.window.document).find('#btnAppointmentId').css("display", "");

    $(top.window.document).find('#btnEncounterId').text(encounterProviderName);
    $(top.window.document).find('#btnEncounterId').css("display", "");
    $(top.window.document).find('#btnCancel').css("display", "");

    localStorage.__proto__.localStorageSave = arguments.callee.caller;

    $(top.window.document).find('#btnAppointmentId').click(function () {
        localStorage.setItem("ErrorCheck", "true");
        if (localStorage.__proto__.localStorageSave != null) {
            localStorage.localStorageSave()
        };
        $(top.window.document).find('#divProviderValidation').modal('hide');
        document.getElementById('hdnSelectPhysicianId').value = document.getElementById('hdnAppointmentProviderId').value;
        document.getElementById('btnHiddenCopyPreviousEncounter').click();
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        return false;
    });

    $(top.window.document).find('#btnEncounterId').click(function () {
        localStorage.setItem("ErrorCheck", "false");
        if (localStorage.__proto__.localStorageSave != null) {
            localStorage.localStorageSave()
        };
        $(top.window.document).find('#divProviderValidation').modal('hide');
        document.getElementById('hdnSelectPhysicianId').value = document.getElementById('hdnEncounterProviderId').value;
        document.getElementById('btnHiddenCopyPreviousEncounter').click();
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        return false;
    });

    $(top.window.document).find('#btnCancel').click(function () {
        localStorage.setItem("ErrorCheck", " ");
        if (localStorage.__proto__.localStorageSave != null) {
            localStorage.localStorageSave()
        };
        $(top.window.document).find('#divProviderValidation').modal('hide');
        return false;
    });
}




function QRCodeClick() {
    if (window.top.document.getElementById("notificationpopup").innerText != "NOTIFICATION : Loading...") {
        if ($(top.window.document).find("#QRCodeInfo") != undefined) {
            var locatn = "frmQRCodeGenerator.aspx";
            $(top.window.document).find('#QRCode_Modal')[0].contentDocument.location.href = locatn;
            //if (triggeredBy != undefined && triggeredBy != '' && triggeredBy == "MovetoNextProcess") {
            //    $(top.window.document).find("#AlertHeader")[0].innerHTML = "Warning";
            //    $(top.window.document).find("#btnCloseAlert").css("display", "none");
            //    $('#btnCloseAlert').removeClass('aspresizedredbutton');
            //    localStorage.setItem('trigerredBy', triggeredBy);
            //}
            //else {
            $(top.window.document).find("#QRCodeHeader")[0].innerHTML = "Scan QR code with your mobile device to dictate";
            $(top.window.document).find("#btnCloseQRCode").css("display", "block");
            $('#btnCloseQRCode').removeClass('btn btn-danger');
            $('#btnCloseQRCode').addClass('aspresizedredbutton');
            //}
            $(top.window.document).find("#QRCodeInfo")[0].style.display = "block";
            //Jira CAP-1215
            event.preventDefault();
        }
    }
}

//Jira CAP-1312
function StartLoadingcursor() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    return true;
}

function CloseQRCodeModal() {
    $("#QRCodeInfo")[0].style.display = "none";
}

function AkidoNoteClick() {
    var CLegalOrg = "";
    var cookies = document.cookie.split(';');
    for (var l = 0; l < cookies.length; l++) {
        if (cookies[l].indexOf("CLegalOrg") > -1)
            CLegalOrg = cookies[l].split("=")[1].toLowerCase();
    }
    var AkidoNoteURL = document.getElementById('hdnAkidoNote').value.replace("[CapellaEncounterID]", document.getElementById('hdnEncounterID').value).replace("[ClientName]", CLegalOrg);
    $('#resultLoading').css("display", "none");
    //Jira #CAP-714
    //Result = openNonModal(AkidoNoteURL, 780, 1250, obj);
    //if (Result == null)
    //    return false;

    //Jira CAP-1215
    event.preventDefault();
    sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart();

    window.open(AkidoNoteURL, '_blank');

    //Jira CAP-1215
    //return false;
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
}

//Jira CAP-1379
$("#tabStripEncounter_tbSummary").click(function () {
    if ($("ul#myTabs li.active")[0].innerText != "SUMMARY") {

        document.getElementById('WaitingMessage').style.display = 'block';
        document.getElementById('Summaryframe').style.display = 'none';
    }
});

//CAP-2678
function autoSaveAndMoveToNextProcess(sender) {
    const intervalId = setInterval(function () {
        const isSaveCompleted = localStorage.getItem("IsSaveCompleted");
        if (isSaveCompleted === "true" || isSaveCompleted == true) {
            clearInterval(intervalId);
            setTimeout(function () {
                __doPostBack(sender.id, 'OnClick');
            }, 500);
        } else if (localStorage.getItem("SaveUnsuccessful") == "true") {
            clearInterval(intervalId);
            localStorage.setItem("SaveUnsuccessful", "false");
        }
        localStorage.setItem("IsSaveCompleted", false);
    }, 500);
}