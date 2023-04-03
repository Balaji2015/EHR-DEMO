<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="frmExamPhotos.aspx.cs"
    Inherits="Acurus.Capella.UI.frmExamPhotos" Async="true"   ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Upload Photos</title>
  
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>

    <style type="text/css">
        .style3,.style4{height:20px}.style3,.style5,.style7{width:254px}.style5,.style6{height:32px}.style7,.style9{height:84px}.style10{height:32px;width:398px}.style11{height:20px;width:398px}.style17,.style19,.style20{height:22px}.RadPicker{vertical-align:middle}.RadPicker .rcTable{table-layout:auto}.RadPicker .RadInput{vertical-align:baseline}.RadInput_Default{font:12px "segoe ui",arial,sans-serif}.riSingle{box-sizing:border-box;-moz-box-sizing:border-box;-ms-box-sizing:border-box;-webkit-box-sizing:border-box;-khtml-box-sizing:border-box;position:relative;display:inline-block;white-space:nowrap;text-align:left}.RadInput{vertical-align:middle}.riSingle .riTextBox{box-sizing:border-box;-moz-box-sizing:border-box;-ms-box-sizing:border-box;-webkit-box-sizing:border-box;-khtml-box-sizing:border-box}.RadPicker_Default .rcCalPopup{background-position:0 0;background-image:url('mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Calendar.sprite.gif')}.RadPicker .rcCalPopup{display:block;overflow:hidden;width:22px;height:22px;background-color:transparent;background-repeat:no-repeat;text-indent:-2222px;text-align:center}div.RadPicker table.rcSingle .rcInputCell{padding-right:0}.RadPicker table.rcTable .rcInputCell{padding:0 4px 0 0}.style19{width:98px}.style20{width:89px}.displayNone{display:none}.modal{position:fixed;top:0;left:0;background-color:#fff;z-index:99;opacity:.8;filter:alpha(opacity=80);-moz-opacity:.8;min-height:100%;width:100%}.underline{text-decoration:underline}
#cboPhysicianName {
            font-size: 12px !important;
        }
    </style>
     <link href="~/CSS/CommonStyle.css" rel="Stylesheet" type="text/css" />
  </head>
