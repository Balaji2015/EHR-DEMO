<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomDLCNew.ascx.cs"
    Inherits="Acurus.Capella.UI.UserControls.CustomDLCNew" %>
<%--<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<style>
    .DLCList option {
        margin-top:3px;

    }
    option:hover { background-color: #DEF; }
</style>

<meta http-equiv="Content-Type" content="text/html; charset=UTF-8"/>
<div>

    <table>

    <tr>
        <td style="margin:0px; padding:0px;position:relative">
            <asp:TextBox ID="txtDLC" runat="server" CssClass="Editabletxtbox" TextMode="MultiLine" Style="position: static;
               resize: none;cursor:default;" MaxLength="32767"
                spellcheck ="true" >                 
            </asp:TextBox>
        </td>
         <td style="margin:0px; padding:5px; width:37px;"  >

            <div class="col-6-btn" id="dvpbdropdown">

                <a runat="server" id="pbDropdown" align="centre" font-bold="false" title="Drop down">
                    <i class="fa fa-plus"></i></a>
            </div>
        </td>
      
     
    </tr>
    <tr>
        <td>
           
          <%--  <telerik:RadListBox ID="listDLC" runat="server" Style="display: none; z-index:17" Font-Bold="false"
                OnClientSelectedIndexChanged="listDLC_SelectedIndexChanged"    >
                <ButtonSettings TransferButtons="All"></ButtonSettings>
            </telerik:RadListBox>--%>


            <asp:ListBox ID="listDLC" runat="server" style="z-index:17;display:none;" Font-Bold="false" CssClass="DLCList"></asp:ListBox>
                 
        </td>
    </tr>
    <%--<telerik:RadWindowManager ID="WindowMngr" runat="server">
        <Windows>
            <telerik:RadWindow ID="MessageWindow" runat="server" Behaviors="Close" Title="Plan">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>--%>
</table>
    </div>

<asp:PlaceHolder ID="PlaceHolder1" runat="server">


    <script src="JScripts/jquery-1.11.3.min.js"></script>
    <script src="JScripts/jquery-ui.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>"></script>
    <script src="JScripts/bootstrap.min.js"></script>
    <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSCustomDLC.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
</asp:PlaceHolder>
