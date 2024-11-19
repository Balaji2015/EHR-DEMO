  function CloseFindAuth()
        {
    var result=new Object();
    result.AuthNo=document.getElementById(GetClientId('hdnSelectedAuthNo')).value;
    window.returnValue=result;
    returnToParent(result);
     return false;     
        }

    function OpenCreateAuthrization()
    {
    if (document.getElementById(GetClientId('txtPatAccNo')).value=="")
    {
   DisplayErrorMessage('3045021');
    return false;
    }
    else
    {
      setTimeout(  
    function()
    {
      var obj=new Array();  
 var oWnd = GetRadWindow();
    obj.PatientName = document.getElementById(GetClientId("txtPatientName")).value;
    obj.PatientDOB = document.getElementById(GetClientId("txtPatientDOB")).value;
    obj.HumanID = document.getElementById(GetClientId("txtPatAccNo")).value;
    obj.PCPID=document.getElementById(GetClientId('hdnPCPID')).value;
    obj.RefToPhyID=document.getElementById(GetClientId('hdnRefToPhyID')).value;
    obj.AuthID="0";
    obj.OpenMode="Create";
    obj.EncID=document.getElementById(GetClientId('hdnEncounterID')).value;
   var PatientType= document.getElementById(GetClientId('hdnPatientType')).value
   obj.PCP=document.getElementById(GetClientId('txtPCPName')).value;
   obj.InsPlanName=document.getElementById(GetClientId('hdnPlanName')).value;
  obj.policyHolderId=document.getElementById(GetClientId('hdnPolicyHolderID')).value
     
   var childWindow=oWnd.BrowserWindow.radopen("frmCaptureAuthorization.aspx?PatientName="+obj.PatientName+"&ScreenName="+"CaptureAuthorization"+"&PatientType="+PatientType+"&PatientDOB="+obj.PatientDOB+"&AccNo="+obj.HumanID+"&AuthID="+obj.AuthID+"&OpenMode="+obj.OpenMode+"&EncID="+obj.EncID+"&PCP="+obj.PCP+"&PolicyHolderId="+obj.policyHolderId+"&PcpId="+obj.PCPID+"&InsPlanName="+obj.InsPlanName ,"MessageWindow");
   setRadWindowProperties(childWindow,782,1250);
 childWindow.add_close(RefreshScreen);  
 
},0);
return false;

  

   }
    } 
    function RefreshScreen(oWindow, args) 
    {
       document.getElementById(GetClientId("btnSearch")).click();

 
    }   
    function OpenModifyAuthorization()
    {
     if (document.getElementById(GetClientId('hdnSelectedIndex')).value=="")
    {
   DisplayErrorMessage('3045021');
    return false;
    }
    else
    {
      setTimeout(  
    function()
    {
      var obj=new Array();  
       var oWnd = GetRadWindow();
       obj.AuthID=document.getElementById(GetClientId('hdnAuthID')).value;
      obj.PatientName = document.getElementById(GetClientId('txtPatientName')).value;
    obj.PatientDOB = document.getElementById(GetClientId('txtPatientDOB')).value;
    obj.HumanID = document.getElementById(GetClientId('txtPatAccNo')).value;
     var PatientType= document.getElementById(GetClientId('hdnPatientType')).value
     obj.OpenMode="Modify";
      obj.PCPID=document.getElementById(GetClientId('hdnPCPID')).value;
     obj.EncID=document.getElementById(GetClientId('hdnEncounterID')).value;
      var PatientType= document.getElementById(GetClientId('hdnPatientType')).value
     obj.PCP=document.getElementById(GetClientId('txtPCPName')).value;
   obj.InsPlanName=document.getElementById(GetClientId('hdnPlanName')).value;
  obj.policyHolderId=document.getElementById(GetClientId('hdnPolicyHolderID')).value
   var childWindow=oWnd.BrowserWindow.radopen("frmCaptureAuthorization.aspx?PatientName="+obj.PatientName+"&PatientDOB="+obj.PatientDOB+"&AccNo="+obj.HumanID+"&PatientType="+PatientType+"&AuthID="+obj.AuthID+"&OpenMode="+obj.OpenMode+"&EncID="+obj.EncID+"&PCP="+obj.PCP+"&PolicyHolderID="+obj.PolicyHolderID+"&PcpId="+obj.PCPID+"&InsPlanName="+obj.InsPlanName,"MessageWindow");
   setRadWindowProperties(childWindow,782,1250);
 childWindow.add_close(RefreshScreen); 
  
 
},0);

return false;
  }   
    }
    function OpenViewAuthorization()
    {
    if (document.getElementById(GetClientId('hdnAuthID')).value=="")
    {
   
    DisplayErrorMessage('3045022');
    return false;
    }
    else
    {
    var grdAuthList=document.getElementById(GetClientId('grdAuthList'));

      
     var obj=new Object();
     obj.AuthID=document.getElementById(GetClientId('hdnAuthID')).value;
      obj.PatientName = document.getElementById(GetClientId('txtPatientName')).value;
    obj.PatientDOB = document.getElementById(GetClientId('txtPatientDOB')).value;
    obj.HumanID = document.getElementById(GetClientId('txtPatAccNo')).value;
   obj.PatientType= document.getElementById(GetClientId('txtPatientType')).value
     obj.OpenMode="View";
     obj.EncID=document.getElementById(GetClientId('hdnEncounterID')).value;
    window.dialogArgument=obj;      
   window.showModalDialog("frmCaptureAuthorization.aspx?PatientName="+obj.PatientName+"&PatientDOB="+obj.PatientDOB+"&PatientType="+obj.PatientType+"&AccNo="+obj.HumanID+"&AuthID="+obj.AuthID+"&OpenMode="+obj.OpenMode+"&EncID="+obj.EncID,obj,"center:yes;resizable:no;dialogHeight:800px;dialogWidth:1100px");
   if (window.dialogArgument!=null)
   {
   return true;
   }
   else
   {  
   return false;
   }
   }
    }
    function  OpenFindPatient()
     {
      setTimeout(  
    function()
    {
 
 var oWnd = GetRadWindow();

 var childWindow=oWnd.BrowserWindow.radopen("frmFindPatient.aspx","MessageWindow");
 setRadWindowProperties(childWindow,251,1200);
 childWindow.add_close(FindPatientClick)
  childWindow.remove_close(FindReferralPhysicianClick); 
 

 },0);
 return false;
     }
     function FindPatientClick(oWindow, args) 
    {
       var Result = args.get_argument();
       if (Result)
       {
document.getElementById(GetClientId("txtPatAccNo")).value=Result.HumanId;
document.getElementById(GetClientId("txtPatientName")).value=Result.PatientName;
document.getElementById(GetClientId("txtPatientDOB")).value=Result.PatientDOB;
document.getElementById(GetClientId("txtPatientType")).value=Result.HumanType;
  document.getElementById(GetClientId("btnSearch")).click();  
 }
 
    }
     
     
     function OnOpenDemographicsClick(oWindow, args) 
    {
       var Result = args.get_argument();
       if (Result)
       {
         var HumanId = Result.HumanId;
         if(HumanId!="0")    
         {
               var obj=new Array();
           obj.push("HumanId="+HumanId);
           obj.push("bInsurance="+true);
           obj.push("ScreenName=Demographics");
           var result=openModal("frmPatientDemographics.aspx",1230,1130,obj,"ctl00_ModalWindow");
          
       }
         
       }
    }
      function OpenFindRefToPhysician()
    {     
          setTimeout(  
    function()
    {
  
   var oWnd = GetRadWindow();
 
 var childWindow=oWnd.BrowserWindow.radopen("frmFindReferralPhysician.aspx","MessageWindow");
 setRadWindowProperties(childWindow, 256, 930);
 childWindow.add_close(FindReferralClick)

 {
   function FindReferralClick(oWindow, args) 
    {
       var Result = args.get_argument();
       if (Result)
       {
 document.getElementById(GetClientId("txtPCPName")).value=Result.sPhyName;
document.getElementById(GetClientId("hdnRefToPhyID")).value==Result.ulPhyId;
   
 }
 
    }
    
 }
 },0);
 return false;
    }
     function OpenFindPhysicianForPCPName()
    {     
          setTimeout(  
    function()
    {
  
   var oWnd = GetRadWindow();
 var HumanId = document.getElementById(GetClientId('txtPatAccNo')).value;
 var childWindow=oWnd.BrowserWindow.radopen("frmFindReferralPhysician.aspx","MessageWindow");
 setRadWindowProperties(childWindow, 256, 930);
 childWindow.add_close(FindReferralPhysicianClick)
  childWindow.remove_close(SelectPlanClick);  
  childWindow.remove_close(FindPatientClick);
  childWindow.remove_close(FindPatientClick);
  childWindow.remove_close(FindPatientClick); 
 
 },0);
 return false;
    }
     function FindReferralPhysicianClick(oWindow, args) 
    {
       var Result = args.get_argument();
       if (Result)
       {
 document.getElementById(GetClientId("txtRefToPhy")).value=Result.sPhyName;
document.getElementById(GetClientId("hdnPCPID")).value=Result.ulPhyId;
   
 }
 
    }
     function OpenSelectPayer()
        {
        var result= window.showModalDialog("frmSelectPayer.aspx",null,"center:yes;resizable:no;scroll:no;dialogHeight:425px;dialogWidth:820px");
        if(result!=null)
        {
        document.getElementById(GetClientId('hdnInsPlanID')).value=result.PlanId;
        return false;
        }
        }

          function OpenPatientInsPolicyMain()
    {
        setTimeout(
    function () {
        var obj = new Array();
        var oWnd = GetRadWindow();
        var HumanId = document.getElementById(GetClientId("txtPatAccNo")).value;
        var childWindow = oWnd.BrowserWindow.radopen("frmPatientInsurancePolicyMaintenance.aspx?HumanId=" + HumanId + "&ParentScreen=" + "CaptureAuthorization", "CaptureAutnorizationWindow");
        setRadWindowProperties(childWindow, 680, 1170);
        childWindow.add_close(SelectPlanClick)
        childWindow.remove_close(FindReferralPhysicianClick);
        childWindow.remove_close(FindPatientClick);
        childWindow.remove_close(OpenFindRefPhy);
        childWindow.remove_close(OpenRefFromPhy);
    }, 0);
 return false;
    }
    function SelectPlanClick(oWindow, args) 
    {
       var Result = args.get_argument();
       if (Result)
       {
 document.getElementById(GetClientId("hdnPlanName")).value=Result.PlanName;
document.getElementById(GetClientId("hdnPolicyHolderId")).value=Result.PolicyHolderId;
document.getElementById(GetClientId("hdnInsurancePlanId")).value=Result.id;
document.getElementById("btnPlan").click();

 }
 
    }

    function OpenFindRefFromPhysician()
    {
        setTimeout(
    function () {

        var oWnd = GetRadWindow();
        var childWindow = oWnd.BrowserWindow.radopen("frmFindReferralPhysician.aspx", "CaptureAutnorizationWindow");
        setRadWindowProperties(childWindow, 256, 930);
        childWindow.add_close(OpenRefFromPhy)
        document.getElementById(GetClientId('btnSave1')).disabled = false;
        childWindow.remove_close(OpenFindRefPhy);
        childWindow.remove_close(SelectDiagnosis);
        childWindow.remove_close(SelectProcedures);
        childWindow.remove_close(SelectPlanClick);
    }, 0);
 return false;
    }
    function OpenRefFromPhy(oWindow, args) 
    {
       var result = args.get_argument();
       if (result)
       {
 document.getElementById(GetClientId("txtRefFromPhy")).value=result.sPhyName;
document.getElementById(GetClientId("hdnRefFromProvider")).value=result.ulPhyId;
document.getElementById(GetClientId("ddlRefSpecialty")).focus();
return;
   
 }
 
    }
    function OpenFindReferalPhysician()
    {
        setTimeout(
    function () {

        var oWnd = GetRadWindow();
        var childWindow = oWnd.BrowserWindow.radopen("frmFindReferralPhysician.aspx", "CaptureAutnorizationWindow");
        setRadWindowProperties(childWindow, 256, 930);
        childWindow.add_close(OpenFindRefPhy)
        document.getElementById(GetClientId('btnSave1')).disabled = false;
        childWindow.remove_close(OpenRefFromPhy);
        childWindow.remove_close(SelectDiagnosis);
        childWindow.remove_close(SelectProcedures);
        childWindow.remove_close(SelectPlanClick);
    }, 0);
 return false;
    }
        function OpenFindRefPhy(oWindow, args) 
    {
       var result = args.get_argument();
       if (result)
       {
 document.getElementById(GetClientId('txtRefToPhy')).value=result.sPhyName;
 document.getElementById(GetClientId('hdnRefPhyID')).value=result.ulPhyId;
   
 }
 
    }

       function showTime() { 
            var dt = new Date(); 
  var now = new Date(); 
  var then = now.getDay()+'-'+(now.getMonth()+1)+'-'+now.getFullYear(); 
      then += ' '+now.getHours()+':'+now.getMinutes()+':'+now.getSeconds(); 
      var utc=(now.getUTCMonth()+1)+'/'+now.getUTCDate()+'/'+now.getUTCFullYear();
utc+=' '+now.getUTCHours()+':'+now.getUTCMinutes()+':'+now.getUTCSeconds();
        document.getElementById(GetClientId("hdnDateAndTime")).value=utc;
        }
         function isNumericKey(e)
{
var charInp = window.event.keyCode; 
if (charInp > 31 && (charInp < 48 || charInp > 57) && (charInp!=46)) 
{
 return false;
 }
 document.getElementById(GetClientId('btnSave1')).disabled=false;
 document.getElementById(GetClientId('hdnCloseFlag')).value=true; 
    return true;
}
    function AutoSave() {
        document.getElementById(GetClientId('btnSave1'))._Enabled = true;
         document.getElementById(GetClientId('hdnSaveFlag')).value=true; 
        }

       function GetClientId(strid)
{var count=document.forms[0].length;var i=0;var eleName;for(i=0;i<count;i++)
{eleName=document.forms[0].elements[i].id;pos=eleName.indexOf(strid);if(pos>=0)break;}
return eleName;}
function OpenSelectPayer()
{var result=window.showModalDialog("frmSelectPayer.aspx",null,"center:yes;resizable:yes;scroll:yes;dialogHeight:460px;dialogWidth:820px");if(result == undefined)  { result=window.returnValue; } if(result!=null)
{document.getElementById("txtInsuranceName").value=result.planName;return true;}}
function OpenAuthRequest()
{
    if (document.getElementById(GetClientId('txtPatAccNo')).value=="")
    {
    
     DisplayErrorMessage('3045021');
    return false;
    }
    else
    {
    var obj = new Object(); 
    obj.PatientName = document.getElementById(GetClientId('txtPatientName')).value;
    obj.PatientDOB = document.getElementById(GetClientId('txtPatientDOB')).value;
    obj.HumanID = document.getElementById(GetClientId('txtPatAccNo')).value;
    obj.PCPID=document.getElementById(GetClientId('hdnPCPID')).value;
    obj.RefToPhyID=document.getElementById(GetClientId('hdnRefToPhyID')).value;
    obj.AuthID="0";
    obj.OpenMode="Create";
    document.getElementById(GetClientId('txtAuthNo')).readOnly=true;
     obj.PCP=document.getElementById(GetClientId('txtPCPName')).value;
      obj.PolicyHolderID=document.getElementById(GetClientId('hdnPolicyHolderID')).value;
   var PatientType=  document.getElementById(GetClientId("hdnPatientType")).value
   
    obj.EncID="0";
    window.dialogArgument=obj;      
   window.showModalDialog("frmCaptureAuthorization.aspx?PatientName="+obj.PatientName+"&ScreenName="+"AuthRequest"+"&PatientType="+PatientType+"&PatientDOB="+obj.PatientDOB+"&AccNo="+obj.HumanID+"&AuthID="+obj.AuthID+"&OpenMode="+obj.OpenMode+"&EncID="+obj.EncID+"&PCP="+obj.PCP+"&PolicyHolderID="+obj.PolicyHolderID,obj,"scroll:yes;center:yes;resizable:no;dialogHeight:680px;dialogWidth:1000px");
   if (window.dialogArgument!=null)
   {
   return true;
   }
   else
   {  
   return false;
   }
   }
    } 
    function Close()
	{
	DisplayErrorMessage('3045005');

	returnToParent(null);

	

	
	} 
	  function Update()
	{
	DisplayErrorMessage('3045024');

	returnToParent(null);

	

	
	}  
	function CloseWndw()
	{
	 self.close();
	}
	function OpenAuthResponse()
{
    if (document.getElementById(GetClientId('txtPatAccNo')).value=="")
    {
   
     DisplayErrorMessage('3045021');
    return false;
    }
    else
    {
    var obj = new Object(); 
    obj.PatientName = document.getElementById(GetClientId('txtPatientName')).value;
    obj.PatientDOB = document.getElementById(GetClientId('txtPatientDOB')).value;
    obj.HumanID = document.getElementById(GetClientId('txtPatAccNo')).value;
    obj.PCPID=document.getElementById(GetClientId('hdnPCPID')).value;
    obj.RefToPhyID=document.getElementById(GetClientId('hdnRefToPhyID')).value;
    obj.AuthID="0";
    obj.OpenMode="Create";
    document.getElementById(GetClientId('txtAuthNo')).readOnly=true;
   var PatientType=  document.getElementById(GetClientId("hdnPatientType")).value
    obj.PCP=document.getElementById(GetClientId('txtPCPName')).value;
      obj.PolicyHolderID=document.getElementById(GetClientId('hdnPolicyHolderID')).value;
   
    obj.EncID="0";
    window.dialogArgument=obj;      
   window.showModalDialog("frmCaptureAuthorization.aspx?PatientName="+obj.PatientName+"&ScreenName="+"AuthResponse"+"&PatientType="+PatientType+"&PatientDOB="+obj.PatientDOB+"&AccNo="+obj.HumanID+"&AuthID="+obj.AuthID+"&OpenMode="+obj.OpenMode+"&EncID="+obj.EncID+"&PCP="+obj.PCP+"&PolicyHolderID="+obj.PolicyHolderID,obj,"scroll:yes;center:yes;resizable:no;dialogHeight:680px;dialogWidth:1000px");
   if (window.dialogArgument!=null)
   {
   return true;
   }
   else
   {  
   return false;
   }
   }
    } 
    function CellSelected()
	{
	    if(DisplayErrorMessage('3045016')==true)
        {
         
        }
        else
        {
          
        }
	
	} 

            
   function change(btn)
{document.getElementById(GetClientId('btnSave1')).disabled=false;}         

