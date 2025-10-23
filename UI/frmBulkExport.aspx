<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmBulkExport.aspx.cs" Inherits="Acurus.Capella.UI.frmBulkExport" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Bulk Export</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style>
        * {
            font-family: sans-serif;
            font-size: 12px;
        }

        div fieldset {
            width: 95%;
            height: 95%;
        }

        .Invisible {
            display: none;
        }

        .auto-style1 {
            width: 337px;
        }
    </style>
    <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
    <link href="~/CSS/CommonStyle.css" rel="Stylesheet" type="text/css" />

</head>
<body class="bodybackground" style="width: 580px; height: 270px; margin: 2px; padding: 0px;" onload=" {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}">
    <form id="frmBulkExport" runat="server" style="width: 100%; height: 100%;">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" EnableViewState="false">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
            </Scripts>
        </telerik:RadScriptManager>
        <div style="width: 100%; height: 100%;">
            <asp:Panel ID="pnlBulkExport" GroupingText="Bulk Export" runat="server" Width="106%" CssClass="LabelStyleBold">
                <table id="tblBulkImport" style="width: 100%; height: 50%;">
                    <tr id="trDates" style="width: 100%; height: 50%;">
                        <td colspan="3">
                            <table id="tblDates" style="width: 100%; height: 100%;">
                                <tr style="width: 100%; height: 35%;">
                                    <td style="width: 100%; vertical-align: top; margin: 0px; padding: 0px;" colspan="4">
                                        <asp:Label ID="lblSelectDate" Text="Select Date Of Service Range" Font-Bold="true" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr style="width: 100%; height: 65%;">
                                    <td style="width: 15%; vertical-align: top; margin: 0px; padding: 0px;">
                                        <asp:Label ID="lblFromDate" Text="From Date:" Style="vertical-align: -webkit-baseline-middle;" runat="server" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td style="width: 35%; text-align: right; vertical-align: top; margin: 0px; padding: 0px;">
                                        <telerik:RadDatePicker ID="dtpFromDate" runat="server">
                                            <DateInput DateFormat="dd-MMM-yyyy" runat="server" ReadOnly="true" CssClass="Editabletxtbox"></DateInput>
                                        </telerik:RadDatePicker>
                                    </td>
                                    <td style="width: 15%; vertical-align: top; margin: 0px; padding: 0px;">
                                        <asp:Label ID="lblToDate" Text="To Date:" Style="vertical-align: -webkit-baseline-middle;" runat="server" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td style="width: 35%; vertical-align: top; text-align: right; margin: 0px; padding: 0px;">
                                        <telerik:RadDatePicker ID="dtpToDate" runat="server">
                                            <DateInput DateFormat="dd-MMM-yyyy" runat="server" ReadOnly="true" CssClass="Editabletxtbox"></DateInput>
                                        </telerik:RadDatePicker>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trProvider" style="width: 100%; height: 25%;">
                        <td style="width: 15%; vertical-align: top; margin: 0px; padding: 0px;">
                            <asp:Label ID="lblProvider" runat="server" Text="Provider" CssClass="Editabletxtbox"></asp:Label>
                        </td>
                        <td style="width: 44%; vertical-align: top; margin: 0px; padding: 0px;">
                            <telerik:RadComboBox ID="cboProvider" runat="server" Width="100%" CssClass="Editabletxtbox"></telerik:RadComboBox>
                        </td>
                        <td style="width: 41%; vertical-align: top; margin: 0px; padding: 0px;">
                            <asp:CheckBox ID="chkShowAllProviders" Text="Show All Physicians" runat="server" OnCheckedChanged="chkShowAllProviders_CheckedChanged" AutoPostBack="true" EnableViewState="true" CssClass="Editabletxtbox" />
                            <asp:Button ID="btnGet" runat="server" Text="Get" Width="64px"  Style="margin-left: 18px; margin-bottom:4px;border-radius:3px"  OnClick="btnGet_Click" CssClass="aspresizedbluebutton" />
                        </td>
                    </tr>
                </table>
                  <telerik:RadGrid ID="grdGetPatient" runat="server" Height="228px" Width="623px"
                   Font-Size="Medium" CellSpacing="0" GridLines="None" AutoGenerateColumns="False" CssClass="Gridbodytyle">
                    <HeaderStyle Font-Bold="false" HorizontalAlign="Center" />
                    <ClientSettings Scrolling-UseStaticHeaders="true" Scrolling-AllowScroll="true">
                        <Scrolling AllowScroll="True" UseStaticHeaders="True" />
                    </ClientSettings>
                    <MasterTableView>

                        <Columns>
                            <telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn" HeaderText="Select">
                                <ItemTemplate>
                                    <input type="checkbox" runat="server" id="chkselect" />
                                </ItemTemplate>

                                <HeaderStyle Width="70px" CssClass="Gridheaderstyle" />
                                <ItemStyle Width="70px" CssClass="Editabletxtbox"/>
                            </telerik:GridTemplateColumn>


                            <telerik:GridBoundColumn DataField="Patient_Account_Num"
                                FilterControlAltText="Filter PatientAccNum column" HeaderText="Patient Account #"
                                UniqueName="PatientAccNum">
                                <HeaderStyle Width="90px" CssClass="Gridheaderstyle" />
                                <ItemStyle Width="90px" CssClass="Editabletxtbox"/>
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn DataField="Patient_Name"
                                FilterControlAltText="Filter PatientName column" HeaderText="Patient Name"
                                UniqueName="PatientName">
                                <HeaderStyle Width="130px" CssClass="Gridheaderstyle" />
                                <ItemStyle Width="130px" CssClass="Editabletxtbox"/>
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn DataField="DOB"
                                FilterControlAltText="Filter DOB column" HeaderText="DOB"
                                UniqueName="DOB">
                                <HeaderStyle Width="70px" CssClass="Gridheaderstyle"/>
                                <ItemStyle Width="70px" CssClass="Editabletxtbox" />
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn DataField="Gender"
                                FilterControlAltText="Filter Gender column" HeaderText="Gender"
                                UniqueName="Gender">
                                <HeaderStyle Width="90px" CssClass="Gridheaderstyle"/>
                                <ItemStyle Width="90px" CssClass="Editabletxtbox" />
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn DataField="DOS"
                                FilterControlAltText="Filter DOS column" HeaderText="DOS"
                                UniqueName="DOS">
                                <HeaderStyle Width="120px" CssClass="Gridheaderstyle"/>
                                <ItemStyle Width="120px" CssClass="Editabletxtbox" />
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn DataField="Encounter_id"
                                FilterControlAltText="Filter Encounter_id column" HeaderText="Encounter_id"
                                UniqueName="Encounter_id" Display="false">
                                <HeaderStyle Width="120px" CssClass="Gridheaderstyle" />
                                <ItemStyle Width="120px" CssClass="Editabletxtbox" />
                            </telerik:GridBoundColumn>


                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
                <table>

                    <tr id="trGenerateButtons" style="width: 50px; height: 25%; ">
                        
                        <td class="auto-style1"></td>
                        <td style="text-align: right; vertical-align: top; margin: 0px; padding: 0px;">
                            <asp:Button ID="Invisiblebuttons" runat="server" Text="Invisible" Width="94px" CssClass="Invisible" OnClick="Invisiblebuttons_Click" />
                            <telerik:RadButton ID="btnGenerate" runat="server" Style="top: 0px; left: 72px; height: 26px !important; font-size: 10px !important;" Text="Generate"
                                Width="71px" OnClick="btnGenerate_Click" OnClientClicking="btnGenerate_ClientClick" ButtonType="LinkButton" CssClass="bluebutton teleriknormalbuttonstyle">
                            </telerik:RadButton>
                            <telerik:RadButton ID="btnClose" runat="server" Style="top: 0px; left: 74px; height: 27px !important; font-size: 13px !important;" Text="Close"
                                Width="51px" OnClientClicked="btnClose_Click" ButtonType="LinkButton" CssClass="redbutton teleriknormalbuttonstyle" >
                            </telerik:RadButton>
                        </td>
                    </tr>
                </table>
            </asp:Panel>

            <asp:Panel ID="Panel1" GroupingText="Export Scheduler" runat="server" Width="113%">
                <table id="tblBulkExport" style="width: 100%; height: 50%;">
                    <tr>
                        <td>
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 20%">
                                        <asp:Label ID="Label1" Text="Destination Path" runat="server" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td style="width: 80%">
                                        <telerik:RadComboBox ID="cboDestination" runat="server" Width="100%" CssClass="Editabletxtbox"></telerik:RadComboBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td colspan="2" style="width: 100%">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 50%">
                                                    <asp:CheckBox ID="chkNonRec" AutoPostBack="true" OnCheckedChanged="chkNonRecChanged" runat="server" Text="Non Recuring" CssClass="Editabletxtbox"/>
                                                </td>
                                                <td style="width: 50%">
                                                    <asp:CheckBox ID="chkRec" AutoPostBack="true" OnCheckedChanged="chkRecChanged" runat="server" Text="Recuring" CssClass="Editabletxtbox" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 50%; vertical-align: top">
                                        <asp:Panel ID="Panel3" GroupingText="Non Recuring" runat="server" Width="259px" Height="115px">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label3" Text="Export Date :" runat="server" CssClass="Editabletxtbox"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <telerik:RadDatePicker ID="RadDateexport" runat="server">
                                                            <DateInput DateFormat="dd-MMM-yyyy" runat="server" ReadOnly="true" CssClass="Editabletxtbox"></DateInput>
                                                        </telerik:RadDatePicker>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label4" runat="server" Text="Time :" CssClass="Editabletxtbox"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <telerik:RadTimePicker ID="dtpNonRecurTime" runat="server" Culture="en-US"
                                                            Height="25px" Skin="Web20" Width="160px" CssClass="Editabletxtbox">
                                                            <Calendar EnableWeekends="True" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False">
                                                            </Calendar>
                                                            <DatePopupButton CssClass="" HoverImageUrl="" ImageUrl="" />
                                                            <TimeView CellSpacing="-1" Columns="7" Interval="00:10:00" TimeFormat="HH:mm">
                                                            </TimeView>
                                                            <TimePopupButton CssClass="" HoverImageUrl="" ImageUrl="" />
                                                        </telerik:RadTimePicker>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                    <td style="width: 50%">
                                        <asp:Panel ID="Panel2" GroupingText="Recuring" runat="server" Width="57%" >
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label2" Text="Start Date:" runat="server" CssClass="Editabletxtbox"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <telerik:RadDatePicker ID="RadDatestart" runat="server">
                                                            <DateInput DateFormat="dd-MMM-yyyy" runat="server" ReadOnly="true" CssClass="Editabletxtbox"></DateInput>
                                                        </telerik:RadDatePicker>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblRecurFromTime" runat="server" Text=" Time :" CssClass="Editabletxtbox"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <telerik:RadTimePicker ID="dtpRecurTime" runat="server" Culture="en-US"
                                                            Height="25px" Skin="Web20" Width="160px" CssClass="Editabletxtbox">
                                                            <Calendar EnableWeekends="True" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False">
                                                            </Calendar>
                                                            <DatePopupButton CssClass="" HoverImageUrl="" ImageUrl="" />
                                                            <TimeView CellSpacing="-1" Columns="7" Interval="00:10:00" TimeFormat="HH:mm">
                                                            </TimeView>
                                                            <TimePopupButton CssClass="" HoverImageUrl="" ImageUrl="" />
                                                        </telerik:RadTimePicker>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label5" runat="server" Text="Frequency:" CssClass="Editabletxtbox"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <telerik:RadComboBox ID="rdschedular" runat="server" Width="85%" CssClass="Editabletxtbox">
                                                        </telerik:RadComboBox>

                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>

                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">
                            <telerik:RadButton ID="btnschedular" runat="server" OnClientClicking="btnGenerate_ClientClick" Style="top: 0px; left: -1px; font-size: 3px !important; height: 27px !important;" Text="Schedule Export"
                                Width="109px" OnClick="btnschedular_Click"  ButtonType="LinkButton" CssClass="bluebutton teleriknormalbuttonstyle">
                            </telerik:RadButton>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlView" GroupingText="View" runat="server" Width="113%" CssClass="LabelStyleBold">
            </asp:Panel>
        </div>
        <asp:HiddenField ID="hdnFileList" runat="server" />
        <asp:HiddenField ID="hdnSelectedPath" runat="server" />
        <asp:PlaceHolder ID="phScripts" runat="server">
            <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
            <script src="JScripts/JSPatientData.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSClinicalSummary.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
        </asp:PlaceHolder>
    </form>
</body>
</html>
