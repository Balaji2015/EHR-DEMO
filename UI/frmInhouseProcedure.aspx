<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmInhouseProcedure.aspx.cs"
    Inherits="Acurus.Capella.UI.frmInhouseProcedure" EnableEventValidation="false"  ValidateRequest="false" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/UserControls/CustomPhrases.ascx" TagName="Phrases" TagPrefix="Phrases" %>
<%@ Register Src="~/UserControls/CustomDLCNew.ascx" TagName="DLC" TagPrefix="DLC" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Procedure</title>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
          ::-webkit-scrollbar {
            width: 6px;
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
        .displayNone {
            display: none;
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

        .underline {
            text-decoration: underline;
        }
        #pnlDLC{
            display: -webkit-inline-box!important;
        }
        .fa fa-question-circle .tooltip:hover .tooltiptext { visibility: visible; }
.fa fa-question-circle .tooltip { position: relative; display: inline-block;border-bottom: 1px dotted black;}
.tooltip { position: relative; display: inline-block;}
.tooltip .tooltiptext {visibility: hidden;width: 200px;background-color: black;color: #fff;text-align: center;border-radius: 6px;padding: 5px 0; position: absolute;z-index: 1;top: -5px;left: 105%;}
#tipforfindbtn:hover .tooltiptext {visibility: visible;}
    </style>
    <link href="CSS/style.css" rel="stylesheet" type="text/css" />
    <link href="CSS/font-awesome.css" rel="stylesheet" type="text/css" />
       <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />

</head>
<body onload="OnLoadProcedure();OnDLCLoad();">
    <form id="frmInhouseProcedure" runat="server" style="background-color: White; font-family: Microsoft Sans Serif; font-size: smaller; width: 100%; height: 700px"
        scrollbars="Auto">
        <telerik:RadStyleSheetManager ID="RadStyleSheetManager1" runat="server">
        </telerik:RadStyleSheetManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="80%" HorizontalAlign="NotSet"
            Width="100%">
            <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            </telerik:RadAjaxManager>
            <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
                <Scripts>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
                </Scripts>
            </telerik:RadScriptManager>
            <telerik:RadWindowManager ID="WindowMngr1" runat="server">
                <Windows>
                    <telerik:RadWindow ID="RadWindow1" runat="server" Height="640px" VisibleStatusbar="false" Width="950px" VisibleOnPageLoad="false"
                        Behaviors="None" Modal="true" IconUrl="Resources/16_16.ico">
                    </telerik:RadWindow>
                    <telerik:RadWindow ID="RadWindow2" runat="server" Title="ViewProcedure" Height="640px"
                        Width="950px" VisibleOnPageLoad="false" Behaviors="Resize,Move,Close" Modal="true"
                        IconUrl="Resources/16_16.ico" VisibleStatusbar="false">
                    </telerik:RadWindow>
                    <telerik:RadWindow ID="RadWindow3" runat="server" Height="900px" Width="900px" VisibleStatusbar="false" VisibleOnPageLoad="false"
                        Behaviors="Resize,Move,Close" Modal="true" IconUrl="Resources/16_16.ico">
                    </telerik:RadWindow>
                </Windows>
            </telerik:RadWindowManager>
            <telerik:RadWindowManager ID="WindowMngr" runat="server">
                <Windows>
                    <telerik:RadWindow ID="MessageWindow" runat="server" Title="InhouseProcedure" IconUrl="Resources/16_16.ico">
                    </telerik:RadWindow>
                </Windows>
            </telerik:RadWindowManager>
            <table id="tblParentInhouse" runat="server" style="width: 100%; height: 100%;">
                <tr style="height: 60%">
                    <td width="27%" height="100%">
                        <asp:Panel ID="pnlFrequent" runat="server" GroupingText="Frequently Used Procedure"
                            Width="100%" Height="100%" class="LabelStyleBold">
                            <table style="width: 100%; height: 70%;">
                                <tr style="height: 10%">
                                    <td width="30%">
                                        <asp:Label ID="lblSelectOtherProcedure"  runat="server" Text="Select the Procedure" CssClass="Editabletxtbox"
                                            ></asp:Label>
                                    </td>
                                </tr>
                                <tr style="height: 80%">
                                    <td width="100%">
                                        <asp:Panel ID="pnlCheckedList" runat="server" Height="390px" Width="100%" ScrollBars="Horizontal">
                                            <asp:CheckBoxList ID="chklstOtherProcedures" runat="server" TextAlign="Right" BorderWidth="1px"
                                                Width="360px" Style="width: auto; height: auto; min-height: 370px" AutoPostBack="true" class="chkitems"
                                                OnChange="{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}"
                                                OnSelectedIndexChanged="chklstOtherProcedures_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr style="height: 10%">
                                    <td width="30%">
                                        <telerik:RadButton ID="btnManageFrequentlyUsed" ButtonType="LinkButton" CssClass="bluebutton teleriknormalbuttonstyle" runat="server" Text="Manage Frequently Used Procedures"
                                             AccessKey="f" AutoPostBack="false" OnClientClicked="btnManageFrequentlyUsed_Click" style="height: 33px !important;">
                                        </telerik:RadButton>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                    <td width="73%">
                        <table style="width: 100%; height: 100%">
                            <tr style="width: 100%; height: 80%">
                                <td>
                                    <asp:Panel ID="pnlSelected" runat="server" GroupingText="Selected Procedure" Height="100%" Width="98%" class="LabelStyleBold">
                                        <table style="width: 100%">
                                            <tr style="width: 100%">
                                                <td style="width: 10.5%">
                                                  <%--  <asp:Label ID="lblProcedure" runat="server" class="Editabletxtbox"  mand="Yes"  Text="Procedure*"
                                                        Font-Bold="False" ></asp:Label>--%>
                                                    <span class="MandLabelstyle">Procedure</span><span class="manredforstar">*</span>
                                                   <%-- --%>
                                                </td>
                                                <td style="width: 90%">
                                                    <telerik:RadTextBox ID="txtProcedure" runat="server" Width="91%" Height="60px" CssClass="nonEditabletxtbox"
                                                        ReadOnly="True" TextMode="MultiLine" Font-Bold="False">
                                                    </telerik:RadTextBox>                                                  
                                                    <asp:ImageButton ID="pictureBox2" runat="server" Height="17px" ToolTip="Clear" ImageUrl="~/Resources/close_small_pressed.png"
                                                        OnClientClick="return procedureClear();" Visible="false" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 10%">
                                                    <asp:Label ID="lblNotes" runat="server" Text="Notes" class="Editabletxtbox" Font-Bold="False" ></asp:Label>
                                                </td>
                                                <td style="width: 90%">
                                                    <asp:Panel ID="pnlDLC" runat="server" Height="100%" Width="100%" BackColor="White"
                                                        Font-Size="Small" class="LabelStyleBold">
                                                         <DLC:DLC ID="ctmDLC_procedure" TextboxHeight="300px" TextboxWidth="700px" runat="server" TextboxMaxLength="8000"
                                                            Value="OTHER PROCEDURE NOTES" />

                                                        <Phrases:Phrases ID="pbProcedure" runat="server" MyFieldName="PROCEDURE" Height="12px"
                                                            Width="12px" />

                                                        <asp:PlaceHolder ID="phlProce" runat="server" />

                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <%--<tr style="width: 100%; height: 40%">
                                <td>
                                    <asp:Panel ID="pnlImplantable" runat="server" Height="100%" GroupingText="Implantable Device" Font-Bold="True">
                                       <table style="width:100%">
                                           <tr style="width: 100%">
                                                <td style="width: 20%">
                                                    <asp:Label ID="DevIdentifier" runat="server" Text="Device Identifier(UDI/DI)"
                                                        Font-Bold="False" Font-Size="Small"></asp:Label> 
                                                    <i id="tipforfindbtn" class="fa fa-question-circle tooltip"><span class="tooltiptext">Please enter the Device Indentifier(UDI/DI) and click find button.</span></i></td>
                                                <td style="width: 60%">
                                                    <asp:TextBox ID="txtDeviceIdentifier" runat="server" Width="85%" onclick="EnableFind();" onchange="EnableFind();"></asp:TextBox>                                              
                                                   <asp:Button ID="btnFind" runat="server" Width="10%" Text="Find"  onclick="btnFind_Click"  /></td>
                                                 <td style="width: 12%">
                                                     <asp:DropDownList ID="ddActive" runat="server" Width="100%" onchange="GetKeyPress();"></asp:DropDownList></td>
                                            </tr>
                                       </table>
                                         <table style="width: 100%">                                             
                                            <tr style="width: 100%">
                                                <td style="width:15%">
                                                    <asp:Label ID="lblSerialNumber" Text="Serial Number" runat="server" Font-Bold="False" Font-Size="Small"></asp:Label></td>
                                                <td style="width: 35%">
                                                   <asp:TextBox ID="txtSerialNumber" runat="server" Width="95%" BackColor="#BFDBFF" ReadOnly="true"></asp:TextBox></td>
                                                <td style="width: 15%;">
                                                    <asp:Label ID="lblLotorBatch" Text="Lot or Batch" runat="server" Font-Bold="False" Font-Size="Small"></asp:Label></td>
                                                <td style="width: 35%">
                                                    <asp:TextBox ID="txtLotorBatch" runat="server" Width="100%" BackColor="#BFDBFF" ReadOnly="true"></asp:TextBox></td>
                                            </tr>
                                            <tr style="width: 100%">
                                                <td style="width: 15%">
                                                    <asp:Label ID="lblManufacturedDate" Text="Manufactured Date" runat="server" Font-Bold="False" Font-Size="Small"></asp:Label></td>
                                                  <td style="width: 35%">
                                                    <asp:TextBox ID="txtManufacturedDate" runat="server" Width="95%" BackColor="#BFDBFF" ReadOnly="true"></asp:TextBox></td>
                                                <td style="width: 15%;">
                                                    <asp:Label ID="lblExpirationDate" Text="Expiration Date" runat="server" Font-Bold="False" Font-Size="Small"></asp:Label></td>
                                                 <td style="width: 35%">
                                                    <asp:TextBox ID="txtExpirationDate" runat="server" Width="100%" BackColor="#BFDBFF" ReadOnly="true"></asp:TextBox></td>
                                            </tr>
                                            <tr style="width: 100%">
                                                <td style="width: 15%">
                                                    <asp:Label ID="lblDistinctID" Text="Distinct ID(HCT/P)" runat="server" Font-Bold="False" Font-Size="Small"></asp:Label></td>
                                                 <td style="width: 33%">
                                                    <asp:TextBox ID="txtDistinctID" runat="server" Width="95%" BackColor="#BFDBFF" ReadOnly="true"></asp:TextBox></td>
                                                 <td style="width: 15%;">
                                                    <asp:Label ID="lblIssuingAgency" Text="Issuing Agency" runat="server" Font-Bold="False" Font-Size="Small"></asp:Label></td>
                                                  <td style="width: 35%">
                                                    <asp:TextBox ID="txtIssuingAgency" runat="server" Width="100%" BackColor="#BFDBFF" ReadOnly="true"></asp:TextBox></td>
                                            </tr>
                                            <tr style="width: 100%">
                                                 <td style="width: 15%">
                                                    <asp:Label ID="lblBrandName" Text="Brand Name" runat="server" Font-Bold="False" Font-Size="Small"></asp:Label></td>
                                                 <td style="width: 35%">
                                                    <asp:TextBox ID="txtBrandName" runat="server" Width="95%" BackColor="#BFDBFF" ReadOnly="true" Wrap="true"></asp:TextBox></td>
                                                 <td style="width: 15%;">
                                                    <asp:Label ID="lblVersionModel" Text="Version/Model" runat="server" Font-Bold="False" Font-Size="Small"></asp:Label></td>
                                                 <td style="width: 35%">
                                                    <asp:TextBox ID="txtVersionModel" runat="server" Width="100%" BackColor="#BFDBFF" ReadOnly="true"></asp:TextBox></td>
                                            </tr>
                                            <tr style="width: 100%">
                                                 <td style="width: 15%">
                                                    <asp:Label ID="lblCompanyName" Text="Company Name" runat="server" Font-Bold="False" Font-Size="Small"></asp:Label></td>
                                                  <td style="width: 35%">
                                                    <asp:TextBox ID="txtCompanyName" runat="server" Width="95%" BackColor="#BFDBFF" ReadOnly="true" Wrap="true" ></asp:TextBox></td>
                                               <td style="width: 15%;">
                                                    <asp:Label ID="lblMRISafetyStatus" Text="MRI Safety Status" runat="server" Font-Bold="False" Font-Size="Small"></asp:Label></td>
                                                  <td style="width: 35%">
                                                    <asp:TextBox ID="txtMRISafetyStatus" runat="server" Width="100%" BackColor="#BFDBFF" ReadOnly="true" Wrap="true"></asp:TextBox></td>
                                            </tr>
                                            <tr style="width: 100%">
                                                
                                                 <td style="width: 15%">
                                                    <asp:Label ID="lblGMDNPTName" Text="GMDN PT Name" runat="server" Font-Bold="False" Font-Size="Small"></asp:Label></td>
                                                   <td style="width: 35%">
                                                    <asp:TextBox ID="txtGMDNPTName" runat="server" Width="95%" BackColor="#BFDBFF" ReadOnly="true" Wrap="true"></asp:TextBox></td>
                                                 <td style="width: 15%;">
                                                    <asp:Label ID="lblrubberContent" Text="Rubber Content" runat="server" Font-Bold="False" Font-Size="Small"></asp:Label></td>
                                                <td style="width: 35%">
                                                    <asp:TextBox ID="txtRubberContent" runat="server" Width="100%" BackColor="#BFDBFF" ReadOnly="true" Wrap="true" ></asp:TextBox>
                                                    </td>
                                            </tr>
                                            <tr style="width: 100%">
                                                 <td style="width: 15%">
                                                    <asp:Label ID="lblDescription" Text="Description" runat="server" Font-Bold="False" Font-Size="Small"></asp:Label></td>
                                                  <td style="width: 55%" colspan="3">
                                                    <asp:TextBox ID="txtDescription" runat="server" Width="100%" BackColor="#BFDBFF" ReadOnly="true" Wrap="true"></asp:TextBox></td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>--%>
                            <tr style="width: 100%; height: 20%">
                                <td style="width: 100%">
                                    <table style="width: 98%">
                                        <tr style="width: 100%">
                                            <td style="width: 95%"></td>                                            
                                            <td style="width: 2%;text-align:right;">
                                                <%--CAP-3474: Removing the fixed width attribute to adjust the button to auto for better text visibility--%>
                                                <telerik:RadButton ID="btnAdd" runat="server" ToolTip="Add" ButtonType="LinkButton" CssClass="greenbutton"
                                                    OnClientClicked="btnAdd_Clicked" OnClick="btnAdd_Click"
                                                    Style="text-align: center; -moz-border-radius: 3px; -webkit-border-radius: 3px; width: auto !important;">

                                                    <ContentTemplate>
                                                        <span id="SpanAdd" runat="server"  class="underline">A</span><span id="SpanAdditionalword" runat="server">dd</span>
                                                    </ContentTemplate>
                                                </telerik:RadButton>
                                            </td>
                                            <td style="width: 3%">
                                                <telerik:RadButton ID="btnClearAll" runat="server" Text="Clear All" AutoPostBack="false" ButtonType="LinkButton" CssClass="redbutton"
                                                    OnClientClicked="btnClearAll_Clicked"
                                                    AccessKey="C" Style="text-align: center; position: static; left: 144px; -moz-border-radius: 3px; -webkit-border-radius: 3px;"
                                                    Width="76px">
                                                    <ContentTemplate>
                                                        <span id="SpanClear" runat="server" class="underline">C</span><span id="SpanClearAdditional" runat="server">lear All</span>
                                                    </ContentTemplate>
                                                </telerik:RadButton>
                                            </td>
                                            <td style="width: 5%">
                                                <input type="button" id="btnImplantable" class="aspbluebutton" runat="server" value="Manage Implantable Device" style="margin-right: 23px;" onclick="ImplantableScreenOpen('false');"/>
                                            </td>
                                        </tr>
                                        <tr style="width: 100%">
                                            <td style="width: 70%"></td>
                                            <td style="width: 10%;">
                                                </td>
                                             <td style="width: 10%;">
                                                </td>
                                            <td style="width: 10%;text-align:right;">
                                              <asp:CheckBox ID="chkActive" runat="server" OnCheckedChanged="chkActive_CheckedChanged" AutoPostBack="true"/>
                                                  <asp:Label ID="lblShowInactiveDevice" Text="Show All" runat="server" Font-Bold="False" Font-Size="Small" style="margin-right: 30px;"></asp:Label></td>
                                        </tr>

                                    </table>
                                </td>
                            </tr>
                        </table>

                    </td>
                </tr>
                <tr style="height: 40%">
                    <td class="style1" colspan="2">
                        <asp:Panel ID="pnlGridProcedure" runat="server" GroupingText="Procedure" Height="290px"
                            Width="96%" Font-Bold="True">
                            <telerik:RadGrid ID="grdOtherProcedure" runat="server" Height="200px" Style="margin-bottom: 0px"
                                AutoGenerateColumns="False" CellSpacing="0" GridLines="Both" EnableEmbeddedSkins="false" 
                                Font-Bold="false" OnItemCommand="grdProcedure_ItemCommand" Width="99%" OnItemDataBound="grdProcedure_ItemDataBound"  CssClass="Gridbodystyle">
                                <FilterMenu EnableImageSprites="False">
                                </FilterMenu>                             
                                <HeaderStyle Font-Bold="true" CssClass="Gridheaderstyle"  />
                                <ClientSettings>
                                    <Selecting AllowRowSelect="True" />
                                    <Scrolling AllowScroll="True" UseStaticHeaders="True" />
                                    <ClientEvents OnCommand="OnItemCommand" />
                                </ClientSettings>

                                <MasterTableView>
                                    <CommandItemSettings ExportToPdfText="Export to PDF" />
                                    <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                                    </RowIndicatorColumn>
                                    <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                                    </ExpandCollapseColumn>
                                    <Columns>
                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="PEdit" FilterControlAltText="Filter Edit column"
                                            HeaderText="Edit" ImageUrl="~/Resources/edit.gif" UniqueName="Edit" Text="Edit">
                                            <HeaderStyle Width="50px" />
                                        </telerik:GridButtonColumn>
                                        <telerik:GridButtonColumn ButtonType="ImageButton" Text="Delete" CommandName="PDelete"
                                            FilterControlAltText="Filter Delete column" HeaderText="Del" ImageUrl="~/Resources/close_small_pressed.png"
                                            UniqueName="Delete">
                                            <HeaderStyle Width="50px" />
                                        </telerik:GridButtonColumn>
                                        <telerik:GridBoundColumn DataField="Procedure" ItemStyle-CssClass="Editabletxtbox" FilterControlAltText="Filter Procedure column"
                                            HeaderText="Procedure" UniqueName="Procedure">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Notes" FilterControlAltText="Filter Notes column" ItemStyle-CssClass="Editabletxtbox"
                                            HeaderText="Notes" UniqueName="Notes" EmptyDataText="">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="View" ItemStyle-CssClass="Editabletxtbox" FilterControlAltText="Filter View column"
                                            HeaderText="View" ImageUrl="~/Resources/Down.bmp" UniqueName="View" Visible="false">
                                            <HeaderStyle Width="50px" />
                                        </telerik:GridButtonColumn>
                                        <telerik:GridBoundColumn DataField="GroupKey" ItemStyle-CssClass="Editabletxtbox" FilterControlAltText="Filter column column"
                                            UniqueName="GroupKey" EmptyDataText="" Display="False">
                                        </telerik:GridBoundColumn>
                                         <telerik:GridBoundColumn DataField="UniqueDeviceIdentifier" ItemStyle-CssClass="Editabletxtbox" FilterControlAltText="Unique Device Identifier"
                                            HeaderText="Unique Device Identifier" UniqueName="UniqueDeviceIdentifier">   </telerik:GridBoundColumn>
                                              <telerik:GridBoundColumn DataField="Status" ItemStyle-CssClass="Editabletxtbox" FilterControlAltText="Status"
                                            HeaderText="Status" UniqueName="Status">    <HeaderStyle Width="60px" /> </telerik:GridBoundColumn>
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
            </table>


            <%--Commented by Ponmozhi Vendan T   --%>
            <%-- <telerik:RadButton ID="btnPlan" runat="server" Text="Plan"  ToolTip="Plan" AutoPostBack="False"  
                                        onclientclicked="btnPlan_Clicked" AccessKey="P" Style="text-align:center; top: 0px; left: 54px; -moz-border-radius: 3px; -webkit-border-radius: 3px;
                                            position: relative;" Width="40px">
                                            <ContentTemplate>
                                                <span class="underline">P</span>lan
                                            </ContentTemplate>
                                        </telerik:RadButton>
                                        <telerik:RadButton ID="btnTestArea" runat="server" Text="Test Area" ToolTip="Test Area"
                                            OnClick="btnTestArea_Click" Enabled="False" AccessKey="T" 
                                            Style="text-align:center; position: static;left:54px; -moz-border-radius: 3px; -webkit-border-radius: 3px;
                                                position: relative;">
                                                <ContentTemplate>
                                                    <span class="underline">T</span>est Area
                                                </ContentTemplate>
                                        </telerik:RadButton>--%>
            <input id="Hidden1" type="hidden" runat="server" />
            <asp:HiddenField ID="hdnDelImmuniztionId" runat="server" />
             <asp:HiddenField ID="hdnBtnFind" runat="server" />
            <asp:HiddenField ID="hdnSelectedIndex" runat="server" />
            <asp:HiddenField ID="hdnTransferVaraible" runat="server" />
            <asp:Button ID="InvisibleButton" runat="server" CssClass="displayNone" OnClick="InvisibleButton_Click" />
             <asp:Button ID="btnInvisibleImplant" runat="server" CssClass="displayNone" OnClick="btnInvisibleImplant_Click" />
             <asp:HiddenField ID="hdngroupkey" runat="server" />
            <asp:Button ID="btnClear" runat="server" CssClass="displayNone" OnClick="btnClearAll_Click" />
            <asp:Button ID="btnRefreshProce" runat="server" Text="RefreshProce" CssClass="displayNone"
                OnClick="btnRefreshProce_Click" />
            <%--<div id="divLoading" class="modal" runat="server" style="text-align: center; display: none">
            <asp:Panel ID="Panel3" runat="server">
                <br />
                <br />
                <br />
                <br />
                <center>
                    <asp:Label ID="Label8" Text="" runat="server"></asp:Label></center>
                <br />
                <img src="Resources/wait.ico" title="[Please wait while the page is loading...]"
                    alt="Loading..." />
                <br />
            </asp:Panel>
        </div>--%>
            <asp:HiddenField ID="hdnLocalTime" runat="server" />
            <asp:HiddenField ID="div_position" runat="server" />
              <asp:HiddenField ID="hProcedure" runat="server" />
              <asp:HiddenField ID="hNotes" runat="server" />
             <asp:HiddenField ID="hEncID" runat="server" />
            <asp:HiddenField ID="IsSaveOrUpdate" runat="server" />
            <asp:HiddenField ID="hdnManageFreqProcedureCount" runat="server" />
        </telerik:RadAjaxPanel>
        <%--
    <script src="JScripts/jquery-1.7.1.min.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>--%>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <script src="JScripts/JSInhouseProcedure.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSCustomPhrases.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSCustomDLC.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <%--<script src="JScripts/JSTestArea.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>--%>
            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
        </asp:PlaceHolder>
    </form>
</body>
</html>
