<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmSummaryOfCare.aspx.cs"
    Inherits="Acurus.Capella.UI.frmSummaryOfCare" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Summary Of Care</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <base target="_self" />
    <style type="text/css">
        .Panel legend {
            font-weight: bold;
        }

        .loading {
            font-family: Arial;
            font-size: 10pt;
            border: 5px solid #67CFF5;
            width: 200px;
            height: 100px;
            display: none;
            position: fixed;
            background-color: White;
            z-index: 999;
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

        .displayNone {
            display: none;
        }

        #SummaryCheckList {
            height: 96% !important;
        }

        #btnGenerate {
            height: 3% !important;
        }

        
    </style>
      <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" id="telerikClientEvents1">
        function btnFindPatient_Clicked(sender, args) {
            var obj = new Array();
            obj.push("Scan=Yes");
            var result = openModal("frmFindPatient.aspx", 251, 1200, obj, "RadPatientWindow");
            var WindowName = $find('RadPatientWindow');
            WindowName.add_close(OnClientClose);
        }

        function OnClientClose(oWindow, args) {
            var arg = args.get_argument();
            if (arg) {
                var HumanId = arg.HumanId;
                if (HumanId != "0") {
                    document.getElementById(GetClientId("hdnHumanID")).value = arg.HumanId;
                    var btnfindpatient = document.getElementById(GetClientId('btnIVfindpatient'));
                    btnfindpatient.click();

                }
            }
        }

        function Close() {
            DisplayErrorMessage('115018');
            var win = GetRadWindow();
            win.close();

        }

        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }

        function ShowLoading() {
            document.getElementById('divLoading').style.display = "block";
        }

        function btnMatch_Clicked(sender, args) {
            var patvalue = document.getElementById('txtAccountNo').value;
            dt = new Date();
            document.getElementById('hdnLocalTime').value = dt.getFullYear() + "/" + (dt.getMonth() + 1) + "/" + dt.getDate() + " " + dt.getHours() + ":" + dt.getMinutes() + ":" + dt.getSeconds();
            if (patvalue != '')
            { ShowLoading(); }
            else {
                DisplayErrorMessage('9093024');
            }
        }

        function btnOK_ClientClick(sender, args) {
            DisplayErrorMessage('115018');
            var win = GetRadWindow();
            win.SetUrl('frmClinicalInformation.aspx?XMLPath=' + document.getElementById('hdnXMLPath').value + "&HumanID=" + document.getElementById('hdnHumanID').value + "&DateofService=" + document.getElementById('hdnDateOfService').value);
            win.SetSize(1240, 900);
            win.center();
          
        }

    </script>
    
