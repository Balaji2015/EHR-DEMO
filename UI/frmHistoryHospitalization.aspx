<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmHistoryHospitalization.aspx.cs" EnableEventValidation="false"
    Inherits="Acurus.Capella.UI.frmHistoryHospitalization"  ValidateRequest="false" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/UserControls/PageNavigator.ascx" TagName="PageNavigator" TagPrefix="PageNavigator" %>
<%@ Register Src="~/UserControls/CustomDLCNew.ascx" TagName="DLC" TagPrefix="DLC" %>
<%@ Register Src="~/UserControls/CustomDateTimePicker.ascx" TagName="CustomDatePicker"
    TagPrefix="UC" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>History Hospitalization</title>
  
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>

    <link href="~/CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .displayNone {
            display: none;
        }

        .style1 {
            width: 28%;
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

        .style5 {
            width: 36%;
        }

        .style6 {
            width: 100%;
            height: 200px;
        }

        .underline {
            text-decoration: underline;
        }

        body {
            zoom: 1.0 !important;
            -moz-transform: scale(1) !important;
            -moz-transform-origin: 0 0 !important;
        }
    </style>
    
    
  <%--  <link href="/CSS/style.css" rel="stylesheet" type="text/css" />--%>
    <link href="~/CSS/font-awesome.css" rel="stylesheet" type="text/css" />
</head>
<body onload="HistoryHosp_Load()">
    <form id="frmHospitalization" runat="server" style="background-color: White; width: 100%; height: 100%; margin-bottom: 0px; font-family: Microsoft Sans Serif; font-size: 8.5pt;">
        <telerik:RadWindowManager ID="WindowMngr" runat="server" IconUrl="Resources/16_16.ico"
            EnableViewState="False">
            <Windows>
                <telerik:RadWindow ID="MessageWindow" runat="server" Behaviors="Close" Title="Hospitalization History"
                    EnableViewState="False" IconUrl="Resources/16_16.ico">
                </telerik:RadWindow>
                <telerik:RadWindow ID="MessageWindowLibrary" runat="server" Behaviors="Close" OnClientClose="ReloadOnClientClose"
                    IconUrl="Resources/16_16.ico">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%" Width="100%">
            <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" EnableViewState="False">
            </telerik:RadAjaxManager>
            <div id="divHospitalizationHistory" style="width: 100%; height: 100%; background-color: White;"
                runat="server">
                <%--<table style="font-family: Microsoft Sans Serif; font-size: small; height: 100%; width: 100%;">--%>
                <table class ="Editabletxtbox; font-size: small; height: 100%; width: 100%;" >
                    <tr valign="top" style="height: 2%; width: 100%;">
                        <td align="right" valign="top" style="width: 100%;">
                            <asp:CheckBox ID="chkPatientDeniesHospitalization" runat="server" Text="Patient Denies Hospitalization"
                                OnCheckedChanged="chkPatientDeniesHospitalization_CheckedChanged" onchange="LoadingSymbol();"
                                AutoPostBack="True"  CssClass="Editabletxtbox"/>
                        </td>
                    </tr>
                    <tr valign="top" style="height: 30%; width: 100%;">
                        <td valign="top" style="width: 100%;">
                            <asp:Panel ID="pnlHospitalization" runat="server" GroupingText="Hospitalization History "
                                BackColor="White" Width="100%" Height="100%" Font-Size="Small" Font-Bold="true" CssClass="Editabletxtbox">
                                <table style="width: 100%; height: 100%;">
                                    <tr style="height: 100%;" valign="top">
                                        <td style="width: 27%" valign="top">
                                            <asp:Panel ID="pnlListHospitalization" runat="server" BackColor="White" CssClass="Editabletxtbox">
                                                <table style="width: 100%; height: 249px;">
                                                    <tr valign="top">
                                                        <td style="height: 26px; width: 250px;" valign="top">

                                                           
                                                            <asp:Label ID="lblReasonForHospitalizationPhrases" runat="server" Text="Reason For Hospitalization Phrases"
                                                                Font-Bold="False" EnableViewState="False" CssClass="Editabletxtbox"></asp:Label>
                                                        </td>
                                                        <td style="height: 26px;" valign="top">
                                                            <asp:ImageButton ID="pbDatabase" runat="server" ImageUrl="~/Resources/Database%20Inactive.jpg"
                                                                Height="16px" OnClientClick="openAddorUpdate();" EnableViewState="False" />
                                                        </td>
                                                    </tr>
                                                    <tr valign="top">
                                                        <td colspan="2" valign="top">
                                                            <asp:Panel ID="pnlLstReason" runat="server" Height="247px" Width="100%" BackColor="White">
                                                                <telerik:RadListBox ID="lstReasonForHospitalization" runat="server" Height="98%"
                                                                    Width="100%" AutoPostBack="false" OnClientSelectedIndexChanged="lstReasonForHospitalization_SelectedIndexChanged"
                                                                     CssClass="Editabletxtbox"
                                                                     Font-Bold="False">
                                                                    <ButtonSettings TransferButtons="All" />
                                                                </telerik:RadListBox>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                        <td style="width: 73%;" valign="top">
                                            <asp:Panel ID="pnlHistoryControls" runat="server" BackColor="White" Width="100%"
                                                Height="100%" Font-Size="Small" CssClass="Editabletxtbox">
                                                <table style="width: 100%; height: 100%">
                                                    <tr style="width: 100%;" valign="top">
                                                        <td>
                                                            <asp:Panel ID="pnlReasonForHopitalization" runat="server" BackColor="White" Width="100%"
                                                                Height="100%" Font-Size="Small" CssClass="Editabletxtbox">
                                                                <table style="width: 100%; height: 100%">
                                                                    <tr style="height: 100%">
                                                                        <td valign="middle" style="width: 18%">
                                                                            <%--<asp:Label ID="lblReasonForHospitalization" runat="server" Text="Reason For Hospitalization*"  Mand="Yes"
                                                                                Font-Bold="False" CssClass="Editabletxtbox" EnableViewState="False"></asp:Label>--%>

                                                                            <span class="MandLabelstyle">Reason For Hospitalization<span class="manredforstar">*</span></span>
                                                                        </td>
                                                                        <td style="width: 77%;" align="left">
                                                                            <telerik:RadTextBox ID="txtReasonForHospitalization" runat="server" MaxLength="255"
                                                                                Height="50px" Style="margin-right: 0px" TextMode="MultiLine" Width="100%" Font-Bold="False" CssClass="Editabletxtbox"
                                                                                EnableViewState="False">
                                                                                <DisabledStyle Resize="None" />
                                                                                <InvalidStyle Resize="None" />
                                                                                <HoveredStyle Resize="None" />
                                                                                <ReadOnlyStyle Resize="None" />
                                                                                <EmptyMessageStyle Resize="None" />
                                                                                <ClientEvents OnKeyPress="txtReasonForHospitalization_OnKeyPress" />
                                                                                <FocusedStyle Resize="None" />
                                                                                <EnabledStyle Resize="None" />
                                                                            </telerik:RadTextBox>
                                                                        </td>
                                                                        <td style="width: 5%;" align="left">
                                                                            <asp:ImageButton ID="pbClear" runat="server" ImageUrl="~/Resources/close_small_pressed.png"
                                                                                OnClientClick="return TextBoxClear();" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                    <tr style="width: 100%;" valign="top">
                                                        <td valign="top">
                                                            <asp:Panel ID="pnlFromDate" runat="server" BackColor="White" Width="100%" Height="100%"
                                                                Font-Size="Small" CssClass="Editabletxtbox">
                                                                <table style="width: 100%; height: 100%">
                                                                    <tr style="height: 100%">
                                                                        <td style="width: 17.8%;">
                                                                            <asp:Label ID="lblFromDate" runat="server" Text="From Date" valign="top" Font-Bold="False"
                                                                                CssClass="Editabletxtbox" EnableViewState="False"></asp:Label>
                                                                        </td>
                                                                        <td style="width:26%;" align="left" class="Editabletxtbox">
                                                                            <UC:CustomDatePicker ID="dtpFromDate" runat="server"  />
                                                                        </td>
                                                                        <td style="width:13%;" class="Editabletxtbox">
                                                                            <asp:CheckBox ID="chkCurrentDate" runat="server" Text="Current/To Date" onclick="CurrentCheckBox();"
                                                                                AutoPostBack="false" Font-Bold="False" CssClass="Editabletxtbox"
                                                                                EnableViewState="False"/>
                                                                        </td>
                                                                        <td style="position: static;" align="left" class="Editabletxtbox">
                                                                            <UC:CustomDatePicker ID="dtpToDate" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                    <%-- <tr style="width: 100%;" valign="top">
                                                        <td valign="top">
                                                                <table style="width: 100%; height: 100%">
                                                                    <tr style="height: 100%">
                                                                        <td style="width: 26.5%;">
                                                                            <asp:CheckBox ID="chkCurrentDate" runat="server" Text="Current Date/To Date" onclick="CurrentCheckBox();"
                                                                                AutoPostBack="false" Font-Bold="False" Font-Names="Microsoft Sans Serif" Font-Size="8.5pt"
                                                                                EnableViewState="False" />
                                                                        </td>
                                                                        <td style="position: static;" align="left">
                                                                            <UC:CustomDatePicker ID="dtpToDate" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                        </td>
                                                    </tr>--%>
                                                    <tr style="width: 100%;" valign="top">
                                                        <td valign="top">
                                                            <table style="width: 100%; height: 100%">
                                                                <tr style="height: 100%">
                                                                    <td style="width: 18%">
                                                                        <asp:Label ID="lblDischargePhysician" runat="server" Text="Discharge Physician"
                                                                            Font-Bold="False" CssClass="Editabletxtbox" EnableViewState="False"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtDischargePhysician" runat="server" MaxLength="100" Width="601px" onkeypress="return EnableSave();" CssClass="Editabletxtbox"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr style="width: 100%;" valign="top">
                                                        <td valign="top">
                                                            <table style="width: 100%; height: 100%">
                                                                <tr style="height: 100%">
                                                                    <td style="width: 18%;">
                                                                        <asp:Label ID="lblReadmitted" runat="server" Text="Readmitted" Font-Bold="False" CssClass="Editabletxtbox"></asp:Label>
                                                                    </td>
                                                                    <td style="width: 26%;" class="Editabletxtbox">
                                                                        <select id="ddlReadmitted" runat="server" style="width: 80%" onchange="return ChangeReadmitted();">
                                                                            <option value=""></option>
                                                                            <option value="Yes"></option>
                                                                            <option value="No"></option>
                                                                        </select>
                                                                    </td>
                                                                    <td style="width: 12.5%;">
                                                                        <asp:Label ID="lblReadmissionDate" runat="server" Text="Date of Readmission" Font-Bold="False" CssClass="Editabletxtbox"></asp:Label>
                                                                    </td>
                                                                    <td style="position: static;" align="left" class="Editabletxtbox">
                                                                        <UC:CustomDatePicker ID="dtpReadmissionDate" runat="server" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr style="width: 100%;" valign="top">
                                                        <td valign="top">
                                                            <asp:Panel ID="pnlHospitalizationHostoryNotes" runat="server" BackColor="White" Width="100%"
                                                                Height="100%" Font-Size="Small" Font-Bold="false">
                                                                <table style="width: 100%; height: 100%">
                                                                    <tr style="height: 100%">
                                                                        <td style="width: 17.5%;" valign="top">
                                                                            <asp:Label ID="lblHospitalizationNotes" runat="server" Text="Hospitalization Notes"
                                                                                Font-Bold="False" CssClass="Editabletxtbox" EnableViewState="False"></asp:Label>
                                                                        </td>
                                                                        <td style="width: 82.5%;" align="left">
                                                                            <DLC:DLC ID="DLC" runat="server" Enable="True" TextboxHeight="50px" TextboxWidth="590px" TextboxMaxLength="1000"
                                                                                ListboxPosition="4px" Value="Hospitalization Notes"  />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                    <tr style="width: 100%;" valign="top">
                                                        <td valign="top">
                                                            <%--<asp:Panel ID="pnlSaveClearAll" runat="server" BackColor="White" Width="100%" Height="100%"
                                                                Font-Size="Small">
                                                                <table style="width: 100%; height: 100%">
                                                                    <tr style="height: 100%">
                                                                        <td style="width: 90%;"></td>
                                                                        <td style="width: 10%;">--%>
                                                            <asp:Panel ID="Panel1" runat="server" BackColor="White" Width="100%" Height="100%"
                                                                Font-Size="Small">
                                                                <table style="width: 100%; height: 100%">
                                                                    <tr style="height: 100%; width: 100%;">
                                                                        <td align="right" style="width: 75%">
                                                                            <telerik:RadButton ID="btnAdd" runat="server" Text="Add"
                                                                                OnClick="btnAdd_Click" OnClientClicked="btnAdd_Clicked" AccessKey="A"
                                                                                Style=" text-align: center; position: static; -moz-border-radius: 3px; -webkit-border-radius: 3px; position: relative; Width: auto !important; height:26px !important; padding: 4px 12px !important; font-size: 13px !important;"  ButtonType="LinkButton" CssClass="greenbutton teleriknormalbuttonstyle" >
                                                                               
                                                                                <ContentTemplate>
                                                                                    <span id="SpanAdd" runat="server" >A</span><span id="SpanAdditionalword" runat="server">dd</span>
                                                                                </ContentTemplate>
                                                                            </telerik:RadButton>
                                                                        </td>
                                                                        <td align="right" style="width: 5%">
                                                                            <telerik:RadButton ID="btnClearAll" runat="server"
                                                                                AutoPostBack="false" Text="Clear All" OnClientClicked="btnClearAll_Clicked" AccessKey="l"
                                                                                Style=" text-align:center;  top: 0px; left: 0px; position: static; -moz-border-radius: 3px; -webkit-border-radius: 3px; position: relative; Width:76px; height:26px !important; padding: 4px 12px !important; font-size: 13px !important;"  ButtonType="LinkButton" CssClass="redbutton teleriknormalbuttonstyle"     >
                                                                           
                                                                                <ContentTemplate>
                                                                                    <span id="SpanClear" runat="server" >C</span><span id="SpanClearAdditional" runat="server">lear All</span>
                                                                                </ContentTemplate>
                                                                            </telerik:RadButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                            <%--  </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>--%>
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
                    <tr valign="top" style="height: 30%; width: 100%;">
                        <td style="background-color: White;" valign="top">
                            <asp:Panel ID="pnlGrid" runat="server" GroupingText="Hospitalization History Details"
                                BackColor="White" Width="100%" Height="225px" Font-Size="Small" Font-Bold="true" CssClass="Editabletxtbox"
                                BorderWidth="1px" Style="margin-left: 1px; margin-right: 1px; margin-top: 1px; margin-bottom: 1px;">
                                <telerik:RadGrid ID="grdHospitalizationHistory" runat="server" AutoGenerateColumns="False"
                                    CellSpacing="0" GridLines="Horizontal" OnItemCommand="grdHospitalizationHistory_ItemCommand"
                                    Font-Bold="False" Height="180px" Width="100%" CssClass="Gridbodystyle">
                                    <HeaderStyle Font-Bold="true"   CssClass="Gridheaderstyle"/>
                                   <ItemStyle CssClass="Gridbodystyle" />
                                    <%--BackColor="#a4d9ff"--%>
                                    <ClientSettings>
                                        <ClientEvents OnCommand="grdHospitalizationHistory_OnCommand" />
                                        <Scrolling AllowScroll="True" ScrollHeight="270px" UseStaticHeaders="True" />
                                        <Selecting AllowRowSelect="true" />
                                    </ClientSettings>
                                    <MasterTableView BorderWidth="1">
                                        <Columns>
                                            <telerik:GridButtonColumn HeaderText="Edit" HeaderStyle-Width="50px" ButtonType="ImageButton"
                                                CommandName="Edt" Text="Edit" UniqueName="EditRows" ImageUrl="~/Resources/edit.gif">
                                                <HeaderStyle Width="50px" CssClass="Editabletxtbox"/>
                                                <ItemStyle  BorderStyle="Dotted" />
                                            </telerik:GridButtonColumn>

                                            <telerik:GridButtonColumn ConfirmText="Are you sure you want to delete this Hospitalization History?"
                                                ConfirmDialogType="RadWindow" ConfirmTitle="Hospitalization History" HeaderText="Del" HeaderStyle-Width="50px"
                                                ButtonType="ImageButton" CommandName="DeleteRows" Text="Delete" UniqueName="DeleteRows"  ConfirmDialogHeight="155px"
                                                ImageUrl="~/Resources/close_small_pressed.png">
                                               <HeaderStyle Width="50px" CssClass="Editabletxtbox"/>
                                            </telerik:GridButtonColumn>
                                            <telerik:GridBoundColumn DataField="ReasonForHospitalization" FilterControlAltText="Filter ReasonForHospitalization column"
                                                HeaderText="Reason For Hospitalization" UniqueName="ReasonForHospitalization">
                                                <HeaderStyle Width="30%" CssClass="Editabletxtbox"/>
                                                <ItemStyle  Font-Bold="false"  BorderStyle="Dotted" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="FromDate" FilterControlAltText="Filter FromDate column"
                                                HeaderText="From Date" UniqueName="FromDate">
                                                <HeaderStyle Width="20%" CssClass="Editabletxtbox"/>
                                                <ItemStyle  Font-Bold="false"  BorderStyle="Dotted" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ToDate" FilterControlAltText="Filter ToDate column"
                                                HeaderText="To Date" UniqueName="ToDate">
                                                <HeaderStyle Width="20%" CssClass="Editabletxtbox"/>
                                                <ItemStyle Font-Bold="false"  BorderStyle="Dotted" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="HospitalizationNotes" FilterControlAltText="Filter HospitalizationNotes column"
                                                HeaderText="Hospitalization Notes" UniqueName="HospitalizationNotes">
                                                <HeaderStyle Width="30%" CssClass="Editabletxtbox"/>
                                                <ItemStyle  Font-Bold="false"  BorderStyle="Dotted" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Id" FilterControlAltText="Filter Id column" HeaderText="ID"
                                                UniqueName="Id" Display="false">
                                                <HeaderStyle Width="5%" CssClass="Editabletxtbox"/>
                                                <ItemStyle   BorderStyle="Dotted" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="DischargePhysician" FilterControlAltText="Filter DischargePhysician column"
                                                HeaderText="Discharge Physician" UniqueName="DischargePhysician" Display="false">
                                                <HeaderStyle Width="30%" CssClass="Editabletxtbox"/>
                                                <ItemStyle  Font-Bold="false"  BorderStyle="Dotted" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Readmitted" FilterControlAltText="Filter Readmitted column"
                                                HeaderText="Readmitted" UniqueName="Readmitted" Display="false">
                                                <HeaderStyle Width="30%" CssClass="Editabletxtbox"/>
                                                <ItemStyle  Font-Bold="false"  BorderStyle="Dotted" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Readmitted_Date" FilterControlAltText="Filter Readmitted_Date column"
                                                HeaderText="Readmitted_Date" UniqueName="Readmitted_Date" Display="false">
                                                <HeaderStyle Width="30%" CssClass="Editabletxtbox"/>
                                                <ItemStyle  Font-Bold="false"  BorderStyle="Dotted" />
                                            </telerik:GridBoundColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </asp:Panel>
                        </td>
                    </tr>
                    <%-- <tr valign="top">
                        <td style="width: 100%; background-color: White; font-size: small; font-family: Microsoft Sans Serif; font-size: 8.5pt"
                            valign="top">--%>
                    <%--  <PageNavigator:PageNavigator ID="mpnHospitalizationHistory" runat="server" OnFirst="FirstPageNavigator"
                            Visible="true" />--%>
                    <%-- </td>
                    </tr>--%>
                </table>
                <telerik:RadScriptManager ID="RadScriptManager2" runat="server" EnableViewState="False">
                    <Scripts>
                        <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                        <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                        <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
                    </Scripts>
                </telerik:RadScriptManager>
            </div>
            <div id="divLoading" class="modal" runat="server" style="text-align: center; display: none">
                <asp:Panel ID="Panel3" runat="server">
                    <br />
                    <br />
                    <br />
                    <br />
                    <center>
                        <asp:Label ID="Label1" Text=" " runat="server"></asp:Label></center>
                    <br />
                    <img src="Resources/wait.ico" title="[Please wait while the page is loading...]"
                        alt="Loading..." />
                    <br />
                </asp:Panel>
            </div>
            <asp:Button ID="InvisibleClearAllButton" runat="server" CssClass="displayNone" OnClick="InvisibleClearAllButton_Click" />
        </telerik:RadAjaxPanel>
        <asp:HiddenField ID="Hiddenupdate" runat="server" />
        <asp:HiddenField ID="hdnDelHospitalizationId" runat="server" EnableViewState="False" />
        <asp:HiddenField ID="Hidden1" runat="server" EnableViewState="False" />
        <asp:HiddenField ID="hdnLocalTime" runat="server" EnableViewState="False" />
        <asp:Button ID="InvisibleButton" runat="server" OnClick="InvisibleButton_Click" CssClass="displayNone" />
        <asp:Button ID="LibraryIconButton" runat="server" OnClick="LibraryIconButton_Click"
            CssClass="displayNone" />
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript" enableviewstate="False"></script>

            <script src="JScripts/JSHistoryHospitalization.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"
                enableviewstate="False"></script>

            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript" enableviewstate="False"></script>

            <script src="JScripts/JSCustomDLC.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript" enableviewstate="False"></script>

            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript" enableviewstate="False"></script>

            <script src="JScripts/JSCustomDateTimePicker.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

        </asp:PlaceHolder>
    </form>
    <script type="text/javascript">
        //    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        //function EndRequestHandler(sender, args) {
        //    if (args.get_error() != undefined) {
        //        args.set_errorHandled(true);
        //    }
        //}
        Telerik.Web.UI.RadListBox.prototype.saveClientState = function () {
            return "{" +
                        "\"isEnabled\":" + this._enabled +
                        ",\"logEntries\":" + this._logEntriesJson +
                       ",\"selectedIndices\":" + this._selectedIndicesJson +
                       ",\"checkedIndices\":" + this._checkedIndicesJson +
                       ",\"scrollPosition\":" + Math.round(this._scrollPosition) +
                   "}";
        }
    </script>
</body>
</html>
