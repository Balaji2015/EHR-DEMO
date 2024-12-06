<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmFindAllAppointments.aspx.cs"
    Inherits="Acurus.Capella.UI.frmFindAllAppointments" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title>Find Appointments</title>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <base target="_self" />
    <style type="text/css">
        .style1 {
            width: 117px;
        }

        .style2 {
            width: 312px;
        }

        .style3 {
            width: 594px;
        }

        .displayNone {
            display: none;
        }

        #frmFindAllAppointments {
            width: 855px;
            margin-bottom: 0px;
        }

        
            .hide_column{
    display:none;
}

/*.dataTable > thead > tr > th[class*="sort"]:before,
.dataTable > thead > tr > th[class*="sort"]:after {
    content: "" !important;
    }*/
table.dataTable thead>tr>th.sorting:before,table.dataTable thead>tr>th.sorting:after{
            width: 0% !important;
        }
table.dataTable > thead > tr > th,
table.dataTable > thead > tr > td {
    padding-right: 10px !important;
    }

.text-align-center{
    text-align:center;
}

.word-break-all{
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
#grdAppointment_info,#grdAppointment_paginate {
            font-family: "Helvetica Neue", Helvetica, Arial, sans-serif !important;
    font-size: 13px !important;
}
        
.TableCellBorder {
            border: 1px solid #9090904d;
 }
    </style>
    <%--<link href="CSS/ElementStyles.css" rel="stylesheet" type="text/css" />--%>
    <link href="CSS/jquery.dataTables.min.css" rel="stylesheet" />
        <link href="CSS/fontawesomenew.css" rel="stylesheet" />
    <link href="CSS/fontawesome.min.css" rel="stylesheet" type="text/css" />
      <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
