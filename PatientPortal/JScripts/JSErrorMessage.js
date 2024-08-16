var FromScreen;
var tab_for_blockdays;
var x = 0;
var count = 0;
window.onerror = function (msg, url, lineNo, columnNo, error) {  
    if (msg.indexOf("Transaction XML") > -1) {
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        if (top.window.document.getElementById('ctl00_Loading') != null)
            top.window.document.getElementById('ctl00_Loading').style.display = "none";
        alert(msg.split('$')[1]);
        // DisplayErrorMessage('000037');
    }
    else if (msg.indexOf("To Process is not found") > -1) {//BugID:53884
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        if (top.window.document.getElementById('ctl00_Loading') != null)
            top.window.document.getElementById('ctl00_Loading').style.display = "none";
        if (top.window.document.getElementById('divLoading') != null)
            top.window.document.getElementById('divLoading').style.display = "none";
        DisplayErrorMessage('000022');
    }
    else if(msg.indexOf("Cannot read property 'window' of null") > -1)
    {
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        if (top.window.document.getElementById('ctl00_Loading') != null)
            top.window.document.getElementById('ctl00_Loading').style.display = "none";
        if (top.window.document.getElementById('divLoading') != null)
            top.window.document.getElementById('divLoading').style.display = "none";
        ScriptErrorLogEntry(error.message, lineNo, columnNo, url, error.stack);
    }
    else if (sessionStorage.getItem('StartLoading')=="true") {
       // {
        //var ErrorLogData = {
        //    ErrorMessage: error.message,           
        //    ErrorLineNo: lineNo,
        //    ErrorColumnNo: columnNo,
        //    ErrorUrl: url,
        //    ErrorStack:error.stack
        //}
        ScriptErrorLogEntry(error.message, lineNo, columnNo, url, error.stack);
        //{ sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        //alert("Script Error : " + error.message + "\n Line No : " + lineNo + " Column No : " + columnNo + "\n Url : " + url + "\n Stack Trace : " + error.stack + "\n");
        alert("Something went wrong! \n There seems to be a problem with this page. Please retry and if the problem persists, contact Capella Support with patient and page details.");
        //For Bug Id:56731
    }
}

function DisplayErrorMessage(ErrorNo, NotificationName, Messagelist) {

    var bCheckTrue = localStorage.getItem("ErrorCheck");

    if (bCheckTrue == "true") {
        localStorage.__proto__.localStorageSave = null;
        localStorage.setItem("ErrorCheck", "");
        $(window.top.document).find('#btnErrorOk').off('click');
        $(window.top.document).find('#btnErrorCancel').off('click');
        return true;
    }
    else if (bCheckTrue == "false") {

        localStorage.__proto__.localStorageSave = null;
        localStorage.setItem("ErrorCheck", "");
        $(window.top.document).find('#btnErrorOk').off('click');
        $(window.top.document).find('#btnErrorCancel').off('click');
        return false;
    }
    var msgList = loadXMLDoc('ErrorMessage.xml' + "?version=" + sessionStorage.getItem("ScriptVersion"));//BugID:40306 .to load new xml on version change.
    if (msgList != null) {
        var Msg = msgList[ErrorNo].split('-');
        var Message = Msg[1];
        var ErrType = Msg[0];
        if (Messagelist != undefined) {
            if (Messagelist.length != 0) {
                var Msg = Messagelist.split('-');;
                for (var i = 0; i < Msg.length; i++) {
                    var replaceMsg = '{' + i + '}';
                    var Message = Message.replace(replaceMsg, Msg[i]);

                }
            }
        }
        switch (ErrType) {
            case "OK":
                if ($(window.top.document).find('#divErrorMessage').modal == undefined || $(window.top.document).find('#divErrorMessage').length == 0)
                    alert(Message, "350", "150", "alert");
                else {
                    $(window.top.document).find('#pErrorMsg').html(Message);
                    $(window.top.document).find('#divErrorMessage').modal({ backdrop: 'static', keyboard: false }, 'show');
                    $(window.top.document).find('#btnErrorOk').css("display", "none");
                    $(window.top.document).find('#btnErrorCancel').css("display", "none");
                    $(window.top.document).find('#btnErrorOkCancel').css("display", "");
                }
                break;
            case "OKCANCEL":


                if ($(window.top.document).find('#divErrorMessage').modal == undefined || $(window.top.document).find('#divErrorMessage').length == 0) {
                    if (window.confirm(Message) == true) {
                        var z = 1;
                        return true;
                    }
                    else {
                        var z = 2;
                        return false;
                    }
                }
                else {
                    $(window.top.document).find('#pErrorMsg').html(Message);


                    $(window.top.document).find('#divErrorMessage').modal({ backdrop: 'static', keyboard: false }, 'show');
                    $(window.top.document).find('#btnErrorCancel').css("display", "");
                    $(window.top.document).find('#btnErrorOk').css("display", "");
                    $(window.top.document).find('#btnErrorOkCancel').css("display", "none");
                    localStorage.__proto__.localStorageSave = arguments.callee.caller;

                    $(window.top.document).find('#btnErrorOk').click(function () { localStorage.setItem("ErrorCheck", "true"); if (localStorage.__proto__.localStorageSave != null) { localStorage.localStorageSave() }; $(window.top.document).find('#divErrorMessage').modal('hide'); return false; });


                    $(window.top.document).find('#btnErrorCancel').click(function () { localStorage.setItem("ErrorCheck", "false"); if (localStorage.__proto__.localStorageSave != null) { localStorage.localStorageSave() }; $(window.top.document).find('#divErrorMessage').modal('hide'); return false; });

                }

                break;
            case "ToolStrip":
                // var WindowManager= $find('WindowMngr').GetWindowByName('MessageWindow'); 
                // 
                //var win = radalert(Message,null,null,WindowManager.get_title());
                //win.setSize(250, 100);
                //win.center();
                // 
                //  win.set_iconUrl("Resources/16_16.ico"); 
                //  $telerik.$('.RadWindow .radalert A.rwPopupButton').css('display', 'none'); 
                //  $telerik.$('.RadWindow div.rwDialogPopup.radalert').css('background-image', 'none');  
                //   window.setTimeout(function()
                //    {
                //        win.close();
                //    }, 2000);   
                //var div_element = window.top.document.getElementById('tbGeneral');
                //var warning_message_label = window.top.document.getElementById('ctl00_Warning_Message');
                //if (warning_message_label != undefined && warning_message_label != null) {
                //    warning_message_label.textContent = Message;
                //    warning_message_label.style.display = "inline-block";
                //    warning_message_label.style.color = "red";
                //    warning_message_label.style.position = "absolute";
                //    var curleft = curtop = 0;
                //    if (div_element.offsetParent) {
                //        do {
                //            curleft += div_element.offsetLeft;
                //            curtop += div_element.offsetTop;
                //        } while (div_element = div_element.offsetParent);
                //    }
                //    curtop += 7;
                //    //warning_message_label.style.top = curtop + "px";
                //    warning_message_label.style.top = "35px";
                //    warning_message_label.style.right = "10px";
                //    window.top.setTimeout(function () {
                //        warning_message_label.style.display = "none";
                //    }, 5000);
                //}
                ToolStripAlert(Message); break;
            case "CCDGenerate":
                xmlregenerateToolStripAlert(Message);
                break;
            case "YesNoCancel":
                var obj = new Array();
                FromScreen = NotificationName;
                obj.push("Title=" + "Navigation");
                obj.push("ErrorMessages=" + Message);
                var result = radopen("frmValidationArea.aspx?Title=Message&ErrorMessages=" + Message, "MessageWindow");
                var WindowName = $find('MessageWindow');
                WindowName.setSize(280, 100);
                WindowName.SetModal(true);
                WindowName.set_visibleStatusbar(false);
                WindowName.set_behaviors(Telerik.Web.UI.WindowBehaviors.Close);
                WindowName.set_keepInScreenBounds(true);
                WindowName.set_centerIfModal(true);
                WindowName.center();
                WindowName.set_iconUrl("Resources/16_16.ico");
                $telerik.$('.RadWindow .rwControlButtons')[0].style.display = "none";

                var width = "270px";
                var Height = "85px";

                var iFrames = $telerik.$('.RadWindow .rwWindowContent iframe');

                for (i = 0; i < iFrames.length; i++) {
                    var n = iFrames[i].src.indexOf("frmValidationArea.aspx");
                    if (n >= 0) {
                        iFrames[i].style.height = "85px";
                        iFrames[i].style.width = "270px";
                    } else {
                        iFrames[i].style.height = "100%";
                        iFrames[i].style.width = "100%";
                    }
                }

                WindowName.add_close(OnClientCloseNavigation);
                break;
        }
    }
}

function DisplayErrorMessageList(ErrorNo, Messagelist) {
    var msgList = loadXMLDoc('ErrorMessage.xml' + "?version=" + sessionStorage.getItem("ScriptVersion"));//BugID:40306 .to load new xml on version change.
    if (msgList != null) {
        var Err = msgList[ErrorNo].split('-');
        Errmessage = Err[1];
        var Msg = Messagelist.split('-');;
        for (var i = 0; i < Msg.length; i++) {
            var replaceMsg = '{' + i + '}';
            var Errmessage = Errmessage.replace(replaceMsg, Msg[i]);

        }
        if ($(window.top.document).find('#divErrorMessage').modal == undefined || $(window.top.document).find('#divErrorMessage').length == 0)
            alert(Errmessage, "350", "150", "alert");
        else {
            $(window.top.document).find('#pErrorMsg').html(Errmessage);
            $(window.top.document).find('#divErrorMessage').modal({ backdrop: 'static', keyboard: false }, 'show');
            $(window.top.document).find('#btnErrorOk').css("display", "none");
            $(window.top.document).find('#btnErrorCancel').css("display", "none");
            $(window.top.document).find('#btnErrorOkCancel').css("display", "");
        }
    }
}
function GetClientId(strid) {
    var count = document.forms[0].length; var i = 0; var eleName; for (i = 0; i < count; i++)
    { eleName = document.forms[0].elements[i].id; pos = eleName.indexOf(strid); if (pos >= 0) break; }
    return eleName;
}

function GetLocalTime() {
    var dt = new Date();
    var now = new Date();
    var then = now.getDay() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear(); then += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds();
    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
    return utc;
}


