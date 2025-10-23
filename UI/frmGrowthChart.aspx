<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmGrowthChart.aspx.cs"
    Inherits="Acurus.Capella.UI.frmGrowthChart" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Charting" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Growth Chart</title>
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
        }
        .loading
        {
            font-family: Arial;
            font-size: 10pt;
            border: 5px solid #67CFF5;
            width: 200px;
            height: 100px;
            display: none;
            position: fixed;
            background-color: White;
            z-index: 999;
        }
    </style>
     <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
     <script src="JScripts/bootstrap.min3.1.1.max.js" type="text/javascript"></script>
   
</head>
<body onload=" {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}">   
    <form id="form1" runat="server">
    <telerik:RadWindowManager ID="ModalWindowMngt" runat="server">
        <Windows>
            <telerik:RadWindow ID="ModalWindow" runat="server" VisibleOnPageLoad="false" Height="625px" IconUrl="Resources/16_16.ico"
                Width="1225px" >
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <div>
        <table width="100%">
            <tr>
                <td style="width: 40%">
                    <asp:Panel ID="pnlChartInformation" GroupingText="Chart Information" runat="server"
                        Width="100%" Height="400px" Font-Size="Small" CssClass="Panel LabelStyleBold" BackColor="White">
                        <div>
                            <table width="100%" style="height: 374px">
                                <tr>
                                    <td colspan="2">
                                        <asp:Panel ID="pnlChartType"  runat="server" GroupingText="Chart Type" Height="333px"
                                            Width="100%" >
                                            <table style="width: 100%; height: 308px;">
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="chkWeightAgeBirth" CssClass="Editabletxtbox" runat="server" Text="Wt./Age Birth - 36 Months"
                                                            OnCheckedChanged="chkWeightAgeBirth_CheckedChanged" AutoPostBack="true" onclick="ClearCheckBoxes('chkWeightAgeBirth','GrowthChart');" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="chkWeightLengthBirth" CssClass="Editabletxtbox" runat="server" Text="Wt./Length Birth - 36 Months"
                                                            OnCheckedChanged="chkWeightLengthBirth_CheckedChanged" AutoPostBack="true" onclick="ClearCheckBoxes('chkWeightLengthBirth','GrowthChart');" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="chkHCAgeBirth" CssClass="Editabletxtbox" runat="server" Text="HC/Age Birth - 36 Months"
                                                            OnCheckedChanged="chkHCAgeBirth_CheckedChanged" AutoPostBack="true" onclick="ClearCheckBoxes('chkHCAgeBirth','GrowthChart');" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="chkLengthAgeBirth" CssClass="Editabletxtbox" runat="server" Text="Length/Age Birth - 36 Months"
                                                            OnCheckedChanged="chkLengthAgeBirth_CheckedChanged" AutoPostBack="true" onclick="ClearCheckBoxes('chkLengthAgeBirth','GrowthChart');" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="chkWeightAge" CssClass="Editabletxtbox" runat="server" Text="Wt./Age 2-20  yrs" OnCheckedChanged="chkWeightAge_CheckedChanged"
                                                            onclick="ClearCheckBoxes('chkWeightAge','GrowthChart');" 
                                                            AutoPostBack="true" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="chkBMIAge" CssClass="Editabletxtbox" runat="server" Text="BMI/Age 2-20 yrs" OnCheckedChanged="chkBMIAge_CheckedChanged"
                                                            AutoPostBack="true" 
                                                            onclick="ClearCheckBoxes('chkBMIAge','GrowthChart');" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="chkStatureAge" CssClass="Editabletxtbox" runat="server" Text="Stature/Age 2-20  yrs" OnCheckedChanged="chkStatureAge_CheckedChanged"
                                                            AutoPostBack="true" 
                                                            onclick="ClearCheckBoxes('chkStatureAge','GrowthChart');" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="chkWeightStature" CssClass="Editabletxtbox" runat="server" Text="Wt./Stature 2-5 yrs"
                                                            OnCheckedChanged="chkWeightStature_CheckedChanged" AutoPostBack="true" onclick="ClearCheckBoxes('chkWeightStature','GrowthChart');" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>                                      
                                        <asp:Button ID="btnRefresh" CssClass="aspresizedbluebutton" runat="server" Text="Refresh Chart" Width="100%" OnClick="btnRefresh_Click" >
                                        </asp:Button>                                     
                                    </td>
                                    <td>                                     
                                          <asp:Button ID="btnPrintChart" CssClass="aspresizedbluebutton" runat="server" Text="Print Chart" Width="100%" OnClick="btnPrintChart_Click" OnClientClick=" return btnPrintChart_Clicked(this);" >
                                        </asp:Button>
                                       
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>
                </td>
                <td rowspan="2" style="width: 60%">
                    <div style="width: 100%; height: 769px;">
                        <asp:Panel ID="pnlGrowthChart" runat="server" Width="100%" Height="770px">
                            <telerik:RadChart ID="chrtGrowth" runat="server" Width="750px" DefaultType="Line"
                                Height="750px" Skin="SkyBlue">
                                <PlotArea>
                                    <XAxis>
                                        <Appearance Color="180, 210, 236" MajorTick-Color="206, 222, 235">
                                            <MajorGridLines Color="206, 222, 235" PenStyle="Solid" />
                                            <TextAppearance TextProperties-Color="51, 51, 51">
                                            </TextAppearance>
                                        </Appearance>
                                        <AxisLabel>
                                            <TextBlock>
                                                <Appearance TextProperties-Color="51, 51, 51">
                                                </Appearance>
                                            </TextBlock>
                                        </AxisLabel>
                                    </XAxis>
                                    <YAxis>
                                        <Appearance Color="180, 210, 236" MajorTick-Color="206, 222, 235" 
                                            MinorTick-Color="206, 222, 235">
                                            <MajorGridLines Color="206, 222, 235" />
                                            <MinorGridLines Color="206, 222, 235" PenStyle="Dash" />
                                            <TextAppearance TextProperties-Color="51, 51, 51">
                                            </TextAppearance>
                                        </Appearance>
                                        <AxisLabel>
                                            <TextBlock>
                                                <Appearance TextProperties-Color="51, 51, 51">
                                                </Appearance>
                                            </TextBlock>
                                        </AxisLabel>
                                    </YAxis>
                                    <Appearance Dimensions-Margins="18%, 100px, 12%, 8%">
                                        <FillStyle FillType="Solid" MainColor="White">
                                        </FillStyle>
                                        <Border Color="180, 210, 236" />
                                    </Appearance>
                                </PlotArea>
                                <Series>
                                    <telerik:ChartSeries Name="Series 1">
                                        <Appearance>
                                            <FillStyle FillType="ComplexGradient">
                                                <FillSettings>
                                                    <ComplexGradient>
                                                        <telerik:GradientElement Color="213, 247, 255" />
                                                        <telerik:GradientElement Color="193, 239, 252" Position="0.5" />
                                                        <telerik:GradientElement Color="157, 217, 238" Position="1" />
                                                    </ComplexGradient>
                                                </FillSettings>
                                            </FillStyle>
                                            <TextAppearance TextProperties-Color="51, 51, 51">
                                            </TextAppearance>
                                        </Appearance>
                                    </telerik:ChartSeries>
                                    <telerik:ChartSeries Name="Series 2">
                                        <Appearance>
                                            <FillStyle FillType="ComplexGradient">
                                                <FillSettings>
                                                    <ComplexGradient>
                                                        <telerik:GradientElement Color="218, 254, 122" />
                                                        <telerik:GradientElement Color="198, 244, 80" Position="0.5" />
                                                        <telerik:GradientElement Color="153, 205, 46" Position="1" />
                                                    </ComplexGradient>
                                                </FillSettings>
                                            </FillStyle>
                                            <TextAppearance TextProperties-Color="51, 51, 51">
                                            </TextAppearance>
                                            <Border Color="111, 174, 12" />
                                        </Appearance>
                                    </telerik:ChartSeries>
                                </Series>
                                <Appearance>
                                    <FillStyle MainColor="226, 247, 255">
                                    </FillStyle>
                                    <Border Color="82, 160, 226" />
                                </Appearance>
                                <Legend Visible="false">
                                    <Appearance Visible="False" dimensions-margins="15%, 2%, 1px, 1px" 
                                        position-alignedposition="TopRight">
                                        <ItemTextAppearance TextProperties-Font="Verdana, 8pt">
                                        </ItemTextAppearance>
                                        <FillStyle MainColor="Transparent">
                                        </FillStyle>
                                        <Border Color="Transparent" />
                                    </Appearance>
                                </Legend>
                                <ChartTitle>
                                    <Appearance Dimensions-Margins="3%, 10px, 14px, 6%">
                                        <FillStyle MainColor="Transparent">
                                        </FillStyle>
                                        <Border Color="Transparent" />
                                    </Appearance>
                                    <TextBlock>
                                        <Appearance TextProperties-Color="19, 111, 182" 
                                            TextProperties-Font="Arial, 18pt">
                                        </Appearance>
                                    </TextBlock>
                                </ChartTitle>
                            </telerik:RadChart>
                        </asp:Panel>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadGrid ID="grdGrowthChart" runat="server" Height="320px" CellSpacing="0"
                        GridLines="None" CssClass="Gridbodystyle">
                           <ClientSettings>
                                  <Scrolling AllowScroll="true"  UseStaticHeaders="true"   />
                        </ClientSettings>
                        <HeaderStyle CssClass="Gridheaderstyle" />
                    </telerik:RadGrid>
                </td>
            </tr>
        </table>
    </div>
    <div id="divLoading" class="modal" runat="server" style="text-align: center; display: none">
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
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
        
   <asp:PlaceHolder ID="PlaceHolder1" runat="server">
   
    <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

    <script src="JScripts/JSChart.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

    <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

    <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
   
   </asp:PlaceHolder>
    </form>
</body>
</html>
