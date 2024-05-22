<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="webfrmBulkAccess.aspx.cs" Inherits="Acurus.Capella.PatientPortal.webfrmBulkAccess"  %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Bulk Access</title>
    <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
    <link href="CSS/ScanningAndIndexing.css" rel="stylesheet" />
    <link href="CSS/font-awesome.css" rel="stylesheet" />
    <style type="text/css">
        body {
            font-family: 'Microsoft Sans Serif' !important;
            font-size: 12px !important;
        }

        .zeromargin {
            margin: 0px !important;
        }

        .pheading {
            font-size: 12.3px;
            padding-left: 2px;
            font-weight: bold;
            font-family: microsoft sans serif;
        }
    </style>

</head>
<body>
    <form id="formBulkAccess" runat="server" name="BulkAccess" >
        <telerik:RadWindowManager ID="radWindowMngr" runat="server" EnableViewState="false">
            <Windows>
                <telerik:RadWindow ID="radWindow" runat="server" Behaviors="Close" ></telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <telerik:RadScriptManager ID="radscriptManger" runat="server"></telerik:RadScriptManager>
        <div style="width: 100%; height: 100%;">
            <div style="height: 20%; border-radius: 2px; border: 1px solid #524e4e; background-color: #bfdbff;">
                <div class="panel panel-success" style="width: 100%; clear: none;margin-bottom:0px;">
                    <div class="panel panel-heading" style="font-size: 13px;height: 19px;font-weight: bold;margin-bottom: 0px;">Select Date Of Service range</div>
                    <div class="panel panel-body" style="margin-bottom: 0px;padding: 4px;">
                       <table>
                           <tr>
                                <td><p class="zeromargin">From Date :</p></td>
                                <td><input type="text" id="dtpFromDt" runat="server"  style="width:100px;"/></td>
                               <td style="width:2%;"></td>
                                <td><p class="zeromargin">To Date :</p></td>
                                <td><input type="text" id="dtpToDt" runat="server"  style="width:100px;"/></td>
                               <td style="width:5%;"></td>
                                <td><asp:Button Text="Generate" runat="server" ID="btnGenerate" OnClick="btnGenerate_Click" OnClientClick=" return btn_generateClick();" /></td>
                                <td></td>
                           </tr>
                       </table>
                    </div>
                </div>
                <%--<table style="height: 55px;">
                    <tbody>
                        <tr>
                            <td colspan="6">
                                <p class="zeromargin pheading">Select Date Of Service range</p>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <p class="zeromargin">From Date :</p>
                            </td>
                            <td>
                                <input type="text" id="dtpFromDt" runat="server" /></td>
                            <td>
                                <p class="zeromargin">To Date :</p>
                            </td>
                            <td>
                                <input type="text" id="dtpToDt" runat="server" /></td>
                            <td>
                                <asp:Button Text="Generate" runat="server" ID="btnGenerate" OnClick="btnGenerate_Click" /></td>
                            <td></td>
                        </tr>
                    </tbody>
                </table>--%>
            </div>
            <div style="width: 100%; display: block; padding-top: 10px;">
                <div class="panel panel-success" style="width: 40%; float: left; clear: none; display: inline-block;">
                    <div class="panel panel-heading">Files List</div>
                    <div id="fileThumbs" runat="server" style="height: 525px; width: 100%; margin-top: -9px; overflow-y: scroll;">
                        <asp:CheckBoxList ID="SummaryList" runat="server"></asp:CheckBoxList>
                    </div>
                </div>
                <div class="panel panel-success" style="width: 59%; height: 525px; float: left; margin-left: 5px; clear: none; display: inline-block;">
                    <div class="panel-heading">Preview</div>
                    <%--<div id="bigImage" runat="server" style="height: 500px; width: 100%; border: 1px solid #6A9C73; overflow: auto;">
                        <img id="bigImg" runat="server" style="display: block; margin-left: auto; margin-right: auto; height: 500px;" src="" alt="Scanned Pages" />
                    </div>--%>
                    <div id="bigImagePDF" runat="server" style="height: 500px; width: 100%; border: 1px solid #6A9C73;">
                        <iframe id="bigImgPDF" runat="server" style="width: 100%; height: 500px; overflow: auto;" frameborder="0"></iframe>
                    </div>
                    <table style="width: 100%">
                        <tr style="height: 50%;float:right;">
                            <td style="width: 10%">
                                <input type="button" runat="server" id="btnSend" class="btn btn-primary" value="Send"  onclick="btnSend_ClientClick();" /><%--onserverclick="btnSend_ServerClick"--%>
                            </td>

                            <td style="width: 10%">
                                <input type="button" runat="server" id="btnDownload" class="btn btn-primary" value="Download" onclick="btnDownloadClick();" /></td>
                            <td style="width: 5%">
                                <input type="button" runat="server" id="btnClose" value="Close" class="btn btn-danger" onclick="btnClose_Clicked();"/></td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hdnFilesList" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnfromdate" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdntodate" runat="server" EnableViewState="false" />
        <asp:PlaceHolder ID="placeholderBulkAccess" runat="server">
            <script src="JScripts/jquery-2.2.3.js" type="text/javascript"></script>
            <script src="JScripts/bootstrap.min.js" type="text/javascript"></script>
            <script src="JScripts/jquery-ui.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/jquery.datetimepicker.js" type="text/javascript"></script>
            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
             <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/jsBulkAccess.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
        </asp:PlaceHolder>
    </form>
</body>
</html>