function AllowNumbers(evt) {
if((document.getElementById(GetClientId("txtNoofVisitsReq")).value)!="")
{
if(((document.getElementById(GetClientId("txtNoofVisitsApp")).value)>(document.getElementById(GetClientId("txtNoofVisitsReq")).value)))
{

DisplayErrorMessage('3045023');
}
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57))
     {
        return false;
    }
    return true;
}
}


function OpenFindApptoAuth()
{

 
if (document.getElementById(GetClientId('hdnSelectedIndex')).value=="")
    {
   
    DisplayErrorMessage('3045022');
    return false;
    }


setTimeout(  
    function()
    {
   var obj = new Object(); 
    obj.AuthID = document.getElementById(GetClientId('hdnAuthID')).value;
    obj.PatientName = document.getElementById(GetClientId('txtPatientName')).value;
    obj.PatientDOB = document.getElementById(GetClientId('txtPatientDOB')).value;
    obj.HumanID = document.getElementById(GetClientId('txtPatAccNo')).value;
    obj.EncID=document.getElementById(GetClientId('hdnEncounterID')).value;
    obj.PCP=document.getElementById(GetClientId('txtPCPName')).value;
    obj.PolicyHolderID=document.getElementById(GetClientId('hdnPolicyHolderID')).value;
   var oWnd = GetRadWindow();
 var HumanId = document.getElementById(GetClientId('txtPatAccNo')).value;

 var childWindow=oWnd.BrowserWindow.radopen("frmFindAppointmentsToAuth.aspx?PatientName="+obj.PatientName+"&PatientDOB="+obj.PatientDOB+"&EncID="+obj.EncID+"&AccNo="+obj.HumanID+"&AuthID="+obj.AuthID+"&PCP="+obj.PCP+"&PolicyHolderID="+obj.PolicyHolderID,"MessageWindow");
setRadWindowProperties(childWindow,400,850);
 
 },0);
 return false;
    }
   
