<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmPatientdata.aspx.cs" Inherits="Acurus.Capella.UI.frmPatientdata" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Patient Data</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
        .style1 {
            width: 119px;
        }

        .style2 {
            width: 556px;
        }

        .style3 {
            width: 550px;
        }

        #frmPatientdata {
            width: 879px;
        }

        .SendSummary_td {
            width: 25%;
        }

        .Display {
            background-color: #BFDBFF;
        }
    </style>
      <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <script>
    function ShowIframe(lblid)
    {
    window.open(document.getElementById('lblid').value, "","","");
    }
    
    </script>
    
</head>
<body class="bodybackground" onload=" {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}">
    <form id="frmPatientdata" runat="server">
        <telerik:RadWindowManager ID="ModalWindowMngt" runat="server" EnableViewState="false">
            <Windows>
                <telerik:RadWindow ID="ModalWindow" runat="server" VisibleOnPageLoad="false" Height="625px"
                    IconUrl="Resources/16_16.ico" Width="1225px" EnableViewState="false" Modal="true" Behaviors="Resize,Move">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>

        <div style="width: 876px">
            <asp:Panel ID="pan" runat="server">
                <table style="width: 100%;">
                    <tr>
                        <td>

                            <asp:Label ID="lblSelectPatient" class="Editabletxtbox" runat="server" Text="Select a Patient"></asp:Label>

                        </td>
                        <td>

                            <asp:TextBox ID="txtPatientname" class="Editabletxtbox" runat="server" Width="538px" ReadOnly="true" CssClass="Display" BorderWidth="1"></asp:TextBox>

                        </td>
                        <td>
                            <telerik:RadButton ID="btnFindPatientData" ButtonType="LinkButton" CssClass="bluebutton"  runat="server" Style="top: 0px; left: -1px" Text="Find Patient"
                                Width="185px" OnClientClicked="OpenfindPatient">
                            </telerik:RadButton>

                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td>

                            <telerik:RadButton ID="btnAdd" runat="server"  ButtonType="LinkButton" CssClass="greenbutton"  Style="top: 0px; left: -1px" Text="Add"
                                Width="90px" OnClick="btnAdd_Click">
                            </telerik:RadButton>
                            &nbsp;<telerik:RadButton ID="btnClearall" runat="server"  ButtonType="LinkButton" CssClass="redbutton" 
                                Style="top: 0px; left: -1px" Text="Clear All"
                                Width="90px" OnClick="btnClearall_Click">
                            </telerik:RadButton>

                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlGrid" runat="server" Height="182px " ScrollBars="Auto"
                Width="877px" class="LabelStyleBold"
                GroupingText="Patient Details">
                <telerik:RadGrid ID="grdHuman" runat="server" AutoGenerateColumns="False" CellSpacing="0"
                    GridLines="None" Height="150px"
                    Width="830px" CssClass="Gridbodytyle">
                    <HeaderStyle Font-Bold="true"  CssClass="Gridheaderstyle"  />
                   
                    <ClientSettings>
                        <ClientEvents />
                        <Scrolling AllowScroll="True" UseStaticHeaders="True" />
                        <Selecting AllowRowSelect="True" />
                    </ClientSettings>
                    <MasterTableView>
                        <Columns>
                            <telerik:GridBoundColumn FilterControlAltText="Filter Issues column" HeaderText="Patient Account No"
                                UniqueName="PatientAccountNo" DataField="Patient Account No">
                                 <ItemStyle  CssClass="Editabletxtbox"/>
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn FilterControlAltText="Filter Feedback column" HeaderText="Patient Name"
                                UniqueName="PatientName" DataField="Patient Name">
                                 <ItemStyle  CssClass="Editabletxtbox"/>
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn FilterControlAltText="Filter EncounterID column" HeaderText="Patient DOB"
                                UniqueName="PatientDOB" DataField="Patient DOB">
                                 <ItemStyle  CssClass="Editabletxtbox"/>
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn FilterControlAltText="Filter Version column" HeaderText="Patient Gender"
                                UniqueName="PatientGender" DataField="Patient Gender">
                                 <ItemStyle  CssClass="Editabletxtbox"/>
                            </telerik:GridBoundColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </asp:Panel>
            <table style="width: 100%;">
                <tr>
                    <td>
                        <telerik:RadButton ID="btnViewXML" runat="server" Style="top: 0px; left: -1px" ButtonType="LinkButton" CssClass="bluebutton teleriknormalbuttonstyle" Text="Generate XML View"
                            Width="122px" OnClick="btnViewXML_Click">
                        </telerik:RadButton>
                    </td>
                    <td align="right">
                        <telerik:RadButton ID="btnGenerate" runat="server" Style="top: 0px; left: -1px" Text="Generate" ButtonType="LinkButton" CssClass="bluebutton teleriknormalbuttonstyle" 
                            Width="90px" OnClick="btnGenerate_Click">
                        </telerik:RadButton>
                        <telerik:RadButton ID="btnClose" runat="server" Style="top: 0px; left: -1px" Text="Close" ButtonType="LinkButton" CssClass="redbutton teleriknormalbuttonstyle" 
                            Width="90px" OnClientClicked="btnClose_Click" OnClick="btnClose_Click">
                        </telerik:RadButton>

                    </td>
                </tr>
            </table>

            <asp:Panel ID="pnlSendSummary" Font-Size="Small" GroupingText="Send Summary" Class="pnlBarGroup" runat="server">
                <table width="100%">
                    <tr>
                        <td class="SendSummary_td" align="center">
                            <asp:Label ID="lblRecpAddress" runat="server" CssClass="Editabletxtbox" Text="Recipient"></asp:Label>
                        </td>
                        <td class="SendSummary_td" rowspan="2">
                            <telerik:RadTextBox ID="txtRecAdd" runat="server" CssClass="Editabletxtbox" TextMode="MultiLine">
                                <DisabledStyle Resize="None" />
                                <InvalidStyle Resize="None" />
                                <HoveredStyle Resize="None" />
                                <ReadOnlyStyle Resize="None" />
                                <EmptyMessageStyle Resize="None" />
                                <FocusedStyle Resize="None" />
                                <EnabledStyle Resize="None" />
                            </telerik:RadTextBox>
                        </td>
                        <td class="SendSummary_td" align="center">
                            <asp:Label ID="lblMailText" runat="server" CssClass="Editabletxtbox" Text="Message"></asp:Label>
                        </td>
                        <td rowspan="3" class="SendSummary_td">
                            <telerik:RadTextBox ID="txtMailText" runat="server" CssClass="Editabletxtbox" TextMode="MultiLine"></telerik:RadTextBox>
                        </td>
                    </tr>

                    <tr>
                        <td class="SendSummary_td"></td>
                        <td class="SendSummary_td"></td>
                        <td class="SendSummary_td"></td>
                        <td class="SendSummary_td"></td>
                    </tr>

                    <tr>
                        <td class="SendSummary_td"></td>
                        <td class="SendSummary_td"></td>
                        <td class="SendSummary_td"></td>
                        <td class="SendSummary_td"></td>
                    </tr>

                </table>
                <div>
                    <asp:Panel ID="pnlAttachments" runat="server">
                    </asp:Panel>
                </div>
            </asp:Panel>

            <table width="100%">
                <tr style="width: 100%;">
                    <td align="right" style="width: 100%;">
                        <telerik:RadButton ID="btnSendSummary" CssClass="bluebutton teleriknormalbuttonstyle" runat="server" Text="Send" ButtonType="LinkButton"
                            OnClick="btnSendSummary_Click">
                        </telerik:RadButton>
                    </td>
                </tr>
            </table>
            <telerik:RadScriptManager ID="RadScriptManager1" runat="server" EnableViewState="false">
                <Scripts>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
                </Scripts>
            </telerik:RadScriptManager>
        </div>
        <asp:HiddenField ID="hdnAccountNo" runat="server" />
        <asp:HiddenField ID="hdnPatientName" runat="server" />
        <asp:HiddenField ID="hdnPatientDOB" runat="server" />
        <asp:HiddenField ID="hdnSelectedPath" runat="server" />
        <asp:HiddenField ID="hdnPatientGender" runat="server" />
        <asp:HiddenField ID="hdnXmlPath" runat="server" />
        <asp:HiddenField ID="hdnFileList" runat="server" />
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
            <script src="JScripts/JSPatientData.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
        </asp:PlaceHolder>
    </form>
</body>
</html>
