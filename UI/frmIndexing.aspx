<%@ Page Async="true" Language="C#" AutoEventWireup="True" CodeBehind="frmIndexing.aspx.cs" Inherits="Acurus.Capella.UI.frmIndexing" EnableViewState="true" EnableEventValidation="false" %>

<%--<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Upload Documents</title>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <base target="_self" />
    <link href="CSS/ScanningAndIndexing.css" rel="stylesheet" />
    <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
     <link href="CSS/font-awesome.4.4.0.css" rel="stylesheet" />

     <style>
         .divhighlight:hover {
             font-weight: bold;
             cursor: pointer;
         }

         .panelborderboxIndexing {
             border-color: #bfdbff !important;
             margin-bottom: 20px;
             background-color: #fff;
             border: 1px solid transparent;
             border-radius: 4px !important;
         }

         .panel-headingdisable {
             color: black !important;
             background-color: #e3e3e3 !important;
             border-color: #bfdbff !important;
             font-family: "Helvetica Neue",Helvetica,Arial,sans-serif !important;
             border-radius: 8px !important;
         }

         .panel-headingIndexing {
             color: black !important;
             background-color: #bfdbff !important;
             border-color: #bfdbff !important;
             font-family: "Helvetica Neue",Helvetica,Arial,sans-serif !important;
             border-radius: 8px !important;
         }

         #CheckAlert {
             display: none;
             background: #fdfeff;
             box-shadow: 0 0 10px rgba(0,0,0,0.4);
             box-sizing: border-box;
             color: #101010;
             left: 50%;
             min-width: 645px;
             max-width: 700px;
             padding: 1.875em;
             position: absolute;
             top: 7%;
             transform: translate(-50%, -50%);
             z-index: 2000000000;
             border-radius: 10px;
             opacity: 0.8;
         }
     </style>
</head>

