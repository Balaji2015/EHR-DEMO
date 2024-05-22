<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmAddorUpdateKeywords.aspx.cs"
    Inherits="Acurus.Capella.UI.frmAddorUpdateKeywords"  ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="aspx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Add/Update Keywords</title>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
        .displayNone {
            display: none;
        }

        .gridfont {
            font-family: Arial, Helvetica, sans-serif;
            font-size: 12px;
        }

        .Headerstyle {
            font-family: Arial, Helvetica, sans-serif;
            font-size: 14px;
            text-align: center;
        }

        #form1 {
            /*background-color: #bfdbff;*/
        }

        .ui-dialog-titlebar-close {
            display: none;
        }

        .ui-widget {
            font-family: Verdana,Arial,sans-serif !important;
        }

        .ui-dialog .ui-dialog-buttonpane .ui-dialog-buttonset {
            float: none !important;
            margin-left: 45px !important;
        }

        .ui-dialog .ui-dialog-buttonpane button {
            width: 60px !important;
        }

        .ui-dialog .ui-dialog-titlebar {
            padding: 0px !important;
        }

        .ui-dialog .ui-dialog-title {
            font-size: 12px !important;
            font-family: Verdana,Arial,sans-serif !important;
        }

        .ui-dialog ui-widget ui-widget-content ui-corner-all ui-front ui-dialog-buttons ui-draggable ui-resizable {
            height: 155px;
            border: 2px solid;
            border-radius: 13px;
            top: 504px !important;
            left: 568px !important;
        }

        .ui-dialog .ui-dialog-content {
            min-height: 0px !important;
        }

        .ui-dialog .ui-dialog-buttonpane {
            margin-top: -10px !important;
            /*padding: 0px !important ;*/
        }

        .ui-widget-content {
            border: 0px !important;
        }

        .ui-widget-header, .ui-state-default, ui-button {
            font-weight: bold !important;
            font-size: 12px !important;
            font-family: sans-serif;
        }


        .ui-widget {
            border: 1px solid #adadad !important;
            background-color: #F7F7F7;
        }
    </style>

    <base target="_self" />
      <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <link href="CSS/jquery-ui.css" rel="Stylesheet" />
    <link href="CSS/bootstrap.min.css" rel="Stylesheet" />

