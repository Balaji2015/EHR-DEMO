<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmViewMessage.aspx.cs"
    Inherits="Acurus.Capella.UI.frmViewMessage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <base target="_self" />
    <style type="text/css">
        .style10
        {
            width: 376px;
        }
        .style20
        {
            height: 25px;
            width: 168px;
        }
        .style21
        {
            height: 25px;
            width: 165px;
        }
        .style22
        {
            height: 25px;
            width: 161px;
        }
        .style24
        {
            width: 91px;
        }
        .style25
        {
            width: 128px;
        }
        .style26
        {
            width: 894px;
        }
        .style27
        {
            width: 158px;
        }
    </style>
       <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
</head>
<body onload=" {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();warningmethod();}">
    <telerik:RadWindowManager ID="ModalWindowMngt" runat="server">
        <Windows>
            <telerik:RadWindow ID="ModalWndViewMessage" runat="server" VisibleOnPageLoad="false"
                Height="625px" IconUrl="Resources/16_16.ico" Width="1225px">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <form id="frmMessage" runat="server" method="get">
    <div>
        <asp:ScriptManager ID="Manager" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="update" runat="server">
            <ContentTemplate>
                <asp:Panel ID="PnlMessage" BackColor="White" runat="server">
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 100%">
                                <asp:Panel ID="PnlPatientMessage" runat="server"  GroupingText="Patient Details"
                                    CssClass="Panel LabelStyleBold">
                                    <table style="width: 100%; height: 63px;">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblPatientNamelblPatientName" Width="100px" mand="Yes"  runat="server" EnableViewState="false"
                                                    Text="Patient Name*" ></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPatientName" runat="server" Class="nonEditabletxtbox"  BorderColor="Black"
                                                    BorderWidth="1px" ReadOnly="True" Width="164px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblDOB" runat="server" Width="80px" class="Editabletxtbox" EnableViewState="false" Text="DOB"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDOB" runat="server" Class="nonEditabletxtbox" BorderColor="Black" BorderWidth="1px"
                                                    ReadOnly="True" Width="164px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblAccount" runat="server" class="Editabletxtbox" Width="80px" EnableViewState="false" Text="Acc. #"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtAccount" runat="server" Class="nonEditabletxtbox" BorderColor="Black"
                                                    BorderWidth="1px" ReadOnly="True" Width="164px">
                                                </asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblPatientStatus0" runat="server" Width="80px" class="Editabletxtbox" EnableViewState="false"
                                                    Text="Patient Status"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPatientStatus" runat="server" Class="nonEditabletxtbox" BorderColor="Black"
                                                    BorderWidth="1px" ReadOnly="True" Width="164px"></asp:TextBox>
                                            </td>
                                            <td>
                                                &nbsp;<asp:Label ID="lblPatientSex" runat="server" Width="80px" class="Editabletxtbox" EnableViewState="false"
                                                    Text="Patient Type"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPatientType" runat="server" Class="nonEditabletxtbox" BorderColor="Black"
                                                    BorderWidth="1px" ReadOnly="True" Width="164px"></asp:TextBox>
                                            </td>
                                            <td colspan="2">
                                                <asp:Button ID="btnFind" runat="server" EnableViewState="false" CssClass="aspresizedbluebutton" OnClientClick="return OpenFindPatientFrmViewMessage();"
                                                    Text="Find Patient" Width="89px" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    
                        <tr>
                            <td style="width: 100%">
                                <asp:Panel ID="PnlMessageTask" ScrollBars="Auto" BorderStyle="Ridge" runat="server" GroupingText="Patient  Message"
                                    Font-Size="Small" Width="1170px" Height="300px" CssClass="Panel LabelStyleBold">
                                    <asp:GridView ID="grdMessageTask" runat="server" CssClass="Gridbodystyle" AutoGenerateColumns="false"
                                        Width="1120px" Font-Size="Small" HeaderStyle-CssClass="Gridheaderstyle" >
                                        <Columns>
                                            <asp:BoundField DataField="Created Date And Time" HeaderStyle-Width="140px" HeaderText="Created Date And Time" />
                                            <asp:BoundField DataField="Source" HeaderStyle-Width="60px" HeaderText="Source" />
                                            <asp:BoundField DataField="SourceID" HeaderStyle-Width="60px" HeaderText="SourceID" />
                                            <asp:BoundField DataField="Message Description" HeaderStyle-Width="140px" HeaderText="Message Description" />
                                            <asp:BoundField DataField="Notes" HeaderStyle-Width="250px" HeaderText="Notes" />
                                            <asp:BoundField DataField="Priority" HeaderStyle-Width="70px" HeaderText="Priority" />
                                            <asp:BoundField DataField="Created By" HeaderStyle-Width="80px" HeaderText="Created By" />
                                            <asp:BoundField DataField="Modified By" HeaderStyle-Width="80px" HeaderText="Modified By" />
                                            <asp:BoundField DataField="Modified Date and Time" HeaderStyle-Width="140px" HeaderText="Modified Date and Time" />
                                        </Columns>
                                      
                                    </asp:GridView>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 30%">
                                            <asp:Label ID="lblGridCount" runat="server" class="Editabletxtbox" EnableViewState="false" Font-Bold="True"></asp:Label>
                                        </td>
                                        <td style="width: 70%" align="right">
                                            &nbsp;&nbsp;&nbsp;
                                            <asp:Button ID="btnViewTask" runat="server" CssClass="aspresizedbluebutton" EnableViewState="false" OnClientClick="return OpenPatienTask();"
                                                Text="View Task" Width="100px" />
                                            &nbsp;&nbsp;&nbsp;
                                            <asp:Button ID="btnAddMessage" runat="server" EnableViewState="false" CssClass="aspresizedbluebutton" OnClientClick="return OpenAddMessage();"
                                                Text="Add Message/Task" Width="154px" />&nbsp;&nbsp;&nbsp;
                                            <asp:Button ID="btnclose" runat="server" CssClass="aspresizedredbutton" EnableViewState="false" OnClientClick="return cancel();"
                                                Text="Close" Width="100px" />&nbsp;&nbsp;&nbsp;

                                            <asp:Button ID="btnGetAccNo"  runat="server" Style="margin-left: 170px;display:none !important" Width="130px"
                                                CssClass="aspbluebutton" Height="26px" OnClick="btnGetAccNo_Click" />

                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:HiddenField ID="SubscriberDOB" runat="server" EnableViewState="false" />
                <asp:HiddenField ID="SubscriberName" runat="server" EnableViewState="false" />
                <asp:HiddenField ID="SubscriberID" runat="server" EnableViewState="false" />
                <asp:HiddenField ID="Subscriberstatus" runat="server" EnableViewState="false" />
                <asp:HiddenField ID="hdnselectedscn" runat="server" EnableViewState="false" />
                <asp:HiddenField ID="hdnTotalCount" runat="server" EnableViewState="false" />
                <asp:HiddenField ID="hdnAccNo" runat="server" EnableViewState="false" />
                <asp:HiddenField ID="hdnSelectedIndex" runat="server" EnableViewState="false" />
            </ContentTemplate>
        </asp:UpdatePanel>
<asp:PlaceHolder ID="PlaceHolder1" runat="server">
    <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
        <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

        <script src="JScripts/JSLibraries.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

        <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript" enableviewstate="false"></script>
        <link href="CSS/ElementStyles.css" rel="stylesheet" type="text/css" />
        </asp:PlaceHolder>
        
    </div>
    
    </form>
</body>
</html>
