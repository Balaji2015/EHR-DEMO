<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmLabException.aspx.cs"
    Inherits="Acurus.Capella.UI.frmLabException" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/UserControls/PageNavigator.ascx" TagName="PageNavigator" TagPrefix="PageNavigator" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Lab Exception</title>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
        .underline {
            text-decoration: underline;
        }

        .displayNone {
            display: none;
        }

        .modal {
            position: fixed;
            top: 0;
            left: 0;
            background-color: White;
            z-index: 99;
            -moz-opacity: 0.8;
            min-height: 100%;
            width: 100%;
        }

        #txtResultDt {
            font-size: Small;
            width: 150px;
            height: 20px;
            margin-left: 20px;
            border-radius: 4px;
        }

        #txtReasonNtMatching {
            font-size: Small;
            width: 235px;
            height: 20px;
            margin-left: 21px;
            border-radius: 4px;
        }

        #txtLabName {
            font-size: Small;
            width: 205px;
            margin-left: 22px;
            height: 20px;
            border-radius: 4px;
        }
        .RadPicker{
            padding-left:7px!important;
        }
        #lblLabName{
            padding-left:125px!important;
        }       
        #cboLabName{
            padding-left:70px!important;
        }
        #cboErrorReason{
            padding-left:30px!important;
        }
        #lblCategory{
            padding-left:90px!important;
        }
        #cboCategory{
            padding-left:20px!important;
        }
    </style>
    <link href="~/CSS/CommonStyle.css" rel="Stylesheet" type="text/css" />
