<%@ Page  Async="true" Language="C#" AutoEventWireup="True" CodeBehind="frmOnlineDocuments.aspx.cs" Inherits="Acurus.Capella.PatientPortal.frmOnlineDocuments" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE  html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Online Documents</title>
    <link href="CSS/ScanningAndIndexing.css" rel="stylesheet" />
   <%-- <link href="CSS/font-awesome.css" rel="stylesheet" />--%>
    <base target="_self" />
      <script src="JScripts/jquery-1.11.3.min.js"></script>
    <script src="JScripts/jquery-ui.min1.10.2.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>"></script>
    <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.4.0/css/font-awesome.css" rel="stylesheet" />
</head>
<body style="margin: 0px; padding: 0px;" onload=" {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}">
    <form id="form1" runat="server">
           <telerik:RadWindowManager EnableViewState="false" Overlay="true" ID="WindowMngr"
            runat="server">
            <Windows>
                <telerik:RadWindow ID="MessageWindow" Overlay="true" ShowContentDuringLoad="true"
                    runat="server" Behaviors="Close" Title="Change Password" IconUrl="Resources/16_16.ico">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <div id="divLoading" class="modal" runat="server" style="text-align: center; display: none">
            <asp:Panel ID="Panel2" runat="server">
                <br />
                <br />
                <br />
                <br />
                <center>
                    <asp:Label ID="Label1" Text="" runat="server" EnableViewState="false"></asp:Label></center>
                <br />
                <img src="Resources/loadimage.gif" height="30px" width="30px" title="[Please wait while the page is loading...]" alt="Loading..." />
                <br />
            </asp:Panel>
        </div>
        <div style="width: 1165px;">
            <asp:Panel ID="pnlScanDoc" runat="server" GroupingText="Scanned Documents" Width="100%">
                <table style="margin-top: -7px; font-size: medium;">
                    <tr>
                        <td></td>
                        <td style="width: 30%;">
                            <div style="height=20px;">
                                <span>Scanned Date*</span><input type="text" id="dtpScannedDate" runat="server" readonly="readonly" />&nbsp;&nbsp;&nbsp;&nbsp;
                            </div>
                        </td>
                        <td style="width: 20%;">
                            <asp:CheckBox ID="chckShowOldFiles" runat="server" Text="Show Past Dates" Checked="false" onchange="enableDates(this.firstChild.checked);" />&nbsp;<i id="tipforcheckbox" class="fa fa-question-circle tooltip"><span class="tooltiptext">For faster search of scanned files with in two months ,do not select this option</span></i>
                        </td>
                        <td align="right" style="width: 25%;"><span>Facility</span>&nbsp;<input type="text" runat="server" id="txtSelectedFacility" disabled="disabled" />
                        </td>
                        <td align="right" style="width: 25%;">
                            <input type="button" id="btnFindDocuments" runat="server" class="btn btn-primary btn-sm" value="Find Documents" onserverclick="btnFindDocuments_Click" onclick="findDocuments();" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlBrowseDoc" runat="server" GroupingText="Browse Documents" Width="100%">
                <table style="margin-top: -7px; font-size: medium;width:100%">
                    <tr>
                        <td style="height: 20px;">
                            <table>
                                <tr>
                                     <td style="width: 10%;"> <asp:FileUpload ID="fileupload" runat="server" onchange="clickUpload();" /> 
                                         <input type="button" id="btnUpload" style="display: none;" runat="server" class="btn btn-primary btn-sm" value="Upload" onserverclick="btnUpload_ServerClick" />
                                     </td>
                                    <td style="width:10%;">
                                        <asp:RegularExpressionValidator ID="uplValidator" runat="server" ControlToValidate="fileupload" 
                                ErrorMessage="Allowed only these Extension files(*.tif,*.png,*.jpeg,*.pdf,*.bmp,*.jpg...)" ValidationExpression="(.+\.([Pp][Nn][Gg])|.+\.([Pp][Dd][Ff])|.+\.([Jj][Pp][Ee][Gg])|.+\.([Bb][Mm][Pp])|.+\.([Jj][Pp][Gg])|.+\.([Tt][Ii][Ff]))"></asp:RegularExpressionValidator></td>        
                                    <td style="width: 15%;" runat="server"> <span id="DocumentType" runat="server" class="mandatory" style="display:none">Document Type*</span></td>
                                    <td style="width: 15%;" runat="server"> <asp:DropDownList ID="cboDocumentType" runat="server" style="display:none" AutoPostBack="true" EnableViewState="true" onchange="changeDocumentsType()" OnSelectedIndexChanged="cboDocumentType_SelectedIndexChanged"></asp:DropDownList></td>
                                    <td style="width: 20%;" runat="server"> <span id="SubDocumentType" runat="server" class="mandatory" style="display:none">Document Sub Type*</span></td>
                                    <td style="width: 20%;" runat="server"> <asp:DropDownList ID="cboDocumentSubType" runat="server" style="display:none;width:100%"></asp:DropDownList></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <div style="width: 1165px; display: block; padding-top: 10px;">
                <div class="panel panel-success" style="width: 450px; float: left; clear: none;">
                    <div class="panel panel-heading">Files List [Supported Formats: tif, jpeg, png, jpg, bmp, pdf]</div>
                    <div id="fileThumbs" runat="server" style="height: 525px; width: 450px; margin-top: 4px; overflow-y: scroll;">
                    </div>
                </div>
                <div class="panel panel-success" style="width: 702px; height: 525px; float: left; clear: none; margin-left: 9px;">
                    <div class="panel-heading">Image Viewer</div>
                    <div id="bigImage" runat="server" style="height: 500px; width: 700px; border: 1px solid #6A9C73; overflow: auto;">
                        <img id="bigImg" runat="server" style="display: block; margin-left: auto; margin-right: auto; height: 500px;" src="" alt="Scanned Pages" />
                    </div>
                    <div id="bigImagePDF" runat="server" style="height: 500px; width: 700px; border: 1px solid #6A9C73;">
                        <iframe id="bigImgPDF" runat="server" style="width: 700px; height: 500px; overflow: auto;" frameborder="0"></iframe>
                    </div>
                    <table width="100%">
                        <tr style="height: 50%;">
                            <td width="15%">&nbsp;</td>
                            <td width="5%"><i class="fa fa-search-plus" id="zoomin" style="margin-left: 15px; margin-top: 15px; cursor: pointer;" title="Zoom in"></i>&nbsp;&nbsp;</td>
                            <td width="5%"><i class="fa fa-search-minus" id="zoomout" style="margin-left: 15px; margin-top: 15px; cursor: pointer;" title="Zoom out"></i>&nbsp;&nbsp;</td>
                            <td width="5%"><i class="fa fa-chevron-left" id="prev" style="margin-left: 15px; cursor: pointer;" title="Previous Image"></i></td>
                            <td width="5%">
                                <input name="currentPage" type="text" id="currentPage" runat="server" style="width: 25px;" /></td>
                            <td width="5%">
                                <input name="totalPages" type="text" id="totalPages" runat="server" style="width: 25px;" /></td>
                            <td width="5%"><i class="fa fa-chevron-right" id="next" style="margin-left: 8px; cursor: pointer;" title="Next Image"></i></td>
                            <td width="5%"><i class="fa fa-rotate-left" style="margin-left: 8px; margin-top: 15px; cursor: pointer;" id="leftrotate" title="Rotate Left"></i>&nbsp;&nbsp;</td>
                            <td width="5%"><i class="fa fa-rotate-right" style="margin-left: 8px; margin-top: 15px; cursor: pointer;" id="rightrotate" title="Rotate Right"></i>&nbsp;&nbsp;</td>
                            <td width="10%">
                                <asp:Button ID="btnSaveOnline" CssClass="btn btn-success btn-sm" runat="server" Text="Save" Style="height: 33px; width: 90%;" Visible="false" Enabled="false" OnClick="btnSaveOnline_Click" OnClientClick="return btnsaveclientclick();"/>

                            </td>
                            <td width="10%">
                                <input type="button" runat="server" id="btnIndex" class="btn btn-success" value="Index" onclick="redirectbySeparateWindow();" />
                            </td>

                            <td width="10%">
                                <input type="button" runat="server" id="btnClearAll" class="btn btn-primary" value="Clear All" onserverclick="btnClearAll_ServerClick" onclick="ResetScanDate();" /></td>
                            <td width="5%">
                                <input type="button" runat="server" id="btnClose" value="Close" class="btn btn-danger" onserverclick="btnClose_ServerClick" /></td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hdnNo" runat="server" EnableViewState="false" />
           <asp:HiddenField ID="hdnfilepath" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnFiles" runat="server" EnableViewState="false" />
         <asp:HiddenField ID="hdnSelecteFile" runat="server" EnableViewState="false" />
         <telerik:RadScriptManager ID="RadScriptManager1" runat="server" EnableViewState="false">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
            </Scripts>
        </telerik:RadScriptManager>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <!--Certain Files are marked as static files, no need to implement the VersionConfiguration Technology in the pages-->
            <script src="JScripts/jquery.datetimepicker.js" type="text/javascript"></script>
            <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript" defer="defer"></script>
            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSOnlineDocuments.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="Jscripts/bootstrap.min.js"></script>
            <script type="text/javascript" src="JScripts/Lazyload.js"></script>
        </asp:PlaceHolder>
    </form>
</body>
</html>