function CloseWindow()
{if(document.getElementById('hdnCloseFlag').value=="true")
{if(document.getElementById(GetClientId('btnSave1')).disabled= false);
{ window.alert("Do you want to Close Without saving the changes?")
{self.close();}}
}
else
{self.close();}}
   
function EnableSave()
{
   document.getElementById(GetClientId('btnSave1')).disabled= false;
} 
function OpenPopup(objevent)
    {
    if (event.keyCode == 120)
    { 
  
   document.onhelp = function() {return(false);
   }
   window.onhelp = function() {return(false);
   }

 setTimeout(  
    function()
    {
   var obj = new Object(); 
    
   var oWnd = GetRadWindow();


 var childWindow=oWnd.BrowserWindow.radopen("frmAllProcedures.aspx","MessageWindow");
setRadWindowProperties(childWindow,700,800);
childWindow.add_close(ClosePopUp)
 
 },0);
    }
    }
    function OpenPopupdiagnosis(objevent)
    {
    if (event.keyCode == 121)
    { 

     document.onhelp = function() {return(false);
   }
   window.onhelp = function() {return(false);
   }

 var result=new Object();
  var result= window.showModalDialog("frmSpecialityDiagnosis.aspx",null,"center:yes;title:no;resizable:no;dialogHeight:700px;dialogWidth:500px");
  window.returnValue= result;
 
   var someVariable =   document.getElementById(GetClientId('<%=Session["Selected_ICDs"]%>')).value;
  return true;

     
    }
    }
 function Check_Click(objRef)
{
    var row = objRef.parentNode.parentNode;
    var GridView = row.parentNode;
    var inputList = GridView.getElementsByTagName("input");
    for (var i=0;i<inputList.length;i++)
    {
        var headerCheckBox = inputList[0];
        var checked = true;
        if(inputList[i].type == "checkbox" && inputList[i] != headerCheckBox)
        {
            if(!inputList[i].checked)
            {
                checked = false;
                break; 
            }
        }
    }
    headerCheckBox.checked = checked;
    return true;
    
}
    function checkAll(objRef)
{
    var GridView = objRef.parentNode.parentNode.parentNode;
    var inputList = GridView.getElementsByTagName("input");
    for (var i=0;i<inputList.length;i++)

    {
        var row = inputList[i].parentNode.parentNode;
        if(inputList[i].type == "checkbox"  && objRef != inputList[i])
        {
            if (objRef.checked)
            {
                inputList[i].checked=true;
            }
            else
            {
                inputList[i].checked=false;
            }
        }
    }
     }
