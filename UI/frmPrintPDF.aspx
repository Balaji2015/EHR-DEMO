
<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmPrintPDF.aspx.cs" Inherits="Acurus.Capella.UI.frmPrintPDF" EnableEventValidation="false" ValidateRequest="false" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" style="max-height:540px; height:100%;">
<head runat="server">
    <style type="text/css" media="print">
        @page
        {
            size: auto;
            margin: 5mm;
        }
        .NonPrintable
        {
            display: none;
        }
        body
        {
            background-color: #FFFFFF;
            border: solid 0px black;
            margin: 20px;
        }
        iframe .img
        {
            -webkit-user-select: none;
            height: 585px;
            width: 100%;
        }
        .modal
        {
            position: fixed;
            top: 0;
            left: 0;
            background-color: White;
            z-index: 99;
            opacity: 0.8;
            filter: alpha(opacity=80);
            -moz-opacity: 0.8;
            min-height: 100%;
            width: 100%;
        }
    </style>
    <title>PDF</title>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <meta name="viewport" content="width=device-width,
         initial-scale=1" />
    <base target="_self" />
    <link href="CSS/jquery-ui.css" rel="stylesheet" />
    <link href="CSS/style.css" rel="stylesheet" type="text/css" />
    <link href="CSS/font-awesome.css" rel="stylesheet" type="text/css" />
    <link href="~/CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
</head>
<body onload="PDF_Load()" style="height:100%;">
    <form id="form1" runat="server" style="height:100%;">
    <div style="height:100%;">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <div>
            <telerik:RadTabStrip ID="RadTabStrip2" runat="server" MultiPageID="RadMultiPage1" OnClientTabSelected="RadTabStrip2_TabSelected"
            SelectedIndex="0" Width="100%" ScrollChildren="True" AutoPostBack="true" OnTabClick="RadTabStrip2_TabClick">
        </telerik:RadTabStrip>
        </div>
        <div style="max-height:520px; height:100%;">
            <telerik:RadMultiPage ID="RadMultiPage1" runat="server" Width="100%" Height="100%" SelectedIndex="0">
            <telerik:RadPageView ID="RadPageView1" runat="server" Height="100%" Width="100%">
                <iframe id="PDFLOAD" runat="server" width="100%"></iframe>
                
            </telerik:RadPageView>
        </telerik:RadMultiPage>
        </div>
        <div align="right">
                    <telerik:RadButton ID="btnprint" runat="server" AutoPostBack="false" Text="Print" OnClientClicked="btnClick" ButtonType="LinkButton" CssClass="bluebutton"
                        Style="position: relative; -moz-border-radius: 3px; -webkit-border-radius: 3px;height:23px !important; padding: 4px 12px !important; font-size:12px !important; margin-top: 10px;" >
                    </telerik:RadButton>                    
                     <telerik:RadButton ID="btnSendfax" runat="server" AutoPostBack="false" Text="Send Fax" OnClientClicked="btnFaxClick" ButtonType="LinkButton" CssClass="bluebutton"
                         Style="position: relative; -moz-border-radius: 3px; -webkit-border-radius: 3px;height:24px !important; padding: 4px 12px !important; font-size:12px !important;  margin-top: 10px;">
                    </telerik:RadButton>
                </div>
        
    </div>
    <div id="divLoading" class="modal" runat="server" style="text-align: center; display: none; position: fixed; top: 0px; left: 0px; min-height: 100%; width:100%; background-color: White; z-index: 99; opacity: 0.8;">
            <asp:Panel ID="Panel2" runat="server">
                <br />
                <br />
                <br />
                <br />
                <center>
                    <asp:Label ID="Label1" Text="" runat="server"></asp:Label></center>
                <br />
                <img src="Resources/wait.ico" title="" alt="Loading..." />
                <br />
            </asp:Panel>
        </div>
         
    <asp:HiddenField ID="SelectedItems" runat="server" />
    <asp:HiddenField ID="FaxCurrentFileName" runat="server" />
         <asp:HiddenField ID="FaxSubject" runat="server" />
        <asp:HiddenField ID="hdnScreenMode" runat="server" />
        <asp:HiddenField ID="hdnHumanName" runat="server" />
    <asp:PlaceHolder ID="PlaceHolder1" runat="server">
    <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
    <script src="JScripts/jquery-ui.min1.10.2.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/bootstrap.min.js" type="text/javascript"></script>
    <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSPrintPDF.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    </asp:PlaceHolder>
    </form>
</body>
</html>
