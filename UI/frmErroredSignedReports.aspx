<%@ Page Async="true" Language="C#" AutoEventWireup="True" CodeBehind="frmErroredSignedReports.aspx.cs" Inherits="Acurus.Capella.UI.frmErroredSignedReports" EnableViewState="true" EnableEventValidation="false" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>View Errored Signed Reports</title>
    <base target="_self" />
    <link href="CSS/ScanningAndIndexing.css" rel="stylesheet" />
    <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <link href="CSS/font-awesome.4.4.0.css" rel="stylesheet" />

    <style>
        .divhighlight:hover {
            font-weight: bold;
            cursor: pointer;
        }

        .panelborderboxIndexing {
            border-color: #bfdbff !important;
            margin-bottom: 20px;
            background-color: #fff;
            border: 1px solid transparent;
            border-radius: 4px !important;
        }

        .panel-headingdisable {
            color: black !important;
            background-color: #e3e3e3 !important;
            border-color: #bfdbff !important;
            font-family: "Helvetica Neue",Helvetica,Arial,sans-serif !important;
            border-radius: 8px !important;
        }

        .panel-headingIndexing {
            color: black !important;
            background-color: #bfdbff !important;
            border-color: #bfdbff !important;
            font-family: "Helvetica Neue",Helvetica,Arial,sans-serif !important;
            border-radius: 8px !important;
        }

        #CheckAlert {
            display: none;
            background: #fdfeff;
            box-shadow: 0 0 10px rgba(0,0,0,0.4);
            box-sizing: border-box;
            color: #101010;
            left: 50%;
            min-width: 645px;
            max-width: 700px;
            padding: 1.875em;
            position: absolute;
            top: 7%;
            transform: translate(-50%, -50%);
            z-index: 2000000000;
            border-radius: 10px;
            opacity: 0.8;
        }
        .date-sort{
            cursor: pointer;
            }
        .date-sort:after{
            top: 2%;
            content: "▲" / "";
            font-size: 10px;
            position: absolute;
            right: 51.5%;
            opacity:0.4;
        }
        .date-sort:before{
            top: 3.4%;
            content: "▼" / "";
            font-size: 10px;
            position: absolute;
            right: 51.5%;
            opacity:0.4;
        }
    </style>
</head>

<body style="margin-top: -25px; font-size: 14px!important;" onloadstart="pageLoad();">
    <form id="frmIndexing" runat="server">
        <telerik:RadStyleSheetManager ID="SSTMngr" runat="server" EnableStyleSheetCombine="true" OutputCompression="Forced"></telerik:RadStyleSheetManager>
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" EnableScriptCombine="true">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
            </Scripts>
        </telerik:RadScriptManager>

        <telerik:RadWindowManager ID="WindowMngr" runat="server">
            <Windows>
                <telerik:RadWindow ID="MessageWindow" runat="server" Behaviors="Close" Title="DLC" IconUrl="Resources/16_16.ico">
                </telerik:RadWindow>
                <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close" Title="DLC" IconUrl="Resources/16_16.ico">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <%--<asp:ToolkitScriptManager ID="ToolkitScriptManager2" runat="server">
</asp:ToolkitScriptManager>--%>
        <%--<div id="dMyscanMessage"  style="display:none;height: 647px; margin-top: 130px;margin-left: 310px;width: 1145px;font-size: 20px !important;align-content: center;">
    <i class="fa fa-check-circle" style="color:green;font-size:165px;margin-left:155px"></i><br />
         <div style="margin-left: 115px;color:green;"> File uploaded Sucessfully.</div><br />
    <div style="margin-left: 68px;"> Please close the screen to continue.</div>
