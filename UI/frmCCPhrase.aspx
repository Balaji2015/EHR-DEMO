<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmCCPhrase.aspx.cs" EnableEventValidation="false" Inherits="Acurus.Capella.UI.frmCCPhrase" ValidateRequest="false" %>

<%@ Register Src="~/UserControls/CustomPhrases.ascx" TagName="Phrases" TagPrefix="Phrases" %>
<%@ Register Src="~/UserControls/CustomDLCNew.ascx" TagName="DLC" TagPrefix="DLC" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="aspx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Chief Complaints</title>
  
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>

    <style type="text/css">
        #frmCCPhrase {
            height: 638px;
            width: 950px;
        }

        .copyPrevious {
            position: static;
            top: 41px;
            left: 430px;
        }

        .dispaly {
            display: none;
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

        .underline {
            text-decoration: underline;
        }

        .notify {
            background-color: #e3f7fc;
            color: #555;
            border: .1em solid;
            border-color: #8ed9f6;
            border-radius: 10px;
            font-family: Tahoma,Geneva,Arial,sans-serif;
            font-size: 1.1em;
            padding: 10px 10px 10px 10px;
            margin: 10px;
            cursor: default;
        }

        .chiefcomplaints-wrapper {
            /*margin-left:80px!important;*/
            margin-left: 0px !important;
        }

        fieldset.scheduler-border {
            border: 1px groove #ddd !important;
            padding: 0 1.4em 1.4em 1.4em !important;
            margin: 0 0 1.5em 0 !important;
            -webkit-box-shadow: 0px 0px 0px 0px #000;
            box-shadow: 0px 0px 0px 0px #000;
        }

       
        legend {
            font-size: 13px !important;
            font-weight: 700 !important;
        }

        label {
            /* font-weight: 100 !important;*/
            font-weight: 100 !important;
        }

        #Paienttrend {
            text-align: center;
            font-weight: bold !important;
            width: 100%;
            font-size: 13px !important;
        }

    </style>

    <link href="CSS/bootstrap.min3.1.1.css" rel="stylesheet" />
    <link href="CSS/style.css" rel="stylesheet" type="text/css" />
    <link href="CSS/font-awesome.css" rel="stylesheet" type="text/css" />
    <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    
