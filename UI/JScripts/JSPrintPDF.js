function btnClick(sender, args) 
{
var iFrame=document.getElementById("PDFLOAD");
iFrame.contentWindow.print();
return false;
}
function GetRadWindow() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement != null && window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    return oWindow;
}
function RadTabStrip2_TabSelected(sender,args)
{
    sender.set_enabled(false);
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
}
function PDF_Load() {
    if (window.parent.document.activeElement.contentDocument!=undefined && window.parent.document.activeElement.contentDocument.URL.indexOf("frmGrowthChart") > -1) {
        if (window.innerHeight >= 780)
            $('#PDFLOAD').height(600);
        else
            if (window.innerHeight >= 550 && window.innerHeight <= 799)
                $('#PDFLOAD').height(508)
            else
                $('#PDFLOAD').height(400);
    }
     {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
}


function btnFaxClick(sender, args) {
    
    var vscreen="";
    var vcontent = $($(top.window.document).find("iframe")[0].contentDocument).find('.tab-content');
    if (vcontent != undefined && vcontent.length != 0) {
        if($($($(vcontent).find('.tab-pane.active')).find('.clsIframe')[0].contentDocument).find('#myTabs li.active a')[0]!=undefined)
        vscreen = $($($(vcontent).find('.tab-pane.active')).find('.clsIframe')[0].contentDocument).find('#myTabs li.active a')[0].innerText;
    }

    var FaxSubject = JSON.parse(localStorage.getItem("FaxSubject1"));
    var vFaxSubject = "";
    var vTabstrip = $($($($("#RadTabStrip2").find('.rtsLevel1')).find('ul>li>a')));//.length;//[1].className
    if (vTabstrip != undefined > 0) {
        for (var i = 0; i < vTabstrip.length; i++) {
            if (vTabstrip[i].className == "rtsLink rtsSelected") {
                if (vscreen != undefined && vscreen == 'Referral Order') {
                    if (vTabstrip[i].innerText.split(".pdf")[0].split('PM').length > 1)
                        vFaxSubject = vTabstrip[i].innerText.split(".pdf")[0].split('PM')[0] + " PM";
                    else if (vTabstrip[i].innerText.split(".pdf")[0].split('AM').length > 1)
                        vFaxSubject = vTabstrip[i].innerText.split(".pdf")[0].split('AM')[0] + " AM";
                }
                else if (vscreen != undefined && vscreen == 'Immunization/Injection') {
                    if (vTabstrip[i].innerText.split(".pdf")[0].split('PM').length > 1)
                        vFaxSubject = vTabstrip[i].innerText.split(".pdf")[0].split('PM')[0] + " PM";
                    else if (vTabstrip[i].innerText.split(".pdf")[0].split('AM').length > 1)
                        vFaxSubject = vTabstrip[i].innerText.split(".pdf")[0].split('AM')[0] + " AM";
                }
                else if (vscreen != undefined && vscreen == 'Diagnostic Order') {

                    var vname = "";
                    if (vTabstrip[i].innerText.split(".pdf")[0].split('_ACUR')[1] != undefined) {
                        for (var k = 0; k < FaxSubject.split("|").length; k++) {
                            if (k == 0) {
                                if (FaxSubject.split("|")[0].split("$")[1].split("_")[0] == vTabstrip[i].innerText.split(".pdf")[0].split('_ACUR')[1]) {
                                    vname = FaxSubject.split("|")[0].split("$")[0] + "_" + FaxSubject.split("|")[0].split("$")[1].split("_")[2];
                                    break;
                                }
                            }
                            else {
                                if (FaxSubject.split("|")[k].split("_")[0] == vTabstrip[i].innerText.split(".pdf")[0].split('_ACUR')[1]) {
                                    vname = FaxSubject.split("|")[0].split("$")[0] + "_" + FaxSubject.split("|")[k].split("_")[2];
                                    break;
                                }
                            }
                        }
                        vFaxSubject = "Order" + vname;
                    }
                    else {
                        var vDIAGNOSTIC = vTabstrip[i].innerText.split(".pdf")[0].split("DIAGNOSTIC_ORDER");
                        var vcreatedate = "";
                        var vlab = "";
                        if (vDIAGNOSTIC != undefined) {
                            vcreatedate = vTabstrip[i].innerText.split(".pdf")[0].split("DIAGNOSTIC_ORDER")[1].split("_")[vDIAGNOSTIC[1].split("_").length - 1];
                            vlab = vTabstrip[i].innerText.split(".pdf")[0].split("DIAGNOSTIC_ORDER")[1].split("_")[vDIAGNOSTIC[1].split("_").length - 3];
                        }
                        vFaxSubject = "Order" + FaxSubject.split("|")[0].split("$")[0] + "_" + vcreatedate;
                    }

                }
                else if (vscreen != undefined && vscreen == 'General Plan' && vTabstrip[i].innerText.split(".pdf")[0].split('_')[0] != undefined) {
                    var vGeneralPlan = vTabstrip[i].innerText.split(".pdf")[0] + "_";
                    if (FaxSubject.split("_")[1] != undefined)
                        vFaxSubject = vGeneralPlan + FaxSubject;
                }
                else if (FaxSubject != undefined&&FaxSubject.indexOf('|')>-1) {
                    vFaxSubject = "Order" + FaxSubject.split("|")[0].split("$")[0] + "_" + FaxSubject.split('$')[1].split('|')[i].split('_')[2];
                }
                else if (FaxSubject != undefined)
                { //Checkout print documents
                    if (FaxSubject.includes('|') == false)
                    {
                        vFaxSubject = "Order" + FaxSubject.split("|")[0].split("$")[0] + "_" + FaxSubject.split('$')[1].split('|')[i].split('_')[2];
                    }
                    else
                        vFaxSubject = vTabstrip[i].innerText.split(".pdf")[0] + FaxSubject;;
                }
                else if (vscreen != undefined && vscreen == 'DME Order' && vTabstrip[i].innerText.split(".pdf")[0].split('_')[0] != undefined) {
                    var vDIAGNOSTIC = vTabstrip[i].innerText.split(".pdf")[0].split("DME_ORDER");
                    var vcreatedate = "";
                    var vlab = "";
                    if (vDIAGNOSTIC != undefined) {
                        vcreatedate = vTabstrip[i].innerText.split(".pdf")[0].split("DME_ORDER")[1].split("_")[vDIAGNOSTIC[1].split("_").length - 1];
                        vlab = vTabstrip[i].innerText.split(".pdf")[0].split("DME_ORDER")[1].split("_")[vDIAGNOSTIC[1].split("_").length - 2];
                    }
                    var vname = "";
                    if (FaxSubject.split("_")[1] != undefined)
                        vname = FaxSubject.split("_")[1];
                    vFaxSubject = "DME_Order_" + vname + "_" + vcreatedate;
                }
                break;
            }
        }
    }
    


    //For Menu Level EV
    if (document.getElementById('hdnScreenMode').value.trim() != "" && document.getElementById('hdnScreenMode').value.trim() == "EV") {
        if (document.getElementById('hdnHumanName').value!="")
            vFaxSubject = "Eligibility Verification_" + document.getElementById('hdnHumanName').value + "_" + document.getElementById('FaxCurrentFileName').value.split("\\")[document.getElementById('FaxCurrentFileName').value.split("\\").length - 1].split("_")[2];
        else if(document.getElementById('FaxSubject').value.trim() != "")
            vFaxSubject = document.getElementById('FaxSubject').value;
    }
    else if (document.getElementById('FaxSubject').value.trim() != "") //This part of code for Checin EV Fax subject autofill.
        vFaxSubject = document.getElementById('FaxSubject').value;
    


    localStorage['FaxSubject'] = "";
    localStorage['FaxSubject'] = vFaxSubject.replace("__", "_");
    localStorage.setItem("IsMenuEFax", "N");
    $(top.window.document).find("#TabFax").modal({ backdrop: "static", keyboard: false }, 'show');
    //$(top.window.document).find("#TabFax").css({"z-index:":"5001"});
    $(top.window.document).find("#TabModalEFaxTitle")[0].textContent = "Efax";
    $(top.window.document).find("#TabmdldlgEFax")[0].style.width = "1050px";
    $(top.window.document).find("#TabmdldlgEFax")[0].style.height = "963px";
    $(top.window.document).find("#TabmdldlgEFax").css({ "margin-left": "22%"});
    var sPath = ""
    sPath = "frmEFax.aspx?DMEOrder=" + document.getElementById('FaxCurrentFileName').value;
    $(top.window.document).find("#TabEFaxFrame")[0].style.height = "659px";
    $(top.window.document).find("#TabEFaxFrame")[0].contentDocument.location.href = sPath;
    $(top.window.document).find("#TabFax").one("hidden.bs.modal", function (e) {
    });
}