</div>--%>
        <div id="dOverall" style="height: 647px; width: 1145px; font-size: 14px!important;">
            <div class="blended_grid" style="width:1180px">
                <div style="margin-left: 3px;">
                    <asp:UpdatePanel ID="updateImages" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="panel panelborderbox" style="height: 620px; width: 575px; float: left; clear: none;margin-bottom: 10px !important;">
                                <%--<div class="divgroupstyle" style="height: 21px!important">Errored Signed Report(s)</div>--%>
                                <div id="fileThumbs" runat="server" style="height: 597px; width: 575px; overflow-y: auto;" enableviewstate="true">
                                    <table id="tblFiles" class="table table-bordered Gridbodystyle" style="width: 99.7%">
                                        <tr>
                                            <th class="table-th-style">Del.</th>
                                            <th class="table-th-style">File Name</th>
                                            <th class="table-th-style" id="divCreatedDateAndTime" data-sort-order="DESC" style="cursor: pointer;">Created Date and Time</th>
                                        </tr>
                                        <tbody id="tbFilesBody" class="Gridbodystyle">
                                        </tbody>
                                    </table>
                                </div>
                                <div class="divgroupstyle" style="height: 21px!important">No. of File(s): <label id="lblFileCount">0</label> </div>
                            </div>
                            <div class="panel panelborderbox" style="height: 620px; width: 575px; float: left; clear: none; margin-left: 10px;margin-bottom: 10px !important;">
                                <div class="divgroupstyle" style="height: 21px!important">Preview</div>
                                <table id="imgControls" style="width: 86%; margin-left: 70px;">
                                    <tr>
                                        <td style="width: 5%;"><i class="fa fa-rotate-left" style="cursor: pointer;" id="leftrotate" title="Rotate Left"></i></td>
                                        <td style="width: 5%;"><i class="fa fa-rotate-right" style="cursor: pointer;" id="rotateright" title="Rotate Right"></i></td>
                                        <td style="width: 5%;"><i class="fa fa-chevron-left" id="prev" title="Previous Image" style="cursor: pointer;" runat="server"></i></td>
                                        <td style="width: 5%;">
                                            <input name="PageBox" type="text" id="PageBox" runat="server" style="width: 18px; height: 15px;" readonly="readonly" /></td>
                                        <td style="width: 5%;">
                                            <label id="PageLabel" runat="server" style="height: 10px;">/ 0</label></td>
                                        <td style="width: 5%;"><i class="fa fa-chevron-right" id="next" style="cursor: pointer;" title="Next Image" runat="server"></i></td>
                                        <td style="width: 5%;"><i class="fa fa-search-plus" id="zoomin" style="cursor: pointer;" title="Zoom in"></i></td>
                                        <td style="width: 5%;"><i class="fa fa-search-minus" id="zoomout" style="cursor: pointer;" title="Zoom out"></i></td>
                                        <td style="width: 5%;"><i class="fa fa-picture-o" id="revert" style="cursor: pointer;" title="Revert to original"></i></td>
                                        <td style="width: 5%;"><i class="fa fa-times" id="deletethumbnail" runat="server" style="cursor: pointer; display: none;" title="delete viewing image"></i></td>
                                    </tr>
                                </table>
                                <div style="margin-top: 10px; height: 568px; width: 575px; border: 1px solid #6A9C73; overflow: auto;" id="imgholder" runat="server">
                                    <img id="_imgBig" runat="server" />
                                </div>
                                <div style="margin-top: 10px; height: 587px; width: 575px; border: 1px solid #6A9C73; overflow: auto;" id="PDFholder" runat="server">
                                    <iframe id="bigImagePDF" runat="server" style="height: 655px; width: 575px !important; overflow: auto;" frameborder="0" src="Viewimg.aspx"></iframe>
                                </div>
                            </div>
                            <table style="width: 98%;">
                                <tr>
                                    <td></td>
                                    <td align="right">
                                        <asp:UpdatePanel ID="updateControlButtons" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <input type="button" class="aspresizedredbutton" style="float: right;" id="btnClose" value="Close" onclick="btnClose_Clicked();" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <asp:UpdatePanel ID="waitCursor" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div id="divLoading" class="modal" runat="server" style="text-align: center; display: none">
                        <asp:Panel ID="Panel2" runat="server">
                            <br />
                            <br />
                            <br />
                            <br />
                            <center>
                                <asp:Label ID="Label1" Text="" runat="server" EnableViewState="false"></asp:Label>
                            </center>
                            <br />
                            <img src="Resources/loadimage.gif" height="30px" width="30px" title="[Please wait while the page is loading...]"
                                alt="Loading..." />
                            <br />
                        </asp:Panel>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div id="CheckAlert" onclick="ToolStripAlertHide();">
            <div id="innerMsgText" style="font-family: Verdana,Arial,sans-serif !important; font-size: 18px; color: #000000;"></div>
        </div>

        <asp:HiddenField ID="hdnPagecount" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnErroredFilePath" runat="server" EnableViewState="false" />

        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <script src="JScripts/jquery-1.11.3.min.js" type="text/javascript"></script>
            <script src="Jscripts/jquery-ui.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script type="text/javascript" src="JScripts/jquery.datetimepicker.js"></script>
            <script type="text/javascript" src="JScripts/Lazyload.js"></script>
            <link href="CSS/bootstrap.min3.4.0.css" rel="stylesheet" />
            <link href="CSS/bootstrap.min.css" rel="stylesheet" />
            <link href="CSS/jquery-ui.css" rel="Stylesheet" />
            <script src="JScripts/bootstrap.min3.4.0.js" type="text/javascript"></script>

            <!--Certain Files are marked as static files, no need to implement the VersionConfiguration Technology in the pages-->
            <script src="JScripts/JSModalWindow.js" type="text/javascript" defer="defer"></script>
            <script src="JScripts/JSErroredSignedReports.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <%--defer="defer"--%>
            <script src="JScripts/JSOnlineDocuments.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSLibraries.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
        </asp:PlaceHolder>
    </form>
</body>
</html>