</head>
<body style="padding: 0px; margin: 3px;">
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
            </Scripts>
        </telerik:RadScriptManager>
        <telerik:RadWindowManager ID="WindowMngr" runat="server" EnableViewState="false">
            <Windows>
                <telerik:RadWindow ID="RadPatientWindow" runat="server" Behaviors="Close" Title="Find Patient"
                    VisibleStatusbar="false" IconUrl="Resources/16_16.ico">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <div style="display: block; height: 100%; width: 100%;">
            <div style="display: inline-block; height: 830px; width: 24%; float: left;">
                <asp:Panel ID="pnlSummarylst" GroupingText="Select Sections" runat="server" CssClass="LabelStyleBold">
                    <asp:CheckBoxList ID="SummaryCheckList" runat="server" CssClass="Editabletxtbox" AutoPostBack="true" RepeatLayout="Flow" OnSelectedIndexChanged="SummaryCheckList_SelectedIndexChanged"></asp:CheckBoxList> <%--onclick="onchangeCheckBox();"--%>
                </asp:Panel>
            </div>
            <div style="display: inline-block; height: 100%; width: 75%;">
                <telerik:RadAjaxPanel ID="pnlSummary" runat="server" Height="850px" Width="100%"
                    HorizontalAlign="NotSet" BorderStyle="Solid" BorderWidth="1px">
                    <table style="width: 100%; height: 100%">
                        <tr style="height: 5%; display: none;">
                            <td>
                                <asp:Panel ID="Panel1" runat="server" Height="100%" Width="100%" CssClass="Editabletxtbox">
                                    <table style="width: 100%; height: 100%">
                                        <tr>
                                            <td width="20%">
                                                <asp:Label ID="lblSelect" runat="server" Text="Select Selection" CssClass="LabelStyleBold"></asp:Label>
                                            </td>
                                            <td width="30%">
                                              
                                            </td>
                                            <td width="50%">&nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr style="height: 88%">
                            <td>
                                <telerik:RadTabStrip ID="RadTabStrip2" runat="server" MultiPageID="RadMultiPage1"
                                    SelectedIndex="0" Width="100%" ScrollChildren="True" AutoPostBack="true">
                                </telerik:RadTabStrip>
                                <telerik:RadMultiPage ID="RadMultiPage1" runat="server" Width="100%" Height="100%"
                                    SelectedIndex="0">
                                    <telerik:RadPageView ID="RadPageView1" runat="server" Height="97%" Width="100%">
                                        <iframe id="PDFLOAD" runat="server" width="100%" height="100%"></iframe>
                                    </telerik:RadPageView>
                                </telerik:RadMultiPage>
                            </td>
                        </tr>
                        <tr style="height: 5%">
                            <td align="right">
                                <table>
                                    <tr>
                                        <td align="right">
                                            <telerik:RadButton ID="btnOK" Text="Reconcile" AutoPostBack="true" runat="server" Visible="false"
                                                OnClientClicked="btnOK_ClientClick" OnClick="btnOK_Click" ButtonType="LinkButton" CssClass="greenbutton">
                                            </telerik:RadButton>
                                        </td>
                                        <td align="right">
                                            <telerik:RadButton ID="btnClose" Text="Close" AutoPostBack="false" runat="server" 
                                                OnClientClicked="Close" ButtonType="LinkButton" CssClass="redbutton">
                                            </telerik:RadButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <%--<tr style="height: 5%">
                    <td>
                        <asp:Panel ID="Panel3" runat="server" Height="100%" Width="100%">
                         <table style="width: 100%;">
                            <tr>
                                <td width="80%">
                                    &nbsp;
                                </td>
                                <td width="20%">
                                    <telerik:RadButton ID="btnFindPatient" runat="server" Text="Find Patient" 
                                        Width="100%" onclientclicked="btnFindPatient_Clicked">
                                    </telerik:RadButton>
                                </td>
                             </tr>
                         </table>
                        </asp:Panel>
                    </td>
                </tr>--%>
                        <%--<tr style="height:7%">
                    <td>
                        <asp:Panel ID="pnlPatientInfo" runat="server" Width="100%" Height="100%"  GroupingText="Patient Information">
                        <table style="width: 100%">
                             <tr>
                                <td width="10%" >
                                    <asp:Label ID="lblPatientBame" runat="server" Text="Patient Name" Width="100%"></asp:Label>  
                                </td>
                                <td width="25%">
                                    <telerik:RadTextBox ID="txtPatientName" runat="server" Width="100%" BackColor="#BFDBFF" ReadOnly="True" BorderColor="Black"
                                BorderWidth="1px">
                                    </telerik:RadTextBox>
                                </td>
                                <td width="10%">
                                     <asp:Label ID="lblAccountNo" runat="server" Text="Account #" Width="100%"></asp:Label>  
                                </td>
                                <td width="10%">
                                    <telerik:RadTextBox ID="txtAccountNo" runat="server" Width="100%" 
                                        BackColor="#BFDBFF" ReadOnly="True" BorderColor="Black"
                                BorderWidth="1px">
                                    </telerik:RadTextBox>
                                </td>
                                <td width="5%">
                                    <asp:Label ID="lblDOB" runat="server" Text="DOB" Width="100%"></asp:Label>  
                                </td>
                                <td width="10%">
                                    <telerik:RadTextBox ID="txtDOB" runat="server" Width="100%" BackColor="#BFDBFF" ReadOnly="True" BorderColor="Black"
                                BorderWidth="1px">
                                    </telerik:RadTextBox>
                                </td>
                                <td width="5%">
                                   <asp:Label ID="lblSex" runat="server" Text="Sex" Width="100%"></asp:Label>  
                                </td>
                                <td width="10%">
                                   <telerik:RadTextBox ID="txtSex" runat="server" Width="100%" BackColor="#BFDBFF" ReadOnly="True" BorderColor="Black"
                                BorderWidth="1px">
                                    </telerik:RadTextBox>
                                </td>
                                <td width="10%">
                                    <telerik:RadButton ID="btnMatch" runat="server" Text="Match" Width="100%" 
                                        onclick="btnMatch_Click" onclientclicked="btnMatch_Clicked">
                                    </telerik:RadButton>
                                 </td>
                              </tr>
                        </table>
                    </asp:Panel>
                    </td>
                </tr>--%>
                    </table>
                    <div id="divLoading" class="modal" runat="server" style="text-align: center; display: none">
                        <asp:Panel ID="Panel2" runat="server">
                            <br />
                            <br />
                            <br />
                            <br />
                            <center>
                                <asp:Label ID="Label1" Text="" runat="server" EnableViewState="false"></asp:Label></center>
                            <br />
                            <img src="Resources/wait.ico" title="[Please wait while the page is loading...]"
                                alt="Loading..." />
                            <br />
                        </asp:Panel>
                    </div>
                    <asp:HiddenField ID="hdnHumanID" runat="server" />
                    <asp:HiddenField ID="hdnXMLPath" runat="server" />
                    <asp:HiddenField ID="hdnDateOfService" runat="server" />
                    <%--<asp:Button ID="btnIVfindpatient" runat="server" Text="Button" onclientclick="ShowLoading();" onclick="btnIVfindpatient_Click" CssClass="displayNone"/>--%>
                </telerik:RadAjaxPanel>
            </div>
        </div>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">

            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>"
                type="text/javascript"></script>

            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>"
                type="text/javascript"></script>

            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>"
                type="text/javascript"></script>

        </asp:PlaceHolder>
    </form>
</body>
</html>