function loadXMLDoc(dname) {

    if (window.XMLHttpRequest) {
        var ErrorMessageTable = new Array();
        xhttp = new XMLHttpRequest();
        xhttp.open("GET", dname, false);
        xhttp.overrideMimeType("text/xml");
        xhttp.send();
        xmlDoc = xhttp.responseXML;
        if (xmlDoc == null) {
            console.log(xhttp.status + xhttp.statusText);
            if (count < 1) {
                count++;
                var ErrMsgTableRetry = loadXMLDoc(dname);
                return ErrMsgTableRetry;
            }
            else {
                count = 0;
                return null;
            }
        }
        else
            count = 0;
        var AllMessages = xmlDoc.getElementsByTagName('Message');
        for (var i = 0; i < AllMessages.length; i++) {
            ErrorMessageTable[AllMessages[i].attributes[0].nodeValue] = AllMessages[i].attributes[1].nodeValue + '-' + AllMessages[i].attributes[2].nodeValue;
        }
        return ErrorMessageTable;
    }
    else {
        var ErrorMessageTable = new Array();
        var xmlDoc = new ActiveXObject("Microsoft.XMLDOM");
        xmlDoc = new ActiveXObject("Microsoft.XMLHTTP");
        xmlDoc.async = "false";
        xmlDoc.load(xmlFile);
        if (xmlDoc != null && xmlDoc != undefined)
            count = 0;
        var xmlObj = xmlDoc.documentElement;
        var XMLList = xmlObj.getElementsByTagName("Message");
        for (var i = 0; i < XMLList.length; i++) {
            ErrorMessageTable[XMLList[i].getAttribute("number")] = XMLList[i].getAttribute("Type") + '-' + XMLList[i].getAttribute("Text");
        }
        return ErrorMessageTable;
    }

}
//Added by manimaran for showing - symbol
function ShowErrorMessageList(ErrorNo, Messagelist) {
    var Err = loadAllXMLDocMessage('ErrorMessage.xml' + "?version=" + sessionStorage.getItem("ScriptVersion"))[ErrorNo].split('|');//BugID:40306 .to load new xml on version change.
    var Errmessage = Err[1];
    var ErrType = Err[0];
    if (Messagelist != undefined) {
        var Msg = Messagelist.split('-');;
        for (var i = 0; i < Msg.length; i++) {
            var replaceMsg = '{' + i + '}';
            var Errmessage = Errmessage.replace(replaceMsg, Msg[i]);

        }
    }
    switch (ErrType) {
        case "OK":
            alert(Errmessage, "350", "150", "alert");
            break;
        case "OKCANCEL":
            if (window.confirm(Errmessage) == true) {
                var x = 1;
                return true;
            }
            else {
                var x = 2;
                return false;
            }
        case "ToolStrip":
            //var warning_message_label = window.top.document.getElementById('lblMessage');
            //if (warning_message_label == null || warning_message_label == undefined) {
            //    var WindowManager = $find('WindowMngr').GetWindowByName('MessageWindow');
            //    var win = radalert(Errmessage, null, null, WindowManager.get_title());
            //    win.setSize(300, 100);
            //    win.center();
            //    win.set_iconUrl("Resources/16_16.ico");
            //    $telerik.$('.RadWindow .radalert A.rwPopupButton').css('display', 'none');
            //    $telerik.$('.RadWindow div.rwDialogPopup.radalert').css('background-image', 'none');
            //    window.setTimeout(function () {
            //        win.close();
            //    }, 2000);
            //    //break;
            //}
            //else {
            //    var warning_message_label = window.top.document.getElementById('lblMessage');
            //    warning_message_label.textContent = Errmessage;
            //    warning_message_label.style.display = "inline-block";
            //    warning_message_label.style.color = "red";
            //    warning_message_label.style.position = "absolute";
            //    warning_message_label.style.top = "30px";
            //    warning_message_label.style.right = "10px";
            //    window.top.setTimeout(function () {
            //        warning_message_label.style.display = "none";
            //    }, 5000);

            //}
            ToolStripAlert(Errmessage); break;
    }
}
function GetClientId(strid) {
    var count = document.forms[0].length; var i = 0; var eleName; for (i = 0; i < count; i++)
    { eleName = document.forms[0].elements[i].id; pos = eleName.indexOf(strid); if (pos >= 0) break; }
    return eleName;
}

