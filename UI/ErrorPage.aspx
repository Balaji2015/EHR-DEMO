<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="ErrorPage.aspx.cs" Inherits="Acurus.Capella.UI.ErrorPage" Trace="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

      <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <title>Error</title>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
        .* {
            /*font-size: small;*/
            /*font-family: Microsoft Sans Serif;*/
            top: 0px;
            left: 0px;
            -webkit-box-sizing: border-box; /* Safari/Chrome, other WebKit */
            -moz-box-sizing: border-box; /* Firefox, other Gecko */
            margin: 0px;
            padding: 0px;
        }
    </style>
    <link href="CSS/bootstrap.min.css" rel="stylesheet" />
    <script src="JScripts/jquery-1.11.3.min.js" type="text/javascript"></script>
</head>

<body onload="myFunction()">
    <form id="form1" runat="server">
        <div>
            <span style="font-family:Calibri;font-size:24px;font-weight:bold">
                Error
               </span>
            <br />
                <asp:Label ID="friendlyErrorMsg" runat="server" Text="Label" Style="font-family:Calibri !important;font-size:15px" ></asp:Label>
            <asp:Panel ID="detailedErrorPanel" runat="server" Visible="false">
                <br />

                <%--<p>
                Detailed Error:
                <br />
                <asp:Label ID="errorDetailedMsg" runat="server" Font-Bold="true" Font-Size="Large" /><br />
            </p>
            <p>
                Error Handler:
                <br />
                <asp:Label ID="errorHandler" runat="server" Font-Bold="true" Font-Size="Large" /><br />
            </p>--%>
                
                    <asp:Label ID="innerMessage" runat="server"  style="font-family:Calibri;font-size:15px"/><br />
               
              <%--  <pre style="border: none;">
                  <asp:Label ID="innerTrace" runat="server" Visible="False" />
            </pre>--%>
                <%--<p>
                Date and Time:
                <br />
                <asp:Label ID="dateandTime" runat="server" Font-Bold="true" Font-Size="Large" /><br />
            </p>--%>
            </asp:Panel>
            
            <asp:Label ID="Labelmsg" runat="server"  style="font-family:Calibri;font-size:21px;color:red;font-weight:bold;"/>
             <p style="height:20px"></p>
            <div style="width: 100%; align-items: center;">
                    <table>
                    <tr style="width: 100%;">
                        <td style="width: 40%;"></td>
                        <td style="width: 10%;">
                             <asp:Button ID="btnLogin" runat="server" CssClass="btn btn-primary" OnClick="btnLogin_Click"
                    Text="Click here to Login" Visible="false"/>
                        </td>
                        <td style="width: 40%;"></td>
                    </tr>
                </table>
               
            </div>
        </div>
    </form>

    <script type="text/javascript">
        function myFunction() {
            jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading .bg').height('100%');
            jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').fadeOut(300);
            jQuery(top.window.parent.parent.parent.parent.document.body).css('cursor', 'default');
            if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block')
                jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').remove();
        }
    </script>
</body>
</html>
