<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmBlockDaysDetails.aspx.cs"
    Inherits="Acurus.Capella.UI.frmBlockDaysDetails" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Block Days Details</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
      <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
<asp:PlaceHolder runat="server">
      <link href="CSS/jquery-ui.css" rel="stylesheet" />
            <%--  <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
            <script src="JScripts/jquery-ui.min1.10.2.js" type="text/javascript"></script>--%>

            <%--<script src="JScripts/jquery-1.11.3.min.js"></script>
            <script src="JScripts/jquery-ui.js"></script>
            <script src="JScripts/bootstrap.min.js"></script>--%>
    <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
     <script src="JScripts/JSBlockDays.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <%--<script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>--%>
    <script language="javascript" type="text/javascript">
        function EditBlockDays() {
            var grid = document.getElementById("grdBlockDays");
            var index = parseInt(document.getElementById('hdnIndex').value) + 1;
            var Result = new Object();
            Result.fromdate = document.getElementById('hdnFromDate').value;
            Result.todate = document.getElementById('hdnToDate').value;
            Result.fromtime = document.getElementById('hdnFromTime').value;
            Result.totime = document.getElementById('hdnToTime').value;
            Result.selectedday = document.getElementById('hdnDaySelected').value;
            Result.description = document.getElementById('hdnDescription').value;
            Result.blocktype = document.getElementById('hdnBlockType').value;
            Result.blockid = document.getElementById('hdnBlockId').value;
            Result.phyname = document.getElementById('hdnPhysician').value;
            Result.AlternateWeeks = document.getElementById('hdnAlternateWeeks').value;
            Result.AlternateMonths = document.getElementById('hdnAlternateMonths').value;
            Result.index = index;
            if (window.opener) {
                window.opener.returnValue = Result;
            }
            window.returnValue = Result;
            returnToParent(Result);
            return true;
            //self.close();
        }
        function CancelClick() {
            var Result = new Object();
           
            document.getElementById('hdnGetClick').value = "True";
            returnToParent(Result);
            return true;
        }

        function Blockdays() {
            $("[id*=pbDropdown]").addClass('pbDropdownBackground');
            $("span[mand=Yes]").addClass('MandLabelstyle');

            $("span[mand=Yes]").each(function () {
                $(this).html($(this).html().replace("*", "<span class='manredforstar'>*</span>"));
            });
        }
        function ShowBlockDaysClick1(oWindow, args) {
            var Result = args.get_argument();
            if (Result != null) {
                document.getElementById('hdnIndex').value = Result.index;
                document.getElementById('hdnFromTime').value = Result.fromtime;
                document.getElementById('hdnToTime').value = Result.totime;
                document.getElementById('hdnBlockDayType').value = Result.blocktype;
                document.getElementById('hdnPhySelected').value = Result.phyname.trim();

            }
           

            if (Result != null) {
                // var dtpRecurFromDate = $find("tbblockDays_tbblockRecurring_dtpRecurFromDate_dateInput"); 
                if (Result.blocktype == "RECURSIVE") {
                    document.getElementById('hdnBlockdaysId').value = Result.blockid;
                    //document.getElementById(GetClientId('dtpRecurFromDate')).value=Result.fromdate;
                    //            $find("tbblockDays_tbblockRecurring_dtpRecurFromDate_dateInput").set_selectedDate = Result.fromdate;
                    //            document.getElementById(GetClientId('dtpRecurToDate')).value = Result.todate;
                    document.getElementById('hdnFromDate').value = Result.fromdate;
                    document.getElementById('hdnToDate').value = Result.todate;
                    document.getElementById('hdnDays').value = Result.selectedday;
                    document.getElementById('hdnRecDescription').value = Result.description;
                }
                else if (Result.blocktype == "NON RECURSIVE") {
                    document.getElementById('hdnNonRecBlockDaysId').value = Result.blockid;
                    document.getElementById('hdnFromDate').value = Result.fromdate;
                    document.getElementById('hdnNonRecurDescription').value = Result.description;
                }
                else {
                    document.getElementById('hdnBlockdaysId').value = Result.blockid;
                    document.getElementById(GetClientId('dtpRecurFromDate')).value = Result.fromdate;
                    document.getElementById(GetClientId('dtpRecurToDate')).value = Result.todate;
                    document.getElementById('hdnDays').value = Result.selectedday;
                    document.getElementById('hdnRecDescription').value = Result.description;
                }
                $find("btnInvisibleForBlockDays").click(true);
            }
            //    document.getElementById(GetClientId("btnload")).click();


        }
        function CloseForBlockDetails() {
            if (document.getElementById("btnEdit").value == "Edit All Occurrence")
                self.close();
            else if(document.getElementById("btnEdit").value == "Edit Occurrence")
               self.close();
        }
        function ConfirmDelete() 
        {
          if(DisplayErrorMessage('110201')==true)
          {
             document.getElementById("hdnTrue").value="Select";
          }
          else
          {
          document.getElementById("hdnTrue").value="";          
          }
        
          //  return DisplayErrorMessage('110201');            
        }
        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement != null && window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }


        function returnToParent(args) {
            var oArg = new Object();
            oArg.result = args;
            var oWnd = GetRadWindow();
            if (oWnd != null) {
                if (oArg.result) {
                    oWnd.close(oArg.result);
                }
                else {
                    // alert("Please Click any one item");
                    oWnd.close(oArg.result);
                }
            }
            else {
                self.close();
            }
        }
    </script>

    <title>Block Days Details</title>
    <base target="_self" />
    <style type="text/css">
        #form1
        {
            width: 1084px;
        }
        .displaynone
        {
            display: none;
        }
        .Panel legend
        {
            font-weight: bold;
        }
    </style>
  <%-- <link href="CSS/ElementStyles.css" rel="stylesheet" type="text/css" />--%>
    </asp:PlaceHolder>