<body style="margin-top: -25px; font-size: 14px!important;" onloadstart="pageLoad();">
    <form id="frmIndexing" runat="server">
        <%-- <telerik:RadStyleSheetManager ID="SSTMngr" runat="server" EnableStyleSheetCombine="true" OutputCompression="Forced"></telerik:RadStyleSheetManager>
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" EnableScriptCombine="true">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
            </Scripts>
        </telerik:RadScriptManager>--%>
        <asp:ToolkitScriptManager ID="ToolkitScriptManager2" runat="server" EnableViewState="false">
        </asp:ToolkitScriptManager>
        <%--<div id="dMyscanMessage"  style="display:none;height: 647px; margin-top: 130px;margin-left: 310px;width: 1145px;font-size: 20px !important;align-content: center;">
            <i class="fa fa-check-circle" style="color:green;font-size:165px;margin-left:155px"></i><br />
                 <div style="margin-left: 115px;color:green;"> File uploaded Sucessfully.</div><br />
            <div style="margin-left: 68px;"> Please close the screen to continue.</div>
        </div>--%>
        <div  id="dOverall"  style="height: 647px; width: 1145px; font-size: 14px!important;">
            <div class="blended_grid">
                <div class="pageLeftMenu" style="margin-left: 3px; margin-top: 9px;">
                    <fieldset class="scheduler-border" id="SelectDir" runat="server">
                        <legend class="scheduler-border LabelStyleBold">Select Files (File formats supported:*.tif,*.png,*.jpeg,*.pdf,*.bmp,*.jpg)</legend>
                        <div class="panel-group" id="accordion" style="margin-top: -10px;">
                            <div class="panel" style="margin-left: 3px; margin-right: 3px;">
                                <div id="dRemoteDir" style="padding-top: 2px; height: 26px; cursor: pointer;" class="panel-headingIndexing LabelStyle">
                                    &nbsp; 
                                    <input type="radio" name="Directory" id="rdbRemoteDir" runat="server" onchange="ToggleButton('Bulk Scanning and Fax','true');" class="Editabletxtbox" data-toggle="collapse" data-parent="#accordion" data-target="#dRemoteDirCollapse" />
                                    Remote Directory
                                </div>
                                <div id="dRemoteDirCollapse" runat="server" class="panelborderboxIndexing panel-collapse collapse">
                                    <table>
                                        <tr style="height: 5px;"></tr>
                                        <tr>
                                            <td>
                                                <span class="MandLabelstyle">Scanned/Fax Received Date</span><span class="manredforstar">*</span>
                                                <input type="text" id="dtpScannedDate" runat="server" readonly="readonly" style="width: 90px;" onchange="ChangeDocdate();" />
                                                <span class="MandLabelstyle" style="margin-left: 11px;">Source/Type</span><span class="manredforstar">*</span>
                                                <asp:DropDownList ID="ddlSourceType" runat="server" AutoPostBack="true" Width="65px" OnSelectedIndexChanged="ddlSourceType_SelectedIndexChanged"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr style="height: 5px;"></tr>
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <span class="MandLabelstyle">Facility</span><span class="manredforstar">*</span>&nbsp;
                                                        <asp:DropDownList ID="ddSelectedFacility" AutoPostBack="false" runat="server" Style="width: 373px;"></asp:DropDownList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td>
                                                <input type="button" id="btnFindDocuments" runat="server" class="aspresizedbluebutton" value="Find Documents" onserverclick="btnFindDocuments_Click" onclick="findDocuments();" />
                                            </td>
                                        </tr>
                                        <tr style="height: 5px;"></tr>
                                    </table>
                                </div>
                            </div>
                            <div class="panel" style="margin-left: 3px; margin-right: 3px;">
                                <div id="dLocalDir" style="padding-top: 2px; height: 26px; cursor: pointer;" class="panel-headingIndexing LabelStyle">
                                    &nbsp; 
                                    <input type="radio" name="Directory" id="rdbLocalDir" runat="server" onchange="ToggleButton('Bulk Scanning and Fax','true');" class="Editabletxtbox" data-toggle="collapse" data-parent="#accordion" data-target="#dLocalDirCollapse" />
                                    Local Directory
                                </div>
                                <div id="dLocalDirCollapse" runat="server" class="panelborderboxIndexing panel-collapse collapse">
                                    <table>
                                        <tr style="height: 5px;"></tr>
                                        <tr>
                                            <td>
                                                <span class="MandLabelstyle">Select Files</span><span class="manredforstar">*</span></td>
                                            <td style="width: 5px;"></td>
                                            <td>
                                                <asp:FileUpload ID="fileupload" runat="server" onchange="clickUpload();" accept="image/tif,image/jpeg,image/png,image/jpg,image/bmp,application/pdf" multiple="multiple" />
                                                <input type="button" id="btnUpload" style="display: none;" runat="server" class="btn btn-primary btn-sm" value="Upload" onserverclick="btnUpload_ServerClick" onclick="StartLoadOnUploadFile();" />
                                            </td>
                                        </tr>
                                        <tr style="height: 5px;"></tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </fieldset>
                    <fieldset class="scheduler-border" id="IndexingOptions" runat="server">
                        <legend class="scheduler-border LabelStyleBold">Indexing Options</legend>
                        <asp:UpdatePanel ID="upPatientDetails" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div style="margin-top: -15px;">
                                    <table style="width: 100%; height: 10px; font-size: 14px!important;" id="indexing">
                                        <tr style="height: 6%" class="Editabletxtbox">
                                            <td>&nbsp;Patient&nbsp;</td>
                                            <td>
                                                <asp:TextBox ID="PatientDetails" runat="server" EnableViewState="true" ReadOnly="true" AutoPostBack="false" CssClass="patientPaneEnabled"></asp:TextBox>
                                            </td>
                                            <td align="right">
                                                <input type="button" class="aspresizedbluebutton " id="btnFindPatient" runat="server" value="Find Patient" onclick="btnFindPatient_Clicked();" />
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel runat="server" ID="upIndexingDetails" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="cboDocumentType" />
                                <asp:AsyncPostBackTrigger ControlID="btnClearAll" />
                            </Triggers>
                            <ContentTemplate>
                                <div class="panel panelborderbox" style="margin-top: 7px; margin-left: 3px; margin-right: 3px;">
                                    <div class="divgroupstyle" style="height: 21px!important">
                                        Indexing Document Details
                                    </div>
                                    <table style="font-size: 14px!important;">
                                        <tr style="height: 6%; width: 100%;">
                                           
                                            <td style="width: 32%">
                                                <span class="MandLabelstyle" id="spndocumenttype">Document Type</span><span class="manredforstar">*</span>
                                            </td>
                                            <td style="width: 32%">
                                                <span class="MandLabelstyle" id="spndocumentsubtype">Document Sub Type</span><span class="manredforstar">*</span>
                                            </td>
                                             <td style="width: 25%">
                                                <span id="lblDocDate" runat="server" class="MandLabelstyle">Document Date</span><span id="DocDateStar" runat="server" class="manredforstar">*</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            
                                            <td style="width: 18%">
                                                <asp:DropDownList ID="cboDocumentType" runat="server" Style="width: 150px;" AutoPostBack="true" EnableViewState="true" OnSelectedIndexChanged="cboDocumentType_SelectedIndexChanged" CssClass="Editabletxtbox"></asp:DropDownList>   <%--onchange="changeDocuments();" --%>
                                            </td>
                                            <td style="width: 18%">
                                                <asp:DropDownList ID="cboDocumentSubType" runat="server" Style="width: 150px;" CssClass="Editabletxtbox"></asp:DropDownList>
                                            </td>
                                            <td style="width: 26%">
                                                <input id="dtpDocumentDate" type="text" runat="server" name="date" style="width: 140px;" readonly="readonly" class="Editabletxtbox" />
                                            </td>
                                        </tr>
                                        <tr> <td style="width: 18%">
                                             <span class="MandLabelstyle" id="lblDosPhy" runat="server">DOS ~ Physician</span><span id="DosPhyStar" runat="server" class="manredforstar">**</span>
                                            </td>
                                            <td colspan="2" >
                                            <asp:DropDownList ID="ddEncPhyName" runat="server" Style="width: 91%;" EnableViewState="true" CssClass="Editabletxtbox"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr style="height: 6%">
                                            <td style="width: 26%">
                                                <span class="MandLabelstyle">Selected Page(s)</span><span class="manredforstar">*</span>
                                                <input type="radio" runat="server" name="SelectedPage" id="rdbAll" onchange="PageRangeAllchange();" />
                                                <span>All</span>

                                            </td>
                                            <td colspan="2" align="centre">
                                                <input type="radio" name="SelectedPage" runat="server" id="rdbPageRange" onchange="PageRangeAllchange();" class="Editabletxtbox" />
                                                <span>Page Range</span>
                                                <input type="text" style="width: 154px;height:100%" id="txtSelectedPages" runat="server" placeholder="For ex., 1, 3, 5-12" onkeypress="txtSelectedPages_OnKeyPress()" />
                                                <asp:Button Width="60px" ID="btnSave" CssClass="aspresizedbluebutton " runat="server" OnClick="btnSave_Click" Text="Add" OnClientClick="return btnSave_Clicked();" EnableViewState="true" />
                                                <asp:Button ID="btnClearAll" CssClass="aspresizedredbutton" runat="server" OnClientClick="return clearall();" Text="Reset" EnableViewState="true" /><%--  OnClick="btnClearAll_Click" --%>
                                                <%--<asp:Button ID="btnSaveOnline" CssClass="btn btn-success btn-sm" runat="server" Text="Save" Style="height: 33px; width: 90%;" Visible="false" Enabled="false" OnClick="btnSaveOnline_Click" OnClientClick="return btnsaveclientclick();" />--%>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <asp:UpdatePanel ID="updateOrders" runat="server" UpdateMode="Conditional">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cboStandingOrders" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSearchCpt" />
                                        <asp:AsyncPostBackTrigger ControlID="btnIVProcedure" />

                                    </Triggers>
                                    <ContentTemplate>
                                        <div class="panel" style="margin-left: 3px; margin-right: 3px;">
                                            <div id="dOrder" runat="server" style="padding-top: 2px; height: 20px; cursor: pointer;" class="panel-headingIndexing LabelStyle" data-toggle="collapse" data-parent="#accordion" data-target="#dOrderCollapse">
                                                &nbsp; <i class="fa fa-chevron-right" aria-hidden="true"></i>Order Details
                                            </div>
                                            <div id="dOrderCollapse" runat="server" class="panelborderboxIndexing panel-collapse collapse">
                                                <asp:Panel ID="OrdersPanel" runat="server">
                                                    <table style="font-size: 14px!important;">
                                                        <tr style="height: 5%; width: 590px;">
                                                            <td>
                                                                <span class="spanstyle" id="spanoutstandorder" runat="server">Outstanding Orders</span><span class="manredforstar" id="spanorderstar" runat="server" visible="false">*</span>
                                                            </td>
                                                            <td colspan="3">
                                                                <asp:DropDownList ID="cboStandingOrders" AutoPostBack="false" runat="server" onchange="return SelectOrders()" Width="398px"></asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 5%; width: 590px;">
                                                            <td style="width: 200px" class="Editabletxtbox">
                                                                <span>Specimen Collected Date</span>
                                                            </td>
                                                            <td style="width: 160px;">
                                                                <input type="text" id="dtpSpecCollection" runat="server" style="width: 146px; height: 20px" enableviewstate="true" readonly="readonly" class="Editabletxtbox" />
                                                            </td>
                                                            <td style="width: 20px;">
                                                                <span id="Labspan" runat="server" class="spanstyle">Lab</span>
                                                                <span id="slabMandatory" runat="server" class="manredforstar" visible="false">*</span>
                                                            </td>
                                                            <td style="width: 150px; margin-left: 28px;">
                                                                <asp:DropDownList ID="cboLab" runat="server" AutoPostBack="false" Style="width: 120px;" CssClass="Editabletxtbox"></asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 5%; width: 590px;">
                                                            <td style="width: 120px;">
                                                                <span class="spanstyle" id="spnOrderPhy" runat="server">Ordering Physician</span><span id="spnOrderPhyStar" runat="server" visible="false" class="manredforstar">*</span>
                                                            </td>
                                                            <td style="width: 160px;">
                                                                <asp:DropDownList ID="cboOrderPhysician" runat="server" AutoPostBack="false" Style="width: 150px;" CssClass="Editabletxtbox"></asp:DropDownList>
                                                            </td>
                                                            <td style="width: 90px;">
                                                                <asp:CheckBox ID="chkOrderingPhyShowAll" runat="server" onchange="showAllOrderingPhy(this);" Text="Show All" CssClass="Editabletxtbox" />
                                                            </td>
                                                            <td style="width: 120px;">
                                                              <input type="button" id="btnSearchCpt" runat="server" class="aspresizedbluebutton" onclick="btnSearchCpt_Clicked()" value="Search Procedure" /> 
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 5%; width: 590px;">
                                                            <td style="width: 120px;">
                                                                <span class="spanstyle" id="spanphy" runat="server">Reviewing Physician Name</span><span id="spanphystar" runat="server" visible="false" class="manredforstar">*</span>
                                                            </td>
                                                            <td style="width: 160px;">
                                                                <asp:DropDownList ID="cboPhysician" runat="server" AutoPostBack="false" Style="width: 150px;" CssClass="Editabletxtbox"></asp:DropDownList>
                                                            </td>
                                                            <td style="width: 90px;">
                                                                <asp:CheckBox ID="chkShowAll" runat="server"  onchange="showAllPhy(this);" Text="Show All" CssClass="Editabletxtbox" />
                                                            </td>
                                                            <td style="width: 120px;">                                                                
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 5%; width: 590px;">

                                                            <td style="width: 200px;" class="Editabletxtbox">
                                                                <span>Is Interpretation Included</span>
                                                            </td>
                                                            <td style="width: 120px;">
                                                                <asp:DropDownList ID="cboIs_Interperated" runat="server" Style="width: 150px;" CssClass="Editabletxtbox">
                                                                    <asp:ListItem Text=" " Value="" />
                                                                    <asp:ListItem Text="Yes" Value="Y" />
                                                                    <asp:ListItem Text="No" Value="N" />
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </div>
                                            <%--<telerik:RadWindowManager ID="winMngr3" runat="server">
                                                <Windows>
                                                    <telerik:RadWindow ID="RadProcedureWindow" runat="server" Behaviors="Close" Title="Search Procedures"
                                                        VisibleStatusbar="false" IconUrl="Resources/16_16.ico">
                                                    </telerik:RadWindow>
                                                </Windows>
                                            </telerik:RadWindowManager>--%>
                                            <asp:Button ID="btnIVProcedure" runat="server" Text="IV Procedure" OnClientClick="ProcedureClick();" OnClick="btnIVProcedure_Click" CssClass="displayNone" />
                                        </div>

                                    </ContentTemplate>
                                </asp:UpdatePanel>

                              
                                    <table>
                                        <tr>
                                            <td>
                                                <div style="width: 535px;">
                                                <label id="EncMsg" runat="server" visible="false" class="MandLabelstyle">** If there is no encounter match, please create an encounter for the date of service of the provider and check-in before attaching the encounter document. </label>
                                                    </div>
                                            </td>
                                            <td>  
                                                <div style="text-align: right; margin-top: 1px; margin-right: 3px; margin-bottom: 4px;">
                                                <input type="button" runat="server" style="margin-right: 3px;" class="aspresizedgreenbutton" id="btnMoveToNextProcess" onclick="return btnMoveToNextProcess_Clicked();" value="Upload" />&nbsp;
                                    <input type="button" runat="server" style="display: none;" class="btn btn-primary btn-xs" id="btnHDNMoveToNextProcess" value="Move To Next Process" onserverclick="btnMoveToNextProcess_Click" />
                                            </div> </td>
                                        </tr>
                                    </table>
                                    
                               
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </fieldset>
                    <br />
                    <asp:UpdatePanel ID="updateGrid" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnInvisible" />
                            <asp:AsyncPostBackTrigger ControlID="grdIndexing" />
                        </Triggers>
                        <ContentTemplate>
                            <div class="panel panel-success Gridbodystyle" style="height: 308px; width: 607px;">
                                <div style="overflow-y: auto; height: 308px; width: 607px;" id="divgrd">
                                    <asp:DataGrid ID="grdIndexing" runat="server" ShowHeader="true" AutoGenerateColumns="false" OnItemCommand="grdIndexing_ItemCommand" Width="100%">
                                        <HeaderStyle BackColor="#bfdbff" Height="30px" Font-Bold="false" HorizontalAlign="Center" VerticalAlign="Middle" Font-Size="Small" Font-Names="sans-serif" CssClass="DataGridFixedHeader Gridheaderstyle" />
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Font-Size="Small" Font-Names="sans-serif" Wrap="true" CssClass="Editabletxtbox" />
                                        <Columns>
                                            <asp:TemplateColumn HeaderStyle-Width="20px">
                                                <HeaderTemplate>
                                                    <span>Edit</span>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:ImageButton ImageUrl="~/Resources/edit.gif" CausesValidation="false" runat="server" HeaderStyle-Width="20px" CommandName="EditRow" OnClientClick="ShowLoading();seteditflag();" />
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderStyle-Width="20px">
                                                <HeaderTemplate>
                                                    <span>Del</span>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:ImageButton ImageUrl="~/Resources/close_small_pressed.png" runat="server" CommandName="DeleteRow" HeaderStyle-Width="20px" OnClientClick="return confirmDelete()" />
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:BoundColumn DataField="Document Sub Type" HeaderText="Doc Sub Type" HeaderStyle-Width="100px"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="Document Date" HeaderText="Doc Date" HeaderStyle-Width="120px"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="Patient Name" HeaderText="Patient Name" HeaderStyle-Width="150px"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="File Name" HeaderText="File Name" HeaderStyle-Width="150px"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="Selected Pages" HeaderText="Pages" HeaderStyle-Width="40px"></asp:BoundColumn>
                                            <asp:TemplateColumn HeaderStyle-Width="20px">
                                                <HeaderTemplate>
                                                    <span>View</span>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:ImageButton ImageUrl="~/Resources/Down.bmp" runat="server" HeaderStyle-Width="20px" CommandName="ViewRow" OnClientClick="return viewImage(this);" />
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:BoundColumn DataField="Scan Index Id" ItemStyle-CssClass="displayNone" HeaderStyle-CssClass="displayNone"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="FilePath" ItemStyle-CssClass="displayNone" HeaderStyle-CssClass="displayNone"></asp:BoundColumn>
                                        </Columns>
                                    </asp:DataGrid>
                                </div>
                                <asp:Button ID="btnInvisible" runat="server" OnClick="btnInvisible_Click" Text="Delete" CssClass="displayNone" OnClientClick="PatientClick();" />
                                <%-- <telerik:RadWindowManager ID="rdWinView" runat="server" EnableViewState="false">
                                    <Windows>
                                        <telerik:RadWindow ID="RadImageWindow" runat="server" Behaviors="Close" Title="Image Viewer" CssClass="aspresizedredbutton"
                                            VisibleStatusbar="false" IconUrl="Resources/16_16.ico">
                                        </telerik:RadWindow>
                                    </Windows>
                                </telerik:RadWindowManager>--%>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="pageContent" style="margin-left: 3px;">
                    <asp:UpdatePanel ID="updateImages" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="panel panelborderbox" style="height: 181px; width: 535px; float: left; clear: none;">
                                <div class="divgroupstyle" style="height: 21px!important">File(s)</div>
                                <div id="fileThumbs" runat="server" style="height: 156px; width: 535px; overflow-y: auto;" enableviewstate="true">
                                    <table id="tblFiles" class="table table-bordered Gridbodystyle" style="width: 99.7%">
                                        <tbody id="tbFilesBody" class="Gridbodystyle">
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                            <div class="panel panelborderbox" style="height: 688px; width: 539px; float: left; clear: none; margin-top: -13px!important">
                                <div class="divgroupstyle" style="height: 21px!important">Preview</div>


                                <%-- <table style="width: 100%; height: 96%">
                                    <tr style="height: 100%">--%>
                                <%--<td style="width: 25%; height: 610px; overflow-y: hidden;">
                                            <div runat="server" id="_plcImgsThumbs" style="height: 585px; overflow-y: auto;"></div>
                                            <br />
                                        </td>--%>
                                <%-- <td style="width: 100%; height: 100%;">
                                            <table>
                                                <tr>--%>
                                <%-- <div style="margin-left: 90px;" id="imgControls"> --%>
                                <table id="imgControls" style="width: 100%; margin-left: 70px;">
                                    <tr>
                                        <td style="width: 5%;"><i class="fa fa-rotate-left" style="cursor: pointer;" id="leftrotate" title="Rotate Left"></i></td>
                                        <td style="width: 5%;"><i class="fa fa-rotate-right" style="cursor: pointer;" id="rotateright" title="Rotate Right"></i></td>
                                        <td style="width: 5%;"><i class="fa fa-chevron-left" id="prev" title="Previous Image" style="cursor: pointer;" runat="server"></i></td>
                                        <td style="width: 5%;">
                                            <input name="PageBox" type="text" id="PageBox" runat="server" style="width: 18px; height: 15px;" readonly="readonly" /></td>
                                        <td style="width: 5%;">
                                            <label id="PageLabel" runat="server" style="height: 10px;">/ 0</label></td>
                                        <td style="width: 5%;"><i class="fa fa-chevron-right" id="next" style="cursor: pointer;" title="Next Image" runat="server"></i></td>
                                        <td style="width: 5%;"><i class="fa fa-search-plus" id="zoomin" style="cursor: pointer;" title="Zoom in"></i></td>
                                        <td style="width: 5%;"><i class="fa fa-search-minus" id="zoomout" style="cursor: pointer;" title="Zoom out"></i></td>
                                        <td style="width: 5%;"><i class="fa fa-picture-o" id="revert" style="cursor: pointer;" title="Revert to original"></i></td>
                                        <td style="width: 5%;"><i class="fa fa-times" id="deletethumbnail" runat="server" style="cursor: pointer; display: none;" title="delete viewing image"></i></td>
                                    </tr>
                                </table>
                                <%--  </div>
                                                </tr>
                                                <tr>--%>
                                <div style="margin-top: 10px; height: 635px; width: 533px; border: 1px solid #6A9C73; overflow: auto;" id="imgholder" runat="server">
                                    <img id="_imgBig" runat="server" /><%-- alt="Loading Page.." />--%>
                                </div>
                                <div style="margin-top: 10px; height: 657px; width: 537px; border: 1px solid #6A9C73; overflow: auto;" id="PDFholder" runat="server">
                                    <iframe id="bigImagePDF" runat="server" style="height: 655px; width: 535px!important; overflow: auto;" frameborder="0" src="Viewimg.aspx"></iframe>
                                </div>
                                <%-- </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>--%>
                            </div>
                            <table style="width: 98%;">
                                <tr>
                                    <td></td>
                                    <td align="right">
                                        <asp:UpdatePanel ID="updateControlButtons" runat="server" UpdateMode="Conditional">
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnMoveToNextProcess" />
                                                <asp:AsyncPostBackTrigger ControlID="btnHDNMoveToNextProcess" />
                                            </Triggers>
                                            <ContentTemplate>

                                                <input type="button" runat="server" class="aspresizedbluebutton" id="btnMovetoNonMedicalFolder" value="Move to Non Capella EHR Fax" style="margin-right: 5px;" onclick="MovetoNonMedicalFolder();" />
                                                <input type="button" class="aspresizedredbutton" style="float: right;" id="btnClose" value="Close" onclick="btnClose_Clicked();" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                            <%--  <div style="width: 450px; float: right;">
                                <input type="button" id="btnSaveImage" runat="server" onclick="btnSaveImage_Clicked()" visible="false" onserverclick="btnSaveImage_Click" value="Save Image" />
                            </div>--%>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <asp:UpdatePanel ID="waitCursor" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div id="divLoading" class="modal" runat="server" style="text-align: center; display: none">
                        <asp:Panel ID="Panel2" runat="server">
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
        </div>
        <%--<div id="TabFindPatient" class="modal fade" style="display:none; margin-top: 50px; margin-left: 0px; padding-left: 55px; background-color: rgba(0, 0, 0, 0.13); z-index: 5001;">
            <div id="TabmdldlgFindPatient" class="modal-dialog" style="width: 100%;margin-top: 15%; position: absolute!important;">
                <div class=" modal-content" style="height: 80%">
                    <div class="modal-header" style="height: 52px">
                        <button type="button" class="close" id="btnFindPatientClose" onclick="btnFindPatientClose_Click();" style="font-size: 30px !important;" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <h3 class="modal-title" style="font-family: sans-serif; font-weight: bold; font-size: medium" id="TabModalFindPatientTitle"></h3>
                    </div>
                    <div class="modal-body" style="width: 100%; margin-top: -10px; padding-top: 0px;">
                        <iframe style="width: 100%; border: none; height: 210px;" id="TabFindPatientFrame"></iframe>
                    </div>
                    <input type="text" id="txtPatientInformation" value="" style="display: none;" />
                </div>
            </div>
        </div>--%>

        <%--<div id="TabQuickPatient" class="modal fade" style="display: none; margin-top: 50px; margin-left: 0px; padding-left: 55px; background-color: rgba(0, 0, 0, 0.13); z-index: 5001;">
            <div id="TabmdldlgQuickPatient" class="modal-dialog" style="width: 95%; margin-left: 55px; margin-top: 225px; position: absolute!important;">
                <div class=" modal-content" style="height: 80%">
                    <div class="modal-header" style="height: 52px">
                        <button type="button" class="close" id="btnQuickPatientClose" onclick="btnQuickPatientClose_Click();" style="font-size: 30px !important;" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <h3 class="modal-title" style="font-family: sans-serif; font-weight: bold; font-size: medium" id="TabModalQuickPatientTitle"></h3>
                    </div>
                    <div class="modal-body" style="width: 100%; margin-top: -10px; padding-top: 0px">
                        <iframe style="width: 100%; border: none; height: 245px;" id="TabQuickPatientFrame"></iframe>
                    </div>
                    <input type="text" id="txtQuickPatientInformation" value="" style="display: none;" />
                </div>
            </div>
        </div>--%>

       <%-- <div id="TabSearchProcedure" class="modal fade" style="display: none; margin-left: 0px; padding-left: 55px; background-color: rgba(0, 0, 0, 0.13); z-index: 5001;">
            <div id="TabmdldlgSearchProcedure" class="modal-dialog" style="width: 900px; margin-left: 130px; margin-top: 70px; position: absolute!important;">
                <div class=" modal-content" style="height: 80%">
                    <div class="modal-header" style="height: 52px">
                        <button type="button" class="close" id="btnSearchProcedureClose" onclick="btnSearchProcedureClose_Click();" style="font-size: 30px !important;" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <h3 class="modal-title" style="font-family: sans-serif; font-weight: bold; font-size: medium" id="TabModalSearchProcedureTitle"></h3>
                    </div>
                    <div class="modal-body" style="width: 100%; margin-top: -10px; padding-top: 0px">
                        <iframe style="width: 100%; border: none; height: 210px;" id="TabSearchProcedureFrame"></iframe>
                    </div>
                    <input type="text" id="txtSearchProcedureInformation" value="" style="display: none;" />
                </div>
            </div>
        </div>--%>

      <%--  <div id="TabViewImageIndexing" class="modal fade" style="display: none; margin-left: 0px; padding-left: 55px; background-color: rgba(0, 0, 0, 0.13); z-index: 5001;">
            <div id="TabmdldlgViewImageIndexing" class="modal-dialog" style="width: 975px; margin-left: 10px; margin-top: 65px; position: absolute!important;">
                <div class=" modal-content" style="height: 80%">
                    <div class="modal-header" style="height: 52px">
                        <button type="button" class="close" id="btnViewImageIndexingClose" onclick="btnViewImageIndexingClose_Click();" style="font-size: 30px !important;" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <h3 class="modal-title" style="font-family: sans-serif; font-weight: bold; font-size: medium" id="TabModalViewImageIndexingTitle"></h3>
                    </div>
                    <div class="modal-body" style="width: 100%; margin-top: -10px; padding-top: 0px">
                        <iframe style="width: 100%; border: none; height: 210px;" id="TabViewImageIndexingFrame"></iframe>
                    </div>
                    <input type="text" id="txtViewImageIndexingInformation" value="" style="display: none;" />
                </div>
            </div>
        </div>--%>
        <%--<div id="divErrorMessage" class="modal modal-wide fade" style="display: none; z-index: 50000000!important; background-color: transparent!important; display: none;">
            <div class="modal-dialog" style="width: 30%; margin-top: 0%;">
                <div class="modal-content">
                    <div class="modal-header" style="height: 46px;">
                        <div class="row" style="margin-top: -25px;">
                            <div class="col-xs-8">
                                <h3>Capella -EHR</h3>
                            </div>
                            <div class="col-xs-4" style="margin-top: 17px;">
                                <button type="button" class="close" onclick="HideLoadIcdon();" data-dismiss="modal" aria-hidden="true">&times;</button>
                            </div>
                        </div>
                    </div>
                    <div class="modal-body" id="divErrorMsgBody" style="overflow-y: auto;">
                        <p style="font-size: small;" id="pErrorMsg"></p>

                    </div>
                    <div class="modal-footer" style="height: 50px;">
                        <button id="btnErrorOk" style="margin-top: -10px; border: 1px solid !important;">Ok</button>
                        <button id="btnErrorCancel" style="margin-top: -10px; border: 1px solid !important;">Cancel</button>
                        <button id="btnErrorOkCancel" data-dismiss="modal" style="margin-top: -10px; border: 1px solid !important;">OK</button>
                    </div>
                </div>
            </div>
        </div>--%>

        <div id="CheckAlert" onclick="ToolStripAlertHide();">
            <div id="innerMsgText" style="font-family: Verdana,Arial,sans-serif !important; font-size: 18px; color: #000000;"></div>
        </div>

        <asp:HiddenField ID="hdnUpdateMode" runat="server" />
        <asp:HiddenField ID="hdnNo" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnHumanID" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnProcedure" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="IsClickDirectUpload" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnFileID" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnfilenamedelete" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdndeletePgNo" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnScreenMode" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnsourceFile" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnfilepath" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnFileName" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnLastIndexFileName" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnPagecount" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnIsMyScan" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnScanningLocal" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnPDFurl" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnIsEditgrid" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnIsAncillary" runat="server" EnableViewState="false" />
        <%--<asp:HiddenField ID="hdnPageBox" runat="server" EnableViewState="false" />--%>
        <%--<asp:HiddenField ID="hdnPageState" runat="server" EnableViewState="false" Value="default" />--%>
        <%--<asp:HiddenField ID="hdnScanIndexId" runat="server" />--%>
        <%--<asp:HiddenField ID="IsSaveClickedSucessfull" runat="server" EnableViewState="false" />--%>
        <%--<asp:HiddenField ID="hdnIsGrid" runat="server" EnableViewState="false" />--%>
        <%--<asp:HiddenField ID="hdnfilename" runat="server" EnableViewState="false" />--%>
        <%--<asp:HiddenField ID="hdnPatientdetails" runat="server" EnableViewState="false" />--%>
        <%--<asp:Button ID="InvisibleOpenFile" runat="server" OnClick="InvisibleOpenFile_Click" CssClass="displayNone" />--%>

        <asp:Button ID="btnIVfindpatient" runat="server" Text="IV Patient" OnClientClick="PatientClick();" OnClick="btnIVfindpatient_Click" CssClass="displayNone" />
        <asp:Button ID="btnclearAfterupload" runat="server" OnClick="btnclearAfterupload_Click" CssClass="displayNone" />
        <asp:Button ID="btnResetFields" runat="server" CssClass="displayNone" OnClick="btnResetFields_Click" />
        <asp:Button ID="btnUpdateCancelok" runat="server" CssClass="displayNone" OnClick="btnUpdateCancelok_Click" />
        <asp:Button ID="btnhdnloadfile" runat="server" Style="display: none;" OnClick="btnhdnloadfile_Click" />
      <%--  <asp:Button ID="btnDeleteThumbnail" runat="server" Style="display: none;" OnClick="btnDeleteThumbnail_Click" />--%>
        <asp:Button ID="hdnMoventoNonmedicalFolder" runat="server" Style="display: none;" OnClick="hdnMoventoNonmedicalFolder_Click" />
        <asp:Button ID="hdnSingleFileCheck" runat="server" Style="display: none;" OnClick="hdnSingleFileCheck_Click" />
        <asp:Button ID="InvisibleButton" runat="server" Text="Button" CssClass="displayNone" OnClick="InvisibleButton_Click" />
        <%--<asp:Button ID="hdnFindPatient" runat="server" Style="display: none;" OnClick="hdnFindPatient_Click" />--%>
      <%--  <asp:HiddenField ID="hdnLocalTime" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnLocalDate" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnUniversaloffset" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnLocalDateAndTime" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnFollowsDayLightSavings" runat="server" Value="false" />--%>
        <asp:Button ID="hdnbtngenerateindexingxml" runat="server" OnClick="hdnbtngeneratexml_Click"  style="display:none" />

        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <%--<script src="JScripts/jquery-1.7.1.min.js" type="text/javascript"></script>--%>
             <script src="JScripts/jquery-1.11.3.min.js" type="text/javascript"></script>
            <script src="JScripts/jquery-ui.min1.10.2.js" type="text/javascript"></script>
            <%--<script src="JScripts/jquery-ui.min.js"></script>--%>
            <%--<script src="JScripts/jquery-2.2.3.js"></script>--%>
            <script type="text/javascript" src="JScripts/jquery.datetimepicker.js"></script>
            <script type="text/javascript" src="JScripts/Lazyload.js"></script>
 
            <link href="CSS/bootstrap.min3.4.0.css" rel="stylesheet" />
            <link href="CSS/bootstrap.min.css" rel="stylesheet" />
              <link href="CSS/jquery-ui.css" rel="Stylesheet" />
              <script src="JScripts/bootstrap.min3.4.0.js" type="text/javascript"></script>
  
            <!--Certain Files are marked as static files, no need to implement the VersionConfiguration Technology in the pages-->
            <script src="JScripts/JSModalWindow.js" type="text/javascript" defer="defer"></script>
            <script src="JScripts/JSIndexingAndRescanning.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <%--defer="defer"--%>
            <script src="JScripts/JSOnlineDocuments.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSLibraries.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
       </asp:PlaceHolder>
    </form>
</body>
</html>