</head>
<body onload="LabExcepLoad();">
    <form id="form1" runat="server">
        <telerik:RadWindowManager ID="WindowMngr" runat="server">
            <Windows>
                <telerik:RadWindow ID="MessageWindow" runat="server" Modal="true" Behaviors="Close"
                    Title="Encounter" IconUrl="Resources/16_16.ico">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="pnllabException" runat="server" Height="372px" CssClass="LabelStyleBold">
            <div>
                <table style="width: 100%">
                    <tr>
                        <td style="width: 100%; height: 30px">
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblSelectProvider" Font-Size="Small" runat="server" Text="Select Provider" CssClass="spanstyle"></asp:Label>
                                    </td>
                                    <td>
                                        <telerik:RadComboBox ID="cboProviderName" Font-Size="Small" Font-Bold="false" runat="server"
                                            Height="75px" Width="255px" />
                                        <asp:CheckBox ID="chkProviderName" Font-Size="Small" runat="server" Font-Bold="false"
                                            Text="Show All Physician" AutoPostBack="true" OnCheckedChanged="chkProviderName_CheckedChanged" />
                                    </td>
                                    <td>
                                        <telerik:RadButton ID="btnSearch" runat="server" Text="Search" Width="250px" AccessKey="A"
                                            Style="margin-left: 0px; top: 0px; left: 0px; font-size: 13px !important; height: 28px !important; position: static; text-align: center; -moz-border-radius: 3px; -webkit-border-radius: 3px; position: relative;" 
                                            OnClick="btnSearch_Click"
                                            OnClientClicked="btnSearch_Clicked" ButtonType="LinkButton" CssClass="bluebutton teleriknormalbuttonstyle">
                                            <ContentTemplate>
                                                <span id="SpanAdd" runat="server" >S</span><span id="SpanAdditionalword"
                                                    runat="server">earch</span>
                                            </ContentTemplate>
                                        </telerik:RadButton>
                                        &nbsp;&nbsp;&nbsp;
                                    <telerik:RadButton ID="btnViewPendingOrder" runat="server" AutoPostBack="false" EnableViewState="false"
                                        Text="View Pending Order For provider" Width="250px" AccessKey="l" Style="position: static; height: 28px !important; text-align: center; font-size: 13px !important; -moz-border-radius: 3px; -webkit-border-radius: 3px; position: relative;"
                                        OnClientClicked="btnViewPendingOrder_Clicked" ButtonType="LinkButton" CssClass="bluebutton teleriknormalbuttonstyle" >
                                        <ContentTemplate>
                                            <span id="SpanClear" runat="server" >V</span><span id="SpanClearAdditional"
                                                runat="server">iew Pending Orders for Provider</span>
                                        </ContentTemplate>
                                    </telerik:RadButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%; height: 36px"><%-- BugID:46054--%>
                            <asp:Label ID="lblFromDate" Font-Size="Small" runat="server" Text="Result Date From" CssClass="spanstyle"></asp:Label>
                            <telerik:RadDatePicker ID="frmDate" runat="server"></telerik:RadDatePicker>
                            <asp:Label ID="lblToDate" Font-Size="Small" runat="server" Text="To" CssClass="spanstyle"></asp:Label>
                            <telerik:RadDatePicker ID="toDate" runat="server"></telerik:RadDatePicker>
                            <asp:Label ID="lblLabName" Font-Size="Small" Text="Lab Name" runat="server" CssClass="spanstyle"></asp:Label>
                            <telerik:RadComboBox ID="cboLabName" Font-Size="Small" Font-Bold="false" runat="server"
                                Height="75px" Width="400px" CssClass="Editabletxtbox">
                            </telerik:RadComboBox>
                            <%--<asp:DropDownList ID="DropDownList1" runat="server" style="margin-left: 70px;padding: 4px 8px;font-size: 13px;width: 400px;line-height: 1.2;box-sizing: border-box;height: 25px;"></asp:DropDownList>--%>

                            <%-- <asp:RadioButton ID="rbtnAllResults" AutoPostBack="true" runat="server" Text="All Results"
                                Width="92px" Height="26px" GroupName="Results" Font-Size="Small" OnClick="checkRadioButton();" />
                            <asp:RadioButton ID="rbtnResultAttachedtoPatientChart" AutoPostBack="true" runat="server"
                                Text="Result Attached to Patient Chart" Width="205px" Height="26px" GroupName="Results"
                                Font-Size="Small" OnClick="checkRadioButton();" />
                            <asp:RadioButton ID="rbtnResultNottoPatientChart" AutoPostBack="true" runat="server"
                                Text="Result Not Attached to Patient Chart" Width="232px" Height="26px" GroupName="Results"
                                Font-Size="Small" OnClick="checkRadioButton();" />
                            <asp:TextBox ID="txtResultDt" runat="server" placeholder=" Search by Result Date" Font-Size="Small"></asp:TextBox>
                            <asp:TextBox ID="txtReasonNtMatching" runat="server"  placeholder=" Search by Reason for Not Matching" Font-Size="Small"></asp:TextBox>
                            <asp:TextBox ID="txtLabName" runat="server" placeholder=" Search by Lab Name" Font-Size="Small"></asp:TextBox>--%>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%; height: 36px">
                            <asp:Label ID="lblErrorReason" Font-Size="Small" Text="Error Reason" runat="server" CssClass="spanstyle"></asp:Label>
                            <telerik:RadComboBox ID="cboErrorReason" Font-Size="Small" Font-Bold="false" runat="server"
                                Height="75px" Width="386px"  CssClass="Editabletxtbox">
                            </telerik:RadComboBox>
                            <asp:Label ID="lblCategory" Font-Size="Small" Text="Matching Category" runat="server" CssClass="spanstyle"></asp:Label>
                            <telerik:RadComboBox ID="cboCategory" Font-Size="Small" Font-Bold="false" runat="server"
                                Height="75px" Width="400px"  CssClass="Editabletxtbox">
                                <Items>
                                    <telerik:RadComboBoxItem runat="server" Text="All Results" Font-Size="Small"  CssClass="Editabletxtbox" />
                                    <telerik:RadComboBoxItem runat="server" Text="Result Attached to Patient Chart" Font-Size="Small"  CssClass="Editabletxtbox"/>
                                    <telerik:RadComboBoxItem runat="server" Text="Result Not Attached to Patient Chart" Font-Size="Small"  CssClass="Editabletxtbox"/>
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%">
                            <asp:Label ID="lblSearch" Font-Size="Small" Font-Bold="true" runat="server" Text="" CssClass="spanstyle"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%; height: 280px" valign="top">
                            <asp:Panel ID="pnlGrid" runat="server" Font-Names="Times New Roman" Font-Size="Small"
                                GroupingText="Unassigned Result" Font-Bold="true" CssClass="LabelStyleBold">
                                <telerik:RadGrid ID="grdUnassignedResults" runat="server" Height="260px" AutoGenerateColumns="False"
                                    CellSpacing="0" Width="1180px" OnItemCommand="grdUnassignedResults_ItemCommand" OnItemDataBound="grdUnassignedResults_ItemDataBound" CssClass="Gridbodystyle">
                                    <HeaderStyle Font-Bold="true" />
                                    <ClientSettings Selecting-AllowRowSelect="true" Scrolling-UseStaticHeaders="true"
                                        EnablePostBackOnRowClick="true">
                                        <Selecting AllowRowSelect="True" />
                                        <ClientEvents OnRowClick="grdUnassignedResults_OnRowClick" />
                                        <Scrolling UseStaticHeaders="True" AllowScroll="true" />
                                    </ClientSettings>
                                    <%--<ClientSettings AllowKeyboardNavigation="true" EnablePostBackOnRowClick="false">
                                    <Selecting AllowRowSelect="true" />
                                    <ClientEvents OnRowClick="grdUnassignedResults_OnRowClick" />
                                    <Scrolling AllowScroll="True" ScrollHeight="270px" UseStaticHeaders="True" />
                                </ClientSettings>--%>
                                    <MasterTableView>
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="Patient Acc # in Result" FilterControlAltText="Filter UnassignedResults column"
                                                HeaderText="Patient Acc # in Result" UniqueName="PatientAcc">
                                                <HeaderStyle Width="55px"  CssClass="Gridheaderstyle" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Patient Name in Result" FilterControlAltText="Filter UnassignedResults column"
                                                HeaderText="Patient Name in Result" UniqueName="PatientNameinResult">
                                                <HeaderStyle  CssClass="Gridheaderstyle" />
                                                <ItemStyle CssClass="Editabletxtbox"/>
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Patient DOB In Result" FilterControlAltText="Filter UnassignedResults column"
                                                HeaderText="Patient DOB In Result" UniqueName="PatientDOBInResult">
                                                <HeaderStyle  CssClass="Gridheaderstyle" />
                                                <ItemStyle CssClass="Editabletxtbox"/>
                                                </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Patient Gender In Result" FilterControlAltText="Filter UnassignedResults column"
                                                HeaderText="Patient Gender In Result" UniqueName="PatientGenderInResult">
                                                 <HeaderStyle  CssClass="Gridheaderstyle" />
                                                <ItemStyle CssClass="Editabletxtbox"/>
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Order ID" FilterControlAltText="Filter UnassignedResults column"
                                                HeaderText="Order ID" UniqueName="OrderID">
                                                <HeaderStyle Width="50px" CssClass="Gridheaderstyle" />
                                                <ItemStyle CssClass="Editabletxtbox"/>
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Specimen" FilterControlAltText="Filter UnassignedResults column"
                                                HeaderText="Specimen" UniqueName="Specimen" >
                                                 <HeaderStyle  CssClass="Gridheaderstyle" />
                                                <ItemStyle CssClass="Editabletxtbox"/>
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Lab Procedure" FilterControlAltText="Filter UnassignedResults column"
                                                HeaderText="Lab Procedure" UniqueName="LabProcedure">
                                                <HeaderStyle Width="200px"  CssClass="Gridheaderstyle"/>
                                                <ItemStyle CssClass="Editabletxtbox"/>
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Reason for not matching" FilterControlAltText="Filter UnassignedResults column"
                                                HeaderText="Reason for not matching" UniqueName="Reason">
                                                 <HeaderStyle  CssClass="Gridheaderstyle" />
                                                <ItemStyle CssClass="Editabletxtbox"/>
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Result Message Date And Time" FilterControlAltText="Filter UnassignedResults column"
                                                HeaderText="Result Message Date And Time" UniqueName="ResultMessageDateAndTime">
                                                 <HeaderStyle  CssClass="Gridheaderstyle" />
                                                <ItemStyle CssClass="Editabletxtbox"/>
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Provider Name" FilterControlAltText="Filter UnassignedResults column"
                                                HeaderText="Provider Name" UniqueName="ProviderName">
                                                <HeaderStyle Width="80px" CssClass="Gridheaderstyle" />
                                                <ItemStyle CssClass="Editabletxtbox"/>
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Lab Name" FilterControlAltText="Filter UnassignedResults column"
                                                HeaderText="Lab Name" UniqueName="LabName">
                                                <HeaderStyle Width="80px" CssClass="Gridheaderstyle"/>
                                                <ItemStyle CssClass="Editabletxtbox"/>
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Abnormal" FilterControlAltText="Filter UnassignedResults column"
                                                HeaderText="Abnormal" UniqueName="Abnormal">
                                                <HeaderStyle Width="73px" CssClass="Gridheaderstyle" />
                                                <ItemStyle CssClass="Editabletxtbox"/>
                                            </telerik:GridBoundColumn>
                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="View" ImageUrl="~/Resources/Down.bmp"
                                                FilterControlAltText="Filter View column" HeaderText="View" UniqueName="View"
                                                DataTextField="View" Resizable="False">
                                                <HeaderStyle Width="40px" CssClass="Gridheaderstyle"/>
                                                <ItemStyle BorderColor="#CCFFFF" BorderStyle="Dotted" CssClass="Editabletxtbox"/>
                                            </telerik:GridButtonColumn>
                                            <telerik:GridBoundColumn DataField="Order Type" FilterControlAltText="Filter UnassignedResults column"
                                                HeaderText="Order Type" UniqueName="OrderType" Display="false">
                                                 <HeaderStyle  CssClass="Gridheaderstyle" />
                                                <ItemStyle CssClass="Editabletxtbox"/>
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="OrderID" FilterControlAltText="Filter UnassignedResults column"
                                                HeaderText="OrderID" UniqueName="OrderID" Display="false">
                                                 <HeaderStyle  CssClass="Gridheaderstyle" />
                                                <ItemStyle CssClass="Editabletxtbox"/>
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Lab ID" FilterControlAltText="Filter UnassignedResults column"
                                                HeaderText="Lab ID" UniqueName="LabID" Display="false">
                                                 <HeaderStyle  CssClass="Gridheaderstyle" />
                                                <ItemStyle CssClass="Editabletxtbox"/>
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Matching Patient ID" FilterControlAltText="Filter UnassignedResults column"
                                                HeaderText="Matching Patient ID" UniqueName="MatchingPatientID" Display="false">
                                                 <HeaderStyle  CssClass="Gridheaderstyle" />
                                                <ItemStyle CssClass="Editabletxtbox"/>
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Reason Code" FilterControlAltText="Filter UnassignedResults column"
                                                HeaderText="Reason Code" UniqueName="ReasonCode" Display="false">
                                                 <HeaderStyle  CssClass="Gridheaderstyle" />
                                                <ItemStyle CssClass="Editabletxtbox"/>
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="OrderingNPI" FilterControlAltText="Filter UnassignedResults column"
                                                HeaderText="OrderingNPI" UniqueName="OrderingNPI" Display="false">
                                                 <HeaderStyle  CssClass="Gridheaderstyle" />
                                                <ItemStyle CssClass="Editabletxtbox"/>
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ResultMasterID" FilterControlAltText="Filter UnassignedResults column"
                                                HeaderText="ResultMasterID" UniqueName="ResultMasterID" Display="false">
                                                 <HeaderStyle  CssClass="Gridheaderstyle" />
                                                <ItemStyle CssClass="Editabletxtbox"/>
                                            </telerik:GridBoundColumn>
                                            
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                                <%--      <Grid>--%>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%; font-size: small;">
                            <%--//Navigater--%>
                            <PageNavigator:PageNavigator ID="pageNavigator1" OnFirst="FirstPageNavigator" Visible="true"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%;" valign="top">
                            <asp:Panel ID="Panel1" runat="server" Font-Names="Times New Roman" Font-Size="Small"
                                Font-Bold="true" GroupingText="Match Information" CssClass="LabelStyleBold">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 100%; height: 30px" valign="top">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label1" Font-Bold="false" Width="150px" runat="server" Text="Map to Physician" CssClass="spanstyle"></asp:Label>
                                                        <telerik:RadComboBox ID="cboUnmatchProvider" Font-Bold="false" runat="server" Height="75px"
                                                            Width="190px" CssClass="Editabletxtbox"  />
                                                        <asp:CheckBox ID="chkUnmatchedProvider" runat="server" Font-Bold="false" Text="Show All Physician"
                                                            AutoPostBack="true" Checked="True" OnCheckedChanged="chkUnmatchedProvider_CheckedChanged" CssClass="Editabletxtbox" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%;">
                                            <asp:Panel ID="Panel2" runat="server" Font-Names="Times New Roman" Font-Size="Small"
                                                Font-Bold="true" GroupingText="Patient Information" CssClass="LabelStyleBold">
                                                <table style="width: 100%; height: 30px">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblPatientname" runat="server" Font-Bold="false" Text="Patient Name" CssClass="spanstyle"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <telerik:RadTextBox ID="txtPatientName" Font-Bold="false" runat="server" Width="190px"
                                                                Height="23px" ReadOnly="True" CssClass="nonEditabletxtbox">
                                                                <ReadOnlyStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
                                                                    ForeColor="Black" />
                                                                <HoveredStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
                                                                    ForeColor="Black" />
                                                            </telerik:RadTextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblAccountNumber" runat="server" Font-Bold="false" Text="Account Number"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <telerik:RadTextBox ID="txtAccountNumber" Font-Bold="false" runat="server" Width="190px"
                                                                Height="23px" ReadOnly="True"  CssClass="nonEditabletxtbox">
                                                                <ReadOnlyStyle  BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
                                                                    ForeColor="Black" />
                                                                <HoveredStyle  BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
                                                                    ForeColor="Black" />
                                                            </telerik:RadTextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label3" runat="server" Font-Bold="false" Text="DOB"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <telerik:RadTextBox ID="txtDOB" Font-Bold="false" runat="server" Width="190px" Height="23px"
                                                                ReadOnly="True"  CssClass="nonEditabletxtbox">
                                                                <ReadOnlyStyle  BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
                                                                    ForeColor="Black" />
                                                                <HoveredStyle  BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
                                                                    ForeColor="Black" />
                                                            </telerik:RadTextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label4" runat="server" Font-Bold="false" Text="Gender"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <telerik:RadTextBox ID="txtGender" Font-Bold="false" runat="server" Width="190px"
                                                                Height="23px" ReadOnly="True"  CssClass="nonEditabletxtbox">
                                                                <ReadOnlyStyle  BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
                                                                    ForeColor="Black" />
                                                                <HoveredStyle  BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
                                                                    ForeColor="Black" />
                                                            </telerik:RadTextBox>
                                                            <telerik:RadButton ID="btnFindPatient" runat="server" AutoPostBack="false" EnableViewState="false"
                                                                Text="Find Patient" Width="100px" AccessKey="l" Style="position: static; height: 28px !important; text-align: center; -moz-border-radius: 3px; -webkit-border-radius: 3px; position: relative; font-size: 13px !important;"
                                                                OnClientClicked="btnFindPatient_Clicked" ButtonType="LinkButton" CssClass="bluebutton teleriknormalbuttonstyle">
                                                            </telerik:RadButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%;" valign="top">
                                            <asp:Panel ID="Panel3" runat="server" Font-Names="Times New Roman" Font-Size="Small"
                                                GroupingText="Order Details" CssClass="LabelStyleBold">
                                                <table style="width: 100%; height: 230px">
                                                    <tr>
                                                        <td valign="top" style="width: 100%; height: 30px">
                                                            <asp:Label ID="Label5" runat="server" Text="Outstanding Order(s)" CssClass="Editabletxtbox"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" style="width: 100%; height: 200px">
                                                            <telerik:RadGrid ID="grdOutstandingOrders" runat="server" Height="200px" AutoGenerateColumns="False"
                                                                CellSpacing="0" Width="1150px" CssClass="Gridbodystyle">
                                                                <FilterMenu EnableImageSprites="False">
                                                                </FilterMenu>
                                                                <ClientSettings AllowKeyboardNavigation="true" EnablePostBackOnRowClick="false">
                                                                    <Scrolling AllowScroll="True" ScrollHeight="270px" UseStaticHeaders="True" />
                                                                    <Selecting AllowRowSelect="true" />
                                                                </ClientSettings>
                                                                <MasterTableView>
                                                                    <Columns>
                                                                        <telerik:GridBoundColumn DataField="Order#" FilterControlAltText="Filter OutstandingOrders column"
                                                                            HeaderText="Order#" UniqueName="Order">
                                                                            <HeaderStyle Width="130px" CssClass="Gridheaderstyle" />
                                                                            <ItemStyle CssClass="Editabletxtbox" />
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn DataField="Order Description(Procedures)" FilterControlAltText="Filter OutstandingOrders column"
                                                                            HeaderText="Order Description(Procedures)" UniqueName="OrderDescription">
                                                                            <HeaderStyle Width="749px" CssClass="Gridheaderstyle"/>
                                                                            <ItemStyle CssClass="Editabletxtbox" />
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn DataField="Order Date" FilterControlAltText="Filter OutstandingOrders column"
                                                                            HeaderText="Order Date" UniqueName="OrderDate">
                                                                            <HeaderStyle Width="140px" CssClass="Gridheaderstyle"/>
                                                                            <ItemStyle CssClass="Editabletxtbox" />
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn DataField="Lab Name" FilterControlAltText="Filter OutstandingOrders column"
                                                                            HeaderText="Lab Name" UniqueName="LabName">
                                                                            <HeaderStyle Width="130px" CssClass="Gridheaderstyle"/>
                                                                            <ItemStyle CssClass="Editabletxtbox" />
                                                                        </telerik:GridBoundColumn>
                                                                        <telerik:GridBoundColumn DataField="Order_Submit_ID" FilterControlAltText="Filter OutstandingOrders column"
                                                                            HeaderText="Order_Submit_ID" UniqueName="Order_Submit_ID" Display="false">
                                                                            <HeaderStyle CssClass="Gridheaderstyle" />
                                                                            <ItemStyle CssClass="Editabletxtbox" />
                                                                        </telerik:GridBoundColumn>
                                                                    </Columns>
                                                                </MasterTableView>
                                                            </telerik:RadGrid>
                                                            <%-- <grid></grid>--%>
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
                        <td style="width: 100%">
                            <table style="width: 100%; height: 30px">
                                <tr>
                                    <td style="width: 50%">
                                        <asp:CheckBox ID="chkNoOrders" runat="server" Font-Bold="false" Font-Size="Small"
                                            OnClick="checkOnclcik();" AutoPostBack="false" Text="No Order In Capella" CssClass="Editabletxtbox" />
                                    </td>
                                    <td style="width: 50%" align="right">
                                        <telerik:RadButton ID="btnMatchOrders" runat="server" AutoPostBack="true" EnableViewState="false"
                                            Text="Match" Width="77px" AccessKey="l" Style="position: static; height: 35px !important; text-align: center; -moz-border-radius: 3px; -webkit-border-radius: 3px; position: relative;"
                                            OnClick="btnMatchOrders_Click"
                                            OnClientClicked="btnMatchOrders_Clicked"  ButtonType="LinkButton" CssClass="bluebutton teleriknormalbuttonstyle">
                                        </telerik:RadButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <telerik:RadScriptManager EnableViewState="false" ID="RadScriptManager1" runat="server">
                    <Scripts>
                        <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                        <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                        <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
                    </Scripts>
                </telerik:RadScriptManager>
            </div>
            <div id="divLoading" class="modal" runat="server" style="text-align: center; display: none">
                <asp:Panel ID="Panel4" runat="server">
                    <br />
                    <br />
                    <br />
                    <br />
                    <center>
                        <asp:Label ID="LodingLabel" Text="" EnableViewState="false" runat="server"></asp:Label></center>
                    <br />
                    <img src="Resources/wait.ico" title="" enableviewstate="false" alt="Loading..." />
                    <br />
                </asp:Panel>
            </div>
        </telerik:RadAjaxPanel>
        <asp:HiddenField ID="hdnHumanID" runat="server" />
        <asp:Button ID="InvisibleButton" runat="server" CssClass="displayNone" OnClick="InvisibleButton_Click" />
        <asp:Button ID="SearchClick" runat="server" CssClass="displayNone" OnClick="SearchClick_Click" />
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">

            <script src="JScripts/JSOrderException.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

        </asp:PlaceHolder>

    </form>
</body>
</html>
