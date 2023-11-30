<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmPatientChart.aspx.cs"
    Inherits="Acurus.Capella.UI.frmPatientChart" MasterPageFile="~/C5PO.Master" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="PatientChartHeader" ContentPlaceHolderID="head" runat="server">
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <%--<link href="CSS/font-awesome.css" rel="stylesheet" type="text/css" />--%>
    <link href="CSS/fontawesome.min.css" rel="stylesheet" type="text/css" />
    <link href="CSS/solid.min.css" rel="stylesheet" />
    <%--<script src="JScripts/jquery-1.11.3.min.js" type="text/javascript"></script>--%>
    <style type="text/css">
        .DockImage {
            background-repeat: no-repeat;
            background-position: center;
            margin: 0px;
            padding: 0px;
        }

        .tooltip {
            display: inline;
            position: relative;
        }


        /*.colored {
            background-color: #bfdbff;
            padding: 3px;
            border-radius: 3px;
            color: black;
            font-weight: bold;
        }*/

        .SmallFont {
            font-size: small;
            font-family: Microsoft Sans Serif;
        }

        .boxModel {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
        }

        /** {
            font-size: 11.5px;
            font-family: Microsoft Sans Serif;
        }*/

        .displayInline {
            display: inline;
        }

        html, body {
            height: 100%;
            margin: 0;
            padding: 0;
        }

        #spnPatientstrip.title {
            border: 1px solid #ccc;
            box-shadow: 0 0 10px 0 #ddd;
            -moz-box-shadow: 0 0 10px 0 #ddd;
            -webkit-box-shadow: 0 0 10px 0 #ddd;
            color: #666;
            background: red;
        }

        .BlockStyle {
            border: thin solid #000000;
        }

        .AlignmentForSummarBar {
            vertical-align: top;
        }

        .OverFlowScroll {
            border: thin solid #000000;
            overflow: scroll;
        }

        .BlockObjects {
            display: inline-block;
        }

        .newStyle1 {
            background-color: #BFDBFF;
        }

        div legend {
            font-weight: bolder;
        }

        #CenPercentWidth {
            width: 100%;
        }

        div.RadTreeView {
            line-height: 16px !important;
        }

            div.RadTreeView .rtSp {
                height: 14px !important;
            }

            div.RadTreeView .rtHover .rtIn, div.RadTreeView .rtSelected .rtIn {
                padding: 0px 1px 0px !important;
            }

            div.RadTreeView .rtIn {
                padding: 1px 2px 1px !important;
            }

            div.RadTreeView .rtTop {
                margin-top: 5px !important;
            }

        .NoWrap {
            white-space: nowrap;
        }

        .AbsoulteStyle {
            position: absolute;
        }

        .WidthResize {
            width: 5%;
        }

        .widthsized {
            width: 20%;
        }

        @keyframes fadeInDown {
            0% {
                opacity: 0;
            }

            100% {
                opacity: 1;
            }
        }

        .checkFade {
            animation-name: fadeInDown;
            animation-duration: 1s;
        }

        #dock {
            margin: 0px;
            padding: 0px;
            list-style: none;
            top: 150px;
            height: 10%;
            z-index: 100;
            background-color: #f0f0f0;
            left: 0px;
        }

            #dock > li {
                width: 40px;
                height: 10%;
                margin: 0 0 1px 0;
                background-color: #dcdcdc;
                background-repeat: no-repeat;
                background-position: left center;
            }

            #dock:hover {
                cursor: pointer;
            }



        /* panels */
        ::-webkit-scrollbar {
            width: 9px !important;
            height: 7px !important;
        }

        /*::-webkit-scrollbar-track {
            background-color: #c3bfbf;
            border-radius:10px;

            
        }*/
        ::-webkit-scrollbar-track {
            background-color: transparent;
            border-radius: 10px;
            height: 8px;
            visibility: hidden;
        }

        ::-webkit-scrollbar-thumb {
            background-color: #707070;
            border-radius: 10px;
        }

            ::-webkit-scrollbar-thumb:hover {
                background-color: #3d3c3a;
            }

        ::-webkit-scrollbar-button {
            display: none;
        }

        ::-webkit-scrollbar-track-piece {
            background-color: transparent;
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
            visibility: hidden;
        }

        .symbol {
            font-size: 0.9em;
            font-family: Times New Roman;
            border-radius: 1em;
            padding: .1em .6em .1em .6em;
            font-weight: bolder;
            color: white;
            background-color: #4E5A56;
        }

        .icon-info {
            background-color: #3229CF;
        }

        .para {
            word-wrap: normal;
            text-wrap: none;
            width: 116% !important;
            font-family: Microsoft sans pro;
            font-size: 13px;
        }



        .tooltip-inner {
            background-color: white !important;
        }

        #divPatchart {
            /* border-left: 23px solid rgb(35, 107, 142);
            border-top: 12px solid transparent;
            border-bottom: 12px solid transparent;
            height: 120px;
            width: 20px;
               */
            cursor: pointer;
        }

        #spnPatchart {
            display: block;
            position: absolute;
            transform: rotate(-90deg);
            margin-left: -56px;
            margin-top: 38px;
            color: white;
            font-size: 14px;
            font-weight: bold;
            font-family: Microsoft Sans serif;
        }

        /*  bhoechie tab */
        div.bhoechie-tab-container {
            z-index: 10;
            background-color: #fff;
            padding: 0 !important;
            border-radius: 4px;
            -moz-border-radius: 4px;
            border: 1px solid #ddd;
            -webkit-box-shadow: 0 6px 12px rgba(0,0,0,.175);
            box-shadow: 0 6px 12px rgba(0,0,0,.175);
            -moz-box-shadow: 0 6px 12px rgba(0,0,0,.175);
            background-clip: padding-box;
            opacity: 1;
            filter: alpha(opacity=97);
        }

        div.bhoechie-tab-menu {
            padding-right: 0;
            padding-left: 0;
            padding-bottom: 0;
        }

            div.bhoechie-tab-menu div.list-group {
                margin-bottom: 0;
            }

                div.bhoechie-tab-menu div.list-group > a {
                    margin-bottom: 0;
                }

                    div.bhoechie-tab-menu div.list-group > a .glyphicon,
                    div.bhoechie-tab-menu div.list-group > a .fa {
                        color: #5A55A3;
                    }

                    div.bhoechie-tab-menu div.list-group > a:first-child {
                        border-top-right-radius: 0;
                        -moz-border-top-right-radius: 0;
                    }

                    div.bhoechie-tab-menu div.list-group > a:last-child {
                        border-bottom-right-radius: 0;
                        -moz-border-bottom-right-radius: 0;
                    }

                    div.bhoechie-tab-menu div.list-group > a.active,
                    div.bhoechie-tab-menu div.list-group > a.active .glyphicon,
                    div.bhoechie-tab-menu div.list-group > a.active .fa {
                        background-color: #fff;
                        background-image: #5A55A3;
                        color: #08769c;
                    }

                        div.bhoechie-tab-menu div.list-group > a.active:after {
                            content: '';
                            position: absolute;
                            left: 100%;
                            top: 50%;
                            margin-top: -13px;
                            border-left: 0;
                            border-bottom: 13px solid transparent;
                            border-top: 13px solid transparent;
                            border-left: 10px solid #08769c;
                        }

        div.bhoechie-tab-content {
            background-color: #fff;
            /* border: 1px solid #eeeeee; */
            /*padding-left: 20px;*/
            padding-top: 0px;
        }

        div.bhoechie-tab div.bhoechie-tab-content:not(.active) {
            display: none;
        }

        div.bhoechie-tab {
            width: 78% !important;
        }

        div.bhoechie-tab-menu {
            width: 22% !important;
        }
         div.a{
            white-space:nowrap;
            max-width: 1499px;
            overflow: hidden ;
            text-overflow:ellipsis;
            display:block;
           
        }
        div.a:hover{
            overflow: hidden;
        }
       
    </style>

   
