//CAP-1158
//CAP-1365
$('*[id*=_listDLC]').keypress(function (event) {
    if (event.keyCode === 10 || event.keyCode === 13) {
        event.preventDefault();
    }
});

function btnClearAll_Clicked(sender, args)
{
    var IsClearAll = DisplayErrorMessage('180010');
	     if (IsClearAll == true) {
	        
	         { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
	         document.getElementById('InvisibleButton').click();
	         window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
	         if (sender != undefined)
	             sender.set_autoPostBack(true);
	     }
	     else {
	         $find('btnSave').set_enabled(true);
	         if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
	             window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
	         else
	             window.parent.theForm.hdnSaveEnable.value = true;
	         return false;
	     }
}

function EnableSave(value) 
{
    var DLC =value + "_txtDLC";
    if (document.getElementById(DLC) != null) {
        if (document.getElementById(DLC).value.length >= 32767) {
            if ($find('btnSave') != null)
                $find('btnSave').set_enabled(true);
            localStorage.setItem("bSave", "false");
                if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
                    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
                else
                    window.parent.theForm.hdnSaveEnable.value = true;
            return false;
        }
    }
    if ($find('btnSave') != null)
        $find('btnSave').set_enabled(true);
    localStorage.setItem("bSave", "false");
       if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
           window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
       else
           window.parent.theForm.hdnSaveEnable.value = true;
}

function EnablePFSH(val) {
    if ($(window.parent.document).find('#btnPFSHVerified') != null)
        $(window.parent.document).find('#btnPFSHVerified')[0].disabled = false;
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
    else
        window.parent.theForm.hdnSaveEnable.value = false;
    var bValue = true;
    var PFSHVerified = localStorage.getItem("PFSHVerified");
    if (PFSHVerified != "") {
        var PFSH = PFSHVerified.split('|');
        for (var i = 0; i < PFSH.length; i++) {
            if (PFSH[i].split('-')[0] == val) {
                PFSHVerified = PFSHVerified.replace(PFSH[i], val + "-" + "TRUE");
                bValue = false;
            }
        }       
    }
    if (bValue == true)
        PFSHVerified = PFSHVerified + "|" + val + "-" + "TRUE";
    localStorage.setItem("PFSHVerified", PFSHVerified);
}
 
  
    function Enable_OR_Disable()
    {
	 if($find("btnSave").get_enabled())
	  document.getElementById("Hidden1").value="True";
	 else
	    document.getElementById("Hidden1").value="";
    }
    function CCTextChanged()
    {
        $find("btnSave").set_enabled(true);
        localStorage.setItem("bSave", "false");
	    if(window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable!=null&&window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable!=undefined)	    
           window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
        else
           window.parent.theForm.hdnSaveEnable.value=true;
	    document.getElementById("Hidden1").value="True";
        return false;
    }   
  function SaveEnable(event)
  {
      $find('btnSave').set_enabled(true);
      localStorage.setItem("bSave", "false");
    if(window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable!=null&&window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable!=undefined)	    
           window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
        else
           window.parent.theForm.hdnSaveEnable.value=true;
    return false;
  }

  function Grid_Enable_OR_Disable(value)
    {
     if(window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable!=null&&window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable!=undefined)	    
           window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
        else
           window.parent.theForm.hdnSaveEnable.value=true;
     $find('btnSave').set_enabled(true);
     localStorage.setItem("bSave", "false");
    }
    
    function KeyPressingText(value)
    {
    if(window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable!=null&&window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable!=undefined)	    
           window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
        else
           window.parent.theForm.hdnSaveEnable.value=true;
    $find('btnSave').set_enabled(true);
    localStorage.setItem("bSave", "false");
      if($find(value+"_txtDLC")._element.value.length>=32767)
         return false;
    }
     function KeyPressing(value)
    {
             
     if(window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable!=null&&window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable!=undefined)	    
           window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
        else
           window.parent.theForm.hdnSaveEnable.value=true;
     $find('btnSave').set_enabled(true);
     localStorage.setItem("bSave", "false");
      if($find(value+"_txtDLC")._element.value.length>=254)
      {
      DisplayErrorMessage('1091008')
      return false;
      }
         
    }
    
    function CutCopyPaste(sender, eventArgs)
    {
        var c = eventArgs.get_keyCode();
        if(c==46)
           eventArgs.set_cancel(true);
           
           
           if(window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable!=null&&window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable!=undefined)	    
           window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
        else
           window.parent.theForm.hdnSaveEnable.value=true;
           $find('btnSave').set_enabled(true);
           localStorage.setItem("bSave", "false");
           
    }
           
 function  LaodWaitCursorForCheckBox()
 {
     top.window.document.getElementById('ctl00_Loading').style.display = 'block';
 }

   function LaodWaitCursor()
   {
       top.window.document.getElementById('ctl00_Loading').style.display = 'block';
   }

    function EndWaitCursor()
	{
	    top.window.document.getElementById('ctl00_Loading').style.display = 'none';
	}
    function enableField(ChkValue) {
        var pcontrol = document.getElementById(ChkValue);
        if ($find('btnSave') != null)
            $find('btnSave').set_enabled(true);
        localStorage.setItem("bSave", "false");
        if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
        else
            window.parent.theForm.hdnSaveEnable.value = true;
        if (pcontrol.checked) {
            var txt = pcontrol.id.replace("chk", "txtAge");
            document.getElementById(txt).disabled = false;
            $find(txt).set_enabled(true);
            document.getElementById(txt).value = "";
            $find(txt).clear();
            var TControl = pcontrol.id.replace("chk", "DLCFamilyDisease");
            document.getElementById(TControl + "_txtDLC").disabled = false;
            document.getElementById(TControl + "_txtDLC").value = "";
            document.getElementById(TControl + "_pbDropdown").disabled = false;
            $('#' + TControl + "_pbDropdown").removeClass('pbDropdownBackgrounddisable');
            $('#' + TControl + "_pbDropdown").addClass('pbDropdownBackground');
            $('#' + TControl + "_pbDropdown").css('background','')
            document.getElementById(TControl + "_txtDLC").value = "";
            
           
            var cboControl = pcontrol.id.replace("chk", "cbo");
            var combo = $find(cboControl);
            combo.enable();
        }
        else {
            var oAlert = radconfirm('Are you sure you want to delete details of ' + pcontrol.id.replace("chk", " "), confirmCallBackFn, 330, 150, null, 'Family History')
            oAlert.set_behaviors(Telerik.Web.UI.WindowBehaviors.Close);
            oAlert.moveTo(300, 100);
            document.getElementById('hdnDeleteItem').value = pcontrol.id.replace("chk", "");

        }
    }

function confirmCallBackFn(arg)
{
    if (arg) {

        var Name = document.getElementById('hdnDeleteItem').value;

        document.getElementById("txtAge" + Name).disabled = true;
        document.getElementById("txtAge" + Name).value = "";
        document.getElementById("txtAge" + Name).style.background = "#EBEBE4";
        $find("txtAge" + Name).clear();
        document.getElementById("txtAge" + Name).style.background = "#EBEBE4";
        var TControl = "DLCFamilyDisease" + Name;
        document.getElementById(TControl + "_txtDLC").disabled = true;
        document.getElementById(TControl + "_txtDLC").value = "";
        document.getElementById(TControl + "_pbDropdown").disabled = true;
        document.getElementById(TControl + "_txtDLC").value = "";
        var cboControl = "cbo" + Name.replace(" ", "");
        var combo = $find(cboControl);
        combo.disable();
        combo.clearSelection();


        var TControlD = "DLCFamilyDisease" + Name;
        document.getElementById(TControlD + "_txtDLC").disabled = true;
        document.getElementById(TControlD + "_txtDLC").value = "";
        document.getElementById(TControlD + "_pbDropdown").disabled = true;
        document.getElementById(TControlD + "_txtDLC").value = "";


        var control = document.getElementById(TControlD + "_pbDropdown");
        if (control.childNodes[0] != undefined && control.childNodes[0].className != null) {
            control.style.background = '#808080';
        }
        else if (control.childNodes[0] != undefined && control.childNodes[0].nextSibling.className != null) {
            control.style.background = '#808080';
        }
   
        $('#' + TControl + "_pbDropdown").removeClass('pbDropdownBackground');
    }
    else {
        var Name = document.getElementById('hdnDeleteItem').value;
        document.getElementById("chk" + Name).checked = true;        
        var cboControl = "cbo" + Name.replace(" ", "");
        var combo = $find(cboControl);

        if (combo.value == "Deceased") {
            var TControlD = "DLCCauseOfDeath" + Name;
            document.getElementById(TControlD + "_txtDLC").disabled = false;
            document.getElementById(TControlD + "_txtDLC").value = "";
            document.getElementById(TControlD + "_pbDropdown").disabled = false;
            document.getElementById(TControlD + "_txtDLC").value = "";

            var control = document.getElementById(TControlD + "_pbDropdown");
            if (control.childNodes[0] != undefined && control.childNodes[0].className != null) {
                control.style.background = ' ';
            }
            else if (control.childNodes[0] != undefined && control.childNodes[0].nextSibling.className != null) {
                control.style.background = ' ';
               
            }

        }
      
        document.getElementById('hdnDeleteItem').value = "";
    }
           
           
  }

function selectedChanged(Name) {
    var cboControl = "cbo" + Name.replace(" ", "");
    var combo = $find(cboControl);
    $find('btnSave').set_enabled(true);
    localStorage.setItem("bSave", "false");
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    else
        window.parent.theForm.hdnSaveEnable.value = true;

    if (combo.get_text() == "Deceased") {
        var TControlD = "DLCCauseOfDeath" + Name.replace(" ", "");
        document.getElementById(TControlD + "_txtDLC").disabled = false;
        document.getElementById(TControlD + "_txtDLC").value = "";
        document.getElementById(TControlD + "_pbDropdown").disabled = false;       
        document.getElementById(TControlD + "_txtDLC").value = "";       
        var controll = document.getElementById(TControlD + "_pbDropdown");
        $('#' + TControlD + "_pbDropdown").removeClass('pbDropdownBackgrounddisable');
        $('#' + TControlD + "_pbDropdown").addClass('pbDropdownBackground');
        $('#' + TControlD + "_pbDropdown").attr('disabled',false)
        if (controll.childNodes[0] != undefined && controll.childNodes[0].className != null) {
            controll.style.background = '';           
        }
        else if (controll.childNodes[0] != undefined && controll.childNodes[0].nextSibling.className != null) {
            controll.style.background = '';           
        }

      
    }
    else {
        var TControlD = "DLCCauseOfDeath" + Name.replace(" ", "");
        document.getElementById(TControlD + "_txtDLC").disabled = true;
        document.getElementById(TControlD + "_txtDLC").value = "";
        document.getElementById(TControlD + "_pbDropdown").disabled = true;        
        document.getElementById(TControlD + "_txtDLC").value = "";
        var controls = document.getElementById(TControlD + "_pbDropdown");
        document.getElementById(TControlD + "_listDLC").style.display = "none";       
        if (controls.childNodes[0] != undefined && controls.childNodes[0].className != null)
            controls.childNodes[0].className = "fa fa-plus margin2";
        else if (controls.childNodes[0] != undefined && controls.childNodes[0].nextSibling.className != null)
            controls.childNodes[0].nextSibling.className = "fa fa-plus margin2";

        var TFamilyDiseaseControlD = "DLCFamilyDisease" + Name.replace(" ", "");
        document.getElementById(TFamilyDiseaseControlD + "_listDLC").style.display = "none";
        var listcontrolFamilyDisease = document.getElementById(TFamilyDiseaseControlD + "_pbDropdown");
        if (listcontrolFamilyDisease.childNodes[0] != undefined && listcontrolFamilyDisease.childNodes[0].className != null)
            listcontrolFamilyDisease.childNodes[0].className = "fa fa-plus margin2";
        else if (listcontrolFamilyDisease.childNodes[0] != undefined && listcontrolFamilyDisease.childNodes[0].nextSibling.className != null)
            listcontrolFamilyDisease.childNodes[0].nextSibling.className = "fa fa-plus margin2";
       
        $('#' + TControlD + "_pbDropdown").removeClass('pbDropdownBackground');
        $('#' + TControlD + "_pbDropdown").addClass('pbDropdownBackgrounddisable');
        $('#' + TControlD + "_pbDropdown").attr('disabled', true)
        if (controls.childNodes[0] != undefined && controls.childNodes[0].className != null) {
            controls.style.background = '#808080';        
        }
        else if (controls.childNodes[0] != undefined && controls.childNodes[0].nextSibling.className != null) {
            controls.style.background = '#808080';         
        }
      
    }
    
}
     

 function cancelBack()
  {
      
	    $find('btnSave').set_enabled(true);
	    localStorage.setItem("bSave", "false");
      if(window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable!=null&&window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable!=undefined)
         window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
     else
        window.parent.theForm.hdnSaveEnable.value=true;
  }

 function KeyPressing(sender, args) {
     if (parseInt(sender._value) > 150) {
         DisplayErrorMessage('180308');
         $find(sender._clientID).set_value("");
         $find(sender._clientID).focus();
         return false;
     }
 }

function btnSave_Clicked(sender,args)
{
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }

var now=new Date();
        var utc = now.toUTCString();    
        document.getElementById("hdnLocalTime").value=utc;
	}
	
	function pbDropDownss(textcontrol,ListControls,ListValue) {
  
    var control=document.getElementById(textcontrol);
    var possible=true;
  
    if(control.style.background!="rgb(128, 128, 128)") 
    {    
    lstCtrl=ListControls;
    if (control.innerHTML.indexOf("plus")!=-1 || control.innerHTML=="+")
    {
    $.ajax({
        type: "POST",
        url: "frmDLC.aspx/GetListBoxValues",
        data: '{fieldName: "'+ListValue+'" }',
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
    if(control.childNodes[0] != undefined&&control.childNodes[0].className!=null)
        control.childNodes[0].className="fa fa-minus margin2";
    else if(control.childNodes[0] != undefined&&control.childNodes[0].nextSibling.className!=null)
         control.childNodes[0].nextSibling.className="fa fa-minus margin2";
    
    }
    else if(control.innerHTML.indexOf("minus")!=-1 || control.innerHTML=="-")
    {
    if(control.childNodes[0] != undefined&&control.childNodes[0].className!=null)
        control.childNodes[0].className="fa fa-plus margin2";
    else if(control.childNodes[0] != undefined&&control.childNodes[0].nextSibling.className!=null)
         control.childNodes[0].nextSibling.className="fa fa-plus margin2";
    document.getElementById(ListControls).style.display="none";  
    hdnFieldName=null; 
    }
    
    if(hdnFieldName!=null&&hdnFieldName.split(',,')[0]!=lstCtrl)
    {
     document.getElementById(hdnFieldName.split(',,')[0]).style.display="none"; 
    
     var listcontrol=document.getElementById(hdnFieldName.split(',,')[1]);
       if(listcontrol.childNodes[0] != undefined&&listcontrol.childNodes[0].className!=null)
        listcontrol.childNodes[0].className="fa fa-plus margin2";
    else if(listcontrol.childNodes[0] != undefined&&listcontrol.childNodes[0].nextSibling.className!=null)
         listcontrol.childNodes[0].nextSibling.className="fa fa-plus margin2";
    }
        hdnFieldName= lstCtrl+",,"+textcontrol;
        
       }
        return false;
}

function OpenAddorUpdates(ctrlID,ID)
{
    var control=document.getElementById(ID);
       if(control.style.background!="rgb(128, 128, 128)") 
       {    
         var obj=new Array();
         obj.push("FieldName="+ctrlID);
         openModal('frmAddorUpdateKeywords.aspx','550','650',obj,'MessageWindow')
         var currWindow=$find('MessageWindow');
currWindow.set_behaviors(-Telerik.Web.UI.WindowAutoSizeBehaviors.None);
       }
    return false;
}

function pbClearAlls(value)
{
   
  return false;
}
function HistoryFamily_Load() {
    
    if (window.parent.parent.theForm.hdnSaveButtonID != undefined)
    window.parent.parent.theForm.hdnSaveButtonID.value = "btnSave,RadMultiPage1";
    top.window.document.getElementById('ctl00_Loading').style.display = "none";
   
    $("textarea[id *= txtDLC]").removeClass('DlcClass');

    $("textarea[id *= txtDLC]").addClass('Editabletxtbox');
    $("textarea[Enable=Y]").addClass('Editabletxtbox');
    $('#DLC_pbDropdown').addClass("pbDropdownBackground");
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }

   
}
function PageKeyPress() {
var focusedElement = document.activeElement;
    if (focusedElement.id == "DLC_txtDLC")
    {
        $find('btnSave').set_enabled(true);
        localStorage.setItem("bSave", "false");
    }
    return true;
}

// Added for Bug id=28876
function RadNumericTextBoxkeypress(sender, args) 
{
    if ((args.get_keyCode() == 109) || (args.get_keyCode() == 45)) 
    {
        args.set_cancel(true);
    }
    else 
    {
        $find('btnSave').set_enabled(true);
        localStorage.setItem("bSave", "false");
        if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
        else
            window.parent.theForm.hdnSaveEnable.value = true;
    }
}
function SavedSuccessfully() {
    
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
    else
        window.parent.theForm.hdnSaveEnable.value = false;
    localStorage.setItem("bSave", "true");
    PFSH_AfterAutoSave();
    DisplayErrorMessage('180015');
}