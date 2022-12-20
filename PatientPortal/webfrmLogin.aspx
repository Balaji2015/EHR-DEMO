<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="webfrmLogin.aspx.cs" Inherits="PatientPortal.webfrmLogin" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="aspx" %>--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en" lang="en">
<head runat="server" >
    <title>CAPELLA 5.0</title>
    <link id="Link1" runat="server" rel="shortcut icon" href="Resources/16_16.ico" type="image/x-icon" /> 
    <style type="text/css">
        .style1
        {
            width: 353px;
            height: 34px;
            position: absolute;
            left: 17px;
            top: 80px;
        }
        div#centered
        {
            background-color: #eee;
            border: 1px solid #ddd;
            position: absolute;
            left: 50%;
            top: 50%;
            width: 400px;
            height:233px;
            margin-left: -200px;
            margin-top: -150px;
        }
         .lnkbtn
        { 
        	float:left ;
        
        	}
        	.btnlogin
        	{
        		float:right ;
        	    background-color :White ;
        		border-color :#507CD1;
        		border-style :solid ;
                border-width :1px;
                font-family :Verdana ;
                font-size :0.8em;
                color :#284E98;
                
        	}
    </style>
   

</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadWindowManager EnableViewState="false" ID="WindowMngr" runat="server">
        <Windows>
            <telerik:RadWindow ID="MessageWindow" ShowContentDuringLoad="true" runat="server"
                Behaviors="Close" Title="Change Password" IconUrl="Resources/16_16.ico">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
   <telerik:RadScriptManager ID="RadScriptManager1" runat="server" EnableViewState="false">
        <Scripts>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js">
            </asp:ScriptReference>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js">
            </asp:ScriptReference>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js">
            </asp:ScriptReference>
        </Scripts>
    </telerik:RadScriptManager>
    <table>
        <tr>
            <td align="center">
                <div id="centered">
                   <table border="0" cellpadding="4" cellspacing="0" >
                                <tr>
                                    <td align="center">
                                        <table border="0" cellpadding="0" style="height: 172px; width: 357px;">
                                            <tr>
                                                <td align="center" colspan="2" style="color: White; background-color: #507CD1; font-size: 0.9em;
                                                    font-weight: bold;">
                                                    Log In
                                                </td>
                                            </tr>
                                            <tr>
                                            <td></td>
                                            <td></td>
                                            </tr>
                                            <tr>
                                            <td><asp:Label ID ="lblLoginas" runat ="server" Text ="Login As"></asp:Label></td>
                                            <td>
                                             <asp:RadioButton ID="rdbtnPatientLogin" GroupName="PatientPortalLogin" 
                                                    Text="Patient" Font-Bold="false" runat="server" AutoPostBack ="true" 
                                                    oncheckedchanged="rdbtnPatientLogin_CheckedChanged" />&nbsp;&nbsp;&nbsp;&nbsp;
                                             <asp:RadioButton ID="rdbtnRepresentativeLogin" GroupName="PatientPortalLogin" 
                                                    Text="Representative" Font-Bold="false" runat="server" AutoPostBack="true" 
                                                    oncheckedchanged="rdbtnRepresentativeLogin_CheckedChanged"  />
                                            </td>
                                            </tr>
                                            <tr>
                                            <td></td>
                                            <td></td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">E-Mail ID:</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="UserName" runat="server" Font-Size="0.8em" Width="267px"></asp:TextBox>
                                                </td>
                                            </tr>
                                             <tr>
                                            <td></td>
                                            <td></td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="Password" runat="server" Font-Size="0.8em" TextMode="Password" Width="265px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="2" style="color: Red;">
                                                    <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                                </td>
                                            </tr>
                                            <tr>
                                               <td colspan ="2">
                                                 <asp:LinkButton ID="lnkForgotPassword" runat="server" onclick="lnkForgotPassword_Click" CssClass="lnkbtn">Forgot Password?</asp:LinkButton>
                                                  <input type="button" runat="server" id="LoginButton" value ="Login" name="btnSave" onclick="PatientPortalLoginValid();" onserverclick="LoginButton_Click" class="btnlogin"/>
                                                 
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                   </table>
                </div>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hdnLocalTime" runat="server" EnableViewState="false" />
    <asp:HiddenField ID="hdnLocalDate" runat="server" EnableViewState="false" />
    <asp:HiddenField ID="hdnUniversaloffset" runat="server" EnableViewState="false" />
    <asp:HiddenField ID="hdnLocalDateAndTime" runat="server" EnableViewState="false" />
    <asp:HiddenField ID="hdnHumanID" EnableViewState="false" runat="server" />
    <asp:HiddenField ID ="hdnEmailID" runat ="server" EnableViewState ="true" />
    
<asp:PlaceHolder ID="PlaceHolder1" runat="server">
    <script src="JScripts/JSPatientPortal.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
</asp:PlaceHolder>
         <%--accessibe script start--%>
    <script> (function () {
            var s = document.createElement('script');
            var h = document.querySelector('head') || document.body;
            s.src = 'https://acsbapp.com/apps/app/dist/js/app.js';
            s.async = true;
            s.onload = function () {
                acsbJS.init({ statementLink: '', footerHtml: '', hideMobile: false, hideTrigger: false, disableBgProcess: false, language: 'en', position: 'right', leadColor: '#146FF8', triggerColor: '#146FF8', triggerRadius: '50%', triggerPositionX: 'right', triggerPositionY: 'bottom', triggerIcon: 'people', triggerSize: 'bottom', triggerOffsetX: 20, triggerOffsetY: 20, mobile: { triggerSize: 'small', triggerPositionX: 'right', triggerPositionY: 'bottom', triggerOffsetX: 20, triggerOffsetY: 20, triggerRadius: '20' } });
            };
            h.appendChild(s);
        })();

    </script>
     <%--accessibe script End--%>
    </form>
</body>
</html>