<body onload="ExamPhotos_Load()">
    <form id="frmExamPhotos" runat="server" style="background-color: White;">
    <telerik:RadWindowManager ID="WindowMngr" runat="server">
        <Windows>
            <telerik:RadWindow ID="RadViewer" runat="server" Behaviors="Close" Title="Image Viewer"
                VisibleStatusbar="false" IconUrl="Resources/16_16.ico" VisibleOnPageLoad="false" >
            </telerik:RadWindow>
            <telerik:RadWindow ID="RadAddWindow" runat="server" Behaviors="Close" Title="Add Or Update KeyWords"
                VisibleStatusbar="false" OnClientClose="ReloadOnClientClose" IconUrl="Resources/16_16.ico">
            </telerik:RadWindow>
            <telerik:RadWindow ID="RadResultWindow" runat="server" Behaviors="Close" Title="Result Viewer"
                VisibleStatusbar="false" IconUrl="Resources/16_16.ico">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server" EnableViewState="false">
    
        <Scripts>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js">
            </asp:ScriptReference>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js">
            </asp:ScriptReference>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js">
            </asp:ScriptReference>
        </Scripts>
    </telerik:RadScriptManager>
    <asp:Button ID="btnUpdateKeywords" runat="server" Text="Button" CssClass="displayNone"
        OnClick="Button1_Click" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <Triggers>
    <asp:PostBackTrigger ControlID="btnClearAll" />
    </Triggers>
        <ContentTemplate>
                        <asp:Panel ID="Panel1" runat="server" Font-Size="Small" GroupingText="Upload Photo(s)"
                            Height="265px" Width="100%" ScrollBars="Auto" CssClass="Editabletxtbox">
                            <table style="width: 100%; height: 230px;">
                                <tr>
                                    <td >
                                       <%-- <asp:Label ID="lblSelectFile" runat="server"  Text="Select File(s) to Upload *" mand="Yes"></asp:Label>--%>

                                        <span class="MandLabelstyle">Select File(s) to Upload </span><span class="manredforstar">*</span>
                                    </td>
                                    <td class="style9" colspan="4">
                                        <telerik:RadAsyncUpload ID="UploadImage" runat="server" 
                                            MultipleFileSelection="Automatic" 
                                            onclientfileuploading="UploadImage_FileUploading" PostbackTriggers="btnSave,btnClearAll"
                                            onclientfileuploadremoved="UploadImage_FileUploadRemoved" 
                                            InitialFileInputsCount="0"   CssClass="bluebutton">
                                            <Localization Select="Browse"/>
                                        </telerik:RadAsyncUpload>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style5">
                                       <%-- <asp:Label ID="lblSelectPhysician" runat="server"  Text="Select Physician *" mand="Yes" ></asp:Label>--%>
                                        <span class="MandLabelstyle">Select Physician</span><span class="manredforstar">*</span>
                                    </td>
                                    <td class="style10">
                                      <asp:DropDownList ID="cboPhysicianName" runat="server" Width="290px" Style="margin-left: 1px;" onchange="getDropdownListSelectedText();"  Enabled="false" CssClass="Editabletxtbox">
                                    </asp:DropDownList>
                                    <asp:CheckBox ID="chkShowAllPhysicians" runat="server" Text="Show all Physicians"
                                        AutoPostBack="false" onchange="chkShowAllPhysicians_CheckedChanged(this);" CssClass="noWrapText Editabletxtbox" Style="margin-left: -2px;" Enabled="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style5">
                                     <%--   <asp:Label ID="lblGroupType" runat="server" Text="Exam Group *" mand="Yes"></asp:Label>--%>

                                          <span id="lblGroupType1" class="MandLabelstyle" runat="server">Exam Group</span><span  id="lblGroupType2" class="manredforstar" runat="server">*</span>

                                    </td>
                                    <td class="style10">
                                        <telerik:RadComboBox ID="cboGroupType" runat="server" Height="72px" MaxHeight="100px" CssClass="Editabletxtbox"
                                            Width="420px" OnClientDropDownClosed="cboGroupType_DropDownClosed" AutoPostBack="false">
                                        </telerik:RadComboBox>
                                    </td>
                                    <td class="style6" colspan="3">
                                        <asp:ImageButton ID="pbLibrary" runat="server" Height="16px" ImageUrl="~/Resources/Database Inactive.jpg" OnClientClick="openAddorUpdate();" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style3">
                                      <%--  <asp:Label ID="lblTestTakenOn" runat="server"  Text="Date of Exam *"  mand="Yes"></asp:Label>--%>

                                          <span id="lblTestTakenOn1" class="MandLabelstyle" runat="server">Date of Exam</span><span class="manredforstar"  id="lblTestTakenOn2" runat="server">*</span>

                                    </td>
                                    <td class="style11">
                                        <telerik:RadDatePicker ID="dtpTestTakenDate" runat="server" Height="26px" Width="421px" ClientEvents-OnPopupClosing="" CssClass="Editabletxtbox">
                                            <Calendar UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False" ViewSelectorText="x">
                                            </Calendar>
                                            <DatePopupButton HoverImageUrl="" ImageUrl="" />
                                            <DateInput DateFormat="dd-MM-yyyy" DisplayDateFormat="dd-MMM-yyyy" DisplayText=""
                                                Height="26px" LabelWidth="40%" type="text" value="">
                                                <EmptyMessageStyle Resize="None" />
                                                <ReadOnlyStyle Resize="None" />
                                                <FocusedStyle Resize="None" />
                                                <DisabledStyle Resize="None" />
                                                <InvalidStyle Resize="None" />
                                                <HoveredStyle Resize="None" />
                                                <EnabledStyle Resize="None" />
                                            </DateInput>
                                            <ClientEvents OnPopupClosing="dtpTestTakenDate_OnPopupClosing" />
                                        </telerik:RadDatePicker>
                                    </td>
                                    <td class="style4" colspan="3">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" class="style17" colspan="2">
                                    <telerik:RadButton ID="btnMoveToNextProcess" runat="server" Enabled="false" Text="Move To Next Process"
                                            Width="30%" OnClick="btnMoveToNextProcess_Click" AccessKey="m" Style="-moz-border-radius: 3px;
                                         -webkit-border-radius: 3px;">
                                           <ContentTemplate>                                                 
                                           <span class="underline">M</span>ove To Next Process                                         
                                           </ContentTemplate> 
                                        </telerik:RadButton>
                                    </td>
                                    <td class="style19" align="center">
                                        <telerik:RadButton ID="btnMove" runat="server" Enabled="false" Text="View Result" AutoPostBack="false"
                                            Width="92%" OnClientClicked="btnView_Clicked" AccessKey="v" Style="-moz-border-radius: 3px;
                                         -webkit-border-radius: 3px;">
                                       <ContentTemplate>                                                 
                                           <span class="underline">V</span>iew Result                                         
                                           </ContentTemplate>
                                        </telerik:RadButton>
                                    </td>
                                    <td class="style19" align="center">
                                        <telerik:RadButton ID="btnSave" runat="server" OnClientClicked="btnSave_Clicked"
                                            Text="Upload" Width="78%" onclick="btnSave_Click" AccessKey="u" Style="-moz-border-radius: 3px;
                                         -webkit-border-radius: 3px;" CssClass="greenbutton" ButtonType="LinkButton">
                                             <ContentTemplate>                                                 
                                           <span>Upload</span>                                        
                                           </ContentTemplate>
                                        </telerik:RadButton>
                                    </td>
                                   
                                    <td class="style20" align="center">
                                        <telerik:RadButton ID="btnClear" runat="server" AutoPostBack="false"  
                                            Text="Clear" Width="100%" AccessKey="c" Style="-moz-border-radius: 3px; margin-left: -28px !important;
                                         -webkit-border-radius: 3px; width:68px" OnClientClicked="btnClear_Clicked" CssClass="redbutton" ButtonType="LinkButton">
                                            <ContentTemplate>                                                 
                                                 <span>Clear</span>                                 
                                           </ContentTemplate> 
                                        </telerik:RadButton>
                                    </td>
                                </tr>
                            </table>
                            </asp:Panel>
                         <asp:Button ID="btnClearAll" runat="server"  Style="display: none;" OnClick="btnClearAll_Click" />
        </ContentTemplate>
    </asp:UpdatePanel>
                    <br />
                    <asp:UpdatePanel ID="grd" runat="server">
                    <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSave" />
                    <asp:AsyncPostBackTrigger ControlID="grdPhoto" />
                    </Triggers>
                    <ContentTemplate>
                        <asp:Panel ID="Panel2" runat="server"  GroupingText="Photos List(s)"
                            Height="280px" Width="100%" CssClass="Editabletxtbox">
                            <telerik:RadGrid ID="grdPhoto" runat="server" AutoGenerateColumns="False" CellSpacing="0"
                                GridLines="Both" Height="280px" OnItemCommand="grdExamPhotos_ItemCommand" 
                                OnItemCreated="grdExamPhotos_ItemCreated"  CssClass="Gridbodystyle" OnSortCommand="grdhuman_SortCommand1"  ClientSettings-ClientEvents-OnCommand="OnCommand"  AllowSorting="true" MasterTableView-AllowNaturalSort="False">
                                <FilterMenu EnableImageSprites="False">
                                </FilterMenu>
                                <ClientSettings EnablePostBackOnRowClick="False">
                                    <Selecting AllowRowSelect="True" />
                                    <Scrolling AllowScroll="True" ScrollHeight="100px" UseStaticHeaders="True" />
                                </ClientSettings>
                                <HeaderStyle CssClass="Gridheaderstyle" />
                                <MasterTableView>
                                    <CommandItemSettings ExportToPdfText="Export to PDF" />
                                    <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                                    </RowIndicatorColumn>
                                    <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                                    </ExpandCollapseColumn>
                                    <Columns>
                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="DeleteRow" FilterControlAltText="Filter column column"
                                            HeaderText="Del" ImageUrl="~/Resources/close_small_pressed.png" UniqueName="column"  ConfirmDialogType="RadWindow" ConfirmTitle="Capella - Confirmation" ConfirmDialogWidth="300px" ConfirmDialogHeight="120px"  ConfirmText="Are you sure want to delete the file?">
                                        <ItemStyle  CssClass="Editabletxtbox" BorderStyle="Dotted" />
                                        </telerik:GridButtonColumn>
                                           <telerik:GridBoundColumn DataField="Test Taken Date" FilterControlAltText="Filter column3 column"
                                            HeaderText="Date of Exam" UniqueName="column3" SortExpression="Test Taken Date" DataType="System.DateTime"  DataFormatString="{0:dd-MMM-yyyy}">
                                            <HeaderStyle CssClass="Gridheaderstyle" />
                                            <ItemStyle CssClass="Editabletxtbox" BorderStyle="Dotted" />
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="Group Type" FilterControlAltText="Filter column1 column"
                                            HeaderText="Group Type" UniqueName="column1" SortExpression="Group Type">
                                            <HeaderStyle CssClass="Gridheaderstyle" />
                                        <ItemStyle CssClass="Editabletxtbox" BorderStyle="Dotted" />
                                        </telerik:GridBoundColumn>
                                         <telerik:GridBoundColumn DataField="Physician name" FilterControlAltText="Filter column3 column"
                                            HeaderText="Physician Name" UniqueName="column3" SortExpression="Physician name">
                                             <HeaderStyle CssClass="Gridheaderstyle" />
                                            <ItemStyle CssClass="Editabletxtbox" BorderStyle="Dotted" Wrap="true" Width="200px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="No of Images" FilterControlAltText="Filter column2 column"
                                            HeaderText="No of Images" UniqueName="column6" SortExpression="No of Images">
                                            <HeaderStyle CssClass="Gridheaderstyle" />
                                            <ItemStyle CssClass="Editabletxtbox" BorderStyle="Dotted" />
                                        </telerik:GridBoundColumn>
                                     

                                      
                                        <telerik:GridBoundColumn DataField="File Name" FilterControlAltText="Filter column3 column"
                                            HeaderText="FileName" UniqueName="column5" Visible="false">
                                            <HeaderStyle CssClass="Gridheaderstyle" />
                                            <ItemStyle CssClass="Editabletxtbox" BorderStyle="Dotted" />
                                        </telerik:GridBoundColumn>

                                      <telerik:GridBoundColumn DataField="PhyID" FilterControlAltText="Filter column3 column"
                                            HeaderText="PhyID" UniqueName="column7" Display="false">
                                            <HeaderStyle CssClass="Gridheaderstyle" />
                                            <ItemStyle CssClass="Editabletxtbox" BorderStyle="Dotted"  />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="FileID" FilterControlAltText="Filter column3 column"
                                            HeaderText="File ID" UniqueName="column8" Display="false">
                                            <HeaderStyle CssClass="Gridheaderstyle" />
                                            <ItemStyle CssClass="Editabletxtbox" BorderStyle="Dotted"  />
                                        </telerik:GridBoundColumn>
                                        
                                         <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="ViewRow" 
                        FilterControlAltText="Filter column4 column" HeaderText="View"  
                        ImageUrl="~/Resources/Down.bmp" UniqueName="column4"></telerik:GridButtonColumn>
                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="CompareRow" 
                        FilterControlAltText="Filter column4 column" HeaderText="Compare" 
                        ImageUrl="~/Resources/Down.bmp" UniqueName="column5"></telerik:GridButtonColumn>

                                    </Columns>
                                    <EditFormSettings>
                                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                        </EditColumn>
                                    </EditFormSettings>
                                </MasterTableView>
                            </telerik:RadGrid></asp:Panel>
               
                <div id="divWaitLoading" class="modal" runat="server" style="text-align: center; display: none">
                    <asp:Panel ID="Panel3" runat="server">
                        <br />
                        <br />
                        <br />
                        <br />
                        <center>
                            <asp:Label ID="Label1" Text="" runat="server" EnableViewState="false"></asp:Label></center>
                        <br />
                        <img src="Resources/loadimage.gif" height="30px" width="30px" title="[Please wait while the page is loading...]"
                            alt="Loading..." />
                        <br />
                    </asp:Panel>
                </div>
                </ContentTemplate>
                </asp:UpdatePanel>