</head>
<body onload="CCPhraseLoad();OpenNotificationPopUp('ERX');">
    <form id="frmCCPhrase" runat="server" class="CCbodystyle" >

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
                <div id="SummaryAlert" runat="server" class="alert alert-info" style="height: 709px; padding: 10px; display: none; border-color: lightblue; color: black; margin: 3px; font-weight: bolder; background-color: aliceblue; font-family: Helvetica Neue,Helvetica,Arial,sans-serif; font-size: 14px;">
                    Xml is not found for this encounter. Please contact support.
                </div>
                <div class="chiefcomplaints-wrapper" style="border: none; width: 100%" id="CCPharse">
                    <table style="width: 100%">
                        <tr style="width: 100%">
                            <td style="width: 30%">
                                <fieldset>
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width:5%">&nbsp;</td>
                                            <td style="width: 25%">
                                                <div >
                                                    <span mand="Yes" class="CCmandLabel"> Chief Complaints* </span>
                                                </div>
                                            </td>
                                            <td style="width: 75%">
                                                <table style="margin-left: 0px" cellspacing="0px" cellpadding="0px">
                                                    <tr style="margin-left: 0px">
                                                        <td style="margin-left: 0px; padding-top: 4px;">
                                                            <DLC:DLC ID="ctmDLCChief_Complaints" runat="server" TextboxHeight="100px" TextboxWidth="610px"
                                                                Value="CHIEF_COMPLAINTS" />
                                                        </td>
                                                        <td style="margin-left: 0px; margin-top: 0px;"></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <table cellspacing="0px" cellpadding="0px" style="margin-left: 3px">
                                                                <tr>
                                                                    <td>
                                                                        <asp:PlaceHolder ID="phlCheifComplaints" runat="server" EnableViewState="false" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>

                                    <div class="clear">
                                    </div>
                                </fieldset>
                                <table>
                                    <tr>
                                        <td style="width:5%">&nbsp;</td>
                                        <td style="width: 25%">
                                            <div >

                                                 <span class="LabelStyleBold"> History of Present Illness </span>
                                                
                                            </div>
                                        </td>
                                        <td style="width: 75%">
                                            <table style="margin-left: 0px" cellspacing="0px" cellpadding="0px">
                                                <tr style="margin-left: 0px">
                                                    <td style="margin-left: 0px; padding-top: 4px;">
                                                        <DLC:DLC ID="ctmDLCHPI_Notes" runat="server" Value="HPI_NOTES" TextboxHeight="300px"
                                                            TextboxWidth="610px" />
                                                    </td>
                                                    <td style="margin-left: 0px; margin-top: 0px;"></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table cellspacing="0px" cellpadding="0px" style="margin-left: 3px">
                                                            <tr>
                                                                <td>
                                                                    <asp:PlaceHolder ID="phlHpiNotes" runat="server" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>

                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <div class="clear"></div>
                                <div style="margin-left: 9px; width: 93.4%; margin-bottom: -25px;">
                                    <fieldset class="scheduler-border" id="fldsetMedDocumentation" runat="server">
                                        <%--Added id for BugID:47790--%>
                                        <legend class="legendschedulerborder">Manual Prescriptions and Medication Documentation Details</legend>
                                        <label id="lblcount" runat="server" mand="Yes"  class="Editabletxtboxcc Editabletxtbox" enableviewstate="false">No of Medication Orders Filled  in Paper </label>
                                        <%--<span mand="Yes" id="lblcount" runat="server" class="CCmandLabel">No of Medication Orders Filled  in Paper *</span>--%>
                                        <input type="text" id="txtcount" runat="server" style="width: 50px;" maxlength="3" onkeypress="return AllowNumbers(event)" enableviewstate="true" />
                                        <br />
                                                   
                                        <input type="checkbox" runat="server" enableviewstate="true" id="chkCurrentMedicationDocumented" onclick="chkCurrentMedicationDocumented_changed();" onserverchange ="chkCurrentMedication_ServerChange" /><span id="txtCurrent"  class="MandLabelstyle" mand="Yes" runat="server">Current Medication Documented/Updated/Reviewed*</span>
                                        &nbsp;&nbsp;

                                            <label id="lblCurrentMedicationDocumented" style="" runat="server"   class="Editabletxtboxcc Editabletxtbox" enableviewstate="false">Reason Not Performed</label>

                                        <select id="cboCurrentMedicationDocumented" runat="server" style="width: 160px;" onchange="cboCurrentMedicationDocumented_Changed();">
                                        </select>
                                    </fieldset>
                                </div>


                                <div>
                                    <table>
                                        <tr>
                                            <td style="width: 60.5%;">
                                                <div style="width: 100%; margin-top: 30px; height: 50px;">
                                                    <div style="float: left; height: 47px; padding-left: 5px; color: white; margin-top: -5px; text-align: center;">
                                                        <img src="Resources/measure_logo.png" alt="" onclick="OpenMeasurePopup('ChiefComplaints');" style="height: 48px; width: 45px; padding-left: 2px; margin-left: -8px;" />
                                                    </div>
                                                    <div id="tag" class="boosterIconstyle" onclick="OpenMeasurePopup('ChiefComplaints');">
                                                        Measure Booster<img src="Resources/measure_info.png" alt="" style="width: 16px; height: 15px; margin-left: 4px;" />
                                                    </div>
                                                </div>
                                            </td>
                                            <td>
                                                <div>
                                                    <table>

                                                        <tr>
                                                            <td style="padding-top: 10px;" align="center">
                                                                <input type="button" runat="server" class="btn aspresizedbluebutton" id="btnCopyCC" accesskey="p" value="Copy Previous  CC"
                                                                    onclick="if (!btnCopyCC_Clicked()) return;" onserverclick="btnCopyCC_Click" />

                                                            </td>
                                                            <td style="padding-top: 10px; padding-left: 10px;" align="center">
                                                                <input type="button" runat="server" class="btn  aspresizedgreenbutton" id="btnAdd" accesskey="s"
                                                                    disabled="disabled" value="Save"
                                                                    onclick="if (!saveCC()) return;" onserverclick="btnSave_Click" />

                                                            </td>
                                                            <td style="padding-top: 10px; padding-left: 10px;" align="center">
                                                                <input type="button" runat="server" class="btn aspresizedredbutton " id="btnClearall" accesskey="c"
                                                                    value="Clear All" onclick="btnClear_Clicked();" onserverclick="btnClearall_ServerClick" />

                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="clear">
                                </div>
                            </td>
                            <td style="width: 5%"></td>
                            <td style="width: 65%">
                                <label runat="server" id="Paienttrend"  class="clinicltrend">Clinical Trend</label>
                                <div id="FlowchartDiv" style="border-style: ridge; width: 128%; padding: 4%; height: 560px; position: sticky;">
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
                                    <div id="Nograph" style="display: none; text-align: center; margin: 35% 0px; width: 100%; height: 100%">No Value(s) available for the selected criteria. </div>
                                </div>

                                <%-- <div class="notify" style="height: 545px; width: 22%; display: none; position: absolute; bottom: 8%;">
                                    <div id='divLoadingnotify' class='modal' style="height: 206px; width: 90%; position: absolute; bottom: 0px; display: none">
                                        <br />
                                        <br />
                                        <br />
                                        <br />
                                        <center></center>
                                        <br />
                                        <img src='Resources/loadimage.gif' style="opacity: 1; height: 15px; width: 15px; position: absolute; top: 103px; left: 50%; position: center;" title='[Please wait while the page is loading...]' alt='Loading...' />
                                        <br />
                                    </div>
                                    <div style="width: 96%; border-bottom: 1px solid red"><span style="color: red"><b>For MACRA Compliance</b></span></div>
                                    <br />
                                    <div id="dvnotifySummary" style="margin-top: 4px; width: 93.8%; position: absolute; height: 100%; font-size: 14px; display: block; overflow-y: auto; overflow-x: hidden;">
                                    </div>
                                </div>--%>
                                <div class="clear">
                                </div>
                            </td>

                        </tr>
                    </table>


                </div>

                <input id="Hidden1" type="hidden" runat="server" />
                <input id="hdnCopyPreviousPhysicianId" type="hidden" runat="server" />
                <asp:HiddenField ID="hdnLocalTime" runat="server" />
                <asp:HiddenField ID="HdnCopyButton" Value="false" runat="server" />
                <asp:HiddenField ID="hdnTrueCheck" Value="false" runat="server" />
                <asp:HiddenField ID="CheckSave" Value="false" runat="server" />
                <asp:HiddenField ID="hdnDisableCurrentProcess" Value="false" runat="server" />
                <asp:HiddenField ID="hdnTechnician" Value="false" runat="server" />

            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:PlaceHolder runat="server">
            <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
            <script src="JScripts/jquery-ui.min1.10.2.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/bootstrap.min.js" type="text/javascript"></script>

            <script src="JScripts/JSCustomPhrases.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSCustomDLC.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSCCPhrase.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

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
                                /* if (XAXisDetails.Intervals.length > 12) {
                                     xInterval.push(XAXisDetails.Intervals[0]);
                                     xInterval.push(XAXisDetails.Intervals[XAXisDetails.Intervals.length - 1]);
                                     var skipval = 1;
                                     skipval = Math.round((XAXisDetails.Intervals.length - 2) / 10);
                                     xlabelShowTextEvery = skipval;
                                     for (var i = skipval; i < XAXisDetails.Intervals.length - 2; i += skipval) {
                                         xInterval.push(XAXisDetails.Intervals[i]);
                                     }
                                 }
                                 else {
                                     xInterval = XAXisDetails.Intervals;
                                     xlabelShowTextEvery = 1;
                                 }*/
                                var options = {
                                    legend: { position: 'top' },
                                    series: {
                                        0: { color: '#71c73e' },
                                        1: { color: '#3366cc' },
                                        2: { color: '#f1ca3a' },
                                        3: { color: '#6f9654' },
                                        4: { color: '#43459d' },
                                    },
                                    //scaleStepWidth: 1,
                                    hAxis: {
                                        viewWindow: {
                                            min: new Date(XAXisDetails.MinValue),
                                            max: new Date(XAXisDetails.MaxValue)
                                        },
                                        showTextEvery: xlabelShowTextEvery,
                                        //ticks: xInterval,
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
                                        //gridlines: { count: 10 },
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




                            //var Result = JSON.parse(sdata.d);
                            // var FlowsheetData = JSON.parse(Result.FlowSheetData);
                            // var VitalRanges = JSON.parse(Result.VitalsRange);
                            // var VitalRangeList = new Array();
                            // var VitalDisplayFormats = new Array();
                            // for (var i = 0; i < VitalRanges.length; i++) {
                            //     VitalRangeList[VitalRanges[i].Field_name.toUpperCase()] = VitalRanges[i].Range;
                            //     VitalDisplayFormats[VitalRanges[i].Field_name.toUpperCase()] = VitalRanges[i].Format;
                            // }
                            // document.getElementById('FlowchartDiv').style.display = "inline-block";
                            // var Resultvalue = FlowsheetData.split("$")[0];
                            // var CountAndDate = FlowsheetData.split("$")[1];
                            // var XCount = CountAndDate.split("*")[0];
                            // var PlotDate = CountAndDate.split("*")[1].split("|")[0];
                            // var yMaxValue = 1000, yMinValue = 0, YCount = 0, xInterval = [], yInterval = [], xDisplayFormat = "MM/DD/YYYY", yDisplayFormat = "#";
                            // yMinValue = CountAndDate.split("*")[1].split("|")[1].split("&")[0];
                            // yMaxValue = CountAndDate.split("*")[1].split("|")[1].split("&")[1];
                            // if (XCount != 0) {
                            //     document.getElementById('container').style.display = "inline-block";
                            //     document.getElementById('Nograph').style.display = "none";
                            //     var data = new google.visualization.DataTable();
                            //     var yValues = [], YVal;
                            //     YVal = VitalRangeList[Value.toUpperCase()].split(',').map(Number);
                            //     for (var i = 0; i < YVal.length; i++) {
                            //         yValues.push(YVal[i]);
                            //     }
                            //     yInterval = yValues;
                            //     yDisplayFormat = VitalDisplayFormats[Value.toUpperCase()];
                            //     /*
                            //     switch (Value.toUpperCase()) {
                            //         case "BP-SITTING SYS/DIA": {
                            //             yMinValue = 35; yMaxValue = 250; yInterval = VitalRangeList[Value.toUpperCase()]; break;
                            //         }
                            //         case "HBA1C": {
                            //             yMinValue = 4; yMaxValue = 18; yInterval = VitalRangeList[Value.toUpperCase()]; yDisplayFormat = VitalDisplayFormats[Value.toUpperCase()]; break;
                            //         }
                            //         case "WEIGHT": {
                            //             yMinValue = 50; yMaxValue = 450; yInterval = VitalRangeList[Value.toUpperCase()]; break;
                            //         }
                            //         case "BMI": {
                            //             yMinValue = 15; yMaxValue = 50; yInterval = VitalRangeList[Value.toUpperCase()]; break;
                            //         }
                            //         default: {
                            //             yMinValue = 0; yMaxValue = 1000; break;
                            //         }
                            //     }
                            //     if (document.getElementById('cmboFlowSheetType').value == "BP-Sitting Sys/Dia") {
                            //          yMaxValue = 300;
                            //      }
                            //      else if (document.getElementById('cmboFlowSheetType').value == "HbA1C") {
                            //          yMaxValue = 40;
                            //      }
                            //      else {
                            //          yMaxValue = 1000;
                            //      }*/


                            //     if (Resultvalue != null && Resultvalue.length > 0) {
                            //         if (Value != "BP-Sitting Sys/Dia") {
                            //             if (Resultvalue.indexOf("~") > -1) {
                            //                 data.addColumn('string', 'Date and Time');
                            //                 data.addColumn('number', Value);
                            //                 data.addColumn({ type: 'number', role: 'annotation' });
                            //                 var content = Resultvalue.split("~");
                            //                 for (var i = 0; i < content.length; i++) {
                            //                     data.addRows([
                            //                        [content[i].split("^")[0], parseFloat(content[i].split("^")[1]), parseFloat(content[i].split("^")[1])]
                            //                     ]);
                            //                     xInterval.push(content[i].split("^")[0]);
                            //                 }
                            //             }
                            //             else {
                            //                 data.addColumn('string', 'Date and Time');
                            //                 data.addColumn('number', Value);
                            //                 data.addColumn({ type: 'number', role: 'annotation' });
                            //                 data.addRows([
                            //                    [Resultvalue.split("^")[0], parseFloat(Resultvalue.split("^")[1]), parseFloat(Resultvalue.split("^")[1])]
                            //                 ]);
                            //                 xInterval.push(Resultvalue.split("^")[0]);
                            //             }
                            //         }
                            //         else {
                            //             if (Resultvalue.indexOf("~") > -1) {
                            //                 data.addColumn('string', 'Date and Time');
                            //                 data.addColumn('number', 'BP-Sitting(Sys)');
                            //                 data.addColumn({ type: 'number', role: 'annotation' });
                            //                 data.addColumn('number', 'BP-Sitting(Dia)');
                            //                 data.addColumn({ type: 'number', role: 'annotation' });
                            //                 var content = Resultvalue.split("~");
                            //                 for (var i = 0; i < content.length; i++) {
                            //                     data.addRows([
                            //                         [content[i].split("^")[0], parseFloat(content[i].split("^")[1].split("/")[0]), parseFloat(content[i].split("^")[1].split("/")[0]), parseFloat(content[i].split("^")[1].split("/")[1]), parseFloat(content[i].split("^")[1].split("/")[1])]
                            //                     ]);
                            //                     xInterval.push(content[i].split("^")[0]);
                            //                 }
                            //             }
                            //             else {
                            //                 data.addColumn('string', 'Date and Time');
                            //                 data.addColumn('number', 'BP-Sitting(Sys)');
                            //                 data.addColumn({ type: 'number', role: 'annotation' });
                            //                 data.addColumn('number', 'BP-Sitting(Dia)');
                            //                 data.addColumn({ type: 'number', role: 'annotation' });
                            //                 data.addRows([
                            //                    [Resultvalue.split("^")[0], parseFloat(Resultvalue.split("^")[1].split("/")[0]), parseFloat(Resultvalue.split("^")[1].split("/")[0]), parseFloat(Resultvalue.split("^")[1].split("/")[1]), parseFloat(Resultvalue.split("^")[1].split("/")[1])]
                            //                 ]);
                            //                 xInterval.push(Resultvalue.split("^")[0]);
                            //             }
                            //         }
                            //     }

                            //     var options = {
                            //         //'title': 'Patient Trend',
                            //         legend: { position: 'top' },
                            //         series: {
                            //             0: { color: '#71c73e' },
                            //             1: { color: '#3366cc' },
                            //             2: { color: '#f1ca3a' },
                            //             3: { color: '#6f9654' },
                            //             4: { color: '#43459d' },
                            //         },
                            //         hAxis: {
                            //             //textPosition: 'none', //to invisible the X axis value yyyy,MM,dd
                            //             //title: 'Captured/Collected Date & Time',   
                            //             viewWindow: {
                            //                 //min: new Date(Resultvalue.split("~")[0].split("^")[0]),
                            //                 min: new Date(PlotDate.split("&")[0]),
                            //                 max: new Date(PlotDate.split("&")[1])
                            //                 // min: new Date('2011-Dec-11')
                            //             },
                            //             gridlines: { count: XCount },
                            //             ticks: xInterval,
                            //             slantedText: true,
                            //             slantedTextAngle: 90,
                            //         },
                            //         vAxis: {
                            //             //maxValue: yMaxValue,
                            //             //minValue: yMinValue
                            //             ////title: 'Temperature'
                            //             viewWindow: {
                            //                 min: yMinValue,
                            //                 max: yMaxValue
                            //             },
                            //             format: yDisplayFormat,
                            //             //ticks: yInterval,
                            //             //gridlines: {
                            //             //    count:10
                            //             //    //count: Math.ceil(yMaxValue * 1.1 / 10) // try to pick the correct number to create intervals of 50000 
                            //             //}
                            //         },
                            //         'width': 420,
                            //         'height': 480,
                            //         'chartArea': {
                            //             'width': '85%', 'height': '70%'
                            //         },
                            //         pointsVisible: true,
                            //         annotations: {
                            //             stemColor: 'none',
                            //             alwaysOutside: true
                            //         }
                            //     };
                            //     var chart = new google.visualization.LineChart(document.getElementById('container'));
                            //     chart.draw(data, options);
                            // }
                            // else {
                            //     document.getElementById('container').style.display = "none";
                            //     document.getElementById('Nograph').style.display = "inline-block";
                            // }
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
