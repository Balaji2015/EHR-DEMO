<%@ Page Async="true" ValidateRequest="false"  Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="frmEFax.aspx.cs" Inherits="Acurus.Capella.UI.frmEFax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="aspx" %>
<%@ Register Src="~/UserControls/CustomDLCNew.ascx" TagName="DLC" TagPrefix="DLC" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<base target="_self" />
<head runat="server">
    <link href="CSS/font-awesome.css" rel="stylesheet" type="text/css" />
    <title>E-Fax</title>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
        .ui-autocomplete {
            -webkit-margin-before: 3px !important;
            max-height: 70px;
            overflow-y: auto;
        }

        .verticalscroll {
            overflow-x: hidden;
            overflow-y: auto;
            height: 150px;
        }

        .ui-autocomplete {
            position: absolute;
            cursor: default;
        }

        .ui-menu {
            list-style: none;
            padding: 10px;
            margin: 0;
            display: block;
            width: 600px !important;
        }

            .ui-menu .ui-menu {
                margin-top: -3px;
            }

            .ui-menu .ui-menu-item {
                margin: 0;
                padding: 0;
                width: 600px !important;
            }

                .ui-menu .ui-menu-item a {
                    text-decoration: none;
                    display: block;
                    padding: .2em .4em;
                    line-height: 1.5;
                    zoom: 1;
                }

                    .ui-menu .ui-menu-item a.ui-state-hover,
                    .ui-menu .ui-menu-item a.ui-state-active {
                        margin: -1px;
                    }

        .style4 {
            width: 30px;
        }

        .MultilineText {
            resize: none;
        }

        ::-webkit-scrollbar {
            width: 6px;
        }

        ::-webkit-scrollbar-track {
            background-color: #c3bfbf;
        }

        ::-webkit-scrollbar-thumb {
            background-color: #707070;
        }

            ::-webkit-scrollbar-thumb:hover {
                background-color: #3d3c3a;
            }

        .displayNone {
            display: none;
        }

        .modal {
            position: fixed;
            top: 0;
            left: 0;
            background-color: White;
            z-index: 99;
            opacity: 0.8;
            filter: alpha(opacity=80);
            -moz-opacity: 0.8;
            min-height: 100%;
            width: 100% !important;
        }

        .labelpadding {
            padding-left: 2%;
        }

        .underline {
            text-decoration: underline;
        }

        #pnlDLC {
            display: -webkit-inline-box !important;
        }

        .fa fa-question-circle .tooltip:hover .tooltiptext {
            visibility: visible;
        }

        .fa fa-question-circle .tooltip {
            position: relative;
            display: inline-block;
            border-bottom: 1px dotted black;
        }

        .tooltip {
            position: relative;
            display: inline-block;
        }

            .tooltip .tooltiptext {
                visibility: hidden;
                width: 200px !important;
                background-color: black;
                color: #fff;
                text-align: center;
                border-radius: 6px;
                padding: 5px 0;
                position: absolute;
                z-index: 1;
                top: -5px;
                left: 105%;
            }

        #tipforfindbtn:hover .tooltiptext {
            visibility: visible;
        }

        .ui-dialog-titlebar-close {
            display: none;
        }

        .ui-widget {
            font-family: Verdana,Arial,sans-serif !important;
        }

        .ui-dialog .ui-dialog-buttonpane .ui-dialog-buttonset {
            float: none !important;
            margin-left: 45px !important;
            font-size: 14px;
        }

        .ui-dialog .ui-dialog-buttonpane button {
            width: 60px !important;
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
            left: 430px !important;
        }

        .ui-dialog .ui-dialog-content {
            min-height: 0px !important;
        }

        .ui-dialog .ui-dialog-buttonpane {
            margin-top: -10px !important;
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

        .ui-autocomplete {
            max-height: 300px;
            overflow-y: auto;
            overflow-x: hidden;
        }

        .paddingtop{
            padding-top: 11px;
        }
    </style>
    <link href="CSS/jquery-ui.css" rel="stylesheet" />
    <link href="CSS/style.css" rel="stylesheet" type="text/css" />
    <link href="CSS/font-awesome.css" rel="stylesheet" type="text/css" />
    <link href="CSS/bootstrap.min.css" rel="stylesheet" />
    <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
</head>
<body bgcolor="#bfdbff" style="margin-top: 0px!important;" onload="LoadEfax();">
    <form id="form1" runat="server">
      
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">

            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
            </Scripts>
        </telerik:RadScriptManager>
        <div>

            <table width="100%">
                <tr>
                    <td>
                       
                          <div class="paddingtop">
                            <div class="panelborderbox">
                               <div class="panel-head Editabletxtbox">Sender's Information</div>
                                <div class="panel-body">                                
                                    <table width="100%" class="Editabletxtbox">

                                        <tr >
                                            <td style="width: 1%; position: absolute; left: 10px !important; padding-bottom: 0.5%">
                                                <span mand="Yes">Name*</span>
                                            </td>
                                            <td style="width: 40%; position: absolute; left: 110px !important; padding-bottom: 0.5% padding-bottom: 0.5%">
                                                <input name="txtSenderName" type="text" id="txtSenderName" runat="server" class="riSingle ui-autocomplete-input Editabletxtbox" maxlength="1000" onchange="EnableSend();" onkeypress="EnableSend();" style="width: 70%;" />

                                            </td>
                                            
                                             <td style="width: 1%; position: absolute; right: 500px !important; padding-bottom: 0.5%">
                                                <span id="lblCompany" class="Editabletxtbox">Company</span>
                                            </td>
                                            <td style="width: 40%;  text-align:left;  padding-bottom: 0.5%">
                                                <input name="txtSenderCompany" type="text" runat="server" id="txtSenderCompany" class="riSingle ui-autocomplete-input Editabletxtbox" onchange="EnableSend();" onkeypress="EnableSend();" style="width: 70%;" />
                                            </td>
                                        </tr>
                                        
                                        <tr >
                                            <td style="width: 1%; position: absolute; left: 10px !important; padding-bottom: 0.5%">
                                                <span id="lblFax" class="Editabletxtbox">Fax</span>
                                            </td>
                                            <td style="width: 40%; position: absolute; left: 110px !important; padding-bottom: 0.5%  padding-bottom: 0.5%">
                                                <input type="text" id="txtSenderMaskFax" runat="server" class="riSingle Editabletxtbox" maxlength="10" style="width: 70%;" />
                                            </td>
                                             <td style="width: 1%; position: absolute; right: 500px !important; padding-bottom: 0.5%">
                                                <span id="lblEmail" class="Editabletxtbox">Email</span>
                                            </td>
                                            <td style="width: 40%;  text-align:left;  padding-bottom: 0.5%">
                                                <input name="txtSenderEmail" type="text" runat="server" id="txtSenderEmail" class="riSingle ui-autocomplete-input Editabletxtbox" onchange="EnableSend();" maxlength="100" onkeypress="EnableSend();" style="width: 70%;" />
                                            </td>
                                        </tr>
                                       
                                    </table>
                               
                              </div>
                            </div>
                            <div class="panelborderbox">
                                <div class="panel-head Editabletxtbox">Recipient's Information</div>
                                <div class="panel-body">
                                    <table width="80%" class="Editabletxtbox">
                                        <tr >
                                            <td style="width: 10%;position: absolute; left: 10px !important; padding-bottom: 0.5%">
                                                <span mand="Yes">Category*</span>
                                            </td>
                                            <td style="width: 88%; padding-bottom: 0.5%" colspan="2">
                                                <input type="radio" name="chkProvider1" id="chkProvider" runat="server" checked value="Provider" /><label for="Provider">Provider/Organization/User/Non-User</label>
                                                <input type="radio" name="chkProvider1" id="chkpatient" runat="server" value="Patient" /><label for="Patient">Patient</label>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td style="width: 10%; position: absolute; left: 10px !important;padding-bottom: 0.5%">
                                                <span mand="Yes">Name*</span>
                                            </td>
                                            <td style="width: 65%; padding-bottom: 0.5%!important; height: 90px">
                                                <textarea name="txtProviderSearch" rows="3" cols="15" id="txtRecName" runat="server" onchange="EnableSend();" maxlength="1000" onkeypress="EnableSend();" class="form-control Editabletxtbox ui-autocomplete-input" data-phy-id="0" data-category="" data-phy-details="" style="width: 155%; resize: none;" autocomplete="off"></textarea>
                                                <img id="imgProviderSearch" runat="server" src="Resources/Delete-Blue.png" alt="X" title="Click to clear the text field." onclick="return ProviderSearchclear();" style="position: absolute; right: 95px !important; top: 280px !important; cursor: pointer; width: 10px; height: 10px;" />
                                            
                                            </td>
                                            <td style="width: 5%;position: absolute; right: 40px !important; top: 270px !important; ">
                                                <button id="btnprov" title="Address book" onclick="OpenProvider();return false;" class="btn"><i class="glyphicon glyphicon-book"></i></button>
                                            </td>
                                        </tr>
                                      <%-- <tr>
                                            <td style="width: 30%;position: absolute; left: 50px !important; padding-bottom: 0.5%">
                                                <span id="lblRecipientcompany" class="Editabletxtbox">Company</span>
                                            </td>
                                            <td style="width: 65%; padding-bottom: 0.5%">
                                                <input name="txtRecipientcompany" type="text" runat="server" id="txtRecipientcompany" class="riSingle ui-autocomplete-input Editabletxtbox" onchange="EnableSend();" onkeypress="EnableSend();" style="width: 115%;" />
                                            </td>
                                            <td style="width: 5%; text-align: left" rowspan="3" valign="middle"></td>
                                      </tr>  --%> 
                                        </table>
                                         <table width="100%" class="Editabletxtbox">
                                        <tr style="text-align:left">
                                            <td style="width: 1%; position: absolute; left: 10px !important; padding-bottom: 0.5%">
                                                <span id="lblRecipientcompany" class="Editabletxtbox">Company</span>
                                            </td>
                                            <td style="width: 30%; position: absolute; left: 110px !important; padding-bottom: 0.5%">
                                                <input name="txtRecipientcompany" type="text" runat="server" id="txtRecipientcompany" class="riSingle Editabletxtbox" onchange="EnableSend();" onkeypress="EnableSend();" style="width: 70%;" />
                                               
                                            </td>
                                             
                                            <td style="width: 1%; position: absolute; left: 350px !important; padding-bottom: 0.5%">
                                                <span mand="Yes">Fax*</span>
                                             </td>
                                            <td style="width: 30%;  position: absolute; right: 300px !important; padding-bottom: 0.5%">
                                                 <input type="text" id="msktxtRecipientFax" runat="server" class="riSingle Editabletxtbox" maxlength="10" style="width: 70%;" />
                                               
                                            </td>
                                            <td style="width: 5%; text-align: left" rowspan="3" valign="middle"></td>
                                        
                                             <td style="width: 1%; position: absolute; right: 350px !important; padding-bottom: 0.5%">
                                                <span id="lblRecipientemail" class="Editabletxtbox">Email</span>
                                            </td>
                                            <td style="width: 30%;  text-align:center;  padding-bottom: 0.5%">
                                                <input name="txtRecipientmail" type="text" id="txtRecipientmail" runat="server" class="riSingle Editabletxtbox" maxlength="100" onchange="EnableSend();" onkeypress="EnableSend();" style="width: 70%;" />
                                              
                                            </td>
                                             <td style="width: 5%; padding-bottom: 0.5%;">
                                                   <button id="btnNew" runat="server" style="margin-left:-30px;" class="btn aspresizedbluebutton" onclick="if(!btnNewAddGrid()){return false;};" onserverclick="btnNew_Click">New</button>
                                             </td>

                                        </tr>
                                        </table>
                                         <table width="100%" class="Editabletxtbox">
                                        <tr>
                                            <td style="width: 100%; height: 100%; padding-bottom: 0.5%" colspan="3">
                                                <div id="divgrd" style="overflow-y: auto; height:130px;">
                                                    <table id="tblFaxDetails" class='table table-bordered' style="width: 97%;">
                                                        <thead class="Gridheaderstyle" style="table-layout:fixed;">
                                                            <tr>
                                                                <th style="width: 5%;">Del.</th>
                                                                <th style="width: 15%;">Category</th>
                                                                <th style="width: 15%;">Name</th>
                                                                <th style="width: 28%;">Company</th>
                                                                <th style="width: 10%;">Fax</th>
                                                                <th style="width: 25%;">EMail</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody class="Editabletxtbox"></tbody>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                  </div>
                            </div>
                       </div>
                        <div style="padding-left: 2%; padding-right: 2%;">
                            <table width="100%">
                                <tr>
                                    <td style="">
                                        <span class="LabelStyle Editabletxtbox">Priority</span>
                                    </td>
                                    <td style="padding-right: 10px">
                                        <select style="width: 250px;" class=" riSingle Editabletxtbox" name="DropDwnpriority" runat="server" id="DropDwnpriority">
                                        </select>
                                    </td>
                                    <td>
                                        <span class="LabelStyle Editabletxtbox">Cover page</span>
                                    </td>
                                    <td>
                                        <select style="width: 250px;" class=" riSingle Editabletxtbox" runat="server" id="DropDwncoverpage">
                                        </select>
                                   <a href="#" onclick="ImageTempalteClick()" style="margin-right: -22%">View Template</a></td>
                                </tr>
                                <tr>
                                    <td><span class="LabelStyle">Subject</span></td>
                                    <td colspan="4" style="padding-top: 8px;">
                                        <input type="text" id="txtSubject" maxlength="115" runat="server" style="width: 95%;" class="riSingle Editabletxtbox" />
                                    </td>
                                </tr>

                            </table>
                        </div>
                        <div style="padding-left: 2%; padding-right: 2%; padding-bottom: 1%">
                            <span class="lblcover Editabletxtbox">Cover page Message</span>
                            <%--<DLC:DLC ID="txtCoverpage" runat="server" TextboxHeight="360px" TextboxWidth="913px" Value="EFAX_COVER_PAGE_MESSAGE" TextboxMaxLength="3000"></DLC:DLC>--%>
                        <textarea id="txtareaCoverpage" maxlength="3000" runat="server" style="height:360px;width:913px; resize: none; " class="Editabletxtbox" cols="100" rows="100" ></textarea>
                            </div>


                    </td>
                </tr>
                <tr>
                    <td>
                        <div style="height: 200px; width: 100%; overflow: auto;">
                            <table style="width: 100%">
                                 <tr>
                                    
                                     <td align="right">
                                        <a href="#" onclick="ActivityHistoryClick()" style="display:none;">Activity Log</a>
                                        <input type="submit" name="btnSendfax" runat="server" value="Send" onclick="return ValidationSendFax();" onserverclick="btnSendfax_Click" style="margin-top: -3px; margin-right: 0px;" id="btnSendfax" class="aspgreenbutton" />
                                        <input type="submit" name="btnCancel" value="Cancel" onclick="Closefax();" id="btnCancel" class="aspredbutton" style="margin-top: -2px; margin-right: 57px;" />
                                    </td>
                                </tr>
                                 <tr>
                                    <td>
                                        <span id="lblattachfile" runat="server" class="Editabletxtbox">Attachments <i class="icon-ok-sign" style="color: blue; ">(File Formats supported:*.TIFF , *.PDF , *.PNG , *.JPG , *.GIF)</i></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span id="lblattach" runat="server" class="Editabletxtbox"></span>
                                    </td>
                                </tr>
                                 <tr>
                                    <td>
                                        <div id="divFileUpload" style="height: 70px; overflow-y: auto; width: 930px;"> 
                                            <telerik:RadAsyncUpload ID="UploadImage" runat="server" Width="775px"
                                                MultipleFileSelection="Automatic" InitialFileInputsCount="0"
                                                UploadedFilesRendering="BelowFileInput" OnClientValidationFailed="fileUploadValidationFailed" 
                                                OnClientFileUploaded="onFileUpload" OnClientFilesUploaded="OnFilesUploaded" OnClientProgressUpdating="OnProgressUpdating"
                                                OnClientFileUploadRemoved="OnFileUploadRemoved">
                                                <Localization Select="Browse" />
                                                <FileFilters>
                                                    <telerik:FileFilter Description="Documents(pdf;tif;tiff;png;jpg;jpeg;gif)" Extensions="pdf,tif,tiff,png,jpg,jpeg,gif" />
                                                </FileFilters>
                                            </telerik:RadAsyncUpload>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span id="lblNotes" class="MandLabelstyle">Notes : Total attachments size should be less than 10MB</span>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>

                </tr>

               
            </table>
        </div>

        <asp:HiddenField ID="hdnpriority" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnGroupID" runat="server" Value="0" />
        <asp:HiddenField ID="hdnavoidPostBack" runat="server" Value="false" />
        <asp:HiddenField ID="hdnAddgrid" runat="server" Value="" />
          <asp:HiddenField ID="hdnfilePath" runat="server"  />
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <asp:HiddenField ID="hdnAttachfile" runat="server" EnableViewState="false" />

            <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
            <script src="JScripts/jquery-ui.min1.11.4.js" type="text/javascript"></script>
            <link href="CSS/jquery-ui.css" rel="stylesheet" />
            <script src="JScripts/JSMaskedinput.js" type="text/javascript"></script>
               
            <script src="JScripts/bootstrap.min.js" type="text/javascript"></script>
            <script src="JScripts/JSEfax.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSCustomDLC.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSCustomPhrases.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSMask.Min.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript" enableviewstate="False"></script>
        </asp:PlaceHolder>

    </form>
</body>
</html>
