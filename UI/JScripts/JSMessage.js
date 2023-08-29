  function closeWindow()
  {
      //CAP-782 Cannot read properties of null
     if(document.getElementById("IsLoginOpen")?.value!="YES")
     {
	    self.close();
	 }
  }
     function SetIntervalTime(time)
     {
      self.setInterval(function(){closeWindow()},time);
     }
    function GetRadWindow() 
    {
        var oWindow = null;
        if (window.radWindow) oWindow = window.radWindow;
        else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
        return oWindow;
    }
  
 function GetUTCTime()
    {
      var now=new Date();
    var utc=(now.getUTCDate()+'/'+ now.getUTCMonth()+1)+'/'+now.getUTCFullYear();utc+=' '+now.getUTCHours()+':'+now.getUTCMinutes()+':'+now.getUTCSeconds();
    document.getElementById("hdnLocalTime").value=utc;
    }
 function RadWindowClosepopup()
 {
     self.close();
     
 }
function RadWindowClose()
{
    var oWindow = null;
          if (window.radWindow)
               oWindow = window.radWindow;
          else if (window.frameElement.radWindow)
               oWindow = window.frameElement.radWindow;
          if(oWindow!=null)
           oWindow.close();
           return false;
}
//BugID:54514
function loadRcopiaRxCount() {

    $.ajax({
        type: "POST",
        url: "frmRCopiaToolbar.aspx/LoadRCopiaNotification",
        data: '',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: OnSuccessRCopia,
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
function OnSuccessRCopia(response) {
    var responseValues = response.d.split('#$%');
    var rxValues = "";
    if (responseValues == "") {
        document.getElementById("tsRefill").style.display = "none";
        document.getElementById("tsRx_Pending").style.display = "none";
        document.getElementById("tsRx_Need_Signing").style.display = "none";
        document.getElementById("tsRx_Change").style.display = "none";
    }
    if (responseValues != null) {
        document.getElementById("tsRefill").innerText = responseValues[0];
        document.getElementById("tsRx_Pending").innerText = responseValues[1];
        document.getElementById("tsRx_Need_Signing").innerText = responseValues[2];
        document.getElementById("tsRx_Change").innerText = responseValues[3];
        rxValues = document.getElementById("tsRefill").innerText + "$:$" + document.getElementById("tsRx_Pending").innerText + "$:$" + document.getElementById("tsRx_Need_Signing").innerText + "$:$" + document.getElementById("tsRx_Change").innerText;
    }
    else {

        document.getElementById("tsRefill").innerText = "Refill : 0";
        document.getElementById("tsRx_Pending").innerText = "Rx_Pending : 0";
        document.getElementById("tsRx_Need_Signing").innerText = "Rx_Need_Signing : 0";
        document.getElementById("tsRx_Change").innerText = "RxChange : 0";
        rxValues = document.getElementById("tsRefill").innerText + "$:$" + document.getElementById("tsRx_Pending").innerText + "$:$" + document.getElementById("tsRx_Need_Signing").innerText + "$:$" + document.getElementById("tsRx_Change").innerText;
    }
    sessionStorage.setItem("RxCount", rxValues);
}
//function btnpatientChart_Click() {
//    humanid = document.getElementById(GetClientId('hdnHumanID')).value;
//    var labResult = document.getElementById(GetClientId('hdnLabResult')).value;
//    if (labResult == 'LabResult') {
//        Result = openNonModal("frmPatientchart.aspx?HumanID=" + humanid + "&from=viewresult&ScreenMode=Menu&openingfrom=Menu", 840, 1278, obj);//BugID:45876,for BugID:45808 increased screen width to 1278px
//    }
//    else {
//        Result = openNonModal("frmPatientchart.aspx?HumanID=" + humanid + "&from=openpatientchart&ScreenMode=Menu&openingfrom=Menu", 840, 1278, obj);//BugID:45876,for BugID:45808 increased screen width to 1278px
//    }
//    $('#resultLoading').css("display", "none");
//    if (Result == null)
//        return false;
//}

function btnpatientChart_Click() {
    humanid = document.getElementById(GetClientId('hdnHumanID')).value;
    var labResult = document.getElementById(GetClientId('hdnLabResult')).value;
    if (labResult == 'N') {
        Result = openNonModal("frmPatientchart.aspx?HumanID=" + humanid + "&from=viewresult&ScreenMode=Menu&openingfrom=Menu", 840, 1278, obj);//BugID:45876,for BugID:45808 increased screen width to 1278px
    }
    else {
        Result = openNonModal("frmPatientchart.aspx?HumanID=" + humanid + "&from=openpatientchart&ScreenMode=Menu&openingfrom=Menu", 840, 1278, obj);//BugID:45876,for BugID:45808 increased screen width to 1278px
    }
    $('#resultLoading').css("display", "none");
    if (Result == null)
        return false;
}
function openNonModal(fromname, height, width, inputargument) {
    var Argument = "";

    if (fromname != undefined && fromname != null) {
        if (fromname.indexOf('?') > -1) {
            fromname += "&allowmultipletab=true";
        }
        else {
            fromname += "?allowmultipletab=true";
        }
    }

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
        if (inputargument.lenght != 0) {
            PageName = PageName + "?";
        }
    }


    var result = window.open(PageName + Argument, '', "Height=" + height + ",Width=" + width + ",resizable=yes,scrollbars=yes,titlebar=no,toolbar=no");
    if (result != null)
        result.moveTo(30, 150);

    if (result == undefined) { result = window.returnValue; }
    return result;
}
//Jira #CAP-889
function btnMoveClientClick() {
    RemoveItem(document.URL, "PrescriptionID");
}