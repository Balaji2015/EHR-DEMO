<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmImageCompare.aspx.cs" Inherits="Acurus.Capella.UI.frmImageCompare" validateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Image Viewer</title>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <link href="CSS/font-awesome.min4.5.0.css" rel="stylesheet" />
    <link href="CSS/ScanningAndIndexing.css" rel="stylesheet" />
     <style>
.ui-dialog-titlebar-close {
            display: none;
        }

        .ui-widget {
            font-family: Verdana,Arial,sans-serif !important;
        }

        .ui-dialog .ui-dialog-buttonpane .ui-dialog-buttonset {
            float: none !important;
            margin-left: 45px !important;
        }

        .ui-dialog .ui-dialog-buttonpane button {
            width: 70px !important;
        }

        .ui-dialog .ui-dialog-titlebar {
            padding: 0px !important;
        }

        .ui-dialog .ui-dialog-title {
            font-size: 12px !important;
            font-family: Verdana,Arial,sans-serif !important;
        }

        .ui-dialog ui-widget ui-widget-content ui-corner-all ui-front ui-dialog-buttons ui-draggable ui-resizable {
            height: 155px;
            border: 2px solid;
            border-radius: 13px;
            top: 504px !important;
            left: 568px !important;
        }

        .ui-dialog .ui-dialog-content {
            min-height: 0px !important;
        }

        .ui-dialog .ui-dialog-buttonpane {
            margin-top: -10px !important;
            /*padding: 0px !important ;*/
        }

        .ui-widget-content {
            border: 0px !important;
        }

        .ui-widget-header, .ui-state-default, ui-button {
          
            font-size: 11px !important;
            font-family: sans-serif;
        }


        .ui-widget {
            border: 1px solid #adadad !important;
            background-color: #F7F7F7;
        }
    </style>
        <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
