<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmWillingPatientList.aspx.cs" Inherits="Acurus.Capella.UI.frmWillingPatientList" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Willing Patient List</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
     <base target="_self"></base>
    <style type="text/css">
        .style1
        {
            height: 86px;
        }
        .style2
        {
            height: 560px;
        }
        .style3
        {
            width: 83px;
        }
        .style4
        {
            width: 229px;
        }
        .style5
        {
            width: 209px;
        }
        .style6
        {
            width: 203px;
        }
        .style7
        {
            width: 131px;
        }
        .style8
        {
            width: 1103px;
        }
    </style>
      <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="frmWillingPatientList" runat="server">
     <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
        </telerik:RadScriptManager>
    <div>
        <asp:Panel ID="Panel1" runat="server" Height="698px">
            <table style="width:100%; height: 703px;">
                <tr>
                    <td class="style1" colspan="3">
                        <asp:Panel ID="pnlAppointmentDetails" runat="server" 
                            GroupingText="Appointment Details" Height="76px">
                            <table style="width:100%; height: 52px;">
                                <tr>
                                    <td class="style3">
                                        <asp:Label ID="lblFacility" class="Editabletxtbox" runat="server" Text="Facility"></asp:Label>
                                    </td>
                                    <td class="style4">
                                        <asp:TextBox ID="txtFacility" runat="server" Width="226px" class="nonEditabletxtbox"
                                            BorderColor="Black" BorderStyle="Solid" ReadOnly="True"></asp:TextBox>
                                    </td>
                                    <td class="style5">
                                        <asp:Label ID="lblAppointmentDateandTime" runat="server" class="Editabletxtbox"
                                            Text="Appointment Date and Time"></asp:Label>
                                    </td>
                                    <td class="style6">
                                        <asp:TextBox ID="txtAppointmentDateandTime" runat="server" Width="191px" 
                                           class="nonEditabletxtbox" BorderColor="Black" BorderStyle="Solid" ReadOnly="True"></asp:TextBox>
                                    </td>
                                    <td class="style7">
                                        <asp:Label ID="lblProviderName" runat="server" class="Editabletxtbox" Text="Provider Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtProviderName" runat="server" Width="292px" 
                                             BorderColor="Black" BorderStyle="Solid" class="nonEditabletxtbox"
                                            ReadOnly="True"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td class="style2" colspan="3">
                        <asp:Panel ID="pnlWillingPatientList" runat="server" 
                            GroupingText="Willing Patient List " Height="554px">
                            <table style="width:100%; height: 577px;">
                                <tr>
                                    <td>
                                         <telerik:RadGrid ID="grdWillingPatientList" runat="server" 
                                        AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Height="479px" 
                                             Skin="Vista" CssClass="Gridbodystyle">
                                          <HeaderStyle Font-Bold="true" CssClass="Gridheaderstyle"  />
                                         <ClientSettings AllowKeyboardNavigation="True">
                                             <selecting allowrowselect="True" />
                                             <ClientEvents OnRowClick="grdWillingCancelList_RowClick" />
                                            <Scrolling AllowScroll="True" UseStaticHeaders="True" />
                                        </ClientSettings>
                                        <MasterTableView>
                                            <Columns>
                                                <telerik:GridBoundColumn DataField="AppointmentDateandTime" ItemStyle-CssClass="Editabletxtbox" EmptyDataText="" 
                                                    FilterControlAltText="Filter AppointmentDate column" 
                                                    HeaderText="AppointmentDateandTime" UniqueName="AppointmentDateandTime">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Facility" EmptyDataText=""  ItemStyle-CssClass="Editabletxtbox"
                                                    FilterControlAltText="Filter Time column" HeaderText="Facility" 
                                                    UniqueName="Facility">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Provider" EmptyDataText="" ItemStyle-CssClass="Editabletxtbox"
                                                    FilterControlAltText="Filter Day column" HeaderText="Provider" 
                                                    UniqueName="Provider">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Account#" EmptyDataText="" ItemStyle-CssClass="Editabletxtbox"
                                                    FilterControlAltText="Filter Facility column" HeaderText="Account#" 
                                                    UniqueName="Account#">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="PatientName" EmptyDataText="" ItemStyle-CssClass="Editabletxtbox"
                                                    FilterControlAltText="Filter Provider column" HeaderText="PatientName" 
                                                    UniqueName="PatientName">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Gender" EmptyDataText="" ItemStyle-CssClass="Editabletxtbox"
                                                    FilterControlAltText="Filter BlockCategory column" HeaderText="Gender" 
                                                    UniqueName="Gender">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="PatientType" ItemStyle-CssClass="Editabletxtbox"
                                                    FilterControlAltText="Filter PatientType column" HeaderText="PatientType" 
                                                    UniqueName="PatientType">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="TypeOfVisit" ItemStyle-CssClass="Editabletxtbox"
                                                    FilterControlAltText="Filter TypeOfVisit column" HeaderText="TypeOfVisit" 
                                                    UniqueName="TypeOfVisit">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="CurrentProcess" ItemStyle-CssClass="Editabletxtbox"
                                                    FilterControlAltText="Filter CurrentProcess column" HeaderText="CurrentProcess" 
                                                    UniqueName="CurrentProcess">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="EncounterID" ItemStyle-CssClass="Editabletxtbox"
                                                    FilterControlAltText="Filter EncounterID column" HeaderText="EncounterID" 
                                                    UniqueName="EncounterID">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="PhysicianID" ItemStyle-CssClass="Editabletxtbox"
                                                    FilterControlAltText="Filter PhysicianID column" HeaderText="PhysicianID" 
                                                    UniqueName="PhysicianID">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Time" ItemStyle-CssClass="Editabletxtbox"
                                                    FilterControlAltText="Filter Time column" HeaderText="Time" 
                                                    UniqueName="Time">
                                                </telerik:GridBoundColumn>
                                            </Columns>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td class="style8" align="right">
                        <asp:Button ID="btnOk" runat="server" Text="Ok" Width="68px" class="aspgreenbutton"
                            onclientclick="return WillingCancelGrid();" onclick="btnOk_Click" />
                    </td>
                    <td>
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="aspredbutton" 
                            onclientclick="return WillingCancelClose();" />
                        </td>
                </tr>
            </table>
        </asp:Panel>
    
    </div>
<asp:PlaceHolder ID="PlaceHolder1" runat="server">
    <script src="JScripts/JSEditAppointment.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
     <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

    <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

    <script src="JScripts/JSWillingonCancel.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <link href="CSS/ElementStyles.css" rel="stylesheet" type="text/css" />
    </asp:PlaceHolder>
     <asp:HiddenField ID="hdnWillingGridIndex" runat="server" />
                    <asp:HiddenField ID="hdnApptDate" runat="server" />
                    <asp:HiddenField ID="hdnFacilityName" runat="server" />
                    <asp:HiddenField ID="hdnProviderName" runat="server" />
                    <asp:HiddenField ID="hdnHumanID" runat="server" />
                    <asp:HiddenField ID="hdnEncounterID" runat="server" />
                    <asp:HiddenField ID="hdnPhysicianID" runat="server" />
                    <asp:HiddenField ID="hdnApptTime" runat="server" />
                    <br />
                    <asp:HiddenField ID="hdnOrderList" runat="server"/>
                    <asp:HiddenField ID="hdnLocalTime" runat="server"/>
    </form>
</body>
</html>
