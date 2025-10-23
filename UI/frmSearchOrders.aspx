<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmSearchOrders.aspx.cs"
    Inherits="Acurus.Capella.UI.frmSearchOrders" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<base target="_self" />
<head runat="server">
      <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <title>Search Orders</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
        .style1
        {
            width: 92px;
        }
        .style2
        {
            width: 24%;
        }
        .style3
        {
            width: 15%;
        }
        .noWrapText
        {
            white-space: nowrap;
            line-height: 30px;     
        }
        #lstvFrequentlyUsedLabProcedures > tbody > tr > td > span
        {
            display: block;
            white-space: nowrap;
            padding: 0px;
        }
        #lstvFrequentlyUsedLabProcedures > tbody > tr > td > span[IsHeader=true]
        {
            color: Blue;
            font-weight: bold;
        }
        #InvisibleButton
        {
        	visibility:hidden;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
     <telerik:RadWindowManager ID="WindowMngr" runat="server">
        <Windows>
            <telerik:RadWindow ID="MessageWindow" runat="server" Modal="true" Behaviors="Close" Title="Manual Result Entry"
                IconUrl="Resources/16_16.ico" Width="100%" Height="100%">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <telerik:RadScriptManager ID="scriptMngr" runat="server">
        <Scripts>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
        </Scripts>
    </telerik:RadScriptManager>
    <div>
        <asp:Panel ID="pnlSearch" runat="server" Width="70%" Style="margin-bottom: 0px;"
            CssClass="Panel" Height="628px">
            <table style="width: 99%">
                <tr>
                    <td style="width: 15%">
                      <asp:Label ID="lblSelectPhysician" runat="server" Text="Select Physician*" CssClass="MandLabelstyle"> 
                      <%--   <span runat="server" class="manredforstar" id="spanPhysicianstar">*</span> --%>
                        </asp:Label>
                       
                    </td>
                    <td class="style2">
                        <telerik:RadComboBox ID="cboPhysician" runat="server" Height="16px" Width="213px"
                            MaxHeight="100px" CssClass="spanstyle">
                        </telerik:RadComboBox>
                    </td>
                    <td class="style3">
                        <asp:CheckBox ID="chkShowAll" runat="server" Text="Show All Physician" AutoPostBack="true"
                            OnCheckedChanged="chkShowAll_CheckedChanged" />
                    </td>
                    <td class="style1">
                        <asp:Label ID="lblLabName" runat="server" Text="Lab Name*" CssClass="MandLabelstyle">       
                        </asp:Label>
                       
                    </td>
                    <td style="width: 15%">
                        <telerik:RadComboBox ID="cboLab" runat="server" MaxHeight="70px" CssClass="spanstyle">
                        </telerik:RadComboBox>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadButton ID="btnGetProcedures" runat="server" Text="Get Procedure" OnClick="btnGetProcedures_Click" CssClass="bluebutton" ButtonType="LinkButton"
                            width="96px" Height="12px" Font-Size="12px">
                        </telerik:RadButton>
                    </td>
                </tr>
                <tr>
                    <td colspan="6" style="width: 90%">
                        <asp:Panel ID="pnlSelectProcedures" runat="server" GroupingText="Select Procedures"
                            Width="100%" Height="580px" CssClass="LabelStyleBold">
                            <table style="width: 100%; height: 539px;">
                                <tr>
                                    <td>
                                        <asp:Panel ID="containerFrequentlyUsedProcedures" runat="server" Width="807px" Height="491px"
                                            ScrollBars="Horizontal">
                                            <asp:CheckBoxList ID="lstvFrequentlyUsedLabProcedures" runat="server" Height="500px"
                                                CssClass="noWrapText Editabletxtbox" onclick="SelectItemsUnderHeader(this);" OnPreRender="chklstFrequentlyUsedProcedures_PreRender">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="btnAllProcedures" runat="server" Text="All Procedures"  
                                            CssClass="aspresizedbluebutton" OnClientClick="return btnAllProcedures_Clicked();" />
                                        <%--<Jira #CAP-591>--%>
                                   <asp:Button ID="btnOk" runat="server" Text="OK" CssClass="aspresizedgreenbutton" OnClick="btnOk_Click" />

                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="aspresizedredbutton" OnClientClick="return CloseOrders();" />

                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                         <%--<Jira #CAP-591>--%>    
                        <%--<asp:Panel ID="pnlButtons" runat="server" Width="100%">
                            <table width="100%">
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="btnOk" runat="server" Text="OK" CssClass="aspresizedgreenbutton" OnClick="btnOk_Click" />
                                        
                                    </td>
                                    <td align="right" style="width: 5%">
                                      <%--  <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClientClick="CloseOrders();"
                                            Width="100%" />--%>
                                      <%--<asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="aspresizedredbutton" OnClientClick="return CloseOrders();" />--%>
                                    <%--</td>--%>
                                <%--</tr>--%>
                            <%--</table>--%>
                        <%--</asp:Panel>--%>
                    </td>
                </tr>
            </table>
        </asp:Panel>
<asp:PlaceHolder ID="PlaceHolder1" runat="server">
        <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

        <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

        <script src="JScripts/JSMRE.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

        <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
       <%-- <link href="CSS/ElementStyles.css" rel="stylesheet" type="text/css" />--%>
        </asp:PlaceHolder>
        <asp:HiddenField ID="hdnScreenMode" runat="server" />
        <asp:HiddenField ID="hdnLabName" runat="server" />
        <asp:HiddenField ID="hdnLabID" runat="server" />
        <asp:HiddenField ID="hdnSelectedItem" runat="server" />
        <asp:HiddenField ID="hdnPhyID" runat="server" />
        <asp:HiddenField ID="hdnTransferVaraible" runat="server" />
        <asp:HiddenField ID="hdnMessageType" runat="server" />
        <asp:Button ID="InvisibleButton" runat="server" 
            onclick="InvisibleButton_Click" />
        <asp:Button ID="btnMessageType" runat="server" Text="Button" Style="display: none" OnClientClick="return CloseOrders();" OnClick="btnOk_Click"/>
    </div>
    </form>
</body>
</html>