</head>
<body  onunload=" {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}"">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="smPopup" runat="server"></asp:ScriptManager>
        <table style="width: 100%; height: 100%">
          
            <tr>
                  
                <td colspan ="3" style="width:100%">
                      <span>Select Image 1:</span>
                         <asp:DropDownList ID="DropDownimagelist" runat="server" Width="60%"  onchange="return DropDownimagelistchange()" OnSelectedIndexChanged="DropDownimagelist_SelectedIndexChanged"  >
                             
                         </asp:DropDownList>
                      <asp:Label ID="lbldays" runat="server" Text="" style="width: 100%; height: 24px; color: red; vertical-align: middle; padding-top: 2px; margin-top: -6px; position: relative; padding-left:20px; border: 0px !important" CssClass="spanstyle"></asp:Label>

                </td>
                <td  align="left" style="width: 90%;">
                     <div style="margin-top: 0px;padding-left:0%" id="Div1" runat="server">
                         <span class="spanstyle">Select Image 2:</span>
                         <asp:DropDownList  class=" spanstyle" ID="droplistfile" runat="server" Width="77%"  onchange="return filechange()" OnSelectedIndexChanged="droplistfile_SelectedIndexChanged"  >
                             
                         </asp:DropDownList>
                     </div>
                </td>
                </tr>
          
            <tr style="height: 100%">
                <td style="width: 10%; height: 610px; overflow-y: hidden;">
                    <div runat="server" id="_plcImgsThumbs" style="height: 585px;width:75px; overflow-y: scroll;"></div>
                    <br />
                </td>
                <td style="width: 40%; height: 100%;">
                    
                    <div style="margin-left: 90px" id="imgControls">
                        <i class="fa fa-chevron-left" id="prevActual" title="Previous Image" style="cursor: pointer;"></i>
                        <input name="PageBox" type="text" id="PageBox" runat="server" style="width: 25px; height: 12px; background-color: lightgray;" readonly="readonly" />
                        <input name="PageLabel" type="text" id="PageLabel" runat="server" style="width: 25px; height: 12px; margin-left: -5px; background-color: lightgray;" readonly="readonly" />
                        <i class="fa fa-chevron-right" id="nextActual" style="cursor: pointer;" title="Next Image"></i>
                        
                        
                        <div id="divrotate" runat="server" style="display:inline-block">
                        <i class="fa fa-rotate-left" style="margin-left: 15px; cursor: pointer;" id="leftrotateActual" title="Rotate Left"></i>&nbsp;&nbsp;
                                        <i class="fa fa-rotate-right" style="margin-left: 15px; cursor: pointer;" id="rotaterightActual" title="Rotate Right"></i>&nbsp;&nbsp;                                                                        
                                        <i class="fa fa-search-plus" id="zoominActual" style="margin-left: 15px; cursor: pointer;" title="Zoom in"></i>&nbsp;&nbsp;
                                        <i class="fa fa-search-minus" id="zoomoutActual" style="margin-left: 15px; cursor: pointer;" title="Zoom out"></i>&nbsp;&nbsp;
                                        <i class="fa fa-picture-o" id="revertActual" style="margin-left: 15px; cursor: pointer;" title="Revert to original"></i>&nbsp;&nbsp;
                                        <i class="fa fa-print" id="Print" style="margin-left: 15px; cursor: pointer;" title="Print" onclick="printActual();"></i>&nbsp;&nbsp;
                   
                            </div>
                             </div>
                    <div style="margin-top: 0px; height: 600px; width: 575px; border: 1px solid #6A9C73; overflow: auto;" id="imgholder" runat="server">
                        <img id="_imgBigActual" runat="server"  style="height: 600px; width: 575px;object-fit:contain;"  />
                    </div>
                    <div style="margin-top: 0px; height: 600px; width: 575px; border: 1px solid #6A9C73; overflow: auto; display: none" id="PDFholder" runat="server">
                        <iframe id="bigImgPDF" runat="server"  frameborder="0"  style="height: 600px; width: 575px;"></iframe>
                    </div>
                    
                </td>

                 <td style="width: 10%; height: 610px; overflow-y: hidden;">
                    <div runat="server" id="_plcImgsThumbsComp" style="height: 585px;width:80px; overflow-y: scroll;"></div>
                    <br />
                </td>
                <td style="width: 40%; height: 100%;">
                    
                    <div style="margin-left: 90px;" id="imgControlscom">
                        <i class="fa fa-chevron-left" id="prevcom" title="Previous Image" style="cursor: pointer;"></i>
                        <input name="PageBox" type="text" id="PageBoxcom" runat="server" style="width: 25px; height: 12px; background-color: lightgray;" readonly="readonly" />
                        <input name="PageLabel" type="text" id="PageLabelcom" runat="server" style="width: 25px; height: 12px; margin-left: -5px; background-color: lightgray;" readonly="readonly" />
                        <i class="fa fa-chevron-right" id="nextcom" style="cursor: pointer;" title="Next Image"></i>
                        
                         <div id="divrotatecomp" runat="server" style="display:inline-block">
                        <i class="fa fa-rotate-left" style="margin-left: 15px; cursor: pointer;" id="leftrotatecom" title="Rotate Left"></i>&nbsp;&nbsp;
                                        <i class="fa fa-rotate-right" style="margin-left: 15px; cursor: pointer;" id="rotaterightcom" title="Rotate Right"></i>&nbsp;&nbsp;                                                                        
                                        <i class="fa fa-search-plus" id="zoomincom" style="margin-left: 15px; cursor: pointer;" title="Zoom in"></i>&nbsp;&nbsp;
                                        <i class="fa fa-search-minus" id="zoomoutcom" style="margin-left: 15px; cursor: pointer;" title="Zoom out"></i>&nbsp;&nbsp;
                                        <i class="fa fa-picture-o" id="revertcom" style="margin-left: 15px; cursor: pointer;" title="Revert to original"></i>&nbsp;&nbsp;
                                        <i class="fa fa-print" id="Printcom" style="margin-left: 15px; cursor: pointer;" title="Print" onclick="printCompare();"></i>&nbsp;&nbsp;
                    </div>
                        </div>
                    <div style="margin-top: 0px; height: 600px; width: 575px; border: 1px solid #6A9C73; overflow: auto;" id="imgholdercom" runat="server">
                        <img id="_imgBigCompare" runat="server"  style="height: 600px; width: 575px;object-fit:contain;"/>
                    </div>
                    <div style="margin-top: 0px; height: 600px; width: 575px; border: 1px solid #6A9C73; overflow: auto;display: none" id="PDFholderComp" runat="server">
                        <iframe id="bigImgPDFCompare" runat="server" style="height: 600px; width: 575px; overflow: hidden;" frameborder="0"></iframe>
                    </div>
                    
                </td>

            </tr>
           
              <tr id="trmesshistory" runat="server" >
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Message" CssClass="spanstyle"></asp:Label>
                </td>
                <td  style="width: 100%;">
                    <asp:TextBox TextMode="MultiLine"  class="spanstyle" ID="txtmessage" runat="server" Style="width: 95%; height: 60px; resize: none;" onkeypress="enablesave();" ></asp:TextBox>
                </td>
                   <td>
                    <asp:Label ID="Label4" runat="server" Text="Message" CssClass="spanstyle"></asp:Label>
                </td>
                <td  style="width: 100%;">
                    <asp:TextBox TextMode="MultiLine" ID="txtmessagecomp" CssClass="spanstyle" Enabled="false" runat="server" Style="width: 99%; height: 60px; resize: none;" onkeypress="enablesave()" ></asp:TextBox>
                </td>
            </tr>
             <tr id="trmessage" runat="server" >
                <td>
                    <asp:Label ID="Label2" runat="server" Text="Message History" CssClass="spanstyle"></asp:Label>
                </td>
                <td style="width: 100%;">
                    <asp:TextBox TextMode="MultiLine" ID="txtmsghistory" runat="server" Style="width: 95%; height: 60px; resize: none;" ReadOnly="true" CssClass="nonEditabletxtbox"></asp:TextBox>
                </td>
                 <td >
                    <asp:Label ID="Label3" runat="server" Text="Message History"></asp:Label>
                </td>
                <td  style="width: 100%;">
                    <asp:TextBox TextMode="MultiLine" ID="txtmsghistorycomp"   runat="server" Style=" width: 99%; height: 60px; resize: none;" ReadOnly="true" CssClass="nonEditabletxtbox"></asp:TextBox>
                </td>
            </tr>
              <tr id="trbuttons" runat="server" >
              
                  <td colspan="4" style="width:100%;" align="right">
                        <asp:Button ID="btnsave" runat="server" Text="Save"  Enabled="false" OnClientClick="return btnsaveClickCompare();" CssClass="aspresizedgreenbutton"/>
                      <asp:Button ID="btnViewerclose" runat="server" Text="Close"  OnClientClick="closeComparepopup();"  CssClass="aspresizedredbutton"/>
                   <asp:Button ID="btnhidden" runat="server" OnClick="btnhidden_click" style="display:none" />
                         <asp:Button ID="btnhiddencompare" runat="server" OnClick="btnhiddencompare_click" style="display:none" />
                  </td>
            </tr>
        </table>
        <asp:HiddenField ID="hdnPageBox" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnPageBoxCompare" runat="server" EnableViewState="false" />
          <asp:HiddenField ID="hdnnotes" runat="server" EnableViewState="false" />
           <asp:HiddenField ID="hdnnotesCompare" runat="server" EnableViewState="false" />
         <asp:HiddenField ID="hdnfileindexid" runat="server" EnableViewState="false" />
           <asp:HiddenField ID="hdnfileindexidCompare" runat="server" EnableViewState="false" />
          <asp:HiddenField ID="hdnfilepath" runat="server" EnableViewState="false" />

         <asp:HiddenField ID="hdnimagesource" runat="server" EnableViewState="false" />
         <asp:HiddenField ID="hdnpgno" runat="server" EnableViewState="false" />
         <asp:HiddenField ID="hdnmsghst" runat="server" EnableViewState="false" />
         <asp:HiddenField ID="hdnmessage" runat="server" EnableViewState="false" />


         <asp:HiddenField ID="hdnimagesource1" runat="server" EnableViewState="false" />
         <asp:HiddenField ID="hdnpgno1" runat="server" EnableViewState="false" />
         <asp:HiddenField ID="hdnmsghst1" runat="server" EnableViewState="false" />
         <asp:HiddenField ID="hdnmessage1" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnpdf" runat="server" />
          <asp:HiddenField ID="hdnid" runat="server" />
         <asp:HiddenField ID="hdidcompare" runat="server" />
          <asp:HiddenField ID="hdnpdfcompare" runat="server" />
        <asp:HiddenField ID="hdnpdfnotes" runat="server" />
 
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <!--Certain Files are marked as static files, no need to implement the VersionConfiguration Technology in the pages-->
           <script src="JScripts/jquery-1.11.3.min.js" type="text/javascript"></script>
            <script src="JScripts/jquery-ui.min1.10.2.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/bootstrap.min.js" type="text/javascript"></script>
    <link href="CSS/jquery-ui.css" rel="Stylesheet" />
    <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script type="text/javascript" src="JScripts/Lazyload.js"></script>
            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript" defer="defer"></script>
            <script src="JScripts/jsimageExam.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
        </asp:PlaceHolder>

    </form>

</body>
</html>