</head>
<body>
    <form id="form1" runat="server" defaultbutton="btnAdd">
        <asp:UpdatePanel ID="UpdatePanel" runat="server">
            <ContentTemplate>
                <aspx:ToolkitScriptManager ID="RadScriptManager1" runat="server" EnableViewState="false" >
                </aspx:ToolkitScriptManager>

        <asp:Panel ID="pnlAddOrUpdateKeyword" runat="server" Height="100%" >
            <table style="width: 100%; height: 100%" class="Editabletxtbox">
                <tr>
                    <td colspan="2" valign="bottom" align="right" style="height: 20px; width: 100%;">
                        <button id="btnClose" type="button" class="aspredbutton" onclick=" btnClose_Clicked();" style="height: 30px !important ; font-size: 14px !important;">Close</button>&nbsp;&nbsp;</td>
                </tr>
                <tr style="height:10px">

                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblKeyword" Width="150px" Text="Keyword" CssClass="Editabletxtbox"></asp:Label>
                    </td>
                    <td style="height: 60px">
                        <asp:TextBox runat="server" ID="txtKeyword" Width="565px" Height="55px"
                            CssClass="textarea" TextMode="MultiLine" onkeypress="btnAddEnabled();" onkeydown="btnAddEnabled();" onkeyup="btnAddEnabled();"
                            onpaste="btnAddEnabled();" EnableViewState="false"
                            Font-Names="Microsoft Sans Serif" Font-Size="Small" style = "resize:none"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblDesc" Visible="false" Text="Description"></asp:Label>
                    </td>
                    <td id="tddesc" runat="server">
                        <asp:TextBox runat="server" ID="txtDesc" Visible="false" Width="565px" Height="55px"
                            CssClass="textarea" TextMode="MultiLine" onkeyup="btnAddEnabled();"
                            onpaste="btnAddEnabled();"
                            Font-Names="Microsoft Sans Serif" Font-Size="Small" style = "resize:none"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="height: 35px;" colspan="2">
                        <table style="width: 100%">
                            <tr>
                                <td valign="bottom" align="left">
                                    <asp:Label runat="server" ID="lbltotal" Font-Bold="true" Width="140px" CssClass="Editabletxtbox"></asp:Label></td>
                              
                                <td valign="bottom" align="right">
                                   <asp:Button ID="btnAdd" runat="server" Text="Add" OnClientClick="btnAdd_ClientClicked" OnClick="btnAdd_Click" CssClass="aspgreenbutton" 
                                       style="height: 28px !important ; font-size: 12px !important ; width: 63px; margin-right: -12px !important; "/> &nbsp; &nbsp;
                                    <asp:Button ID="btnClearAll" runat="server" Text="Clear All" AutoPostBack="false" 
                                        OnClientClick="btnClearAll_ClientClicked()" CssClass="aspredbutton" style="height: 28px !important ; font-size: 12px !important"/> &nbsp;&nbsp;
                                </td>
                            </tr>
                        </table>
                        <%----%>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" valign="top" align="right">
                        <asp:Panel ID="pnlAddUpdate" runat="server" Height="420px" Width="100%" ScrollBars="Vertical" CssClass="Editabletxtbox">
                            <asp:GridView ID="grdAddOrUpdateKeyword" runat="server" AutoGenerateColumns="False"
                                OnRowCommand="grdAddOrUpdateKeyword_RowCommand"
                                BorderStyle="None" BorderWidth="1px" CellPadding="3" Height="0px" Width="100%"
                                OnRowDataBound="grdAddOrUpdateKeyword_RowDataBound" CssClass="Gridbodystyle">

                                <RowStyle ForeColor="#000066"  CssClass="Gridtrevenstyle"/>
                                <HeaderStyle CssClass="Gridheaderstyle" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Edit" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="4%">
                                        
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnEdit" OnClientClick="enabledfunction();" runat="server" ItemStyle-HorizontalAlign="Center" CommandName="EditC" Text="Edit" ImageUrl="~/Resources/edit.gif" ToolTip="Edit" />
                                        </ItemTemplate>
                                       
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Copy" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="4%" >
                                        
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnCopy" runat="server" OnClientClick="enabledfunction();" ImageAlign="Middle" CommandName="CopyC" Text="Copy" ImageUrl="~/Resources/Copy2.png" ToolTip="Copy" />
                                        </ItemTemplate>
                                     
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Del" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="4%" >
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnDel" runat="server" OnClientClick="return gridCellDeleteClick(this);" CommandName="Delc" Text="Del" ItemStyle-HorizontalAlign="Center" ImageUrl="~/Resources/close_small_pressed.png" ToolTip="Delete" />
                                        </ItemTemplate>
                                     
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Keyword" HeaderText="Keyword">
                                        <ItemStyle ForeColor="Black" />
                                       
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Description" HeaderText="HPI" Visible="False" HeaderStyle-Width="21%">
                                        <ItemStyle ForeColor="Black" />
                                       
                                    </asp:BoundField>
                                    <asp:BoundField DataField="UserLookupID" HeaderText="UserLookupID" HeaderStyle-CssClass="displayNone"
                                        ItemStyle-CssClass="displayNone">
                                        <HeaderStyle CssClass="displayNone" />
                                        <ItemStyle CssClass="displayNone" />
                                    </asp:BoundField>
                                </Columns>
                                <FooterStyle BackColor="White" ForeColor="#000066" />
                                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                <SelectedRowStyle BackColor="#A9A9A9" Font-Bold="True" ForeColor="White" />
                              
                            </asp:GridView>

                            <div style="height: 5px" align="right"></div>

                            

                        </asp:Panel>
                    </td>
                </tr>
                

            </table>
        </asp:Panel>
                <asp:Button ID="btnDelete" runat="server" CssClass="displayNone" OnClick="btnDelete_Click" />
        <asp:HiddenField ID="hdmRowIndex" runat="server" />
        <asp:HiddenField ID="hdneditid" runat="server" />
        <asp:HiddenField ID="hdnMessageType" runat="server" />
        <asp:HiddenField ID="hdndelid" runat="server" />
        <asp:HiddenField ID="hdnEnableYesNo" runat="server" />
        <asp:Button ID="btnMessageType" runat="server" Text="Button" Style="display: none" OnClientClick="btnClose_Clicked();" />
        <asp:HiddenField ID="upadeID" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="isGridDelete" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="isClearAll" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnMaxSortOrder" runat="server" />
        <asp:Button ID="btnClear" runat="server" CssClass="displayNone" OnClick="btnClear_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>
        
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <script src="JScripts/jquery-1.11.3.min.js" type="text/javascript"></script>
            <script src="JScripts/jquery-ui.min1.10.2.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/bootstrap.min.js" type="text/javascript"></script>
            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSAddorUpdateKeywords.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
        </asp:PlaceHolder>
    </form>
</body>
</html>