</head>
<body onload=" {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}">
    <form id="frmFindAllAppointments" runat="server">
        <telerik:RadWindowManager ID="ModalWindowMngt" runat="server">
            <Windows>
                <telerik:RadWindow ID="ModalWindowAppmnt" runat="server" VisibleOnPageLoad="false"
                    Height="625px" IconUrl="Resources/16_16.ico" Width="1225px">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <asp:ToolkitScriptManager ID="ToolkitScriptManager2" runat="server">
        </asp:ToolkitScriptManager>
        <asp:UpdatePanel ID="updatepanel" runat="server">
            <ContentTemplate>
                <div>
                    <div>
                        
                            <table style="width: 100%;">
                                <tr>
                                    <td class="style1">&nbsp;
                           
                                        <label id="lblPatientName" runat="server" enableviewstate="false" class="Editabletxtbox" >Patient Name</label>
                                    </td>
                                    <td class="style2">&nbsp;
                            
                                        <input type="text" id="txtPatientName" runat="server" style="width:256px; background-color:#BFDBFF;" class="nonEditabletxtbox" readonly="readonly" />
                                    </td>
                                    <td colspan="4">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style1">&nbsp;
                            
                                    <label id="lblPatientAccNO" runat="server" enableviewstate="false" class="Editabletxtbox">Patient Account #</label>
                                    </td>
                                    <td class="style2">&nbsp;
                           
                                        <input type="text" id="txtPatientAccountNO" runat="server" style="width:256px; background-color:#BFDBFF;" class="nonEditabletxtbox" readonly="readonly" />
                                    </td>
                                    <td colspan="2">&nbsp;
                                    </td>
                                    <td colspan="2">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style1">&nbsp;
                            
                                        <label id="lblPatientDOB" runat="server" enableviewstate="false" class="Editabletxtbox">Patient DOB</label>
                                    </td>
                                    <td class="style2">&nbsp;
                           
                                        <input type="text" id="txtPatientDOB" runat="server" style="width:256px; background-color:#BFDBFF;" class="nonEditabletxtbox" readonly="readonly" />
                                    </td>
                                    <td colspan="4">&nbsp;
                            
                                        <input type="button" id="btnFindPatient" runat="server" value="Find Patient"  onserverclick="btnFindPatient_Click" onclick="return OpenFindPatinet();" class="aspbluebutton" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style1">
                                        <%--<asp:Label ID="lblSearchResult" runat="server" Text="SearchResultFound" Font-Bold="True"></asp:Label>--%>
                                    </td>
                                    <td class="style2">&nbsp;
                                    </td>
                                    <td>
                                        
                                        <input type="checkbox" id="chkShowOldAppointments" runat="server"  onclick="ShowAllAppoinmentsClick();" class="Editabletxtbox" /> <label for="chkShowOldAppointments" class="Editabletxtbox"> Show Old Appointments</label>
                                        <label></label>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                            </table>
                       
                    </div>
                    <div style="width: 875px">
                        <div class="table-responsive" style="overflow-x:auto;overflow-y:auto;width:100%;height:405px;" id="divTable">                            
                    </div>
                    <div>
                        <asp:Panel ID="pnlButtons" runat="server" Font-Size="Small" Width="818px" Height="33px">
                            <table style="width: 100%;">
                                <tr>
                                    <td class="style3">&nbsp;
                                        <%--<asp:LinkButton ID="btnFirst" runat="server" CommandArgument="First" OnCommand="PageChangeEventHandler">First</asp:LinkButton>--%>
                                        <%--&nbsp;<asp:LinkButton ID="btnPrevious" runat="server" CommandArgument="Previous"
                                            OnCommand="PageChangeEventHandler">Previous</asp:LinkButton>--%>
                                        <%--&nbsp;<%--<asp:LinkButton ID="btnNext" runat="server" CommandArgument="Next" OnCommand="PageChangeEventHandler">Next</asp:LinkButton>--%>
                                        <%--&nbsp;<%--<asp:LinkButton ID="btnLast" runat="server" CommandArgument="Last" OnCommand="PageChangeEventHandler">Last</asp:LinkButton>--%>
                                        <%--&nbsp;<asp:Label ID="lblShowing" EnableViewState="false" runat="server" ClientIdMode="Static"></asp:Label>--%>
                                        
                                        <input type="button" id="btnFindPatientRefresh" runat="server" class="displayNone" onserverclick="btnFindPatientRefresh_Click" value="Button" />
                                    </td>

                                    <td>
                                        
                                        <input type="button" id="btnCancelAppointment" runat="server" value="Cancel Appointment" style="margin-top: 10px;" 
                                            onserverclick="btnCancelAppointment_Click" class="aspbluebutton" />
                                    </td>
                                    <td>
                                       
                                        <input type="button" id="btnEditAppointment" runat="server" value="Edit Appointment" onclick="return OpenEditAppointment();" style="margin-top: 9px;"
                                            onserverclick="btnEditAppointment_Click" class="aspgreenbutton" />
                                    </td>
                                    <td>
                                       
                                        <input type="button" id="btnCancel" runat="server" value="Cancel" onclick="return CloseWindow();" style="margin-top: 9px;margin-right: -35px;"
                                            enableviewstate="false" class="aspredbutton" />
                                    </td>
                                </tr>
                            </table>
                            <asp:HiddenField ID="hdnSelectedIndex" runat="server" EnableViewState="false" />
                            <asp:HiddenField ID="hdnHumanID" runat="server" EnableViewState="false" />
                            <asp:HiddenField ID="hdnLastPageNo" runat="server" EnableViewState="false" />
                            <asp:HiddenField ID="hdnTotalCount" runat="server" EnableViewState="false" />
                        </asp:Panel>
                    </div>
                </div>
                
                    <input type="button" id="btnRefresh" runat="server" style="display: none" onserverclick="btnRefresh_Click" value="Refresh" />
                <br />

                <asp:PlaceHolder ID="PlaceHolder1" runat="server">
                    
                    <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>"
                        type="text/javascript"></script>

                    <script src="JScripts/JSFindAllAppointments.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>"
                        type="text/javascript"></script>

                    <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>"
                        type="text/javascript"></script>

                    <script src="JScripts/jquery-1.7.1.min.js" type="text/javascript"></script>
                    <script src="JScripts/pako.min.js" type="text/javascript"></script>
                    <script src="JScripts/jquery.dataTables.min.js" type="text/javascript"></script>

                </asp:PlaceHolder>
            </ContentTemplate>
        </asp:UpdatePanel>

    </form>
    
</body>
</html>
