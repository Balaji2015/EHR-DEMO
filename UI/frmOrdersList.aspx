
<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmOrdersList.aspx.cs"
    Inherits="Acurus.Capella.UI.frmOrdersList"  ValidateRequest="false" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
 <base target="_self"   />   
    
   
<style type="text/css">
.DisplayNone
{
    display:none;
}
.underline
{
    text-decoration: underline;
}
  .btn-primary{
            color:#fff;background-color:#337ab7;border-color:#2e6da4;border: 1px solid transparent;border-radius: 4px;padding: 4px 0px;
        }
        .btn-success{
            color:#fff;background-color:#5cb85c;border-color:#4cae4c;border: 1px solid transparent;border-radius: 4px;padding: 4px 4px;
        }
        .btn-danger{
            color:#fff;background-color:#d9534f;border-color:#d43f3a;border: 1px solid transparent;border-radius: 4px;padding: 4px 4px;
        }
legend {font-size:13px; font-weight:bold; }
.rgDataDiv{
    height:480px!important;
}

.listresize{
    height:90px!important;
    margin-top: -19em!important;
    width:23px;
    margin-left: 1px!important; 
    font-size:11px;
}
</style>
 <script type="text/javascript">
   
    </script>
 <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />  

</head>
<body onload="OnloadOrderList();">
    <form id="form1" runat="server">
    <telerik:RadWindowManager ID="WindowMngr" runat="server">
        <Windows>
            <telerik:RadWindow ID="MessageWindow" runat="server" Behaviors="Close" Title="Vitals"
                IconUrl="Resources/16_16.ico">
            </telerik:RadWindow>
              <telerik:RadWindow ID="RadResultWindow" runat="server" Behaviors="Close" Title="Result Viewer" VisibleStatusbar= "false" VisibleOnPageLoad="false" 
                IconUrl="Resources/16_16.ico">   
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
   <%-- <asp:HyperLink ID="lnkDiagnosticOrder" runat="server" Style="font-family:'Microsoft Sans Serif'; font-size:11.5px;"  NavigateUrl="~/frmImageAndLabOrder.aspx">Detailed Order</asp:HyperLink><span style="padding-left:20px;">
    <asp:HyperLink ID="lnkOrderList" runat="server" Style="font-family:'Microsoft Sans Serif'; font-size:11.5px;"  NavigateUrl="~/frmOrdersList.aspx">Order List</asp:HyperLink></span>--%>
    <div>
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <table style="width:100%;">
            <tr>
                <td ><table>
                    <tr>
                      <%--  <td ><p style="height:90px;margin-top: -16em;width:22px;"><asp:HyperLink ID="lnkDiagnosticOrder" Style="position: absolute;
    width: 105px;font-weight:bold;margin-left: -52px;font-family:'Microsoft Sans Serif'; font-size:11.5px;" CssClass="verti"  runat="server" NavigateUrl="~/frmImageAndLabOrder.aspx">Detailed Order</asp:HyperLink></p>
                             <p style="height:90px;margin-top: -2em;width:20px;">
                            <asp:HyperLink Style="position: absolute;
    width: 105px;font-weight:bold;margin-left: -52px;font-family:'Microsoft Sans Serif'; font-size:11.5px;" CssClass="verti" ID="lnkOrderList" runat="server" NavigateUrl="~/frmOrdersList.aspx">Order List</asp:HyperLink></p>
                           </td>--%>
                         <td ><%--<p class="listresize"><asp:HyperLink ID="lnkDiagnosticOrder" Style="position: absolute; top: 45px; 
    width: 78px;font-weight:bold;font-family:'Microsoft Sans Serif'; font-size:11px;margin-left: -37px;" CssClass="vertiAlign"  runat="server" NavigateUrl="~/frmImageAndLabOrder.aspx"
                        >Detailed Order</asp:HyperLink></p>
                             <p>
                            <asp:HyperLink Style="position: absolute; left: -37px; top: 120px;
    width: 89px;font-weight:bold;font-family:'Microsoft Sans Serif'; font-size:11.5px;" CssClass="vertiAlign" ID="lnkOrderList" runat="server" NavigateUrl="~/frmOrdersList.aspx"
                                >Order List</asp:HyperLink>  </p>--%>
                              <p style="height: 90px; margin-top: -16.3em; width: 22px; margin-left: -54px;">
                                        <asp:HyperLink ID="lnkDiagnosticOrder" Style="position: absolute; width: 105px; font-weight:bold;font-family:'Microsoft Sans Serif'; font-size:11px;transform: rotate(270deg);"
                                            runat="server" NavigateUrl="~/frmImageAndLabOrder.aspx"
                                            EnableViewState="false">Detailed Order</asp:HyperLink>
                                    </p>
                                    <p style="height: 90px; margin-top: -1em; width: 20px; margin-left: -40px;">
                                        <asp:HyperLink Style="position: absolute; width: 75px; font-weight:bold;font-family:'Microsoft Sans Serif'; font-size:11px;transform: rotate(270deg);"
                                            ID="lnkOrderList" runat="server" NavigateUrl="~/frmOrdersList.aspx"
                                            EnableViewState="false">Order List</asp:HyperLink>
                                    </p>
                           </td>
                    </tr>
                </table>
                </td>
                <td>
                    <table style="width:100%;">
            <tr>
                <td style="vertical-align: top; width: 100%;">
                    <asp:Panel ID="Panel1" runat="server" GroupingText="List Of Orders"
                        Width="100%" Height="540px" Style="margin-left: -3px;" CssClass="LabelStyleBold">
                        <telerik:RadGrid ID="grdOrders" runat="server" Width="1140px" Style="margin-left: -8px;margin-right: -8px;"  AutoGenerateColumns="False"
                            CellSpacing="0" GridLines="None" OnItemCommand="grdOrders_ItemCommand"
                            onitemcreated="grdOrders_ItemCreated" Height="500px" CssClass="Gridbodystyle"  >
                            
                            <ClientSettings>
                                <Scrolling AllowScroll="True"/>
                                <ClientEvents OnCommand="grdOrders_OnCommand"/>
                            </ClientSettings>
                            <MasterTableView>
                                <Columns>
                                    <telerik:GridButtonColumn DataTextField="Edit" ButtonType="ImageButton" FilterControlAltText="Filter Edit column"
                                        HeaderText="Edit" UniqueName="Edit" ImageUrl="~/Resources/edit.GIF" 
                                        CommandName="EditC">
                                        <HeaderStyle CssClass="Gridheaderstyle" />
                                     <ItemStyle BorderColor="#CCFFFF" BorderStyle="Dotted" CssClass="Editabletxtbox" />   
                                    </telerik:GridButtonColumn>
                                    <telerik:GridButtonColumn  DataTextField="Del" ButtonType="ImageButton" FilterControlAltText="Filter Del column"
                                        HeaderText="Del" UniqueName="Del" ImageUrl="~/Resources/close_small_pressed.png" 
                                        CommandName="Del">
                                          <HeaderStyle CssClass="Gridheaderstyle" />
                                        <ItemStyle BorderColor="#CCFFFF" BorderStyle="Dotted" CssClass="Editabletxtbox"/>
                                    </telerik:GridButtonColumn>
                                    <telerik:GridBoundColumn DataField="Quantity" FilterControlAltText="Filter Quantity column"
                                        HeaderText="Quantity" UniqueName="Quantity" Visible="False">
                                          <HeaderStyle CssClass="Gridheaderstyle" />
                                        <ItemStyle BorderColor="#CCFFFF" BorderStyle="Dotted" CssClass="Editabletxtbox"/>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Procedure" FilterControlAltText="Filter Procedure column"
                                        HeaderText="Lab Procedure" UniqueName="Procedure">
                                          <HeaderStyle CssClass="Gridheaderstyle" />
                                        <ItemStyle CssClass="Editabletxtbox" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Assessment" FilterControlAltText="Filter Assessment column"
                                        HeaderText="Diagnosis" UniqueName="Assessment">
                                          <HeaderStyle CssClass="Gridheaderstyle" />
                                        <ItemStyle BorderColor="#CCFFFF" BorderStyle="Dotted" CssClass="Editabletxtbox"/>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="SpecimenInHouse" FilterControlAltText="Filter SpecimenInHouse column"
                                        HeaderText="Specimen In House" UniqueName="SpecimenInHouse">
                                          <HeaderStyle CssClass="Gridheaderstyle" />
                                       <ItemStyle BorderColor="#CCFFFF" BorderStyle="Dotted" CssClass="Editabletxtbox" /> 
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Specimen" FilterControlAltText="Filter Specimen column"
                                        HeaderText="Specimen" UniqueName="Specimen">
                                          <HeaderStyle CssClass="Gridheaderstyle" />
                                    <ItemStyle BorderColor="#CCFFFF" BorderStyle="Dotted" CssClass="Editabletxtbox" />    
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Unit" FilterControlAltText="Filter Unit column"
                                        HeaderText="Specimen Qty " UniqueName="Unit">
                                          <HeaderStyle CssClass="Gridheaderstyle" />
                                        <ItemStyle BorderColor="#CCFFFF" BorderStyle="Dotted" CssClass="Editabletxtbox" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="AuthReq" FilterControlAltText="Filter AuthReq column"
                                        HeaderText="Auth. Req." UniqueName="AuthReq">
                                          <HeaderStyle CssClass="Gridheaderstyle" />
                                        <ItemStyle BorderColor="#CCFFFF" BorderStyle="Dotted" CssClass="Editabletxtbox"/>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Stat" FilterControlAltText="Filter Stat column"
                                        HeaderText="Stat" UniqueName="Stat">
                                          <HeaderStyle CssClass="Gridheaderstyle" />
                                        <ItemStyle BorderColor="#CCFFFF" BorderStyle="Dotted" CssClass="Editabletxtbox"/>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Test Date" FilterControlAltText="Filter TestDate column"
                                        HeaderText="Test Date" UniqueName="TestDate">
                                          <HeaderStyle CssClass="Gridheaderstyle" />
                                        <ItemStyle BorderColor="#CCFFFF" BorderStyle="Dotted" CssClass="Editabletxtbox"/>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Fasting" FilterControlAltText="Filter Fasting column"
                                        HeaderText="Fasting" UniqueName="Fasting">
                                          <HeaderStyle CssClass="Gridheaderstyle" />
                                        <ItemStyle BorderColor="#CCFFFF" BorderStyle="Dotted" CssClass="Editabletxtbox"/>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Lab" FilterControlAltText="Filter Lab column"
                                        HeaderText="Lab" UniqueName="Lab">
                                          <HeaderStyle CssClass="Gridheaderstyle" />
                                        <ItemStyle BorderColor="#CCFFFF" BorderStyle="Dotted" CssClass="Editabletxtbox" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Location" FilterControlAltText="Filter Location column"
                                        HeaderText="Location" UniqueName="Location">
                                          <HeaderStyle CssClass="Gridheaderstyle" />
                                        <ItemStyle BorderColor="#CCFFFF" BorderStyle="Dotted" CssClass="Editabletxtbox"/>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Clinical Information" FilterControlAltText="Filter ClinicalInformation column"
                                        HeaderText="Clinical Information" UniqueName="ClinicalInformation">
                                          <HeaderStyle CssClass="Gridheaderstyle" />
                                        <ItemStyle BorderColor="#CCFFFF" BorderStyle="Dotted" CssClass="Editabletxtbox"/>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Current Process" FilterControlAltText="Filter CurrentProcess column"
                                        HeaderText="Current Process" UniqueName="CurrentProcess">
                                          <HeaderStyle CssClass="Gridheaderstyle" />
                                        <ItemStyle BorderColor="#CCFFFF" BorderStyle="Dotted" CssClass="Editabletxtbox" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Move_To_MA" FilterControlAltText="Filter Move_To_MA column"
                                        HeaderText="Move_To_MA" UniqueName="Move_To_MA" Visible="False">
                                          <HeaderStyle CssClass="Gridheaderstyle" />
                                        <ItemStyle BorderColor="#CCFFFF" BorderStyle="Dotted" CssClass="Editabletxtbox"/>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Submit_ID" FilterControlAltText="Filter Submit_ID column"
                                        HeaderText="Submit_ID" UniqueName="Submit_ID" >
                                          <HeaderStyle CssClass="Gridheaderstyle" />
                                        <HeaderStyle CssClass="DisplayNone" />
                                        <ItemStyle CssClass="DisplayNone Editabletxtbox" BorderColor="#CCFFFF" BorderStyle="Dotted" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Test Codes" FilterControlAltText="Filter TestCodes column"
                                        HeaderText="Test Codes" UniqueName="TestCodes" Visible="False">
                                          <HeaderStyle CssClass="Gridheaderstyle" />
                                        <ItemStyle BorderColor="#CCFFFF" BorderStyle="Dotted" CssClass="Editabletxtbox"/>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="ICD" FilterControlAltText="Filter ICD column"
                                        HeaderText="ICD" UniqueName="ICD" Visible="False">
                                          <HeaderStyle CssClass="Gridheaderstyle" />
                                        <ItemStyle BorderColor="#CCFFFF" BorderStyle="Dotted" CssClass="Editabletxtbox"/>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridButtonColumn ButtonType="PushButton" CommandName="FillQuestionSet" FilterControlAltText="Filter FillQuestionSet column"
                                        HeaderText="Fill Question Set" UniqueName="FillQuestionSet" 
                                        Text="Question Set">
                                        <HeaderStyle CssClass="DisplayNone Gridheaderstyle" />
                                        <ItemStyle CssClass="DisplayNone Editabletxtbox" BorderColor="#CCFFFF"/>
                                    </telerik:GridButtonColumn>
                                    
                                    <telerik:GridButtonColumn ButtonType="ImageButton" DataTextField="View" 
                                        FilterControlAltText="Filter View column" HeaderText="View Result" 
                                        ImageUrl="~/Resources/Down.bmp" UniqueName="View" CommandName="View">
                                          <HeaderStyle CssClass="Gridheaderstyle" />
                                        <ItemStyle BorderColor="#CCFFFF" BorderStyle="Dotted" CssClass="Editabletxtbox"/>
                                    </telerik:GridButtonColumn>
                                    
                                      <telerik:GridButtonColumn ButtonType="ImageButton" DataTextField="Print" 
                                        FilterControlAltText="Filter Print column" HeaderText="Ref" 
                                        ImageUrl="~/Resources/PrintReq.png" UniqueName="Print" CommandName="Print">
                                            <HeaderStyle CssClass="Gridheaderstyle" />
                                        <ItemStyle BorderColor="#CCFFFF" BorderStyle="Dotted" CssClass="Editabletxtbox"/>
                                    </telerik:GridButtonColumn>
                                    
                                    <telerik:GridBoundColumn DataField="FillQuestionSetsValue" 
                                        FilterControlAltText="Filter FillQuestionSetsValue column" 
                                        HeaderText="FillQuestionSetsValue" UniqueName="FillQuestionSetsValue">
                                        <HeaderStyle CssClass="DisplayNone Gridheaderstyle" />
                                        <ItemStyle CssClass="DisplayNone Editabletxtbox" BorderColor="#CCFFFF" BorderStyle="Dotted"/>
                                    </telerik:GridBoundColumn>
                                    
                                    <telerik:GridBoundColumn DataField="FilePath" 
                                        FilterControlAltText="FilePath" UniqueName="FilePath" 
                                         EmptyDataText="" Display="False">
                                          <HeaderStyle CssClass="Gridheaderstyle" />
                                         <ItemStyle BorderColor="#CCFFFF" BorderStyle="Dotted" CssClass="Editabletxtbox"/>
                                    </telerik:GridBoundColumn>
                                    
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <%--<telerik:RadButton ID="btnMoveToNextProcess" runat="server" Text="Move To Next Process">
                    </telerik:RadButton>--%>
                    <input type="button" id="btnMoveToNextProcess" class="btn-primary" runat="server" style="margin-right: 2px; font-size: 13px;width:170px;" value="Move To Next Process" />
                   <%-- <telerik:RadButton ID="btnPlan" runat="server" Text="Plan" 
                        OnClick="btnPlan_Click" onclientclicking="btnPlan_Clicking">
                    </telerik:RadButton>--%>
                    <input type="button" id="btnPlan" runat="server" class="btn-primary" value="Plan" onclick="btnPlan_Clicking();"   onserverclick="btnPlan_Click"
                                          accesskey="L" style="margin-right: 2px; font-size: 13px;text-align: center;display:none; position: relative;width:40px;"/>
                    <%--<telerik:RadButton ID="btnPrintlabel" runat="server" Text="Print label" CssClass="btn-success" OnClientClicking="StartLoadFromPatChart"
                        onclick="btnPrintlabel_Click" AccessKey="R" 
                     Style="-moz-border-radius: 3px; -webkit-border-radius: 3px;position: relative;">
                    <ContentTemplate>
                        P<span class="underline">r</span>int Label
                    </ContentTemplate>
                    </telerik:RadButton>--%>
                    <input type="button" id="btnPrintlabel" runat="server" value="Print label" class="aspresizedbluebutton" onclick="{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}" onserverclick="btnPrintlabel_Click" 
                                          accesskey="R" style="margin-right: 2px; font-size: 13px; position: relative;width:100px;"/>
                    <%--<telerik:RadButton ID="btnPrintRequsition" runat="server" Text="Print Requisition" CssClass="btn-success" OnClientClicking="StartLoadFromPatChart"
                        OnClick="btnPrintRequsition_Click" AccessKey="E" Style="-moz-border-radius: 3px; -webkit-border-radius: 3px;
                        position: relative;">
                        <ContentTemplate>
                            Print R<span class="underline">e</span>quisition
                        </ContentTemplate>
                    </telerik:RadButton>--%>
                    <input type="button" id="btnPrintRequsition" runat="server" value="Print Requisition" class="aspresizedbluebutton" onclick="{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}" onserverclick="btnPrintRequsition_Click" 
                                          accesskey="P" style="margin-right: 2px; font-size: 13px; position: relative;width:150px;"/>
                    <%--<telerik:RadButton ID="btnGenerateABN" runat="server" Text="Generate ABN" CssClass="btn-success" OnClientClicking="StartLoadFromPatChart" OnClick="btnGenerateABN_Click" AccessKey="G" 
                     Style="-moz-border-radius: 3px; -webkit-border-radius: 3px;
                    position: relative;">
                    <ContentTemplate>
                        <span class="underline">G</span>enerate ABN
                    </ContentTemplate>
                    </telerik:RadButton>--%>
                    <input type="button" id="btnGenerateABN" runat="server" value="Generate ABN" class="aspresizedbluebutton" onclick="{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}" onserverclick="btnGenerateABN_Click" 
                                          accesskey="G" style="margin-right: 2px; font-size: 13px;position: relative;width:100px;"/>
                </td>
            </tr>
        </table>
                </td>
            </tr>
        </table>
        
         <asp:Button ID="btnrefreshgrid" runat="server" Text="Button" CssClass="DisplayNone"
                onclick="btnrefreshgrid_Click" />
                <asp:Button ID="btnDisableLoad" runat="server" Text="DisableLoad" CssClass="DisplayNone"
                onclick="btnDisableLoad_Click" />
    </div>
         <asp:HiddenField ID="hdnRowIndex" runat="server" EnableViewState="false"/>
        <asp:HiddenField ID="hdnCommandField" runat="server" EnableViewState="false"/>
<asp:PlaceHolder ID="PlaceHolder1" runat="server">
     <asp:Button ID="btnDelete" style="display:none;" runat="server" Text="DisableLoad" onclick="btnDelete_Click" />

    <%-- <script src="JScripts/jquery-1.11.3.min.js" type="text/javascript"></script>
     <script src="JScripts/jquery-ui.min1.10.2.js" type="text/javascript"></script>--%>
    <script src="JScripts/jQueryAngular.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/bootstrap.min.js" type="text/javascript"></script>
     <script src="JScripts/jquery.datetimepicker.js" type="text/javascript"></script>  
    <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

    <script src="JScripts/JSImageAndLabOrder.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

    <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
   
   </asp:PlaceHolder>
           <asp:HiddenField ID="hdnForEditErrorMsg" runat="server" />
    <asp:HiddenField ID="hdnSelectedItem" runat="server" />
    <asp:HiddenField ID="hdnPaperForm" runat="server" />
    </form>
</body>
</html>
