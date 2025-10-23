<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmManualResultEntry.aspx.cs"
    Inherits="Acurus.Capella.UI.frmManualResultEntry" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server" >
    <title>Manual Result Entry</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
        .BackGround
        {
            background-color: White;
        }
        .modal
        {
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
    </style>
    <link href="CSS/ElementStyles.css" rel="stylesheet" type="text/css" />
    <link href="CSS/font-awesome.css" rel="stylesheet" type="text/css" />
    <link href="~/CSS/CommonStyle.css" rel="Stylesheet" type="text/css" />
    <base target="_self" />
</head>
<body bgcolor="white" onkeydown="CallBack(event)" oncontextmenu="return false" onload=" loadmanualentry(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}">
    <form id="form1" runat="server" class="BackGround" >
    <telerik:RadWindowManager ID="WindowMngr" runat="server" ShowContentDuringLoad="true">
        <windows>
            <telerik:RadWindow ID="MessageWindow" runat="server" Modal="true" Behaviors="Close"
                Title="Manual Result Entry" IconUrl="Resources/16_16.ico" Height="100%" Width="100%">
            </telerik:RadWindow>
            <telerik:RadWindow ID="MREWindow" runat="server" Modal="true" Behaviors="Close"
                Title="Manual Result Entry" IconUrl="Resources/16_16.ico" Height="100%" Width="100%">
            </telerik:RadWindow>
            <telerik:RadWindow ID="SearchWindow" runat="server" Modal="true" Behaviors="Close" Title="Search Order" IconUrl="Resources/16_16.ico" Height="100%" Width="100%"></telerik:RadWindow>
            
        </windows> 
    </telerik:RadWindowManager>
    <div>
        <telerik:RadAjaxPanel ID="RefreshPanel" runat="server">
            <asp:Panel GroupingText="Enter Result" runat="server" ID="pnlEnterResult" Height="783px"
                Font-Size="Small" Width="100%" Font-Names="Microsoft San Serif" CssClass="Panel LabelStyleBold">
                <table width="100%">
                    <tr>
                        <td colspan="3">
                            <telerik:RadPanelBar ID="pnlBarGroupTabs" runat="server" Width="100%">
                                <items>
                                    <telerik:RadPanelItem runat="server" Text="Root RadPanelItem1" BackColor="White" style="background-image:none"   CssClass="pnlBarGroup">
                                    </telerik:RadPanelItem>
                                </items>
                            </telerik:RadPanelBar>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="pnlTestOrdered" runat="server" GroupingText="Test Ordered" CssClass="Panel LabelStyleBold" Style="position:static;"
                                Width="100%" Height="715px" >
                                <telerik:RadGrid ID="grdTestOrdered" runat="server" Width="100%" Height="681px" AutoGenerateColumns="False"
                                    CellSpacing="0" GridLines="None" OnItemCommand="grdTestOrdered_ItemCommand" 
                                    OnNeedDataSource="grdTestOrdered_NeedDataSource" CssClass="Gridbodystyle"  >
                                    <filtermenu enableimagesprites="False">
                                    </filtermenu>
                                    <clientsettings selecting-allowrowselect="true" scrolling-usestaticheaders="true"
                                        enablepostbackonrowclick="true">
                                        <Selecting AllowRowSelect="True" />
                                        <ClientEvents OnRowSelecting="RowClick" />
                                        <Scrolling UseStaticHeaders="True" AllowScroll="true" />
                                    </clientsettings>
                                    <mastertableview>
                                        <CommandItemSettings ExportToPdfText="Export to PDF" />
                                        <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                                        </RowIndicatorColumn>
                                        <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                                        </ExpandCollapseColumn>
                                        <Columns>
                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Image" DataTextField="Image"
                                                FilterControlAltText="Filter Image column" HeaderText="Image" ImageUrl="Resources/blue_open.png"
                                                UniqueName="Image" HeaderStyle-Width="44px" ItemStyle-Width="44px">
                                                <HeaderStyle CssClass="Gridheaderstyle" />
                                                <ItemStyle CssClass="Editabletxtbox" />
                                            </telerik:GridButtonColumn>
                                            <telerik:GridDateTimeColumn DataField="DateTime" HeaderText="DateTime" UniqueName="DateTime">
                                                 <HeaderStyle CssClass="Gridheaderstyle" />
                                                 <ItemStyle CssClass="Editabletxtbox" />
                                            </telerik:GridDateTimeColumn>
                                            <telerik:GridBoundColumn DataField="Lab" HeaderText="Lab" UniqueName="Lab">
                                                 <HeaderStyle CssClass="Gridheaderstyle" />
                                                 <ItemStyle CssClass="Editabletxtbox" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Tag" HeaderStyle-CssClass="displayNone" Display="false"
                                                UniqueName="Tag">
                                                <HeaderStyle CssClass="Gridheaderstyle" />
                                                <ItemStyle CssClass="Editabletxtbox" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="EnableImage" HeaderText="EnableImage" UniqueName="EnableImage"
                                                Display="false">
                                                <HeaderStyle CssClass="Gridheaderstyle" />
                                                <ItemStyle CssClass="Editabletxtbox" />
                                            </telerik:GridBoundColumn>
                                        </Columns>
                                        <EditFormSettings>
                                            <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                            </EditColumn>
                                        </EditFormSettings>
                                    </mastertableview>
                                </telerik:RadGrid>
                            </asp:Panel>
                        </td>
                        <td valign="top">
                            <asp:ImageButton ID="pbTestOrdered" runat="server" ImageUrl="~/Resources/Database Inactive.jpg" ToolTip="Orders"
                                OnClientClick="return OpenSearchOrders('MRE');" OnClick="pbTestOrdered_Click" />
                        </td>
                        <td valign="top" >
                            <asp:Panel ID="pnlEnterValues" runat="server" GroupingText="Enter Values" Height="720px"
                                Width="100%" CssClass="LabelStyleBold">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblLabName" runat="server" Text="Lab Name" CssClass="spanstyle" >
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtLabName" runat="server" BorderColor="Black"
                                                BorderWidth="1" OnTextChanged="txtLabName_TextChanged" ReadOnly="true" CssClass="nonEditabletxtbox">
                                            </telerik:RadTextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblPhyName" runat="server" Text="Physician Name" CssClass="spanstyle"></asp:Label>
                                        </td>
                                        <td colspan="4">
                                            <telerik:RadTextBox ID="txtPhysicianName" runat="server" Height="21px" 
                                                BorderColor="Black" BorderWidth="1" Width="100%" ReadOnly="true" CssClass="nonEditabletxtbox">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>                                 
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblDateCollected" runat="server" Text="Date and time Collected *"
                                                 CssClass="spanstyle" mand="Yes">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <telerik:RadDateTimePicker ID="dtpDateCollected" Width="180px"  runat="server" DateInput-DateFormat="dd-MMM-yyyy hh:mmtt"
                                                Calendar-ClientEvents-OnDateSelected="DateSelected" DateInput-ClientEvents-OnKeyPress="DateSelected" CssClass="Editabletxtbox">
                                            </telerik:RadDateTimePicker>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblDateReported" runat="server" Text="Date and time Reported *" CssClass="spanstyle"  mand="Yes"></asp:Label>
                                        </td>
                                        <td>
                                            <telerik:RadDateTimePicker ID="dtpDateReported" runat="server" DateInput-DateFormat="dd-MMM-yyyy hh:mmtt"
                                                Calendar-ClientEvents-OnDateSelected="DateSelected" Width="180px" DateInput-ClientEvents-OnKeyPress="DateSelected" CssClass="Editabletxtbox">
                                            </telerik:RadDateTimePicker>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblReportStatus" runat="server" Text="Report Status *" CssClass="spanstyle"  mand="Yes"></asp:Label>
                                        </td>
                                        <td>
                                            <telerik:RadComboBox ID="cboReportStatus" runat="server" OnClientSelectedIndexChanged="EnableSave" CssClass="Editabletxtbox">
                                            </telerik:RadComboBox>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkFasting" runat="server" Text="Fasting" onclick="EnableSave();" CssClass="Editabletxtbox" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="7">
                                            <telerik:RadGrid ID="grdSubComponent" runat="server" Width="788px" Height="520px"
                                                CellSpacing="0" AutoGenerateColumns="False" OnItemCreated="grdSubComponent_ItemCreated" 
                                                OnItemUpdated="grdSubComponent_ItemUpdated" OnItemCommand="grdSubComponent_ItemCommand"
                                                 CssClass="Gridbodystyle">
                                                <filtermenu enableimagesprites="False">
                                                </filtermenu>
                                                <clientsettings scrolling-usestaticheaders="true">
                                                    <Selecting AllowRowSelect="True" />
                                                    <Scrolling UseStaticHeaders="True" AllowScroll="true" />
                                                    <ClientEvents OnRowClick="EnableSave" OnCommand="MREOnCommand" OnRowSelected="RowSelected" />
                                                </clientsettings>
                                                <mastertableview tablelayout="Auto" datakeynames="Test">
                                                    <CommandItemSettings ExportToPdfText="Export to PDF" />
                                                    <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                                                    </RowIndicatorColumn>
                                                    <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                                                    </ExpandCollapseColumn>
                                                    <Columns >
                                                        <telerik:GridButtonColumn ButtonType="ImageButton"  CommandName="Del" DataTextField="Del"
                                                            FilterControlAltText="Filter Del column"  HeaderText="Del" ImageUrl="~/Resources/close_small_pressed.png"
                                                            UniqueName="Del" HeaderStyle-Width="35px" ItemStyle-Width="35px" ItemStyle-BorderWidth="1px">
                                                            
                                                            <HeaderStyle Width="35px" CssClass="Gridheaderstyle" />
                                                            <ItemStyle Width="35px" CssClass="Editabletxtbox" />
                                                            
                                                        </telerik:GridButtonColumn>
                                                        <telerik:GridTemplateColumn HeaderText="Test" UniqueName="Test" HeaderStyle-Width="200px"
                                                            ItemStyle-Width="200px" DataField="Test">
                                                            <ItemTemplate>
                                                                <telerik:RadTextBox ID="Test" runat="server" Text='<%#Eval("Test")%>' Width="190px"
                                                                    onkeypress="EnableSave();" onpaste="EnableSave();" oncut="EnableSave();" >
                                                                   </telerik:RadTextBox>
                                                            </ItemTemplate>
                                                             <EditItemTemplate>
                                                                    <telerik:RadTextBox ID="Test" runat="server" 
                                                                        Text='<%# Bind("Test") %>' onpaste="EnableSave();" oncut="EnableSave();">
                                                                    </telerik:RadTextBox>
                                                                </EditItemTemplate>
                                                            <HeaderStyle Width="200px" CssClass="Gridheaderstyle"/>
                                                            <ItemStyle Width="200px" CssClass="Editabletxtbox" />
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn HeaderText="Result" UniqueName="Result" HeaderStyle-Width="60px"
                                                            DataField="Result" ItemStyle-Width="60px">
                                                            <ItemTemplate>
                                                                <telerik:RadTextBox ID="Result" runat="server" Text='<%#Bind("Result")%>' Width="50px"
                                                                    onkeypress="EnableSave();" onpaste="EnableSave();" oncut="EnableSave();">
                                                                </telerik:RadTextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="60px" CssClass="Gridheaderstyle"/>
                                                            <ItemStyle Width="60px" CssClass="Editabletxtbox" />
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn HeaderText="Flag" UniqueName="Flag" DataField="Flag"
                                                            HeaderStyle-Width="120px" ItemStyle-Width="120px">
                                                            <ItemTemplate>
                                                                <telerik:RadComboBox ID="Flag" runat="server" Width="105px" AutoPostBack="false"
                                                                    OnClientSelectedIndexChanged="EnableSave">
                                                                </telerik:RadComboBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="120px" CssClass="Gridheaderstyle"/>
                                                            <ItemStyle Width="120px"  CssClass="Editabletxtbox"/>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn HeaderText="Units" UniqueName="Units" HeaderStyle-Width="70px"
                                                            ItemStyle-Width="70px" DataField="Units">
                                                            <ItemTemplate>
                                                                <telerik:RadTextBox ID="Units" runat="server" Text='<%#Eval("Units")%>' Width="60px"
                                                                    onkeypress="EnableSave();" onpaste="EnableSave();" oncut="EnableSave();">
                                                                </telerik:RadTextBox>
                                                            </ItemTemplate>
                                                             <EditItemTemplate>
                                                               <telerik:RadTextBox ID="Units" runat="server" Text='<%# Bind("Units") %>' Width="60px" onpaste="EnableSave();" oncut="EnableSave();">
                                                           </telerik:RadTextBox>
                                                           </EditItemTemplate>
                                                            <HeaderStyle Width="70px" CssClass="Gridheaderstyle" />
                                                            <ItemStyle Width="70px"  CssClass="Editabletxtbox"/>
                                                        </telerik:GridTemplateColumn>    
                                                        
                                                          <telerik:GridTemplateColumn HeaderText="Ref range" UniqueName="Refrange" HeaderStyle-Width="100px"
                                                            ItemStyle-Width="100px" DataField="Ref range">
                                                            <ItemTemplate>
                                                                <telerik:RadTextBox ID="Refrange" runat="server" Text='<%#Eval("Refrange")%>' Width="90px"
                                                                    onkeypress="EnableSave();" onpaste="EnableSave();" oncut="EnableSave();">
                                                                </telerik:RadTextBox>
                                                            </ItemTemplate>
                                                             <EditItemTemplate>
                                                               <telerik:RadTextBox ID="Refrange" runat="server" Text='<%# Bind("Refrange") %>' Width="90px" onpaste="EnableSave();" oncut="EnableSave();">
                                                           </telerik:RadTextBox>
                                                           </EditItemTemplate>
                                                            <HeaderStyle Width="100px" CssClass="Gridheaderstyle"/>
                                                            <ItemStyle Width="100px"  CssClass="Editabletxtbox"/>
                                                        </telerik:GridTemplateColumn>   
                                                       
                                                        <telerik:GridBoundColumn DataField="NTE" HeaderText="NTE" UniqueName="NTE" Display="false"
                                                            EmptyDataText="">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Source" HeaderText="Source" UniqueName="Source"
                                                            Display="false" EmptyDataText="">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Order Submit ID" HeaderText="Order Submit ID"
                                                            UniqueName="OrderSubmitID" Display="false" EmptyDataText="">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Current Process" HeaderText="Current Process"
                                                            UniqueName="CurrentProcess" Display="false" EmptyDataText="">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Test Code" HeaderText="Test Code" UniqueName="TestCode"
                                                            Display="false" EmptyDataText="">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Acurus Result Code" HeaderText="Acurus Result Code"
                                                            UniqueName="AcurusResultCode" Display="false" EmptyDataText="">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Acurus Result Description" HeaderText="Acurus Result Description"
                                                            UniqueName="AcurusResultDescription" Display="false" EmptyDataText="">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="ProcedureCode" HeaderText="ProcedureCode" UniqueName="ProcedureCode"
                                                            Display="false" EmptyDataText="">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="SNo" HeaderText="SNo" UniqueName="SNo" Display="false"
                                                            EmptyDataText="">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="OrderCodeDescription" HeaderText="OrderCodeDescription"
                                                            UniqueName="OrderCodeDescription" Display="false" EmptyDataText="">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Order Code" HeaderText="Order Code" UniqueName="OrderCode"
                                                            EmptyDataText="" Display="false">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridTemplateColumn HeaderText="Notes" UniqueName="Notes" DataField="Notes"
                                                            HeaderStyle-Width="160px" ItemStyle-Width="160px">
                                                            <ItemTemplate>
                                                                <telerik:RadTextBox ID="Notes" runat="server" Width="150px" Text='<%#Bind("Notes")%>'
                                                                    onkeypress="EnableSave();" onpaste="EnableSave();" oncut="EnableSave();">
                                                                </telerik:RadTextBox></ItemTemplate>
                                                            <HeaderStyle Width="160px" CssClass="Gridheaderstyle"/>
                                                            <ItemStyle Width="160px"  CssClass="Editabletxtbox"/>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridBoundColumn DataField="OBX ID" HeaderText="OBX ID" UniqueName="OBXID" Display="false"
                                                            EmptyDataText="">
                                                        </telerik:GridBoundColumn>
                                                    </Columns>
                                                    <EditFormSettings>
                                                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                                        </EditColumn>
                                                    </EditFormSettings>
                                                </mastertableview>
                                            </telerik:RadGrid>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="7" align="right">
                                            <asp:Button ID="btnAddNewRow" runat="server" Text="Add New Row" OnClick="btnAddNewRow_Click" CssClass="aspresizedbluebutton"
                                               ></asp:Button>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                        </td>
                                        <td style="width: 10%" align="right">                                           
                                            <asp:Button ID="btnAdd" runat="server" Text="Save" Width="100%" OnClick="btnAdd_Click" CssClass="aspresizedgreenbutton" >
                                            </asp:Button>   
                                       
                                        </td>
                                        <td style="width: 10%" align="right" colspan="2">
                                            <asp:Button ID="btnSaveandMovetonextprocess" Width="100%" runat="server" Text="Save and Move to Next Process"  OnClick="btnSaveandMovetonextprocess_Click" CssClass="aspresizedbluebutton" ></asp:Button>
                                        </td>
                                        <td style="width: 10%" align="right">
                                            <asp:Button ID="btnClearAll" runat="server" Width="100%" Text="Clear All" OnClientClick="return ClearAll();" CssClass="aspresizedredbutton">
                                            </asp:Button>
                                        </td>
                                        <td style="width: 10%" align="right">
                                            <asp:Button ID="btnClose" runat="server" Width="100%" Text="Close" OnClientClick="return Close();" CssClass="aspresizedredbutton">
                                            </asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
                <telerik:RadScriptManager ID="scriptMngr" runat="server" AsyncPostBackTimeout="36000">
                    <scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js">
                </asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js">
                </asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js">
                </asp:ScriptReference>
            </scripts>
                </telerik:RadScriptManager>
                <asp:HiddenField ID="hdnSelectedItem" runat="server" />
                <asp:HiddenField ID="hdnLabName" runat="server" />
                <asp:HiddenField ID="hdnLabID" runat="server" />
                <asp:HiddenField ID="hdnBirthDate" runat="server" />
                <asp:HiddenField ID="hdnOrderSubmitID" runat="server" />
                <asp:HiddenField ID="hdnHumanID" runat="server" />
                <asp:HiddenField ID="hdnOrderSubmitWorkflow" runat="server" />
                <asp:HiddenField ID="ulLabID" runat="server" />
                <asp:HiddenField ID="Gender" runat="server" />
                <asp:HiddenField ID="SaveAndMoveClick" runat="server" />
                <asp:HiddenField ID="hdnPhyID" runat="server" />
                <asp:HiddenField ID="hdnSelectedPanel" runat="server" />
                <asp:HiddenField ID="hdnAddNewOrderSubmitID" runat="server" />
                <asp:HiddenField ID="hdnHeader" runat="server" />
                <asp:HiddenField ID="hdnMessageType" runat="server" />
                <asp:HiddenField ID="hdnLocalTime" runat="server" />
                <asp:HiddenField ID="hdnOpenningMode" runat="server" />
                 <asp:HiddenField ID="hdnMoveToNext" runat="server" />
                 <asp:HiddenField ID="hdnIsRowClick" runat="server" />
                 <asp:HiddenField ID="hdnSelectedIndex" runat="server" />
                 <asp:HiddenField ID="YesNoCancelMessage" runat="server" />
                <asp:HiddenField ID="hdnScreenMode" runat="server" />
                 <asp:Button ID="btnFillGrid" runat="server" Text="Button" Style="display: none" OnClick="btnFillGrid_Click" />
                <asp:Button ID="btnMessageType" runat="server" Text="Button" Style="display: none"
                    OnClientClick="return Close();" />
                <asp:Button ID="InvisibleButton" runat="server" OnClick="InvisibleButton_Click" CssClass="displayNone" />
                <asp:Button ID="btnClear" runat="server" OnClick="btnClearAll_Click" CssClass="displayNone" />            
            </asp:Panel>
        </telerik:RadAjaxPanel>
    </div>
    <asp:PlaceHolder ID="PlaceHolder1" runat="server">   

    <script src="JScripts/JSMRE.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

    <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

    <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

    <script src="JScripts/jquery-1.7.1.min.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

    <script src="JScripts/jquery.min.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

    <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
</asp:PlaceHolder>
    </form>
</body>
</html>
