<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmOrdersQuestionSetsBloodLead.aspx.cs" Inherits="Acurus.Capella.UI.frmOrdersQuestionSetsBloodLead" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
      <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <title>Orders Question Sets Blood Lead</title>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
</head>
<body onload="Blood();">
    <form id="form1" runat="server">
     <telerik:RadWindowManager ID="WindowMngr" runat="server">
        <windows>
            <telerik:RadWindow ID="YesNoCancelWindow" runat="server" Behaviors="Close" Title=""
                IconUrl="Resources/16_16.ico">
            </telerik:RadWindow>            
        </windows>
    </telerik:RadWindowManager>

    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <div>
        <table>
            <tr>
                <td>
                    <asp:Panel ID="Panel1" runat="server" GroupingText="Blood Lead" Font-Size="Small" CssClass="Editabletxtbox">
                        <asp:Panel ID="pblBloodLead" runat="server" BackColor="White">
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:Label ID="lblBloodLeadType" runat="server" Text="Blood Lead Type*" Width="105px"  CssClass="Editabletxtbox" mand="Yes"></asp:Label>
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="cboBloodLeadType" Runat="server" OnSelectedIndexChanged="cboBloodLeadType_SelectedIndexChanged" AutoPostBack="true" CssClass="Editabletxtbox">
                                    </telerik:RadComboBox>
                                </td>
                                <td>
                                    <asp:Label ID="lblBloodLeadPurpose" runat="server" Text="Blood Lead Purpose*" Width="120px"  CssClass="Editabletxtbox" mand="Yes"></asp:Label>
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="cboBloodLeadPurpose" Runat="server" OnSelectedIndexChanged="cboBloodLeadPurpose_SelectedIndexChanged" AutoPostBack="true" CssClass="Editabletxtbox">
                                    </telerik:RadComboBox>
                                </td>
                              </tr>
                        </table>
                        </asp:Panel>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <table width="40%">
                        <tr>
                            <td>
                                <telerik:RadButton ID="btnQuestionOk" runat="server" Text="Ok" 
                                    OnClientClicked="GetUTCTimeNew" Width="37%" onclick="btnQuestionOk_Click" ButtonType="LinkButton" CssClass="greenbutton">
                                </telerik:RadButton>                            
                            </td>
                            <td>
                                <telerik:RadButton ID="btnClearAll" runat="server" Text="Clear All" OnClientClicked="BloodLeadClearAll"
                                    Width="65%" ButtonType="LinkButton" CssClass="redbutton">
                                </telerik:RadButton>
                            </td>
                            <td>
                                <telerik:RadButton ID="btnCancel" runat="server" Text="Cancel" Width="66%" OnClientClicking="CloseQuestionSet" ButtonType="LinkButton" CssClass="redbutton">
                                </telerik:RadButton>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <asp:Button ID="btnMessageType" runat="server" Text="Button" Style="display: none" OnClientClick="CloseQuestionSet();"/>
    <asp:HiddenField ID="hdnMessageType" runat="server" />   
    <asp:HiddenField ID="hdnLocalTime" runat="server" />
     <asp:HiddenField ID="hdnYesClick" runat="server" />
    
    <asp:PlaceHolder ID="PlaceHolder1" runat="server">
        <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
            <script src="JScripts/jquery-ui.min1.10.2.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/bootstrap.min.js" type="text/javascript"></script>
    <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSImageAndLabOrder.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

    <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
   
   </asp:PlaceHolder>
    </form>
</body>
</html>
