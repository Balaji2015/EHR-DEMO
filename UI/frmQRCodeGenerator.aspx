<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmQRCodeGenerator.aspx.cs" Inherits="Acurus.Capella.UI.frmQRCodeGenerator" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    

</head>
<body>
    <div class="container">
    <form id="form1" runat="server">
        <div>
            <asp:PlaceHolder ID="plBarCode" runat="server" />
        </div>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <script src="JScripts/jquery-1.11.3.min.js" type="text/javascript"></script>
            <script src="JScripts/jquery-ui.min1.10.2.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/bootstrap.min.js" type="text/javascript"></script>
            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            </asp:PlaceHolder>
    </form>
        </div>
    
</body>
</html>
