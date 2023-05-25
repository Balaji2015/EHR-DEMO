//Atalasoft.Utils.InitClientScript(myPageLoad);

function myPageLoad() {
   
    WebThumbnailViewer1.UrlChanged = OnThumbnailUrlChanged;
    WebThumbnailViewer1.ThumbnailDropServer = OnThumbnailsReordered;
    WebThumbnailViewer1.ThumbnailClicked = OnThumbnailClicked;
    WebThumbnailViewer1.ThumbnailDropServer = OnThumbnailDroppedServer;
    WebThumbnailViewer2.ThumbnailClicked = OnThumbnail2Clicked;
    WebThumbnailViewer1.KeyDown = OnKeyDown;
    WebThumbnailViewer1.Focus();
    var count = WebThumbnailViewer1.getCount();
    var label = '/ ' + count.toString();
    $('#PageLabel').text(label);
    $('#PageBox').val(1);
    var FileCount = WebThumbnailViewer2.getCount();
    var FileCountlabel = '/ ' + FileCount.toString();
    $('#FileCount').text(FileCountlabel);
    if(WebThumbnailViewer2.getSelectedIndex()!=0)
    $('#FilePageBox').val(WebThumbnailViewer2.getSelectedIndex()+1);
    else
        $('#FilePageBox').val(1);
}

function AppendStatus(msg) {
    $('#status').append('<p>' + msg + '</p>');
}

function OnThumbnailUrlChanged() {

    var count = WebThumbnailViewer1.getCount();
    var label = '/ ' + count.toString();
    $('#PageLabel').text(label);
}

function OnThumbnailsReordered(e) {
    WebThumbnailViewer1.SelectThumb(e.dropIndex);
    $('#PageBox').val(e.dropIndex + 1);
}

function OnThumbnailClicked() {
    var index = WebThumbnailViewer1.getSelectedIndex();
    $('#PageBox').val(index + 1);
    document.getElementById('hdnPageBox').value= index + 1;
}


function OnThumbnailDroppedServer(e) {
    DragDropLayers(e.dragIndex, e.dropIndex);
}


function DragDropLayers(startIndex, destIndex) {
    WebAnnotationViewer1.RemoteInvoked = DragDropLayersCallBack;
    WebAnnotationViewer1.RemoteInvoke('Remote_DragDropLayers', [startIndex, destIndex]);
}

function DragDropLayersCallBack() {
    WebAnnotationViewer1.RemoteInvoked = function () { };
    WebThumbnailViewer1.SelectThumb(parseInt(WebAnnotationViewer1.getReturnValue()));
}

function OnThumbnail2Clicked(args)
{

if( document.getElementById('hdnfilename')!=null && document.getElementById('hdnfilename').value!="")
{
  try
  { 
     var index = args.index+1;
     var test= document.getElementById('hdnfilename').value;
     var temp_name = test.split("%");
     $('#PageBox').val(1);
      WebThumbnailViewer1.OpenUrl(temp_name[args.index]);
      WebThumbnailViewer1.SelectedIndex = 0;
      if (temp_name[args.index].indexOf(".dcm") !=-1) 
      {
      document.getElementById('hdnDicomFile').value = temp_name[args.index];
      var btnload=document.getElementById('btnLoadDicom');
	  btnload.click(); 
	  }
     else
     {
      WebThumbnailViewer1.OpenUrl(temp_name[args.index]);
      WebThumbnailViewer1.SelectedIndex = 0;
      WebThumbnailViewer1.SelectThumb(0);
      $('#PageBox').val(1);
      WebAnnotationViewer1.RemoteInvoke('alignTree');
      var btnhidetree=document.getElementById('btnallignTree');
     }
  }
  catch(e)
  {
  //do nothing
  }
}
}

function OnRefresh()
{
  $('#PageBox').val(document.getElementById('hdnPageBox').value);
}



function OnKeyDown(e)
{
  if(WebThumbnailViewer1.getCount()!=0)
  {
    var myKeyCode = e.keyCode;
    var n = WebThumbnailViewer1.getSelectedIndex();
    var c = WebThumbnailViewer1.getCount();
    if (myKeyCode == 38)
    {
        n = (n - 1 >= 0) ? n - 1 : 0;
        WebThumbnailViewer1.SelectThumb(n);
        var index = n;
        $('#PageBox').val(index + 1);
        document.getElementById('hdnPageBox').value= index + 1;
        return false;
    }
    else if (myKeyCode == 40)
    { 
    n = (n + 1 < c) ? n + 1 : c - 1;
    WebThumbnailViewer1.SelectThumb(n);
    var index = n;
    $('#PageBox').val(index + 1);
    document.getElementById('hdnPageBox').value= index + 1;
     return false;
    }
  }
}