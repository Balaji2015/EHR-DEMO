<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmVitalsHistory.aspx.cs"
    Inherits="Acurus.Capella.UI.frmVitalsHistory" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Past Vitals</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
       <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="frmvitalsHistory" runat="server" style="width:758px; height:436px;">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server" EnableViewState="false">
    </telerik:RadScriptManager>
    <telerik:RadAjaxPanel ID="RadAjaxPnlPastVitals" runat="server" Width="16px">
        <div>
           
            <asp:Panel ID="pnlGrid" runat="server" BorderWidth="1px" Height="436px" 
                Width="750px">
                <telerik:RadGrid ID="grdPastVitals" runat="server" CellSpacing="0" 
                    EnableViewState="False" GridLines="Both" CssClass="Gridbodystyle"
                    Height="436px" Width="750px">
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                      <HeaderStyle Font-Bold="true"   CssClass="Gridheaderstyle"/>
                                   <ItemStyle CssClass="Gridbodystyle" />
                    <ClientSettings>
                        <Scrolling AllowScroll="True" UseStaticHeaders="True"
                            SaveScrollPosition="True" />
                    </ClientSettings>
                    <MasterTableView>
                        <CommandItemSettings ExportToPdfText="Export to PDF" />
                        <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                            <HeaderStyle Width="20px" />
                             <ItemStyle  CssClass="Editabletxtbox" BorderStyle="Dotted" />
                        </RowIndicatorColumn>
                        <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                            <HeaderStyle Width="20px" />
                            <ItemStyle  CssClass="Editabletxtbox" BorderStyle="Dotted" />
                        </ExpandCollapseColumn>
                        <EditFormSettings>
                            <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                            </EditColumn>
                        </EditFormSettings>
                        
                    </MasterTableView>
                  

                </telerik:RadGrid>
            </asp:Panel>
        </div>
    </telerik:RadAjaxPanel>
<asp:PlaceHolder ID="PlaceHolder1" runat="server">
    <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
   
   </asp:PlaceHolder>
    </form>
</body>
</html>
