<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmOrders.aspx.cs" Inherits="Acurus.Capella.UI.frmOrders"  ValidateRequest="false"  %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Orders</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
</head>
      <link href="~/CSS/CommonStyle.css" rel="Stylesheet" type="text/css" />
<body>
    <form id="form1" runat="server" Height="950px" >
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <telerik:RadWindowManager ID="WindowMngr" runat="server">
        <Windows>
            <telerik:RadWindow ID="MessageWindow" runat="server" Behaviors="Resize,Move,Close"
                IconUrl="Resources/16_16.ico">
            </telerik:RadWindow>
            </Windows>
            </telerik:RadWindowManager>
    <div>
        <telerik:RadTabStrip ID="tabOrders" runat="server" Width="100%" 
            SelectedIndex="0"  MultiPageID="mulPageOrders" ScrollChildren="True" 
            ontabclick="tabOrders_TabClick" 
            onclienttabselecting="tbOrders_TabSelecting">
            <Tabs>
                <telerik:RadTab runat="server" Text="Diagnostic order" IsBreak=false 
                    PageViewID="pgViewDiagnosticorder" Selected="True" >
                </telerik:RadTab>
                <telerik:RadTab runat="server" Text="Referral Order" 
                    PageViewID="pgViewReferralOrder" IsBreak=false  CssClass="tabstyle">
                </telerik:RadTab>
                <telerik:RadTab runat="server" Text="Immunization/Injection" 
                    PageViewID="pgViewImmunizationInjection" Height="500px"  CssClass="tabstyle">
                </telerik:RadTab>
                <telerik:RadTab runat="server" Text="Procedures" PageViewID="pgViewProcedures"  IsBreak=false  CssClass="tabstyle">
                </telerik:RadTab>
            </Tabs>
        </telerik:RadTabStrip>
        <telerik:RadMultiPage ID="mulPageOrders" runat="server" Width="100%" 
            SelectedIndex="0">
            <telerik:RadPageView ID="pgViewDiagnosticorder" runat="server" Height="650px" >
                RadPageView</telerik:RadPageView>
                <telerik:RadPageView ID="pgViewReferralOrder" runat="server" 
                Height="650px">
                RadPageView</telerik:RadPageView>
                <telerik:RadPageView ID="pgViewImmunizationInjection" runat="server" 
                Height="650px" >
                RadPageView</telerik:RadPageView>
                <telerik:RadPageView ID="pgViewProcedures" runat="server" Height="650px">
                RadPageView</telerik:RadPageView>
        </telerik:RadMultiPage>
    </div>
<asp:HiddenField id="hdnIsSaveEnableOrders" Value="false" runat="server" />
<asp:PlaceHolder ID="PlaceHolder1" runat="server">
<script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
<script src="JScripts/JSOrders.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
<script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

 </asp:PlaceHolder>
    </form>
</body>
</html>