function IsAlphanumeric(evt)
{

var cha = String.fromCharCode(evt.keyCode || evt.charCode) 


if (cha.match(/[a-zA-Z0-9]/)) 
{ 
return true; 
} 
document.getElementById(GetClientId('btnSave1')).disabled=false;
document.getElementById(GetClientId('hdnCloseFlag')).value=true; 
    return true;
}


   function OpenAppointment()
    {  
    setTimeout(  
    function()
    {
   var obj = new Object(); 
    var oWnd = GetRadWindow();
  obj.PatientName = document.getElementById(GetClientId('txtPatientName')).value;
obj.PatientDOB = document.getElementById(GetClientId('txtPatientDOB')).value;
obj.HumanID = document.getElementById(GetClientId('txtPatAccNo')).value;   

 var childWindow=oWnd.BrowserWindow.radopen("frmFindAllAppointments.aspx?PatientName="+obj.PatientName+"&PatientDOB="+obj.PatientDOB+"&HumanID="+obj.HumanID,"CaptureAutnorizationWindow");
setRadWindowProperties(childWindow,625,910);
 childWindow.add_close(OpenSelectAppointment)
 },0);
 return false;

 }
  function OpenSelectAppointment(oWindow, args) 
    {
       var Result = args.get_argument();
       if (Result)
       {
  document.getElementById(GetClientId('hdnAppDate')).value= Result.App;
   document.getElementById(GetClientId("btnAppDate")).click();
 }
 
    }
 
   function OpenViewImage()
    {     
        var result= window.showModalDialog("frmViewImage.aspx",null,"center:yes;resizable:no;dialogHeight:600px;dialogWidth:1110px");
       if(result!=null)
       {       
      return true
       }
       else
       {
       return false;
       }
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
    function GetRadWindow() 
    {
        var oWindow = null;
        if (window.radWindow) oWindow = window.radWindow;
        else if (window.frameElement!=null&&window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
        return oWindow;
    }
    
    
    function returnToParent(args)
     {
            var oArg = new Object();
            oArg.result = args;
            var oWnd = GetRadWindow();
            if(oWnd!=null)                      
            {
            if (oArg.result)
            {
                 oWnd.close(oArg.result);
            }
            else 
            {
              
                oWnd.close(oArg.result);
            }
            }
            else
            {
            self.close();
            }
     }
         function ClosePopUp(oWindow, args) 
    {
       var result = args.get_argument();
       if (result)
       {
  document.getElementById(GetClientId('txtProcCode')).value=result.SelectedCPT.split('-')[0] ; 
 document.getElementById(GetClientId('btnSave1')).disabled= false;
 }
 
    }
     function OpenUploadDocuments()
        {
         var now=new Date();
         var utc=(now.getUTCMonth()+1)+'/'+now.getUTCDate()+'/'+now.getUTCFullYear();utc+=' '+now.getUTCHours()+':'+now.getUTCMinutes()+':'+now.getUTCSeconds();
         document.getElementById(GetClientId("hdnLocalTime")).value=utc;
         setTimeout(  
         function()
         {
          var oWnd = GetRadWindow();
          var childWindow=oWnd.BrowserWindow.radopen("frmOnlineDocuments.aspx?HumanId="+document.getElementById("txtPatAccNo").value+"&Screen=LocalDocuments"+"&Title=Upload Documents"+"&CurrentTime="+utc,"MessageWindow");
          setRadWindowProperties(childWindow,880,1200);
        },0);
        return false;
      }
    function OpenUpload()
    {

       var obj=new Array();
       obj.push("HumanId=" + document.getElementById(GetClientId("txtPatAccNo")).value);
       obj.push("Screen=Demographic");
       openNonModal("frmViewResult.aspx",1000,1200,obj); 
    }  
    function DisableChkNo()
    {
       document.getElementById(GetClientId('btnSave1')).disabled=  false; 
       document.getElementById(GetClientId('hdnSaveFlag')).value=true; 
       document.getElementById(GetClientId('ChkNo')).checked=false;
    }  
    function DisableChkYes()
    {
      document.getElementById(GetClientId('btnSave1')).disabled=  false; 
      document.getElementById(GetClientId('hdnSaveFlag')).value=true; 
      document.getElementById(GetClientId('ChkYes')).checked=false;
  }
  function AllDiagnosis() {
      setTimeout
      (function () {
          var oWnd = GetRadWindow();
          var childWindow = oWnd.BrowserWindow.radopen("frmSpecialityDiagnosis.aspx", "CaptureAutnorizationWindow");
          SetRadWindowProperties(childWindow, 560, 900);
          childWindow.add_close(SelectDiagnosis);
          childWindow.remove_close(OpenFindRefPhy);
          childWindow.remove_close(OpenRefFromPhy);
          childWindow.remove_close(SelectPlanClick);
          childWindow.remove_close(SelectProcedures);
      }, 0);
      return false;
  }
  function SelectDiagnosis(oWindow, args) {
      var Result = args.get_argument();
      if (Result != null) {
          document.getElementById(GetClientId("hdnICDType")).value = Result.medList;
          document.getElementById(GetClientId("btnDiagnosis")).click();
      }
  }
  function AllProcedures() {
      setTimeout
      (function () {
          var oWnd = GetRadWindow();
          var childWindow = oWnd.BrowserWindow.radopen("frmAllProcedures.aspx", "CaptureAutnorizationWindow");
          SetRadWindowProperties(childWindow, 560, 900);
          childWindow.add_close(SelectProcedures);
          childWindow.remove_close(OpenFindRefPhy);
          childWindow.remove_close(OpenRefFromPhy);
          childWindow.remove_close(SelectPlanClick);
          childWindow.remove_close(SelectDiagnosis);
      }, 0);
      return false;
  }
  function SelectProcedures(oWindow, args) {
      var Result = args.get_argument();
      if (Result != null) {
          document.getElementById(GetClientId("hdnProcedures")).value = Result.SelectedCPT;
          document.getElementById(GetClientId("btnProcedures")).click();
      }
  }