<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmImageViewer.aspx.cs" Inherits="Acurus.Capella.PatientPortal.frmImageViewer" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Image Viewer</title>
    <link href="https://maxcdn.bootstrapcdn.com/font-awesome/4.5.0/css/font-awesome.min.css" rel="stylesheet" />
    <link href="CSS/ScanningAndIndexing.css" rel="stylesheet" />
        <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
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
</head>
<body onload=" {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="smPopup" runat="server"></asp:ScriptManager>
        <table style="width: 100%; height: 100%">
            <tr id="trselect" runat="server" visible="false">
                <td colspan="4" align="left" style="width: 90%;">
                    <div style="margin-top: 0px;" id="Div2" runat="server">
                        <span class="spanstyle">Select Image to View: </span>

                        <asp:DropDownList CssClass="spanstyle" ID="DropDownimagelist" runat="server" Width="82%" OnSelectedIndexChanged="DropDownimagelist_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </div>
                </td>

            </tr>
            <tr style="height: 100%">
                <td style="width: 30%; height: 610px; overflow-y: hidden;">
                    <div runat="server" id="_plcImgsThumbs" style="height: 585px; overflow-y: scroll;"></div>
                    <br />
                </td>
                <td style="width: 70%; height: 100%;">
                    <div style="margin-left: 90px" id="imgControls">
                        <i class="fa fa-chevron-left" id="prev" title="Previous Image" style="cursor: pointer;"></i>
                        <input name="PageBox" type="text" id="PageBox" runat="server" style="width: 25px; height: 12px; background-color: lightgray;" readonly="readonly" />
                        <input name="PageLabel" type="text" id="PageLabel" runat="server" style="width: 25px; height: 12px; margin-left: -5px; background-color: lightgray;" readonly="readonly" />
                        <i class="fa fa-chevron-right" id="next" style="cursor: pointer;" title="Next Image"></i>
                        <div id="divrotate" runat="server" style="display:inline-block">
                            <i class="fa fa-rotate-left" style="margin-left: 15px; cursor: pointer;" id="leftrotate" title="Rotate Left"></i>&nbsp;&nbsp;
                                        <i class="fa fa-rotate-right" style="margin-left: 15px; cursor: pointer;" id="rotateright" title="Rotate Right"></i>&nbsp;&nbsp;                                                                        
                                        <i class="fa fa-search-plus" id="zoomin" style="margin-left: 15px; cursor: pointer;" title="Zoom in"></i>&nbsp;&nbsp;
                                        <i class="fa fa-search-minus" id="zoomout" style="margin-left: 15px; cursor: pointer;" title="Zoom out"></i>&nbsp;&nbsp;
                                        <i class="fa fa-picture-o" id="revert" style="margin-left: 15px; cursor: pointer;" title="Revert to original"></i>&nbsp;&nbsp;
                     
                                               <i class="fa fa-print" id="Print" style="margin-left: 15px; cursor: pointer;" title="Print" onclick="print();"></i>&nbsp;&nbsp;
                        </div>
                    </div>
                    <div style="margin-top: 0px; height: 585px; width: 1000px; border: 1px solid #6A9C73; overflow: auto;" id="imgholder" runat="server">
                        <img id="_imgBig" runat="server" alt="Uploaded File Not available in the location, it may be deleted or corrupted.Please Contact Support." />
                    </div>
                    <div style="margin-top: 0px; height: 630px; width: 1000px; border: 1px solid #6A9C73; overflow: auto; display: none" id="PDFholder" runat="server">
                        <iframe id="bigImgPDF" runat="server" style="width: 100%; height: 100%; overflow: hidden;" frameborder="0"></iframe>
                    </div>

                </td>
            </tr>

            <tr id="trmesshistory" runat="server" visible="false">

                <td>
                    <asp:Label ID="Label1" runat="server" Text="Message" CssClass="spanstyle"></asp:Label>
                </td>
                <td colspan="2" style="width: 100%;">
                    <asp:TextBox TextMode="MultiLine" ID="txtmessage" runat="server" Style="width: 99%; height: 60px; resize: none;" onkeypress="enablesave()" CssClass="spanstyle"></asp:TextBox>
                </td>


            </tr>
            <tr id="trmessage" runat="server" visible="false">

                <td>
                    <asp:Label ID="Label2" runat="server" Text="Message History"></asp:Label>
                </td>
                <td colspan="2" style="width: 100%;">
                    <asp:TextBox TextMode="MultiLine" ID="txtmsghistory" runat="server" Style=" width: 99%; height: 60px; resize: none;" ReadOnly="true" CssClass="nonEditabletxtbox"></asp:TextBox>
                </td>


            </tr>
            <tr id="trbuttons" runat="server" visible="false">

                <td style="width: 75%;"></td>

                <td colspan="2" align="right">
                    <asp:Button ID="btnsave" runat="server" Text="Save" Enabled="false"  CssClass="aspresizedgreenbutton"/>
                    <asp:Button ID="btnViewerclose" runat="server" Text="Close" OnClientClick="closepopup();" CssClass="aspresizedredbutton" />
                    <asp:Button ID="btnhidden" runat="server" Style="display: none" OnClick="btnhidden_click" />

                  
                    
                    <%--<asp:Button ID="btnhiddenEfax" runat="server" Style="display: none" OnClick="btnhiddenEFax_click" />--%>
                </td>
            </tr>
          
        </table>
        <asp:HiddenField ID="hdnPageBox" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnnotes" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnfileindexid" runat="server" EnableViewState="false" />
      
        <asp:HiddenField ID="hdnid" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnpdf" runat="server" EnableViewState="false" />

        <asp:HiddenField ID="hdnpdfnotes" runat="server" EnableViewState="false" />
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
              <asp:Button ID="btnhiddenfax" style="display:none;" runat="server"  OnClick="btnhiddenfax_click" />
            <!--Certain Files are marked as static files, no need to implement the VersionConfiguration Technology in the pages-->
            <script src="JScripts/jquery-1.11.3.min.js" type="text/javascript"></script>
            <script src="JScripts/jquery-ui.min1.10.2.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/bootstrap.min.js" type="text/javascript"></script>
            <link href="CSS/jquery-ui.css" rel="Stylesheet" />
            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script type="text/javascript" src="JScripts/Lazyload.js"></script>
            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript" defer="defer"></script>
            <script src="JScripts/JSImageViewer.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
        </asp:PlaceHolder>

    </form>

</body>
</html>




