<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmMedicationManager.aspx.cs"
    Inherits="Acurus.Capella.UI.frmMedicationManager" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
      <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <title>Medication Manager</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
        #form1
        {
            width: 1114px;
        }
        .style1
        {
            height: 547px;
        }
        .style4
        {
            height: 461px;
        }
        .style5
        {
            height: 222px;
        }
        .style6
        {
            height: 93px;
        }
        .style8
        {
            height: 547px;
            width: 571px;
        }
        .RadGrid_Default
        {
            font: 12px/16px "segoe ui" ,arial,sans-serif;
        }
        .RadGrid_Default
        {
            border: 1px solid #828282;
            background: #fff;
            color: #333;
        }
        .RadGrid_Default .rgMasterTable
        {
            font: 12px/16px "segoe ui" ,arial,sans-serif;
        }
        .RadGrid .rgMasterTable
        {
            border-collapse: separate;
            border-spacing: 0;
        }
        .RadGrid_Default .rgHeader
        {
            color: #333;
        }
        .RadGrid_Default .rgHeader
        {
            border: 0;
            border-bottom: 1px solid #828282;
            background: #eaeaea 0 -2300px repeat-x url(            'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Grid.sprite.gif' );
        }
        .RadGrid .rgHeader
        {
            padding-top: 5px;
            padding-bottom: 4px;
            text-align: left;
            font-weight: normal;
        }
        .RadGrid .rgHeader
        {
            padding-left: 7px;
            padding-right: 7px;
        }
        .RadGrid .rgHeader
        {
            cursor: default;
        }
        .style18
        {
            height: 196px;
        }
        .style20
        {
            width: 106px;
        }
        .style21
        {
            width: 320px;
        }
        .style22
        {
            width: 338px;
        }
        .style23
        {
            width: 87px;
        }
        .style24
        {
            width: 65px;
        }
        .style25
        {
            width: 98px;
        }
        .style26
        {
            width: 284px;
        }
        .style27
        {
            width: 55px;
        }
        .style28
        {
            width: 420px;
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
          .display
        {
            display:none; 
        }  
        .style29
        {
            width: 156px;
        }
    </style>    

   
    <script type="text/javascript" id="telerikClientEvents1">
//<![CDATA[

	
//]]>
</script>    

   
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%" Width="100%">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <div style="height: 577px; width: 100%">
            <asp:Panel ID="pnlMain" runat="server" Height="100%" Width="100%">
                <table style="width: 100%; height: 100%;">
                    <tr>
                        <td class="style8">
                            <asp:Panel ID="pnlMedication" runat="server" Height="100%" Width="100%" Style="margin-right: 0px">
                                <table id="tblMedication" style="width: 100%; height: 100%;" runat="server">
                                    <tr id="tblMedicationRow1">
                                        <td id="tblMedicationData1">
                                            <asp:Panel ID="pnlTextboxLabel" runat="server" Height="100%" Width="100%">
                                                <table style="width: 100%; height: 100%;" >
                                                    <tr>
                                                        <td class="style25">
                                                            <asp:Label ID="lblMedicationName" runat="server" Font-Size="Small" Text="Medication Name" CssClass="Editabletxtbox"></asp:Label>
                                                        </td>
                                                        <td bgcolor="White">
                                                            <telerik:RadTextBox ID="txtMedicationName" runat="server" Height="100%" Width="100%"
                                                                BackColor="White" CssClas="nonEditabletxtbox">
                                                            </telerik:RadTextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr id="tblMedicationRow2">
                                        <td id="tblMedicationData2">
                                            <asp:Panel ID="pnlClear" runat="server" Height="100%" Width="100%">
                                                <table style="width: 100%; height: 100%;">
                                                    <tr>
                                                        <td class="style26">
                                                            <asp:Label ID="lblResult" runat="server" Font-Size="Small" Font-Bold="True" CssClass="Editabletxtbox"></asp:Label>
                                                        </td>
                                                        <td class="style24">
                                                        </td>
                                                        <td align="right" class="style29">
                                                            <telerik:RadButton ID="btnSearchMed" runat="server" Text="Search" Width="50px" 
                                                                OnClick="btnSearch_Click" onclientclicked="btnSearchMed_Clicked" ButtonType="LinkButton" CssClass="bluebutton">
                                                            </telerik:RadButton>
                                                        </td>
                                                        <td>
                                                            <telerik:RadButton ID="btnClearMed" runat="server" Text="Clear" Width="50px" 
                                                                onclientclicked="btnClearMed_Clicked" AutoPostBack="False" ButtonType="LinkButton" CssClass="redbutton">
                                                            </telerik:RadButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style4">
                                            <asp:Panel ID="pnlGrid" runat="server" Font-Size="Small" GroupingText="Frequently Used ICDs"
                                                Height="100%" Width="100%" ScrollBars="Vertical" CssClass="Editabletxtbox">
                                                <telerik:RadGrid ID="grdMedicationList" runat="server" Height="440px" Width="100%"
                                                    AutoGenerateColumns="False" CellSpacing="0" GridLines="None" MasterTableView-ShowHeadersWhenNoRecords="False">
                                                    <FilterMenu EnableImageSprites="False">
                                                    </FilterMenu>
                                                    <ClientSettings EnablePostBackOnRowClick="True">
                                                    <Scrolling AllowScroll="True" />
                                                   <Selecting AllowRowSelect="true" />
                                                    </ClientSettings>
                                                     <SelectedItemStyle     BorderStyle="Dashed"
    BorderWidth="1px" />
                                                    <MasterTableView>
                                                        <CommandItemSettings ExportToPdfText="Export to PDF" />
                                                        <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                                                        </RowIndicatorColumn>
                                                        <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                                                        </ExpandCollapseColumn>
                                                        <Columns>
                                                            <telerik:GridTemplateColumn DataField="Select" FilterControlAltText="Filter Select column"
                                                                HeaderText="Select" UniqueName="Select">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelect_checkedChanged" />
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="Gridheaderstyle" Width="56px" />
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridBoundColumn DataField="ICD" FilterControlAltText="Filter ICD column"
                                                                HeaderText="ICD" ReadOnly="True" Resizable="False" UniqueName="ICD">
                                                                <HeaderStyle Width="65px" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="Description" FilterControlAltText="Filter Description column"
                                                                HeaderText="Description" ReadOnly="True" Resizable="False" UniqueName="Description">
                                                                <HeaderStyle CssClass="Gridheaderstyle" Width="612px" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="Tag" FilterControlAltText="Filter Tag column"
                                                                HeaderText="Tag" UniqueName="Tag" Display="false"  EmptyDataText="">
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
                                        <td>
                                            <table id="tblMedicationOkCancel" style="width: 100%; height: 100%;" runat="server">
                                                <tr>
                                                    <td class="style28">
                                                        &nbsp;
                                                    </td>
                                                    <td class="style24">
                                                        <telerik:RadButton ID="btnOkMed" runat="server"  ButtonType="LinkButton" CssClass="greenbutton" OnClick="btnOk_Click" Text="Ok" Width="50%">
                                                        </telerik:RadButton>
                                                    </td>
                                                    <td>
                                                        <telerik:RadButton ID="btnCancelMed" runat="server" Text="Cancel"  ButtonType="LinkButton" CssClass="redbutton"  Width="70%" OnClick="btnCancel_Click">
                                                        </telerik:RadButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                        <td>
                            <table id="searchTable" style="width: 100%;" runat="server">
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlSearch" runat="server" Height="100%" Width="100%" GroupingText="Search ICD"
                                            BackColor="White">
                                            <table style="width: 100%; height: 100%;">
                                                <tr>
                                                    <td class="style6">
                                                        <asp:Panel ID="pnlTextSearch" runat="server" Height="100%" Width="100%">
                                                            <table style="width: 100%; height: 100%;" bgcolor="White">
                                                                <tr>
                                                                    <td class="style20">
                                                                        <asp:Label ID="lblDescription" runat="server" Font-Size="Small" Text="Enter Description"></asp:Label>
                                                                    </td>
                                                                    <td colspan="2">
                                                                        <telerik:RadTextBox ID="txtEnterDescription" runat="server" Width="100%">
                                                                        </telerik:RadTextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="style20">
                                                                        <asp:Label ID="lblICDCode" runat="server" Font-Size="Small" Text="Enter ICD Code"></asp:Label>
                                                                    </td>
                                                                    <td colspan="2">
                                                                        <telerik:RadTextBox ID="txtICDCode" runat="server" Width="100%">
                                                                        </telerik:RadTextBox>
                                                                    </td>
                                                                    <td><telerik:RadComboBox ID="cboICD910" runat="server" Width="70px">
                                                                        </telerik:RadComboBox></td> 
                                                                </tr>
                                                                <tr>
                                                                    <td class="style20">
                                                                        <asp:Label ID="lblError" runat="server" Font-Size="Small"></asp:Label>
                                                                    </td> 
                                                                                                                                      
                                                                    <td align="right" class="style22">                                                                        
                                                                        <telerik:RadButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search"
                                                                            Width="60px" onclientclicked="btnSearch_ClientClicked">
                                                                        </telerik:RadButton>
                                                                    </td>
                                                                    <td>
                                                                        <telerik:RadButton ID="btnClear" runat="server" Text="Clear All" Width="60px" 
                                                                            onclientclicked="btnClearAll_ClientClicked">
                                                                        </telerik:RadButton>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style18">
                                                        <asp:Panel ID="pnlSearchICD" runat="server" Height="100%" Width="100%" BorderStyle="Inset"
                                                            BorderWidth="1px" ScrollBars="Both">
                                                            <asp:CheckBoxList ID="chklstSearchICD" runat="server" Font-Size="Small" Height="190px"
                                                                Width="100%" AutoPostBack="True" OnSelectedIndexChanged="chklstSearchICD_SelectedIndexChanged"
                                                                BackColor="White">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style5">
                                                        <asp:Panel ID="pnlSelectedICD" runat="server" Font-Size="Small" GroupingText="Selected ICD"
                                                            Height="100%" Width="100%">
                                                            <asp:CheckBoxList ID="chklstSelectedICD" runat="server" Height="200px" Width="100%"
                                                                AutoPostBack="True" OnSelectedIndexChanged="chklstSelectedICD_SelectedIndexChanged"
                                                                BackColor="White">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlUncheck" runat="server" Height="100%" Width="100%">
                                            <table style="width: 100%; height: 100%">
                                                <tr>
                                                    <td align="right" class="style21" valign="top">
                                                        <telerik:RadButton ID="btnUncheckAll" runat="server" Height="100%" OnClick="btnUncheckAll_Click"
                                                            Style="top: 0px; left: 0px" Text="Uncheck All">
                                                        </telerik:RadButton>
                                                    </td>
                                                    <td align="right" class="style23">
                                                        <telerik:RadButton ID="btnOk" runat="server" Height="100%" OnClick="btnOk_Click"
                                                            Text="Ok" Width="100%" >
                                                        </telerik:RadButton>
                                                    </td>
                                                    <td align="right">
                                                        <telerik:RadButton ID="btnCancel" runat="server" Height="100%" Text="Cancel" Width="100%"
                                                            OnClick="btnCancel_Click">
                                                        </telerik:RadButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
        <asp:HiddenField ID="finalProblemList" runat="server" />
        <asp:HiddenField ID="selectedMedicationList" runat="server" />
         <asp:HiddenField ID="selectedAllergyList" runat="server" />
        <asp:HiddenField ID="iProblemList" runat="server" />
        <asp:HiddenField ID="ilstproblem" runat="server" />
        <asp:HiddenField ID="hdnRule" runat="server" />
         <asp:HiddenField ID="hdnLabResult" runat="server" />
        <div id="divLoading" class="modal" runat="server" style="text-align: center; display: none">
      <asp:Panel ID="Panel4" runat="server">
             <br />
             <br />
             <br />
             <br />
             <center>                 
             <br />
             <img src="Resources/wait.ico" title="[Please wait while the page is saving...]"
                 alt="saving..." />
             <br />
         </asp:Panel>
     </div>
    </telerik:RadAjaxPanel>
<asp:PlaceHolder ID="PlaceHolder1" runat="server">

    <script src="JScripts/JSMedicationManager.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

    <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

  </asp:PlaceHolder>
    </form>
</body>
</html>
