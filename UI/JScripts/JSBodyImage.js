//Atalasoft.Utils.InitClientScript(MyPageLoad);
function mouseDown(n) { try { if (2 == event.button || 3 == event.button) return !1 } catch (n) { if (3 == n.which) return !1 } } document.oncontextmenu = function () { return !1 }, document.onmousedown = mouseDown;
function EnableSave() { $find('btnSave').set_enabled(true); window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true"; localStorage.setItem("bSave", "false");}
function MyPageLoad() { WebAnnotationViewer1.AnnotationClicked = OnAnnotationClicked; WebAnnotationViewer1.AnnotationChanged = OnAnnotationChanged;}
function OnAnnotationChanged(e){EnableSave();};
function OnAnnotationClicked(e){EnableSave();
var clickedAnno = e.annotation;
switch( clickedAnno.getType() ){
case "TextData":{if(e.annotation.getName() != "DefaultTextAnnotation" && document.getElementById('hdnCallout').value !=null && document.getElementById('hdnCallout').value != "Callout"+e.annotation.getName())
{var a = new Array();a.push(e.annotation.getWidth());a.push(e.annotation.getHeight());a.push(e.annotation.getPosition().X);
a.push(e.annotation.getPosition().Y);document.getElementById('hdnCallout').value="Callout"+e.annotation.getName();
var btnload= document.getElementById('btnLoadList');btnload.click();break;}
else{break;}}}
}
function IndexChanged(){{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}}
function CreateAnnotation(type, name){EnableSave();WebAnnotationViewer1.CreateAnnotation(type, name);}
function Delete()
 {var anns = WebAnnotationViewer1.getSelectedAnnotations();if(anns.length > 0){WebAnnotationViewer1.DeleteAnnotations(anns);}EnableSave();}
  function Burn()
  {
       WebAnnotationViewer1.RemoteInvoked = RefreshBurnedImage;
       WebAnnotationViewer1.RemoteInvoke('RemoteBurnImage');
  }
 
  function RefreshBurnedImage()
   {
      WebAnnotationViewer1.RemoteInvoked = function () { };
      var frame = WebAnnotationViewer1.getCurrentLayer();
      WebThumbnailViewer2.UpdateThumb(frame);
   }
   function RefreshThumbs()
   {
   WebAnnotationViewer1.RemoteInvoked = function () {var frame = WebAnnotationViewer1.getLayers();
   for(var i=0;i<frame.length;i++)
   {
   WebThumbnailViewer2.UpdateThumb(frame[i]);
   }};
   }

function btnClearAll_Clicked(sender,args)
	{
var obj=new Array();
obj.push("ScreenName=BodyImage");
var result = openModal("frmValidationArea.aspx",20,300,obj,"SelectOption");
var WindowName = $find('SelectOption');
WindowName.add_close(OnClientClose); 
return false;
}
function OnClientClose(oWindow, args)
{  
var arg = args.get_argument();
if (arg)
{ var result =arg;
if(result=="This Image")    
{ { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
WebAnnotationViewer1.RemoteInvoked = function () {WebThumbnailViewer2.Update(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}};
WebAnnotationViewer1.RemoteInvoke('ClearCurrentAnnotations');
$find('btnSave').set_enabled(true)
}
if(result == "All Images")
{ { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
WebAnnotationViewer1.RemoteInvoked = function () {WebThumbnailViewer2.Update(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}};
WebAnnotationViewer1.RemoteInvoke('ClearAnnotations');
$find('btnSave').set_enabled(true);
}
if(result == "Cancel")
{ }
}
}

function Html()
{
var div = $("<div id='DivId' runat='server'/>");
var select = $("<select id='lstId' runat='server'/>");
select.appendTo(div);
var test= document.getElementById("hdnLstData").value;
var lst =  test.split(",");
 for (var i = 0; i < lst.length; i++)
 {
 var option = $("<option />");
 option.text(lst[i]);
 option.val(i+1);
 option.appendTo(select);
 }

div.css({"position": "absolute", 
"left": "300px",
"top": "10px", 
"width": "1",
"height": "1",
"background-color": "white",
"z-index": "10"});

div.appendTo($("body"));
select.change(function () {
getValue();
});
}


function getValue()
{
var current_text;
var current_annotation;
var annos = WebAnnotationViewer1.getCurrentLayer().getAnnotations();
for (var j = 0; j < annos.length; j++)
 {
      if( annos[j].getType() == "TextData")
      {
            current_text = annos[j].getEditorObject().value;
            current_annotation =annos[j];
      }
  }

  var x= document.getElementById("lstId");
  for (var i = 0; i < x.options.length; i++) 
  {
     if(x.options[i].selected ==true)
      {
        current_annotation.ShowEditor();
        if(current_text == "")
            current_annotation.getEditorObject().value = x.options[i].text;
        else
            current_annotation.getEditorObject().value = current_text+', '+x.options[i].text;
        break;
      }
       
   }
    
}

function btnSave_Clicked(sender,args)
{
//Burn();
var splitvalues = window.parent.parent.theForm.hdnTabClick.value.split('$#$');
$find('btnSave').set_enabled(false);
localStorage.setItem("bSave", "true");
{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
var annos = WebAnnotationViewer1.getAnnotations(); //Get all the annotations
annos.forEach(function(anno, index) { anno.HideEditor(); }); //Make sure they aren't in edit mode.
ifInCallback(function() { 
WebAnnotationViewer1.RemoteInvoked = function () {DisplayErrorMessage('7540003');  {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}};
WebAnnotationViewer1.RemoteInvoke('SaveAnnotations');
WebAnnotationViewer1.CallBackReturned($find('btnSave').set_enabled(false));
});
}
function Saved_Changes() {
    window.parent.theForm.hdnIsSaveEnableExam.value = "false";
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
    var which_tab = window.parent.parent.theForm.hdnTabClick.value.split('$#$')[0];
    var screen_name;
    if (which_tab.indexOf('btn') > -1) {
        screen_name = 'MoveToButtonsClick';
    }
    else if (which_tab == 'first') {
        screen_name = '';
    }
    else if (which_tab != "first" && which_tab != "CC / HPI" && which_tab != "QUESTIONNAIRE" && which_tab != "PFSH" && which_tab != "ROS" && which_tab != "VITALS" && which_tab != "EXAM" && which_tab != "TEST" && which_tab != "ASSESSMENT" && which_tab != "ORDERS" && which_tab != "eRx" && which_tab != "SERV./PROC. CODES" && which_tab != "PLAN" && which_tab != "SUMMARY")
        screen_name = "ExamTabClick";
    else
        screen_name = "EncounterTabClick";
    SavedSuccessfully_NowProceed(screen_name);
    DisplayErrorMessage('7540003');

}
var _currentFrame = 0;

 function OnPageChanged()
    {
        var layers = WebAnnotationViewer1.getLayers();
	    WebAnnotationViewer1.DeselectAll();
	    
	    for (var i = 0; i < layers.length; ++i) {
		    var v = 'hidden';
		    if (i == _currentFrame) {
			    v = 'visible';
			    WebAnnotationViewer1.setCurrentLayer(i);
		    }
		    layers[i].setVisibility(v);
	    }
    }


    function Invalidate()
    {
        WebAnnotationViewer1.Update();
	    OnPageChanged();
    }
    
    
function LoadCopyPrevious()
{
    var EncounterType = document.getElementById('hdnPageBox');
    if(EncounterType.value == 'Exist') {     
        var copy_previous=DisplayErrorMessage('7540005');
        if (copy_previous == true) {
            top.window.document.getElementById('ctl00_C5POBody_hdnIsSaveEnable').value = "true";
            var btndelete = document.getElementById('btnIvLoadAnnot'); 
            btndelete.click();
        }
	    else {
	        args._cancel = true;
        }
}
else{var btndelete1=document.getElementById('btnIvLoadAnnot'); btndelete1.click();}
}


function BodyImage_Load() {
    window.parent.parent.theForm.hdnSaveButtonID.value = "btnSave,RadMultiPage1";
     {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
}    

function OnClientCloseValidation(oWindow, args) {
document.getElementById("HdnCopyButton").value="";
    var button2 = $find('btnSave');
    var btnCopyCheck = $find('btnCopyPrevious');
    var arg = args.get_argument();
    if (arg) {
        var result = arg;
        if (result == "Yes") {
            document.getElementById("HdnCopyButton").value = "trueValidate";
            testCopy = "true";
            button2.click();
            document.getElementById("btnCopyPrevious").click();
        } else if (result == "Cancel") {


        } else {
        
            document.getElementById("HdnCopyButton").value = "ClickNo";
            button2.set_enabled(false);
            document.getElementById("btnCopyPrevious").click();
        }
    }
}
function btnCopyCC_Clicked(sender, args) {



if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value == "true" && localStorage.getItem("bSave") == "false") {
        event.preventDefault();
        event.stopPropagation();
        dvdialog = window.parent.parent.parent.parent.document.getElementsByTagName('div').namedItem('dvdialog');
        $(dvdialog).dialog({
            modal: true,
            title: "Capella -EHR",
            position: {
                my: 'left' + " " + 'center',
                at: 'center' + " " + 'center + 100px'

            },
            buttons: {
                "Yes": function () {
                    document.getElementById('btnSave').click();
                    localStorage.setItem("bSave", "true");
                    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value == "false";


{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
WebAnnotationViewer1.RemoteInvoked = function () {var Status = WebAnnotationViewer1.getReturnValue();
if(Status == "False")  
{DisplayErrorMessage('210010');  {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();} return false;}
else{
 {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();} EnableSave();}
};
WebAnnotationViewer1.RemoteInvoke('CopyPrev');
WebAnnotationViewer1.CallBackReturned($find('btnSave').set_enabled(false)); 
                $(dvdialog).dialog("close");
                    return;
                },
                "No": function () {
                    localStorage.setItem("bSave", "true");
                    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value == "false";
                   

{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
WebAnnotationViewer1.RemoteInvoked = function () {var Status = WebAnnotationViewer1.getReturnValue();
if(Status == "False")  
{DisplayErrorMessage('210010');   {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();} return false;}
else{
 {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}  EnableSave();}
};
WebAnnotationViewer1.RemoteInvoke('CopyPrev');


                    $(dvdialog).dialog("close");

                },
                "Cancel": function () {
                    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                    $(dvdialog).dialog("close");
                    localStorage.setItem("bSave", "true");
                    return;

                }
            }
        });
    }
    else {


{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
WebAnnotationViewer1.RemoteInvoked = function () {var Status = WebAnnotationViewer1.getReturnValue();
if(Status == "False")  
{DisplayErrorMessage('210010');;  {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();} return false;}
else{
 {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}  EnableSave();}
};
WebAnnotationViewer1.RemoteInvoke('CopyPrev');
}

}

function ifInCallback(doSomethingLater)
{ if(!WebAnnotationViewer1.inCallBack())
{ doSomethingLater(); } 
else  { setTimeout(function() { ifInCallback(doSomethingLater); }, 100); /*Wait 1/10 of a second and check again. */ } }
function notApplicable(){ DisplayErrorMessage('8007');document.getElementById("pnlBodyImage").style.display = 'none';}