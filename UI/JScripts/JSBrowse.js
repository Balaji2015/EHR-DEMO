function btnCancelClient_Clicked()
{
    var cntCCD = document.getElementById("hdnFileCnt").value;
    if($(top.window.document).find("#tsbrowse")[0] != null )
        $(top.window.document).find("#tsbrowse")[0].innerText = "CCD : " + cntCCD;
    if (sessionStorage.getItem("MailClinicalCnt") != null && sessionStorage.getItem("MailClinicalCnt") != undefined) {
        var val = cntCCD + ":" + sessionStorage.getItem("MailClinicalCnt").split(':')[1];
        sessionStorage.setItem("MailClinicalCnt", val);
    }
    GetRadWindow().close();
    return false;
}

function GetRadWindow() 
{


    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement!=null&&window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    return oWindow;
}
  
function openform(filepath)
{
    window.parent.parent.radopen(filepath, "browseRadWindow", 800, 750);
}
function downloadURI(uri) {
     {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
    var link = document.createElement('a'); link.download = name; link.href = uri; link.click();
}
function btnReceive_Clicked(sender,args)
	{
	
	 var now=new Date();
     var then=now.getDay()+'-'+(now.getMonth()+1)+'-'+now.getFullYear();then+=' '+now.getHours()+':'+now.getMinutes()+':'+now.getSeconds();
     var utc=(now.getUTCMonth()+1)+'/'+now.getUTCDate()+'/'+now.getUTCFullYear();utc+=' '+now.getUTCHours()+':'+now.getUTCMinutes()+':'+now.getUTCSeconds();
     document.getElementById(GetClientId("hdnLocalTime")).value=utc;

	}
function btnReceiveMail_Clicked() {

    var now = new Date();
    var then = now.getDay() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear(); then += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds();
    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
    document.getElementById(GetClientId("hdnLocalTime")).value = utc;
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
   
}
	function ShowIframe() 
	{
	    var filelocation = document.getElementById('hdn').value
        window.open(filelocation,"CDA Human Readable","","")
    }
	
	
function IsEmail(email)
    {
      var expr = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
      return expr.test(email);
    }
function ActivityHistoryClick()
{
 { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
 $.ajax({
        type: "POST",
        url: "frmBrowse.aspx/GetActivities",
        data: '{FieldValues: "'+"CCD Import"+","+"CCD Export"+'"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: OnSuccess,
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
             {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
        }
 });
  {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
return false;

}
function OnSuccess(response)
 {
 var RadDownload=$find('actLogRadWindow');
    RadDownload.show();
    RadDownload._resizeExtender.set_hideIframes(true);
    var TextBox=document.getElementById("actLogRadWindow_C_txtActivityLog");
    TextBox.value = response.d;
    //Jira Cap - 4012
    TextBox.readOnly = true;
    TextBox.disabled = true;
}

    
    


function OpenPDF()
{
 var Value = document.getElementById("hdnFilePath").value;
  setTimeout(  
    function()
    {
 var oWindow = GetRadWindow();
 
 var childWindow=oWindow.BrowserWindow.radopen("frmSummaryOfCare.aspx?FileName=" + Value + "&Type="+ document.getElementById("hdnCCR").value + "&UniversalTime="+ document.getElementById("hdnLocalTime").value,"browseRadWindow");
 setRadWindowProperties(childWindow,900,1100);
 childWindow.add_close(Test);//BugID:51185 - File moved to ImportedFiles folder on Reconcile Click.

   },0);
 return false;
}
function Test(oWindow, args)
{
    $("#btnReceiveMail").click();
}

function CloseImport(oWindow, args) {
    $.ajax({
        type: "POST",
        url: "frmerror.aspx/MoveFile",
        data: JSON.stringify({
            "filename": document.getElementById('hdnFilePath').value
        }),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            $("#btnReceiveMail").click();
        },
        error: function (result) {
        }
    });
   
}
function GetRadWindow() 
    {
        var oWindow = null;
        if (window.radWindow) oWindow = window.radWindow;
        else if (window.frameElement!=null&&window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
        return oWindow;
    }
    
    
   function setRadWindowProperties(childWindow,height,width)
 {
       childWindow.SetModal(true);
       childWindow.set_visibleStatusbar(false);
       childWindow.setSize(width, height);
       childWindow.set_behaviors(Telerik.Web.UI.WindowBehaviors.Close|Telerik.Web.UI.WindowBehaviors.Move);
       childWindow.set_iconUrl("Resources/16_16.ico");
       childWindow.set_keepInScreenBounds(true);
       childWindow.set_centerIfModal(true);
       childWindow.center();

 }
 
 
 function ShowLocalTime()
{
var dt=new Date();
var now=new Date();
var then=now.getDay()+'-'+(now.getMonth()+1)+'-'+now.getFullYear();then+=' '+now.getHours()+':'+now.getMinutes()+':'+now.getSeconds();
var utc=(now.getUTCMonth()+1)+'/'+now.getUTCDate()+'/'+now.getUTCFullYear();
utc+=' '+now.getUTCHours()+':'+now.getUTCMinutes()+':'+now.getUTCSeconds();
document.getElementById('hdnLocalTime').value = utc;
}

function btnOk_Clicked()
	{
    ShowLocalTime();
    var ImportGrid = $find('grdImport');
    var no_Items_Import = false;
    var no_selectedItems_Import = false;
    var no_Items_Negative = false;
    var no_selectedItems_Negative = false;
    if (ImportGrid != null)
    {
        var MasterTable = ImportGrid.get_masterTableView();
        if (MasterTable.get_dataItems().length == 0)
        {
            no_Items_Import = true;
           
        }
        if (MasterTable.get_selectedItems().length == 0)
        {
            no_selectedItems_Import = true;
           
        }
    }
    var NegativeGrid = $find('grdVerifiedNegativeFiles');
    if (NegativeGrid != null) {
        var MasterTable = NegativeGrid.get_masterTableView();
        if (MasterTable == null || MasterTable.get_dataItems().length == 0) {
            no_Items_Negative=true;
            
        }
        if (MasterTable == null || MasterTable.get_selectedItems().length == 0) {
            no_selectedItems_Negative = true;
            
        }
    }
    if (no_Items_Import && no_Items_Negative) {
        DisplayErrorMessage('8001');
        return false;
    }
    if (no_selectedItems_Import && no_selectedItems_Negative) {
        DisplayErrorMessage('000015');
        return false;
    }
  

    }
	function ShowXML()
	{
	    window.open(document.getElementById("hdnFilePath").value,"CDA Human Readable","","")
        return false;
	}
	function ShowAlert(s) {
	    alert(s);
	}


	function openErrorForm(FileType)
	{
	    var Value = document.getElementById("hdnFilePath").value;
	    setTimeout(
          function () {
              var oWindow = GetRadWindow();

              var childWindow = oWindow.BrowserWindow.radopen("frmError.aspx?FileName=" + Value+"&Type="+FileType , "browseRadWindow");
              setRadWindowProperties(childWindow, 700, 700);
             
              childWindow.add_close(CloseImport);

          }, 0);
	    return false;

	}
	function clearSelectedItemsgrdImport(sender, args) {
	    $find('grdImport').clearSelectedItems();
	}
	function clearSelectedItemsgrdNegative(sender, args) {
	    $find('grdVerifiedNegativeFiles').clearSelectedItems();
	}