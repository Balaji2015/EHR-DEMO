<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmACOValidation.aspx.cs"
    Inherits="Acurus.Capella.UI.frmACOValidation" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ACO Validation</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
<style type="text/css">
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
            text-align:center;
        }
        *
        {
        	font-family: Microsoft Sans Serif;
        	font-size: small;
        }
        .Unwwrap
        {
        	white-space:nowrap;
        }
        legend
        {
            font-weight: bold;
        }
</style>
    <script type="text/javascript">
   
    
    </script>

</head>
<body onload=" {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}">
    <form id="form1" runat="server" style="width: 500px; height:100%">
    <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <telerik:RadWindowManager ID="WindowMngr" runat="server" EnableViewState="false">
        <Windows>
            <telerik:RadWindow ID="MessageWindow" runat="server" Behaviors="Close" Title="ACO Validaion" EnableViewState="false"
                IconUrl="Resources/16_16.ico">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <div>
        <asp:Panel runat="server" ID="pnlVerifyPFSH" GroupingText="Verify PFSH">
            <table>
                <tr>
                    <td>
                        You have to verify PFSH before continuing.
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chkPFSHVerified" runat="server" Text="I have verified PFSH for this patient"
                            AutoPostBack="True" OnCheckedChanged="chkPFSHVerified_CheckedChanged" EnableViewState="false"/>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlACOParticipation" GroupingText="ACO Participation">
            <table runat="server">
                <tr>
                    <td colspan="2">
                        ACO Participation Discussed with patient
                    </td>
                    <td>
                        <asp:DropDownList ID="cboACODiscusion" runat="server" Width="100px" 
                            AutoPostBack="True" 
                            onselectedindexchanged="cboACODiscusion_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
               
                <tr id="tblPhysicianDetails">
                    <td>
                        <span id="spnDiscussedBy" >Discussed By</span>
                    </td>
                    <td>
                        <asp:DropDownList ID="cboPhysicianNames" runat="server" Width="200px" 
                            AutoPostBack="True" 
                            onselectedindexchanged="cboPhysicianNames_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkShowAllPhysician" runat="server" OnCheckedChanged="CheckBox1_CheckedChanged" EnableViewState="true"
                            Text="Show all Physicians" AutoPostBack="True" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <div style="position: relative; height: 20px;">
            <div style="position: absolute; right: 3px; height: 20px;">
                <asp:Button ID="btnSave" Text="Save" runat="server" OnClick="btnSave_Click" 
                    OnClientClick="return MoveToValidation();" AccessKey="s" />
                <asp:Button ID="btnCancel" Text="Cancel" runat="server" 
                    OnClick="btnCancel_Click" AccessKey="C" OnClientClick="checkAutoSave();" />
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnACOHumanID" runat="server"  EnableViewState="false"/>
    <asp:HiddenField ID="hdnHumanName" runat="server" EnableViewState="false" />
    <asp:HiddenField ID="hdnIsDirty" runat="server" Value="false" EnableViewState="false"/>
<asp:PlaceHolder ID="PlaceHolder1" runat="server">
    <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
    <script src="JScripts/JSACOValidation.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript" enableviewstate="false"></script>
    <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript" enableviewstate="false"></script>

    <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    </asp:PlaceHolder>
    </form>
</body>
</html>
