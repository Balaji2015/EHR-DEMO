<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmClinicalInformation.aspx.cs" Inherits="Acurus.Capella.UI.frmClinicalInformation" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Clinical Information</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
        .Panel legend {
            font-weight: bold;
        }

        .rgDataDiv {
            height: 170px !important;
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

        .ddlStyle {
            border-color: #ccc!important;
            color: #333!important;
            border-radius: 2px!important;
            font: 12px / 16px "segoe ui",arial,sans-serif!important;
        }
    </style>

</head>
<body onload=" {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}">
    <form id="form1" runat="server" style="background-color: White;">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI"
                    Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI"
                    Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI"
                    Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
            </Scripts>
        </telerik:RadScriptManager>
        <div>
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="900px"
                Width="100%" HorizontalAlign="NotSet">
                <table style="width: 100%; height: 100%">
                    <tr style="height: 5%">
                        <td>
                            <asp:Panel ID="Panel2" runat="server" Width="100%" Height="100%" GroupingText="Patient Information">
                                <table style="width: 100%; height: 100%">
                                    <tr>
                                        <td width="10%">
                                            <asp:Label ID="lblHumanId" runat="server" Text="Patient Id" Width="100%"></asp:Label>
                                        </td>
                                        <td width="15%">
                                            <telerik:RadTextBox ID="txtHumanId" runat="server" Width="100%" BackColor="#BFDBFF" BorderColor="Black" ReadOnly="true">
                                            </telerik:RadTextBox>
                                        </td>
                                        <td width="10%">
                                            <asp:Label ID="lblHumanName" runat="server" Text="Patient Name" Width="100%"></asp:Label>
                                        </td>
                                        <td width="20%">
                                            <telerik:RadTextBox ID="txtHumanName" runat="server" Width="100%" BackColor="#BFDBFF" BorderColor="Black" ReadOnly="true">
                                            </telerik:RadTextBox>
                                        </td>
                                        <td width="10%">
                                            <asp:Label ID="lblSex" runat="server" Text="Sex" Width="100%"></asp:Label>
                                        </td>
                                        <td width="10%">
                                            <telerik:RadTextBox ID="txtSex" runat="server" Width="100%" BackColor="#BFDBFF" BorderColor="Black" ReadOnly="true">
                                            </telerik:RadTextBox>
                                        </td>
                                        <td width="10%">
                                            <asp:Label ID="lblDOB" runat="server" Text="Patient DOB" Width="100%"></asp:Label>
                                        </td>
                                        <td width="15%">
                                            <telerik:RadTextBox ID="txtDOB" runat="server" Width="100%" BackColor="#BFDBFF" BorderColor="Black" ReadOnly="true">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr style="height: 5%">
                        <td>
                            <asp:Panel ID="Panel1" runat="server" Width="100%" Height="100%">
                                <table style="width: 100%; height: 100%">
                                    <tr>
                                        <td width="40%"></td>
                                        <td width="10%">
                                            <asp:Label ID="lblXml" runat="server" Text="Incorporate CCD" Width="100%"></asp:Label>
                                        </td>
                                        <td width="20%">
                                            <telerik:RadAsyncUpload ID="UploadImage" runat="server" Width="100%" Height="100%"
                                                MultipleFileSelection="Disabled" MaxFileInputsCount="1">
                                            </telerik:RadAsyncUpload>
                                        </td>
                                        <td width="10%">
                                            <telerik:RadButton ID="btnLoad" runat="server" Text="Load" Width="100%"
                                                OnClick="btnLoad_Click">
                                            </telerik:RadButton>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr style="height: 29%">
                        <td>
                            <asp:Panel ID="pnlMedicationList" runat="server" Width="100%" Height="100%" GroupingText="Medication List">
                                <table style="width: 100%; height: 100%">
                                    <tr>
                                        <td width="30%">
                                            <asp:Panel ID="Panel9" runat="server" GroupingText="Actual List" Height="100%" Width="100%">
                                                <telerik:RadGrid ID="grdMedicationActualList" runat="server" Height="220px" AutoGenerateColumns="False" Skin="Vista" CellSpacing="0" GridLines="None" Width="330px">
                                                    <ClientSettings EnablePostBackOnRowClick="True">
                                                        <Selecting AllowRowSelect="True" />
                                                        <Scrolling AllowScroll="True" ScrollHeight="100px" UseStaticHeaders="True" />
                                                    </ClientSettings>
                                                    <MasterTableView>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn>
                                                               
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="ChkMedicationActual" runat="server" AutoPostBack="true" />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridBoundColumn FilterControlAltText="Filter column column"
                                                                HeaderText="Name" UniqueName="column" DataField="Name">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn FilterControlAltText="Filter column1 column"
                                                                HeaderText="Status" UniqueName="column1" DataField="Status">
                                                            </telerik:GridBoundColumn>

                                                            <telerik:GridBoundColumn FilterControlAltText="Filter column2 column"
                                                                HeaderText="Last Modified Date" UniqueName="column2"
                                                                DataField="Last Modified Date">
                                                            </telerik:GridBoundColumn>

                                                            <telerik:GridBoundColumn DataField="Rcopia Id"
                                                                FilterControlAltText="Filter column3 column" HeaderText="Rcopia Id"
                                                                UniqueName="column3" Display="False">
                                                            </telerik:GridBoundColumn>

                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </asp:Panel>

                                        </td>
                                        <td width="30%">
                                            <asp:Panel ID="Panel10" runat="server" GroupingText="Incorporated List"
                                                Height="100%" Width="100%">
                                                <telerik:RadGrid ID="grdMedicationIncorporetedList" runat="server" Height="220px" AutoGenerateColumns="False" Skin="Vista" CellSpacing="0" GridLines="None" Width="330px">
                                                    <ClientSettings EnablePostBackOnRowClick="True">
                                                        <Selecting AllowRowSelect="True" />
                                                        <Scrolling AllowScroll="True" ScrollHeight="100px" UseStaticHeaders="True" />
                                                    </ClientSettings>
                                                    <MasterTableView>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn>
                                                                
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkMedicationIncorporated" runat="server" AutoPostBack="true" />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridBoundColumn FilterControlAltText="Filter column column"
                                                                HeaderText="Name" UniqueName="column" DataField="Name">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn FilterControlAltText="Filter column1 column"
                                                                HeaderText="Status" UniqueName="column1" DataField="Status">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn FilterControlAltText="Filter column2 column"
                                                                HeaderText="Last Modified Date" UniqueName="column2"
                                                                DataField="Last Modified Date">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="Rcopia Id" Display="False"
                                                                FilterControlAltText="Filter column3 column" HeaderText="Rcopia Id"
                                                                UniqueName="column3">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="Data From" Display="False"
                                                                FilterControlAltText="Filter column4 column" HeaderText="data From"
                                                                UniqueName="column4">
                                                            </telerik:GridBoundColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </asp:Panel>
                                        </td>
                                        <td width="10%">
                                            <telerik:RadButton ID="btnMedicationMerge" runat="server" Text="-->"
                                                Width="100%" OnClick="btnMedicationMerge_Click"
                                                OnClientClicked="btnMedicationMerge_Clicked">
                                            </telerik:RadButton>
                                            <telerik:RadButton ID="btnMedicationRecall" runat="server" Text="<--"
                                                Width="100%" OnClick="btnMedicationMerge_Recall"
                                                OnClientClicked="btnAllergyMerge_Clicked">
                                            </telerik:RadButton>
                                        </td>
                                        <td width="30%">
                                            <asp:Panel ID="Panel11" runat="server" GroupingText="Reconciled List"
                                                Height="100%" Width="100%">
                                                <telerik:RadGrid ID="grdMedicationMergedList" runat="server" Height="220px" AutoGenerateColumns="False" Skin="Vista" CellSpacing="0" GridLines="None" Width="330px">
                                                    <ClientSettings EnablePostBackOnRowClick="True">
                                                        <Selecting AllowRowSelect="True" />
                                                        <Scrolling AllowScroll="True" ScrollHeight="100px" UseStaticHeaders="True" />
                                                    </ClientSettings>
                                                    <MasterTableView>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn>
                                                               
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkMedicationMerged" runat="server" AutoPostBack="true" />
                                                                </ItemTemplate>

                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridBoundColumn FilterControlAltText="Filter column column"
                                                                HeaderText="Name" UniqueName="column" DataField="Name">
                                                            </telerik:GridBoundColumn>
                                                            
                                                            <telerik:GridTemplateColumn HeaderText="Status">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="ddlMedicationStatus" DataTextField="Status" DataValueField="Status"
                                                                        runat="server" CssClass="ddlStyle">
                                                                        <asp:ListItem Text="Active" Value="Active"></asp:ListItem>
                                                                        <asp:ListItem Text="Inactive" Value="Inactive"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridBoundColumn FilterControlAltText="Filter column2 column"
                                                                HeaderText="Last Modified Date" UniqueName="column2"
                                                                DataField="Last Modified Date">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="Rcopia Id" Display="False"
                                                                FilterControlAltText="Filter column3 column" HeaderText="Rcopia Id"
                                                                UniqueName="column3">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="Data From" Display="False"
                                                                FilterControlAltText="Filter column4 column" HeaderText="data From"
                                                                UniqueName="column4">
                                                            </telerik:GridBoundColumn>

                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </asp:Panel>
                                        </td>
                                    </tr>

                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr style="height: 28%">
                        <td>
                            <asp:Panel ID="PnlMedicationAllergy" runat="server" Width="100%" Height="100%" GroupingText="Medication Allergy List">
                                <table style="width: 100%; height: 100%">
                                    <tr>
                                        <td width="30%">
                                            <asp:Panel ID="Panel6" runat="server" GroupingText="Actual List" Height="100%" Width="100%">
                                                <telerik:RadGrid ID="grdAllergyActualList" runat="server" Height="220px" AutoGenerateColumns="False" Skin="Vista" CellSpacing="0" GridLines="None" Width="330px">
                                                    <ClientSettings EnablePostBackOnRowClick="True">
                                                        <Selecting AllowRowSelect="True" />
                                                        <Scrolling AllowScroll="True" ScrollHeight="100px" UseStaticHeaders="True" />
                                                    </ClientSettings>
                                                    <MasterTableView>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn>
                                                               
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkAllergyActual" runat="server" AutoPostBack="true" />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridBoundColumn FilterControlAltText="Filter column column"
                                                                HeaderText="Name" UniqueName="column" DataField="Name">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="Reaction"
                                                                FilterControlAltText="Filter column3 column" HeaderText="Reaction"
                                                                UniqueName="column3">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn FilterControlAltText="Filter column1 column"
                                                                HeaderText="Status" UniqueName="column1" DataField="Status">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn FilterControlAltText="Filter column2 column"
                                                                HeaderText="Last Modified Date" UniqueName="column2"
                                                                DataField="Last Modified Date">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="Rcopia Id" Display="False"
                                                                FilterControlAltText="Filter column4 column" HeaderText="Rcopia Id"
                                                                UniqueName="column4">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="Data From" Display="False"
                                                                FilterControlAltText="Filter column4 column" HeaderText="data From"
                                                                UniqueName="column4">
                                                            </telerik:GridBoundColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </asp:Panel>

                                        </td>
                                        <td width="30%">
                                            <asp:Panel ID="Panel7" runat="server" GroupingText="Incorporated List"
                                                Height="100%" Width="100%">
                                                <telerik:RadGrid ID="grdAllergyIncorporetedList" runat="server" Height="220px" AutoGenerateColumns="False" Skin="Vista" CellSpacing="0" GridLines="None" Width="330px">
                                                    <ClientSettings EnablePostBackOnRowClick="True">
                                                        <Selecting AllowRowSelect="True" />
                                                        <Scrolling AllowScroll="True" ScrollHeight="100px" UseStaticHeaders="True" />
                                                    </ClientSettings>
                                                    <MasterTableView>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn>
                                                                
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkAllergyIncorporated" runat="server" AutoPostBack="true" />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridBoundColumn FilterControlAltText="Filter column column"
                                                                HeaderText="Name" UniqueName="column" DataField="Name">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn FilterControlAltText="Filter column3 column"
                                                                HeaderText="Reaction" UniqueName="column3" DataField="Reaction">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn FilterControlAltText="Filter column1 column"
                                                                HeaderText="Status" UniqueName="column1" DataField="Status">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn FilterControlAltText="Filter column2 column"
                                                                HeaderText="Last Modified Date" UniqueName="column2"
                                                                DataField="Last Modified Date">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="Rcopia Id" Display="False"
                                                                FilterControlAltText="Filter column4 column" HeaderText="Rcopia Id"
                                                                UniqueName="column4">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="Data From" Display="False"
                                                                FilterControlAltText="Filter column4 column" HeaderText="data From"
                                                                UniqueName="column4">
                                                            </telerik:GridBoundColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </asp:Panel>
                                        </td>
                                        <td width="10%">
                                            <telerik:RadButton ID="btnAllergyMerge" runat="server" Text="-->"
                                                Width="100%" OnClick="btnAllergyMerge_Click"
                                                OnClientClicked="btnAllergyMerge_Clicked">
                                            </telerik:RadButton>
                                            <telerik:RadButton ID="btnAllergyRecall" runat="server" Text="<--"
                                                Width="100%" OnClick="btnAllergyMerge_Recall"
                                                OnClientClicked="btnAllergyMerge_Clicked">
                                            </telerik:RadButton>
                                        </td>
                                        <td width="30%">
                                            <asp:Panel ID="Panel8" runat="server" GroupingText="Reconciled List" Height="100%"
                                                Width="100%">
                                                <telerik:RadGrid ID="grdAllergyMergedList" runat="server" Height="220px" AutoGenerateColumns="False" Skin="Vista" CellSpacing="0" GridLines="None" Width="330px">
                                                    <ClientSettings EnablePostBackOnRowClick="True">
                                                        <Selecting AllowRowSelect="True" />
                                                        <Scrolling AllowScroll="True" ScrollHeight="100px" UseStaticHeaders="True" />
                                                    </ClientSettings>
                                                    <MasterTableView>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn>
                                                               
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkAllergyMerged" runat="server" AutoPostBack="true" />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridBoundColumn FilterControlAltText="Filter column column"
                                                                HeaderText="Name" UniqueName="column" DataField="Name">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn FilterControlAltText="Filter column3 column"
                                                                HeaderText="Reaction" UniqueName="column3" DataField="Reaction">
                                                            </telerik:GridBoundColumn>
                                                         
                                                             <telerik:GridTemplateColumn HeaderText="Status">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="ddlAllergyStatus" DataTextField="Status" DataValueField="Status"
                                                                        runat="server" CssClass="ddlStyle">
                                                                        <asp:ListItem Text="Active" Value="Active"></asp:ListItem>
                                                                        <asp:ListItem Text="Inactive" Value="Inactive"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridBoundColumn FilterControlAltText="Filter column2 column"
                                                                HeaderText="Last Modified Date" UniqueName="column2"
                                                                DataField="Last Modified Date">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="Rcopia Id" Display="False"
                                                                FilterControlAltText="Filter column4 column" HeaderText="Rcopia Id"
                                                                UniqueName="column4">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="ListFrom" Display="False"
                                                                FilterControlAltText="Filter column3 column" HeaderText="ListFrom"
                                                                UniqueName="column3">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="Data From" Display="False"
                                                                FilterControlAltText="Filter column4 column" HeaderText="data From"
                                                                UniqueName="column4">
                                                            </telerik:GridBoundColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </asp:Panel>
                                        </td>
                                    </tr>

                                </table>
                            </asp:Panel>
                        </td>

                    </tr>
                    <tr style="height: 28%">
                        <td>
                            <asp:Panel ID="pnlProblemLIst" runat="server" Width="100%" Height="100%" GroupingText="Problem List">
                                <table style="width: 100%; height: 100%">
                                    <tr>
                                        <td width="30%">
                                            <asp:Panel ID="pnlProblemActualList" runat="server" GroupingText="Actual List" Height="100%" Width="100%">
                                                <telerik:RadGrid ID="grdProblemActualList" runat="server" Height="220px" AutoGenerateColumns="False" Skin="Vista" CellSpacing="0" GridLines="None" Width="330px">
                                                    <ClientSettings EnablePostBackOnRowClick="True">
                                                        <Selecting AllowRowSelect="True" />
                                                        <Scrolling AllowScroll="True" ScrollHeight="100px" UseStaticHeaders="True" />
                                                    </ClientSettings>
                                                    <MasterTableView>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn>
                                                               
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkProblemActual" runat="server" AutoPostBack="true" />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridBoundColumn FilterControlAltText="Filter column column"
                                                                HeaderText="Name" UniqueName="column" DataField="Name">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn FilterControlAltText="Filter column1 column"
                                                                HeaderText="Status" UniqueName="column1" DataField="Status">
                                                            </telerik:GridBoundColumn>

                                                            <telerik:GridBoundColumn FilterControlAltText="Filter column2 column"
                                                                HeaderText="Last Modified Date" UniqueName="column2"
                                                                DataField="Last Modified Date">
                                                            </telerik:GridBoundColumn>

                                                            <telerik:GridBoundColumn DataField="Problem ID" Display="False"
                                                                FilterControlAltText="Filter column3 column" HeaderText="Problem ID"
                                                                UniqueName="column3">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="Data From" Display="False"
                                                                FilterControlAltText="Filter column4 column" HeaderText="data From"
                                                                UniqueName="column4">
                                                            </telerik:GridBoundColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </asp:Panel>

                                        </td>
                                        <td width="30%">
                                            <asp:Panel ID="pnlProblemIncorporatedList" runat="server"
                                                GroupingText="Incorporated List" Height="100%" Width="100%">
                                                <telerik:RadGrid ID="grdProblemIncorporatedList" runat="server" Height="220px" AutoGenerateColumns="False" Skin="Vista" CellSpacing="0" GridLines="None" Width="330px">
                                                    <ClientSettings EnablePostBackOnRowClick="True">
                                                        <Selecting AllowRowSelect="True" />
                                                        <Scrolling AllowScroll="True" ScrollHeight="100px" UseStaticHeaders="True" />
                                                    </ClientSettings>
                                                    <MasterTableView>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn>
                                                                
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkProblemIncorporated" runat="server" AutoPostBack="true" />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridBoundColumn FilterControlAltText="Filter column column"
                                                                HeaderText="Name" UniqueName="column" DataField="Name">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn FilterControlAltText="Filter column1 column"
                                                                HeaderText="Status" UniqueName="column1" DataField="Status">
                                                            </telerik:GridBoundColumn>
                                                           
                                                            <telerik:GridBoundColumn FilterControlAltText="Filter column2 column"
                                                                HeaderText="Last Modified Date" UniqueName="column2"
                                                                DataField="Last Modified Date">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="Problem ID" Display="False"
                                                                FilterControlAltText="Filter column3 column" HeaderText="Problem ID"
                                                                UniqueName="column3">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="Data From" Display="False"
                                                                FilterControlAltText="Filter column4 column" HeaderText="data From"
                                                                UniqueName="column4">
                                                            </telerik:GridBoundColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </asp:Panel>
                                        </td>
                                        <td width="10%">
                                            <telerik:RadButton ID="btnProblemMerge" runat="server" Text="-->"
                                                Width="100%" OnClick="btnProblemMerge_Click"
                                                OnClientClicked="btnProblemMerge_Clicked">
                                            </telerik:RadButton>
                                            <telerik:RadButton ID="btnProblemRecall" runat="server" Text="<--"
                                                Width="100%" OnClick="btnProblemMerge_Recall"
                                                OnClientClicked="btnProblemMerge_Clicked">
                                            </telerik:RadButton>
                                        </td>
                                        <td width="30%">
                                            <asp:Panel ID="pnlProblemMergedList" runat="server" GroupingText="Reconciled List"
                                                Height="100%" Width="100%">
                                                <telerik:RadGrid ID="grdProblemMergedList" runat="server" Height="220px" AutoGenerateColumns="False" Skin="Vista" CellSpacing="0" GridLines="None" Width="330px">
                                                    <ClientSettings EnablePostBackOnRowClick="True">
                                                        <Selecting AllowRowSelect="True" />
                                                        <Scrolling AllowScroll="True" ScrollHeight="100px" UseStaticHeaders="True" />
                                                    </ClientSettings>
                                                    <MasterTableView>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn>
                                                                
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkProblemMerged" runat="server" AutoPostBack="true" />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridBoundColumn FilterControlAltText="Filter column column"
                                                                HeaderText="Name" UniqueName="column" DataField="Name">
                                                            </telerik:GridBoundColumn>
                                                         
                                                             <telerik:GridTemplateColumn HeaderText="Status">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="ddlProblemStatus" DataTextField="Status" DataValueField="Status"
                                                                        runat="server" CssClass="ddlStyle">
                                                                        <asp:ListItem Text="Active" Value="Active"></asp:ListItem>
                                                                        <asp:ListItem Text="Resolved" Value="Resolved"></asp:ListItem>
                                                                        <asp:ListItem Text="Inactive" Value="Inactive"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>

                                                            <telerik:GridBoundColumn FilterControlAltText="Filter column2 column"
                                                                HeaderText="Last Modified Date" UniqueName="column2"
                                                                DataField="Last Modified Date">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="Problem ID" Display="False"
                                                                FilterControlAltText="Filter column3 column" HeaderText="Problem ID"
                                                                UniqueName="column3">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="ListFrom" Display="False"
                                                                FilterControlAltText="Filter column3 column" HeaderText="ListFrom"
                                                                UniqueName="column4">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="Data From" Display="False"
                                                                FilterControlAltText="Filter column4 column" HeaderText="data From"
                                                                UniqueName="column4">
                                                            </telerik:GridBoundColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </asp:Panel>
                                        </td>
                                    </tr>

                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr style="height: 5%">
                        <td>
                            <asp:Panel ID="Panel5" runat="server" Width="100%" Height="100%">
                                <table style="width: 100%; height: 100%">
                                    <tr>
                                        <td width="80%"></td>
                                        <td width="10%">
                                            <telerik:RadButton ID="btnSave" runat="server" Text="Save" Width="100%"
                                                OnClick="btnSave_Click" OnClientClicked="btnSave_Clicked">
                                            </telerik:RadButton>
                                        </td>
                                        <td width="10%">
                                            <telerik:RadButton ID="btnClose" runat="server" Text="Cancel" Width="100%"
                                                OnClientClicked="btnClose_Clicked">
                                            </telerik:RadButton>
                                        </td>
                                    </tr>
                                </table>
                                <asp:HiddenField ID="hdnLocalDate" runat="server" />
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
                <div id="divLoading" class="modal" runat="server" style="text-align: center; display: none">
                    <asp:Panel ID="Panel3" runat="server">
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
            </telerik:RadAjaxPanel>
        </div>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
            <script src="JScripts/JSClinicalInformation.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

        </asp:PlaceHolder>
    </form>
</body>
</html>
