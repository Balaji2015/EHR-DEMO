<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmImpersonateUser.aspx.cs" Inherits="Acurus.Capella.UI.frmImpersonateUser" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login As</title>
     <link href="~/CSS/CommonStyle.css" rel="Stylesheet" type="text/css" />
      

</head>
<body>
    
    <form id="frmImpersonateUser" runat="server">
        <div>
            <table>
               <tr style="height:10px"></tr>
                <tr>
                    <td><span class="MandLabelstyle">User Name</span><span class="manredforstar">*</span></td>
                    <td><select id="cboUserName" runat="server" style="width:235px; height:20px;"></select></td>
                </tr>
                <tr style="height:10px"></tr>
               <tr>
                   <td><span class="MandLabelstyle">Password</span><span class="manredforstar">*</span></td>
                   <td><input type="password" id="txtPassword" runat="server" style="width:228px; height:16px;" /></td>
               </tr>
                <tr style="height:12px"></tr>
               <tr>
                   <td></td>
                   <td colspan="2" align="right" >
                       <input type="button" id="btnOk" class="btn aspbluebutton" style="width:110px" onclick="if (!CheckUserAndPassword()) { return false; };" onserverclick="btnOk_ServerClick" runat="server" value="OK" />
                  
                       <input type="button" id="btnClose" class="aspredbutton" style="width:110px" onclick="CloseWindow();" runat="server" value="Close"/>
                   </td>
               </tr>

            </table>
             <button id="hdnbtnLogin" runat="server" style="display: none;" onserverclick="hdnbtnLogin_ServerClick">hdnLogin</button>
        <button id="hdnMultiUserLogin" runat="server" style="display: none;"  onserverclick="hdnMultiUserLogin_ServerClick">hdnMultiUserLogin</button>
        <button id="hdnbtnLogOutAndLogIn" runat="server" style="display: none;" onserverclick="hdbbtnLogOutAndLogIn_ServerClick" >hdnMultiUserLogin</button>
        
            <asp:HiddenField ID="hdnLocalTime" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnLocalDate" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnUniversaloffset" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnLocalDateAndTime" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnFollowsDayLightSavings" runat="server" Value="false" />
        <asp:HiddenField ID="hdnroleLanding" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnRCopia_User_NameLanding" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnIs_RCopia_Notification_RequiredLanding" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnPhysician_Library_IDLanding" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnLanding_Screen_IDLanding" runat="server" Value="false" />
            <asp:HiddenField ID="hdnEMailAddress" runat="server" Value="false" />
            <asp:HiddenField ID="hdnPersonName" runat="server" EnableViewState="false" />


            <asp:HiddenField ID="hdnVersion" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnProjectName" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnreportPath" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnLoginheader" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnVersionKey" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnServiceLink" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="HiddenField1" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnEvProjectName" runat="server" EnableViewState="false" />  <asp:HiddenField ID="hdnReportPathhttp" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnGroupId" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnFacltyName" runat="server" EnableViewState="false" />

        
            <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <script src="JScripts/jquery-1.11.3.min.js" type="text/javascript"></script>
        <script src="JScripts/bootstrap.min.js" type="text/javascript"></script>   
    <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSImpersonateUser.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
        </asp:PlaceHolder>
        </div>
        
       

        
    </form>
</body>
</html>
