<%@ Page Async="true" Language="C#" AutoEventWireup="True" CodeBehind="frmErrorIndexing.aspx.cs" Inherits="Acurus.Capella.UI.frmErrorIndexing" EnableViewState="true" EnableEventValidation="false" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>View Errored Indexing Documents</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <base target="_self" />
    <link href="CSS/ScanningAndIndexing.css" rel="stylesheet" />
    <asp:PlaceHolder ID="PlaceHolder2" runat="server">
        <link href="CSS/CommonStyle.css?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" rel="stylesheet" type="text/css" />
    </asp:PlaceHolder>
    <link href="CSS/font-awesome.4.4.0.css" rel="stylesheet" />
    <link href="CSS/jquery.dataTables.min.css" rel="stylesheet" />
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

        .date-sort {
            cursor: pointer;
        }

            .date-sort:after {
                top: 2%;
                content: "▲" / "";
                font-size: 10px;
                position: absolute;
                right: 51.5%;
                opacity: 0.4;
            }

            .date-sort:before {
                top: 3.4%;
                content: "▼" / "";
                font-size: 10px;
                position: absolute;
                right: 51.5%;
                opacity: 0.4;
            }

        table.dataTable thead > tr > th.sorting:before, table.dataTable thead > tr > th.sorting:after {
            width: 0% !important;
        }

        table.dataTable > thead > tr > th,
        table.dataTable > thead > tr > td {
            padding-right: 10px !important;
        }

        .text-align-center {
            text-align: center;
        }

        .word-break-all {
            word-break: break-all;
        }

        .dataTables_empty {
            display: none;
        }

        .dataTables_filter input {
            width: 330px !important;
        }

        .dataTables_wrapper th {
            padding: 8px !important;
        }

        .process-word-wrap {
            word-wrap: break-word;
        }

        .searchicon {
            background-image: url(../Resources/SearchIcon.png);
            background-repeat: no-repeat;
            padding-left: 26px !important;
        }

        #tblErrorIndexing thead {
            visibility: collapse;
        }

        .hide_column {
            display: none;
        }
        #tblErrorIndexing_info,#tblErrorIndexing_paginate {
            font-family: "Helvetica Neue", Helvetica, Arial, sans-serif !important;
    font-size: 13px !important;
}
        .searchicon::placeholder {
            font-size: 12px;
            font-weight: bold;
        }
    </style>
</head>

<body style="margin-top: -25px; font-size: 14px!important;">
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
        <div id="dOverall" style="font-size: 14px !important;">
            <div style="display:block;margin:30px auto 0 2px; margin-left: 2px;">
                <div class="row">
                    <div class="col-md-12">
                        <div style="float: right;">
                            <button type="button" class="btn btn-primary btncolor" id="btnRefreshMyScan">Refresh My Scan</button>
                            <button type="button" class="btn btn-primary btncolor" id="btnProcessScan">Process Scan</button>
                        </div>
                    </div>
                </div>
                <%--<div class="row">
                    <div class="col-md-12">
                        <hr style="margin-top: 12px !important; margin-bottom: 12px !important; border-top: 2px solid #afafaf !important;" />
                    </div>
                </div>--%>
                <div class="row">
                    <div class="col-md-12" >
                        <div id="divErrorIndexing">
                        </div>
                    </div>
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
            <script src="JScripts/jquery.dataTables.min.js"></script>
            <!--Certain Files are marked as static files, no need to implement the VersionConfiguration Technology in the pages-->
            <script src="JScripts/JSModalWindow.js" type="text/javascript" defer="defer"></script>
            <script src="JScripts/JSErrorIndexing.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <%--defer="defer"--%>
            <script src="JScripts/JSOnlineDocuments.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSLibraries.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
        </asp:PlaceHolder>
    </form>
</body>
</html>
