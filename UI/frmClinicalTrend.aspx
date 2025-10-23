<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmClinicalTrend.aspx.cs" Inherits="Acurus.Capella.UI.frmClinicalTrend" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Clinical Trend</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
  <%--  <style>
         #Paienttrend {
            text-align: center;
            font-weight: bold !important;
            width: 100%;
            font-size: 13px !important;
        }
    </style>--%>
   
    <link href="CSS/bootstrap.min3.1.1.css" rel="stylesheet" />
    <link href="CSS/style.css" rel="stylesheet" type="text/css" />
    <link href="CSS/font-awesome.css" rel="stylesheet" type="text/css" />
    <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
</head>
<body onload="{ sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }">
    <form id="form1" runat="server">
    
        <table style="width: 100%">
            <tr style="width: 100%">
                <td  style="width: 65%">
                    <label runat="server" id="Paienttrend"  class="clinicltrend" style="margin-left: 479px;">Clinical Trend</label>
                    <div id="FlowchartDiv" style="display: none; border-style: ridge; width: 611px; padding: 2%; height: 560px; position: fixed; margin-top: 29px; margin-left: -276px;">
                                    <div style="height: 5%;">
                                        <table>
                                            <tr style="font-family: Microsoft sans pro; font-size: 13.4px!important;">
                                                <td style="width: 12%; text-align: center;">
                                                    <label style="font-weight: bold!important;" class="Editabletxtbox">Vitals </label>
                                                </td>
                                                <td style="width: 30%; text-align: left;">
                                                    <asp:DropDownList ID="cboFlowSheetType" runat="server" EnableViewState="true" onchange="drawChart();"></asp:DropDownList></td>
                                                <td style="width: 6%;"></td>
                                                <td style="width: 12%; text-align: center;">
                                                    <label style="font-weight: bold!important;" class="Editabletxtbox">Duration </label>
                                                </td>
                                                <td style="width: 30%; text-align: center;">
                                                    <asp:DropDownList ID="cboFlowSheetPeriod" runat="server" EnableViewState="true" onchange="drawChart();"></asp:DropDownList></td>

                                            </tr>
                                        </table>
                                    </div>
                                    <div id="container" style="margin: 0px auto; height: 94%;"></div>
                                    <div id="Nograph" style="display: none; text-align: center; margin: 35% 0px; width: 100%;">No Value(s) available for the selected criteria. </div>
                                </div>

                </td>
            </tr>

        </table>

        <asp:PlaceHolder runat="server">
             <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
            <script src="JScripts/jquery-ui.min1.10.2.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/bootstrap.min.js" type="text/javascript"></script>
            <script src="JScripts/JSLibraries.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
