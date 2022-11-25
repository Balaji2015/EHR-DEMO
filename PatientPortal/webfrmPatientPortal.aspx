<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="webfrmPatientPortal.aspx.cs" Inherits="Acurus.Capella.PatientPortal.webfrmPatientPortal" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="aspx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en" lang="en">
<head id="Head1" runat="server">
    <title>CAPELLA 5.0</title>
    <link id="Link1" runat="server" rel="shortcut icon" href="Resources/16_16.ico" type="image/x-icon" />
    <script src="JScripts/jquery-2.1.3.js"></script>
    <link href="CSS/metro.css" rel="stylesheet" />
    <script type="text/javascript" src="JScripts/metro.js"></script>
    <link href="CSS/jqx.base.min.css" rel="stylesheet" />
    <script src="JScripts/tooltip.min.js" type="text/javascript"></script>
    <style>
        .newStyle1 {
            background-color: #BFDBFF;
        }

        #CheckAlert {
            display: none;
            background: #fdfeff;
            box-shadow: 0 0 10px rgba(0,0,0,0.4);
            box-sizing: border-box;
            color: #101010;
            left: 50%;
            min-width: 645px;
            max-width: 700px;
            padding: 1.875em;
            position: absolute;
            top: 7%;
            transform: translate(-50%, -50%);
            z-index: 2000000000;
            border-radius: 10px;
            opacity: 0.8;
        }

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


        .colored {
            background-color: #bfdbff;
            padding: 3px;
            border-radius: 3px;
            color: black;
            font-weight: bold;
        }

        .SmallFont {
            font-size: small;
            font-family: Microsoft Sans Serif;
        }

        .boxModel {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
        }

        * {
            font-size: 11.5px;
            font-family: Microsoft Sans Serif;
        }

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

        div#ctl00_C5POBody_dvCheck > div > ul > li > span {
            font-family: Microsoft sans pro;
            font-size: 15.3px;
        }

        li p {
            line-height: 0.9rem !important;
        }
    </style>

