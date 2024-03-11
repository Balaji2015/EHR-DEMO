<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmLandingScreen.aspx.cs" Inherits="Acurus.Capella.UI.frmLandingScreen" EnableEventValidation="true" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="CSS/font-awesome.4.4.0.css" rel="stylesheet" />
    <script src="JScripts/jquery-1.11.3.min.js" type="text/javascript"></script>
    <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <button id="hdnbtnLogin" runat="server" style="display: none;" onserverclick="hdnbtnLogin_Click">hdnLogin</button>
            <%--<asp:HiddenField ID="hdnLocalTime" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnLocalDate" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnUniversaloffset" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnLocalDateAndTime" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnFollowsDayLightSavings" runat="server" Value="false" />--%>
            <asp:HiddenField ID="hdnGroupId" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnPersonName" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnVersion" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnProjectName" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnreportPath" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnLoginheader" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnVersionKey" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnServiceLink" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="HiddenField1" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnEvProjectName" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnReportPathhttp" runat="server" EnableViewState="false" />

            <asp:HiddenField ID="hdnroleLanding" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnRCopia_User_NameLanding" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnIs_RCopia_Notification_RequiredLanding" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnPhysician_Library_IDLanding" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnLanding_Screen_IDLanding" runat="server" Value="false" />
            <asp:HiddenField ID="hdnEMailAddress" runat="server" Value="false" />

            <link href="CSS/bootstrap.min3.1.1.css" rel="stylesheet" />
            <script src="JScripts/bootstrap.min3.1.1.max.js" type="text/javascript"></script>
            <script src="JScripts/JSLandingScreen.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"]%>"></script>
            <script src="JScripts/jsErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"]%>"></script>
        </div>
    </form>
</body>
</html>