</asp:Content>
<asp:Content ID="PatientChartBody" ContentPlaceHolderID="C5POBody" runat="server">
    <telerik:RadWindowManager ID="WindowMngr" runat="server" EnableViewState="false">
        <Windows>
            <telerik:RadWindow ID="RadModalWindow" runat="server" Behaviors="Resize"
                EnableViewState="false" Title="Viewer" VisibleOnPageLoad="false" Modal="true"
                IconUrl="Resources/16_16.ico" OpenerElementID="Button1" VisibleStatusbar="False" Style="top: 100px;">
            </telerik:RadWindow>
            <telerik:RadWindow ID="RadWindowNotification" runat="server" Behaviors="Resize,Move,Close"
                EnableViewState="false" Title="Notification" VisibleOnPageLoad="false" Modal="true"
                IconUrl="Resources/16_16.ico">
            </telerik:RadWindow>
            <telerik:RadWindow ID="RadWindowSummary" runat="server" Behaviors="Resize,Move,Close"
                EnableViewState="false" Title="Summary" VisibleOnPageLoad="false" Modal="true"
                IconUrl="Resources/16_16.ico">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <telerik:RadFormDecorator ID="RadFormDecorator1" DecoratedControls="All" runat="server"
        DecorationZoneID="CenPercentWidth" EnableViewState="false"></telerik:RadFormDecorator>
    <body onload="{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}">
        <div id="CenPercentWidth" style="margin: 0px; padding: 0px; position: relative">
            <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            </telerik:RadAjaxManager>
            <telerik:RadScriptManager ID="RadScriptManager1" runat="server" EnableViewState="false">
                <Scripts>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                    <%--<asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>--%>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
                </Scripts>
            </telerik:RadScriptManager>
            <telerik:RadToolTip ID="vitalsTip" VisibleOnPageLoad="false" runat="server" Width="300px"
                AutoCloseDelay="25000">
            </telerik:RadToolTip>

            <%--<telerik:RadToolTip ID="RadToolTip1" VisibleOnPageLoad="true" runat="server" Width="0px"
                Position="BottomLeft" AutoCloseDelay="1">
            </telerik:RadToolTip>--%>
             
            <div style="height: 102px; margin-top: -3px; margin-bottom: 3px;" >
                <table id="tblPatientSummaryBar" style="width: 100%; height: 25px; cellpadding: 0px; cellspacing: 0px;">
                    <tr style="height: 100px; padding: 0px; margin: 0px">
                        <td style="width: 90%">


                            <div style="height: 25px; margin-top: -5px; margin-bottom: 3px;width: 118%;" class="a">
                                <table id="tblPatientStrip" style="width: 100%; height: 25px; margin-top: 1px; cellpadding: 0px; cellspacing: 0px;">
                                    <tr style="margin: 0px; padding: 0px; height: 35px">
                                        <td style="width: 100%; margin-left: 40px; margin-top: 0px; margin-bottom: 0px;">

                                            <fieldset id="lblPatientStrip" runat="server" onmouseover="showTip(this)" text="Root RadPanelItem1" onclientclicked="lblPatientStrip_ItemClicked" name="lblPatientStrip" data-tooltp="PatientStrip_tooltp" maxlength="60"
                                                class="pnlBarGroup " style="max-width: 1499%; height: 24%; background-color: #FFFFF1; background-image: none; vertical-align: middle; padding-top: 0.3%; margin-top: -1.5%; position: relative; padding-left: 0%; border: 0px !important">
                                            </fieldset>
                                        </td>
                                    </tr>
                                    <tr>
                                        <%--<td> 
                                            <span runat="server" id="spnPatientstrip1"  class="fa fa-medkit" style="font-size:20px;color:#057105;cursor:pointer;position:absolute;margin-top:-2.1%;margin-left:88%;" 
                                                onclick="getInsuranceDetails();"></span>
                                        </td>--%>
                                    </tr>
                                </table>
                            </div>
                            <div id="divpanel" style="margin: 0px; padding: 0px; height: 70px; margin-top: -6px;">
                                <table id="tblPatientSummary" style="width: 100%; height: 70px; table-layout: fixed"
                                    cellpadding="0" cellspacing="0" class="boxModel" runat="server">
                                    <tr style="height: 70px; padding: 0px; margin: 0px">
                                        <td>
                                            <asp:Panel ID="pnlSummarybar" runat="server" Style="width: 100%; height: 93px;">
                                                <table id="tblSummary" style="width: 99.9%; height: 63%;">
                                                    <tr>
                                                        <td style="width: 16%; vertical-align: top; background-color: rgb(255,255,192); overflow-x: hidden;">
                                                            <fieldset class="patientchartfontfamily" id="pnlAllergies" runat="server" onmouseover="showtooltip(this)"  onmouseout="hidetooltip();" style="width: 100%; height: 81px; overflow-y: auto; background-image: none; -webkit-padding-before: 0em; -webkit-padding-start: 0em; -webkit-padding-after: 0em;" data-sqre="Allergies_sqre" data-tooltp="Allergies_tooltp">
                                                                <label id="lblAllergies" class="BlockObjects patientchartfontfamily" runat="server" style="width: 100%"></label>

                                                            </fieldset>

                                                        </td>
                                                        <td style="width: 16%; vertical-align: top; background-color: #FFFFFF; overflow-x: hidden;">
                                                            <fieldset class="patientchartfontfamily" id="pnlCheifComplaints" runat="server" onmouseover="showtooltip(this)" onmouseout="hidetooltip();" style="width: 100%; height: 81px; overflow-y: auto; visibility: visible; background-image: none; -webkit-padding-before: 0em; -webkit-padding-start: 0em; -webkit-padding-after: 0em;" data-sqre="CheifComplaints_sqre" data-tooltp="CheifComplaints_tooltp">
                                                                <label id="lblCheifComplaints" class="BlockObjects patientchartfontfamily" runat="server" style="width: 100%; word-wrap: word-wrap: normal; word-break: break-all;"></label>
                                                            </fieldset>
                                                        </td>
                                                        <td style="width: 16%; vertical-align: top; background-color: rgb(255,255,192); overflow-x: hidden;">
                                                            <fieldset class="patientchartfontfamily" id="pnlProblemList" runat="server" onmouseover="showtooltip(this)" onmouseout="hidetooltip();" style="width: 100%; height: 81px; overflow-y: auto; visibility: visible; background-image: none; -webkit-padding-before: 0em; -webkit-padding-start: 0em; -webkit-padding-after: 0em;" data-sqre="ProblemList_sqre" data-tooltp="ProblemList_tooltp">
                                                                <label id="lblProblemList" runat="server" class="BlockObjects patientchartfontfamily" style="width: 100%;">
                                                                </label>
                                                            </fieldset>
                                                        </td>
                                                        <td style="width: 16%; vertical-align: top; background-color: #FFFFFF; overflow-x: hidden;">
                                                            <fieldset class="patientchartfontfamily" id="pnlVitals" runat="server" onmouseover="showtooltip(this)" onmouseout="hidetooltip();"  style="width: 100%; height: 81px; overflow-y: auto; visibility: visible; background-image: none; -webkit-padding-before: 0em; -webkit-padding-start: 0em; -webkit-padding-after: 0em;" data-sqre="Vitals_sqre" data-tooltp="Vitals_tooltp">
                                                                <label id="lblVitals" runat="server" class="BlockObjects patientchartfontfamily" style="width: 100%;" name="lblVitals">
                                                                </label>
                                                            </fieldset>
                                                        </td>
                                                        <td style="width: 16%; vertical-align: top; background-color: rgb(255,255,192); overflow-x: hidden;">
                                                            <fieldset class="patientchartfontfamily" id="pnlMedication" runat="server" onmouseover="showtooltip(this)" onmouseout="hidetooltip();"  style="width: 100%; height: 81px; overflow-y: auto; overflow-x: hidden; visibility: visible; background-image: none; -webkit-padding-before: 0em; -webkit-padding-start: 0em; -webkit-padding-after: 0em;" data-sqre="Medication_sqre" data-tooltp="Medication_tooltp">
                                                                <label id="lblMedication" runat="server" class="BlockObjects patientchartfontfamily" style="width: 100%;">
                                                                </label>
                                                            </fieldset>
                                                        </td>
                                                        <td style="width: 14%; vertical-align: top; background-color: #FFFFFF; overflow-x: hidden;">
                                                            <fieldset class="patientchartfontfamily" id="pnlRAF" runat="server" onmouseover="showtooltip(this)" onmouseout="hidetooltip();"  style="width: 100%; height: 81px; overflow-y: auto; visibility: visible; background-image: none; -webkit-padding-before: 0em; -webkit-padding-start: 0em; -webkit-padding-after: 0em;" data-sqre="RAF_sqre" data-tooltp="RAF_tooltp">
                                                                RAF Score :<a title="Refresh" onclick="RAFRefreshCLick()" style="text-underline-position: below; text-decoration: underline; cursor: pointer"> Refresh</a>
                                                                <br />
                                                                <label id="lblRAF" class="BlockObjects patientchartfontfamily" runat="server" style="width: 100%"></label>
                                                            </fieldset>
                                                        </td>

                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                                <%--font-family: calibri!important;font-size:14px!important;--%>
                                <span class="tooltpShow" style="position: absolute; border: 1px solid rgb(204, 204, 204); border-radius: 6px; color: black; font-weight: normal; margin-top: -53px; margin-left: 92px; padding: 6px; z-index: 999; display: none; background-color: white; max-height: 600px; overflow-y: auto;" id="Allergies_tooltp"></span>
                                <span class="tooltpShow" id="Allergies_sqre" style="position: absolute; border-bottom-width: 1px; border-bottom-style: solid; border-bottom-color: rgb(204, 204, 204); margin-left: 86px; margin-top: -46px; width: 12px; height: 12px; transform: rotate(45deg); border-left-width: 1px; border-left-style: solid; border-left-color: rgb(204, 204, 204); display: none; background-color: white; z-index: 999;"></span>
                                <span class="tooltpShow" style="position: absolute; border: 1px solid rgb(204, 204, 204); border-radius: 6px; color: black; font-weight: normal; margin-top: -53px; margin-left: 365px; padding: 6px; z-index: 999; display: none; background-color: white; max-height: 600px; overflow-y: auto;" id="CheifComplaints_tooltp"></span>
                                <span class="tooltpShow" id="CheifComplaints_sqre" style="position: absolute; border-bottom-width: 1px; border-bottom-style: solid; border-bottom-color: rgb(204, 204, 204); margin-left: 359px; margin-top: -46px; width: 12px; height: 12px; transform: rotate(45deg); border-left-width: 1px; border-left-style: solid; border-left-color: rgb(204, 204, 204); display: none; background-color: white; z-index: 999;"></span>
                                <span class="tooltpShow" style="position: absolute; border: 1px solid rgb(204, 204, 204); border-radius: 6px; color: black; font-weight: normal; margin-top: -53px; margin-left: 598px; padding: 6px; z-index: 999; display: none; background-color: white; max-height: 600px; overflow-y: auto;" id="ProblemList_tooltp"></span>
                                <span class="tooltpShow" id="ProblemList_sqre" style="position: absolute; border-bottom-width: 1px; border-bottom-style: solid; border-bottom-color: rgb(204, 204, 204); margin-left: 592px; margin-top: -46px; width: 12px; height: 12px; transform: rotate(45deg); border-left-width: 1px; border-left-style: solid; border-left-color: rgb(204, 204, 204); display: none; background-color: white; z-index: 999;"></span>
                                <span class="tooltpShow" style="position: absolute; border: 1px solid rgb(204, 204, 204); border-radius: 6px; color: black; font-weight: normal; margin-top: -53px; margin-left: 760px; padding: 6px; z-index: 999; display: none; width: 290px; background-color: white; max-height: 600px; overflow-y: auto;" id="Vitals_tooltp"></span>
                                <span class="tooltpShow" id="Vitals_sqre" style="position: absolute; border-bottom-width: 1px; border-bottom-style: solid; border-bottom-color: rgb(204, 204, 204); margin-left: 754px; margin-top: -46px; width: 12px; height: 12px; transform: rotate(45deg); border-left-width: 1px; border-left-style: solid; border-left-color: rgb(204, 204, 204); display: none; background-color: white; z-index: 999;"></span>
                                <span class="tooltpShow" style="position: absolute; border: 1px solid rgb(204, 204, 204); border-radius: 6px; color: black; font-weight: normal; margin-top: -53px; margin-left: 1022px; padding: 6px; z-index: 999; display: none; width: 252px; max-width: 252px; background-color: white; max-height: 600px; overflow-y: auto;" id="Medication_tooltp"></span>
                                <span class="tooltpShow" id="Medication_sqre" style="position: absolute; border-bottom-width: 1px; border-bottom-style: solid; border-bottom-color: rgb(204, 204, 204); margin-left: 1016px; margin-top: -46px; width: 12px; height: 12px; transform: rotate(45deg); border-left-width: 1px; border-left-style: solid; border-left-color: rgb(204, 204, 204); display: none; background-color: white; z-index: 1000;"></span>
                                <span class="tooltpShow" style="position: absolute; border: 1px solid rgb(204, 204, 204); border-radius: 6px; color: black; font-weight: normal; margin-top: -53px; margin-left: 1154px; padding: 6px; z-index: 999; display: none; background-color: white; max-height: 600px; overflow-y: auto;" id="RAF_tooltp"></span>
                                <span class="tooltpShow" id="RAF_sqre" style="position: absolute; border-bottom-width: 1px; border-bottom-style: solid; border-bottom-color: rgb(204, 204, 204); margin-left: 1148px; margin-top: -46px; width: 12px; height: 12px; transform: rotate(45deg); border-left-width: 1px; border-left-style: solid; border-left-color: rgb(204, 204, 204); display: none; background-color: white; z-index: 999;"></span>
                                <span class="tooltpShow" style="position: absolute; border: 1px solid rgb(204, 204, 204); border-radius: 6px; color: black; font-weight: normal; margin-top: -45px; margin-left: 950px; width: 300px; margin-right: 7px; padding: 6px; z-index: 999; display: none; background-color: white; max-height: 600px; overflow-y: auto;" id="imgOverAllSummary_tooltp"></span>
                                <span class="tooltpShow" id="imgOverAllSummary_sqre" style="position: absolute; border-top-width: 1px; border-top-style: solid; border-top-color: rgb(204, 204, 204); margin-top: -42px; margin-left: 1242px; width: 12px; height: 12px; transform: rotate(135deg); border-left-width: 1px; border-left-style: solid; border-left-color: rgb(204, 204, 204); z-index: 998; display: none; background-color: white;"></span>
                            </div>
                        </td>
                        <td style="width: 10%">
                            <asp:Image ID="imgOverAllSummary" Width="97%" onmouseover="showtooltip(this)" onmouseout="hidetooltip();" Height="105px" ImageAlign="Top" runat="server" data-sqre="imgOverAllSummary_sqre" data-tooltp="imgOverAllSummary_tooltp" margin-left="-1%"
                                CssClass="displayInline boxModel DockImage" ImageUrl="~/Resources/person.gif" />
                        </td>
                    </tr>
                </table>

            </div>
            <div style="margin-top: 0px; width: 100%;">
                <%--<div id='jqxSplitter' style="height: 100%; width: 100%;">
                    <div style="background-color: #BFDBFF; border: solid 1px #969696; width: 19%; height: 100%; display: inline-block; position: relative;">
                        <div id='divLoading' class='modal' style="margin-top: 90px; overflow-x: hidden; overflow-y: hidden; margin-left: 105px; height: 460px; width: 240px; top: 165px; display: none">
                            <br />
                            <br />
                            <br />
                            <br />
                            <center></center>
                            <br />
                            <img src='Resources/wait.ico' title='[Please wait while the page is loading...]' alt='Loading...' /><br />
                        </div>
                        <div style="cursor: pointer;" id="dvTest" runat="server">
                            <div id='divLoadingPatChart' class='modal' style="height: 460px; width: 104%; position: absolute; bottom: 0px; display: none">
                                <br />
                                <br />
                                <br />
                                <br />
                                <center></center>
                                <br />
                                <img src='Resources/loadimage.gif' style="opacity: 1; height: 15px; width: 15px; position: absolute; top: 197px; left: 40%; position: center;" title='[Please wait while the page is loading...]' alt='Loading...' />
                                <br />
                            </div>
                            <details style="border: none;" onclick="return CheckMe();" id="PatChart">
                                <summary style="height: 20px; border: none;">Patient Chart</summary>
                                <div id="dvCheck" class="checkFade" style="height: 450px; width: 237px; background-color: #bfdbff; display: none; border: solid 1px #969696; overflow-y: auto; overflow-x: hidden; -webkit-padding-before: 0em; -webkit-padding-start: 0em; -webkit-padding-after: 0em;" runat="server">
                                    <div class="treeview" data-role="treeview" data-on-click="tree_add_leaf_example_click" id="divTreeview">
                                    </div>
                                </div>
                            </details>

                            <div id="Test" style="display: block;">
                                <label id="lblPatient" runat="server" style="display: block; font-style: italic;">Expand to load the Patient Chart</label>
                            </div>

                            <br />

                        </div>
                        <div class="notify" style="height: 206px; width: 90%; display: block; position: absolute; bottom: 0px;" >
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
                            <div style="width: 96%; border-bottom: 1px solid red" id="divnotification" runat="server"><span style="color: red"><b>Notification</b></span></div>
                            <div id="dvnotify"  runat="server" style="margin-top: 4px; width: 93.8%; position: absolute; height: 174px; font-size: 14px; display: block; overflow-y: auto; overflow-x: hidden;"></div>
                        </div>

                    </div>
                    <div style="display: inline-block; height: 100%; width: 80.5%; left: 19.9%!important;">
                        <iframe id="EncounterContainer" runat="server" style="height: 100%; width: 100%;"></iframe>
                    </div>
                </div>--%>
                <%--<div id='jqxSplitter' style="height: 100%; width: 100%;">
                    <div style="display: inline-block; height: 100%; background-color: white; width: 4%;">
                        <div style="display: inline-block; position: absolute;">
                            <img src="Resources/icon-documentation.png" style="width: 15%; margin-top: -18px; margin-left: 6px;" onclick="return CheckMe();" />
                            <p style="font-family: Microsoft sans pro; padding-left: 3px; padding-top: 4px; font-size: 13px; font-weight: bold;">Charts </p>
                        </div>
                        <div class="container" style="height: 710px; display: inline-block; margin-left: 109%; /* margin-top: -4%; */">
                            <div class="row" style="margin-top: -28px;">
                                <div class="col-lg-5 col-md-5 col-sm-8 col-xs-9 bhoechie-tab-container" id="divPatChartContainer" style="width: 245px; display: none;">
                                    <div class="col-lg-9 col-md-9 col-sm-9 col-xs-9 bhoechie-tab" style="height: 710px;">
                                        <div class="bhoechie-tab-content active">
                                            <div id="ctl00_C5POBody_dvTest" style="cursor: pointer; display: block; position: absolute;">
                                                <div id="divLoadingPatChart" class="modal" style="height: 740px; width: 237px; position: absolute; bottom: 0px; display: none;">
                                                    <br>
                                                    <br>
                                                    <br>
                                                    <br>
                                                    <center></center>
                                                    <br>
                                                    <img src="Resources/loadimage.gif" style="opacity: 1; height: 15px; width: 15px; position: absolute; top: 197px; left: 40%; position: center;" title="[Please wait while the page is loading...]" alt="Loading...">
                                                    <br>
                                                </div>
                                                <div>
                                                    <div id="ctl00_C5POBody_dvCheck" class="checkFade" style="height: 740px; width: 237px; display: block; overflow-y: auto; overflow-x: hidden; margin-left: -29px; -webkit-padding-before: 0em; -webkit-padding-start: 0em; -webkit-padding-after: 0em;">
                                                        <div class="treeview" data-role="treeview" data-on-click="tree_add_leaf_example_click" id="divTreeview">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div style="display: inline-block; height: 100%; width: 95.4%;">
                        <div style="height: 100%;">
                            <iframe id="EncounterContainer" runat="server" style="height: 100%; width: 100%;"></iframe>
                        </div>
                    </div>
                </div>--%>
                 <div id='jqxSplitter' style="height: 100%; width: 100%;">
                    <div style="display: inline-block; height: 100%; width: 5%;" class="patientchart">
                         <table>
                              <tr>
                                <td>
                            
                            <i class="fas fa-notes-medical" style="margin-left: 24px;color:#3275B1;font-size:25px;margin-top: 6px;cursor: pointer;" onclick="return CheckMe('Encounters',this);" id="Encountersimg" ></i>
                           
                            <p class="patientchartformat" style="font-weight: bold; padding-top: 2px; font-size: 11px; margin-left: 4px; text-align: center;cursor: pointer;" onclick="return CheckMe('Encounters',Encountersimg);" id="EncountersText">
                                Encounters
                            </p>
                              <hr style="height:1.5px;border-width:0;color:gray;background-color:#dedede;margin-top: 5px;margin-bottom: 5px;width:71px;"/>
                          </td>
                            </tr>
                            <tr>
                                <td>
                               
                            <i class="fas fa-vial" style="margin-left: 24px;color:#3275B1;font-size:25px;cursor: pointer;"  onclick="return CheckMe('Results',this);" id="Resultsimg"></i>
                           
                            <p class="patientchartformat" style="font-weight: bold; padding-top: 2px; font-size: 11px; margin-left: 4px; text-align: center;cursor: pointer;" onclick="return CheckMe('Results',Resultsimg);" id="ResultsText">
                                Results
                            </p>
                                <hr style="height:1.5px;border-width:0;color:gray;background-color:#dedede;margin-top: 5px;margin-bottom: 5px;width:71px;" id="ResultLine"/>
                          </td>
                              
                            </tr>
                            <tr>
                                <td>
                                   
                            <i class="fas fa-tasks"  style="margin-left: 24px;color:#3275B1;font-size:25px;cursor: pointer;"  onclick="return CheckMe('Patient Task',this);" id="PatientTaskimg" ></i>
                            <p class="patientchartformat" style="font-weight: bold; padding-top: 2px; font-size: 11px; margin-left: 4px; text-align: center;cursor: pointer;" onclick="return CheckMe('Patient Task',PatientTaskimg);" id="PatientTaskText">
                                Patient Tasks
                            </p>
                                 <hr style="height:1.5px;border-width:0;color:gray;background-color:#dedede;margin-top: 5px;margin-bottom: 5px;width:71px;"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                            <i class="fas fa-diagnoses"  style="margin-left: 20px;color:#3275B1;font-size:25px;cursor: pointer;"  onclick="return CheckMe('Exam',this);" id="Examimg"></i>
                            <p class="patientchartformat" style="font-weight: bold; padding-top: 2px; font-size: 11px; margin-left: 4px; text-align: center;cursor: pointer;" onclick="return CheckMe('Exam',Examimg);" id="ExamText">
                                Exam Photos
                            </p>
                                    <hr style="height:1.5px;border-width:0;color:gray;background-color:#dedede;margin-top: 5px;margin-bottom: 5px;width:71px;"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                            <i class="fas fa-folder-open"  style="margin-left: 24px;color:#3275B1;font-size:25px;cursor: pointer;"  onclick="return CheckMe('Document',this);" id="Documentimg"></i>
                            <p class="patientchartformat" style="font-weight: bold; padding-top: 2px; font-size: 11px; margin-left: 4px; text-align: center;cursor: pointer;" onclick="return CheckMe('Document',Documentimg);" id="DocumentText">
                                Patient Documents
                            </p>
                                    <hr style="height:1.5px;border-width:0;color:gray;background-color:#dedede;margin-top: 5px;margin-bottom: 5px;width:71px;"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                            <i class="fas fa-exchange-alt"  style="margin-left: 24px;color:#3275B1;font-size:25px;cursor: pointer;"  onclick="return CheckMe('Summary of Care',this);" id="SummaryofCareimg"></i>
                            <p style="font-weight: bold; padding-top: 2px; font-size: 11px; margin-left: 4px; text-align: center;cursor: pointer; font-family:'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;" onclick="return CheckMe('Summary of Care',SummaryofCareimg);" id="SummaryofCareText">
                                Summary of Care
                            </p>
                                    <hr style="height:1.5px;border-width:0;color:gray;background-color:#dedede;margin-top: 5px;margin-bottom: 5px;width:71px;"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                            <i class="fas fa-chart-line"  style="margin-left: 24px;color:#3275B1;font-size:25px;cursor: pointer;" onclick="return CheckMe('Analytics',this);" id="Analyticsimg"></i>
                            <p class="patientchartformat" style="font-weight: bold; padding-top: 2px; font-size: 11px; margin-left: 4px; text-align: center;cursor: pointer;" onclick="return CheckMe('Analytics',Analyticsimg);" id="AnalyticsText">
                                Analytics
                            </p>
                                    <hr style="height:1.5px;border-width:0;color:gray;background-color:#dedede;margin-top: 5px;margin-bottom: 5px;width:71px;"/>
                                </td>
                            </tr>
                        </table>


                    </div>
                    

                    <div style="display: inline-block; height: 100%; width: 94.4%;">
                        <div style="cursor: pointer; display: none; position: absolute;" id="dvTest" runat="server">
                            <div id='divLoadingPatChart' class='modal' style="height: 710px; width: 258px; position: absolute; bottom: 0px; display: none">
                                <br />
                                <br />
                                <br />
                                <br />
                                <center></center>
                                <br />
                                <img src='Resources/loadimage.gif' style="opacity: 1; height: 15px; width: 15px; position: absolute; top: 197px; left: 40%; position: center;" title='[Please wait while the page is loading...]' alt='Loading...' />
                                <br />
                            </div>
                            <div class="container" style="height: 710px; display: inline-block; width: 260px; padding-top: 3px; padding-left: 2px!important;">
                                <div>
                                    <div class="col-lg-5 col-md-5 col-sm-8 col-xs-9 bhoechie-tab-container" id="divPatChartContainer" style="width: 260px; display: none;">
                                        <div class="col-lg-9 col-md-9 col-sm-9 col-xs-9 bhoechie-tab" style="height: 710px; width: 100%; padding-top: 6px;">
                                            <div class="bhoechie-tab-content active">
                                                <div id="dvCheck" class="checkFade" style="height: 700px; width: 240px; display: none; overflow-y: auto; overflow-x: hidden; -webkit-padding-before: 0em; -webkit-padding-start: 0em; -webkit-padding-after: 0em;" runat="server">
                                                    <div class="treeview" data-role="treeview" data-on-click="tree_add_leaf_example_click" id="divTreeview">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                            <div id="WaitingMessage" style=" display: none; height: 100%; width: 100%;"> <label style=" font-family: Helvetica Neue,Helvetica,Arial,sans-serif !important;
    font-size: 13px !important;
    font-weight: bold;"> Summary is loading. Please wait.</label>
                            </div>
                        <div style="height: 100%;">
                            <iframe id="EncounterContainer" runat="server" style="display: block; height: 100%; width: 100%;"></iframe>
                        </div>
                    </div>
                </div>
            </div>
    </body>
    <asp:HiddenField ID="hdnLocalTime" runat="server" EnableViewState="false" />
    <asp:HiddenField ID="hdnOrderSubmitId" runat="server" EnableViewState="false" />
    <asp:HiddenField ID="hdnHumanNo" runat="server" EnableViewState="false" />
    <asp:HiddenField ID="hdnResultId" runat="server" EnableViewState="false" />
    <asp:HiddenField ID="hdnIndexId" runat="server" EnableViewState="false" />
    <asp:HiddenField ID="hdnIsSaveEnable" runat="server" Value="false" EnableViewState="false" />
    <asp:HiddenField ID="hdnPFSHOthertxt" runat="server" Value="false" EnableViewState="false" />
    <asp:HiddenField ID="hdnSocialHistoryMandatory" runat="server" Value="false" EnableViewState="false" />
    <asp:HiddenField ID="hdnTabClick" runat="server" Value="first" EnableViewState="false" />
    <asp:HiddenField ID="hdnSaveButtonID" runat="server" EnableViewState="false" />
    <asp:HiddenField ID="hdnAddendumID" runat="server" EnableViewState="false" />
    <asp:HiddenField ID="hdnFacilityRole" runat="server" EnableViewState="false" />
    <asp:HiddenField ID="hdnccAutosave" runat="server" Value="false" EnableViewState="false" />
    <asp:HiddenField ID="hdnOwnerEncMismatch" runat="server" EnableViewState="false" />
    <asp:HiddenField ID="hdnOwnerEncMismatchEncID" runat="server" EnableViewState="false" />
     <asp:HiddenField ID="hdnSummaryEncID" runat="server" EnableViewState="false" />
    <asp:HiddenField ID="hdnSummaryPageFlag" runat="server" EnableViewState="false" />
    <asp:HiddenField ID="hdnQencounterId" runat="server" Value="" EnableViewState="false" />
    <asp:HiddenField ID="hdnPatientStrip" runat="server" EnableViewState="false" />
    <asp:Button ID="hdnbtngeneratexml" runat="server" OnClick="hdnbtngeneratexml_Click"  style="display:none" />

        <script src="JScripts/jquery-2.1.3.js"></script>
        <link href="CSS/metro.css" rel="stylesheet" />
        <script type="text/javascript" src="JScripts/metro.js"></script>
        <link href="CSS/jqx.base.min.css" rel="stylesheet" />
        <script src="JScripts/tooltip.min.js" type="text/javascript"></script>
        <%-- <script src="JScripts/jqxcore.min.js" type="text/javascript"></script>
        <script src="JScripts/jqxsplitter.min.js" type="text/javascript"></script>--%>
        <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>"
            type="text/javascript"></script>
        <script src="JScripts/jsPatientChart.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>"
            type="text/javascript"></script>
        <script type="text/javascript">
            window.onbeforeunload = function OnUnload() {
                if (Result != undefined) {
                    if (false == Result.closed) {

                        Result.close();
                    }
                }
            }
            if (sessionStorage.getItem("nav") == null) {
                var ProjectName = '<%=ConfigurationManager.AppSettings["ProjectName"]%>';
                //Jira - #CAP-80
                //sessionStorage.setItem("Projname", ProjectName.trim().toUpperCase());
                if (ProjectName != undefined && ProjectName != null)
                localStorage.setItem("Projname", ProjectName.trim().toUpperCase());
                var ReportPath = '<%=ConfigurationManager.AppSettings["Reportpath"]%>';
                sessionStorage.setItem("ReportPath", ReportPath);
                var nav = '<%=ConfigurationManager.AppSettings["nav"]%>';
                sessionStorage.setItem("nav", nav);
                var navhover = '<%=ConfigurationManager.AppSettings["navhover"]%>';
                sessionStorage.setItem("navhover", navhover);
                var Tableheader = '<%=ConfigurationManager.AppSettings["Tableheader"]%>';
                sessionStorage.setItem("Tableheader", Tableheader);
             <%--   var Encounterheader1 = '<%=ConfigurationManager.AppSettings["Encounterheader1"]%>';
                sessionStorage.setItem("Encounterheader1", Encounterheader1);--%>
                var versionkey = '<%=ConfigurationManager.AppSettings["versionkey"]%>';
            }
        </script>
    </asp:PlaceHolder>
</asp:Content>