</head>
<body onload="Blockdays();">
    <form id="form11" runat="server">
    <div>
        <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>--%>
        <%--<asp:ToolkitScriptManager ID="ToolkitScriptManager2" runat="server">
        </asp:ToolkitScriptManager>--%>
        <telerik:RadWindowManager ID="WindowMngr" runat="server">
            <Windows>
                <telerik:RadWindow ID="MessageWindow" runat="server" Title="DLC"
                    Height="625px" IconUrl="Resources/16_16.ico" Width="1225px" BackColor="#BFDBFF">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
            </Scripts>
        </ajaxToolkit:ToolkitScriptManager>
        <asp:Panel ID="pnlBlockDetails" runat="server" Font-Size="Small" CssClass="Editabletxtbox"
            GroupingText="Block Details">
            <table style="width: 100%;">
                <tr>
                    <td style="width: 100%;">
                        <asp:Panel ID="pnlSelectFacilityAndPhysician" runat="server" GroupingText="Select Facility &amp; Physician" CssClass="Editabletxtbox">
                            <table style="width: 100%;">
                                <tr>
                                    <td style="width: 5%;">
                                        <asp:Label ID="Label1" runat="server" Width="80px"  Text="Facility*" CssClass="Editabletxtbox" mand="Yes"></asp:Label>
                                    </td>
                                    <td style="width: 20%;">
                                        <asp:DropDownList ID="ddlFacilityName" runat="server" AutoPostBack="True" Height="20px"
                                            OnSelectedIndexChanged="ddlFacilityName_SelectedIndexChanged" Width="220px" CssClass="Editabletxtbox">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 5%;">
                                        <asp:Label ID="lblProvider" Width="80px" runat="server" Text="Provider*" CssClass="Editabletxtbox" mand="Yes"></asp:Label>
                                    </td>
                                   <td style="width: 20%;">
                                        <asp:DropDownList ID="ddlProvider" runat="server" AutoPostBack="True" Height="20px"
                                            Width="220px">
                                        </asp:DropDownList>
                                    </td>
                                   <td style="width: 25%;">
                                        <asp:CheckBox ID="chkShowAll" runat="server" Text="Show All Block Days" CssClass="Editabletxtbox"/>
                                    </td>
                                   <td style="width: 25%;">
                                        <asp:Button ID="btnGetBlockDetails" runat="server" OnClick="btnGetBlockDetails_Click"
                                            Text="Get Block Details" Width="150px" CssClass="aspresizedbluebutton"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <asp:Label ID="lblResult" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Panel ID="pnlBlockDyasDetails" runat="server" Height="260px">
                            <%-- <asp:GridView ID="grdBlockDays" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="True"
                                BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Both" Width="1020px"
                                DataKeyNames="Block Date,Facility Id,Group Id,Block Id,Day Choosan" OnRowCommand="grdBlockDays_RowCommand"
                                CssClass="Radcss_Vista" OnRowDataBound="grdBlockDays_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Del" ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="false" CommandName="Del"
                                                OnClientClick="return ConfirmDelete();" ImageUrl="~/Resources/close_small_pressed.png"
                                                Text="Del" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField AccessibleHeaderText="Facility Name" DataField="Facility Name" HeaderText="Facility Name" />
                                    <asp:BoundField AccessibleHeaderText="Provider Name" DataField="Provider Name" HeaderText="Provider Name" />
                                    <asp:BoundField AccessibleHeaderText="Block Day" DataField="Block Day" HeaderText="Block Day"
                                        ItemStyle-Wrap="true">
                                        <ItemStyle Wrap="True" />
                                    </asp:BoundField>
                                    <asp:BoundField AccessibleHeaderText="Block Type" DataField="Block Type" HeaderText="Block Type" />
                                    <asp:BoundField AccessibleHeaderText="Description" DataField="Description" HeaderText="Description" />
                                    <asp:BoundField AccessibleHeaderText="From Date" DataField="From Date" HeaderText="From Date" />
                                    <asp:BoundField AccessibleHeaderText="To Date" DataField="To Date" HeaderText="To Date" />
                                    <asp:BoundField AccessibleHeaderText="From Time" DataField="From Time" HeaderText="From Time" />
                                    <asp:BoundField AccessibleHeaderText="To Time" DataField="To Time" HeaderText="To Time" />
                                    <asp:BoundField AccessibleHeaderText="Block Id" DataField="Block Id" HeaderText="Block Id"
                                        ItemStyle-CssClass="displaynone" HeaderStyle-CssClass="displaynone">
                                        <HeaderStyle CssClass="displaynone" />
                                        <ItemStyle CssClass="displaynone" />
                                    </asp:BoundField>
                                    <asp:BoundField AccessibleHeaderText="Group Id" DataField="Group Id" HeaderText="Group Id"
                                        ItemStyle-CssClass="displaynone" HeaderStyle-CssClass="displaynone">
                                        <HeaderStyle CssClass="displaynone" />
                                        <ItemStyle CssClass="displaynone" />
                                    </asp:BoundField>
                                    <asp:BoundField AccessibleHeaderText="Facility Id" DataField="Facility Id" HeaderText="Facility Id"
                                        Visible="False" />
                                    <asp:BoundField AccessibleHeaderText="Block Date" DataField="Block Date" HeaderText="Block Date"
                                        Visible="False" />
                                    <asp:BoundField AccessibleHeaderText="Day Choosan" DataField="Day Choosan" HeaderText="Day Choosan"
                                        Visible="False" />
                                    <asp:BoundField AccessibleHeaderText="BlockDaysId" DataField="BlockDaysId" HeaderText="BlockDaysId"
                                        ItemStyle-CssClass="displaynone" HeaderStyle-CssClass="displaynone">
                                        <HeaderStyle CssClass="displaynone" />
                                        <ItemStyle CssClass="displaynone" />
                                    </asp:BoundField>
                                    <asp:ButtonField CommandName="SingleClick" Text="btnSingleClick" Visible="False" />
                                </Columns>
                                <SelectedRowStyle CssClass="gridSelectedRow" />
                                <HeaderStyle CssClass="GridHeaderRow" />
                            </asp:GridView>--%>
                            <telerik:RadGrid ID="grdBlockDays" runat="server" AutoGenerateColumns="False" CellSpacing="0"
                                GridLines="None" Width="1050px"  OnItemCommand="grdBlockDays_ItemCommand"
                                OnSelectedIndexChanged="grdBlockDays_SelectedIndexChanged" CssClass="Gridbodystyle"><%-- ClientSettings-EnablePostBackOnRowClick="false" --%>
                                <FilterMenu EnableImageSprites="False">
                                </FilterMenu>
                                <ClientSettings AllowKeyboardNavigation="true" Scrolling-UseStaticHeaders="true"
                                    EnablePostBackOnRowClick="true">
                                    <Selecting AllowRowSelect="True" />
                                    <Scrolling AllowScroll="True" ScrollHeight="220px" UseStaticHeaders="True" />
                                   <%-- <ClientEvents OnRowClick="grdBlockDays_OnRowClick" />--%>
                                    <KeyboardNavigationSettings EnableKeyboardShortcuts="true"></KeyboardNavigationSettings>
                                </ClientSettings>
                                <MasterTableView CssClass="Gridbodystyle">
                                    <CommandItemSettings ExportToPdfText="Export to PDF" />
                                    <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                                    </RowIndicatorColumn>
                                    <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                                    </ExpandCollapseColumn>
                                    <Columns>
                                        <telerik:GridButtonColumn ButtonType="ImageButton" ConfirmText="Are you sure you want to delete this line item?"
                                            ConfirmDialogType="RadWindow" ConfirmTitle="Delete" CommandName="DeleteRow" FilterControlAltText="Filter column column"
                                            HeaderText="Del" ImageUrl="~/Resources/close_small_pressed.png" UniqueName="column">
                                            <HeaderStyle Width="30px" CssClass="Gridheaderstyle" />
                                            </telerik:GridButtonColumn>
                                        <telerik:GridBoundColumn DataField="Facility Name" FilterControlAltText="Filter column1 column"
                                            HeaderText="Facility Name" UniqueName="column1">
                                            <HeaderStyle Width="130px" CssClass="Gridheaderstyle" />
                                             </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Provider Name" FilterControlAltText="Filter column2 column"
                                            HeaderText="Provider Name" UniqueName="column2">
                                            <HeaderStyle Width="150px" CssClass="Gridheaderstyle"  />
                                             </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Block Day" FilterControlAltText="Filter column3 column"
                                            HeaderText="Block Day" UniqueName="column3">
                                            <HeaderStyle Width="100px" CssClass="Gridheaderstyle" />
                                            </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Block Type" FilterControlAltText="Filter column4 column"
                                            HeaderText="Block Type" UniqueName="column4">
                                            <HeaderStyle Width="100px" CssClass="Gridheaderstyle" />
                                            </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Description" FilterControlAltText="Filter column5 column"
                                            HeaderText="Description" UniqueName="column5">
                                            <HeaderStyle Width="100px" CssClass="Gridheaderstyle" />
                                            </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="From Date" FilterControlAltText="Filter column6 column"
                                            HeaderText="From Date" UniqueName="column6">
                                            <HeaderStyle Width="100px" CssClass="Gridheaderstyle" />
                                            </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="To Date" FilterControlAltText="Filter column7 column"
                                            HeaderText="To Date" UniqueName="column7">
                                            <HeaderStyle Width="100px" CssClass="Gridheaderstyle"  />
                                            </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="From Time" FilterControlAltText="Filter column8 column"
                                            HeaderText="From Time" UniqueName="column8">
                                            <HeaderStyle Width="80px" CssClass="Gridheaderstyle" />
                                             </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="To Time" FilterControlAltText="Filter column9 column"
                                            HeaderText="To Time" UniqueName="column9">
                                            <HeaderStyle Width="80px" CssClass="Gridheaderstyle" />
                                            </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Block Id" Display="false" FilterControlAltText="Filter column10 column"
                                            HeaderText="Block Id" UniqueName="column10">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Group Id" Display="false" FilterControlAltText="Filter column11 column"
                                            HeaderText="Group Id" UniqueName="column11">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Facility Id" Display="false" FilterControlAltText="Filter column12 column"
                                            HeaderText="Facility Id" UniqueName="column12">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Block Date" Display="false" FilterControlAltText="Filter column13 column"
                                            HeaderText="Block Date" UniqueName="column13">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Day Choosan" Display="false" FilterControlAltText="Filter column14 column"
                                            HeaderText="Day Choosan" UniqueName="column14">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="BlockDaysId" Display="false" FilterControlAltText="Filter column15 column"
                                            HeaderText="BlockDaysId" UniqueName="column15">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Physician Id" Display="false" FilterControlAltText="Filter column16 column"
                                            HeaderText="Physician Id" UniqueName="column16" ></telerik:GridBoundColumn>
                                          <telerik:GridBoundColumn DataField="AlternateWeeks"  FilterControlAltText="Filter column17 column"
                                            HeaderText="Alternate Weeks" UniqueName="AlternateWeeks">
                                              <HeaderStyle Width="150px" CssClass="Gridheaderstyle" />
                                        </telerik:GridBoundColumn>
                                          <telerik:GridBoundColumn DataField="AlternateMonths"  FilterControlAltText="Filter column18 column"
                                            HeaderText="Alternate Months" UniqueName="AlternateMonths">
                                              <HeaderStyle Width="150px" CssClass="Gridheaderstyle" />
                                        </telerik:GridBoundColumn>
                                    </Columns>
                                    <EditFormSettings>
                                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                        </EditColumn>
                                    </EditFormSettings>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Panel ID="Bottom" runat="server" Font-Size="Small">
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                        <asp:LinkButton ID="btnFirst" runat="server"  OnCommand="PageChangeEventHandler" CommandArgument="First" CssClass="alinkstyle" disabled="disabled" >First</asp:LinkButton>
                                        &nbsp;
                                        <asp:LinkButton ID="btnPrevious" runat="server" OnCommand="PageChangeEventHandler"
                                            CommandArgument="Previous" CssClass="alinkstyle" disabled="disabled" >Previous</asp:LinkButton>
                                        &nbsp;
                                        <asp:LinkButton ID="btnNext" runat="server" OnCommand="PageChangeEventHandler" CommandArgument="Next" CssClass="alinkstyle" disabled="disabled" >Next</asp:LinkButton>
                                        &nbsp;
                                        <asp:LinkButton ID="btnLast" runat="server" OnCommand="PageChangeEventHandler" CommandArgument="Last" CssClass="alinkstyle"  disabled="disabled">Last</asp:LinkButton>
                                        &nbsp; &nbsp;
                                       <asp:Label ID="lblShowing" EnableViewState="false" runat="server" ClientIdMode="Static"></asp:Label>
                                    </td>
                                    <td align="right">
                                        <asp:Button ID="btnAll" Text="Edit Single Occurrence"  runat="server" Width="150px" Enabled="false" OnClick="btnAll_Click" CssClass="aspresizedbluebutton"/>&nbsp;&nbsp;&nbsp;<%--OnClientClick="return OpenBlockDetails();"--%>
                                        <asp:Button ID="btnEdit" runat="server" Text="Edit All Occurrence"  OnClick="btnEdit_Click" Width="125px" Enabled="false" CssClass="aspresizedbluebutton"/>&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" Width="75px" OnClientClick="return CloseForBlockDetails();" CssClass="aspresizedredbutton" OnClick="btnCancel_Click"/>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <%-- <asp:Panel ID="Bottom" runat="server" Font-Size="Small" Width="1054px">
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:LinkButton ID="btnFirst" runat="server" OnCommand="PageChangeEventHandler" CommandArgument="First">First</asp:LinkButton>
                        &nbsp;
                        <asp:LinkButton ID="btnPrevious" runat="server" OnCommand="PageChangeEventHandler"
                            CommandArgument="Previous">Previous</asp:LinkButton>
                        &nbsp;
                        <asp:LinkButton ID="btnNext" runat="server" OnCommand="PageChangeEventHandler" CommandArgument="Next">Next</asp:LinkButton>
                        &nbsp;
                        <asp:LinkButton ID="btnLast" runat="server" OnCommand="PageChangeEventHandler" CommandArgument="Last">Last</asp:LinkButton>
                    </td>
                    <td>
                        <asp:Label ID="lblShowing" EnableViewState="false" runat="server" ClientIdMode="Static"></asp:Label>
                    </td>
                    <td align="right">
                        <asp:Button ID="btnEdit" runat="server" OnClick="btnEdit_Click" Text="Edit" Width="75px" />&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" Width="75px" OnClientClick="return Close();" />
                    </td>
                </tr>
            </table>
        </asp:Panel>--%>
        <asp:HiddenField ID="hdnFromDate" runat="server" />
        <asp:HiddenField ID="hdnToDate" runat="server" />
        <asp:HiddenField ID="hdnFromTime" runat="server" />
        <asp:HiddenField ID="hdnToTime" runat="server" />
        <asp:HiddenField ID="hdnDaySelected" runat="server" />
        <asp:HiddenField ID="hdnDescription" runat="server" />
        <asp:HiddenField ID="hdnBlockType" runat="server" />
        <asp:HiddenField ID="hdnFacilityName" runat="server" />
        <asp:HiddenField ID="hdnPhysician" runat="server" />
        <asp:HiddenField ID="hdnBlockId" runat="server" />
        <asp:HiddenField ID="hdnIndex" runat="server" />
        <asp:HiddenField ID="hdnSelectedIndex" runat="server" />
        <asp:HiddenField ID="hdnTrue" runat="server" />
        <asp:HiddenField ID="hdnPhysicianID" runat="server" />
        <asp:HiddenField ID="hdnGroupID" runat="server" />      
        <asp:HiddenField ID="hdnAlternateWeeks" runat="server" />
        <asp:HiddenField ID="hdnAlternateMonths" runat="server" />  
        <%--</ContentTemplate>
        </asp:UpdatePanel>--%>
    </div>
    <%-- <div style="width: 1077px; height: 435px;">
        <asp:Panel ID="pnlBlockDetails" runat="server" Font-Size="Small" GroupingText="Block Details"
            Height="441px" Style="margin-right: 0px" Width="1071px" CssClass="Panel">
            <table style="width: 99%; height: 394px;">
                <tr>
                    <td class="style1" colspan="3">
                        <asp:Panel ID="pnlSelectFacilityAndPhysician" runat="server" GroupingText="Select Facility &amp; Physician"
                            Height="75px" Width="1057px">
                            <table style="width: 100%; height: 37px;">
                                <tr>
                                    <td class="style3">
                                        <asp:Label ID="Label1" runat="server" ForeColor="Red" Text="Facility*"></asp:Label>
                                    </td>
                                    <td class="style2">
                                        <asp:DropDownList ID="ddlFacilityName" runat="server" AutoPostBack="True" Height="20px"
                                            OnSelectedIndexChanged="ddlFacilityName_SelectedIndexChanged" Width="227px">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="style5">
                                        <asp:Label ID="lblProvider" runat="server" ForeColor="Red" Text="Provider*"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlProvider" runat="server" AutoPostBack="True" Height="27px"
                                            Width="225px">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="style4">
                                        <asp:CheckBox ID="chkShowAll" runat="server" Text="Show All Block Days" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnGetBlockDetails" runat="server" OnClick="btnGetBlockDetails_Click"
                                            Text="Get Block Details" Width="181px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style3" colspan="2">
                                        <asp:Label ID="lblResult" runat="server"></asp:Label>
                                    </td>
                                    <td class="style5">
                                    </td>
                                    <td>
                                    </td>
                                    <td class="style4">
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td class="style6" colspan="3">
                        <asp:Panel ID="pnlBlockDyasDetails" runat="server" Height="284px" ScrollBars="Auto"
                            BorderWidth="1px">
                            <asp:GridView ID="grdBlockDays" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="True"
                                BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Both" Width="1051px"
                                DataKeyNames="Block Date,Facility Id,Group Id,Block Id,Day Choosan" OnRowCommand="grdBlockDays_RowCommand"
                                CssClass="Radcss_Vista" OnRowDataBound="grdBlockDays_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Del" ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="false" CommandName="Del"
                                                OnClientClick="return ConfirmDelete();" ImageUrl="~/Resources/close_small_pressed.png"
                                                Text="Del" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField AccessibleHeaderText="Facility Name" DataField="Facility Name" HeaderText="Facility Name" />
                                    <asp:BoundField AccessibleHeaderText="Provider Name" DataField="Provider Name" HeaderText="Provider Name" />
                                    <asp:BoundField AccessibleHeaderText="Block Day" DataField="Block Day" HeaderText="Block Day"
                                        ItemStyle-Wrap="true">
                                        <ItemStyle Wrap="True" />
                                    </asp:BoundField>
                                    <asp:BoundField AccessibleHeaderText="Block Type" DataField="Block Type" HeaderText="Block Type" />
                                    <asp:BoundField AccessibleHeaderText="Description" DataField="Description" HeaderText="Description" />
                                    <asp:BoundField AccessibleHeaderText="From Date" DataField="From Date" HeaderText="From Date" />
                                    <asp:BoundField AccessibleHeaderText="To Date" DataField="To Date" HeaderText="To Date" />
                                    <asp:BoundField AccessibleHeaderText="From Time" DataField="From Time" HeaderText="From Time" />
                                    <asp:BoundField AccessibleHeaderText="To Time" DataField="To Time" HeaderText="To Time" />
                                    <asp:BoundField AccessibleHeaderText="Block Id" DataField="Block Id" HeaderText="Block Id"
                                        ItemStyle-CssClass="displaynone" HeaderStyle-CssClass="displaynone">
                                        <HeaderStyle CssClass="displaynone" />
                                        <ItemStyle CssClass="displaynone" />
                                    </asp:BoundField>
                                    <asp:BoundField AccessibleHeaderText="Group Id" DataField="Group Id" HeaderText="Group Id"
                                        ItemStyle-CssClass="displaynone" HeaderStyle-CssClass="displaynone">
                                        <HeaderStyle CssClass="displaynone" />
                                        <ItemStyle CssClass="displaynone" />
                                    </asp:BoundField>
                                    <asp:BoundField AccessibleHeaderText="Facility Id" DataField="Facility Id" HeaderText="Facility Id"
                                        Visible="False" />
                                    <asp:BoundField AccessibleHeaderText="Block Date" DataField="Block Date" HeaderText="Block Date"
                                        Visible="False" />
                                    <asp:BoundField AccessibleHeaderText="Day Choosan" DataField="Day Choosan" HeaderText="Day Choosan"
                                        Visible="False" />
                                    <asp:BoundField AccessibleHeaderText="BlockDaysId" DataField="BlockDaysId" HeaderText="BlockDaysId"
                                        ItemStyle-CssClass="displaynone" HeaderStyle-CssClass="displaynone">
                                        <HeaderStyle CssClass="displaynone" />
                                        <ItemStyle CssClass="displaynone" />
                                    </asp:BoundField>
                                    <asp:ButtonField CommandName="SingleClick" Text="btnSingleClick" Visible="False" />
                                </Columns>
                                <SelectedRowStyle CssClass="gridSelectedRow" />
                                <HeaderStyle CssClass="GridHeaderRow" />
                            </asp:GridView>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td class="style8">
                        <asp:HiddenField ID="hdnBlockId" runat="server" />
                    </td>
                    <td align="right" class="style7">
                        <asp:Button ID="btnEdit" runat="server" OnClick="btnEdit_Click" Text="Edit" Width="75px" />
                    </td>
                    <td>
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" Width="75px" OnClientClick="return Close();" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>--%>
    <asp:HiddenField ID="hdnLastPageNo" runat="server" EnableViewState="false" />
    <asp:HiddenField ID="hdnTotalCount" runat="server" EnableViewState="false" />
         <asp:HiddenField ID="hdnUniqueBLockID" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnIs_Single_Occurence" runat="server" EnableViewState="false" />
        
    </form>
</body>
</html>
