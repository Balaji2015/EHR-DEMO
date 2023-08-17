<%@ page async="true" language="C#" autoeventwireup="true" codebehind="frmPatientCommunication.aspx.cs" enableeventvalidation="false" inherits="Acurus.Capella.UI.frmAddorViewMessage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="aspx" %>
<%@ register src="~/UserControls/CustomDLCNew.ascx" tagname="DLC" tagprefix="DLC" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

      <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <base target="_self" />
    <title>Patient Communication</title>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
        .ui-dialog-titlebar-close {
            display: none !important;
        }

        .ui-widget {
            font-family: Verdana,Arial,sans-serif !important;
        }

        .ui-dialog .ui-dialog-buttonpane .ui-dialog-buttonset {
            float: none !important;
            margin-left: 45px !important;
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
            height: 155px !important;
            border: 2px solid !important;
            border-radius: 13px !important;
            top: 504px !important;
            left: 568px !important;
        }

        .ui-dialog .ui-dialog-content {
            min-height: 0px !important;
        }

        .ui-dialog .ui-dialog-buttonpane {
            margin-top: -10px !important;
            font-size: 14px !important;
            /*padding: 0px !important ;*/
        }

        .ui-widget-content {
            border: 0px !important;
        }

        .ui-widget-header, .ui-state-default, ui-button {
            font-weight: bold !important;
            font-size: 12px !important;
            font-family: sans-serif !important;
        }


        .ui-widget {
            border: 1px solid #adadad !important;
            background-color: #F7F7F7 !important;
        }

        .modal-open {
            overflow: hidden;
        }

        .Panel legend {
            font-weight: bold;
        }

        .displayNone {
            display: none;
        }

        select {
            width: 150px;
        }

        .table td {
            /*background-color: #fff !important;*/
            font-family: "Helvetica Neue",Helvetica,Arial,sans-serif;
            font-size: 12px;
            line-height: 1.82857143;
            color: #333;
            cursor: pointer;
        }



        .table {
            word-wrap: break-word;
            word-break: normal;
            white-space: nowrap;
            line-height: 1.42857143;
            border-collapse: collapse !important;
            width: 100%;
        }

        .table-bordered th, .table-bordered td {
            border: 1px solid #ddd !important;
        }

        .highlight {
            background-color: #7D3939 !important;
        }

        .ordhighlight {
            background-color: #fff !important;
        }

        .modal-open {
            overflow: hidden;
        }

        .modal {
            position: fixed;
            top: 0;
            right: 0;
            bottom: 0;
            left: 0;
            z-index: 1050;
            display: none;
            overflow: hidden;
            -webkit-overflow-scrolling: touch;
            outline: 0;
        }

            .modal.fade .modal-dialog {
                -webkit-transition: -webkit-transform .3s ease-out;
                -o-transition: -o-transform .3s ease-out;
                transition: transform .3s ease-out;
                -webkit-transform: translate(0, -25%);
                -ms-transform: translate(0, -25%);
                -o-transform: translate(0, -25%);
                transform: translate(0, -25%);
            }

            .modal.in .modal-dialog {
                -webkit-transform: translate(0, 0);
                -ms-transform: translate(0, 0);
                -o-transform: translate(0, 0);
                transform: translate(0, 0);
            }

        .modal-open .modal {
            overflow-x: hidden;
            overflow-y: auto;
        }

        .modal-dialog {
            position: relative;
            width: auto;
            margin: 10px;
        }

        .modal-content {
            position: relative;
            background-color: #fff;
            -webkit-background-clip: padding-box;
            background-clip: padding-box;
            border: 1px solid #999;
            border: 1px solid rgba(0, 0, 0, .2);
            border-radius: 6px;
            outline: 0;
            -webkit-box-shadow: 0 3px 9px rgba(0, 0, 0, .5);
            box-shadow: 0 3px 9px rgba(0, 0, 0, .5);
        }

        .modal-backdrop {
            position: fixed;
            top: 0;
            right: 0;
            bottom: 0;
            left: 0;
            z-index: 1040;
            background-color: #000;
        }

            .modal-backdrop.fade {
                filter: alpha(opacity=0);
                opacity: 0;
            }

            .modal-backdrop.in {
                filter: alpha(opacity=50);
                opacity: .5;
            }

        .modal-header {
            padding: 15px;
            border-bottom: 1px solid #e5e5e5;
        }

            .modal-header .close {
                margin-top: -2px;
            }

        .modal-title {
            margin: 0;
            line-height: 1.42857143;
        }

        .modal-body {
            position: relative;
            padding: 15px;
        }

        .modal-footer {
            padding: 15px;
            text-align: right;
            border-top: 1px solid #e5e5e5;
        }

            .modal-footer .btn + .btn {
                margin-bottom: 0;
                margin-left: 5px;
            }

            .modal-footer .btn-group .btn + .btn {
                margin-left: -1px;
            }

            .modal-footer .btn-block + .btn-block {
                margin-left: 0;
            }

        .modal-scrollbar-measure {
            position: absolute;
            top: -9999px;
            width: 50px;
            height: 50px;
            overflow: scroll;
        }

        @media (min-width: 768px) {
            .modal-dialog {
                width: 600px;
                margin: 30px auto;
            }

            .modal-content {
                -webkit-box-shadow: 0 5px 15px rgba(0, 0, 0, .5);
                box-shadow: 0 5px 15px rgba(0, 0, 0, .5);
            }

            .modal-sm {
                width: 300px;
            }
        }

        @media (min-width: 992px) {
            .modal-lg {
                width: 900px;
            }
        }

        .modal-header:before,
        .modal-header:after,
        .modal-footer:before,
        .modal-footer:after {
            display: table;
            content: " ";
        }

        .modal-header:after,
        .modal-footer:after {
            clear: both;
        }

        .close {
            float: right;
            font-size: 21px;
            font-weight: bold;
            line-height: 1;
            color: #000;
            text-shadow: 0 1px 0 #fff;
            filter: alpha(opacity=20);
            opacity: .2;
        }

            .close:hover,
            .close:focus {
                color: #000;
                text-decoration: none;
                cursor: pointer;
                filter: alpha(opacity=50);
                opacity: .5;
            }

        button.close {
            -webkit-appearance: none;
            padding: 0;
            cursor: pointer;
            background: transparent;
            border: 0;
        }
    </style>
    <link href="CSS/jquery-ui.css" rel="stylesheet" />
    <%-- <link href="CSS/ElementStyles.css" rel="stylesheet" />--%>
    <%--<link href="CSS/style.css" rel="stylesheet" type="text/css" />--%>
    <link href="CSS/font-awesome.css" rel="stylesheet" type="text/css" />

    <link rel="stylesheet" href="CSS/datetimepicker.css" />