<script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
<script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
<script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
             <script src="JScripts/googlechart-V1.js" type="text/javascript"></script>


            <script type="text/javascript">
                google.charts.load('current', { packages: ['corechart', 'line'] });
            </script>
            <script type="text/javascript">
                function drawChart() {
                    var Value = document.getElementById('cboFlowSheetType').value;
                    var SelectedPeriod = document.getElementById('cboFlowSheetPeriod').value;
                    $.ajax({
                        type: "POST",
                        async: true,
                        url: "frmRCopiaToolbar.aspx/LoadFlowSheetData",
                        data: "{'FieldName':'" + Value + "','PeriodValue':'" + SelectedPeriod + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (sdata) {

                            document.getElementById('FlowchartDiv').style.display = "inline-block";
                            var Result = JSON.parse(sdata.d);
                            var XAXisDetails = JSON.parse(Result.XAxis);
                            var YAXisDetails = JSON.parse(Result.YAxis);
                            var IsAnnotate = JSON.parse(Result.IsAnnotationReq);
                            var DataPoints = JSON.parse(Result.PlotPoints);
                            var DisplaySets = JSON.parse(Result.DisplaySets);
                            if (DataPoints.trim() != "" && DataPoints.split('~').length > 0) {
                                document.getElementById('container').style.display = "inline-block";
                                document.getElementById('Nograph').style.display = "none";
                                var data = new google.visualization.DataTable();
                                if (DisplaySets > 1) {
                                    if (IsAnnotate == true) {
                                        data.addColumn('string', XAXisDetails.DisplayText);
                                        data.addColumn('number', YAXisDetails.DisplayText.split('|')[0]);
                                        data.addColumn({ type: 'number', role: 'annotation' });
                                        data.addColumn('number', YAXisDetails.DisplayText.split('|')[1]);
                                        data.addColumn({ type: 'number', role: 'annotation' });
                                        var content = DataPoints.split("~");
                                        for (var i = 0; i < content.length; i++) {
                                            data.addRows([
                                               [content[i].split("^")[0], parseFloat(content[i].split("^")[1].split("/")[0]), parseFloat(content[i].split("^")[1].split("/")[0]), parseFloat(content[i].split("^")[1].split("/")[1]), parseFloat(content[i].split("^")[1].split("/")[1])]
                                            ]);
                                        }
                                    }
                                    else {
                                        data.addColumn('string', XAXisDetails.DisplayText);
                                        data.addColumn('number', YAXisDetails.DisplayText.split('|')[0]);
                                        data.addColumn('number', YAXisDetails.DisplayText.split('|')[1]);
                                        var content = DataPoints.split("~");
                                        for (var i = 0; i < content.length; i++) {
                                            data.addRows([
                                               [content[i].split("^")[0], parseFloat(content[i].split("^")[1].split("/")[0]), parseFloat(content[i].split("^")[1].split("/")[1])]
                                            ]);
                                        }
                                    }
                                }
                                else {
                                    if (IsAnnotate == true) {
                                        data.addColumn('string', XAXisDetails.DisplayText);
                                        data.addColumn('number', YAXisDetails.DisplayText);
                                        data.addColumn({ type: 'number', role: 'annotation' });
                                        var content = DataPoints.split("~");
                                        for (var i = 0; i < content.length; i++) {
                                            data.addRows([
                                               [content[i].split("^")[0], parseFloat(content[i].split("^")[1]), parseFloat(content[i].split("^")[1])]
                                            ]);
                                        }
                                    }
                                    else {
                                        data.addColumn('string', XAXisDetails.DisplayText);
                                        data.addColumn('number', YAXisDetails.DisplayText);
                                        var content = DataPoints.split("~");
                                        for (var i = 0; i < content.length; i++) {
                                            data.addRows([
                                               [content[i].split("^")[0], parseFloat(content[i].split("^")[1])]
                                            ]);
                                        }
                                    }
                                }
                                var yInterval = [];
                                var xInterval = [];
                                for (var i = 0; i < YAXisDetails.Intervals.length; i++) {
                                    yInterval.push(YAXisDetails.Intervals[i]);
                                }
                                var xlabelShowTextEvery = 1;
                                if (XAXisDetails.Intervals.length > 12) {
                                    var skipval = 1;
                                    skipval = Math.ceil((XAXisDetails.Intervals.length) / 12);
                                    xlabelShowTextEvery = skipval;
                                }
                                
                                var options = {
                                    legend: { position: 'top' },
                                    series: {
                                        0: { color: '#71c73e' },
                                        1: { color: '#3366cc' },
                                        2: { color: '#f1ca3a' },
                                        3: { color: '#6f9654' },
                                        4: { color: '#43459d' },
                                    },
                                    hAxis: {
                                        viewWindow: {
                                            min: new Date(XAXisDetails.MinValue),
                                            max: new Date(XAXisDetails.MaxValue)
                                        },
                                        showTextEvery: xlabelShowTextEvery,
                                        gridlines: { count: 10 },
                                        slantedText: true,
                                        slantedTextAngle: 90,
                                        format: XAXisDetails.DisplayFormat
                                    },
                                    vAxis: {
                                        viewWindow: {
                                            min: parseFloat(YAXisDetails.MinValue),
                                            max: parseFloat(YAXisDetails.MaxValue)
                                        },
                                        ticks: yInterval,
                                        format: YAXisDetails.DisplayFormat,
                                    },
                                    'width': 420,
                                    'height': 480,
                                    'chartArea': {
                                        'width': '85%', 'height': '70%'
                                    },
                                    pointsVisible: true,
                                    annotations: {
                                        stemColor: 'none',
                                        alwaysOutside: true
                                    }
                                };
                                var chart = new google.visualization.LineChart(document.getElementById('container'));
                                chart.draw(data, options);
                            }
                            else {
                                document.getElementById('container').style.display = "none";
                                document.getElementById('Nograph').style.display = "inline-block";
                            }

                        },
                        error: function OnError(xhr) {

                            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                            if (xhr.status == 999)
                                window.location = "/frmSessionExpired.aspx";
                            else {
                                var log = JSON.parse(xhr.responseText);
                                console.log(log);
                                alert("USER MESSAGE:\n" +
                                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                                   "Message: " + log.Message);
                            }
                        }
                    });
                }
                google.charts.setOnLoadCallback(drawChart);
            </script>
        </asp:PlaceHolder>

    </form>
</body>
</html>
