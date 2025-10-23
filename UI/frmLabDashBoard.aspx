<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmLabDashBoard.aspx.cs" Inherits="Acurus.Capella.UI.frmLabDashBoard" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="aspx" %>
<%@ Register Assembly="TimePicker" Namespace="MKB.TimePicker" TagPrefix="MKB" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <base target="_self" />
    <title>Order Dash Board</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style>
        * {
            font-size: 11.5px;
            font-family: Microsoft Sans Serif;
        }

        .inlineStyle {
            display: inline;
        }

        .modal {
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
        .align{
            text-align: initial;
            border-left: 1px dotted #a4a4a4 !important;
            border-right: 1px dotted #a4a4a4 !important;
        }
    </style>

    <link href="CSS/style.css" rel="stylesheet" type="text/css" />
    <link href="CSS/jquery-ui.css" rel="stylesheet" />
    <link href="CSS/font-awesome.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="CSS/datetimepicker.css" />
    <link href="~/CSS/CommonStyle.css" rel="Stylesheet" type="text/css" />

</head>
<body onload="DashBoardLoad();">
    <form id="form1" runat="server">
        <div>
            
            <asp:Panel runat="server" ID="pnlOrderDashBoard" GroupingText="Order Dash Board" CssClass="LabelStyleBold">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 150px">
                            <asp:Label ID="lblProviderName" runat="server" Text="Provider Name*" mand="Yes"></asp:Label>
                        </td>
                        <td colspan="2" style="width: 300px">
                            <asp:DropDownList ID="cboProviderName" runat="server" Width="100%" CssClass="Editabletxtbox" MaxHeight="100px"></asp:DropDownList>
                        </td>
                        <td style="width: 150px">
                            <asp:CheckBox ID="chkShowActive" onclick="chkShowActiveloading()" runat="server" Text="Show All Physicians"
                                Width="100%" OnCheckedChanged="chkShowActive_CheckedChanged"
                                AutoPostBack="True" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkDateRange" runat="server"
                                onclick="return CheckedChange();" Text="Date Range"
                                AutoPostBack="True" CssClass="Editabletxtbox" />
                        </td>
                        <td>
                            <asp:Label ID="lblFromDate" runat="server" Width="100px" Text="From Date" CssClass="spanstyle"></asp:Label>
                        </td>
                        <td>
                            <input type="text" id="dtpFromDate" runat="server" value="" style="width: 88%;" class="Editabletxtbox" />

                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:Label ID="lblToDate" runat="server" Width="100px" Text="To Date" CssClass="spanstyle"></asp:Label>
                        </td>
                        <td>
                            <input type="text" id="dtpToDate" runat="server" value="" style="width: 88%;" class="Editabletxtbox" />

                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>

                        <td colspan="2" align="right">
                            <asp:Button ID="btnGenerate" runat="server" AccessKey="G" OnClientClick="Load();" OnClick="btnGenerate_Click" Text="Generate" CssClass="aspbluebutton" />
                            <asp:Button ID="btnClearAll" runat="server" AccessKey="C" OnClientClicked="Clear();" OnClick="btnClearAll_Click" Text="Clear All" CssClass="aspredbutton" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="height: 400px; width: 100%" valign="top">
                            <asp:Panel runat="server" ID="pnlDetails" GroupingText="Details" CssClass="LabelStyleBold">
                                <asp:GridView ID="grdLab" runat="server" Height="380px" Width="100%" AutoGenerateColumns="False" CellSpacing="0"
                                    GridLines="None" CssClass="Gridbodystyle">
                                    <HeaderStyle CssClass="Gridheaderstyle" />

                                    <Columns>
                                        <asp:BoundField DataField="Category" HeaderText="Category" Visible="false">
                                            <HeaderStyle CssClass="Gridheaderstyle" />
                                            <ItemStyle Width="90%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Description" HeaderText="Description" HeaderStyle-Font-Bold="true">
                                            <HeaderStyle CssClass="Gridheaderstyle align" />
                                            <ItemStyle Width="90%" CssClass="align" Font-Bold="false" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Total" HeaderText="Total" HeaderStyle-Font-Bold="true">
                                            <HeaderStyle CssClass="Gridheaderstyle align" />
                                            <ItemStyle Width="10%" CssClass="align" Font-Bold="false"/>
                                        </asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
        <asp:HiddenField ID="hdnLocalTime" runat="server" />
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">

            <script src="JScripts/jquery-1.11.3.min.js" type="text/javascript"></script>
            <script src="JScripts/jquery-ui.min1.10.2.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/bootstrap.min.js" type="text/javascript"></script>
            <script type="text/javascript" src="JScripts/jquery-1.11.3.min.js"></script>
            <script src="JScripts/JSLabDashBoard.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/jquery.datetimepicker.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/jquery-1.11.3.min.js" type="text/javascript"></script>
            <script src="JScripts/jquery-ui.min1.10.2.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/bootstrap.min.js" type="text/javascript"></script>
            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSCustomDLC.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/jquery.datetimepicker.js" type="text/javascript"></script>
        </asp:PlaceHolder>
        <div id="divLoading" class="modal" runat="server" style="text-align: center; display: none">
            <asp:Panel ID="Panel2" runat="server">
                <br />
                <br />
                <br />
                <br />
                <br />
                <img src="Resources/wait.ico" title="[Please wait while the page is loading...]"
                    alt="Loading..." />
                <br />
            </asp:Panel>
        </div>
    </form>
</body>
</html>
