<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmPatientReminderReport.aspx.cs"
    Inherits="Acurus.Capella.UI.frmPatientReminderReport" %>

<%@ Register Src="~/UserControls/PageNavigator.ascx" TagName="PageNavigator" TagPrefix="PageNavigator" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Patient Reminder Report</title>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
        .Panel {
            background-color: White;
        }

            .Panel legend {
                font-weight: bold;
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

        #grdPatientReminder {
            overflow: hidden;
        }       
    </style>
    <link href="~/CSS/CommonStyle.css" rel="Stylesheet" type="text/css" />
</head>
<body class="bodybackground" onload=" onloadpatientremainder();{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}">
    <form id="form1" runat="server">
        <telerik:RadWindowManager ID="WindowMngr" runat="server">
            <Windows>
                <telerik:RadWindow ID="MessageWindow" runat="server" Style="display: none; position: absolute"
                    Width="1000px" Height="700px">
                </telerik:RadWindow>

                <telerik:RadWindow ID="RadWindowSendMail" runat="server" Style="display: none; position: absolute"
                    Width="1000px" Height="700px">
                </telerik:RadWindow>

            </Windows>
        </telerik:RadWindowManager>
        <div>
            <table width="100%">
                <tr>
                    <td style="width: 25%" colspan="5">
                        <asp:Panel ID="pnlGeneralPatientReminder" runat="server" GroupingText="Generate Patient Reminder"
                            Width="100%" Height="120px" Font-Size="Small" CssClass="Panel LabelStyleBold" Font-Names="Times New Roman"
                            BackColor="White">
                            <table width="100%" style="height: 75px">
                                <tr style="width: 50%">
                                    <td style="width: 10%">
                                        <asp:Label ID="lblRuleName" runat="server" Text="Rule Name*" Width="100%" CssClass="Editabletxtbox" mand="Yes">
                                        </asp:Label>
                                    </td>
                                    <td style="width: 10%" colspan="2">
                                        <asp:DropDownList ID="cboRule" AutoPostBack="true" runat="server" Width="158px" onchange="getDropdownListSelectedText();" OnSelectedIndexChanged="cboRule_SelectedIndexChanged" CssClass="Editabletxtbox">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 2%"></td>
                                    <td style="width: 15%">
                                        <asp:Label ID="lblRuleDescription" runat="server" Text="Rule Description" Width="100%" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td style="width: 30%">
                                        <telerik:RadTextBox ID="txtRuleDescription" runat="server" Width="100%"  CssClass="nonEditabletxtbox"
                                             ReadOnly="true" ReadOnlyStyle-BorderColor="Black" Height="20px">
                                        </telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10%">
                                        <asp:Label ID="lblLastRunDate" runat="server" Text="Last Run Date" Width="100%" CssClass="Editabletxtbox">
                                        </asp:Label>
                                    </td>
                                    <td style="width: 15%">
                                        <telerik:RadTextBox ID="txtLastRunDate" runat="server" Width="100%"  CssClass="nonEditabletxtbox"
                                             ReadOnly="true" ReadOnlyStyle-BorderColor="Black" Height="20px">
                                        </telerik:RadTextBox>
                                    </td>
                                    <td style="width: 10%">
                                        <asp:Label ID="lblPhysicianName" runat="server" Text="Physician Name" Width="100%" Style="display: none" CssClass="Editabletxtbox">
                                        </asp:Label>
                                    </td>
                                    <td style="width: 25%">
                                        <telerik:RadComboBox ID="cboPhysicianName" runat="server" Width="100%" MaxHeight="100" Style="display: none" CssClass="nonEditabletxtbox">
                                        </telerik:RadComboBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblActionNeeded" runat="server" Text="Action Needed" Width="71%" Height="19px" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtActionNeeded" runat="server" Width="100%"  CssClass="nonEditabletxtbox"
                                             ReadOnly="true" ReadOnlyStyle-BorderColor="Black" Height="20px">
                                        </telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblCount" runat="server" Visible="False" Font-Bold="True" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td colspan="4" align="right">
                                        <telerik:RadButton ID="btnGenerateReport" runat="server" Text="Generate Report" Width="17%" style="height: 32px !important;"
                                            OnClick="btnGenerateReport_Click" OnClientClicked="ShowLoading" ButtonType="LinkButton" CssClass="bluebutton teleriknormalbuttonstyle">
                                        </telerik:RadButton>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <asp:Panel ID="pnlGrid" runat="server" GroupingText="Patient Reminder List" Font-Size="Small"
                            Font-Names="Times New Roman" CssClass="Panel LabelStyleBold">
                            <telerik:RadGrid ID="grdPatientReminder" GridLines="Both" runat="server" class="Editabletxtbox" AutoGenerateColumns="False"
                                CellSpacing="0" CssClass="Gridbodystyle"
                                Height="335px" Width="1150px">
                                <FilterMenu EnableImageSprites="False">
                                </FilterMenu>
                                <HeaderStyle Font-Bold="true" CssClass="Gridheaderstyle" />
                                <SelectedItemStyle Font-Bold="true" BorderWidth="1px" BorderStyle="Dashed" />
                                <ClientSettings>
                                    <%--<Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="2" />--%>
                                    <Scrolling AllowScroll="True" UseStaticHeaders="true" />
                                    <Selecting AllowRowSelect="true" />
                                    <Resizing AllowColumnResize="true" ClipCellContentOnResize="true" ResizeGridOnColumnResize="true" />
                                </ClientSettings>

                                <MasterTableView>
                                    <CommandItemSettings ExportToPdfText="Export to PDF" />
                                    <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                                    </RowIndicatorColumn>
                                    <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                                    </ExpandCollapseColumn>
                                    <Columns>
                                        <telerik:GridTemplateColumn AllowFiltering="False" DataField="Primary" FilterControlAltText="Filter Primary column"
                                            Groupable="False" HeaderText="Select Patient" Resizable="False" UniqueName="Primary">
                                            <ItemTemplate >
                                                <asp:CheckBox ID="Primary" runat="server" />
                                            </ItemTemplate>
                                            <HeaderStyle Wrap="true" Width="70px" CssClass="Gridheaderstyle" />
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridBoundColumn DataField="Acc#" ItemStyle-CssClass="Gridbodystyle" FilterControlAltText="Filter Acc column"
                                            HeaderText="Acc#" UniqueName="Acc">                                          
                                            <HeaderStyle CssClass="Gridbodystyle"  Width="70px" />
                                            <ItemStyle CssClass="Gridbodystyle"  Width="70px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="PatientName" FilterControlAltText="Filter PatientName column" ItemStyle-CssClass="Gridbodystyle"
                                            HeaderText="Patient Name" UniqueName="PatientName">
                                            <HeaderStyle CssClass="Gridbodystyle"  Width="150px" />
                                            <ItemStyle CssClass="Gridbodystyle"  Width="150px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="DOB" FilterControlAltText="Filter DOB column" ItemStyle-CssClass="Gridbodystyle"
                                            HeaderText="DOB" UniqueName="DOB">
                                            <HeaderStyle CssClass="Gridbodystyle"  Width="100px" />
                                            <ItemStyle CssClass="Gridbodystyle"  Width="100px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="CommMode" HeaderText="CommMode" UniqueName="CommMode">
                                           <HeaderStyle CssClass="Gridbodystyle"  Width="100px" />
                                            <ItemStyle CssClass="Gridbodystyle"  Width="100px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Pref Lang" HeaderText="Pref Lang Name" UniqueName="PrefLang">
                                            <HeaderStyle CssClass="Gridbodystyle"  Width="150px" />
                                            <ItemStyle CssClass="Gridbodystyle"  Width="150px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="HomePh#" HeaderText="Home Phone" UniqueName="HomePh">
                                           <HeaderStyle CssClass="Gridbodystyle"  Width="120px" />
                                            <ItemStyle CssClass="Gridbodystyle"  Width="120px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="CellPh#" HeaderText="Cell Phone" UniqueName="CellPh">
                                          <HeaderStyle CssClass="Gridbodystyle"  Width="120px" />
                                            <ItemStyle CssClass="Gridbodystyle"  Width="120px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="WorkPh#" HeaderText="Work Phone" UniqueName="WorkPh">
                                           <HeaderStyle CssClass="Gridbodystyle"  Width="120px" />
                                            <ItemStyle CssClass="Gridbodystyle"  Width="120px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="CreatedDateTime#" HeaderText="Created Date" UniqueName="CreatedDateTime">
                                            <HeaderStyle CssClass="Gridbodystyle"  Width="160px" />
                                            <ItemStyle CssClass="Gridbodystyle"  Width="160px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Email" HeaderText="Email" UniqueName="Email">
                                            <HeaderStyle CssClass="Gridbodystyle"  Width="100px" />
                                            <ItemStyle CssClass="Gridbodystyle"  Width="100px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Gender" HeaderText="Gender" UniqueName="Gender">
                                            <HeaderStyle CssClass="Gridbodystyle"  Width="80px" />
                                            <ItemStyle CssClass="Gridbodystyle"  Width="80px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Ethnicity" HeaderText="Ethnicity" UniqueName="Ethnicity">
                                            <HeaderStyle CssClass="Gridbodystyle"  Width="100px" />
                                            <ItemStyle CssClass="Gridbodystyle"  Width="100px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Race" HeaderText="Race" UniqueName="Race">
                                            <HeaderStyle CssClass="Gridbodystyle"  Width="120px" />
                                            <ItemStyle CssClass="Gridbodystyle"  Width="120px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="PreferredLang" HeaderText="Preferred Lang" UniqueName="PreferredLang">
                                            <HeaderStyle CssClass="Gridbodystyle"  Width="120px" />
                                            <ItemStyle CssClass="Gridbodystyle"  Width="120px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Problem" HeaderText="Problem" UniqueName="Problem">
                                            <HeaderStyle CssClass="Gridbodystyle"  Width="150px" />
                                            <ItemStyle CssClass="Gridbodystyle"  Width="150px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="ProblemDateTime" HeaderText="Problem Date" UniqueName="ProblemDateTime">
                                            <HeaderStyle CssClass="Gridbodystyle"  Width="150px" />
                                            <ItemStyle CssClass="Gridbodystyle"  Width="150px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Medication" HeaderText="Medication" UniqueName="Medication">
                                           <HeaderStyle CssClass="Gridbodystyle"  Width="120px" />
                                            <ItemStyle CssClass="Gridbodystyle"  Width="120px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="MedicationDateTime#" HeaderText="Medication Date" UniqueName="MedicationDateTime" DataFormatString="{0:dd-MM-yyyy}">
                                           <HeaderStyle CssClass="Gridbodystyle"  Width="150px" />
                                            <ItemStyle CssClass="Gridbodystyle"  Width="150px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="MedAlergy" HeaderText="Med Allergy" UniqueName="MedAlergy">
                                            <HeaderStyle CssClass="Gridbodystyle"  Width="100px" />
                                            <ItemStyle CssClass="Gridbodystyle"  Width="100px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="MedAlergyDateTime#" HeaderText="Med Allergy Date" UniqueName="MedAlergyDateTime" DataFormatString="{0:dd-MM-yyyy}">
                                          <HeaderStyle CssClass="Gridbodystyle"  Width="150px" />
                                            <ItemStyle CssClass="Gridbodystyle"  Width="150px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="LabResult" HeaderText="Lab Result" UniqueName="LabResult">
                                           <HeaderStyle CssClass="Gridbodystyle"  Width="100px" />
                                            <ItemStyle CssClass="Gridbodystyle"  Width="100px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="LabResultDateTime#" HeaderText="Lab Result Date" UniqueName="LabResultDateTime" DataFormatString="{0:dd-MM-yyyy}">
                                           <HeaderStyle CssClass="Gridbodystyle"  Width="150px" />
                                            <ItemStyle CssClass="Gridbodystyle"  Width="150px" />
                                        </telerik:GridBoundColumn>
                                    </Columns>
                                    <EditFormSettings>
                                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                        </EditColumn>
                                    </EditFormSettings>
                                </MasterTableView><AlternatingItemStyle BorderStyle="None" />
                            </telerik:RadGrid>

                            <%--<telerik:RadGrid ID="grdPatientReminder" runat="server" AutoGenerateColumns="False"
                                Height="335px" Width="1130px" CellSpacing="0" GridLines="Both"    AllowSorting="true" CssClass="Gridbodystyle">
                                <FilterMenu EnableImageSprites="False">
                                </FilterMenu>
                                <HeaderStyle CssClass="Gridheaderstyle" />
                                <SelectedItemStyle Font-Bold="true" BorderWidth="1px" BorderStyle="Dashed" />
                                <ClientSettings>

                                    <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="2" />
                                    <Selecting AllowRowSelect="true" />
                                    <Resizing AllowColumnResize="true" ClipCellContentOnResize="true" ResizeGridOnColumnResize="true" />
                                </ClientSettings>
                                <MasterTableView Width="100%" Height="335px" TableLayout="Fixed">
                                    <CommandItemSettings ExportToPdfText="Export to PDF" />
                                    <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                                    </RowIndicatorColumn>
                                    <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                                    </ExpandCollapseColumn>
                                    <Columns>

                                        <telerik:GridTemplateColumn AllowFiltering="False" DataField="Primary" FilterControlAltText="Filter Primary column"
                                            Groupable="False" HeaderText="Select Patient" Resizable="False" UniqueName="Primary">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="Primary" runat="server" />
                                            </ItemTemplate>
                                            <HeaderStyle Wrap="true" Width="50px" CssClass="Gridheaderstyle" />

                                        </telerik:GridTemplateColumn>


                                        <telerik:GridBoundColumn DataField="Acc#" FilterControlAltText="Filter Acc# column"
                                            HeaderText="Acc#" UniqueName="Acc#">
                                            <HeaderStyle Wrap="true" Width="50px" CssClass="Gridheaderstyle" />
                                            <ItemStyle Font-Bold="false"  />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="PatientName" FilterControlAltText="PatientName"
                                            HeaderText="PatientName"
                                            UniqueName="PatientName">

                                            <HeaderStyle Width="100px" CssClass="Gridheaderstyle" />
                                           <ItemStyle Font-Bold="false"  />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="DOB" FilterControlAltText="DOB" DataFormatString="{0:dd-MM-yyyy}" HeaderText="DOB" UniqueName="DOB">
                                            <HeaderStyle Width="100px" />
                                            <ItemStyle Font-Bold="false"  />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="CommMode" FilterControlAltText="Filter CommMode column"
                                            HeaderText="CommMode"
                                            UniqueName="CommMode">
                                            <HeaderStyle Width="100px" CssClass="Gridheaderstyle" />
                                            <ItemStyle Font-Bold="false"  />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Pref Lang" FilterControlAltText="Filter PrefLang column"
                                            HeaderText="Pref Lang Name"
                                            UniqueName="PrefLang">

                                            <HeaderStyle Width="100px" CssClass="Gridheaderstyle" />
                                            <ItemStyle Font-Bold="false"  />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridDateTimeColumn DataField="HomePh#" FilterControlAltText="Filter HomePh# column"
                                            HeaderText="HomePh#"
                                            UniqueName="HomePh#">

                                            <HeaderStyle Width="100px" CssClass="Gridheaderstyle" />
                                            <ItemStyle Font-Bold="false"  />
                                        </telerik:GridDateTimeColumn>
                                        <telerik:GridBoundColumn DataField="CellPh#" FilterControlAltText="Filter CellPh# column"
                                            HeaderText="CellPh#"
                                            UniqueName="CellPh#">

                                            <HeaderStyle Width="100px" CssClass="Gridheaderstyle" />
                                            <ItemStyle Font-Bold="false"  />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="WorkPh#" FilterControlAltText="Filter WorkPh# column"
                                            HeaderText="WorkPh#"
                                            UniqueName="WorkPh#">

                                            <HeaderStyle Width="100px" CssClass="Gridheaderstyle" />
                                            <ItemStyle Font-Bold="false"  />
                                        </telerik:GridBoundColumn>

                                        <telerik:GridDateTimeColumn DataField="CreatedDateTime#" FilterControlAltText="Filter CreatedDateTime# column"
                                            HeaderText="Created DateTime#"
                                            UniqueName="CreatedDateTime#" DataFormatString="{0:dd-MM-yyyy}">

                                            <HeaderStyle Width="100px" CssClass="Gridheaderstyle" />
                                            <ItemStyle Font-Bold="false"  />
                                        </telerik:GridDateTimeColumn>


                                        <telerik:GridBoundColumn DataField="Email" FilterControlAltText="Filter Email column"
                                            HeaderText="Email"
                                            UniqueName="Email">
                                            <HeaderStyle Width="100px" CssClass="Gridheaderstyle" />
                                            <ItemStyle Font-Bold="false"  />
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="Gender" FilterControlAltText="Filter Gender column"
                                            HeaderText="Gender"
                                            UniqueName="Gender">

                                            <HeaderStyle Width="100px" CssClass="Gridheaderstyle" />
                                            <ItemStyle Font-Bold="false"  />
                                        </telerik:GridBoundColumn>


                                        <telerik:GridBoundColumn DataField="Ethnicity" FilterControlAltText="Filter Ethnicity column"
                                            HeaderText="Ethnicity"
                                            UniqueName="Ethnicity">

                                            <HeaderStyle Width="100px" CssClass="Gridheaderstyle" />
                                           <ItemStyle Font-Bold="false"  />
                                        </telerik:GridBoundColumn>


                                        <telerik:GridBoundColumn DataField="Race" FilterControlAltText="Filter Race column"
                                            HeaderText="Race"
                                            UniqueName="Race">

                                            <HeaderStyle Width="100px" CssClass="Gridheaderstyle" />
                                            <ItemStyle Font-Bold="false"  />
                                        </telerik:GridBoundColumn>


                                        <telerik:GridBoundColumn DataField="PreferredLang" FilterControlAltText="Filter PreferredLang column"
                                            HeaderText="PreferredLang"
                                            UniqueName="PreferredLang">

                                            <HeaderStyle Width="100px" CssClass="Gridheaderstyle" />
                                            <ItemStyle Font-Bold="false"  />
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="Problem" FilterControlAltText="Filter Problem column"
                                            HeaderText="Problem"
                                            UniqueName="Problem">

                                            <HeaderStyle Width="100px" CssClass="Gridheaderstyle" />
                                            <ItemStyle Font-Bold="false"  />
                                        </telerik:GridBoundColumn>


                                        <telerik:GridBoundColumn DataField="ProblemDateTime#" FilterControlAltText="Filter ProblemDateTime# column"
                                            HeaderText="Problem DateTime#"
                                            UniqueName="ProblemDateTime#">

                                            <HeaderStyle Width="100px" CssClass="Gridheaderstyle" />
                                            <ItemStyle Font-Bold="false"  />
                                        </telerik:GridBoundColumn>


                                        <telerik:GridBoundColumn DataField="Medication" FilterControlAltText="Filter Medication column"
                                            HeaderText="Medication"
                                            UniqueName="Medication">

                                            <HeaderStyle Width="100px" CssClass="Gridheaderstyle" />
                                            <ItemStyle Font-Bold="false"  />
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="MedicationDateTime#" FilterControlAltText="Filter MedicationDateTime# column"
                                            HeaderText="Medication DateTime#"
                                            UniqueName="MedicationDateTime#" DataFormatString="{0:dd-MM-yyyy}">

                                            <HeaderStyle Width="100px" CssClass="Gridheaderstyle" />
                                           <ItemStyle Font-Bold="false"  />
                                        </telerik:GridBoundColumn>


                                        <telerik:GridBoundColumn DataField="MedAlergy" FilterControlAltText="Filter MedAlergy column"
                                            HeaderText="MedAlergy"
                                            UniqueName="MedAlergy">

                                            <HeaderStyle Width="100px" CssClass="Gridheaderstyle" />
                                           <ItemStyle Font-Bold="false"  />
                                        </telerik:GridBoundColumn>


                                        <telerik:GridBoundColumn DataField="MedAlergyDateTime#" FilterControlAltText="Filter MedAlergyDateTime# column"
                                            HeaderText="MedAlergy DateTime#"
                                            UniqueName="MedAlergyDateTime#" DataFormatString="{0:dd-MM-yyyy}">

                                            <HeaderStyle Width="100px" CssClass="Gridheaderstyle" />
                                           <ItemStyle Font-Bold="false"  />
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="LabResult" FilterControlAltText="Filter LabResult column"
                                            HeaderText="LabResult"
                                            UniqueName="LabResult">

                                            <HeaderStyle Width="100px" CssClass="Gridheaderstyle" />
                                            <ItemStyle Font-Bold="false"  />
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="LabResultDateTime#" FilterControlAltText="Filter LabResultDateTime# column"
                                            HeaderText="LabResult DateTime#"
                                            UniqueName="LabResultDateTime#" DataFormatString="{0:dd-MM-yyyy}">

                                            <HeaderStyle Width="100px" CssClass="Gridheaderstyle" />
                                            <ItemStyle Font-Bold="false"  />
                                        </telerik:GridBoundColumn>

                                    </Columns>
                                    <EditFormSettings>
                                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                        </EditColumn>
                                    </EditFormSettings>
                                </MasterTableView>
                            </telerik:RadGrid>--%>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="5" style="width: 100%; background-color: White; font-size: small; font-family: Microsoft Sans Serif; font-size: 8.5pt"
                        valign="top">
                        <PageNavigator:PageNavigator ID="mpnHospitalizationHistory" runat="server" OnFirst="FirstPageNavigator"
                            Visible="true" />
                    </td>
                </tr>

                <tr>
                    <td style="width: 10%" align="right">
                        <telerik:RadComboBox ID="cbocommunicat" runat="server" CssClass="Editabletxtbox">
                        </telerik:RadComboBox>
                    </td>
                    <td style="width: 10%;">
                        <telerik:RadButton ID="btnSendMessage" runat="server" Text="Send Reminder" style="height: 32px !important;"
                            Width="160px" OnClick="btnSendMessage_Click" ButtonType="LinkButton" CssClass="bluebutton teleriknormalbuttonstyle">
                        </telerik:RadButton>

                    </td>

                    <td style="width: 10%">
                        <telerik:RadButton ID="btnPrintExcel" runat="server" Visible="true" Text="Print Excel" style="height: 32px !important; margin-left: -24px"
                            Width="155px" ButtonType="LinkButton"  onclick="btnPrintExcel_Click" CssClass="bluebutton teleriknormalbuttonstyle">
                        </telerik:RadButton>
                    </td>

                    <td style="width: 20%" align="right">
                        <telerik:RadButton ID="btnPrintinEnglish" runat="server" Visible="false" Text="Print Letter in English"
                            Width="100%" OnClick="btnPrintinEnglish_Click">
                        </telerik:RadButton>
                    </td>
                    <td style="width: 10%; display: none;" align="right">
                        <telerik:RadButton ID="btnPrintPdf" runat="server" Visible="false" Text="Print PDF" Width="150%"
                            OnClick="btnPrintPdf_Click">
                        </telerik:RadButton>
                    </td>
                    <td style="width: 10%; display: none;" align="right"></td>
                    <td style="width: 10%;" align="right">
                        <telerik:RadButton ID="btnClose" runat="server" Text="Close" Width="50%" OnClientClicked="Close" style="height: 30px !important;" ButtonType="LinkButton" CssClass="redbutton teleriknormalbuttonstyle">
                        </telerik:RadButton>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divLoading" class="modal" runat="server" style="text-align: center; display: none">
            <asp:Panel ID="Panel2" runat="server">
                <br />
                <br />
                <br />
                <br />
                <center>
                    <asp:Label ID="Label1" Text="" runat="server"></asp:Label></center>
                <br />
                <img src="Resources/wait.ico" title="" alt="Loading..." />
                <br />
            </asp:Panel>
        </div>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
            <script src="JScripts/JSPatientReminderReport.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
        </asp:PlaceHolder>
        <asp:HiddenField ID="hdnRuleID" runat="server" />
        <asp:HiddenField ID="hdnRule" runat="server" />
        <asp:HiddenField ID="SelectedItem" runat="server" />
        <asp:Button ID="btnRule" runat="server" OnClick="btnRule_Click" Style="display: none" />
        <telerik:RadScriptManager ID="scriptMngr" runat="server">
        </telerik:RadScriptManager>
    </form>
</body>
</html>
