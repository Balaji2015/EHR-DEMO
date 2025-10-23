<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmViewPatientTask.aspx.cs"
    Inherits="Acurus.Capella.UI.ViewPatientTask" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <base target="_self"></base>
    <style type="text/css">
        .style2
        {
            width: 248px;
        }
        .style11
        {
            height: 13px;
        }
        #frmViewPatientTask
        {
            width: 922px;
            height: 864px;
        }
        .style15
        {
            height: 13px;
            width: 2px;
        }
        #Div1
        {
            height: 663px;
            width: 1022px;
        }
        .style17
        {
            width: 154px;
        }
        .style18
        {
            width: 1260px;
        }
        .style20
        {
            width: 132px;
        }
        .style21
        {
            width: 138px;
        }
        .style24
        {
            width: 250px;
        }
        .style25
        {
            width: 215px;
        }
        .style26
        {
            width: 96px;
        }
        .Panel legend
        {
	        font-family: "Helvetica Neue",Helvetica,Arial,sans-serif !important;
            font-size: 13px !important;
            font-weight: bold;
        }
        .textwrap
        {
            /*word-break: break-all;*/
            white-space: pre-line;
        }
    </style>
    <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css"/>
</head>
<body onload=" {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart(); patienttaskload();}">
    <telerik:RadWindowManager ID="ModalWndw" runat="server" EnableViewState="false">
        <Windows>
            <telerik:RadWindow ID="ModalWndPatientTask" runat="server" VisibleOnPageLoad="false"
                Height="625px" IconUrl="Resources/16_16.ico" Width="1225px" EnableViewState="false">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <form id="frmViewPatientTask" name="ViewPatientTask" runat="server" method="get">
    <telerik:RadFormDecorator ID="RadDecorator" runat="server" DecoratedControls="Select" />
    <div style="height: 831px; width: 1042px;">
        <asp:ScriptManager ID="Manager" runat="server" EnableViewState="false">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="update" runat="server">
            <ContentTemplate>
                <asp:Panel ID="PnlViewPatientTask" BackColor="White" runat="server" Height="864px"
                    Width="1024px">
                    <asp:Panel ID="PnlPatientMessage" runat="server"  GroupingText="Patient Details"
                        Height="75px" Width="1038px" Style="margin-top: 0px" CssClass="Panel">
                        <table style="width: 88%; height: 63px;">
                            <tr>
                                <td class="style21">
                                    <asp:Label ID="lblPatientNamelblPatientName" runat="server" EnableViewState="false"
                                        Text="Patient Name" CssClass="Editabletxtbox"  ></asp:Label>
                                </td>
                                <td class="style25">
                                    <asp:TextBox ID="txtPatientName" runat="server" CssClass="nonEditabletxtbox" ReadOnly="True" Width="164px"></asp:TextBox>
                                </td>
                                <td class="style20">
                                    <asp:Label ID="lblDOB" runat="server" CssClass="Editabletxtbox" EnableViewState="false" Text="DOB"></asp:Label>
                                </td>
                                <td class="style24">
                                    <asp:TextBox ID="txtDOB" runat="server" CssClass="nonEditabletxtbox"
                                        ReadOnly="True" Width="164px"></asp:TextBox>
                                </td>
                                <td class="style26">
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="lblAccount" runat="server" CssClass="Editabletxtbox" EnableViewState="false" Text="Acc. #"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAccount" runat="server" CssClass="nonEditabletxtbox" ReadOnly="True" Width="164px" Style="margin-left: 0px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="style21">
                                    <asp:Label ID="lblPatientStatus0"  CssClass="Editabletxtbox" runat="server" EnableViewState="false" Text="Patient Status"></asp:Label>
                                </td>
                                <td class="style25">
                                    <asp:TextBox ID="txtPatientStatus" runat="server" CssClass="nonEditabletxtbox" ReadOnly="True" Width="164px"></asp:TextBox>
                                </td>
                                <td class="style20">
                                    &nbsp;<asp:Label ID="lblPatientSex" CssClass="Editabletxtbox" runat="server" EnableViewState="false" Text="Patient Type"></asp:Label>
                                </td>
                                <td class="style24">
                                    <asp:TextBox ID="txtPatientType" runat="server" CssClass="nonEditabletxtbox" ReadOnly="True" Width="164px"></asp:TextBox>
                                </td>
                              
                            </tr>
                        </table>
                    </asp:Panel>
                    <table style="width: 62%; height: 25px;">
                        <tr>
                            <td class="style15">
                                &nbsp;
                            </td>
                            <td class="style11">
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <br />
                                <asp:CheckBox ID="chkShowTask" runat="server" Text="Show Task Created By Me" AutoPostBack="True"
                                    OnCheckedChanged="chkShowTask_CheckedChanged" cssclass="Editabletxtbox" OnChange="patienttaskload();" />
                            </td>
                            <td class="style11">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                    <asp:Panel ID="PnlPatientDetails" BackColor="White" runat="server" GroupingText="View Task" CssClass="Panel"
                        Width="1013px" >
                        <div id="Div1" style="overflow: auto; width: 1006px">
                            <asp:GridView ID="grdPatientDetails" runat="server" EnableViewState="false" 
                                AutoGenerateColumns="False" Width="1580px" cssclass="Gridbodystyle">
                                <Columns>
                                    <asp:BoundField HeaderText="Created Date And Time" DataField="Created Date And Time"
                                        ControlStyle-Width="11000px">
                                        <HeaderStyle Width="11000px" CssClass="Gridheaderstyle" />
                                        <ItemStyle Width="11000px" />
                                        <ControlStyle Width="11000px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Source" DataField="Source" />
                                    <asp:BoundField HeaderText="Source ID" DataField="Source ID" ControlStyle-Width="5000px">
                                        <HeaderStyle Width="5000px"  CssClass="Gridheaderstyle" />
                                        <ItemStyle Width="5000px" />
                                        <ControlStyle Width="5000px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Acc. #" DataField="Acc. #" ControlStyle-Width="5000px">
                                        <HeaderStyle Width="5000px"  CssClass="Gridheaderstyle"/>
                                        <ItemStyle Width="5000px" />
                                        <ControlStyle Width="5000px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Patient Name" DataField="Patient Name" ControlStyle-Width="5000px">
                                        <HeaderStyle Width="5000px"  CssClass="Gridheaderstyle"/>
                                        <ItemStyle Width="5000px" />
                                        <ControlStyle Width="5000px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="DOB" DataField="DOB" ControlStyle-Width="13000px">
                                        <HeaderStyle Width="13000px"  CssClass="Gridheaderstyle"/>
                                        <ItemStyle Width="13000px" />
                                        <ControlStyle Width="13000px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Description" DataField="Description" ControlStyle-Width="15000px">
                                        <HeaderStyle Width="15000px"  CssClass="Gridheaderstyle"/>
                                        <ItemStyle Width="15000px" />
                                        <ControlStyle Width="15000px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Notes" DataField="Notes" ControlStyle-Width="18000px" ItemStyle-CssClass="textwrap"> <%--HtmlEncode="false"--%>
                                        <HeaderStyle Width="18000px" CssClass="Gridheaderstyle" />
                                        <ItemStyle Width="18000px"   />
                                        <ControlStyle Width="18000px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Priority" DataField="Priority" />
                                    <asp:BoundField HeaderText="Facility" DataField="Facility" ControlStyle-Width="16000px">
                                        <HeaderStyle Width="16000px"  CssClass="Gridheaderstyle" />
                                        <ItemStyle Width="16000px" />
                                        <ControlStyle Width="16000px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Assigned To" DataField="Assigned To" ControlStyle-Width="14000px">
                                        <HeaderStyle Width="14000px"  CssClass="Gridheaderstyle"/>
                                        <ItemStyle Width="14000px" />
                                        <ControlStyle Width="14000px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Created By" DataField="Created By" ControlStyle-Width="14000px">
                                        <HeaderStyle Width="14000px"  CssClass="Gridheaderstyle"/>
                                        <ItemStyle Width="14000px" />
                                        <ControlStyle Width="14000px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Modified By" DataField="Modified By" />
                                    <asp:BoundField HeaderText="Modified Date and Time" DataField="Modified Date and Time"
                                        ControlStyle-Width="15700px">
                                        <HeaderStyle Width="15700px"  CssClass="Gridheaderstyle"/>
                                        <ItemStyle Width="15700px" />
                                        <ControlStyle Width="15700px" />
                                    </asp:BoundField>
                                </Columns>
                                <SelectedRowStyle CssClass="highlight" />
                                <HeaderStyle CssClass="Gridheaderstyle" />
                            </asp:GridView>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="PnlButton" runat="server" Width="1031px">
                        <table style="width: 1016px; height: 60px">
                            <tr>
                                <td class="style18">
                                    &nbsp;
                                </td>
                                <td class="style2">
                                    <asp:Button ID="btnViewMessages" runat="server" EnableViewState="false" OnClientClick="return OpenViewMessage();"
                                        Style="margin-left: 170px" Text="View  Messages" Width="130px" Height="26px" CssClass="aspresizedbluebutton" />
                                </td>
                                <td class="style17">
                                    <asp:Button ID="btnaddMessages" runat="server" OnClientClick="return AddMessage();"
                                        Text="Add Task" Height="26px" OnClick="btnaddMessages_Click" Width="143px" CssClass="aspresizedgreenbutton" />
                                </td>
                                <td>
                                    <asp:Button ID="btnClose" runat="server" EnableViewState="false" OnClientClick="return cancel();"
                                        Text="Close" Width="130px" Height="26px" CssClass="aspresizedredbutton" />
                                </td>
                                <td>

                                    <asp:Button ID="btnGetAccNo" runat="server" Style="margin-left: 170px;display: none !important;" Width="130px"
                                        CssClass="aspresizedbluebutton" Height="26px" OnClick="btnGetAccNo_Click"  />

                                 
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:Panel>
                <asp:HiddenField ID="hdnAccNo" runat="server" EnableViewState="false" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:HiddenField ID="hdnacc" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnSelectedScreen" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnParentscreen" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnEncounterId" runat="server" EnableViewState="false" />
<asp:PlaceHolder ID="PlaceHolder1" runat="server">
    <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
    <script src="JScripts/JSEditAppointment.js"></script>

        <script src="JScripts/JSLibraries.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

        <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

        <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

        <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
        
        </asp:PlaceHolder>
        <br />
    </div>
    </form>
 
</body>
</html>
