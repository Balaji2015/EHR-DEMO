<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmSummaryNew.aspx.cs" Inherits="Acurus.Capella.UI.frmSummaryNew" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="aspx" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <script src="JScripts/jquery-1.11.3.min.js" type="text/javascript"></script>
    <link href="CSS/bootstrap.min.css" rel="stylesheet" />
     <%--<script src="JScripts/bootstrap.min3.1.1.max.js" type="text/javascript"></script>--%>

    <style type="text/css">
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

        .highlight {
            background: yellow;
        }

        .rowspace {
            margin-top: 0.5%;
            margin-bottom: 0.5%;
        }

        #RadWindowWrapper_RadViewer {
            top: 115px !important;
        }
    </style>

</head>
<body >
    <form id="form1" runat="server">
        <telerik:RadWindowManager ID="WindowMngr" runat="server">
            <Windows>
                <telerik:RadWindow ID="RadViewer" runat="server" Behaviors="Close" Title="Image Viewer"
                    VisibleStatusbar="false" IconUrl="Resources/16_16.ico" VisibleOnPageLoad="false">
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


        <%-- <div style="height: 33px; background-color: #E3F7FC; margin-bottom: 3px; margin-left: 8px; margin-right: 8px; margin-top: 8px; border-color: #92A9C5;color:#0951AD;width:95%" class="panel panel-success">Summary
        
        </div>
        <div>        <asp:ImageButton ID="btnWord" runat="server" ImageUrl="~/Resources/Word.png" ImageAlign="Right"  OnClick="btnWord_Click" ToolTip="Download Summary" style=" height: 41px;
    border-width: 0px;
    margin-top: -41px;" /></div>--%>
        <div id="summarydiv" runat="server" style="height: 660px; display: none; padding: 15px; font-weight: bolder; background-color: aliceblue">Encounter ID is not found. Please close and reopen the patient chart to solve the issue.</div>
        <%-- <div style="height:50px;display:block" id="divSummary" >
        <table style="width:100%">
            <tr style="width:100%">
                <td style="width:300px;align-content:flex-end" >
           
                  <input type="text" id="txtSearch" placeholder="Find/Search Keyword" style="display:none;margin-top: 6px;margin-bottom: 6px;padding-top: 4px;padding-left:300px;width:100px" runat="server"  />
                   
                </td>
                <td style="align-content:flex-start">
                       
              <button type="button" class="btn btn-primary btn-md"  runat="server" onserverclick="btnWord_Click" style="width: 228px;margin-top: 6px;margin-bottom: 6px;padding-top: 4px;height: 29px;" >Print Progress Note</button>

                </td>
            </tr>
        </table>
             
        </div>--%>

        <div class="rowspace">
        </div>
        <div class="row" id="divSummary" style="width: 100%">

            <%--  <div class="bluebutton" style="float: right;margin-right:15px">
                <button type="button"  runat="server" onserverclick="btnWord_Click" id="btnPrint" class=" aspresizedbluebutton">Print Progress Note</button>
            </div>--%>

            <div class="bluebutton" style="float: right;margin-right:15px">
                <button type="button"  runat="server" onclick="OpenServiceProcedureCode();" id="btnServiceProcedureCode" class=" aspresizedbluebutton">View Service Procedure Code</button>
            </div>

            <asp:Button ID="btntreatment" class="aspresizedbluebutton dropdown-toggle" OnClientClick="PrintTreatmentNote();" style="float: right; margin-right: 15px" Text="Print Treatment Notes" runat="server" />
            <asp:Button ID="btnwellness" class="aspresizedbluebutton dropdown-toggle" OnClientClick="PrintWellnessNote();" style="float: right; margin-right: 15px" Text="Print Wellness Notes" runat="server" />

            <div class="dropdown" style="float: right; margin-right: 15px">
                <button class="aspresizedbluebutton dropdown-toggle" type="button" data-toggle="dropdown" id="Button1" runat="server">
                    Print Consultation Notes
    <span class="caret"></span>
                </button>
                <ul class="dropdown-menu">
                    <%--<li><a href="#" runat="server" onserverclick="btnWordconsult_Click" id="btnconword">Export as Doc</a></li>--%>
                    <li><a href="#" runat="server" onserverclick="btnPDFconsult_Click" id="btnconpdf">Export as pdf</a></li>
                    <li><a href="#" runat="server" onserverclick="btnsendFaxconsult_Click" id="btnconfax">Send as Fax </a></li>
                </ul>
            </div>
            <div class="dropdown" style="float: right; margin-right: 15px">
                <button class="aspresizedbluebutton dropdown-toggle" type="button" data-toggle="dropdown" id="btnPrint" runat="server">
                    Print Progress Notes
    <span class="caret"></span>
                </button>
                <ul class="dropdown-menu">
                    <%--<li><a href="#" runat="server" onserverclick="btnWord_Click" id="btnword">Export as Doc</a></li>--%>
                    <li><a href="#" runat="server" onserverclick="btnPDF_Click" id="btnpdf">Export as pdf</a></li>
                    <li><a href="#" runat="server" onserverclick="btnsendFax_Click" id="btnFax">Send as Fax </a></li>
                </ul>
            </div>
            <div style="float: right; margin-right: 15px">
                <button type="button" runat="server" style="display: none;" onclick="OpenPhoneEncounterCancelAppt();" id="btnCancelPhoneEnc" class="aspresizedbluebutton">Cancel Phone Encounter</button>
            </div>
            <div class="col-md-3" style="float: right; width: 18%">
                <input type="text" id="txtSearch" placeholder="Find/Search Keyword" style="display: none;" class="Editabletxtbox" runat="server" />




            </div>
            <div class="col-md-2" style="float: right">
            </div>


        </div>
        <div class="rowspace">
        </div>
        <div id="xslFrame" style="margin: 0px 8px 0px 8px; border: 1px solid #92A9C5; height: 570px; display: block; overflow-y: scroll; overflow-x: hidden;" runat="server">
            <div style="margin: 3px 3px 3px 3px; overflow-x: hidden;">
                <asp:Xml ID="DownloadFrame" runat="server" Visible="false" />
                <asp:Literal ID="ltlDownloadFrame" runat="server" />
            </div>
        </div>
        <div id="AkidoFrame" visible="false" runat="server" style=" border: 1px solid #92A9C5; height: 630px; display: block; overflow-y: scroll; overflow-x: hidden;">
            <iframe id="iFrameAkidoSummary" runat="server" style= "border: 1px solid #92A9C5; height: 100%;width:100%; display: block; overflow-y: scroll; overflow-x: hidden;"></iframe>
        </div>
         <div id="dvsignphy" runat="server">
            <label style="font-family: Helvetica Neue,Helvetica,Arial,sans-serif !important;font-size: 13px !important;font-weight: normal;color: black;" id="lblSignedPhysician" runat="server" ></label>
        </div>
        <div id="dvsignreviewphy" runat="server">
            <label style="font-family: Helvetica Neue,Helvetica,Arial,sans-serif !important;font-size: 13px !important;font-weight: normal;color: black;"  id="lblReviewSignedPhysician" runat="server"></label>
        </div>
        <asp:HiddenField runat="server" ID="hdnFilePath" />
        <asp:HiddenField runat="server" ID="hdnEncounterId" />
        <asp:HiddenField runat="server" ID="hdnBatchStatus" />
        <asp:Button ID="hdnbtngeneratexmlsummary" runat="server" OnClick="hdnbtngeneratexmlsummary_Click"  style="display:none" />

        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
             <script src="JScripts/jquery-1.11.3.min.js" type="text/javascript"></script>
            <script src="JScripts/jquery-ui.min1.10.2.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/bootstrap.min.js" type="text/javascript"></script>
            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSSummaryNew.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JsHighlight.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
        </asp:PlaceHolder>
        <script>     
            //Jira CAP-1379
            window.onload = function () {
                // Hide loading overlay        
                window.parent.document.getElementById('WaitingMessage').style.display = 'none';
                // Show main content       
                if (window?.parent?.document?.getElementById('Summaryframe') != undefined) {
                    window.parent.document.getElementById('Summaryframe').style.display = 'block';
                }
                else if (window?.parent?.document?.getElementById('ctl00_C5POBody_EncounterContainer') != undefined) {
                    window.parent.document.getElementById('jqxSplitter').style.height = '750px';
                    window.parent.document.getElementById('ctl00_C5POBody_EncounterContainer').style.display = 'block';
                }
            };

        </script>
    </form>
</body>
</html>
