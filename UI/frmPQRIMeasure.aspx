<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmPQRIMeasure.aspx.cs"
    Inherits="Acurus.Capella.UI.frmPQRIMeasure" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
      <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <title>eCQM Calculator</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
        .style2 {
            height: 92px;
        }

        .style4 {
            height: 44px;
        }

        .style7 {
            width: 101px;
        }

        .style8 {
            width: 150px;
        }

        .style9 {
            width: 125px;
        }

        .style10id="grdPQRIMeasure" {
            width: 89px;
        }

        .style12 {
            width: 227px;
        }

        .BackGround {
            background-color: White;
            width: 1188px;
            height: 687px;
        }

        .style13 {
            width: 117px;
        }

        .style14 {
            width: 44px;
        }

        .style15 {
            width: 152px;
        }

        .style16 {
            width: 89px;
            height: 56px;
        }

        .style17 {
            width: 44px;
            height: 56px;
        }

        .style18 {
            width: 117px;
            height: 56px;
        }

        .style19 {
            width: 101px;
            height: 56px;
        }

        .style20 {
            width: 150px;
            height: 56px;
        }

        .style21 {
            width: 152px;
            height: 56px;
        }

        .style22 {
            width: 125px;
            height: 56px;
        }

        .style23 {
            width: 227px;
            height: 56px;
        }

        .style24 {
            height: 56px;
        }

        .style25 {
            height: 499px;
        }
    </style>
    <asp:PlaceHolder runat="server">
        <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
        <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
        <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
        <script src="JScripts/JSPQRI.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

        <script type="text/javascript" id="telerikClientEvents1">

            //<![CDATA[

            function btnGenerateReport_Clicked(sender, args) {
                var FromDate = document.getElementById('dtpFromDate').value;
                var ToDate = document.getElementById('dtpToDate').value;
                if (FromDate == "" && ToDate == "") {
                    DisplayErrorMessage('380006');
                }
                { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
                var dt = new Date();
                document.getElementById('hdnLocalTime').value = dt.getTimezoneOffset().toString();
            }
            function GenerateMeasures() {
                $("#btnGenerate").click();
                return true;
            }
            function DownloadFile(path, file) {
                //CAP-2751
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                const link = document.createElement('a');
                link.href = path;
                link.download = file;
                document.body.appendChild(link);
                link.click();
                document.body.removeChild(link);
                return true;
            }

            function DownloadCaTIFile(path, file) {
                //CAP-2751
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                const link = document.createElement('a');
                link.href = path;
                link.download = file;
                document.body.appendChild(link);
                link.click();
                document.body.removeChild(link);
                return true;
            }

            //]]>
        </script>
        <script type="text/javascript">
            function RadWindowClose() {
                var oWindow = null;
                if (window.radWindow)
                    oWindow = window.radWindow;
                else if (window.frameElement.radWindow)
                    oWindow = window.frameElement.radWindow;
                if (oWindow != null)
                    oWindow.close();
            }
            function Load() {
                { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }

            }
        </script>
    </asp:PlaceHolder>

</head>

<script type="text/javascript">

</script>

<body onload="OnPQRILoad(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}">
    <form id="form1" style="font-family: Microsoft Sans Serif; font-size: small" runat="server"
        class="BackGround">
        <telerik:RadWindowManager ID="PQRIMeasureWindowMngr" runat="server" EnableViewState="false">
            <Windows>
                <telerik:RadWindow ID="PQRIMeasureWindow" runat="server" Modal="true" VisibleOnPageLoad="false"
                    Behaviors="Close" IconUrl="Resources/16_16.ico" EnableViewState="false" Title="CQM Details">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <div style="height: 729px; width: 1182px">
            <table style="width: 100%; height: 682px;">
                <tr>
                    <td class="style2">
                        <asp:Panel ID="Panel1" 
                            runat="server" GroupingText="Measure Calculation Details" Height="123px" 
                            BackColor="White" CssClass="spanstyle">
                            <table style="width: 100%; height: 104px;">
                                <tr>
                                    <td class="style16">
                                        <asp:Label ID="lblStage" runat="server" Width="30px" Text="Stage*" cssclass="spanstyle" mand="Yes"></asp:Label>
                                    </td>
                                    <td class="style17">
                                        <telerik:RadComboBox ID="cboStage" runat="server" Height="100px" Width="125px" CssClass="Editabletxtbox">
                                            <Items>
                                                <telerik:RadComboBoxItem runat="server" Text="Stage 3" Selected="true" />
                                                <%--<telerik:RadComboBoxItem runat="server" Text="Stage 2" />--%>
                                                <%-- Selected="true" --%>
                                                <%--<telerik:RadComboBoxItem runat="server" Text="Stage 1" />--%>
                                            </Items>
                                        </telerik:RadComboBox>
                                    </td>
                                    <td class="style18">
                                        <asp:Label ID="lblFromDate" runat="server" Text="From Date*" cssclass="spanstyle" mand="Yes"></asp:Label>
                                    </td>
                                    <td class="style19">
                                        <telerik:RadDatePicker ID="dtpFromDate" runat="server" Culture="English (United States)"
                                            Width="145px">
                                            <Calendar UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False" ViewSelectorText="x">
                                            </Calendar>
                                            <DatePopupButton HoverImageUrl="" ImageUrl="" />
                                            <DateInput DateFormat="dd-MMM-yyyy" DisplayDateFormat="dd-MMM-yyyy" DisplayText=""
                                                LabelWidth="40%" type="text" value="">
                                                <EmptyMessageStyle Resize="None" />
                                                <ReadOnlyStyle Resize="None" />
                                                <FocusedStyle Resize="None" />
                                                <DisabledStyle Resize="None" />
                                                <InvalidStyle Resize="None" />
                                                <HoveredStyle Resize="None" />
                                                <EnabledStyle Resize="None" />
                                            </DateInput>
                                        </telerik:RadDatePicker>
                                    </td>
                                    <td class="style20">
                                        <asp:Label ID="lblToDate" runat="server" Text="To Date*" cssclass="spanstyle" mand="Yes"></asp:Label>
                                    </td>
                                    <td class="style21">
                                        <telerik:RadDatePicker ID="dtpToDate" runat="server" Culture="English (United States)"
                                            Width="142px">
                                            <Calendar UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False" ViewSelectorText="x">
                                            </Calendar>
                                            <DatePopupButton HoverImageUrl="" ImageUrl="" />
                                            <DateInput DateFormat="dd-MMM-yyyy" DisplayDateFormat="dd-MMM-yyyy" DisplayText=""
                                                LabelWidth="40%" type="text" value="">
                                            </DateInput>
                                        </telerik:RadDatePicker>
                                    </td>
                                    <td class="style22">
                                        <asp:Label ID="lblProviderName" runat="server" Text="Provider Name*" cssclass="spanstyle" mand="Yes"></asp:Label>
                                    </td>
                                    <td class="style23">
                                        <telerik:RadComboBox ID="cboProviderName" runat="server" Height="100px" Width="218px" Enabled="true" CssClass="spanstyle"/>
                                    </td>
                                    <td class="style24">
                                        <asp:CheckBox ID="chkProviderName" Text="Show All Physicians" runat="server" onclick="chkShowActiveloading()" OnCheckedChanged="chkProviderName_CheckedChanged" AutoPostBack="True" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style10">&nbsp;
                                    </td>
                                    <td class="style14">&nbsp;
                                    </td>
                                    <td class="style13">&nbsp;
                                    </td>
                                    <td class="style7">&nbsp;
                                    </td>
                                    <td class="style8">&nbsp;
                                    </td>
                                    <td class="style15">&nbsp;
                                    </td>
                                    <td class="style9">&nbsp;
                                    </td>
                                    <td class="style12">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td>
                                        <telerik:RadButton ID="btnGenerateReport" runat="server" Text="Generate Report" Width="141px"
                                            OnClick="btnGenerateReport_Click" OnClientClicked="btnGenerateReport_Clicked" ButtonType="LinkButton" CssClass="bluebutton"/>
                                        <asp:Button ID="btnGenerate" runat="server" Style="display: none" OnClick="btnGenerateReport_Click" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td class="style25">
                        <asp:Panel ID="Panel2" runat="server" GroupingText="Measure Calculation" Height="475px"
                            CssClass="spanstyle" BackColor="White" Width="1184px" >
                            <telerik:RadGrid ID="grdPQRIMeasure" runat="server" Height="455px" Width="1175px" CssClass="Gridbodystyle" EnableEmbeddedSkins="false"
                                 CellSpacing="0" GridLines="None" AutoGenerateColumns="False" OnPreRender="grdPQRIMeasure_PreRender" OnItemCommand="grdMeasureCalculation_ItemCommand">
                                <HeaderStyle cssclass="Gridheaderstyle" HorizontalAlign="Center" />
                                <ClientSettings Scrolling-UseStaticHeaders="true" Scrolling-AllowScroll="true">
                                    <Scrolling AllowScroll="True" UseStaticHeaders="True" />
                                </ClientSettings>
                                <MasterTableView>
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="Measure Name" FilterControlAltText="Filter MeasureName column"
                                            HeaderText="Measure Name" Resizable="False" UniqueName="MeasureName">
                                            <HeaderStyle Width="130px" />
                                            <ItemStyle Width="130px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Measure in Detail" FilterControlAltText="Filter MeasureinDetail column"
                                            HeaderText="Measure in Detail" UniqueName="MeasureinDetail">
                                            <HeaderStyle Width="280px" />
                                            <ItemStyle Width="280px" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Initial Patient Population" FilterControlAltText="Filter InitialPatientPopulation column"
                                            HeaderText="Initial Patient Population" UniqueName="InitialPatientPopulation">
                                            <HeaderStyle Width="80px" />
                                            <ItemStyle Width="80px" HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Denominator" FilterControlAltText="Filter Denominator column"
                                            HeaderText="Denominator" UniqueName="Denominator">
                                            <HeaderStyle Width="90px" />
                                            <ItemStyle Width="90px" HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Denominator Exclusion" FilterControlAltText="Filter DenominatorExclusion column"
                                            HeaderText="Denominator Exclusion" UniqueName="DenominatorExclusion">
                                            <HeaderStyle Width="90px" />
                                            <ItemStyle Width="90px" HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Denominator Exception" FilterControlAltText="Filter DenominatorException column"
                                            HeaderText="Denominator Exception" UniqueName="DenominatorException">
                                            <HeaderStyle Width="90px" />
                                            <ItemStyle Width="90px" HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Numerator" FilterControlAltText="Filter Numerator column"
                                            HeaderText="Numerator" UniqueName="Numerator">
                                            <HeaderStyle Width="80px" />
                                            <ItemStyle Width="80px" HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="Numerator Exclusion" FilterControlAltText="Filter NumeratorExclusion column"
                                            HeaderText="Numerator Exclusion" UniqueName="NumeratorExclusion">
                                            <HeaderStyle Width="80px" />
                                            <ItemStyle Width="80px" HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="Rate" FilterControlAltText="Filter Rate column"
                                            HeaderText="Rate" UniqueName="Rate">
                                            <HeaderStyle Width="80px" />
                                            <ItemStyle Width="80px" HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Rate Required" FilterControlAltText="Filter RateRequired column"
                                            HeaderText="Rate Required" UniqueName="RateRequired">
                                            <HeaderStyle Width="150px" />
                                            <ItemStyle Width="150px" HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Cleared" FilterControlAltText="Filter Cleared column"
                                            HeaderText="Cleared" UniqueName="Cleared">
                                            <HeaderStyle Width="50px" />
                                            <ItemStyle Width="50px" HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Detail" ImageUrl="~/Resources/Down.bmp"
                                            FilterControlAltText="Filter column2 column" Text="View Detail"
                                            UniqueName="column2" HeaderText="View Detail">
                                            <HeaderStyle Width="70px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridButtonColumn>
                                        <telerik:GridBoundColumn DataField="Denominator Human Id"
                                            FilterControlAltText="Filter column Denom_Human_ID" HeaderText="Denominator Human Id"
                                            UniqueName="Denom_Human_ID" Display="False">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Numerator Human Id"
                                            FilterControlAltText="Filter Numerator_Human_ID column" HeaderText="Numerator Human Id"
                                            UniqueName="Numerator_Human_ID" Display="False">
                                        </telerik:GridBoundColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td class="style4">
                        <%-- &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <telerik:RadButton ID="btnCATIZIP" runat="server" Text="CAT I Download" 
                        Width="105px" onclick="btnCATIZIP_Click" Height="25px" 
                        />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <telerik:RadButton ID="btnCQM" runat="server" Text="CAT III" Width="77px" 
                        onclick="btnCQM_Click" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <telerik:RadButton ID="btnPrintPDF" runat="server" Text="Print PDF" Width="77px" />
                    &nbsp;&nbsp;&nbsp;
                    <telerik:RadButton ID="btnExportExcel" runat="server" Text="Export to Excel" Width="86px" />
                    &nbsp;&nbsp;
                    <telerik:RadButton ID="btnClose" runat="server" Text="Close" Width="93px" />--%>
                        <table style="width: 100%; height: 40px;">
                            <tr>

                                <td style="width: 68%;"></td>
                                <td style="width: 10%;">
                                    <telerik:RadButton ID="btnCATIZIP" runat="server" Text="CAT I Download" Width="140px"
                                        OnClick="btnCATIZIP_Click" OnClientClicked="Load" ButtonType="LinkButton" CssClass="bluebutton"/>
                                </td>
                                <td style="width: 10%;">
                                    <telerik:RadButton ID="btnCQM" runat="server" Text="CAT III Download" Width="139px" OnClick="btnCQM_Click" OnClientClicked="Load" ButtonType="LinkButton" CssClass="bluebutton" />
                                </td>
                                <%-- <td style="width:7%; ">
                                <telerik:RadButton ID="btnPrintPDF" runat="server" Text="Print PDF" Width="70px" />
                            </td>
                            <td style="width:10%; ">
                                <telerik:RadButton ID="btnExportExcel" runat="server" Text="Export to Excel" Width="90px" />
                            </td>--%>
                                <td style="width: 5%;">
                                    <telerik:RadButton ID="btnClose" runat="server" Text="Close" Width="75px" AutoPostBack="false" OnClientClicked="RadWindowClose" ButtonType="LinkButton" CssClass="redbutton" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                         <asp:Button id="btndownload" runat="server" OnClick="btndownload_Click" style="display:none;" />
                        <asp:Button id="btndownloadCATI" runat="server" OnClick="btndownloadCATI_Click" style="display:none;" />
                        
                    </td>
                </tr>
            </table>
            <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
            </telerik:RadScriptManager>
        </div>
       
        <asp:HiddenField ID="hdnLocalTime" runat="server" EnableViewState="false" />
    </form>
</body>
</html>