<asp:HiddenField ID="hdnFormType" runat="server" />
    <asp:HiddenField ID="hdnHumanId" runat="server" />
    <asp:HiddenField ID="hdnDocumentType" runat="server" />
    <asp:HiddenField ID="hdnDate" runat="server" />
    <asp:HiddenField ID="hdnOrderSubmitId" runat="server" />
     <asp:HiddenField ID="hdnMoveToMAID" runat="server" />
    <asp:HiddenField ID="hdnCurrentProcess" runat="server" />
    <asp:HiddenField ID="hdnOrder" runat="server" />
 <asp:HiddenField ID="hdnfileid" runat="server" />
        <asp:HiddenField ID="hdnLibraryCheck" runat="server" />


    <asp:Button ID="btnLibrary" runat="server" Style="display: none;" OnClick="btnLibrary_Click" />
    <asp:Button ID="Invisiblebtn" runat="server"  Style="display: none;" OnClick="Invisiblebtn_Click" />
    <asp:Button ID="btnDisableOnload" runat="server"  Style="display: none;" OnClick="btnDisableOnload_Click" />
        <asp:HiddenField ID="hdnindex" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnLocalPhy" runat="server" EnableViewState="true" />
    <asp:HiddenField ID="hdndocphysician" runat="server" EnableViewState="true" />
    <asp:Button ID="LibraryButton" runat="server" Style="display: none;" OnClick="LibraryButton_Click" />
    <asp:PlaceHolder ID="PlaceHolder1" runat="server">
    <script src="JScripts/jquery-2.1.3.js" type="text/javascript" ></script>
        <script src="JScripts/bootstrap.min.js" type="text/javascript"></script>   
    <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSExamPhotos.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
   </asp:PlaceHolder>
    </form>
</body>
</html>
    