</head>
<body bgcolor="#bfdbff" text="#000000" link="#0000ee" vlink="#0000ee" alink="#0000ee">
    <form id="form1" runat="server">
        <telerik:RadWindowManager ID="WindowMngrSendMail" runat="server">
            <Windows>
                <telerik:RadWindow ID="SendMailWindow" ShowContentDuringLoad="true" runat="server"
                    Behaviors="Close" Title="Send Message" IconUrl="Resources/16_16.ico">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <telerik:RadWindowManager EnableViewState="false" Overlay="true" ID="WindowMngr"
            runat="server">
            <Windows>
                <telerik:RadWindow ID="MessageWindow" Overlay="true" ShowContentDuringLoad="true"
                    runat="server" Behaviors="Close" Title="Change Password" IconUrl="Resources/16_16.ico">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <div align="left">
            <asp:Label ID="lblEmailIDActual0" runat="server" Font-Bold="True" Font-Italic="True"
                Font-Size="XX-Large" ForeColor="#000099" Text="Patient Online Access Portal"></asp:Label>
            &nbsp;&nbsp;
        <asp:Label ID="lblEmailID" runat="server" Font-Bold="True" Font-Names="Calibri" Text="Logged in as "></asp:Label>
            <asp:Label ID="lblEmailIDActual" runat="server" Style="font-family: Microsoft Sans Serif; font-size: 8.5pt;"
                Text="Email ID"></asp:Label>
        </div>
        <div align="right">
            <asp:Label ID="lblMessage" runat="server" Style="font-family: Microsoft Sans Serif; font-size: 8.5pt;"
                Text=""></asp:Label>
        </div>
        <div align="right">
            <asp:LinkButton ID="lnkadddocument" runat="server" Style="font-weight: 700; font-size: small;">Add Document</asp:LinkButton>&nbsp;
            <asp:LinkButton ID="lnkPatientAccount" runat="server" Style="font-weight: 700; font-size: small">Patient Account</asp:LinkButton>&nbsp;
            <asp:LinkButton ID="lnkActiveHistory" runat="server" Style="font-weight: 700; font-size: small;">Activity History</asp:LinkButton>&nbsp;
            <asp:LinkButton ID="lnkChangePassword" runat="server" Style="font-weight: 700; font-size: small;" OnClientClick="return ChangePasswordClick();">Change Password</asp:LinkButton>&nbsp;
            <asp:LinkButton ID="lnkLogout" runat="server" OnClick="LinkButton1_Click" Style="font-weight: 700; font-size: small">Logout</asp:LinkButton>&nbsp;&nbsp;
        </div>
        <div align="center">
            <table id="tblPatientStrip" style="width: 100%; height: 30px">
                <tr>
                    <td style="width: 100%; margin-left: 40px;">
                        <telerik:RadPanelBar ID="lblPatientStrip" runat="server" Width="100%" BackColor="#BFDBFF"
                            CssClass="newStyle1" Skin="Outlook">
                            <Items>
                                <telerik:RadPanelItem runat="server" Text="Root RadPanelItem1" BackColor="White"
                                    CssClass="newStyle1" Font-Bold="False" ForeColor="Black" Font-Size="Small">
                                </telerik:RadPanelItem>
                            </Items>
                        </telerik:RadPanelBar>
                    </td>
                </tr>
            </table>
        </div>
        <div id='jqxSplitter' style="height: 670px; width: 100%; background-color: #bfdbff;">
            <div style="display: inline-block; height: 100%; width: 5.5%; background-color: #bfdbff;">
                <div id="divPatchart" style="position: absolute; margin-top: 6px; margin-left: 9px;">
                    <img src="Resources/articleicon.png" style="width: 36px; height: 36px; cursor: pointer" onclick="return CheckMe();" alt="document" />
                    <p style="font-family: Microsoft sans pro; font-weight: bold; padding-top: 2px; font-size: 14px; margin-left: -4px; cursor: default">
                        Documents
                    </p>
                    <img src="Resources/Email.png" style="width: 36px; height: 36px; cursor: pointer" id="imgmessage" runat="server" alt="message" />
                    <p style="font-family: Microsoft sans pro; font-weight: bold; padding-top: 2px; font-size: 14px; margin-left: -4px; cursor: default">
                        Messages
                    </p>
                </div>
            </div>
            <div id="CheckAlert" onclick="ToolStripAlertHide();">
                <div id="innerMsgText" style="font-family: Verdana,Arial,sans-serif !important; font-size: 18px; color: #000000;"></div>
            </div>
            <div style="display: inline-block; height: 100%; width: 94%;">
                <div style="cursor: pointer; display: none; position: absolute;" id="dvTest" runat="server">
                    <div id='divLoadingPatChart' class='modal' style="height: 650px; width: 258px; position: absolute; bottom: 0px; display: none">
                        <br />
                        <br />
                        <br />
                        <br />
                        <center></center>
                        <br />
                        <img src='Resources/loadimage.gif' style="opacity: 1; height: 15px; width: 15px; position: absolute; top: 197px; left: 40%; position: center;" title='[Please wait while the page is loading...]' alt='Loading...' />
                        <br />
                    </div>
                    <div class="container" style="height: 660px; display: inline-block; width: 260px; padding-top: 3px; padding-left: 2px!important;">
                        <div>
                            <div class="col-lg-5 col-md-5 col-sm-8 col-xs-9 bhoechie-tab-container" id="divPatChartContainer" style="width: 260px; display: none; height: 666px;">
                                <div class="col-lg-9 col-md-9 col-sm-9 col-xs-9 bhoechie-tab" style="height: 650px; width: 250px; padding-top: 6px;">
                                    <div class="bhoechie-tab-content active">
                                        <div id="dvCheck" class="checkFade" style="height: 660px; width: 250px; display: none; overflow-y: auto; overflow-x: hidden; -webkit-padding-before: 0em; -webkit-padding-start: 0em; -webkit-padding-after: 0em;" runat="server">
                                            <div class="treeview" data-role="treeview" data-on-click="tree_add_leaf_example_click" id="divTreeview">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div style="height: 100%;">
                    <iframe id="EncounterContainer" runat="server" style="height: 100%; width: 100%;"></iframe>
                </div>
            </div>
        </div>
        <div align="center" style="display: none;">
            <asp:Panel ID="pnlDashBoard" BackColor="#bfdbff" runat="server" HorizontalAlign="Left"
                Style="font-family: Microsoft Sans Serif; height: 60px; width: 1110px; font-size: 8.5pt;"
                Font-Bold="true" GroupingText="Dashboard">
                <asp:Label ID="lblDate" runat="server" Style="font-family: Microsoft Sans Serif; font-size: 8.5pt;"
                    Font-Bold="false" Height="22px" Text="Date of Service"></asp:Label>
                &nbsp;
            <asp:DropDownList ID="cboEncounter" runat="server" Height="20px" Width="208px" onchange="getEncounter();">
            </asp:DropDownList>
                <asp:Button ID="btnShowReport" runat="server" Text="Preview" OnClick="btnShowReport_Click"
                    OnClientClick="saveEnabled();" />


            </asp:Panel>
        </div>
        <div align="center" style="display: none;">
            <asp:Panel ID="pnlFrame" GroupingText="Preview" HorizontalAlign="Left" runat="server"
                Style="height: 700px; width: 1100px;" Font-Bold="true">
                <iframe id="frmWord" runat="server" style="height: 650px; background-color: White; width: 1074px;"></iframe>
            </asp:Panel>
        </div>
        <br />
     
            <table style="width:100%">
                <tr style="float:right">
                    <td>

                        <asp:Button ID="btndelete" runat="server" Text="Delete"
                            Width="100px" OnClick="btndelete_Click" />
        <asp:Button ID="btnDownload" runat="server" Text="Download"
            Width="100px" />
                     </td>
                    <td>
    <asp:Button ID="btnSend" runat="server" Text="Send Document" Width="100px" />
                       </td>
                    <td>
              <asp:Button ID="btnBulkAccess" runat="server" Text="Bulk Access" Width="100px" OnClientClick="return openBulkAccess()" Style="display: block" />
            <%--  <asp:Button ID="btngeneratelink" runat="server" Text="Generate Link" Width="100px"  Enabled="false" OnClientClick="return openGenrateLink()" />--%>
                        <%--<asp:Button ID="btnSendMessage" runat="server" Text="Message"
        Width="100px" />--%>
                 
                    </td>
                </tr>
            </table>


       
        <br />
        <telerik:RadWindowManager EnableViewState="false" ID="RadWindowManager1" Overlay="true"
            runat="server">
            <Windows>
                <telerik:RadWindow ID="RadWindow1" IconUrl="~/Resources/16_16.ico" Height="115px"
                    Width="388px" VisibleStatusbar="false" Behaviors="Close" Title="Download" Style="display: none; position: absolute; z-index: 6500;"
                    runat="server" Overlay="true" ReloadOnShow="true"
                    ShowContentDuringLoad="false" Animation="None" KeepInScreenBounds="true" Modal="true">
                    <ContentTemplate>
                        <table width="100%">

                            <tr>
                                <td>
                                    <asp:Label ID="lblDownloadIn" runat="server" Text="Download in" Style="display: none;" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:RadioButton ID="rdnPdf" Text="PDF Format [For Human readable format]" GroupName="Check" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:RadioButton ID="rdnXml" Text="XML Format [For Sharing CCD XML with others]" GroupName="Check" runat="server" /></td>
                            </tr>

                            <tr>
                                <td></td>
                                <td></td>
                                <td align="center">
                                    <asp:Button ID="Button1" runat="server" Text="Download" OnClick="Button1_Click" /></td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <telerik:RadWindowManager EnableViewState="false" ID="SendRecordRdMngr" Overlay="true"
            runat="server">
            <Windows>
                <telerik:RadWindow ID="SendRecordWindow" IconUrl="~/Resources/16_16.ico" Height="135px"
                    Width="340px" VisibleStatusbar="false" Behaviors="Close" Title="Send Record" Style="display: none; position: absolute; z-index: 6500;"
                    runat="server" Overlay="true" ReloadOnShow="true"
                    ShowContentDuringLoad="false" Animation="None" KeepInScreenBounds="true" Modal="true">
                    <ContentTemplate>

                        <table width="100%">

                            <tr>
                                <td>
                                    <asp:RadioButton ID="rbtSendRecordPDF" Text="PDF Format [For Human readable format]" GroupName="Check" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:RadioButton ID="rbtSendRecordXML" Text="XML Format [For Sharing CCD XML with others]" GroupName="Check" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:RadioButton ID="rbtSendRecordBoth" Text="Both" GroupName="Check" runat="server" />
                                </td>
                            </tr>

                            <tr>
                                <td></td>
                                <td></td>
                                <td align="center">
                                    <asp:Button ID="btnSendDocument" runat="server" Width="95%" Text="Ok" OnClick="btnSendDocument_Click" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnsendResult" runat="server" Style="display: none" OnClick="btnsendResult_Click" />
                                </td>

                            </tr>
                        </table>
                    </ContentTemplate>
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <telerik:RadWindow ID="RadWindow2" IconUrl="~/Resources/16_16.ico" Height="380px"
            Width="650px" VisibleStatusbar="false" Behaviors="Close" Title="Activity Log"
            Style="display: none" Overlay="true" Modal="true" runat="server">
            <ContentTemplate>
                <br />
                <table style="width: 100%;">
                    <tr style="width: 100%;">
                        <td>
                            <asp:TextBox ID="txtActivityLog" Wrap="true" TextMode="MultiLine"
                                Style="font-family: Microsoft Sans Serif; font-size: 8.5pt;" Height="270px" Width="620px"
                                runat="server" ReadOnly="true"/>
                        </td>
                    </tr>
                    <tr style="width: 100%;">
                        <td style="width: 100%;" align="right">
                            <asp:Button ID="btnClose" runat="server" Text="Close" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </telerik:RadWindow>

        <asp:HiddenField ID="HumanID" runat="server" />
        <asp:HiddenField ID="hdnPatientName" runat="server" />
        <asp:HiddenField ID="hdnLocalTime" EnableViewState="false" runat="server" />
        <asp:HiddenField ID="hdnPatientID" EnableViewState="false" runat="server" />
        <asp:HiddenField ID="hdnEmailID" EnableViewState="false" runat="server" />
        <asp:HiddenField ID="hdnRole" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnEncounterId" runat="server" />
        <asp:HiddenField ID="hdnfilepath" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnindexid" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnMailDetails" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnEncList" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnIsZip" runat="server" EnableViewState="false" />
        <%--BugID:49297--%>
        <asp:HiddenField ID="hdnbulkaccess" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnPatientPortal" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnXmlPath" runat="server" />
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" EnableViewState="false">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
            </Scripts>
        </telerik:RadScriptManager>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <script src="JScripts/jquery-2.1.3.js"></script>
            <script type="text/javascript" src="JScripts/metro.js"></script>

            <script src="JScripts/tooltip.min.js" type="text/javascript"></script>
            <script src="JScripts/JSPatientPortal.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

        </asp:PlaceHolder>
    </form>
</body>
</html>