function loadAllXMLDocMessage(dname) {
    if (window.XMLHttpRequest) {
        var ErrorMessageTable = new Array();
        xhttp = new XMLHttpRequest();
        xhttp.open("GET", dname, false);
        xhttp.send();
        xmlDoc = xhttp.responseXML;
        var AllMessages = xmlDoc.getElementsByTagName('Message');
        for (var i = 0; i < AllMessages.length; i++) {
            ErrorMessageTable[AllMessages[i].attributes[0].nodeValue] = AllMessages[i].attributes[1].nodeValue + '|' + AllMessages[i].attributes[2].nodeValue;
        }
        return ErrorMessageTable;
    }
    else {
        var ErrorMessageTable = new Array();
        var xmlDoc = new ActiveXObject("Microsoft.XMLDOM");
        xmlDoc = new ActiveXObject("Microsoft.XMLHTTP");
        xmlDoc.async = "false";
        xmlDoc.load(xmlFile);
        var xmlObj = xmlDoc.documentElement;
        var XMLList = xmlObj.getElementsByTagName("Message");
        for (var i = 0; i < XMLList.length; i++) {
            ErrorMessageTable[XMLList[i].getAttribute("number")] = XMLList[i].getAttribute("Type") + '-' + XMLList[i].getAttribute("Text");
        }
        return ErrorMessageTable;
    }

}
function OnClientCloseNavigation(oWindow, args) {
    var arg = args.get_argument();
    if (FromScreen == "Patient Chart Close" || FromScreen == "LogOut") {
        if (arg == "Yes") {
            var currentTab = window.parent.parent.parent.parent.theForm.ctl00_hdnCurrentTab.value;
            if (FromScreen == "Patient Chart Close")
                window.parent.parent.parent.parent.theForm.ctl00_hdnCurrentTab.value += "CloseChart";
            else
                window.parent.parent.parent.parent.theForm.ctl00_hdnCurrentTab.value += "LogOut";
            var IDs = window.top.frames[0].frameElement.contentDocument.getElementById('hdnSaveButtonID').value.split(',');
            if (IDs.length == 1) {
                var save_button;
                save_button = window.top.frames[0].frameElement.contentDocument.getElementById('pageContainer').control.get_selectedPageView().get_element().getElementsByTagName("iframe")[0].contentDocument.getElementById(IDs[0]);
                if (save_button != undefined || save_button != null) {
                    save_button.click();
                    return;
                }
            } else if (IDs.length == 2) {
                var childControlsofParentContainer = window.top.frames[0].frameElement.contentDocument.getElementById('pageContainer').control.get_selectedPageView().get_element().getElementsByTagName("iframe")[0].contentWindow.$telerik.radControls;
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
        }
        else
            if (arg == "No") {
                window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                if (FromScreen == "Patient Chart Close")
                    window.parent.parent.parent.parent.theForm.ctl00_hdnCurrentTab.value += "CloseChart";
                else
                    window.parent.parent.parent.parent.theForm.ctl00_hdnCurrentTab.value += "LogOut";
                document.getElementById('ctl00_Button1').click();
            }
    }
    else if (FromScreen == undefined) {

        var MsgType = document.getElementById(GetClientId("hdnMessageType"));
        if (MsgType != null || MsgType != undefined) {
            MsgType.value = arg;
            oWindow.remove_close(OnClientCloseNavigation);
            oWindow.close();
            document.getElementById(GetClientId("btnMessageType")).click();
        }
        else {
            var MsgType = document.getElementById("hdnMessageType");
            if (MsgType != null || MsgType != undefined) {
                MsgType.value = arg;
                oWindow.remove_close(OnClientCloseNavigation);
                oWindow.close();
                document.getElementById("btnMessageType").click();
            }
        }

    }
    else if (FromScreen == "EncounterTabClick") {
        var tabstrip = window.top.frames[0].frameElement.contentDocument.getElementById('tabStripEncounter').control;
        var TabClick = window.top.frames[0].frameElement.contentDocument.getElementById('hdnTabClick');
        var text = TabClick.value.split('$#$');
        if (text[0] != "first" && text[1] == "") {
            if (arg == "Yes")
                TabClick.value += "second,true";
            else if (arg == "No")
                TabClick.value += "second,false";
            else if (arg == "Cancel")
                TabClick.value += "second,cancel";

            var tab = tabstrip.findTabByText(text[0]);
            if (tab != null)
                tab.click();
        }
    }
    else if (FromScreen == "QuestionnaireTabClick") {
        var tabstrip = $find('RadTabStrip2');
        var TabClick = window.top.frames[0].frameElement.contentDocument.getElementById('hdnTabClick');
        var text = TabClick.value.split('$#$');
        if (text[0] != "first" && text[1] == "") {
            if (arg == "Yes")
                TabClick.value += "second,true";
            else if (arg == "No")
                TabClick.value += "second,false";
            else if (arg == "Cancel")
                TabClick.value += "second,cancel";

            var tab = tabstrip.findTabByText(text[0]);
            if (tab != null)
                tab.click();
        }
        window.top.document.getElementById('ctl00_Loading').style.display = "block";
    }
    else if (FromScreen == "PFSHTabClick") {
        var tabstrip = $find('RadTabStrip1');
        var TabClick = window.top.frames[0].frameElement.contentDocument.getElementById('hdnTabClick');
        var text = TabClick.value.split('$#$');
        if (text[0] != "first" && text[1] == "") {
            if (arg == "Yes")
                TabClick.value += "second,true";
            else if (arg == "No")
                TabClick.value += "second,false";
            else if (arg == "Cancel")
                TabClick.value += "second,cancel";

            var tab = tabstrip.findTabByText(text[0]);
            if (tab != null)
                tab.click();
        }
        window.top.document.getElementById('ctl00_Loading').style.display = "block";
    }
    else if (FromScreen == "AssessmentTabClick") {
        var tabstrip = $find('tbAssessment');
        var TabClick = window.top.frames[0].frameElement.contentDocument.getElementById('hdnTabClick');
        var text = TabClick.value.split('$#$');
        if (text[0] != "first" && text[1] == "") {
            if (arg == "Yes")
                TabClick.value += "second,true";
            else if (arg == "No")
                TabClick.value += "second,false";
            else if (arg == "Cancel")
                TabClick.value += "second,cancel";

            var tab = tabstrip.findTabByText(text[0]);
            if (tab != null)
                tab.click();
        }
        window.top.document.getElementById('ctl00_Loading').style.display = "block";
    }
    else if (FromScreen == "ExamTabClick") {
        var tabstrip = $find('RadTabStrip1');
        var TabClick = window.top.frames[0].frameElement.contentDocument.getElementById('hdnTabClick');
        var text = TabClick.value.split('$#$');
        if (text[0] != "first" && text[1] == "") {
            if (arg == "Yes")
                TabClick.value += "second,true";
            else if (arg == "No")
                TabClick.value += "second,false";
            else if (arg == "Cancel")
                TabClick.value += "second,cancel";

            var tab = tabstrip.findTabByText(text[0]);
            if (tab != null)
                tab.click();
        }
    }
    else if (FromScreen == "OrdersTabClick") {
        var tabstrip = $find('tabOrders');
        if (window.top.frames[0].frameElement.contentDocument.getElementById('hdnTabClick') != null)
            var TabClick = window.top.frames[0].frameElement.contentDocument.getElementById('hdnTabClick');
        else
            var TabClick = window.top.document.getElementById('ctl00_C5POBody_hdnTabClick');
        var text = TabClick.value.split('$#$');
        if (text[0] != "first" && text[1] == "") {
            if (arg == "Yes")
                TabClick.value += "second,true";
            else if (arg == "No")
                TabClick.value += "second,false";
            else if (arg == "Cancel")
                TabClick.value += "second,cancel";

            var tab = tabstrip.findTabByText(text[0]);
            if (tab != null) {
                window.top.document.getElementById('ctl00_Loading').style.display = "block";
                tab.click();

            }
        }
        //window.top.document.getElementById('ctl00_Loading').style.display = "block";
    }
    else if (FromScreen == "PlanTabClick") {
        var tabstrip = $find('tbPlanTab');
        var TabClick = window.top.frames[0].frameElement.contentDocument.getElementById('hdnTabClick');
        var text = TabClick.value.split('$#$');
        if (text[0] != "first" && text[1] == "") {
            if (arg == "Yes")
                TabClick.value += "second,true";
            else if (arg == "No")
                TabClick.value += "second,false";
            else if (arg == "Cancel")
                TabClick.value += "second,cancel";

            var tab = tabstrip.findTabByText(text[0]);
            if (tab != null) {
                window.top.document.getElementById('ctl00_Loading').style.display = "block";
                tab.click();
            }
        }
        //window.top.document.getElementById('ctl00_Loading').style.display = "block";
    }

    else if (FromScreen == "BlockDaysTabClick") {
        var tabstrip = $find('tabBlockDays');
        if (window.parent.document.getElementById('ctl00_hdnTabClick') != null)
            var TabClick = window.parent.document.getElementById('ctl00_hdnTabClick');
        var text = TabClick.value.split('$#$');
        if (text[0] != "first" && text[1] == "") {
            if (arg == "Yes")
                TabClick.value += "second,true";
            else if (arg == "No")
                TabClick.value += "second,false";
            else if (arg == "Cancel")
                TabClick.value += "second,cancel";
            tab_for_blockdays = tabstrip;
            var tab = tabstrip.findTabByText(text[0]);
            if (tab != null)
                tab.click();
        }
    }

    else if (FromScreen == "MoveToButtonsClick") {
        var TabClick = window.top.frames[0].frameElement.contentDocument.getElementById('hdnTabClick');
        var text = TabClick.value.split('$#$');
        if (text[0] != "first" && text[1] == "") {
            if (arg == "Yes")
                TabClick.value += "second,true";
            else if (arg == "No") {
                if (window.top.frames[0].frameElement.contentDocument.getElementById('pageContainer').control.get_selectedPageView()._contentUrl.indexOf('PlanTab') > 0) {
                    if (window.top.frames[0].frameElement.contentDocument.getElementById('pageContainer').control.get_selectedPageView().get_element().getElementsByTagName("iframe")[0].contentDocument.getElementById('rdmpPlanTab').control.get_selectedPageView()._contentUrl.indexOf('frmGeneralPlanNew') > 0)
                        TabClick.value += "second,cancel";
                    else
                        TabClick.value += "second,false";
                }
                else
                    TabClick.value += "second,false";
            }
            else if (arg == "Cancel")
                TabClick.value += "second,cancel";

            var button = $find(text[0]);
            if (button != null)
                button.click(true);
        }
    }
    else if (FromScreen == "PatientChartTreeViewNodeClick") {
        var treeview = document.getElementById('ctl00_C5POBody_trvPatinetChart').control;
        var TabClick = window.top.frames[0].frameElement.contentDocument.getElementById('hdnTabClick');
        var text = TabClick.value.split('$#$');
        if (text[0] != "first" && text[1] == "") {
            if (arg == "Yes")
                TabClick.value += "second,true";
            else if (arg == "No")
                TabClick.value += "second,false";
            else if (arg == "Cancel")
                TabClick.value += "second,cancel";

            var node = treeview.findNodeByText(text[0]);
            if (node != null) {
                $(node._element).click();
                if (arg == "No")
                    node.select();
            }
        }
    }
    // The following block of code must always be executed only at the end of this method.
    // If statements need to be added, kindly include them before this code block.-Pujhitha
    if (arg == "Cancel") {
        window.top.document.getElementById('ctl00_Loading').style.display = "none";
    }

}
function SavedSuccessfully_NowProceed(screen) {
    if ((window.parent.parent.parent.parent.theForm.ctl00_hdnCurrentTab.value.indexOf("CloseChart") > -1) || (window.parent.parent.parent.parent.theForm.ctl00_hdnCurrentTab.value.indexOf("LogOut") > -1)) {
        window.parent.parent.parent.parent.theForm.ctl00_Button1.click();
        return;
    }
    if (screen == "EncounterTabClick") {
        if (window.top.frames[0].frameElement.contentDocument.getElementById('tabStripEncounter') != null || window.top.frames[0].frameElement.contentDocument.getElementById('tabStripEncounter') != undefined) {
            var tabstrip = window.top.frames[0].frameElement.contentDocument.getElementById('tabStripEncounter').control;
            var TabClick = window.top.frames[0].frameElement.contentDocument.getElementById('hdnTabClick');
            var text = TabClick.value.split('$#$');
            if (text[1] == "third") {
                var tab = tabstrip.findTabByText(text[0]);
                if (tab != null)
                    tab.click();
            }
        }
    }
    else if (screen == "QuestionnaireTabClick") {
        var tabstrip = window.parent.theForm.ownerDocument.getElementById('RadTabStrip2').control;
        var TabClick = window.top.frames[0].frameElement.contentDocument.getElementById('hdnTabClick');
        var text = TabClick.value.split('$#$');
        if (text[1] == "third") {
            var tab = tabstrip.findTabByText(text[0]);
            if (tab != null)
                tab.click();
        }
    }
    else if (screen == "AssessmentTabClick") {
        var tabstrip = window.parent.theForm.ownerDocument.getElementById('tbAssessment').control;
        var TabClick = window.top.frames[0].frameElement.contentDocument.getElementById('hdnTabClick');
        var text = TabClick.value.split('$#$');
        if (text[1] == "third") {
            var tab = tabstrip.findTabByText(text[0]);
            if (tab != null)
                tab.click();
        }
    }
    else if (screen == "PFSHTabClick") {
        var tabstrip = window.parent.theForm.ownerDocument.getElementById('RadTabStrip1').control;
        var TabClick = window.top.frames[0].frameElement.contentDocument.getElementById('hdnTabClick');
        var text = TabClick.value.split('$#$');
        if (text[1] == "third") {
            var tab = tabstrip.findTabByText(text[0]);
            if (tab != null)
                tab.click();
        }
    }
    else if (screen == "ExamTabClick") {
        var tabstrip = window.parent.theForm.ownerDocument.getElementById('RadTabStrip1').control;
        var TabClick = window.top.frames[0].frameElement.contentDocument.getElementById('hdnTabClick');
        var text = TabClick.value.split('$#$');
        if (text[1] == "third") {
            var tab = tabstrip.findTabByText(text[0]);
            if (tab != null)
                tab.click();
        }
    }
    else if (screen == "OrdersTabClick") {
        var tabstrip = window.parent.theForm.ownerDocument.getElementById('tabOrders').control;
        if (window.top.frames[0].frameElement.contentDocument.getElementById('hdnTabClick') != null)
            var TabClick = window.top.frames[0].frameElement.contentDocument.getElementById('hdnTabClick');
        else
            var TabClick = window.top.document.getElementById('ctl00_C5POBody_hdnTabClick');
        var text = TabClick.value.split('$#$');
        if (text[1] == "third") {
            var tab = tabstrip.findTabByText(text[0]);
            if (tab != null)
                tab.click();
        }
    }
    else if (screen == "PlanTabClick") {
        var tabstrip = window.parent.theForm.ownerDocument.getElementById('tbPlanTab').control;
        var TabClick = window.top.frames[0].frameElement.contentDocument.getElementById('hdnTabClick');
        var text = TabClick.value.split('$#$');
        if (text[1] == "third") {
            var tab = tabstrip.findTabByText(text[0]);
            if (tab != null)
                tab.click();
        }
    }

    else if (screen == "BlockDaysTabClick") {
        var tabstrip = tab_for_blockdays;
        if (window.parent.document.getElementById('ctl00_hdnTabClick') != null)
            var TabClick = window.parent.document.getElementById('ctl00_hdnTabClick');
        var text = TabClick.value.split('$#$');
        if (text[1] == "third") {
            var tab = tabstrip.findTabByText(text[0]);
            if (tab != null)
                tab.click();
            TabClick.value = "first";
        }
    }
    else if (screen == "MoveToButtonsClick") {
        var TabClick = window.top.frames[0].frameElement.contentDocument.getElementById('hdnTabClick');
        var text = TabClick.value.split('$#$');
        if (text[1] == "third") {
            if (window.parent.document.getElementById(text[0]) != undefined && window.parent.document.getElementById(text[0]) != null)
                var button = window.parent.document.getElementById(text[0]).control;
            else
                var button = window.parent.parent.document.getElementById(text[0]).control;
            if (button != null)
                button.click(true);
        }
    }
    else if (screen == "PatientChartTreeViewNodeClick") {
        var treeview = top.document.getElementById('ctl00_C5POBody_trvPatinetChart').control;
        var TabClick = window.top.frames[0].frameElement.contentDocument.getElementById('hdnTabClick');
        var text = TabClick.value.split('$#$');
        if (text[1] == "third") {
            if (treeview != undefined && treeview != null)
                var node = treeview.findNodeByText(text[0]);
            if (node != null) {
                $(node._element).click();
                node.select();
            }
        }
    }
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
    else if (window.parent.theForm.hdnIsSaveEnable != null && window.parent.theForm.hdnIsSaveEnable != undefined) {
        window.parent.theForm.hdnIsSaveEnable.value = "false";
    }
    else if (window.parent.parent.theForm.hdnIsSaveEnable != null && window.parent.parent.theForm.hdnIsSaveEnable != undefined) {
        window.parent.parent.theForm.hdnIsSaveEnable.value = "false";
    }
}
function SaveUnsuccessful() {
    //window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
    //localStorage.setItem("bSave", "false");

    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null || window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
    localStorage.setItem("bSave", "false");
    Exam_AfterAutoSave();
}
function Exam_AfterAutoSave() {
    if (JSON.parse(sessionStorage.getItem("AutoSave_Exam")) == true) {
        if (localStorage.getItem("bSave") == "true") {
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
            paneID = sessionStorage.getItem('Exam_paneID');
            src = sessionStorage.getItem('Exam_src');
            if ($(window.parent.document).find(paneID + " iframe").attr("src") == "") {
                $(window.parent.document).find(paneID + " iframe").attr("src", src);
            }
        }
        else {
            //bOrderCancel = true;
            sessionStorage.setItem('bOrderCancel', 'true');
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
            //PrevTab.tab('show');
            $(window.parent.document).find('#btnHiddenExam')[0].click();
        }
        sessionStorage.setItem("AutoSave_Exam", "false");
    }
    if (JSON.parse(sessionStorage.getItem("EncAutoSave")) == true)
        Encounter_AfterAutoSave();
}

function DisplayErrorMessageCopyPrevious(ErrorNo, NotificationName, Messagelist) {

    var bCheckTrue = localStorage.getItem("ErrorCheck");

    if (bCheckTrue == "true") {
        localStorage.__proto__.localStorageSave = null;
        localStorage.setItem("ErrorCheck", "");
        $(window.top.document).find('#btnErrorOk').off('click');
        $(window.top.document).find('#btnErrorCancel').off('click');
        return true;
    }
    else if (bCheckTrue == "false") {

        localStorage.__proto__.localStorageSave = null;
        localStorage.setItem("ErrorCheck", "");
        $(window.top.document).find('#btnErrorOk').off('click');
        $(window.top.document).find('#btnErrorCancel').off('click');
        return false;
    }


    var Msg = loadXMLDoc1('ErrorMessage.xml' + "?version=" + sessionStorage.getItem("ScriptVersion"))[ErrorNo].split('^');//BugID:40306 .to load new xml on version change.
    var Message = Msg[1];
    var ErrType = Msg[0];
    if (Messagelist != undefined) {
        if (Messagelist.length != 0) {
            var Msg = Messagelist.split('^');;
            for (var i = 0; i < Msg.length; i++) {
                var replaceMsg = '{' + i + '}';
                var Message = Message.replace(replaceMsg, Msg[i]);

            }
        }
    }

    switch (ErrType) {
        case "OK":
            if ($(window.top.document).find('#divErrorMessage').modal == undefined || $(window.top.document).find('#divErrorMessage').length == 0)
                alert(Message, "350", "150", "alert");
            else {
                $(window.top.document).find('#pErrorMsg').html(Message);
                $(window.top.document).find('#divErrorMessage').modal({ backdrop: 'static', keyboard: false }, 'show');
                $(window.top.document).find('#btnErrorOk').css("display", "none");
                $(window.top.document).find('#btnErrorCancel').css("display", "none");
                $(window.top.document).find('#btnErrorOkCancel').css("display", "");
            }
            break;
        case "OKCANCEL":


            if ($(window.top.document).find('#divErrorMessage').modal == undefined || $(window.top.document).find('#divErrorMessage').length == 0) {
                if (window.confirm(Message) == true) {
                    var z = 1;
                    return true;
                }
                else {
                    var z = 2;
                    return false;
                }
            }
            else {
                $(window.top.document).find('#pErrorMsg').html(Message);


                $(window.top.document).find('#divErrorMessage').modal({ backdrop: 'static', keyboard: false }, 'show');
                $(window.top.document).find('#btnErrorCancel').css("display", "");
                $(window.top.document).find('#btnErrorOk').css("display", "");
                $(window.top.document).find('#btnErrorOkCancel').css("display", "none");
                localStorage.__proto__.localStorageSave = arguments.callee.caller;

                $(window.top.document).find('#btnErrorOk').click(function () { localStorage.setItem("ErrorCheck", "true"); if (localStorage.__proto__.localStorageSave != null) { localStorage.localStorageSave() }; $(window.top.document).find('#divErrorMessage').modal('hide'); return false; });


                $(window.top.document).find('#btnErrorCancel').click(function () { localStorage.setItem("ErrorCheck", "false"); if (localStorage.__proto__.localStorageSave != null) { localStorage.localStorageSave() }; $(window.top.document).find('#divErrorMessage').modal('hide'); return false; });

            }

            break;
        case "ToolStrip":
            // var WindowManager= $find('WindowMngr').GetWindowByName('MessageWindow'); 
            // 
            //var win = radalert(Message,null,null,WindowManager.get_title());
            //win.setSize(250, 100);
            //win.center();
            // 
            //  win.set_iconUrl("Resources/16_16.ico"); 
            //  $telerik.$('.RadWindow .radalert A.rwPopupButton').css('display', 'none'); 
            //  $telerik.$('.RadWindow div.rwDialogPopup.radalert').css('background-image', 'none');  
            //   window.setTimeout(function()
            //    {
            //        win.close();
            //    }, 2000);   
            var div_element = window.top.document.getElementById('tbGeneral');
            var warning_message_label = window.top.document.getElementById('ctl00_Warning_Message');
            warning_message_label.textContent = Message;
            warning_message_label.style.display = "inline-block";
            warning_message_label.style.color = "red";
            warning_message_label.style.position = "absolute";
            var curleft = curtop = 0;
            if (div_element.offsetParent) {
                do {
                    curleft += div_element.offsetLeft;
                    curtop += div_element.offsetTop;
                } while (div_element = div_element.offsetParent);
            }
            curtop += 7;
            //warning_message_label.style.top = curtop + "px";
            warning_message_label.style.top = "35px";
            warning_message_label.style.right = "10px";
            window.top.setTimeout(function () {
                warning_message_label.style.display = "none";
            }, 5000);
            break;
        case "YesNoCancel":
            var obj = new Array();
            FromScreen = NotificationName;
            obj.push("Title=" + "Navigation");
            obj.push("ErrorMessages=" + Message);
            var result = radopen("frmValidationArea.aspx?Title=Message&ErrorMessages=" + Message, "MessageWindow");
            var WindowName = $find('MessageWindow');
            WindowName.setSize(280, 100);
            WindowName.SetModal(true);
            WindowName.set_visibleStatusbar(false);
            WindowName.set_behaviors(Telerik.Web.UI.WindowBehaviors.Close);
            WindowName.set_keepInScreenBounds(true);
            WindowName.set_centerIfModal(true);
            WindowName.center();
            WindowName.set_iconUrl("Resources/16_16.ico");
            $telerik.$('.RadWindow .rwControlButtons')[0].style.display = "none";

            var width = "270px";
            var Height = "85px";

            var iFrames = $telerik.$('.RadWindow .rwWindowContent iframe');

            for (i = 0; i < iFrames.length; i++) {
                var n = iFrames[i].src.indexOf("frmValidationArea.aspx");
                if (n >= 0) {
                    iFrames[i].style.height = "85px";
                    iFrames[i].style.width = "270px";
                } else {
                    iFrames[i].style.height = "100%";
                    iFrames[i].style.width = "100%";
                }
            }

            WindowName.add_close(OnClientCloseNavigation);
            break;
    }
}

function loadXMLDoc1(dname) {
    if (window.XMLHttpRequest) {
        var ErrorMessageTable = new Array();
        xhttp = new XMLHttpRequest();
        xhttp.open("GET", dname, false);
        xhttp.send();
        xmlDoc = xhttp.responseXML;
        var AllMessages = xmlDoc.getElementsByTagName('Message');
        for (var i = 0; i < AllMessages.length; i++) {
            ErrorMessageTable[AllMessages[i].attributes[0].nodeValue] = AllMessages[i].attributes[1].nodeValue + '^' + AllMessages[i].attributes[2].nodeValue;
        }
        return ErrorMessageTable;
    }
    else {
        var ErrorMessageTable = new Array();
        var xmlDoc = new ActiveXObject("Microsoft.XMLDOM");
        xmlDoc = new ActiveXObject("Microsoft.XMLHTTP");
        xmlDoc.async = "false";
        xmlDoc.load(xmlFile);
        var xmlObj = xmlDoc.documentElement;
        var XMLList = xmlObj.getElementsByTagName("Message");
        for (var i = 0; i < XMLList.length; i++) {
            ErrorMessageTable[XMLList[i].getAttribute("number")] = XMLList[i].getAttribute("Type") + '^' + XMLList[i].getAttribute("Text");
        }
        return ErrorMessageTable;
    }

}

function StartLoadingImage() {
    if (jQuery('body').find('#resultLoading').attr('id') != 'resultLoading') {
        jQuery('body').append('<div id="resultLoading" class="masterLoad" style="display:none"><div><img src="./Resources/loadimage.gif" style="opacity:1;height:30px;width:30px;"><div style="font-size:16px;padding-top:5px;padding-left:15px;">Loading...</div></div><div class="bg"></div></div>');
    }
    else {
        jQuery('body').find('#resultLoading').remove();
        jQuery('body').append('<div id="resultLoading" class="masterLoad" style="display:none"><div><img src="./Resources/loadimage.gif" style="opacity:1;height:30px;width:30px;"><div style="font-size:16px;padding-top:5px;padding-left:15px;">Loading...</div></div><div class="bg"></div></div>');
    }
    jQuery('#resultLoading').css({
        'width': '100%',
        'height': '100%',
        'position': 'fixed',
        'z-index': '10000000',
        'top': '0',
        'left': '0',
        'right': '0',
        'bottom': '0',
        'margin': 'auto'
    });
    jQuery('#resultLoading .bg').css({
        'background': '#ffffff',
        'opacity': '0.7',
        'width': '100%',
        'height': '100%',
        'position': 'absolute',
        'top': '0'
    });
    jQuery('#resultLoading>div:first').css({
        'width': '250px',
        'height': '75px',
        'text-align': 'center',
        'position': 'fixed',
        'top': '0',
        'left': '0',
        'right': '0',
        'bottom': '0',
        'margin': 'auto',
        'font-size': '16px',
        'z-index': '10',
        'color': '#000000'
    });
    jQuery('#resultLoading .bg').height('100%');
    jQuery('#resultLoading').fadeIn(300);
    jQuery('body').css('cursor', 'wait');
}
function StopLoadingImage() {
    jQuery('#resultLoading .bg').height('100%');
    jQuery('#resultLoading').fadeOut(300);
    jQuery('body').css('cursor', 'default');
    if (jQuery('#resultLoading').css('display') == 'block')
        jQuery('#resultLoading').remove();
    //    jQuery('.masterLoad .bg').height('100%');
    //    jQuery('.masterLoad').fadeOut(300);
    //    jQuery('body').css('cursor', 'default');
}
function StartLoadFromPatChart() {
    if (sessionStorage.getItem('StartLoading') == 'true') {
        if (jQuery(window.top.parent.parent.parent.parent.document.body).find('#resultLoading').attr('id') != 'resultLoading') {
            jQuery(window.top.parent.parent.parent.parent.document.body).append('<div id="resultLoading" class="masterLoad" style="display:none"><div><img src="./Resources/loadimage.gif" style="opacity:0.7;height:30px;width:30px;"><div style="font-size:16px;padding-top:5px;padding-left:15px;">Loading...</div></div><div class="bg"></div></div>');
        }
        else {
            jQuery(window.top.parent.parent.parent.parent.document.body).find('#resultLoading').remove();
            jQuery(window.top.parent.parent.parent.parent.document.body).append('<div id="resultLoading" class="masterLoad" style="display:none"><div><img src="./Resources/loadimage.gif" style="opacity:0.7;height:30px;width:30px;"><div style="font-size:16px;padding-top:5px;padding-left:15px;">Loading...</div></div><div class="bg"></div></div>');
        }
        jQuery(window.top.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css({
            'display': 'block',
            'width': '100%',
            'height': '100%',
            'position': 'fixed',
            'z-index': '10000000',
            'top': '0',
            'left': '0',
            'right': '0',
            'bottom': '0',
            'margin': 'auto'
        });
        jQuery(window.top.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading .bg').css({
            'background': '#ffffff',
            'opacity': '0.7',
            'width': '100%',
            'height': '100%',
            'position': 'absolute',
            'top': '0'
        });
        jQuery(window.top.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading>div:first').css({
            'width': '250px',
            'height': '75px',
            'text-align': 'center',
            'position': 'fixed',
            'top': '0',
            'left': '0',
            'right': '0',
            'bottom': '0',
            'margin': 'auto',
            'font-size': '16px',
            'z-index': '10',
            'color': '#000000'
        });
        jQuery(window.top.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading .bg').height('100%');
        jQuery(window.top.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').fadeIn(300);
        jQuery(window.top.parent.parent.parent.parent.document.body).css('cursor', 'wait');
    }
}
function StopLoadFromPatChart() {
    jQuery(window.top.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading .bg').height('100%');
    jQuery(window.top.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').fadeOut(300);
    jQuery(window.top.parent.parent.parent.parent.document.body).css('cursor', 'default');
    if (jQuery(window.top.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block')
        jQuery(window.top.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').remove();
    //jQuery('.masterLoad .bg').height('100%');
    //jQuery('.masterLoad').fadeOut(300);
    //jQuery(window.top.parent.parent.parent.parent.document.body).css('cursor', 'default');
}


function RefreshNotification(Value) {
    
    debugger;
    LoadNotification(Value);
    //var sOldText = null;
    //if (window.top.document.getElementById("dvnotify") != null)
    //    sOldText = window.top.document.getElementById("dvnotify").innerHTML.replace(/"/g, "Quotes");
    //else {
    //    if (window.top.document.getElementById("ctl00_C5POBody_dvnotify") != null)
    //        sOldText = window.top.document.getElementById("ctl00_C5POBody_dvnotify").innerHTML.replace(/"/g, "Quotes");
    //}
    //jQuery(window.top.parent.document).find('#divLoadingnotify').css('display', "block");
    //$.ajax({
    //    type: "POST",
    //    url: "frmRCopiaToolbar.aspx/LoadNotification",
    //    data: '{ScreenName: "' + Value + '",NotifyOldText: "' + sOldText + '"}',
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    success: function (data) {
    //        var regex = /<BR\s*[\/]?>/gi;
    //        debugger;

    //        if (window.top.document.getElementById("dvnotify") != null)
    //            window.top.document.getElementById("dvnotify").innerHTML = data.d;
    //        else {
    //            if (window.top.document.getElementById("ctl00_C5POBody_dvnotify") != null)
    //                window.top.document.getElementById("ctl00_C5POBody_dvnotify").innerHTML = data.d;
    //        }
    //        if (jQuery(window.top.parent.document).find('#divLoadingnotify') != null)
    //            jQuery(window.top.parent.document).find('#divLoadingnotify').css('display', "none");
    //    },
    //    failure: function (response) {
    //        if (jQuery(window.top.parent.document).find('#divLoadingnotify') != null)
    //            jQuery(window.top.parent.document).find('#divLoadingnotify').css('display', "none");
    //        alert(response.d);
    //    }
    //});


}
function PFSH_AfterAutoSave() {
    if (JSON.parse(sessionStorage.getItem("AutoSave_PFSHClose")) == true && localStorage.getItem("bSave") == "true") {
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        $($(window.top.document).find('iframe')[0].contentDocument).find('#btnClose')[0].click();
    }
    sessionStorage.setItem("AutoSave_PFSHClose", "false");
    if (JSON.parse(sessionStorage.getItem("AutoSave_PFSH")) == true) {
        if (localStorage.getItem("bSave") == "true") {
            paneID = sessionStorage.getItem('PFSH_paneID');
            src = sessionStorage.getItem('PFSH_src');
            $(window.parent.document).find(paneID + " iframe").attr("src", src);
        }
        else {
            //bPFSHCancel = true;
            sessionStorage.setItem('bPFSHCancel', 'true');
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
            //var PrevTabtxt = sessionStorage.getItem('PFSH_PrevTabText');
            //$(window.parent.document).find("a:contains('" + PrevTabtxt + "')").tab('show');
            $(window.parent.document).find('#btnHiddenPFSH')[0].click();
        }
        sessionStorage.setItem("AutoSave_PFSH", "false");
    }
    if (JSON.parse(sessionStorage.getItem("EncAutoSave")) == true)
        Encounter_AfterAutoSave();
}
function PFSH_SaveUnsuccessful() {
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
    localStorage.setItem("bSave", "false");
    PFSH_AfterAutoSave();
}

function Order_AfterAutoSave() {
    //if (sessionStorage.getItem("autoMovetonxtProc") != undefined) {
    //    //$($($(window.top.document).context.aspnetForm.children[0])[0].children[0].children[0].children[1].children[1].children[0].contentDocument).find('#btnOrderSubmit')[0].enabled == false
    //    if (JSON.parse(sessionStorage.getItem("autoMovetonxtProc")) == true)
    //    {
    //        sessionStorage.removeItem("autoMovetonxtProc");
    //        $(window.top.document).find('iframe')[0].contentDocument.all.namedItem('btnMoveToNextProcess').click();
    //    }
    //}
    //else 
    if (JSON.parse(sessionStorage.getItem("AutoSave_OrderMenu")) == true && localStorage.getItem("bSave") == "true") {
        if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null || window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        if ($($(window.top.document).find('iframe')[0].contentDocument).find('#btnClose') != null && $($(window.top.document).find('iframe')[0].contentDocument).find('#btnClose') != undefined && $($(window.top.document).find('iframe')[0].contentDocument).find('#btnClose').length > 0)
            $($(window.top.document).find('iframe')[0].contentDocument).find('#btnClose')[0].click();
        sessionStorage.setItem("AutoSave_OrderMenu", "false");
    }
    if (JSON.parse(sessionStorage.getItem("AutoSave_Order")) == true) {
        if (localStorage.getItem("bSave") == "true") {
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
            paneID = sessionStorage.getItem('Order_paneID');
            src = sessionStorage.getItem('Order_src');
            if ($(window.parent.document).find(paneID + " iframe").attr("src") == "") {
                $(window.parent.document).find(paneID + " iframe").attr("src", src);
            }
        }
        else {
            //bOrderCancel = true;
            sessionStorage.setItem('bOrderCancel', 'true');
            if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null || window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
                window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
            //PrevTab.tab('show');
            if ($(window.parent.document).find('#btnHiddenOrder') != null && $(window.parent.document).find('#btnHiddenOrder') != undefined && $(window.parent.document).find('#btnHiddenOrder').length>0)
                $(window.parent.document).find('#btnHiddenOrder')[0].click();
        }
        sessionStorage.setItem("AutoSave_Order", "false");
    }
    if (JSON.parse(sessionStorage.getItem("EncAutoSave")) == true)
        Encounter_AfterAutoSave();
}
function Order_SaveUnsuccessful() {
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null || window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
    localStorage.setItem("bSave", "false");
    Order_AfterAutoSave();
}

function AutoSaveUnsuccessful() {
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null || window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
    localStorage.setItem("bSave", "false");
    Encounter_AfterAutoSave();
}
function AutoSaveSuccessful() {
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    localStorage.setItem("bSave", "true");
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
    Encounter_AfterAutoSave();
}
function Encounter_AfterAutoSave() {
    if (JSON.parse(sessionStorage.getItem("EncAutoSave")) == true) {
        sessionStorage.removeItem("EncAutoSave");
        if (localStorage.getItem("bSave") == "true" && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value == "false") {
            paneID = sessionStorage.getItem("Enc_PaneId");
            src = sessionStorage.getItem("Enc_Src");
            //$($(top.window.document).find('.jqx-widget-content.jqx-splitter-panel.jqx-reset>iframe')[0].contentDocument).find(paneID + " iframe").attr("src", src);//BugID:47428..
            $($(top.window.document).find('#jqxSplitter').find('iframe')[0].contentDocument).find(paneID + " iframe").attr("src", src);
        }
        else {
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
            sessionStorage.setItem("EncCancel", "true");
            sessionStorage.setItem("Encounter_PrevTabRevert", "true");
            //$($(top.window.document).find('.jqx-widget-content.jqx-splitter-panel.jqx-reset>iframe')[0].contentDocument).find('#btnHiddenTab')[0].click();//BugID:47428..
            $($(top.window.document).find('#jqxSplitter').find('iframe')[0].contentDocument).find('#btnHiddenTab')[0].click();
            return;
        }
    }
}
//BugID:45541
function insertTab(textbox, event) {
    if (textbox.value.indexOf('??') != -1) {
        var keyChar = event.keyCode ? event.keyCode : event.charCode ? event.charCode : event.which;
        if (keyChar == 9 && !event.shiftKey && !event.ctrlKey && !event.altKey) {
            var oS = textbox.scrollTop;
            if (textbox.setSelectionRange) {

                var sStart = textbox.selectionStart;
                var sEnd = textbox.selectionEnd;
                var sString = textbox.value;
                sStart = sString.indexOf('??');
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
            return false;
        }
    }
    return true;
}

function Notification_Popup(triggeredBy) {
    debugger;
    if (window.top.document.getElementById("notificationpopup").innerText != "NOTIFICATION : Loading...") {
        if ($(top.window.document).find("#AlertInfo") != undefined) {
            var locatn = "HtmlNotification.html?version=" + sessionStorage.getItem("ScriptVersion");
            $(top.window.document).find('#Alert_Modal')[0].contentDocument.location.href = locatn;
            if (triggeredBy != undefined && triggeredBy != '' && triggeredBy == "MovetoNextProcess") {
                $(top.window.document).find("#AlertHeader")[0].innerHTML = "Warning";
                $(top.window.document).find("#btnCloseAlert").css("display", "none");
                localStorage.setItem('trigerredBy', triggeredBy);
            }
            else {
                $(top.window.document).find("#AlertHeader")[0].innerHTML = "Notification";
                $(top.window.document).find("#btnCloseAlert").css("display", "block");
            }
            $(top.window.document).find("#AlertInfo")[0].style.display = "block";
        }
    }
}

function CloseAlertModal() {
    $("#AlertInfo")[0].style.display = "none";
}

function LoadNotification(Value) {
    debugger;
   
    //if ((window.location.href.indexOf("frmAppointments.aspx") == -1 && window.location.href.indexOf("frmMyQueueNew.aspx") == -1) && (window.parent == null || (window.parent.location.href.indexOf("frmAppointments.aspx") == -1 && window.parent.location.href.indexOf("frmMyQueueNew.aspx") == -1))) {
    if ((window.location.href.indexOf("frmAppointments.aspx") == -1) && (window.parent == null || (window.parent.location.href.indexOf("frmAppointments.aspx") == -1))) {
        if (window.parent.location.href.indexOf("frmMyQueueNew.aspx") == -1 || window.location.href.indexOf("frmMyQueueNew.aspx") == -1) {
            $(top.window.document).find("#divNotifyPullUp")[0].style.display = "block";//BugID:48010
            $(top.window.document).find("#divNotify")[0].style.display = "block";
            if ($('#notificationpopup')[0] != null) {
                $('#notificationpopup')[0].innerText = "NOTIFICATION : Loading...";
            }
            else {
                if (window.top.document.getElementById("notificationpopup") != null) {
                    window.top.document.getElementById("notificationpopup").innerText = "NOTIFICATION : Loading...";
                }
            }
        }
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        $.ajax({
            type: "POST",
            async: true,
            url: "frmRCopiaToolbar.aspx/LoadNotification",
            data: '{ScreenName: "' + Value + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                debugger;
                if (data != "") {
                    var regex = /<BR\s*[\/]?>/gi;
                    var NotificationText = data.d.split("~~~")[0];
                    if (NotificationText.indexOf('MANDATORY') > -1)
                        sessionStorage.setItem("mandatoryPresent", "true");
                    else
                        sessionStorage.setItem("mandatoryPresent", "false");
                    var showOnlyMandatory = false;
                    if (document.getElementById("divnotificationPopup") != null) {
                        document.getElementById("divnotificationPopup").style.display = "block";
                        if (localStorage.getItem('trigerredBy') != undefined && localStorage.getItem('trigerredBy') == 'MovetoNextProcess') {
                            if (NotificationText.indexOf('MANDATORY') > -1) {
                                document.getElementById("Notification_Warning_For_Mandatory").style.display = "block";
                                localStorage.removeItem('trigerredBy');
                                showOnlyMandatory = true;
                            }
                            else {
                                document.getElementById("Notification_Warning").style.display = "block";
                                localStorage.removeItem('trigerredBy');
                            }
                        }
                        var content = NotificationText.split('~@ ');
                        var InnerContent = null;
                        for (var i = 0; i < content.length - 1; i++) {
                            var NotificationInnerText = content[i].split('~~RESOLVE~~')[0];
                            var NotificationResolveText = content[i].split('~~RESOLVE~~')[1];
                            if (InnerContent == null) {

                                if (content[i].toUpperCase().indexOf("MACRA") > -1 && !showOnlyMandatory) {
                                    if (NotificationResolveText != null && NotificationResolveText != "")
                                        InnerContent = "<div class='alert' style='color: #4e349e;background-color: #eadff5;border-color: #cac2d0;font-size: 12.5px;font-family: Microsoft Sans Serif;' id='" + i + "'> " + NotificationInnerText + "<br/><a style='font-weight: bold;cursor: pointer;' onclick=\"movetotab('" + NotificationResolveText + "');\"> Click here to Resolve </a></div>";
                                    else
                                        InnerContent = "<div class='alert' style='color: #4e349e;background-color: #eadff5;border-color: #cac2d0;font-size: 12.5px;font-family: Microsoft Sans Serif;' id='" + i + "'> " + NotificationInnerText + "<br/></div>";

                                }
                                else if (content[i].toUpperCase().indexOf("STAGE III") > -1 && !showOnlyMandatory) {
                                    if (NotificationResolveText != null && NotificationResolveText != "")
                                        InnerContent = "<div class='alert alert-success fade in' style='font-size: 12.5px;font-family: Microsoft Sans Serif;' id='" + i + "'> " + NotificationInnerText + "<br/><a style='font-weight: bold;cursor: pointer;' onclick=\"movetotab('" + NotificationResolveText + "');\"> Click here to Resolve </a></div>";
                                    else
                                        InnerContent = "<div class='alert alert-success fade in' style='font-size: 12.5px;font-family: Microsoft Sans Serif;' id='" + i + "'> " + NotificationInnerText + "<br/></div>";
                                }
                                else if (content[i].toUpperCase().indexOf("STAGE II") > -1 && !showOnlyMandatory) {
                                    if (NotificationResolveText != null && NotificationResolveText != "")
                                        InnerContent = "<div class='alert alert-info fade in' style='font-size: 12.5px;font-family: Microsoft Sans Serif;' id='" + i + "'> " + NotificationInnerText + "<br/><a style='font-weight: bold;cursor: pointer;' onclick=\"movetotab('" + NotificationResolveText + "');\"> Click here to Resolve </a></div>";
                                    else
                                        InnerContent = "<div class='alert alert-info fade in' style='font-size: 12.5px;font-family: Microsoft Sans Serif;' id='" + i + "'> " + NotificationInnerText + "<br/></div>";
                                }
                                else {
                                    if (content[i].toUpperCase().indexOf("MANDATORY") > -1) {
                                        if (NotificationResolveText != null && NotificationResolveText != "")
                                            InnerContent = "<div class='alert alert-danger' style='font-size: 12.5px;font-family: Microsoft Sans Serif;' id='" + i + "'> " + NotificationInnerText + "<br/><a style='font-weight: bold;cursor: pointer;' onclick=\"movetotab('" + NotificationResolveText + "');\"> Click here to Resolve </a></div>";
                                        else
                                            InnerContent = "<div class='alert alert-danger' style='font-size: 12.5px;font-family: Microsoft Sans Serif;' id='" + i + "'> " + NotificationInnerText + "<br/></div>";
                                    }
                                }
                            }
                            else {

                                if (content[i].toUpperCase().indexOf("MACRA") > -1 && !showOnlyMandatory) {
                                    if (NotificationResolveText != null && NotificationResolveText != "")
                                        InnerContent = InnerContent + "<div class='alert' style='color: #4e349e;background-color: #eadff5;border-color: #cac2d0;font-size: 12.5px;font-family: Microsoft Sans Serif;' id='" + i + "'> " + NotificationInnerText + "<br/><a style='font-weight: bold;cursor: pointer;' onclick=\"movetotab('" + NotificationResolveText + "');\"> Click here to Resolve </a></div>";
                                    else
                                        InnerContent = InnerContent + "<div class='alert' style='color: #4e349e;background-color: #eadff5;border-color: #cac2d0;font-size: 12.5px;font-family: Microsoft Sans Serif;' id='" + i + "'> " + NotificationInnerText + "<br/></div>";

                                }
                                else if (content[i].toUpperCase().indexOf("STAGE III") > -1 && !showOnlyMandatory) {
                                    if (NotificationResolveText != null && NotificationResolveText != "")
                                        InnerContent = InnerContent + "<div class='alert alert-success fade in' style='font-size: 12.5px;font-family: Microsoft Sans Serif;' id='" + i + "'> " + NotificationInnerText + "<br/><a style='font-weight: bold;cursor: pointer;' onclick=\"movetotab('" + NotificationResolveText + "');\"> Click here to Resolve </a></div>";
                                    else
                                        InnerContent = InnerContent + "<div class='alert alert-success fade in' style='font-size: 12.5px;font-family: Microsoft Sans Serif;' id='" + i + "'> " + NotificationInnerText + "<br/></div>";
                                }
                                else if (content[i].toUpperCase().indexOf("STAGE II") > -1 && !showOnlyMandatory) {
                                    if (NotificationResolveText != null && NotificationResolveText != "")
                                        InnerContent = InnerContent + "<div class='alert alert-info fade in' style='font-size: 12.5px;font-family: Microsoft Sans Serif;' id='" + i + "'> " + NotificationInnerText + "<br/><a style='font-weight: bold;cursor: pointer;' onclick=\"movetotab('" + NotificationResolveText + "');\"> Click here to Resolve </a></div>";
                                    else
                                        InnerContent = InnerContent + "<div class='alert alert-info fade in' style='font-size: 12.5px;font-family: Microsoft Sans Serif;' id='" + i + "'> " + NotificationInnerText + "<br/></div>";
                                }
                                else {
                                    if (content[i].toUpperCase().indexOf("MANDATORY") > -1) {
                                        if (NotificationResolveText != null && NotificationResolveText != "")
                                            InnerContent = InnerContent + "<div class='alert alert-danger' style='font-size: 12.5px;font-family: Microsoft Sans Serif;' id='" + i + "'> " + NotificationInnerText + "<br/><a style='font-weight: bold;cursor: pointer;' onclick=\"movetotab('" + NotificationResolveText + "');\"> Click here to Resolve </a></div>";
                                        else
                                            InnerContent = InnerContent + "<div class='alert alert-danger' style='font-size: 12.5px;font-family: Microsoft Sans Serif;' id='" + i + "'> " + NotificationInnerText + "<br/></div>";
                                    }
                                }
                            }
                        }
                        document.getElementById("divnotificationPopup").innerHTML = InnerContent;
                    }

                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    var NotificationCount = 0;
                    if (data.d.split("~~~")[1].split("~*~")[0] != undefined && data.d.split("~~~")[1].split("~*~")[0].trim != "") {
                        NotificationCount = data.d.split("~~~")[1].split("~*~")[0];
                        //$(top.window.document).find("#NotificationCountText")[0].innerText = NotificationCount;
                        var Notification_Content = NotificationText.split('~@ ');
                        var divNotify = $(top.window.document).find("#innerContent");
                        if (divNotify != null && divNotify[0].children.length > 0)
                            $(divNotify[0].children).remove();//BugID:48470
                        var NotificationScreens = ""; var Macra_Notify_present = false;
                        if (divNotify != undefined) {
                            for (var i = 0; i < Notification_Content.length - 1; i++) {
                                if (Notification_Content[i].toUpperCase().indexOf("MACRA") > -1) {
                                    var Notification_Text = Notification_Content[i].split('~~RESOLVE~~')[0].split('- MACRA MEASURE')[0].split('<u><strong>')[1];
                                    var Notification_Screen = Notification_Content[i].split('~~RESOLVE~~')[1];
                                    NotificationScreens += "*" + Notification_Screen + "*";
                                    var pText = "<p class='pContentClickable pContent' data-src='" + Notification_Screen + "'>" + Notification_Text + "</p>";
                                    divNotify.append(pText);
                                    Macra_Notify_present = true;
                                }
                            }
                            var hiddenNotificScreens = "<p class='NotificScreens' style='display:none;'>" + NotificationScreens + "</p>";
                            divNotify.append(hiddenNotificScreens);
                            if (!Macra_Notify_present) {
                                var pText = "<p class='pContentClickable pContent fontstyle'> MACRA requirements satisfied! </p>";//BugID:48465
                                divNotify.append(pText);
                            }
                        }
                    }
                    if (window.parent.location.href.indexOf("frmMyQueueNew.aspx") == -1 || window.location.href.indexOf("frmMyQueueNew.aspx") == -1) {
                        if ($('#notificationpopup')[0] != null) {
                            $('#notificationpopup')[0].innerText = "NOTIFICATION : " + NotificationCount;
                        }
                        else {
                            if (window.top.document.getElementById("notificationpopup") != null) {
                                window.top.document.getElementById("notificationpopup").innerText = "NOTIFICATION : " + NotificationCount;
                            }
                        }
                    }
                    //var summaryNotification = $($($(window.top.document).find('iframe[id=ctl00_C5POBody_EncounterContainer]')[0].contentDocument).find('iframe')[0].contentDocument).find('div[id=dvnotifySummary]')[0];
                    //var summaryNotification = $($(window.top.document).find('iframe[id=ctl00_C5POBody_EncounterContainer]')[0].contentDocument).find('iframe')[0];
                    //if (summaryNotification != null && summaryNotification != undefined) {
                    //    summaryNotification = $($($(window.top.document).find('iframe[id=ctl00_C5POBody_EncounterContainer]')[0].contentDocument).find('iframe')[0].contentDocument).find('div[id=dvnotifySummary]')[0];
                    //    if (data.d.split("~~~")[1].split("~*~")[1]!=null)
                    //        summaryNotification.innerHTML = data.d.split("~~~")[1].split("~*~")[1]+"</ul>";
                    //}
                }
            },
            error: function OnError(xhr) {
                debugger;
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                if (xhr.status == 999)
                    window.location = "/frmSessionExpired.aspx";
                else {
                    var log = JSON.parse(xhr.responseText);
                    console.log(log);
                    alert("USER MESSAGE:\n" + xhr.status + "-" + xhr.statusText +
                        ". \nCannot process request. Please Login again and retry. If issue persists, Please contact Support.\n\nEXCEPTION DETAILS: \nException Type" +
                        log.ExceptionType + " \nMessage: " + log.Message);
                }
            }
        });
    }
}


function CloseNotification() {
    debugger;
    if (document.getElementsByClassName('alert alert-danger') != null && document.getElementsByClassName('alert alert-danger').length > 0) {
        //DisplayErrorMessage('180056');
    }
    else {
        $(top.window.document).find('#btnCloseAlert').click();
        sessionStorage.setItem('CloseNotification', 'true');
        //$($(top.window.document).find('iframe[id=ctl00_C5POBody_EncounterContainer]')[0].contentDocument).find("#hdnNotificationOkClicked")[0].value = "true";//BugID:47780
        //sessionStorage.setItem("triggeredfromOKClick", "true");//BUgID:47761
        //$($(top.window.document).find('iframe[id=ctl00_C5POBody_EncounterContainer]')[0].contentDocument).find('#btnMove').click();
    }
}

function movetotab(ResolveScreen) {
    debugger;
    if (ResolveScreen == "PFSH-SOCIAL_HISTORY") {
        $($(window.top.document).find('iframe[id=ctl00_C5POBody_EncounterContainer]')[0].contentDocument).find("ul li a")[0].click();
        $($(window.top.document).find('iframe[id=ctl00_C5POBody_EncounterContainer]')[0].contentDocument).find("ul li a")[2].click();
        localStorage.setItem("notification", "Social")
    }
    else if (ResolveScreen == "VITALS")
        $($(window.top.document).find('iframe[id=ctl00_C5POBody_EncounterContainer]')[0].contentDocument).find("ul li a")[4].click();
    else if (ResolveScreen == "ASSESSMENT")
        $($(window.top.document).find('iframe[id=ctl00_C5POBody_EncounterContainer]')[0].contentDocument).find("ul li a")[7].click();
    else if (ResolveScreen == "ORDER") {
        $($(window.top.document).find('iframe[id=ctl00_C5POBody_EncounterContainer]')[0].contentDocument).find("ul li a")[0].click();
        $($(window.top.document).find('iframe[id=ctl00_C5POBody_EncounterContainer]')[0].contentDocument).find("ul li a")[8].click();
    }
    else if (ResolveScreen == "ORDER-IMMUNIZATION") {
        $($(window.top.document).find('iframe[id=ctl00_C5POBody_EncounterContainer]')[0].contentDocument).find("ul li a")[0].click();
        $($(window.top.document).find('iframe[id=ctl00_C5POBody_EncounterContainer]')[0].contentDocument).find("ul li a")[8].click();
        localStorage.setItem("notification", "Immunizationorder");
    }
    else if (ResolveScreen == "ERX")
        $($(window.top.document).find('iframe[id=ctl00_C5POBody_EncounterContainer]')[0].contentDocument).find("ul li a")[9].click();
    else if (ResolveScreen == "SERVICE_AND_PROCEDURE_CODE")
        $($(window.top.document).find('iframe[id=ctl00_C5POBody_EncounterContainer]')[0].contentDocument).find("ul li a")[11].click();
    else if (ResolveScreen == "PLAN") {
        $($(window.top.document).find('iframe[id=ctl00_C5POBody_EncounterContainer]')[0].contentDocument).find("ul li a")[0].click();
        $($(window.top.document).find('iframe[id=ctl00_C5POBody_EncounterContainer]')[0].contentDocument).find("ul li a")[10].click();
    }
    else if (ResolveScreen == "PLAN-CAREPLAN") {
        $($(window.top.document).find('iframe[id=ctl00_C5POBody_EncounterContainer]')[0].contentDocument).find("ul li a")[0].click();
        $($(window.top.document).find('iframe[id=ctl00_C5POBody_EncounterContainer]')[0].contentDocument).find("ul li a")[10].click();
        localStorage.setItem("notification", "CarePlan")
    }
    else if (ResolveScreen == "DEMOGRAPHICS") {
        $(top.window.document).find("#ctl00_mnuPatient_smnuDemographics a").click();

    }
    $(top.window.document).find("#AlertInfo")[0].style.display = "none";
}

function CloseMeasureModal() {
    $("#MeasureInfo")[0].style.display = "none";
}
function OpenMeasurePopup(Source) {
    debugger;
    if ($(top.window.document).find("#MeasureInfo") != undefined) {
        var locatn = "htmlMACRA_Measures.html?Source=" + Source + "&version=" + sessionStorage.getItem("ScriptVersion");
        $(top.window.document).find('#Measure_Modal')[0].contentDocument.location.href = locatn;
        $(top.window.document).find("#modalHeader")[0].innerHTML = "Measure Calculator Guidance";
        $(top.window.document).find("#MeasureInfo")[0].style.display = "block";
    }
}
//BugID:48010
function PullUpNotification() {
    //$(top.window.document).find('#divNotify').css({ display: 'block', transition: 'opacity 10s ease-in', opacity: '0.9' });
    //$(top.window.document).find('#newwlwm').addClass('in-view');
    //$(top.window.document).find('#divNotifyPullUp').css("display", "none");
    //setTimeout(HideNotify, 5000);
    if ($(top.window.document).find('#divNotify').hasClass('notify-shown')) {
        $(top.window.document).find('#newwlwm').removeClass('in-view');
        $(top.window.document).find('#newwlwm').removeClass('position-set');//BugID:48506
        //var height = 0;
        //$(top.window.document).find('#newwlwm').css("margin-top", height);
        $(top.window.document).find('#divNotify').removeClass('notify-shown');
    }
    else {
        //var height = 0;
        //if (screen.availHeight >= 860 && screen.availHeight < 984) {
        //    height = $(top.window.document).find('#newwlwm').height() + 110;
        //}
        //height = $(top.window.document).find('#newwlwm').height() + 30;
        $(top.window.document).find('#newwlwm').addClass("position-set");//BugID:48506
        $(top.window.document).find('#newwlwm').addClass('in-view');
        $(top.window.document).find('#divNotify').addClass('notify-shown');
        setTimeout(HideNotify, 5000);
    }
}
function HideNotify() {
    $(top.window.document).find('#newwlwm').removeClass('in-view');
    $(top.window.document).find('#newwlwm').removeClass('position-set');//BugID:48506
    //var height = 0;
    //$(top.window.document).find('#newwlwm').css("margin-top", height);
    if ($(top.window.document).find('#divNotify').hasClass('notify-shown'))
        $(top.window.document).find('#divNotify').removeClass('notify-shown');
    //$(top.window.document).find('#divNotify').css({ display: 'none', transition: 'opacity 10s ease-out', opacity: '0' });
    //$(top.window.document).find('#divNotifyPullUp').css("display", "block");
}

function OpenNotificationPopUp(screen) {
    debugger;
    var NotificScreens = $(top.window.document).find("#innerContent .NotificScreens");
    if (NotificScreens != null && NotificScreens != undefined && NotificScreens.length > 0) {
        var screenSearchText = "*" + screen.toUpperCase() + "*";
        if ($(NotificScreens)[0].innerText != undefined && $(NotificScreens)[0].innerText.indexOf(screenSearchText) != -1) {
            PullUpNotification();
        }
    }
}
//BugID:49685
function CreateAuditLogEntryForTransactions(TransactionType, EntityName, HumanID) {
    var WSData = {
        Transaction_Type: TransactionType,
        Entity_Name: EntityName,
        Human_ID: HumanID
    }
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "./frmFindPatient.aspx/InsertIntoAuditLog",
        data: JSON.stringify(WSData),
        dataType: "json",
        error: function OnError(xhr) {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if (xhr.status == 999)
                window.location = "/frmSessionExpired.aspx";
            else {
                var log = JSON.parse(xhr.responseText);
                console.log(log);
                alert("USER MESSAGE:\n" + xhr.status + "-" + xhr.statusText +
                    ". \nCannot process request. Please Login again and retry. If issue persists, Please contact Support.\n\nEXCEPTION DETAILS: \nException Type" +
                    log.ExceptionType + " \nMessage: " + log.Message);
            }
        }
    });
}

var rafcalc = "";
function RafCalculation() {
    var HUMAN_ID = "";
    var regex = /<BR\s*[\/]?>/gi;
    var RAF_Score = "RAF Score :" + "<br/>";
    if (rafcalc != undefined && rafcalc != "") {
        HUMAN_ID = rafcalc.split('^')[0];
    }
    //var RAF_Score = Raf_Head.bold();
    $.ajax({
        type: "POST",
        url: "frmAssessmentNew.aspx/GetEncounterDetailsforRaf",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ Human_id: HUMAN_ID }),
        dataType: "json",
        async: true,
        success: function (data) {
            var Getdata = data.d.split('~');
            var HUMAN_ID = '';
            var DOB = '';
            var Gender = '';
            var icdlist = '';
            var sIsMedicaid = '';
            var sIsDisabled = '';
            var sIsCommunity = '';
            var sOriginallyDisabled = '';
            var sEnrollmentStatus = '';
            var PrimaryCarrier = '';
            if (Getdata.length > 0) {
                icdlist = Getdata[0];
                sIsMedicaid = Getdata[1].split(':')[1];
                sOriginallyDisabled = Getdata[2].split(':')[1];
                sIsCommunity = Getdata[3].split(':')[1];
                sIsDisabled = Getdata[4].split(':')[1];
                sEnrollmentStatus = Getdata[5].split(':')[1];
                PrimaryCarrier = Getdata[6].split(':')[1];
            }

            if (window.parent.parent.document.getElementsByName('lblPatientStrip')[0] != undefined) {
                var sHumanDetails = window.parent.parent.document.getElementsByName('lblPatientStrip')[0].innerText;
                HUMAN_ID = sHumanDetails.split('|')[4].split(':')[1].trim();
                if (sHumanDetails.split('|')[3].trim() == 'F')
                    Gender = 'FEMALE';
                else if (sHumanDetails.split('|')[3].trim() == 'M')
                    Gender = 'MALE'
                DOB = sHumanDetails.split('|')[1];
            }

            var now = new Date();
            var Current = now.getUTCFullYear() + "|Y";
            var Previous = now.getUTCFullYear() - 1 + "|N";
            //var Project_Name = 'EHR';
            var year = Current + '~' + Previous;
            var icdlist = icdlist;
            var is_store_Value = 'Y';
            var ProjectName = '';
            var surl = '';

            if (top.window.document.getElementById("ctl00_hdnProjectName").value != '') {
                ProjectName = top.window.document.getElementById("ctl00_hdnProjectName").value;
            }
            if (top.window.document.getElementById("ctl00_hdnProjectIPAddress").value != '')
                surl = top.window.document.getElementById("ctl00_hdnProjectIPAddress").value;
            if (rafcalc != undefined && rafcalc != "") {
                HUMAN_ID = rafcalc.split('^')[0];
                DOB = rafcalc.split('^')[1];
                Gender = rafcalc.split('^')[2];
            }

            var WSData = JSON.stringify({
                ProjectName: ProjectName,
                human_id: HUMAN_ID,
                Gender: Gender,
                DOB: DOB,
                year: year,
                icdlist: icdlist,
                is_store_Value: is_store_Value,
                sIsMedicaid: sIsMedicaid,
                sIsDisabled: sIsDisabled,
                sIsCommunity: sIsCommunity,
                sOriginallyDisabled: sOriginallyDisabled,
                sEnrollmentStatus: sEnrollmentStatus,
                PrimaryCarrier: PrimaryCarrier
            });
            $.ajax({
                type: "POST",
                url: surl,
                data: WSData,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                success: function (data) {
                    //var RAF_Score = "";
                    var jsonData = $.parseJSON(data.d);
                    if (top.window.document.getElementById("ctl00_C5POBody_lblRAF") != undefined) {
                        if (jsonData.length != 0) {
                            for (var i = 0; i < jsonData.length; i++)
                                if (jsonData[i] != "")
                                    RAF_Score += jsonData[i] + "<br/>";

                            top.window.document.getElementById("ctl00_C5POBody_lblRAF").innerHTML = RAF_Score;
                            if (RAF_Score.length != 0)
                                top.window.document.getElementById("RAF_tooltp").innerText = RAF_Score.replace(regex, "\n") + "\n";
                            else
                                top.window.document.getElementById("RAF_tooltp").innerText = "";
                        }
                        else {
                            top.window.document.getElementById("ctl00_C5POBody_lblRAF").innerHTML = (new Date).getFullYear() + ": " + RAF_Score + "<br/>" + (new Date).getFullYear() - 1 + ": " + RAF_Score_Year + "<br/>" + "HPN" + ": " + Score;
                            top.window.document.getElementById("RAF_tooltp").innerText = (new Date).getFullYear() + ": " + RAF_Score + "<br/>" + (new Date).getFullYear() - 1 + ": " + RAF_Score_Year + "<br/>" + "HPN" + ": " + Score;
                        }
                    }
                },
                error: function OnError(data, textStatus, jqXHR) {
                    console.log('error!', data, textStatus, jqXHR);
                }
            });
        },
        error: function OnError(xhr) {
            if (xhr.status == 999)
                window.location = "/frmSessionExpired.aspx";
            else {
                var log = JSON.parse(xhr.responseText);
                console.log(log);
                alert("USER MESSAGE:\n" + xhr.status + "-" + xhr.statusText +
                    ". \nCannot process request. Please Login again and retry. If issue persists, Please contact Support.\n\nEXCEPTION DETAILS: \nException Type" +
                    log.ExceptionType + " \nMessage: " + log.Message);
            }
        }
    });

}
function LoadReport() {
    var phyID;
    var role;
    var cookies = document.cookie.split(';');
    for (var l = 0; l < cookies.length; l++) {
        if (cookies[l].indexOf("CUserRole") > -1)
            role = cookies[l].split("=")[1];
        if (cookies[l].indexOf("CurrPhyId") > -1)
            phyID = cookies[l].split("=")[1];
    }
    $.ajax({
        type: "POST",
        contentType: "application/json;charset=utf-8",
        url: "frmRcopiaToolbar.aspx/GetBirtReportDetails",
        dataType: "json",
        success: function (data) {
            var ReportInfo = JSON.parse(data.d);
            var ReportURL = ReportInfo.BIRTUrl;
            var dbConn = ReportInfo.DBConnection;
            var role = ReportInfo.UserRole;
            if (role.toUpperCase() == "PHYSICIAN")
                ReportUrl = ReportURL + "_" + "Physician_Review_Status.rptdesign" + dbConn + "&PhysicianID=" + phyID;
            else if (role.toUpperCase() == "PHYSICIAN ASSISTANT")
                ReportUrl = ReportURL + "_" + "Physician_Assistant_Review_Status.rptdesign" + dbConn + "&PhysicianID=" + phyID;
            document.getElementById("iframeDashboard").src = ReportUrl;
        },
        error: function OnError(xhr) {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if (xhr.status == 999)
                window.location = "/frmSessionExpired.aspx";
            else {
                var log = JSON.parse(xhr.responseText);
                console.log(log);
                alert("USER MESSAGE:\n" + xhr.status + "-" + xhr.statusText +
                    ". \nCannot process request. Please Login again and retry. If issue persists, Please contact Support.\n\nEXCEPTION DETAILS: \nException Type" +
                    log.ExceptionType + " \nMessage: " + log.Message);
            }
        }
    });
}
function GetProtocolList() {
    debugger;
    var phyID;
    var role;
    var cookies = document.cookie.split(';');
    for (var l = 0; l < cookies.length; l++) {
        if (cookies[l].indexOf("CUserRole") > -1)
            role = cookies[l].split("=")[1];
        if (cookies[l].indexOf("CurrPhyId") > -1)
            phyID = cookies[l].split("=")[1];
    }
    var WSData = {
        UserID: phyID,
        UserRole: role
    }
    $.ajax({
        type: "POST",
        contentType: "application/json;charset=utf-8",
        url: "frmRcopiaToolbar.aspx/GetProtocols",
        data: JSON.stringify(WSData),
        dataType: "json",
        success: function (data) {
            var ProtocolLst = JSON.parse(data.d);
            var userID = "";
            var tagdata = "";
            for (var i = 0; i < ProtocolLst.length; i++) {
                if (userID != ProtocolLst[i].UserID) {
                    if (i != 0)
                        tagdata += "</br></ul>";
                    tagdata += "<h5><u><b>" + ProtocolLst[i].UserName + "</b></u></h5><ul>";
                    tagdata += "<li>" + ProtocolLst[i].Protocol + "</li>";
                    userID = ProtocolLst[i].UserID;
                }
                else {
                    tagdata += "<li>" + ProtocolLst[i].Protocol + "</li>";
                }
                if (i == ProtocolLst.length - 1)
                    tagdata += "</ul>";
            }
            $("#divprotocols").append(tagdata);
            window.setTimeout(function () { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }, 6000);

        },
        error: function OnError(xhr) {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if (xhr.status == 999)
                window.location = "/frmSessionExpired.aspx";
            else {
                var log = JSON.parse(xhr.responseText);
                console.log(log);
                alert("USER MESSAGE:\n" + xhr.status + "-" + xhr.statusText +
                    ". \nCannot process request. Please Login again and retry. If issue persists, Please contact Support.\n\nEXCEPTION DETAILS: \nException Type" +
                    log.ExceptionType + " \nMessage: " + log.Message);
            }
        }
    });
}
//Open print documents as a Non-Modal window for PA workflow change.
var Result;
var obj = new Array();
function OpenPrintDocReviewCorrection(url) {
    debugger;
    Result = openWindowNonModal(url, 365, 855, obj);
    $('#resultLoading').css("display", "none");
    if (Result == null)
        return false;
}
function openWindowNonModal(fromname, height, width, inputargument) {
    debugger;
    var Argument = "";
    var PageName = fromname;
    if (inputargument != undefined) {
        for (var i = 0; i < inputargument.length; i++) {
            if (i != 0) {
                Argument = Argument + "&" + inputargument[i];
            }
            else {
                Argument = inputargument[i];
            }
        }
        if (inputargument.indexOf('?') == -1 && inputargument.length != 0) {
            PageName = PageName + "?";
        }
    }


    var result = window.open(PageName + Argument, '', "Height=" + height + ",Width=" + width + ",resizable=yes,scrollbars=yes,titlebar=no,toolbar=no");
    if(result!=null)
        result.moveTo(((screen.width - width) / 2), ((screen.height - height) / 2));

    if (result == undefined) { result = window.returnValue; }
    return result;
}
function GetRadWindow() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    return oWindow;
}
function EnableChartLevelAutoSave() {
    localStorage.setItem("bSave", "false");
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
}
function DisableChartLevelAutoSave() {
    localStorage.setItem("bSave", "true");
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
}
var month = ["JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC"];

function Convert_to_Date(to_date) {
    if (to_date == "0001-Jan-01")
        to_date = "";
    var now = new Date();

    var currentdate = now.getFullYear() + '-' + month[now.getUTCMonth()] + '-' + ((now.getDate() < 10) ? "0" + now.getDate() : now.getDate());
    if (to_date.trim() == "" || to_date.trim().toUpperCase() == "CURRENT")
        return "CURRENT";
    else {
        var dt = format(to_date);
        if (Date.parse(dt) >= Date.parse(currentdate))
            return "CURRENT";
        else
            return "NONE";
    }

}
function format(to_date) {
    if (to_date.split('-').length == 1) {
        to_date = to_date + "-DEC-31";
    }
    else if (to_date.split('-').length == 2) {
        var Month = month[month.indexOf(to_date.split('-')[1].toUpperCase())];
        if (Month == "FEB") {
            if (to_date.split('-')[0] % 4 == 0)
                to_date = to_date + "-29";
            else
                to_date = to_date + "-28";
        }
        else if (Month == "SEP" || Month == "APR" || Month == "JUN" || Month == "NOV")
            to_date = to_date + "-30"
        else
            to_date = to_date + "-31";
    }
    else if (to_date.split('-').length == 3) {
        to_date = to_date.split('-')[0] + month[month.indexOf(to_date.split('-')[1].toUpperCase())] + to_date.split('-')[2];
    }
    return to_date;
}
function scrolify_RxHis(tblAsJQueryObject, height) {
    if (document.getElementById('dvAdd') != undefined)
        $('#dvAdd').remove();
    var oTbl = tblAsJQueryObject;
    var oTblDiv = $("<div id='dvAdd'/>");
    oTblDiv.css('height', height);
    oTblDiv.css('overflow', 'auto');

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

}

function showtooltip(x) {
    $(".tooltpShow").css('display', 'none');
    if ($("#" + $(x).data("tooltp")).text() != "") {
        $("#" + $(x).data("sqre")).css('display', '');
        $("#" + $(x).data("tooltp")).css('display', '');
    }
}

function hidetooltip() {
    if (top.window.document.getElementById('CheifComplaints_sqre') != undefined)
        top.window.document.getElementById('CheifComplaints_sqre').style.display = "none";
    if (top.window.document.getElementById('CheifComplaints_tooltp') != undefined)
        top.window.document.getElementById('CheifComplaints_tooltp').style.display = "none";

    if (top.window.document.getElementById('Allergies_sqre') != undefined)
        top.window.document.getElementById('Allergies_sqre').style.display = "none";
    if (top.window.document.getElementById('Allergies_tooltp') != undefined)
        top.window.document.getElementById('Allergies_tooltp').style.display = "none";

    if (top.window.document.getElementById('ProblemList_sqre') != undefined)
        top.window.document.getElementById('ProblemList_sqre').style.display = "none";
    if (top.window.document.getElementById('ProblemList_tooltp') != undefined)
        top.window.document.getElementById('ProblemList_tooltp').style.display = "none";

    if (top.window.document.getElementById('Vitals_sqre') != undefined)
        top.window.document.getElementById('Vitals_sqre').style.display = "none";
    if (top.window.document.getElementById('Vitals_tooltp') != undefined)
        top.window.document.getElementById('Vitals_tooltp').style.display = "none";

    if (top.window.document.getElementById('Medication_sqre') != undefined)
        top.window.document.getElementById('Medication_sqre').style.display = "none";
    if (top.window.document.getElementById('Medication_tooltp') != undefined)
        top.window.document.getElementById('Medication_tooltp').style.display = "none";

    if (top.window.document.getElementById('RAF_sqre') != undefined)
        top.window.document.getElementById('RAF_sqre').style.display = "none";
    if (top.window.document.getElementById('RAF_tooltp') != undefined)
        top.window.document.getElementById('RAF_tooltp').style.display = "none";

    if (top.window.document.getElementById('imgOverAllSummary_sqre') != undefined)
        top.window.document.getElementById('imgOverAllSummary_sqre').style.display = "none";
    if (top.window.document.getElementById('imgOverAllSummary_tooltp') != undefined)
        top.window.document.getElementById('imgOverAllSummary_tooltp').style.display = "none";
};

document.addEventListener('click', function () {
    hidetooltip();
});

//For Bug Id 57146
function RefreshOverallSummaryTooltip() {
    var regex = /<BR\s*[\/]?>/gi;
    var overalltext = "";
    if (top.window.document.getElementById("Allergies_tooltp").innerHTML != null && top.window.document.getElementById("Allergies_tooltp").innerHTML != "")
        overalltext = top.window.document.getElementById("Allergies_tooltp").innerHTML.replace(regex, "\n") ;
    if (top.window.document.getElementById("CheifComplaints_tooltp").innerHTML != null && top.window.document.getElementById("CheifComplaints_tooltp").innerHTML != "")
        overalltext += top.window.document.getElementById("CheifComplaints_tooltp").innerHTML.replace(regex, "\n") ;
    if (top.window.document.getElementById("ProblemList_tooltp").innerHTML != null && top.window.document.getElementById("ProblemList_tooltp").innerHTML != "")
        overalltext += top.window.document.getElementById("ProblemList_tooltp").innerHTML.replace(regex, "\n") ;
    if (top.window.document.getElementById("Vitals_tooltp").innerHTML != null && top.window.document.getElementById("Vitals_tooltp").innerHTML != "")
        overalltext += top.window.document.getElementById("Vitals_tooltp").innerHTML.replace(regex, "\n") ;
    if (top.window.document.getElementById("Medication_tooltp").innerHTML != null && top.window.document.getElementById("Medication_tooltp").innerHTML != "")
        overalltext += top.window.document.getElementById("Medication_tooltp").innerHTML.replace(regex, "\n") ;
    if (top.window.document.getElementById("RAF_tooltp").innerHTML != null && top.window.document.getElementById("RAF_tooltp").innerHTML != "")
        overalltext += top.window.document.getElementById("RAF_tooltp").innerHTML.replace(regex, "\n") ;

    top.window.document.getElementById("imgOverAllSummary_tooltp").innerText = overalltext;
}

function ScriptErrorLogEntry(sErrorMessage, sErrorLineNo, sErrorColumnNo, sErrorUrl, sErrorStack) {
    var ErrorLogData = {
        ErrorMessage: sErrorMessage,
        ErrorLineNo: sErrorLineNo,
        ErrorColumnNo: sErrorColumnNo,
        ErrorUrl: sErrorUrl,
        ErrorStack: sErrorStack
    }
    $.ajax({
            type: "POST",
            async: true,
            url: "frmRCopiaToolbar.aspx/ErrorLogEntry",
            data: JSON.stringify(ErrorLogData),
            dataType: "json",
            contentType: "application/json; charset=utf-8",            
            success: function (data) {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            },
            error: function OnError(xhr) {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                if (xhr.status == 999)
                    window.location = "/frmSessionExpired.aspx";
                else {
                    var log = JSON.parse(xhr.responseText);
                    console.log(log);
                    alert("USER MESSAGE:\n" + xhr.status + "-" + xhr.statusText +
                        ". \nCannot process request. Please Login again and retry. If issue persists, Please contact Support.\n\nEXCEPTION DETAILS: \nException Type" +
                        log.ExceptionType + " \nMessage: " + log.Message);
                }
            }
        });
    }

function ToolStripAlert(message) {
    if ($(top.window.document).find("#CheckAlert") != undefined && $(top.window.document).find("#CheckAlert")[0] != undefined)
    { 
    $(top.window.document).find("#CheckAlert")[0].style.display = "block";
    //Show message
    $(top.window.document).find("#innerMsgText")[0].innerText = message;
    }
    //Hide Tooltip
    // setTimeout(ToolStripAlertHide, 4000);
    //window.top.setInterval(function () {
    //    if ($(top.window.document).find("#CheckAlert") != undefined)
    //        $(top.window.document).find("#CheckAlert")[0].style.display = "none";
    //        }, 5000);
    setTimeout(function () { ToolStripAlertHide(); }, 5000);
}

function ToolStripAlertHide() {
    if ($(top.window.document).find("#CheckAlert") != undefined && $(top.window.document).find("#CheckAlert")[0] != undefined)
        $(top.window.document).find("#CheckAlert")[0].style.display = "none";
}
function ToolStripAlertHidexml() {
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    if ($(top.window.document).find("#CheckAlertxml") != undefined)
        $(top.window.document).find("#CheckAlertxml")[0].style.display = "none";
}
function xmlregenerateToolStripAlert(message) {
    $(top.window.document).find("#CheckAlertxml")[0].style.display = "block";
    $(top.window.document).find("#innerMsgTextxml")[0].innerText = message;

    //setTimeout(function () { ToolStripAlertHide(); }, 5000);
}