</head>
<body onload="{ sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }">
    <%--Changed onload for BugID:45808--%>
    <form id="frmPatientCommunication" runat="server" name="frmPatientCommunication">
        <telerik:RadWindowManager ID="Resulst" runat="server" EnableViewState="false">
            <Windows>
                <telerik:RadWindow ID="MessageWindow" runat="server" VisibleOnPageLoad="false" Modal="true"
                    Behaviors="Close" IconUrl="Resources/16_16.ico" EnableViewState="false">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
         <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
            </Scripts>
        </telerik:RadScriptManager>
       <%-- <asp:scriptmanager id="scriptMngr" runat="server"></asp:scriptmanager>--%>
        <div id="Divider" runat="server">
            <asp:panel id="pnlPatientCommunication" runat="server">
                 <asp:Button ID="hdnbtngeneratetaskxml" runat="server" OnClick="hdnbtngeneratexml_Click"  style="display:none" />

                <table style="width: 100%">
                    <tr>
                        <td style="width: 100%">
                            <%--<asp:Panel ID="PnlPatientDetails" runat="server" Font-Size="Small" CssClass="Panel"
                                GroupingText="Patient Details">
                                <table id="Table1" runat="server" style="width: 100%">
                                    <tr>
                                        <td>--%>
                             <div id="divPatientstrip" runat="server" class=" pnlBarGroup Editabletxtbox " style="margin-top: -8px; vertical-align: middle; padding-top: 4px; position: relative; padding-left: 0px; border: 0px !important;"></div>
                            <asp:TextBox ID="txtAccount" runat="server" style="display:none;"></asp:TextBox>
                                       <%--</td>
                                    </tr>
                                </table>
                            </asp:Panel>--%>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%" class="Editabletxtbox">
                            <asp:Panel ID="PnlCommunicationDetails" CssClass="Panel" GroupingText="Patient  Communication  Details"
                                runat="server" Font-Size="Small">
                                <table id="Table2" runat="server" style="width: 100%">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbltype" runat="server" Text="Type" CssClass="Editabletxtbox"></asp:Label>
                                        </td>
                                        <td>
                                            <select id="ddlType" onchange="return TypeChange();" class="Editabletxtbox"></select>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRelationship" runat="server" Text="Relationship" CssClass="Editabletxtbox"></asp:Label>
                                        </td>
                                        <td>
                                            <select id="ddlRelationship" onchange="RelationChange();" class="Editabletxtbox"></select>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCallerName" runat="server" Text="Spoken To" CssClass="spanstyle"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtCallerName" Width="160px" OnKeyPress="AutoSave();" onchange="AutoSave();"
                                                runat="server" CssClass="nonEditabletxtbox"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblMessageorigin" runat="server" Text="Message Origin" Width="100px" CssClass="Editabletxtbox"></asp:Label>
                                        </td>
                                        <td>
                                            <select id="ddlMessageOrigin" onchange="AutoSave();" class="Editabletxtbox"></select>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblFacilityname" runat="server" Text="Facility" CssClass="Editabletxtbox"></asp:Label>
                                        </td>
                                        <td> 
                                            <select id="ddlFacilityName" onchange="return FacilityChange(this);" class="Editabletxtbox"></select>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblassignedto" runat="server" Text="Assigned To*" mand="Yes"></asp:Label>
                                        </td>
                                        <td>
                                            <select id="ddlAssignedTo" onchange="EnableAll();" class="Editabletxtbox"></select>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkshowall" runat="server" Text="Show All" onclick="chkShowAllChange();" Width="70px" CssClass="Editabletxtbox"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblPriority" runat="server" Text="Priority" CssClass="Editabletxtbox"></asp:Label>
                                        </td>
                                        <td>
                                            <select id="ddlPriority" onchange="AutoSave();" class="Editabletxtbox"></select>
                                         </td>
                                        <td style="width: 90px">
                                            <span id="lblMessageType" class="MandLabelstyle">Message Description</span><span id="lblMessageTypeStar" class="manredforstar">*</span>
                                        </td>
                                        <td>
                                            <select id="ddlMessageType" onchange="AutoSave();" class="Editabletxtbox"></select>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblMessagedatetime" runat="server" Text="Message Date  " Width="85px" CssClass="Editabletxtbox"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <input type="text" id="txtMessageDate" style="width: 150px;" onchange="AutoSave();" class="Editabletxtbox"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span id="lblMessageNotes" class="MandLabelstyle">Message Notes</span><span id="lblMessageNotesStar" class="manredforstar">*</span>
                                        </td>
                                        <td colspan="3" style="width: 100%" class="Editabletxtbox">
                                            <DLC:DLC ID="DLC" runat="server" TextboxHeight="60px" TextboxMaxLength="8000"
                                                TextboxWidth="360px" Value="MESSAGE NOTES"/>
                                        </td>
                                         <td>
                                            <asp:Label ID="lblCreatedBy" runat="server" Text="Created By" Width="85px" CssClass="Editabletxtbox"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtCreatedBy" Width="150px" OnKeyPress="AutoSave();" onchange="AutoSave();"
                                                runat="server" CssClass="nonEditabletxtbox"></asp:TextBox>
                                        </td>
                                    </tr>
                                     <tr>
                                        <td>
                                            <asp:Label ID="Label2" runat="server"  Text="Message History" CssClass="Editabletxtbox"></asp:Label>
                                        </td>
                                        <td colspan="3" style="width: 100%">
                                            <asp:TextBox TextMode="MultiLine"  id="txtmsghistory"  runat="server" style="width:360px;height:60px;resize: none;" ReadOnly="true" CssClass="nonEditabletxtbox"></asp:TextBox> 
                                         
                                        </td>
                                        
                                    </tr>
                                    <tr>
                                        <td colspan="7">
                                            <asp:CheckBox ID="ChkPatientChart" runat="server" Text="Add To PatientChart" onchange="EnableAll();" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%" colspan="7">
                                            <asp:Panel runat="server" ID="PnlForSave">
                                                <table style="width: 100%">
                                                    <tr id="RowForAll" runat="server" style="width: 100%">
                                                        <td align="right" style="width: 50%">
                                                            
                                                           <input type="button" id="btnFindAllAppointmentsMenu" value="Find All Appointments" onclick="return OpenFindAllAppointments();" class="aspresizedbluebutton" />
                                                           <button id="btnOpenPatientChartMenu" runat="server" onclick="return btnpatientChart_Click();" visible="true" class="aspresizedbluebutton">Open Patient Chart</button>
                                                             <input type="button" id="btnSaveMenu" value="Add" onclick="return SaveMenuClick(this);" class="aspresizedgreenbutton" />
