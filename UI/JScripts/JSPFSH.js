function RadTabStrip1_TabSelecting(sender, args) {
    var now = new Date();
    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
    document.getElementById(GetClientId("hdnDateTime")).value = utc;
    var sMandatory = "";
    if ($find('btnPFSH') != null) {
        if ($find('btnPFSH').get_enabled())
            document.getElementById('hdnTabPFSHEnable').value = "true";
        else
            document.getElementById('hdnTabPFSHEnable').value = "";

        var cbo = $find("cboSourceOfInformation");
        var textBox = $find("txtSourceOfInformation");

        if (cbo != null && textBox != null) {
            if (cbo.get_text() == "Others" || cbo.get_text() == "Other")
                textBox.set_value(textBox._text);
        }
    }

    if (document.getElementById('hdnSocialHistoryMandatory').value == "true") {
        DisplayErrorMessage('180020');
        EndWaitCursor();
        sMandatory = "true";
        args.set_cancel(true);
        return true;
    }

    if (document.getElementById('hdnMenuLevelAutoSave').value != "Menu" && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined) {
        var TabClick = window.parent.theForm.hdnTabClick;
        if (TabClick.value == "first") {
            if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value == "true") {
                TabClick.value = args._tab._element.textContent + "$#$";
                args.set_cancel(true);
                DisplayErrorMessage('1100000', 'PFSHTabClick');
                return;
            }
        }
        else {
            var splitvalue = TabClick.value.split('$#$');
            var clicked_tab = splitvalue[0];
            var switchcase = splitvalue[1];
            if (switchcase == "second,true") {
                var IDs = window.parent.theForm.hdnSaveButtonID.value.split(',');

                var childControlsofChildContainer = $find(IDs[1]).get_selectedPageView().get_element().getElementsByTagName("iframe")[0].contentWindow.$telerik.radControls;
                for (var count = (childControlsofChildContainer.length - 1) ; count >= 0; count--) {
                    if (childControlsofChildContainer[count]._element.id == IDs[0]) {
                        var save_button = childControlsofChildContainer[count];
                        var refresh_summary_bar = $find(IDs[1]).get_selectedPageView().get_element().getElementsByTagName("iframe")[0].contentDocument.getElementById('hdnTabSelected');
                        if (refresh_summary_bar != undefined || refresh_summary_bar != null)
                            refresh_summary_bar.value = "true";
                        if ($find(IDs[1]).get_selectedPageView()._contentUrl.indexOf('frmOtherHistory.aspx') > -1)
                            var add_button = $find(IDs[1]).get_selectedPageView().get_element().getElementsByTagName("iframe")[0].contentDocument.getElementById('btnSave');
                        if (save_button != undefined || save_button != null) {
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
            else if (switchcase == "second,false") {
                window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                if (sMandatory == "")
                    LaodWaitCursor();
            }
            else if (switchcase == "second,cancel") {
                args.set_cancel(true);
            }
            TabClick.value = "first";
        }

        document.getElementById('hdnSaveEnable').value = "false"
    }
    else {
        var TabClick = window.parent.theForm.hdnTabClick;
        if (TabClick.value == "first") {
            if (document.getElementById('hdnSaveEnable').value == "true") {
                TabClick.value = args._tab._element.textContent + "$#$";
                args.set_cancel(true);
                DisplayErrorMessage('1100000', 'PFSHTabClick');
                return;
            }
        }
        else {
            var splitvalue = TabClick.value.split('$#$');
            var clicked_tab = splitvalue[0];
            var switchcase = splitvalue[1];
            if (switchcase == "second,true") {
                var IDs = window.parent.theForm.hdnSaveButtonID.value.split(',');

                var childControlsofChildContainer = $find(IDs[1]).get_selectedPageView().get_element().getElementsByTagName("iframe")[0].contentWindow.$telerik.radControls;
                for (var count = (childControlsofChildContainer.length - 1) ; count >= 0; count--) {
                    if (childControlsofChildContainer[count]._element.id == IDs[0]) {
                        var save_button = childControlsofChildContainer[count];
                        if (save_button != undefined || save_button != null) {
                            args.set_cancel(true);
                            TabClick.value = clicked_tab + "$#$third";
                            save_button.click();
                            return;
                        }
                        break;
                    }
                }

            }
            else if (switchcase == "second,false") {
                document.getElementById('hdnSaveEnable').value = "false";
                if (sMandatory == "")
                    LaodWaitCursor();
            }
            else if (switchcase == "second,cancel") {
                args.set_cancel(true);
            }
            TabClick.value = "first";
        }

    }
}


function txtVisible(Parameter) {
    var Tcontrols = $find("txtSourceOfInformation");
    if (Parameter == "true") {
        Tcontrols.set_visible(true);
    }
    else {
        Tcontrols.set_visible(false);
    }
}



function OnClientSelectedIndexChanged(sender, eventArgs) {

    var item = eventArgs.get_item();
    var Tcontrols = $find("txtSourceOfInformation");
    if (item.get_text() == "Other" || item.get_text() == "Others") {
        Tcontrols.set_visible(true);
    }
    else {
        Tcontrols.set_visible(false);
    }

}

function LaodWaitCursor() {
    document.getElementById('divLoading').style.display = "block";
}

function EndWaitCursor() {
}

function btnPFSH_Clicked(sender, args) {
    var now = new Date();
    var then = now.getDay() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear(); then += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds();
    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
    document.getElementById(GetClientId("hdnDateTime")).value = utc;
    parent.document.getElementById('hdnPFSHVerifiedEnable').value = "FALSE";
    window.parent.parent.parent.theForm.ctl00_C5POBody_hdnPFSHOthertxt.value = "false";
}

function cboSourceOfInformation_SelectedIndexChanged(sender, args) {
    if (sender._text == "Other" || sender._text == "Others") {
        $find('txtSourceOfInformation').set_visible(true);


    }
    else {
        window.parent.parent.parent.theForm.ctl00_C5POBody_hdnPFSHOthertxt.value = "false";
        $find('txtSourceOfInformation').set_visible(false);
    }
    parent.document.getElementById('hdnPFSHInfoBy').value = args._item._text;
    parent.document.getElementById('hdnPFSHVerifiedEnable').value = "TRUE";

    $find('btnPFSH').set_enabled(true);

    parent.document.getElementById('hdnPFSHVerifiedEnable').value = "TRUE";
}

function txtSourceOfInformation_OnKeyPress(sender, args) {

    $find('btnPFSH').set_enabled(true);
    parent.document.getElementById('hdnPFSHVerifiedEnable').value = "TRUE";
    window.parent.parent.parent.theForm.ctl00_C5POBody_hdnPFSHOthertxt.value = "true";
}

function LoadEnableDisable() {
    var cboSourceOfInformation = $("#cboSourceOfInformation")[0];
    if (cboSourceOfInformation != null && cboSourceOfInformation != undefined) {
        if (cboSourceOfInformation.value != "Other" && cboSourceOfInformation.value != "Others") {
            $("#txtSourceOfInformation")[0].style.visibility = "hidden";
            $("#txtSourceOfInformation")[0].value = "";
        }
    }
}

function SetACOFlag(FlagValue) {
    parent.document.getElementById('hdnIsACOValid').value = FlagValue;
}
var Sessionvalues;
var PhysicianName = "";
var PhysicianID = "";
var CurTab;
var PrevTab;
sessionStorage.setItem('bPFSHCancel', 'false');
var openingFrom = "";
var now = new Date();
var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
var PFSH_tab = [];
var PFSH_tab_to_disable;
$(document).ready(function () {
    $('#myTabs li a').click(function () {
        localStorage.setItem("notification", "");
        var is_disabled = false;
        if (PFSH_tab != undefined && PFSH_tab.length > 0) {
            for (var t = 0; t < PFSH_tab.length; t++) {
                if (this.hash == "#" + PFSH_tab[t]) {
                    event.stopPropagation();
                    is_disabled = true;
                }
            }
            if (is_disabled == false) {
                if ($("ul#myTabs li.active a")[0].innerText != this.innerText)
                { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
            }
        }
        else {
            if ($("ul#myTabs li.active a")[0].innerText != this.innerText)
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        }
    });


    $("#btnHiddenPFSH").click(function () {
        var PrevTabtxt = sessionStorage.getItem('PFSH_PrevTabText');
        $("a:contains('" + PrevTabtxt + "')").tab('show');
    });
    var value = window.location.search.toString();
    var HumanID = "";

    if (value != "" && value.indexOf('&') > -1) {
        HumanID = value.split('&')[0].split('=')[1];
        openingFrom = value.split('&')[1].split('=')[1];
    }
    if (openingFrom == "Menu") {
        $("#lblSelectPhysician").show();
        $("#cboPhysician").show();
        $("#chkShowAllPhysician").show();
        $("#lblShowAllPhysician").show();
        $("#lblSourceOfInfo").hide();
        $("#txtSourceOfInformation").hide();
        $("#cboSourceOfInformation").hide();
        $("#btnPFSHVerified").hide();
        var FacilityRole = window.parent.parent.parent.theForm.ctl00_C5POBody_hdnFacilityRole.value;
        if (FacilityRole != "") {
            LoadPhysicianList();

        }
    }

    else {
        $("#lblSelectPhysician").hide();
        $("#cboPhysician").hide();
        $("#chkShowAllPhysician").hide();
        $("#lblShowAllPhysician").hide();
        document.getElementById("btnClose").style.display = 'none';
        openingFrom = "Queue";
    }
    var cboSourceOfInformation = $("#cboSourceOfInformation")[0];
    if (cboSourceOfInformation != null && cboSourceOfInformation != undefined) {
        if (cboSourceOfInformation.value != "Other" && cboSourceOfInformation.value != "Others") {
            $("#txtSourceOfInformation")[0].style.visibility = "hidden";
            $("#txtSourceOfInformation")[0].value = "";
        }
    }
    var cookies = document.cookie.split(';');
    var CUserRole = "";
    for (var l = 0; l < cookies.length; l++) {
        if (cookies[l].indexOf("CUserRole") > -1)
            CUserRole = cookies[l].split("=")[1];
    }
    if (CUserRole == "Coder")
    {
        cboSourceOfInformation.disabled = true;
    }
    else
    {
        cboSourceOfInformation.disabled = false;
    }
    if ($("ul#myTabs li.active").length != 0) {
        if (localStorage.getItem("notification") == 'Social') {
            $('#myTabs a')[4].click();
            var target = $('#myTabs li:eq(4) a').tab('show')
            paneID = $(target).attr('href');
        }
        else if (localStorage.getItem("notification") == 'Immunization') {
            $('#myTabs a')[6].click();
            var target = $('#myTabs li:eq(6) a').tab('show')
            paneID = $(target).attr('href');
        }
        else if (localStorage.getItem("bSave") == "true") {
            var target = $('#myTabs a:first').tab('show');
            localStorage.setItem("PrevSubTab", target[0].innerText);
            paneID = $(target).attr('href');

        }



        else {
            if (localStorage.getItem("PrevSubTab") == "Past Medical History") {
                var target = $('#myTabs a:first').tab('show');
                localStorage.setItem("PrevSubTab", target[0].innerText);
                paneID = $(target).attr('href');
                localStorage.setItem("bSave", "true");
            }
            else if (localStorage.getItem("PrevSubTab") == "Surg./Proc.") {
                var target = $('#myTabs li:eq(1) a').tab('show')
                localStorage.setItem("PrevSubTab", target[0].innerText);
                paneID = $(target).attr('href');
                localStorage.setItem("bSave", "true");
            }
            else if (localStorage.getItem("PrevSubTab") == "Hospitalization History") {
                var target = $('#myTabs li:eq(2) a').tab('show')
                localStorage.setItem("PrevSubTab", target[0].innerText);
                paneID = $(target).attr('href');
                localStorage.setItem("bSave", "true");
            }
            else if (localStorage.getItem("PrevSubTab") == "Family History") {
                var target = $('#myTabs li:eq(3) a').tab('show')
                localStorage.setItem("PrevSubTab", target[0].innerText);
                paneID = $(target).attr('href');
                localStorage.setItem("bSave", "true");
            }
            else if (localStorage.getItem("PrevSubTab") == "Social History") {
                var target = $('#myTabs li:eq(4) a').tab('show')
                localStorage.setItem("PrevSubTab", target[0].innerText);
                paneID = $(target).attr('href');
                localStorage.setItem("bSave", "true");
            }
            //else if (localStorage.getItem("PrevSubTab") == "Rx History") {
            //    var target = $('#myTabs li:eq(5) a').tab('show')
            //    localStorage.setItem("PrevSubTab", target[0].innerText);
            //    paneID = $(target).attr('href');
            //    localStorage.setItem("bSave", "true");
            //}
            else if (localStorage.getItem("PrevSubTab") == "Non Drug Allergy") {
                var target = $('#myTabs li:eq(5) a').tab('show')
                localStorage.setItem("PrevSubTab", target[0].innerText);
                paneID = $(target).attr('href');
                localStorage.setItem("bSave", "true");
            }
            //else if (localStorage.getItem("PrevSubTab") == "Drug Allergy") {
            //    var target = $('#myTabs li:eq(7) a').tab('show')
            //    localStorage.setItem("PrevSubTab", target[0].innerText);
            //    paneID = $(target).attr('href');
            //    localStorage.setItem("bSave", "true");
            //}
            else if (localStorage.getItem("PrevSubTab") == "Immunization History") {
                var target = $('#myTabs li:eq(6) a').tab('show')
                localStorage.setItem("PrevSubTab", target[0].innerText);
                paneID = $(target).attr('href');
                localStorage.setItem("bSave", "true");
            }


            else if (localStorage.getItem("PrevSubTab") == "AD") {
                var target = $('#myTabs li:eq(7) a').tab('show')
                localStorage.setItem("PrevSubTab", target[0].innerText);
                paneID = $(target).attr('href');
                localStorage.setItem("bSave", "true");
            }
            else {
                var target = $('#myTabs a:first').tab('show');
                localStorage.setItem("PrevSubTab", target[0].innerText);
                localStorage.setItem("bSave", "true");
                paneID = $(target).attr('href');
            }
        }
        if ($("#cboPhysician")[0] != undefined && $("#cboPhysician")[0].options.length > 0) {
            PhysicianName = $("#cboPhysician")[0].options[$("#cboPhysician")[0].selectedIndex].text.split('-')[0];
            PhysicianID = $("#cboPhysician")[0].options[$("#cboPhysician")[0].selectedIndex].value;
        }
        if (PhysicianName != "" || openingFrom == "Queue") {
            $.ajax({
                type: "POST",
                url: "WebServices/PastMedicalHistoryService.asmx/LoadPFSHtab",
                contentType: "application/json;charset=utf-8",
                data: JSON.stringify({
                    "HumanID": HumanID,
                    "OpeningFrom": openingFrom,
                    "PhysicianName": PhysicianName,
                    "PhysicianID": PhysicianID
                }),
                dataType: "json",
                async: true,
                success: function (data) {
                    Sessionvalues = (data.d);
                    src = $(paneID).attr('data-src') + "?" + (data.d);
                    var tab_disable = (data.d).split('-')[1].split('=')[1];
                    if (tab_disable != "") {
                        PFSH_tab_to_disable = JSON.parse((data.d).split('-')[1].split('=')[1]);
                        for (var i = 0; i < PFSH_tab_to_disable.length; i++) {
                            PFSH_tab.push(PFSH_tab_to_disable[i].tab.split('_')[1].replace("tb", ""));
                            $("#myTabs a[href*='" + PFSH_tab[i] + "']").addClass("disableTab");
                        }
                    }
                    $(paneID + " iframe").attr("src", src);
                    if (Sessionvalues.split('~')[1] != "")
                        $("#cboSourceOfInformation").val(Sessionvalues.split('~')[1]);
                    if ($("#cboSourceOfInformation :selected").text() == "Other") {
                        $("#hdntxtOthersValue").val(Sessionvalues.split('~')[3]);
                        document.getElementById("txtSourceOfInformation").value = Sessionvalues.split('~')[3];
                        document.getElementById("txtSourceOfInformation").style.visibility = 'visible';
                    }
                    else
                        $("#hdntxtOthersValue").val("");
                    var PFSHVerified = localStorage.getItem("PFSHVerified");
                    var Verified = "";
                    if (PFSHVerified != "") {
                        var PFSH = PFSHVerified.split('|');
                        for (var i = 0; i < PFSH.length; i++) {
                            if (PFSH[i].split('-')[0] == Sessionvalues.split('~')[4].split('&')[0]) {
                                Verified = PFSH[i].split('-')[1];
                                break;
                            }
                        }
                    }
                    if (Verified == "" || Verified == "TRUE")
                        $("#btnPFSHVerified")[0].disabled = false;
                    else
                        $("#btnPFSHVerified")[0].disabled = true;
                    if (Sessionvalues.split('~')[2] == "Y" && Verified == "")
                        $("#btnPFSHVerified")[0].disabled = true;
                    if ($("#txtSourceOfInformation").css("visibility") == "hidden")
                        $("#txtSourceOfInformation").val($("#hdntxtOthersValue").value);
                    if (CUserRole == "Coder") {
                        $("#btnPFSHVerified")[0].disabled = true;
                    }
                },

                error: function OnError(xhr) {
                    if (xhr.status == 999)
                        window.location = xhr.statusText;
                    else {
                        var log = JSON.parse(xhr.responseText);
                        console.log(log);
                        //alert("USER MESSAGE:\n" +
                        //            ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                        //           "Message: " + log.Message);

                        window.location = "ErrorPage.aspx?Message=" + log.Message + "|$|" + log.StackTrace;;

                    }
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                }
            });
        } else if (PhysicianName != "" || openingFrom == "Menu") {
            $.ajax({
                type: "POST",
                url: "WebServices/PastMedicalHistoryService.asmx/LoadPFSHtabMenu",
                contentType: "application/json;charset=utf-8",
                data: JSON.stringify({
                    "HumanID": HumanID,
                    "OpeningFrom": openingFrom,
                    "PhysicianName": PhysicianName,
                    "PhysicianID": PhysicianID
                }),
                dataType: "json",
                async: true,
                success: function (data) {
                    Sessionvalues = (data.d);
                    src = $(paneID).attr('data-src') + "?" + (data.d);
                    var tab_disable = (data.d).split('-')[1].split('=')[1];
                    if (tab_disable != "") {
                        PFSH_tab_to_disable = JSON.parse((data.d).split('-')[1].split('=')[1]);
                        for (var i = 0; i < PFSH_tab_to_disable.length; i++) {
                            PFSH_tab.push(PFSH_tab_to_disable[i].tab.split('_')[1].replace("tb", ""));
                            $("#myTabs a[href*='" + PFSH_tab[i] + "']").addClass("disableTab");
                        }
                    }
                    //For bug Id :  60957 
                    if (Sessionvalues.split('~')[1] != "")
                        $("#cboSourceOfInformation").val(Sessionvalues.split('~')[1]);
                    if ($("#cboSourceOfInformation :selected").text() == "Other") {
                        $("#hdntxtOthersValue").val(Sessionvalues.split('~')[3]);
                        document.getElementById("txtSourceOfInformation").value = Sessionvalues.split('~')[3];
                        document.getElementById("txtSourceOfInformation").style.visibility = 'visible';
                    }
                    else
                        $("#hdntxtOthersValue").val("");
                    var PFSHVerified = localStorage.getItem("PFSHVerified");
                    var Verified = "";
                    if (PFSHVerified != "") {
                        var PFSH = PFSHVerified.split('|');
                        for (var i = 0; i < PFSH.length; i++) {
                            if (PFSH[i].split('-')[0] == Sessionvalues.split('~')[4].split('&')[0]) {
                                Verified = PFSH[i].split('-')[1];
                                break;
                            }
                        }
                    }
                    if (Verified == "" || Verified == "TRUE")
                        $("#btnPFSHVerified")[0].disabled = false;
                    else
                        $("#btnPFSHVerified")[0].disabled = true;
                    if (Sessionvalues.split('~')[2] == "Y" && Verified == "")
                        $("#btnPFSHVerified")[0].disabled = true;
                    if (CUserRole == "Coder") {
                        $("#btnPFSHVerified")[0].disabled = true;
                    }
                    if ($("#txtSourceOfInformation").css("visibility") == "hidden")
                        $("#txtSourceOfInformation").val($("#hdntxtOthersValue").value);


                },

                error: function OnError(xhr) {
                    if (xhr.status == 999)
                        window.location = xhr.statusText;
                    else {
                        var log = JSON.parse(xhr.responseText);
                        console.log(log);
                        //alert("USER MESSAGE:\n" +
                        //            ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                        //           "Message: " + log.Message);

                        window.location = "ErrorPage.aspx?Message=" + log.Message + "|$|" + log.StackTrace;;

                    }
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                }
            });
        }
    }
});

function LoadPFSHTabs(event) {
    if ($("#cboPhysician")[0] != undefined && $("#cboPhysician")[0].options.length > 0) {
        PhysicianName = $("#cboPhysician")[0].options[$("#cboPhysician")[0].selectedIndex].text.split('-')[0];
    }
    if (PhysicianName != "" || openingFrom == "Queue") {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        CurTab = $(event.target);         // active tab
        PrevTab = $(event.relatedTarget);  // previous tab
        localStorage.setItem("PrevSubTab", CurTab[0].innerText);
        var myPos, atPos;
        if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value == "true") {
            event.preventDefault();
            sessionStorage.setItem("AutoSave_PFSH", "false");
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
            //$(dvdialog).dialog("close");
            //$(dvdialog).remove();
            sessionStorage.setItem("AutoSave_PFSH", "true");
            if (PrevTab[0].innerText == "Past Medical History") {
                paneID = $(event.target).attr('href');
                sessionStorage.setItem('PFSH_paneID', paneID);
                src = $(paneID).attr('data-src') + "?" + Sessionvalues;
                sessionStorage.setItem('PFSH_src', src);
                var prevTabtxt = $(PrevTab).text();
                sessionStorage.setItem('PFSH_PrevTabText', prevTabtxt);
                //CAP-537 : PFSH - Family History screen loading for long time
                $(paneID + " iframe").attr("src", src);
                $('.clsIframe').contents()[0].all.namedItem('btnSave').click();
            }
            else if (PrevTab[0].innerText == "Surg./Proc.") {
                paneID = $(event.target).attr('href');
                sessionStorage.setItem('PFSH_paneID', paneID);
                src = $(paneID).attr('data-src') + "?" + Sessionvalues;
                sessionStorage.setItem('PFSH_src', src);
                var prevTabtxt = $(PrevTab).text();
                sessionStorage.setItem('PFSH_PrevTabText', prevTabtxt);
                
                //CAP-537 : PFSH - Family History screen loading for long time
                $(paneID + " iframe").attr("src", src);
                $('.clsIframe').contents()[1]?.all?.namedItem('btnAdd')?.click();
            }
            else if (PrevTab[0].innerText == "Hospitalization History") {
                paneID = $(event.target).attr('href');
                sessionStorage.setItem('PFSH_paneID', paneID);
                src = $(paneID).attr('data-src') + "?" + Sessionvalues;
                sessionStorage.setItem('PFSH_src', src);
                var prevTabtxt = $(PrevTab).text();
                sessionStorage.setItem('PFSH_PrevTabText', prevTabtxt);
                //CAP-537 : PFSH - Family History screen loading for long time
                $(paneID + " iframe").attr("src", src);
                if ($('.clsIframe').contents()[2].all.namedItem('btnAdd') != null && $('.clsIframe').contents()[2].all.namedItem('btnAdd') != undefined) {
                    $('.clsIframe').contents()[2].all.namedItem('btnAdd').click();
                }

            }
            else if (PrevTab[0].innerText == "Family History") {
                paneID = $(event.target).attr('href');
                sessionStorage.setItem('PFSH_paneID', paneID);
                src = $(paneID).attr('data-src') + "?" + Sessionvalues;
                sessionStorage.setItem('PFSH_src', src);
                var prevTabtxt = $(PrevTab).text();
                sessionStorage.setItem('PFSH_PrevTabText', prevTabtxt);
                //CAP-537 : PFSH - Family History screen loading for long time
                $(paneID + " iframe").attr("src", src);
                $('.clsIframe').contents()[3]?.all?.namedItem('btnSave')?.click();
            }
            else if (PrevTab[0].innerText == "Social History") {
                paneID = $(event.target).attr('href');
                sessionStorage.setItem('PFSH_paneID', paneID);
                src = $(paneID).attr('data-src') + "?" + Sessionvalues;
                sessionStorage.setItem('PFSH_src', src);
                var prevTabtxt = $(PrevTab).text();
                sessionStorage.setItem('PFSH_PrevTabText', prevTabtxt);
                //CAP-537 : PFSH - Family History screen loading for long time
                $(paneID + " iframe").attr("src", src);
                // CAP-303 Cannot read properties of null (reading click)
                $('.clsIframe').contents()[4]?.all?.namedItem('btnSave')?.click();

            }
            //else if (PrevTab[0].innerText == "Rx History") {
            //    paneID = $(event.target).attr('href');
            //    sessionStorage.setItem('PFSH_paneID', paneID);
            //    src = $(paneID).attr('data-src') + "?" + Sessionvalues;
            //    sessionStorage.setItem('PFSH_src', src);
            //    var prevTabtxt = $(PrevTab).text();
            //    sessionStorage.setItem('PFSH_PrevTabText', prevTabtxt);
            //    $('.clsIframe').contents()[5].all.namedItem('btnAdd').click();
            //}
            else if (PrevTab[0].innerText == "Non Drug Allergy") {
                paneID = $(event.target).attr('href');
                sessionStorage.setItem('PFSH_paneID', paneID);
                src = $(paneID).attr('data-src') + "?" + Sessionvalues;
                sessionStorage.setItem('PFSH_src', src);
                var prevTabtxt = $(PrevTab).text();
                sessionStorage.setItem('PFSH_PrevTabText', prevTabtxt);
                //CAP-537 : PFSH - Family History screen loading for long time
                $(paneID + " iframe").attr("src", src);
                $('.clsIframe').contents()[5].all.namedItem('btnSave').click();
            }
            //else if (PrevTab[0].innerText == "Drug Allergy") {
            //    paneID = $(event.target).attr('href');
            //    sessionStorage.setItem('PFSH_paneID', paneID);
            //    src = $(paneID).attr('data-src') + "?" + Sessionvalues;
            //    sessionStorage.setItem('PFSH_src', src);
            //    var prevTabtxt = $(PrevTab).text();
            //    sessionStorage.setItem('PFSH_PrevTabText', prevTabtxt);
            //    $('.clsIframe').contents()[7].all.namedItem('btnAdd').click();
            //}
            else if (PrevTab[0].innerText == "Immunization History") {
                paneID = $(event.target).attr('href');
                sessionStorage.setItem('PFSH_paneID', paneID);
                src = $(paneID).attr('data-src') + "?" + Sessionvalues;
                sessionStorage.setItem('PFSH_src', src);
                var prevTabtxt = $(PrevTab).text();
                sessionStorage.setItem('PFSH_PrevTabText', prevTabtxt);
                //CAP-537 : PFSH - Family History screen loading for long time
                $(paneID + " iframe").attr("src", src);
                $('.clsIframe').contents()[6].all.namedItem('btnSave').click();

            }
            else if (PrevTab[0].innerText == "AD") {
                paneID = $(event.target).attr('href');
                sessionStorage.setItem('PFSH_paneID', paneID);
                src = $(paneID).attr('data-src') + "?" + Sessionvalues;
                sessionStorage.setItem('PFSH_src', src);
                var prevTabtxt = $(PrevTab).text();
                sessionStorage.setItem('PFSH_PrevTabText', prevTabtxt);
                //CAP-537 : PFSH - Family History screen loading for long time
                $(paneID + " iframe").attr("src", src);
                $('.clsIframe').contents()[7].all.namedItem('btnPFSHAutoSave').click();
            }

        }
        else {
            if ($(".ui-dialog").is(":visible")) {
                $(dvdialog).dialog("close");
                $(dvdialog).remove();
            }
            if (JSON.parse(sessionStorage.getItem('bPFSHCancel')) == false) {
                paneID = $(event.target).attr('href');
                src = $(paneID).attr('data-src') + "?" + Sessionvalues;
                $(paneID + " iframe").attr("src", src);
            } else { //bPFSHCancel = true;
                sessionStorage.setItem('bPFSHCancel', 'false');
                localStorage.setItem("bSave", "false");
                window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            }

        }
        if (paneID == "#pastmedHis") {
            var HtmlVersion = $(paneID)[0].attributes.getNamedItem('data-src').nodeValue;
            if (HtmlVersion.indexOf('?') > -1) {
                if (HtmlVersion.split('?')[1].split("=")[1] != sessionStorage.getItem("ScriptVersion")) {
                    var HtmlVersion = $(paneID)[0].attributes.getNamedItem('data-src').nodeValue.split('?')[0] + "?version=" + sessionStorage.getItem("ScriptVersion");
                }
            }
            else
                var HtmlVersion = $(paneID)[0].attributes.getNamedItem('data-src').nodeValue + "?version=" + sessionStorage.getItem("ScriptVersion");

            $(paneID).attr('data-src', HtmlVersion);
        }

    }
    else { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
}
$('.nav-tabs a').on('shown.bs.tab', function (event) {
    
    var is_disabled = false;
    if (PFSH_tab != undefined && PFSH_tab.length > 0) {
        for (var t = 0; t < PFSH_tab.length; t++) {
            if (this.hash == "#" + PFSH_tab[t])//to disable BodyImage Tab
            {
                event.stopPropagation();
                is_disabled = true;
            }
        }
        if (is_disabled == false) {
            LoadPFSHTabs(event);
        }
    }
    else
        LoadPFSHTabs(event);

});
var val;
$("#btnPFSHVerified").click(function () {
    if ($("#cboSourceOfInformation option:selected").text() == "Other" || $("#cboSourceOfInformation option:selected").text() == "Others") {
        if (document.getElementById("txtSourceOfInformation").value == "") {
            DisplayErrorMessage('180008');
            document.getElementById("txtSourceOfInformation").focus();
            return false;
        }
    }
    $.ajax({
        type: "POST",
        url: "WebServices/PastMedicalHistoryService.asmx/PFSH_click",
        contentType: "application/json;charset=utf-8",
        data: JSON.stringify({
            S_of_info: $("#cboSourceOfInformation :selected").text(), hdnDateTime: utc,
            txtS_of_info: document.getElementById("txtSourceOfInformation").value,
        }),
        dataType: "json",
        async: true,
        success: function (data) {
            val = (data.d);
            if (val == "true") {
                $("#txtSourceOfInformation").focus();
            }
            else
                if (val.split('~')[1] != null && val.split('~')[1] != undefined) {
                    $("#hdntxtOthersValue").val(val.split('~')[1]);
                    $("#btnPFSHVerified")[0].disabled = true;
                    DisplayErrorMessage('180005');
                    var v = val.split('~')[2];
                    SetACOFlag(v);
                }
            var bValue = true;
            var PFSHVerified = localStorage.getItem("PFSHVerified");
            if (PFSHVerified != "") {
                var PFSH = PFSHVerified.split('|');
                for (var i = 0; i < PFSH.length; i++) {
                    if (PFSH[i].split('-')[0] == Sessionvalues.split('~')[4].split('&')[0]) {
                        PFSHVerified = PFSHVerified.replace(PFSH[i], Sessionvalues.split('~')[4].split('&')[0] + "-" + "FALSE");
                        bValue = false;
                        break;
                    }
                }
            }
            if (bValue == true)
                PFSHVerified = PFSHVerified + "|" + Sessionvalues.split('~')[4] + "-" + "FALSE";
            localStorage.setItem("PFSHVerified", PFSHVerified);

        },
        error: function OnError(xhr) {
            if (xhr.status == 999)
                window.location = xhr.statusText;
            else {
                var log = JSON.parse(xhr.responseText);
                console.log(log);
                //alert("USER MESSAGE:\n" +
                //                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                //                   "Message: " + log.Message);

                window.location = "ErrorPage.aspx?Message=" + log.Message + "|$|" + log.StackTrace;;

            }
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        }
    });
});
var bValue = true;
function LoadPhysicianList(Control) {
    if ($("#cboPhysician")[0] != undefined && $("#cboPhysician")[0].options.length > 0) {
        PhysicianName = $("#cboPhysician")[0].options[$("#cboPhysician")[0].selectedIndex].text.split('-')[0];
    }
    var request = new XMLHttpRequest();
    request.onreadystatechange = function () {
        if (request.readyState == 4) {
            var xml = request.responseXML;
            var FacilityList = xml.getElementsByTagName("Facility");
            var FacilityRole = window.parent.parent.parent.theForm.ctl00_C5POBody_hdnFacilityRole.value;
            var cookies = document.cookie.split(';');
            var CLegalOrg = "";
            for (var l = 0; l < cookies.length; l++) {
                if (cookies[l].indexOf("CLegalOrg") > -1)
                    CLegalOrg = cookies[l].split("=")[1];
            }
            if (FacilityRole != "") {
                if (FacilityRole.split('&')[1] != "" && (FacilityRole.split('&')[1].toUpperCase() == "PHYSICIAN" || FacilityRole.split('&')[1].toUpperCase() == "PHYSICIAN ASSISTANT")) {
                    $("#lblSelectPhysician")[0].disabled = true;
                    $("#cboPhysician")[0].disabled = true;
                    $("#lblShowAllPhysician")[0].disabled = true;
                    $("#chkShowAllPhysician")[0].disabled = true;
                    $("#cboPhysician").addClass('nonEditabletxtbox');
                    if (FacilityRole.split('&')[0] != "" && FacilityRole.split('&')[2] != "" ) {
                        for (var i = 0; i < FacilityList.length; i++) {
                            if (FacilityList[i].getAttribute("name") == FacilityRole.split('&')[0]) {

                                document.getElementById("cboPhysician").options.length = 0;
                                var PhyEmptyOption = document.createElement("option");
                                PhyEmptyOption.text = "";
                                PhyEmptyOption.value = "0";
                                $("#cboPhysician")[0].options.add(PhyEmptyOption);
                                var PhyList = FacilityList[i].children;
                                for (var j = 0; j < PhyList.length; j++) {
                                    if (PhyList[j].getAttribute("username") != "" && PhyList[j].getAttribute("username").toUpperCase() == FacilityRole.split('&')[2] && PhyList[j].getAttribute("Legal_Org").toUpperCase() == CLegalOrg.toUpperCase()) {
                                        var PhyOption = document.createElement("option");
                                        //Old Code
                                        //var PhyName = PhyList[j].getAttribute("username") + "-" + PhyList[j].getAttribute("prefix") + " " + PhyList[j].getAttribute("firstname") + " " + PhyList[j].getAttribute("middlename") + " " + PhyList[j].getAttribute("lastname") + " " + PhyList[j].getAttribute("suffix");
                                        //PhyOption.text = PhyName;
                                        //Gitlab# 2485 - Physician Name Display Change
                                        var PhyName="";
                                        if (PhyList[j].getAttribute("lastname") != "")
                                            PhyName += PhyList[j].getAttribute("lastname");
                                        if (PhyList[j].getAttribute("firstname") != "") {
                                            if (PhyName != "")
                                                PhyName += "," + PhyList[j].getAttribute("firstname");
                                            else
                                                PhyName += PhyList[j].getAttribute("firstname");
                                        }
                                        if (PhyList[j].getAttribute("middlename") != "")
                                            PhyName += " " + PhyList[j].getAttribute("middlename");
                                        if (PhyList[j].getAttribute("suffix") != "")
                                            PhyName += "," + PhyList[j].getAttribute("suffix");
                                        PhyOption.text = PhyName;
                                        PhyOption.value = PhyList[j].getAttribute("ID");
                                        document.getElementById("cboPhysician").options.add(PhyOption);
                                        break;
                                    }

                                }
                                break;
                            }
                        }
                    }
                }
                else {
                    if (Control != undefined && Control.checked == true) {
                        var chkphysician = new Array();
                        document.getElementById("cboPhysician").options.length = 0;
                        var PhyEmptyOption = document.createElement("option");
                        PhyEmptyOption.text = "";
                        PhyEmptyOption.value = "0";
                        $("#cboPhysician")[0].options.add(PhyEmptyOption);
                        for (var i = 0; i < FacilityList.length; i++) {
                            var PhyList = FacilityList[i].children;
                            for (var j = 0; j < PhyList.length; j++) {
                                if (PhyList[j].getAttribute("username") != "" && CLegalOrg.toUpperCase() == PhyList[j].getAttribute("Legal_Org").toUpperCase()) {
                                    var PhyOption = document.createElement("option");
                                    //Old Code
                                    //var PhyName = PhyList[j].getAttribute("username") + "-" + PhyList[j].getAttribute("prefix") + " " + PhyList[j].getAttribute("firstname") + " " + PhyList[j].getAttribute("middlename") + " " + PhyList[j].getAttribute("lastname") + " " + PhyList[j].getAttribute("suffix");
                                    //PhyOption.text = PhyName;
                                    //Gitlab# 2485 - Physician Name Display Change
                                    var PhyName = "";
                                    if (PhyList[j].getAttribute("lastname") != "")
                                        PhyName += PhyList[j].getAttribute("lastname");
                                    if (PhyList[j].getAttribute("firstname") != "") {
                                        if (PhyName != "")
                                            PhyName += "," + PhyList[j].getAttribute("firstname");
                                        else
                                            PhyName += PhyList[j].getAttribute("firstname");
                                    }
                                    if (PhyList[j].getAttribute("middlename") != "")
                                        PhyName += " " + PhyList[j].getAttribute("middlename");
                                    if (PhyList[j].getAttribute("suffix") != "")
                                        PhyName += "," + PhyList[j].getAttribute("suffix");
                                    PhyOption.text = PhyName;
                                    PhyOption.value = PhyList[j].getAttribute("ID");
                                    if (chkphysician.indexOf(PhyName.toUpperCase()) <= -1) {
                                        chkphysician.push(PhyName.toUpperCase());
                                        document.getElementById("cboPhysician").options.add(PhyOption);
                                    }
                                }
                            }

                        }
                    }
                    else {
                        var FacilityRole = window.parent.parent.parent.theForm.ctl00_C5POBody_hdnFacilityRole.value;
                        if (FacilityRole.split('&')[0] != "") {
                            for (var i = 0; i < FacilityList.length; i++) {
                                if (FacilityList[i].getAttribute("name") == FacilityRole.split('&')[0]) {
                                    document.getElementById("cboPhysician").options.length = 0;
                                    var PhyEmptyOption = document.createElement("option");
                                    PhyEmptyOption.text = "";
                                    PhyEmptyOption.value = "0";
                                    $("#cboPhysician")[0].options.add(PhyEmptyOption);
                                    var PhyList = FacilityList[i].children;
                                    for (var j = 0; j < PhyList.length; j++) {
                                        if (PhyList[j].getAttribute("username") != "" && CLegalOrg.toUpperCase() == PhyList[j].getAttribute("Legal_Org").toUpperCase()) {
                                            var PhyOption = document.createElement("option");
                                            //Old Code
                                            //var PhyName = PhyList[j].getAttribute("username") + "-" + PhyList[j].getAttribute("prefix") + " " + PhyList[j].getAttribute("firstname") + " " + PhyList[j].getAttribute("middlename") + " " + PhyList[j].getAttribute("lastname") + " " + PhyList[j].getAttribute("suffix");
                                            //PhyOption.text = PhyName;
                                            //Gitlab# 2485 - Physician Name Display Change
                                            var PhyName = "";
                                            if (PhyList[j].getAttribute("lastname") != "")
                                                PhyName += PhyList[j].getAttribute("lastname");
                                            if (PhyList[j].getAttribute("firstname") != "") {
                                                if (PhyName != "")
                                                    PhyName += "," + PhyList[j].getAttribute("firstname");
                                                else
                                                    PhyName += PhyList[j].getAttribute("firstname");
                                            }
                                            if (PhyList[j].getAttribute("middlename") != "")
                                                PhyName += " " + PhyList[j].getAttribute("middlename");
                                            if (PhyList[j].getAttribute("suffix") != "")
                                                PhyName += "," + PhyList[j].getAttribute("suffix");
                                            PhyOption.text = PhyName;
                                            PhyOption.value = PhyList[j].getAttribute("ID");
                                            document.getElementById("cboPhysician").options.add(PhyOption);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if ($("#cboPhysician")[0].disabled == true)
                PhysicianName = FacilityRole.split('&')[3].toUpperCase();
            if (PhysicianName != "") {
                $('#cboPhysician option').each(function () {
                    //Old Code
                    //if (this.text.indexOf(PhysicianName) > -1) {
                    //    option = this;
                    //    option.selected = true;
                    //}
                    //Gitlab# 2485 - Physician Name Display Change
                    if (this.value.indexOf(PhysicianName) > -1) {
                        option = this;
                        option.selected = true;
                    }
                });
            }
            if (FacilityRole.split('&')[1] != "" && (FacilityRole.split('&')[1].toUpperCase() == "PHYSICIAN" || FacilityRole.split('&')[1].toUpperCase() == "PHYSICIAN ASSISTANT")) {
                if ($("#cboPhysician")[0].value == "0" || ($("#cboPhysician")[0].selectedOptions.length > 0 && $("#cboPhysician")[0].selectedOptions[0].text == "")) {
                    document.getElementById("cboPhysician").options.length = 0;
                    $("#chkShowAllPhysician")[0].checked = true;
                    var PhyEmptyOption = document.createElement("option");
                    PhyEmptyOption.text = "";
                    PhyEmptyOption.value = "0";
                    $("#cboPhysician")[0].options.add(PhyEmptyOption);
                    for (var i = 0; i < FacilityList.length; i++) {
                        var PhyList = FacilityList[i].children;
                        for (var j = 0; j < PhyList.length; j++) {
                            if (PhyList[j].getAttribute("username") != "" && CLegalOrg.toUpperCase() == PhyList[j].getAttribute("Legal_Org").toUpperCase()) {
                                var PhyOption = document.createElement("option");
                                //Old Code
                                //var PhyName = PhyList[j].getAttribute("username") + "-" + PhyList[j].getAttribute("prefix") + " " + PhyList[j].getAttribute("firstname") + " " + PhyList[j].getAttribute("middlename") + " " + PhyList[j].getAttribute("lastname") + " " + PhyList[j].getAttribute("suffix");
                                //PhyOption.text = PhyName;
                                //Gitlab# 2485 - Physician Name Display Change
                                var PhyName = "";
                                if (PhyList[j].getAttribute("lastname") != "")
                                    PhyName += PhyList[j].getAttribute("lastname");
                                if (PhyList[j].getAttribute("firstname") != "") {
                                    if (PhyName != "")
                                        PhyName += "," + PhyList[j].getAttribute("firstname");
                                    else
                                        PhyName += PhyList[j].getAttribute("firstname");
                                }
                                if (PhyList[j].getAttribute("middlename") != "")
                                    PhyName += " " + PhyList[j].getAttribute("middlename");
                                if (PhyList[j].getAttribute("suffix") != "")
                                    PhyName += "," + PhyList[j].getAttribute("suffix");
                                PhyOption.text = PhyName;
                                PhyOption.value = PhyList[j].getAttribute("ID");
                                document.getElementById("cboPhysician").options.add(PhyOption);
                            }
                        }
                    }
                }
                if ($("#cboPhysician")[0].disabled == true)
                    PhysicianName = FacilityRole.split('&')[3].toUpperCase();
                if (PhysicianName != "") {
                    $('#cboPhysician option').each(function () {
                        //Old Code
                        //if (this.text.indexOf(PhysicianName) > -1) {
                        //    option = this;
                        //    option.selected = true;
                        //}
                        //Gitlab# 2485 - Physician Name Display Change
                        if (this.value.indexOf(PhysicianName) > -1) {
                            option = this;
                            option.selected = true;
                        }
                    });
                }
            }
        }
    };
    request.open("GET", "ConfigXML/PhysicianFacilityMapping.xml", false);
    request.send();
    var sel = $('#cboPhysician');
    var selected = sel.val(); // cache selected value, before reordering
    var opts_list = sel.find('option');

    opts_list.sort(function (a, b) { return a.text == b.text ? 0 : a.text < b.text ? -1 : 1; });
    sel.html('').append(opts_list);
    sel.val(selected);

}
function PhysicianSelected(control) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    if (bValue == true)
        $("#cboPhysician option[value='0']").remove();
    LoadPFSHTab(control);
    bValue = false;
}
function LoadPFSHTab(control) {
    var PhysicianName = "";
    var PhysicianID = "";
    if ($("#cboPhysician")[0] != undefined && $("#cboPhysician")[0].options.length > 0) {
        PhysicianName = $("#cboPhysician")[0].options[$("#cboPhysician")[0].selectedIndex].text.split('-')[0];
        PhysicianID = $("#cboPhysician")[0].options[$("#cboPhysician")[0].selectedIndex].value;
    }
    var value = window.location.search.toString();
    var openingFrom = "";
    var HumanID = "";
    if (value != "") {
        HumanID = value.split('&')[0].split('=')[1];
        openingFrom = value.split('&')[1].split('=')[1];
    }
    if (openingFrom == "Menu") {
        $("#lblSelectPhysician").show();
        $("#cboPhysician").show();
        $("#chkShowAllPhysician").show();
        $("#lblShowAllPhysician").show();
        $("#lblSourceOfInfo").hide();
        $("#txtSourceOfInformation").hide();
        $("#cboSourceOfInformation").hide();
        $("#btnPFSHVerified").hide();
    }
    else {
        $("#lblSelectPhysician").hide();
        $("#cboPhysician").hide();
        $("#chkShowAllPhysician").hide();
        $("#lblShowAllPhysician").hide();
        openingFrom = "Queue";
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    }
    if (PhysicianName != "" || openingFrom == "Queue") {
        var cboSourceOfInformation = $("#cboSourceOfInformation")[0];
        if (cboSourceOfInformation != null && cboSourceOfInformation != undefined) {
            if (cboSourceOfInformation.value != "Other" && cboSourceOfInformation.value != "Others") {
                document.getElementById("txtSourceOfInformation").style.visibility = 'hidden';
                document.getElementById("txtSourceOfInformation").value = "";
            }
        }
        if ($("ul#myTabs li.active").length != 0) {
            if (localStorage.getItem("bSave") == "true") {
                var target = $('#myTabs a:first').tab('show');
                localStorage.setItem("PrevSubTab", target[0].innerText);
                paneID = $(target).attr('href');

            }
            else {
                if (localStorage.getItem("PrevSubTab") == "Past Medical History") {
                    var target = $('#myTabs a:first').tab('show');
                    localStorage.setItem("PrevSubTab", target[0].innerText);
                    paneID = $(target).attr('href');
                    localStorage.setItem("bSave", "true");
                }
                else if (localStorage.getItem("PrevSubTab") == "Surg./Proc.") {
                    var target = $('#myTabs li:eq(1) a').tab('show')
                    localStorage.setItem("PrevSubTab", target[0].innerText);
                    paneID = $(target).attr('href');
                    localStorage.setItem("bSave", "true");
                }
                else if (localStorage.getItem("PrevSubTab") == "Hospitalization History") {
                    var target = $('#myTabs li:eq(2) a').tab('show')
                    localStorage.setItem("PrevSubTab", target[0].innerText);
                    paneID = $(target).attr('href');
                    localStorage.setItem("bSave", "true");
                }
                else if (localStorage.getItem("PrevSubTab") == "Family History") {
                    var target = $('#myTabs li:eq(3) a').tab('show')
                    localStorage.setItem("PrevSubTab", target[0].innerText);
                    paneID = $(target).attr('href');
                    localStorage.setItem("bSave", "true");
                }
                else if (localStorage.getItem("PrevSubTab") == "Social History") {
                    var target = $('#myTabs li:eq(4) a').tab('show')
                    localStorage.setItem("PrevSubTab", target[0].innerText);
                    paneID = $(target).attr('href');
                    localStorage.setItem("bSave", "true");
                }

                //else if (localStorage.getItem("PrevSubTab") == "Rx History") {
                //    var target = $('#myTabs li:eq(5) a').tab('show')
                //    localStorage.setItem("PrevSubTab", target[0].innerText);
                //    paneID = $(target).attr('href');
                //    localStorage.setItem("bSave", "true");
                //}
                else if (localStorage.getItem("PrevSubTab") == "Non Drug Allergy") {
                    var target = $('#myTabs li:eq(5) a').tab('show')
                    localStorage.setItem("PrevSubTab", target[0].innerText);
                    paneID = $(target).attr('href');
                    localStorage.setItem("bSave", "true");
                }
                //else if (localStorage.getItem("PrevSubTab") == "Drug Allergy") {
                //    var target = $('#myTabs li:eq(7) a').tab('show')
                //    localStorage.setItem("PrevSubTab", target[0].innerText);
                //    paneID = $(target).attr('href');
                //    localStorage.setItem("bSave", "true");
                //}
                else if (localStorage.getItem("PrevSubTab") == "Immunization History") {
                    var target = $('#myTabs li:eq(6) a').tab('show')
                    localStorage.setItem("PrevSubTab", target[0].innerText);
                    paneID = $(target).attr('href');
                    localStorage.setItem("bSave", "true");
                }
                else if (localStorage.getItem("PrevSubTab") == "AD") {
                    var target = $('#myTabs li:eq(7) a').tab('show')
                    localStorage.setItem("PrevSubTab", target[0].innerText);
                    paneID = $(target).attr('href');
                    localStorage.setItem("bSave", "true");
                }
                else {
                    var target = $('#myTabs a:first').tab('show');
                    localStorage.setItem("PrevSubTab", target[0].innerText);
                    localStorage.setItem("bSave", "true");
                    paneID = $(target).attr('href');
                }
            }
            if (PhysicianName != "" || openingFrom == "Queue") {
                $.ajax({
                    type: "POST",
                    url: "WebServices/PastMedicalHistoryService.asmx/LoadPFSHtab",
                    contentType: "application/json;charset=utf-8",
                    data: JSON.stringify({
                        "HumanID": HumanID,
                        "OpeningFrom": openingFrom,
                        "PhysicianName": PhysicianName,
                        "PhysicianID": PhysicianID
                    }),
                    dataType: "json",
                    async: true,
                    success: function (data) {
                        Sessionvalues = (data.d);
                        src = $(paneID).attr('data-src') + "?" + (data.d);
                        $(paneID + " iframe").attr("src", src);
                        if (Sessionvalues.split('~')[1] != "")
                            $("#cboSourceOfInformation").val(Sessionvalues.split('~')[1]);
                        if ($("#cboSourceOfInformation :selected").text() == "Other") {
                            $("#hdntxtOthersValue").val(Sessionvalues.split('~')[3]);
                            document.getElementById("txtSourceOfInformation").value = Sessionvalues.split('~')[3];
                            document.getElementById("txtSourceOfInformation").style.visibility = 'visible';
                        }
                        else
                            $("#hdntxtOthersValue").val("");
                        var PFSHVerified = localStorage.getItem("PFSHVerified");
                        var Verified = "";
                        if (PFSHVerified != "") {
                            var PFSH = PFSHVerified.split('|');
                            for (var i = 0; i < PFSH.length; i++) {
                                if (PFSH[i].split('-')[0] == Sessionvalues.split('~')[4].split('&')[0]) {
                                    Verified = PFSH[i].split('-')[1];
                                    break;
                                }
                            }
                        }
                        if (Verified == "" || Verified == "TRUE")
                            $("#btnPFSHVerified")[0].disabled = false;
                        else
                            $("#btnPFSHVerified")[0].disabled = true;
                        if (Sessionvalues.split('~')[2] == "Y" && Verified == "")
                            $("#btnPFSHVerified")[0].disabled = true;
                        if ($("#txtSourceOfInformation").css("visibility") == "hidden")
                            $("#txtSourceOfInformation").val($("#hdntxtOthersValue").value);
                    },

                    error: function OnError(xhr) {
                        if (xhr.status == 999)
                            window.location = xhr.statusText;
                        else {
                            var log = JSON.parse(xhr.responseText);
                            console.log(log);
                            //alert("USER MESSAGE:\n" +
                            //        ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                            //       "Message: " + log.Message);

                            window.location = "ErrorPage.aspx?Message=" + log.Message + "|$|" + log.StackTrace;;

                        }
                        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    }
                });
            }

        }
    }
}
function EnablePFSH() {
    if ($('#btnPFSHVerified') != null)
        $('#btnPFSHVerified')[0].disabled = false;
    if ($("#cboSourceOfInformation option:selected").text() == "Other" || $("#cboSourceOfInformation option:selected").text() == "Others") {
        document.getElementById("txtSourceOfInformation").style.visibility = 'visible';
    }
    else {
        document.getElementById("txtSourceOfInformation").style.visibility = 'hidden';
    }
    var bValue = true;
    var PFSHVerified = localStorage.getItem("PFSHVerified");
    if (PFSHVerified!=null && PFSHVerified!=undefined && PFSHVerified != "") {
        var PFSH = PFSHVerified.split('|');
        for (var i = 0; i < PFSH.length; i++) {
            if (PFSH[i].split('-')[0] == Sessionvalues.split('~')[4].split('&')[0]) {
                PFSHVerified = PFSHVerified.replace(PFSH[i], Sessionvalues.split('~')[4].split('&')[0] + "-" + "TRUE");
                bValue = false;
                break;
            }
        }
    }
    if (bValue == true)
        PFSHVerified = PFSHVerified + "|" + Sessionvalues.split('~')[4].split('&')[0] + "-" + "TRUE";
    localStorage.setItem("PFSHVerified", PFSHVerified);
}

function CloseWnd() {

    PrevTab = $($(top.window.document).find('iframe')[0].contentDocument).find("li.active a");  // previous tab
    var myPos, atPos;
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value == "true") {
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        event.preventDefault();
        sessionStorage.setItem("AutoSave_PFSHClose", "false");
        if (document.title == 'PFSH' && document.URL.indexOf('openingfrom=Menu') > -1) {
            $(window.document).find('body').append('<div id="dvdialogMenu" style="min-height: 65px !important; width: auto; max-height: none; height: auto; display: none;">' +
                '<p style="font-family: Verdana,Arial,sans-serif; font-size: 13.5px;">There are unsaved changes.Do you want to save the them?</p></div>');
            dvdialog = $('#dvdialogMenu');
            myPos = "center center";
            atPos = 'center center';
        }
        $(dvdialog).dialog({
            modal: true,
            title: "Capella -EHR",
            position: {
                my: myPos,
                at: atPos

            },
            buttons: {
                "Yes": function () {

                    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
                    $(dvdialog).dialog("close");
                    $(dvdialog).remove();
                    sessionStorage.setItem("AutoSave_PFSHClose", "true");
                    if (PrevTab[0].innerText == "Past Medical History") {
                        var prevTabtxt = $(PrevTab).text();
                        sessionStorage.setItem('PFSH_PrevTabText', prevTabtxt);
                        $('.clsIframe').contents()[0].all.namedItem('btnSave').click();
                    }
                    else if (PrevTab[0].innerText == "Surg./Proc.") {
                        var prevTabtxt = $(PrevTab).text();
                        sessionStorage.setItem('PFSH_PrevTabText', prevTabtxt);
                        $('.clsIframe').contents()[1].all.namedItem('btnAdd').click();
                    }
                    else if (PrevTab[0].innerText == "Hospitalization History") {
                        var prevTabtxt = $(PrevTab).text();
                        sessionStorage.setItem('PFSH_PrevTabText', prevTabtxt);
                        $('.clsIframe').contents()[2].all.namedItem('btnAdd').click();
                    }
                    else if (PrevTab[0].innerText == "Family History") {
                        var prevTabtxt = $(PrevTab).text();
                        sessionStorage.setItem('PFSH_PrevTabText', prevTabtxt);
                        $('.clsIframe').contents()[3].all.namedItem('btnSave').click();
                    }
                    else if (PrevTab[0].innerText == "Social History") {
                        var prevTabtxt = $(PrevTab).text();
                        sessionStorage.setItem('PFSH_PrevTabText', prevTabtxt);
                        $('.clsIframe').contents()[4].all.namedItem('btnSave').click();
                    }
                    //else if (PrevTab[0].innerText == "Rx History") {
                    //    var prevTabtxt = $(PrevTab).text();
                    //    sessionStorage.setItem('PFSH_PrevTabText', prevTabtxt);
                    //    $('.clsIframe').contents()[5].all.namedItem('btnAdd').click();
                    //}
                    else if (PrevTab[0].innerText == "Non Drug Allergy") {
                        var prevTabtxt = $(PrevTab).text();
                        sessionStorage.setItem('PFSH_PrevTabText', prevTabtxt);
                        $('.clsIframe').contents()[5].all.namedItem('btnSave').click();
                    }
                    //else if (PrevTab[0].innerText == "Drug Allergy") {
                    //    var prevTabtxt = $(PrevTab).text();
                    //    sessionStorage.setItem('PFSH_PrevTabText', prevTabtxt);
                    //    $('.clsIframe').contents()[7].all.namedItem('btnAdd').click();
                    //}
                    else if (PrevTab[0].innerText == "Immunization History") {
                        var prevTabtxt = $(PrevTab).text();
                        sessionStorage.setItem('PFSH_PrevTabText', prevTabtxt);
                        $('.clsIframe').contents()[6].all.namedItem('btnSave').click();
                    }
                    else if (PrevTab[0].innerText == "AD") {
                        var prevTabtxt = $(PrevTab).text();
                        sessionStorage.setItem('PFSH_PrevTabText', prevTabtxt);
                        $('.clsIframe').contents()[7].all.namedItem('btnPFSHAutoSave').click();
                    }
                },
                "No": function () {

                    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                    $(dvdialog).dialog("close");
                    $(dvdialog).remove();
                    self.close();
                },
                "Cancel": function () {

                    sessionStorage.setItem("AutoSave_PFSHClose", "false");
                    $(dvdialog).dialog("close");
                    $(dvdialog).remove();
                    return;
                }
            }
        });
    }
    else {
        if ($(".ui-dialog").is(":visible")) {
            $(dvdialog).dialog("close");
            $(dvdialog).remove();
        }
        self.close();
    }
}
