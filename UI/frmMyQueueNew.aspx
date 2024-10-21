<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmMyQueueNew.aspx.cs" Inherits="Acurus.Capella.UI.frmMyQueueNew"  MasterPageFile="~/C5PO.Master" EnableEventValidation="true" %>


<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="MyQBodyContent" ContentPlaceHolderID="C5POBody" runat="server">
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style>
        .rowspace {
            margin-top: 0.5%;
            margin-bottom: 0.5%;
        }

        th {
            margin: 1%;
            color: black;
        }

        div .inlineArrangement {
            display: inline-block;
        }

        .rgSortedAsc {
            background: url(../Resources/GridSortDown.png);
            background-repeat: no-repeat;
            background-color: #bfdbff;
        }

        .rgSortedDesc {
            background: url(../Resources/GridUpSort.png);
            background-repeat: no-repeat;
            background-color: #bfdbff;
        }

        .btn-default:hover {
            color: darkseagreen !important;
            background-color: white !important;
        }

        label {
            font-weight: 500;
        }

        td {
            font: 12px/16px "segoe ui",arial,sans-serif;
        }

            td .selected {
                background-color: #FFFFFF;
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
            width: 100%;
        }

        ::-webkit-scrollbar {
            width: 8px;
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
            .referralorder{
                left:15px;
                top:80px;
            }
    </style>
    <link href="CSS/fontawesomenew.css" rel="stylesheet" />
    <link href="CSS/fontawesome.min.css" rel="stylesheet" type="text/css" />
    <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <telerik:RadWindowManager ID="WindowMngr" runat="server">
        <Windows>
            <telerik:RadWindow ID="MessageWindow" CssClass="referralorder" runat="server" Behaviors="Resize,Move,Close" EnableViewState="false" Modal="false"
                VisibleOnPageLoad="false" IconUrl="Resources/16_16.ico" ShowContentDuringLoad="false">
            </telerik:RadWindow>
            <telerik:RadWindow ID="RadWindowCheckout" runat="server" Behaviors="Resize,Move,Close" EnableViewState="false" Modal="false"
                VisibleOnPageLoad="false" IconUrl="Resources/16_16.ico" ShowContentDuringLoad="false">
            </telerik:RadWindow>
           
            <telerik:RadWindow ID="IndexWindow" runat="server" Behaviors="Resize,Move,Close"
                VisibleStatusbar="false" VisibleOnPageLoad="false" Title="Indexing" IconUrl="Resources/16_16.ico" ShowContentDuringLoad="true">
            </telerik:RadWindow>
            
        </Windows>
    </telerik:RadWindowManager>
    <div id="divMainTabs" style="width: 100%; height: 100%;">
        <div class="rowspace col-md-12" style="margin-bottom: 0px; width: 100%; height: 7%;">
            <button type="button" class="btn btn-default" onclick="ShowMyQTabs(this)" id="btnMyQ">My Q</button>
            <button type="button" class="btn btn-default" onclick="ShowMyQTabs(this)" id="btnGeneral">General Q</button>
        </div>
        <div id="divMyQ" style="display: none; width: 100%; height: 92%;" class="col-md-12 rowspace">
            <div id="divMyQTab" style="margin-bottom: 5px; height: 7%;">
                <button id="btnMyEnc" type="button" class="btn btn-default" onclick="ChangeTableForTabs(this)">My Encounters</button>
                <button id="btnMyTask" type="button" class="btn btn-default" onclick="ChangeTableForTabs(this)">My Tasks</button>
                <button id="btnMyOrder" type="button" class="btn btn-default" onclick="ChangeTableForTabs(this)">My Orders</button>
                <button id="btnMyScan" type="button" class="btn btn-default" onclick="ChangeTableForTabs(this)">My Scan</button>
                <button id="btnMyPres" type="button" class="btn btn-default" onclick="ChangeTableForTabs(this)">My Prescription</button>
                <button id="btnMyAmendmnt" type="button" class="btn btn-default" onclick="ChangeTableForTabs(this)">My Amendment</button>
            </div>

            <div style="width: 100%; height: 7%;">
                <table style="width: 100% !important;">
                    <tr>
                        <td style="width: 47%  !important">
                            <asp:Label ID="lblcount" runat="server" Style="font-weight: bold"></asp:Label>
                        </td>
                        <td style="width: 53% !important;">
                            <table style="float: right;">
                                <tr>
                                    <td>
                                         <input type="checkbox" id="chkOpenTask" class="Editabletxtbox" style="display:none;" onclick="chkOpenTaskClick()" />
                                     </td>
                                     <td>
                                        <label id="lblOpenTask" for="chkOpenTask" class="checkbox-inline Editabletxtbox" style="padding-left: 4px; padding-right: 10px;display:none;">Open Tasks created by me</label>
                                     </td>
                                     <td>
                                         <input type="checkbox" id="chkMyTask14" class="Editabletxtbox" style="display:none;" onclick="chkMyTask14Click(this)" />
                                     </td>
                                     <td>
                                        <label id="lbl14days" for="chkMyTask14" class="checkbox-inline Editabletxtbox" style="padding-left: 4px; padding-right: 10px;display:none;">Completed Last 14 days</label>
                                     </td>
                                    <td>
                                        <input type="checkbox" id="chkMyShowAll" class="Editabletxtbox" onclick="chkShowAllClick(this)" /></td>
                                    <td>
                                        <label for="chkMyShowAll" class="checkbox-inline Editabletxtbox" style="padding-left: 4px; padding-right: 10px">ShowAll</label></td>
                                    <td>
                                        <button type="button" class="btn btn-primary btncolor" id="btnChangeExamRoom" style="background-color: none;">Change Exam Room</button>
                                    </td>
                                    <td>
                                        <button type="button" class="btn btn-primary btncolor" onclick="chkShowAllClick(this)" id="RefreshMyQ" style="background-color: none; margin-left: 1px;">Refresh My Encounters</button>
                                    </td>
                                    <td>
                                        <button type="button" class="btn btn-primary btncolor" id="Processenctr" style="background-color: none; margin-left: 2px;">Process Encounter</button>
                                    </td>
                                    <td>
                                        <button type="button" class="btn btn-primary btncolor" id="MovetoNxtProcess" style="background-color: none; margin-left: 2px; display: none;" onclick="PerformMovetoNextProcess()">Move to Next Process</button>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="col-md-12 rowspace" style="overflow-y: auto; height: 85%; width: 100%; padding: 0px;" id="scrollID">
                <div class="table-responsive" style="height: 100%;" id="MyQTable">
                </div>
            </div>
        </div>
        <div id="divGeneralQ" style="display: none; width: 100%; height: 92%;" class="col-md-12 rowspace">
            <div id="divGeneralQTabs" class="inlineArrangement" style="margin-bottom: -7px; height: 7%;">

                <button id="btnEnc" type="button" class="btn btn-default" onclick="ChangeTableForTabs(this)">Encounters Q</button>
                <button id="btnTask" type="button" class="btn btn-default" onclick="ChangeTableForTabs(this)">Tasks Q</button>
                <button id="btnOrder" type="button" class="btn btn-default" onclick="ChangeTableForTabs(this)">Orders Q</button>
                <button id="btnScan" type="button" class="btn btn-default" onclick="ChangeTableForTabs(this)" style="display: none">Scan Q</button>
                <button id="btnAmendmnt" type="button" class="btn btn-default" onclick="ChangeTableForTabs(this)">Amendment Q</button>
            </div>
            <div id="pnlBtn" style="height: 7%; width: 100%; display: inline-block; padding-bottom: 3px; padding-top: 16px;">

                <table style="float:right;">
                    <tr>
                        <td>
                           
                            <label id="lblEr" for="Exam" style="font-weight: normal">Exam Room</label>
                            <select id="Exam">
                            </select>
                            <label for="chkShowAll" class="checkbox-inline Editabletxtbox">
                                <input type="checkbox" id="chkShowAll" class="Editabletxtbox" onclick="chkShowAllClick(this)" />Show All</label>
                            <button type="button" class="btn btn-primary btncolor" id="btnChkOut" style="background-color: none;">Check Out</button>
                            <button type="button" class="btn btn-primary btncolor" onclick="chkShowAllClick(this)" id="RefreshQ" style="background-color: none;">Refresh Encounters Q</button>
                            <button type="button" class="btn btn-primary btncolor" onclick="MoveToClick(this)" id="MoveTo" style="background-color: none;">Move To My Encounters</button>
                            <button type="button" class="btn btn-primary btncolor" id="Processenc" style="background-color: none;">Process Encounter</button>
                            <label id="lblViewAllFac"  for="chkViewAllFacilities"  class="checkbox-inline Editabletxtbox" runat="server">
                                <input type="checkbox" id="chkViewAllFacilities" class="Editabletxtbox" runat="server" onclick="chkShowAllClick(this)" />View All Facilities</label>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="col-md-12 rowspace" style="overflow-y: auto; height: 85%; width: 100%; padding: 0px; word-wrap: break-word;" id="ScrollIDGeneral">
                <div class="table-responsive" style="height: 100%; word-wrap: break-word;" id="GeneralQTable">
                </div>
            </div>
        </div>
        <div id="ProcessModal" class="modal fade" style="padding-left: 0px; padding-right: 0px">
            <div class="modal-dialog" style="width: 90%;">
                <div class="modal-content" style="width: 100%">
                    <div class="modal-header" style="padding-top: 0px; padding-bottom: 0px">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <h5 class="modal-title" id="ModalTitle"></h5>
                    </div>
                    <div class="modal-body">
                        <iframe style="width: 100%; height: 800px; border: none" id="ProcessFrame"></iframe>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server" EnableViewState="false" EnableScriptCombine="true">
        <Scripts>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
        </Scripts>
    </telerik:RadScriptManager>
  

    <script src="JScripts/JSMyQueueNew.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
</asp:Content>
