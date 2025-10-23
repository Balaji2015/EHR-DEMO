<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmFlowSheet.aspx.cs" Inherits="Acurus.Capella.UI.frmFlowSheet"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="aspx" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
      <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <title>Flow Sheet</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>

     <style type="text/css">
    #pnlFlowSheetDetails fieldset{
            height: 128px;
        }
     </style>

    <script type="text/javascript">
     function LoadFlowSheet()
        {
            $("span[mand=Yes]").addClass('MandLabelstyle');

            $("span[mand=Yes]").each(function () {
                $(this).html($(this).html().replace("*", "<span class='manredforstar'>*</span>"));
            });
     }
        </script>
</head>

<body oncontextmenu="return false" onload="{ sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); LoadFlowSheet(); }"><%--Added onload for BUgID:45808 --%>
    <form id="form1" runat="server">
        <telerik:RadWindowManager ID="ModalWindowMngt" runat="server">
            <Windows>
                <telerik:RadWindow ID="ModalWindow" runat="server" VisibleOnPageLoad="false" Height="625px" IconUrl="Resources/16_16.ico"
                    Width="1225px">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <div>
            <aspx:ToolkitScriptManager ID="ScriptMngr" runat="server" CombineScripts="true">
            </aspx:ToolkitScriptManager>
            <table width="100%" bgcolor="white">
                <tr>
                    <td>
                        <asp:Panel ID="pnlFlowSheetDetails" runat="server" GroupingText="Flow Sheet Details"
                            Width="100%" CssClass="Panel" Font-Size="Small" BackColor="White" Height="142px"
                            
                            ScrollBars="None">
                            <table width="100%" style="height: 67px">
                                <tr>
                                    <td style="width: 15%">
                                        <asp:Label ID="lblSelectPhysician" runat="server" Text="Select Physician *" cssclass="MandLabelstyle" mand="Yes"
                                            Width="100%"></asp:Label>
                                    </td>
                                    <td colspan="2" style="width: 15%">
                                        <telerik:RadComboBox ID="cboPhysician" runat="server" Width="100%" CssClass="nonEditabletxtbox"
                                            OnSelectedIndexChanged="cboPhysician_SelectedIndexChanged" OnClientSelectedIndexChanged="StartLoadFromPatChart"
                                            AutoPostBack="true">
                                        </telerik:RadComboBox>
                                    </td>
                                    <td style="width: 16%">
                                        <asp:CheckBox ID="chkShowallPhysician" runat="server" Text="Show All Physician" Width="100%" CssClass="spanstyle"
                                            OnCheckedChanged="chkShowallPhysician_CheckedChanged" AutoPostBack="true" onclick="javascript: { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}" />
                                    </td>
                                    <td style="width: 14%">
                                        <asp:Label ID="pnlFlowSheet" runat="server" Text="Flow Sheet *" cssclass="MandLabelstyle" mand="Yes" Width="100%"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <telerik:RadComboBox ID="cboFlowSheet" runat="server" Width="100%" OnClientSelectedIndexChanged="cboFlowSheet_SelectedIndexChanged" CssClass="Editabletxtbox">
                                        </telerik:RadComboBox>
                                    </td>
                                    <td style="width: 2%">
                                        <asp:ImageButton ID="pbLibraryCondition" runat="server" ImageUrl="~/Resources/Database Inactive.jpg"
                                            OnClientClick="return OpenFlowSheetManager();" OnClick="pbLibraryCondition_Click" ToolTip="Manager Library" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%">
                                        <asp:Label ID="lblSelectEncounter" runat="server" Text="Select Encounter *" cssclass="MandLabelstyle" mand="Yes"
                                            Width="100%"></asp:Label>
                                    </td>

                                    <td style="width: 5%">
                                        <asp:RadioButton ID="rdAll" runat="server" Text="All" Width="80%"
                                            onclick="ClearCheckBoxes('rdAll','FlowSheet');" CssClass="spanstyle" />
                                    </td>
                                    <td style="width: 15%">
                                        <asp:RadioButton ID="rdLast3Month" runat="server" Text="Last 3 Month" Width="100%"
                                            onclick="ClearCheckBoxes('rdLast3Month','FlowSheet');" CssClass="spanstyle" />
                                    </td>
                                    <td style="width: 9%">
                                        <asp:RadioButton ID="rdLast6Month" runat="server" Text="Last 6 Month" Width="100%"
                                            onclick="ClearCheckBoxes('rdLast6Month','FlowSheet');" CssClass="spanstyle" />
                                    </td>
                                    <td style="width: 9%">
                                        <asp:RadioButton ID="rdLast12Month" runat="server" Text="Last 12 Month" Width="100%"
                                            onclick="ClearCheckBoxes('rdLast12Month','FlowSheet');" CssClass="spanstyle" />
                                    </td>
                                    <td style="width: 15%"></td>
                                    <td style="width: 10%"></td>
                                </tr>
                                <tr>
                                    <td style="width: 7%">
                                        <asp:RadioButton ID="rdRange" runat="server" Text="Date Range" Width="100%" onclick="ClearCheckBoxes('rdRange','FlowSheet');" CssClass="spanstyle"/>
                                    </td>
                                    <td style="width: 17%" align="center">
                                        <asp:Label ID="lblFromDate" runat="server" Text="From Date" Width="55%" CssClass="spanstyle"></asp:Label>
                                    </td>
                                    <td style="width: 7%">
                                        <telerik:RadDatePicker ID="fromDate" runat="server" DateInput-DateFormat="dd-MMM-yyyy" Culture="English (United States)" AutoPostBack="false"
                                            Width="100%">
                                            <Calendar runat="server">
                                                <SpecialDays>
                                                    <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="Silver" />
                                                </SpecialDays>
                                            </Calendar>
                                        </telerik:RadDatePicker>
                                    </td>
                                    <td style="width: 7%" align="center">
                                        <asp:Label ID="lblToDate" runat="server" Style="margin-bottom: 0px" Text="To Date"
                                            Width="100%" CssClass="spanstyle"></asp:Label>
                                    </td>
                                    <td style="width: 7%">
                                        <telerik:RadDatePicker ID="todate" runat="server" DateInput-DateFormat="dd-MMM-yyyy" Culture="English (United States)" AutoPostBack="false"
                                            Width="100%">
                                            <Calendar runat="server">
                                                <SpecialDays>
                                                    <telerik:RadCalendarDay Repeatable="Today" ItemStyle-BackColor="Silver" />
                                                </SpecialDays>
                                            </Calendar>
                                        </telerik:RadDatePicker>
                                    </td>
                                    <td style="width: 13%">
                                        <telerik:RadButton ID="btnGet" runat="server" Text="Generate Flow Sheet" Width="101%" style="height: 34px !important;"
                                            OnClientClicked="btnGet_Clicked" OnClick="btnGet_Click" ButtonType="LinkButton" CssClass="bluebutton teleriknormalbuttonstyle">
                                        </telerik:RadButton>
                                    </td>
                                    <td style="width: 12%">
                                        <telerik:RadButton ID="btnClearAll" runat="server" Text="Clear All" Width="94%" style="height: 34px !important;"
                                            OnClientClicked="btnClearAll_Clicked" ButtonType="LinkButton" CssClass="redbutton teleriknormalbuttonstyle">
                                        </telerik:RadButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7">
                                        <asp:Label ID="lblMessage" runat="server" Font-Bold="true" Text="No results found for the selected Flow Sheet Template"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr></table>
            <table  style="height:30%;width:100%;"><%--Set width to 100% for BUgID:45808 --%>
                <tr>
                    <td>
                        <asp:Panel ID="pnlflowsheetgraph" runat="server" GroupingText="Flow Sheet(s)"  Height="560px"
                            CssClass="LabelStyleBold">
                            <telerik:RadGrid ID="grdFlowSheet" runat="server" CellSpacing="0"  Height="560px"
                                GridLines="Vertical" EnableEmbeddedSkins="true" Skin="Vista" OnPreRender="RadGrid1_PreRender" MasterTableView-TableLayout="Fixed">
                                <FilterMenu EnableImageSprites="False">
                                </FilterMenu>
                             
                                <ClientSettings>
                                    <Selecting CellSelectionMode="MultiCell" />
                                    <ClientEvents OnCellSelected="CellSelected" />
                                    <Scrolling AllowScroll="True" ></Scrolling>
                                </ClientSettings>
                                <MasterTableView>
                                    <CommandItemSettings ExportToPdfText="Export to PDF" />
                                    <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                                    </RowIndicatorColumn>
                                    <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                                    </ExpandCollapseColumn>
                                    <Columns>
                                        <telerik:GridImageColumn  HeaderStyle-Width="50px" HeaderText="Graph" ImageUrl="Resources/graph.JPG">
                                        </telerik:GridImageColumn>
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
            <asp:Button ID="InvisibleButton" runat="server" CssClass="displayNone" OnClick="InvisibleButton_Click" />
            <asp:HiddenField ID="SelectedItem" runat="server" />
            <asp:HiddenField ID="hdnPhyId" runat="server" />
            <asp:HiddenField ID="IsFromClear" runat="server" />
        </div>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <script src="JScripts/jquery-1.11.3.min.js" type="text/javascript"></script>
            <script src="JScripts/jquery-ui.min1.10.2.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/bootstrap.min.js" type="text/javascript"></script>
            <script src="JScripts/JSChart.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <link href="CSS/ElementStyles.css" rel="stylesheet" type="text/css" />
        </asp:PlaceHolder>
    </form>
</body>
</html>
<asp:PlaceHolder runat="server">
<script src="JScripts/JSPQRI.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
<script src="JScripts/JSLibraries.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
<script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
<script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
<script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
</asp:PlaceHolder>
