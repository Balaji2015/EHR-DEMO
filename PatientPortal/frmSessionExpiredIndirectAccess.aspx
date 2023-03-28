<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmSessionExpiredIndirectAccess.aspx.cs" Inherits="Acurus.Capella.PatientPortal.frmSessionExpiredIndirectAccess" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Session Expired</title>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
      <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <link href="CSS/bootstrap.min.css" rel="stylesheet"/>
</head>
<body onload=" {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}" style="height: 90%; width: 99%;">
    <form id="frmSessionExpired" runat="server">
        <div style="text-align: center; vertical-align: middle; padding-top: 20%; width: 480px; height: 200px; padding-left: 31%;">
            <div style="border: medium; width: 100%; height: 100%;">
                <table class="tablebordered">
                    <thead>
                        <tr>
                            <td class="divgroupstyle">
                                <h4  class="LabelStyleBold">Capella - EHR
                                </h4>
                            </td>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td style="text-align: center;">
                                <asp:Label ID="Label1" runat="server" Font-Bold="True" style="font-size:16px;"
                                    Text="You have tried to open the application without logging in" class="Editabletxtbox"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center;">
                                <asp:Button ID="btnLogin" runat="server" CssClass="btn btn-primary htmlbtnstyle" OnClick="btnLogin_Click" OnClientClick="return SessionExpired();"
                                    Text="Click here to Login" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </form>
    <asp:PlaceHolder ID="PlaceHolder1" runat="server">
        <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
        <%--<script src="JScripts/JSLogin.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>--%>

        <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
        <script type="text/javascript">
            function StopLoadFromPatChart() {
                jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading .bg').height('100%');
                jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').fadeOut(300);
                jQuery(top.window.parent.parent.parent.parent.document.body).css('cursor', 'default');
                if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block')
                    jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').remove();
            }
            function SessionExpired() {
                window.location.href = "frmLogin.aspx";
            }

        </script>
    </asp:PlaceHolder>
</body>
</html>
