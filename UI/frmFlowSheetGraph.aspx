<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmFlowSheetGraph.aspx.cs"
    Inherits="Acurus.Capella.UI.frmFlowSheetGraph" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<base target="_self" />
<head runat="server">
      <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <title>Flow Sheet Graph</title>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
        body {
            zoom: 1.0 !important;
            -moz-transform: scale(1) !important;
            -moz-transform-origin: 0 0 !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table style="width: 94%; height: 575px;">
                <tr>
                    <td style="width: 30%">
                        <asp:Panel ID="pnlCategory" runat="server" GroupingText="Category" Height="535px"
                            cssClass="spanstyle" Width="100%">
                            <telerik:RadListBox ID="chkDataitems" runat="server" Width="100%" AutoPostBack="true"
                                CheckBoxes="true" SelectionMode="Multiple" OnClientSelectedIndexChanging="OnClientItemSelectedIndexChanging"
                                OnSelectedIndexChanged="chkDataitems_SelectedIndexChanged" OnClientItemChecked="OnClientItemChecked"
                                OnItemCheck="chkDataitems_ItemCheck">
                                <ButtonSettings TransferButtons="All" />
                            </telerik:RadListBox>
                        </asp:Panel>
                    </td>
                    <td style="width: 70%">
                        <telerik:RadChart ID="chrtFlowSheet" runat="server" Height="600px" Width="800px"
                            DefaultType="Line" Skin="LightBlue">
                            <Appearance>
                                <FillStyle MainColor="240, 252, 255">
                                </FillStyle>
                                <Border Color="182, 224, 249" />
                            </Appearance>
                            <Legend>
                                <Appearance Corners="Round, Round, Round, Round, 6">
                                    <ItemTextAppearance TextProperties-Color="Black" AutoTextWrap="True">
                                    </ItemTextAppearance>
                                    <Border Color="208, 237, 255" />
                                </Appearance>
                            </Legend>
                            <PlotArea XAxis-AxisLabel-TextBlock-Text="Captured/Collected Date & Time">
                                <EmptySeriesMessage Visible="True">
                                    <Appearance Visible="True">
                                    </Appearance>
                                </EmptySeriesMessage>
                                <XAxis>
                                    <AxisLabel>
                                        <TextBlock Text="Captured/Collected Date &amp; Time">
                                            <Appearance TextProperties-Color="Black"></Appearance>
                                        </TextBlock>

                                    </AxisLabel>
                                </XAxis>

                            </PlotArea>
                            <ChartTitle>
                                <Appearance>
                                    <FillStyle>
                                    </FillStyle>
                                </Appearance>
                                <TextBlock>
                                </TextBlock>
                            </ChartTitle>
                        </telerik:RadChart>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td align="right">
                        <telerik:RadButton ID="btnPrintChart" runat="server" Text="Print Chart" OnClick="btnPrintChart_Click" ButtonType="LinkButton" CssClass="bluebutton teleriknormalbuttonstyle">
                        </telerik:RadButton>
                    </td>
                </tr>
            </table>
        </div>
        <telerik:RadScriptManager ID="scriptMngr" runat="server">
        </telerik:RadScriptManager>
        <asp:HiddenField ID="SelectedItem" runat="server" />
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <link href="CSS/ElementStyles.css" rel="stylesheet" type="text/css" />
            <script src="JScripts/jquery-1.11.3.min.js" type="text/javascript"></script>
            <script src="JScripts/jquery-ui.min1.10.2.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/bootstrap.min.js" type="text/javascript"></script>
            <script src="JScripts/JSChart.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
        </asp:PlaceHolder>
        <script type="text/javascript">
            Telerik.Web.UI.RadListBox.prototype.saveClientState = function () {
                return "{" +
                            "\"isEnabled\":" + this._enabled +
                            ",\"logEntries\":" + this._logEntriesJson +
                           ",\"selectedIndices\":" + this._selectedIndicesJson +
                           ",\"checkedIndices\":" + this._checkedIndicesJson +
                           ",\"scrollPosition\":" + Math.round(this._scrollPosition) +
                       "}";
            }
        </script>
    </form>
</body>
</html>