<%--                                                             <button id="btnPrint" type="button" class="aspresizedbluebutton" onclick="btnPrint_Clicked();return false;" >Print</button>--%>
                                                           <input type="button" id="btnPrint" value="Print" onclick="btnPrint_Clicked();" class="aspresizedbluebutton" />
                                                                   <input type="button" id="btnClearAll" value="Clear All" onclick="return ClearAllMenu(this);" class="aspresizedredbutton" />
                                                        </td>
                                                    </tr>
                                                    <tr id="RowForHide" runat="server">
                                                        <td align="right" style="width: 100%">
                                                            <input type="button" id="btnFindAllAppointments" value="Find All Appointments" onclick="return OpenFindAllAppointments();" class="aspresizedbluebutton" />
                                                            <input type="button" id="btnSaveMyQ" value="Save" onclick="return SaveClick(this);" style="display:none;" /> <%--class="aspresizedgreenbutton" --%>
                                                            <input type="button" id="btnSaveSendMyQ" value="Send" onclick="return SaveClick(this);" class="aspresizedgreenbutton" />
                                                            <input type="button" id="btnSaveCompletedMyQ" value="Task Complete" onclick="return SaveClick(this);" class="aspresizedgreenbutton" />
                                                            <button id="btnPatient" runat="server" onclick="return btnpatientChart_Click();" visible="true" class="aspresizedbluebutton">Open Patient Chart</button>
                                                              <input type="button" id="btnePrescribe" value="ePrescribe" class="aspresizedbluebutton" onclick="return btnePrescribe_Click();"/>
                                                            <button id="btnCancelMyQ" onclick="return CancelMyQ();" class="aspresizedredbutton" >Close</button>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%" class="Editabletxtbox">
                            <asp:Panel ID="pnlMessageInfo" runat="server" GroupingText="Messages" CssClass="Panel"
                                Font-Size="Small">
                                <table style="width: 100%">
                                    <tr>
                                        <td class="displayNone" style="width: 100%">
                                            <asp:Panel ID="pnlSearchMessage" runat="server" GroupingText="Search Message">
                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblMessageDescription" runat="server" Text="Message Description" EnableViewState="False"
                                                                Width="137px"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <select id="ddlMessageDescription" onchange="EnableAll();"></select>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label1" runat="server" Text="Message" EnableViewState="False"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtMessageNotes" runat="server" Width="160px"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <button id="btnSearch" onclick="return SearchClick();">Search</button>
                                                            &nbsp;&nbsp;&nbsp;
                                                            <button id="btnClear" onclick="return ClearClick();">Clear All</button>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%">
                                            <div class="table-responsive" id="grdMessages" style="vertical-align: top; height: 300px; width: 980px; overflow-x: scroll; overflow-y: scroll">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%">
                                            <table style="width: 100%">
                                                <tr id="Tr1ForHide" runat="server" style="width: 100%">
                                                    <td align="right" style="width: 100%" class="Editabletxtbox">
                                                        <button id="btnCancelMenu" onclick="return CancelMenu(this);" type="button" class="aspresizedredbutton">Close</button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </asp:panel>
        </div>
         <asp:placeholder id="PlaceHolder1" runat="server">
            <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>

            <script src="JScripts/jquery-ui.min1.10.2.js" type="text/javascript"></script>

            <script src="JScripts/bootstrap.min.js" type="text/javascript"></script>

            <script src="JScripts/jquery.datetimepicker.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
              
            <script src="JScripts/JSPatientCommunication.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSCustomDLC.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
        </asp:placeholder>
    </form>
</body>
</html>
