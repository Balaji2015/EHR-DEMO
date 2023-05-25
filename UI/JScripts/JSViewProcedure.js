//Atalasoft.Utils.InitClientScript(MyPageLoad);
function MyPageLoad(){WebAnnotationViewer1.AnnotationClicked = OnAnnotationClicked;WebAnnotationViewer1.AnnotationChanged=OnAnnotationChanged;}
function OnAnnotationChanged(e){};
function OnAnnotationClicked(e){};
         
    function CreateAnnotation(type, name) {
        WebAnnotationViewer1.CreateAnnotation(type, name);
    }

    function Burn() {

        WebAnnotationViewer1.RemoteInvoked = RefreshBurnedImage;
        WebAnnotationViewer1.RemoteInvoke('RemoteBurnImage');
    }

    function Delete() {
        var anns = WebAnnotationViewer1.getSelectedAnnotations();
        WebAnnotationViewer1.DeleteAnnotations(anns);

    }

    function RefreshBurnedImage() {
        WebAnnotationViewer1.RemoteInvoked = function () { };

        var frame = WebThumbnailViewer1.getSelectedIndex();
        WebThumbnailViewer1.UpdateThumb(frame);
    }
          
   
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
  
  function AutoSave()
    {
            var Result=new Object();
            Result.IsEnabled="TRUE";
            window.returnValue=Result;
            self.close();
    }
         function GetRadWindow()
        {
                    var oWindow = null;
                    if (window.radWindow)
                        oWindow = window.radWindow;
                    else if (window.frameElement.radWindow)
                        oWindow = window.frameElement.radWindow;
                    return oWindow;
        }

        
         function returnToParent()
          {
                     var Result=new Object();
                     if(!document.getElementById('btnAdd').disable)
                     Result.IsEnabled="TRUE";
                     else
                      Result.IsEnabled="false";
                      
                     var oWnd = GetRadWindow();
                     oWnd.close(Result);
          }