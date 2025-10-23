<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmResetPassword.aspx.cs" Inherits="Acurus.Capella.UI.frmResetPassword" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reset Password</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
        .auto-style2 {
            width: 242px;
            height: 47px;
        }
        .auto-style3 {
            width: 132px;
            height: 47px;
        }
        .auto-style5 {
            width: 100%;
            height: 28px;
        }
        #form1 {
            width: 310px;
            height: 107%;
        }
        .auto-style6 {
            width: 132px;
            height: 28px;
        }
       
    </style>
     <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
</head>
<body bgcolor="#BFDBFF" style="width: 304px; height: 83px;" onload=" {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}">
    <form id="form1" runat="server">
    <div>
        <%--<asp:Panel ID="pnlResetPassword" runat="server" Height="75px">--%>
            <table style="width: 306px">
                <tr>
                    <td class="auto-style6" style="width:25%">
                        <asp:Label ID="lblUserName" Text="User Name" runat="server" Width="100%"></asp:Label>
                    </td>
                    <td class="auto-style5" bgcolor="White" style="width:75%">
                        <telerik:RadComboBox ID="cboUserName" runat="server" Width="100%"></telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">
                       
                    </td>
                     <td class="auto-style2" style="text-align:right">
                          <telerik:RadButton ID="btnOK" runat="server" Width="20%" Style="text-align:center;" OnClick="btnOK_Click" OnClientClicked="Ok_ClientClick" ButtonType="LinkButton" CssClass="greenbutton" 
                              Height="30px" Font-Size="13px">
                              
                         <ContentTemplate>
                                <span class="underline">O</span>k
                         </ContentTemplate>
                         </telerik:RadButton>
                         &nbsp;
                        <telerik:RadButton ID="btnCancel" Text="Cancel" runat="server" Width="65px" Style="text-align:center;" OnClientClicked="Close_ClientClick" ButtonType="LinkButton" CssClass="redbutton"
                            Height="30px" Font-Size="13px" >
                         <ContentTemplate>
                                <span class="underline">C</span>ancel
                        </ContentTemplate>

                        </telerik:RadButton>
                    </td>
                </tr>
            </table>
        <%--</asp:Panel>--%>
         <asp:HiddenField ID="hdnLocalTime" Value="" runat="server" />
    </div>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
         <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
        <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
        <script src="JScripts/JSResetPassword.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
        </asp:PlaceHolder>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    </form>
</body>
</html>
