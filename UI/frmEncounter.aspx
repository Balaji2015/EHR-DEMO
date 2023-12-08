<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmEncounter.aspx.cs" Inherits="Acurus.Capella.UI.frmEncounter" EnableEventValidation="false"%>



<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.7.2/font/bootstrap-icons.css" />
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
        label {
            font-weight: lighter !important;
        }

        .noWrapText {
            white-space: nowrap;
        }

        .DisplayNone {
            display: none;
        }

        .DisplayControlInline {
            display: inline;
        }
        soc
        .displayTableCell {
            display: table-cell;
        }

        .InheritSize {
            size: inherit;
        }

        .RightAlign {
            position: absolute;
            right: 0px;
        }

        .pnlControlFlowFloatLeft {
            float: left;
        }

        .pnlControlFlowFloatRight {
            float: right;
        }

        .RadTabStrip1 {
            font-family: Microsoft Sans Serif;
        }

        #RadAjaxPanel1Panel {
            display: inline;
        }

        .ui-widget-header, .ui-state-default, ui-button {
            background: #b9cd6d;
            border: 1px solid #b9cd6d;
            color: #FFFFFF;
            font-weight: bold;
        }

        .nav > li > a {
            padding: 2px 8px !important;
        }
        /*border: 2px solid rgb(191, 219, 255);*/
        .nav > li {
            /*border: 2px solid rgb(191, 219, 255);*/
            /*border-radius: 12px !important;*/
            margin-top: 5px !important;
        }

        .row {
            width: 105%;
        }

        ul {
            margin-top: 5px !important;
            margin-bottom: 2px !important;
            height: 32px !important;
            /*margin-left: -52px !important;
            width: 66pc;*/
            /*background-color: rgb(191, 219, 255);*/
        }

        :focus {
            outline: none !important;
        }

        a {
            /*background-color: #f5f4f4 !important;*/
            /*font-weight: bold;
            padding-top: 3px !important;
            font-size: smaller;
            font-weight: bold;
            color: black !important;
            border-color: #adadad;*/
            color: black !important;
        }

        .nav-tabs {
            border-bottom: 0px !important;
            /*border-bottom: 1px solid #dedede !important;*/
        }

        .container {
            margin-right: 0px !important;
            width:100%!important;
            /*margin-left: 36px !important;*/
        }

        .nav-tabs > li > a {
            border-radius: 4px !important;
            margin-right: 0px !important;
        }

        .not-active {
            pointer-events: none;
            cursor: default;
        }

        .rwControlButtons {
            height: 20px !important;
            margin-top: 0px !important;
            width: 30px;
            background-color: #FAFAFA !important;
        }

        .rwCloseButton {
            background-color: #FAFAFA !important;
        }

        #cboPhysicianName {
            font-size: 12px !important;
        }
         .disableTab{
            background-color:#a4a4a4!important;
            cursor:default!important;
        }
         #pnlEncScroll{
             padding-left:0px!important;
        }
         .enwrapper{
            /*background-color: rgb(191, 219, 255);*/
              margin: 0px; padding: 0px;
        }
    </style>
    <link href="CSS/CommonStyle.css?version=1.2" rel="stylesheet" type="text/css" />
    <script type="text/javascript" id="telerikClientEvents1">



</script>
    <link href="CSS/fontawesomenew.css" rel="stylesheet" />
    <link href="CSS/bootstrap.min.css" rel="Stylesheet" />
    <link href="CSS/jquery-ui.css" rel="Stylesheet" />
</head>
<body style="overflow: hidden; margin: 0px;" bgcolor="#BFDBFF">
    <form id="form1" runat="server" style="height: 730px">
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        </telerik:RadAjaxManager>
        <telerik:RadWindowManager ID="WindowMngr" runat="server">
            <Windows>
                <telerik:RadWindow ID="MessageWindow" runat="server" Modal="true" Behaviors="Close" VisibleStatusbar="false"
                    EnableViewState="false" Title="Print Documents" IconUrl="Resources/16_16.ico">
                </telerik:RadWindow>
                <telerik:RadWindow ID="RadWindow1" runat="server" Modal="true" Behaviors="Close"
                    EnableViewState="false" Title="EXCEPTION" IconUrl="Resources/16_16.ico">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" VisibleStatusbar="false">
            <Windows>
                <telerik:RadWindow ID="RadFollowUpEncounter" runat="server" Behaviors="Move,Minimize"
                    Width="560px" Height="500px" OnClientClose="FollowUpWindowClientClose"
                    Title="FollowUpEncounter" IconUrl="Resources/16_16.ico">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <telerik:RadWindowManager ID="RadWindowManager2" runat="server" VisibleStatusbar="false"
            VisibleOnPageLoad="false">
            <Windows>
                <telerik:RadWindow ID="RadWindow2" runat="server" Behaviors="Move,Minimize"
                    Width="500px" Height="550px" OnClientClose="FollowUpWindowClientClosePageLoad"
                    Title="FollowUpEncounter"
                    IconUrl="Resources/16_16.ico">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>

        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
            </Scripts>
        </telerik:RadScriptManager>
        <div id="SummaryAlert" runat="server" class="alert alert-info" style="height: 709px; padding: 10px; display: none; border-color: lightblue; color: black; margin: 3px; font-weight: bolder; background-color: aliceblue">
            Xml is not found for this encounter. Please contact support.
        </div>
        <div id="divEncounter" runat="server">
            <div id="Wrapper" runat="server"  class="enwrapper">

                <div style="width: 100%; margin: 0px; padding: 0px;">
                    <table cellpadding="0px" cellspacing="0px" style="width: 100%;">
                        <tr style="margin: 0px; padding: 0px;">
                            <td style="width: 100%; margin: 0px; padding: 0px;">
                                <%-- To Make Template textbox and button visible like before, change width of this table cell to 83% - For BugID:30521 - Pujhitha --%>
                                <fieldset id="pnlBarGroupTabs" runat="server" text="Root RadPanelItem1" onclientclicked="lblPatientStrip_ItemClicked"
                                    class="newStyle1 pnlBarGroup" style="width: 100%; height: 11px;">
                                </fieldset>

                            </td>
                            <td style="width: 0%; margin: 0px; padding: 0px;">
                            </td>

                        </tr>
                    </table>
                </div>
                <asp:Panel runat="server" Style="margin-top: 10px">
                    <table style="display: inline;" cellpadding="0px" cellspacing="0px">
                        <tr style="margin: 0px; padding: 0px;">
                            <td style="width: 690px; margin: 0px; padding: 0px;">
                                <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" CssClass="displayTableCell"
                                    HorizontalAlign="NotSet">
                                    <asp:DropDownList ID="cboPhysicianName" runat="server" Width="244px" Style="margin-left: 1px;" onchange="getDropdownListSelectedText();">
                                    </asp:DropDownList>
                                    <asp:CheckBox ID="chkShowAllPhysicians" runat="server" Text="Show all Physicians" onchange="chkShowAllPhysicians_CheckedChanged(this);"
                                        AutoPostBack="true" OnCheckedChanged="chkShowAllPhysicians_CheckedChanged" CssClass="noWrapText Editabletxtbox " Style="margin-left: -2px;" />
                                    <asp:CheckBox ID="chkACOValidation" runat="server" AutoPostBack="false" CssClass="noWrapText" Text="ACO Discussed" Style="font-weight: lighter; margin-left: 4px;" />
                                    <asp:CheckBox ID="chkProviderReview" runat="server" AutoPostBack="true" CssClass="noWrapText" Text="Is Review Required" Style="font-weight: lighter; margin-left: 4px;" />
                                    <asp:CheckBox ID="chkcorrectionreview" runat="server"  CssClass="noWrapText" Text="Is Correction Required" Style="font-weight: lighter; margin-left: 4px;" />
                                </telerik:RadAjaxPanel>
                            </td>
                            <td>
                                 

                            </td>
                            <td style="margin: 0px; padding: 0px;">
                                <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server">
                                    <input type="button" id="btnCopyPreviousEncounter" style="margin-right: 2px;" class="btn btn-primary btncolor" value="Copy Previous Encounter" onclick=" if (!btnCopyPrevious_ClientClick(this)) return;" runat="server" onserverclick="btnCopyPrevious_ServerClick" />
                                </telerik:RadAjaxPanel>
                            </td>
                            <td style="margin: 0px; padding: 0px;">
                                <input type="button" id="btnMoveToMA" style="margin-right: 2px;" visible="false" class="btn btn-primary btncolor" value="Move to MA" runat="server" onclick="if (!IsSaveEnabled(this)) return;" onserverclick="btnMoveToMA_Click" />
                            </td>
                            <td style="margin: 0px; padding: 0px;">
                                <input type="button" id="btnCorrections" style="margin-right: 2px; "  class="btn btn-primary btncolor" value="Corrections to be Done" runat="server" onclick="if (!IsSaveEnabled(this)) return;" onserverclick="btnCorrections_ServerClick" />
                            </td>
                            <td style="margin: 0px; padding: 0px;">
                                <input type="button" id="btnPhysiciancorrection" style="margin-right: 2px;"
                                    onclick="if (!IsSaveEnabled(this)) return;" class="btn btn-primary btncolor" value="Ready for Physician Correction" runat="server" onserverclick="btnPhysiciancorrection_Click" />
                            </td>
                            <td style="margin: 0px; padding: 0px;">
                                <input type="button" id="btnMoveToDictation" style="margin-right: 2px; display: none" class="btn btn-primary btncolor" value="Move To Dictation" runat="server" />

                            </td>
                            <td style="margin: 0px; padding: 0px;">
                                <input type="button" id="btnMove" style="margin-right: 2px;" class="btn btn-primary btncolor"
                                    value="Move To" runat="server" onclick=" if (!IsSaveEnabled(this)) return; " onserverclick="btnMove_Click" />

                            </td>
                            <td style="margin: 0px; padding: 0px;">
                                <input type="button" id="btnmovetoscribe" style="margin-right: 2px; display:none" class="btn btn-primary btncolor" 
                                    value="Move To Scribe" runat="server" onclick=" if (!IsSaveEnabled(this)) return; " onserverclick="btnmovetoscribe_click" />

                            </td>
                            <td style="margin: 0px; padding: 0px;">
                                <button id="btnQRCode" style="margin-right: 2px;margin-left:8px !important " class="btn btn-primary aspAkidoBluebutton" 
                                value="" runat="server" onclick="if(StartLoadingcursor());  QRCodeClick();" >Dictate (Mobile) <i class="fa fa-qrcode" aria-hidden="true"></i></button>

                            </td>
                            <td style="margin: 0px; padding: 0px;">
                                <button id="btnAkidoNote" style="margin-right: 2px; height: 34px; " class="btn btn-primary aspAkidoBluebutton" 
                                value="" runat="server" onclick="if(StartLoadingcursor()); AkidoNoteClick(); "  >Akido Note <i class="bi bi-box-arrow-up-right" aria-hidden="true"></i></button>

                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
            <div style="margin: 0px; padding: 0px; margin-top: -5px">

                <div class="container" id="dvcontainer">
                    <div class="row">
                        <asp:Panel ID="pnlEncScroll" runat="server" class="col-md-12">

                            <ul class="nav nav-tabs " id="myTabs">
                                <li class="active liencounter" runat="server" id="tabStripEncounter_tbCCHPI">
                                    <a href="#tbCCHPI" data-toggle="tab" style="width: 65px;text-align: center;color:#6504d0!important;" >CC / HPI</a>
                                </li>
                                <li runat="server" id="tabStripEncounter_tbQuestion" class="liencounter">
                                    <%--<a href="#tbQuestion" data-toggle="tab">QUESTIONNAIRE</a>--%>
                                    <a href="#tbQuestion" data-toggle="tab">SCREENING</a>
                                </li>
                                <li runat="server" id="tabStripEncounter_tbPFSH" class="liencounter">
                                    <a href="#tbPFSH" data-toggle="tab" style="width: 65px; text-align: center;color:#6504d0!important;" >PFSH</a>
                                </li>
                                <li runat="server" id="tabStripEncounter_tbROS" class=" liencounter">
                                    <a href="#tbROS" data-toggle="tab" style="width: 65px; text-align: center;">ROS</a>
                                </li>
                                <li runat="server" id="tabStripEncounter_tbVitals" class="liencounter">
                                    <a href="#tbVitals" data-toggle="tab" style="color:#6504d0!important;">VITALS</a>
                                </li>
                                <li runat="server" id="tabStripEncounter_tbExam" class="liencounter">
                                    <a href="#tbExam" data-toggle="tab">EXAM</a>
                                </li>
                                <li runat="server" id="tabStripEncounter_tabTest" class="liencounter">
                                    <a href="#tbTest" data-toggle="tab">TEST</a>
                                </li>
                                <li runat="server" id="tabStripEncounter_tbAssessment" class="liencounter">
                                    <a href="#tbAssessment" data-toggle="tab" style="width: 111px; text-align: center;" >ASSESSMENT</a>
                                </li>
                                <li runat="server" id="tabStripEncounter_tbOrders" class=" liencounter">
                                    <a href="#tbOrders" data-toggle="tab" style="color:#6504d0!important;">ORDERS</a>
                                </li>
                                <li runat="server" id="tabStripEncounter_tbEPrescription" class="liencounter">
                                    <a href="#tbEPrescription" data-toggle="tab" style="width: 60px; text-align: center;color:#6504d0!important;">eRx</a>
                                </li>
                                <li runat="server" id="tabStripEncounter_tbPlan" class="liencounter">
                                    <a href="#tbPlan" data-toggle="tab" style="width: 65px; text-align: center;color:#6504d0!important;">PLAN</a>
                                </li>
                                <li runat="server" id="tabStripEncounter_tbEandM" class="liencounter">
                                    <a href="#tbEandM" data-toggle="tab" style="color:#6504d0!important;">SERV./PROC. CODES</a>
                                </li>
                                <li runat="server" id="tabStripEncounter_tbSummary" class="liencounter">
                                    <a href="#tbSummary" data-toggle="tab" style="width: 85px; text-align: center;">SUMMARY</a>
                                </li>
                            </ul>

                            <div class="tab-content ">
                                <div class="tab-pane active" id="tbCCHPI" data-src="frmCCPhrase.aspx">
                                    <iframe class="clsIframe" src="" style="height: 643px; width: 100%; margin-top: -2px !important" frameborder="0" scrolling="no" marginheight="0" marginwidth="0"></iframe>
                                </div>

                                <div class="tab-pane" id="tbQuestion" data-src="HtmlQuestionnaireTab.html">
                                    <iframe class="clsIframe" src="" style="height: 742px; width: 100%;" width="100%" height="100%" frameborder="0" allowfullscreen="allowFullScreen"></iframe>
                                </div>


                                <div class="tab-pane" id="tbPFSH" data-src="htmlPFSH.html">
                                    <iframe class="clsIframe" src="" id="iframePFSH" style="height: 707px; width: 100%;" frameborder="0" allowfullscreen="allowFullScreen"></iframe>
                                </div>
                                <div class="tab-pane" id="tbROS" data-src="frmRos.aspx">
                                    <iframe class="clsIframe" src="" style="height: 681px; width: 96%; margin-top: -2px !important" frameborder="0" scrolling="yes" marginheight="0" marginwidth="0"></iframe>
                                </div>

                                <div class="tab-pane" id="tbVitals" >
                                    <iframe class="clsIframe" src="" style="height: 696px; width: 97%; " width="100%" height="100%" frameborder="0" allowfullscreen="allowFullScreen"></iframe>
                                </div>


                                <div class="tab-pane" id="tbExam" data-src="htmlExamTab.html">
                                    <iframe class="clsIframe" src="" style="height: 695px; width: 99%;" frameborder="0" allowfullscreen="allowFullScreen"></iframe>
                                </div>
                                <div class="tab-pane " id="tbTest" data-src="HtmlTestTab.html">
                                    <iframe class="clsIframe" src="" style="height: 643px; width: 98%; margin-top: -2px !important" frameborder="0" scrolling="no" marginheight="0" marginwidth="0"></iframe>
                                </div>

                                <div class="tab-pane" id="tbAssessment" data-src="htmlAssessment.html">
                                    <iframe class="clsIframe" src="" style="height: 643px; width: 97%;margin-left:10px!important;" width="100%" height="100%" frameborder="0" allowfullscreen="allowFullScreen"></iframe>
                                </div>


                                <div class="tab-pane" id="tbOrders" data-src="HtmlOrdersTab.html">
                                    <iframe class="clsIframe" src="" style="height: 745px; width: 100%;" frameborder="0" allowfullscreen="allowFullScreen"></iframe>
                                </div>
                                <div class="tab-pane" id="tbEPrescription" data-src="frmRCopiaWebBrowser.aspx?MyType=GENERAL&IsMoveButton=false&IsMoveCheckbox=true&IsPrescriptiontobePushed=&openingFrom=Tab&IsSentToRCopia=''&LocalTime=''">
                                    <iframe class="clsIframe" src="" style="height: 662px; width: 98%;" frameborder="0" allowfullscreen="allowFullScreen"></iframe>
                                </div>



                                <%--   <div class="tab-pane "  id="tbEandM" data-src="frmEandMcoding.aspx" >
                            <iframe class="clsIframe" src="" style=" height: 643px; width: 107%; margin-left: -43px;margin-top:-2px !important" frameborder="0" scrolling="no" marginheight="0" marginwidth="0"></iframe>
                        </div>--%>

                                <div class="tab-pane" id="tbPlan" data-src="HtmlPlanTab.html">
                                    <iframe class="clsIframe" src="" style="height: 686px; width: 100%; margin-top: -3px" width="100%" height="100%" frameborder="0" allowfullscreen="allowFullScreen"></iframe>
                                </div>
                                <div class="tab-pane " id="tbEandM" data-src="htmlEandMCoding.html">
                                    <iframe class="clsIframe" src="" style="height: 675px; width: 92%;margin-left:10px!important; margin-top: -2px !important" frameborder="0" scrolling="no" marginheight="0" marginwidth="0"></iframe>
                                </div>

                                <div class="tab-pane" id="tbSummary" data-src="frmSummaryNew.aspx?AkidoSummary=N">
                                    <div id="WaitingMessage">  
                                        <label style=" font-family: Helvetica Neue,Helvetica,Arial,sans-serif !important;font-size: 13px !important;font-weight: bold  !important;"> Summary is loading. Please wait.</label>
                                    </div>
                                    <iframe id="Summaryframe" class="clsIframe" src="" style="display:block; height: 651px; width: 96%;" frameborder="0" allowfullscreen="allowFullScreen"></iframe>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                </div>

            </div>

        </div>


        <asp:HiddenField ID="hdnLocalTime" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="sMyPreviousTab" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="sMyNextTab" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="IsSaveEnable" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnPurposeOfVisit_SaveRejected" runat="server" EnableViewState="false" Value="false" />
        <asp:HiddenField ID="hdnTab" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnChkOut" runat="server" EnableViewState="false" Value="true" />
        <asp:HiddenField ID="MysignID" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="SignedDateAndTime" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnIsACOValid" runat="server" Value="false" EnableViewState="false" />
        <asp:HiddenField ID="hdnACOValidated" runat="server" Value="false" EnableViewState="false" />
        <asp:HiddenField ID="hdnPFSHInfoBy" runat="server" Value="false" EnableViewState="false" />
        <asp:HiddenField ID="hdnindex" runat="server" EnableViewState="true"/>
        <asp:HiddenField ID="hdnPFSHVerifiedEnable" runat="server" Value="false" EnableViewState="false" />
        <asp:HiddenField ID="hdnSaveButtonID" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnTabClick" runat="server" EnableViewState="false" Value="first" />
        <asp:HiddenField ID="hdnCopyToProviderForReview" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnAddendumID" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnLocalPhy" runat="server" EnableViewState="true" />
        <asp:HiddenField ID="hdnUserRole" runat="server" EnableViewState="true" />
        <asp:HiddenField ID="hdnReviewStatus" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnCloseFS" runat="server" />
        <asp:HiddenField ID="hdnAkidoNote" runat="server" />
        <asp:HiddenField ID="hdnEncounterID" runat="server" />

        <asp:HiddenField ID="hdnPreviousEncounterId" runat="server" />
        <asp:HiddenField ID="hdnAppointmentProviderId" runat="server" />
        <asp:HiddenField ID="hdnAppointmentProviderName" runat="server" />
        <asp:HiddenField ID="hdnEncounterProviderName" runat="server" />
        <asp:HiddenField ID="hdnEncounterProviderId" runat="server" />
        <asp:HiddenField ID="hdnSelectPhysicianId" runat="server" />
        <%--<asp:HiddenField ID="hdnNotificationOkClicked" runat="server" />--%><%--//BugID:47780--%>
        <asp:HiddenField ID="hdnProvRev" runat="server" />
        <asp:HiddenField ID="hdnEncounterIDSummary" runat="server" EnableViewState="false" />
        <asp:UpdatePanel ID="upPanle" runat="server">
            <ContentTemplate>
                <asp:HiddenField ID="hdnCopyPreviousClick" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Button ID="hiddenButton" runat="server" OnClick="hiddenButton_Click" CssClass="DisplayNone" />
        <asp:Button ID="btnhdncbofill" runat="server" OnClick="btnhdncbofill_Click" CssClass="DisplayNone" />
        <asp:Button ID="hdnButtonEnable" runat="server" OnClick="hdnButtonEnable_Click" CssClass="DisplayNone" />
        <asp:Button ID="btnhiddenConfirmOk" runat="server" OnClick="btnhiddenConfirmOk_Click" CssClass="DisplayNone" />
        <asp:Button ID="btnHiddenDuplicateCheck" runat="server" OnClick="btnHiddenDuplicateCheck_Click" CssClass="DisplayNone" />
        <asp:Button ID="btnHiddenCopyPreviousEncounter" runat="server" OnClick="btnCopyPreviousHidden_Click" CssClass="DisplayNone" />
        <button id="btnHiddenTab" class="DisplayNone"></button>
        <asp:Button ID="hdnbtngeneratexmlEncounter" runat="server" OnClick="hdnbtngeneratexml_Click"  style="display:none" />

        <div id="divLoading" class="modal" runat="server" style="text-align: center; display: none">
            <asp:Panel ID="Panel2" runat="server">
                <br />
                <br />
                <br />
                <br />
                <center>
                    <asp:Label ID="Label1" Text="" runat="server" EnableViewState="false"></asp:Label></center>
                <br />
                <img src="Resources/wait.ico" title="" alt="Loading..." />
                <br />
            </asp:Panel>
        </div>

        <asp:PlaceHolder runat="server">

            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>"
                type="text/javascript" enableviewstate="false"></script>

            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>"
                type="text/javascript" enableviewstate="false"></script>

            <script src="JScripts/JSEncounter.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>"
                type="text/javascript" enableviewstate="false"></script>

            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>"
                type="text/javascript"></script>
            <script src="JScripts/jquery-2.1.3.js"></script>
            <script src="JScripts/jquery-ui.min1.10.2.js" type="text/javascript"></script>
            <script src="JScripts/bootstrap.min.js" type="text/javascript"></script>
        </asp:PlaceHolder>
    </form>
</body>
</html>
<script type="text/javascript">
    var PrevTab;
    var CurTab;
    var bCancel = false;
    sessionStorage.setItem("EncCancel", "false");
    var dvdialog;
    window.onbeforeunload = function funcUnload() {
        PrevTab = $(window.document).find(".tab-pane.active ");
        var prvTab = PrevTab[0].attributes.id.value;

        if (prvTab.match("tbEPrescription") != null) {
            $.ajax({
                type: "POST",
                url: "frmEncounter.aspx/DownloadRcoipa",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                async: true,
                success: function (data) {
                },
                error: function (result) {
                    //alert(result.d);
                }
            });
        }
        //BugID:51562 
        if (prvTab.match("tbEandM") != null) {
            if ($(top.window.document)?.find(".in")?.find(".close") != undefined && $(top.window.document)?.find(".in")?.find(".close")?.length > 1)
                $(top.window.document).find(".in").find(".close")[0].click();
        }

    }
    var val_set = false;
    var event_bkup;
    var AspxPages = ["#tbSummary", "#tbEPrescription", "#tbVitals", "#tbROS", "#tbCCHPI"];
    $('.nav-tabs a').on('shown.bs.tab', function (event) {

        // {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();} Commented to Persist Loading Screen till the Child Screen Loads Completely.
        //BugID:46035 - autosave when Codes are checked in Fav.Sheet.
        var evt = event;
        if (event == undefined && event_bkup != undefined) {
            event = event_bkup;
        }
        if (val_set == true) {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            val_set = false;
            return;
        }
        PrevTab = $(event.relatedTarget);  // previous tab  
        if (PrevTab[0].innerText == "SERV./PROC. CODES") {
            if ($(top.window.document)?.find(".in")?.find(".close") != undefined && $(top.window.document)?.find(".in")?.find(".close")?.length > 1 && ($(top.window.document)?.find("#tbFavCPTsContainer input[type=checkbox]:checked")?.length > 0 || $(top.window.document)?.find("#tbFavICDsContainer input[type=checkbox]:checked")?.length > 0)) {
                if (event != undefined)
                    event_bkup = event;
                if (DisplayErrorMessage('530020') == true) {
                    //CAP-625: Handle Click event                    
                    if ($(top.window.document)?.find(".in")?.find(".close")[0] != null && $(top.window.document)?.find(".in")?.find(".close")[0] != undefined) {
                        $(top.window.document).find(".in").find(".close")[0].click();
                    }
                }
                if (evt != undefined) {
                    event_bkup = event;
                    return;
                }
                else {
                    val_set = true;
                    PrevTab.tab('show');
                    return;
                }
            }
        }

        CurTab = $(event.target);         // active tab
        PrevTab = $(event.relatedTarget);  // previous tab   
        sessionStorage.setItem('TabAssesment', CurTab[0].innerText);
        //BUgID:42888
        if (CurTab[0].innerText == "SERV./PROC. CODES" && PrevTab[0].innerText == "CC / HPI") {
            //sessionStorage.setItem("Encounter_PrevTabRevert", false);
        }
        if (CurTab[0].innerText == "SUMMARY") {
            localStorage.setItem("SummaryTab", "true");
        }
        if (JSON.parse(sessionStorage.getItem("Encounter_PrevTabRevert")) != true) {
            sessionStorage.setItem("Encounter_PrevTabRevert", false);
            if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value == "true") {

                event.preventDefault();
                { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
                sessionStorage.setItem("EncCancel", "false");
                sessionStorage.setItem("EncPrevTabText", PrevTab[0].innerText);
                sessionStorage.setItem("EncAutoSave", "true");
                if (PrevTab[0].innerText == "CC / HPI") {
                    paneID = $(event.target).attr('href');
                    sessionStorage.setItem("Enc_PaneId", paneID);
                    sessionStorage.setItem("Enc_Src", $(paneID).attr('data-src'));

                    if ($('.clsIframe')?.contents()[0]?.all?.namedItem('btnAdd') != null && $('.clsIframe')?.contents()[0]?.all?.namedItem('btnAdd') != undefined) {
                        $('.clsIframe').contents()[0].all.namedItem('btnAdd').click();
                    }




                    //if (localStorage.getItem("bSave") == "true" && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value == "false") {
                    //    paneID = $(event.target).attr('href');
                    //    src = $(paneID).attr('data-src');
                    //    $(paneID + " iframe").attr("src", src);
                    //}
                    //else {
                    //    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                    //    bCancel = true;
                    //    sessionStorage.setItem("EncCancel", "true");
                    //    PrevTab.tab('show');
                    //    return;
                    //}
                }
                else if (PrevTab[0].innerText == "SCREENING") {
                    var subtab = localStorage.getItem("PrevSubTab");
                    paneID = $(event.target).attr('href');
                    sessionStorage.setItem("Enc_PaneId", paneID);
                    sessionStorage.setItem("Enc_Src", $(paneID).attr('data-src'));
                    if (subtab == "General") {
                        if ($('.clsIframe')?.contents()[1]?.all?.namedItem('General')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[1]?.all?.namedItem('General')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[1].all.namedItem('General').children[0].contentDocument.all.namedItem('btnSave').click();
                        }
                    }
                    else if (subtab == "TB Risk Assessment") {
                        if ($('.clsIframe')?.contents()[1]?.all?.namedItem('TBRiskAssessment')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[1]?.all?.namedItem('TBRiskAssessment')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[1].all.namedItem('TBRiskAssessment').children[0].contentDocument.all.namedItem('btnSave').click();
                        }

                    }
                    else if (subtab == "Diabetic Foot Screening") {
                        if ($('.clsIframe')?.contents()[1]?.all?.namedItem('DiabeticFootScreening')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[1]?.all?.namedItem('DiabeticFootScreening')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[1].all.namedItem('DiabeticFootScreening').children[0].contentDocument.all.namedItem('btnSave').click();
                        }
                    }
                    else if (subtab == "Sleep Screening") {
                        if ($('.clsIframe')?.contents()[1]?.all?.namedItem('SleepScreening')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[1]?.all?.namedItem('SleepScreening')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[1].all.namedItem('SleepScreening').children[0].contentDocument.all.namedItem('btnSave').click();
                        }
                    }
                    else if (subtab == "Sleep") {
                        if ($('.clsIframe')?.contents()[1]?.all?.namedItem('Sleep')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[1]?.all?.namedItem('Sleep')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[1].all.namedItem('Sleep').children[0].contentDocument.all.namedItem('btnSave').click();
                        }

                    }
                    else if (subtab == "Pulmonary") {
                        if ($('.clsIframe')?.contents()[1]?.all?.namedItem('Pulmonary')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[1]?.all?.namedItem('Pulmonary')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[1].all.namedItem('Pulmonary').children[0].contentDocument.all.namedItem('btnSave').click();
                        }

                    }
                    else if (subtab == "Epworth Sleep Score") {
                        if ($('.clsIframe')?.contents()[1]?.all?.namedItem('EpworthSleepScore')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[1]?.all?.namedItem('EpworthSleepScore')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[1].all.namedItem('EpworthSleepScore').children[0].contentDocument.all.namedItem('btnSave').click();
                        }

                    }
                    else if (subtab == "Pulmonary/Sleep Exam") {
                        if ($('.clsIframe')?.contents()[1]?.all?.namedItem('PulmonarySleepExam')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[1]?.all?.namedItem('PulmonarySleepExam')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[1].all.namedItem('PulmonarySleepExam').children[0].contentDocument.all.namedItem('btnSave').click();
                        }
                    }
                    else if (subtab == "Dermatology Questionnaire") {
                        if ($('.clsIframe')?.contents()[1]?.all?.namedItem('DermatologyQuestionnaire')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[1]?.all?.namedItem('DermatologyQuestionnaire')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[1].all.namedItem('DermatologyQuestionnaire').children[0].contentDocument.all.namedItem('btnSave').click();
                        }
                    }
                    else if (subtab == "Asthma Control Test") {
                        if ($('.clsIframe')?.contents()[1]?.all?.namedItem('AsthmaControlTest')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[1]?.all?.namedItem('AsthmaControlTest')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[1].all.namedItem('AsthmaControlTest').children[0].contentDocument.all.namedItem('btnSave').click();
                        }

                    }
                    else if (subtab == "PHQ-9 Screening") {
                        if ($('.clsIframe')?.contents()[1]?.all?.namedItem('PHQ-9Screening')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[1]?.all?.namedItem('PHQ-9Screening')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[1].all.namedItem('PHQ-9Screening').children[0].contentDocument.all.namedItem('btnSave').click();
                        }
                    }
                    else if (subtab == "Depression Test") {
                        if ($('.clsIframe')?.contents()[1]?.all?.namedItem('DepressionTest')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[1]?.all?.namedItem('DepressionTest')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[1].all.namedItem('DepressionTest').children[0].contentDocument.all.namedItem('btnSave').click();
                        }
                    }
                    else if (subtab == "Neck Disability Index") {
                        if ($('.clsIframe')?.contents()[1]?.all?.namedItem('NeckDisabilityIndex')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[1]?.all?.namedItem('NeckDisabilityIndex')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[1].all.namedItem('NeckDisabilityIndex').children[0].contentDocument.all.namedItem('btnSave').click();
                        }

                    }
                    else if (subtab == "Oswestry Disability Index") {
                        if ($('.clsIframe')?.contents()[1]?.all?.namedItem('OswestryDisabilityIndex')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[1]?.all?.namedItem('OswestryDisabilityIndex')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[1].all.namedItem('OswestryDisabilityIndex').children[0].contentDocument.all.namedItem('btnSave').click();
                        }

                    }
                    else if (subtab == "Development") {
                        if ($('.clsIframe')?.contents()[1]?.all?.namedItem('Development')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[1]?.all?.namedItem('Development')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[1].all.namedItem('Development').children[0].contentDocument.all.namedItem('btnSave').click();
                        }

                    }

                    else if (subtab == "Chronic Cough Scale") {
                        if ($('.clsIframe')?.contents()[1]?.all?.namedItem('ChronicCoughScale')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[1]?.all?.namedItem('ChronicCoughScale')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[1].all.namedItem('ChronicCoughScale').children[0].contentDocument.all.namedItem('btnSave').click();
                        }

                    }
                    else if (subtab == "Gynecological") {
                        if ($('.clsIframe')?.contents()[1]?.all?.namedItem('Gynecological')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[1]?.all?.namedItem('Gynecological')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[1].all.namedItem('Gynecological').children[0].contentDocument.all.namedItem('btnSave').click();
                        }
                    }
                    else if (subtab == "Pediatric Sleep Questionnaire") {
                        if ($('.clsIframe')?.contents()[1]?.all?.namedItem('PediatricSleepQuestionnaire')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[1]?.all?.namedItem('PediatricSleepQuestionnaire')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[1].all.namedItem('PediatricSleepQuestionnaire').children[0].contentDocument.all.namedItem('btnSave').click();
                        }
                    }
                    else if (subtab == "Sleep Short") {
                        if ($('.clsIframe')?.contents()[1]?.all?.namedItem('SleepShort')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[1]?.all?.namedItem('SleepShort')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[1].all.namedItem('SleepShort').children[0].contentDocument.all.namedItem('btnSave').click();
                        }

                    }
                    else if (subtab == "Sleep Tendency Scale") {
                        if ($('.clsIframe')?.contents()[1]?.all?.namedItem('SleepTendencyScale')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[1]?.all?.namedItem('SleepTendencyScale')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[1].all.namedItem('SleepTendencyScale').children[0].contentDocument.all.namedItem('btnSave').click();
                        }

                    }
                    else if (subtab == "Functional Assessment") {
                        if ($('.clsIframe')?.contents()[1]?.all?.namedItem('FunctionalAssessment')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[1]?.all?.namedItem('FunctionalAssessment')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[1].all.namedItem('FunctionalAssessment').children[0].contentDocument.all.namedItem('btnSave').click();
                        }

                    }
                    else if (subtab == "Karnofsky") {
                        if ($('.clsIframe')?.contents()[1]?.all?.namedItem('Karnofsky')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[1]?.all?.namedItem('Karnofsky')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[1].all.namedItem('Karnofsky').children[0].contentDocument.all.namedItem('btnSave').click();
                        }

                    }
                    else if (subtab == "Mini Mental") {
                        if ($('.clsIframe')?.contents()[1]?.all?.namedItem('MiniMental')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[1]?.all?.namedItem('MiniMental')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[1].all.namedItem('MiniMental').children[0].contentDocument.all.namedItem('btnSave').click();
                        }

                    }
                    else if (subtab == "Nutritional Screening") {
                        if ($('.clsIframe')?.contents()[1]?.all?.namedItem('NutritionalScreening')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[1]?.all?.namedItem('NutritionalScreening')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[1].all.namedItem('NutritionalScreening').children[0].contentDocument.all.namedItem('btnSave').click();
                        }

                    }
                    else if (subtab == "Safety Guidelines") {
                        if ($('.clsIframe')?.contents()[1]?.all?.namedItem('SafetyGuidelines')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[1]?.all?.namedItem('SafetyGuidelines')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[1].all.namedItem('SafetyGuidelines').children[0].contentDocument.all.namedItem('btnSave').click();
                        }

                    }
                    else if (subtab == "Support Needs") {
                        if ($('.clsIframe')?.contents()[1]?.all?.namedItem('SupportNeeds')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[1]?.all?.namedItem('SupportNeeds')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[1].all.namedItem('SupportNeeds').children[0].contentDocument.all.namedItem('btnSave').click();
                        }
                    }
                    else if (subtab == "Pain Assessment") {
                        if ($('.clsIframe')?.contents()[1]?.all?.namedItem('PainAssessment')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[1]?.all?.namedItem('PainAssessment')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[1].all.namedItem('PainAssessment').children[0].contentDocument.all.namedItem('btnSave').click();
                        }

                    }
                    else if (subtab == "CAT Screening") {
                        if ($('.clsIframe')?.contents()[1]?.all?.namedItem('CATScreening')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[1]?.all?.namedItem('CATScreening')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[1].all.namedItem('CATScreening').children[0].contentDocument.all.namedItem('btnSave').click();
                        }

                    }
                    else if (subtab == "COPD Breathe Well Program") {
                        if ($('.clsIframe')?.contents()[1]?.all?.namedItem('COPDBreatheWellProgram')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[1]?.all?.namedItem('COPDBreatheWellProgram')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[1].all.namedItem('COPDBreatheWellProgram').children[0].contentDocument.all.namedItem('btnSave').click();
                        }

                    }
                    else if (subtab == "Fall Risk Screening") {
                        if ($('.clsIframe')?.contents()[1]?.all?.namedItem('FallRiskScreening')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[1]?.all?.namedItem('FallRiskScreening')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[1].all.namedItem('FallRiskScreening').children[0].contentDocument.all.namedItem('btnSave').click();
                        }

                    }
                    else if (subtab == "Cognitive Screening") {
                        if ($('.clsIframe')?.contents()[1]?.all?.namedItem('CognitiveScreening')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[1]?.all?.namedItem('CognitiveScreening')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[1].all.namedItem('CognitiveScreening').children[0].contentDocument.all.namedItem('btnSave').click();
                        }

                    }
                    else if (subtab == "Pain Screening") {
                        if ($('.clsIframe')?.contents()[1]?.all?.namedItem('PainScreening')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[1]?.all?.namedItem('PainScreening')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[1].all.namedItem('PainScreening').children[0].contentDocument.all.namedItem('btnSave').click();
                        }

                    }
                    else if (subtab == "PHQ-9 Screening") {
                        if ($('.clsIframe')?.contents()[1]?.all?.namedItem('PHQ-9Screening')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[1]?.all?.namedItem('PHQ-9Screening')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[1].all.namedItem('PHQ-9Screening').children[0].contentDocument.all.namedItem('btnSave').click();
                        }

                    }
                    else if (subtab == "Home Safety") {
                        if ($('.clsIframe')?.contents()[1]?.all?.namedItem('HomeSafety')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[1]?.all?.namedItem('HomeSafety')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[1].all.namedItem('HomeSafety').children[0].contentDocument.all.namedItem('btnSave').click();
                        }

                    }
                    else if (subtab == "Katz Index Screening") {
                        if ($('.clsIframe')?.contents()[1]?.all?.namedItem('KatzIndexScreening')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[1]?.all?.namedItem('KatzIndexScreening')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[1].all.namedItem('KatzIndexScreening').children[0].contentDocument.all.namedItem('btnSave').click();
                        }

                    }
                    else if (subtab == "ADL Screening") {
                        if ($('.clsIframe')?.contents()[1]?.all?.namedItem('ADLScreening')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[1]?.all?.namedItem('ADLScreening')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[1].all.namedItem('ADLScreening').children[0].contentDocument.all.namedItem('btnSave').click();
                        }

                    }
                    else if (subtab == "Get Up and Go") {
                        if ($('.clsIframe')?.contents()[1]?.all?.namedItem('GetUpandGo')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[1]?.all?.namedItem('GetUpandGo')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[1].all.namedItem('GetUpandGo').children[0].contentDocument.all.namedItem('btnSave').click();
                        }
                    }
                    else if (subtab == "Lawton Screening") {
                        if ($('.clsIframe')?.contents()[1]?.all?.namedItem('LawtonScreening')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[1]?.all?.namedItem('LawtonScreening')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[1].all.namedItem('LawtonScreening').children[0].contentDocument.all.namedItem('btnSave').click();
                        }

                    }
                    else if (subtab == "Anxiety Screening") {
                        if ($('.clsIframe')?.contents()[1]?.all?.namedItem('AnxietyScreening')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[1]?.all?.namedItem('AnxietyScreening')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[1].all.namedItem('AnxietyScreening').children[0].contentDocument.all.namedItem('btnSave').click();
                        }

                    }
                    else if (subtab == "Spine Intake") {
                        if ($('.clsIframe')?.contents()[1]?.all?.namedItem('SpineIntake')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[1]?.all?.namedItem('SpineIntake')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[1].all.namedItem('SpineIntake').children[0].contentDocument.all.namedItem('btnSave').click();
                        }

                    }
                    else if (subtab == "Monofilament Foot Exam") {
                        if ($('.clsIframe')?.contents()[1]?.all?.namedItem('MonofilamentFootExam')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[1]?.all?.namedItem('MonofilamentFootExam')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[1].all.namedItem('MonofilamentFootExam').children[0].contentDocument.all.namedItem('btnSave').click();
                        }

                    }
                    else if (subtab == "Cervical Spine") {
                        if ($('.clsIframe')?.contents()[1]?.all?.namedItem('CervicalSpine')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[1]?.all?.namedItem('CervicalSpine')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[1].all.namedItem('CervicalSpine').children[0].contentDocument.all.namedItem('btnSave').click();
                        }

                    }
                    else if (subtab == "Lumbar Spine") {
                        if ($('.clsIframe')?.contents()[1]?.all?.namedItem('LumbarSpine')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[1]?.all?.namedItem('LumbarSpine')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[1].all.namedItem('LumbarSpine').children[0].contentDocument.all.namedItem('btnSave').click();
                        }
                    }
                    else if (subtab == "Urinalysis") {
                        if ($('.clsIframe')?.contents()[1]?.all?.namedItem('Urinalysis')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[1]?.all?.namedItem('Urinalysis')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[1].all.namedItem('Urinalysis').children[0].contentDocument.all.namedItem('btnSave').click();
                        }

                    }
                    else if (subtab == "Staying Healthy Assessment") {
                        if ($('.clsIframe')?.contents()[1]?.all?.namedItem('StayingHealthyAssessment')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[1]?.all?.namedItem('StayingHealthyAssessment')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[1].all.namedItem('StayingHealthyAssessment').children[0].contentDocument.all.namedItem('btnSave').click();
                        }

                    }
                    else if (subtab == "AUA BPH Symptom") {
                        if ($('.clsIframe')?.contents()[1]?.all?.namedItem('AUABPHSymptom')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[1]?.all?.namedItem('AUABPHSymptom')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[1].all.namedItem('AUABPHSymptom').children[0].contentDocument.all.namedItem('btnSave').click();
                        }

                    }
                    //if (localStorage.getItem("bSave") == "true") {
                    //    paneID = $(event.target).attr('href');
                    //    src = $(paneID).attr('data-src');
                    //    $(paneID + " iframe").attr("src", src);
                    //}
                    //else {
                    //    bCancel = true;
                    //    sessionStorage.setItem("EncCancel", "true");
                    //    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                    //    PrevTab.tab('show');
                    //    return;
                    //}
                }





                else if (PrevTab[0].innerText == "PFSH") {
                    var subtab = localStorage.getItem("PrevSubTab");
                    paneID = $(event.target).attr('href');
                    sessionStorage.setItem("Enc_PaneId", paneID);
                    sessionStorage.setItem("Enc_Src", $(paneID).attr('data-src'));
                    if (subtab == "Past Medical History") {
                        if ($('.clsIframe')?.contents()[2]?.all?.namedItem('pastmedHis')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[2]?.all?.namedItem('pastmedHis')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[2].all.namedItem('pastmedHis').children[0].contentDocument.all.namedItem('btnSave').click();
                        }
                    }
                    else if (subtab == "Surg./Proc.") {
                        if ($('.clsIframe')?.contents()[2]?.all?.namedItem('surgproc')?.children[0]?.contentDocument?.all?.namedItem('btnAdd') != null && $('.clsIframe')?.contents()[2]?.all?.namedItem('surgproc')?.children[0]?.contentDocument?.all?.namedItem('btnAdd') != undefined) {
                            $('.clsIframe').contents()[2].all.namedItem('surgproc').children[0].contentDocument.all.namedItem('btnAdd').click();
                        }

                    }
                    else if (subtab == "Hospitalization History") {
                        if ($('.clsIframe')?.contents()[2]?.all?.namedItem('hospHis')?.children[0]?.contentDocument?.all?.namedItem('btnAdd') != null && $('.clsIframe')?.contents()[2]?.all?.namedItem('hospHis')?.children[0]?.contentDocument?.all?.namedItem('btnAdd') != undefined) {
                            $('.clsIframe').contents()[2].all.namedItem('hospHis').children[0].contentDocument.all.namedItem('btnAdd').click();
                        }

                    }
                    else if (subtab == "Family History") {
                        if ($('.clsIframe')?.contents()[2]?.all?.namedItem('famHis')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[2]?.all?.namedItem('famHis')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[2].all.namedItem('famHis').children[0].contentDocument.all.namedItem('btnSave').click();
                        }

                    }
                    else if (subtab == "Social History") {
                        if ($('.clsIframe')?.contents()[2]?.all?.namedItem('socialHistory')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[2]?.all?.namedItem('socialHistory')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[2].all.namedItem('socialHistory').children[0].contentDocument.all.namedItem('btnSave').click();
                        }
                    }
                    else if (subtab == "Non Drug Allergy") {
                        if ($('.clsIframe')?.contents()[2]?.all?.namedItem('NdDugAllergy')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[2]?.all?.namedItem('NdDugAllergy')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[2].all.namedItem('NdDugAllergy').children[0].contentDocument.all.namedItem('btnSave').click();
                        }
                    }
                    else if (subtab == "Immunization History") {
                        if ($('.clsIframe')?.contents()[2]?.all?.namedItem('immHis')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[2]?.all?.namedItem('immHis')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[2].all.namedItem('immHis').children[0].contentDocument.all.namedItem('btnSave').click();
                        }
                    }
                    else if (subtab == "AD") {
                        if ($('.clsIframe')?.contents()[2]?.all?.namedItem('ad')?.children[0]?.contentDocument?.all?.namedItem('btnPFSHAutoSave') != null && $('.clsIframe')?.contents()[2]?.all?.namedItem('ad')?.children[0]?.contentDocument?.all?.namedItem('btnPFSHAutoSave') != undefined) {
                            $('.clsIframe').contents()[2].all.namedItem('ad').children[0].contentDocument.all.namedItem('btnPFSHAutoSave').click();
                        }
                    }
                    else if (subtab == "Rx History") {
                        if ($('.clsIframe')?.contents()[2]?.all?.namedItem('RxH')?.children[0]?.contentDocument?.all?.namedItem('btnAdd') != null && $('.clsIframe')?.contents()[2]?.all?.namedItem('RxH')?.children[0]?.contentDocument?.all?.namedItem('btnAdd') != undefined) {
                            $('.clsIframe').contents()[2].all.namedItem('RxH').children[0].contentDocument.all.namedItem('btnAdd').click();
                        }
                    }
                    //if (localStorage.getItem("bSave") == "true") {
                    //    paneID = $(event.target).attr('href');
                    //    src = $(paneID).attr('data-src');
                    //    $(paneID + " iframe").attr("src", src);
                    //}
                    //else {
                    //    bCancel = true;
                    //    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                    //    PrevTab.tab('show');
                    //    return;
                    //}
                }
                else if (PrevTab[0].innerText == "ROS") {
                    paneID = $(event.target).attr('href');
                    sessionStorage.setItem("Enc_PaneId", paneID);
                    sessionStorage.setItem("Enc_Src", $(paneID).attr('data-src'));
                    if ($('.clsIframe')?.contents()[3]?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[3]?.all?.namedItem('btnSave') != undefined) {
                        $('.clsIframe').contents()[3].all.namedItem('btnSave').click();
                    }
                }
                else if (PrevTab[0].innerText == "VITALS") {
                    //  var subtab = localStorage.getItem("PrevSubTab");
                    paneID = $(event.target).attr('href');
                    sessionStorage.setItem("Enc_PaneId", paneID);
                    sessionStorage.setItem("Enc_Src", $(paneID).attr('data-src'));
                    if ($('.clsIframe')?.contents()[4]?.all?.namedItem('btnSaveVitals') != null && $('.clsIframe')?.contents()[4]?.all?.namedItem('btnSaveVitals') != undefined) {
                        $('.clsIframe').contents()[4].all.namedItem('btnSaveVitals').click();
                    }

                    paneID = $(event.target).attr('href');
                    src = $(paneID).attr('data-src');
                    $(paneID + " iframe").attr("src", src);
                }
                else if (PrevTab[0].innerText == "EXAM") {
                    var subtab = localStorage.getItem("PrevSubTab");
                    paneID = $(event.target).attr('href');
                    sessionStorage.setItem("Enc_PaneId", paneID);
                    sessionStorage.setItem("Enc_Src", $(paneID).attr('data-src'));
                    if (subtab == "General With Specialty") {
                        if ($('.clsIframe')?.contents()[5]?.all?.namedItem('generalwithspeciality')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[5]?.all?.namedItem('generalwithspeciality')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[5].all.namedItem('generalwithspeciality').children[0].contentDocument.all.namedItem('btnSave').click();
                        }
                    }
                    else if (subtab == "Focused") {
                        if ($('.clsIframe')?.contents()[5]?.all?.namedItem('Focused')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[5]?.all?.namedItem('Focused')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[5].all.namedItem('Focused').children[0].contentDocument.all.namedItem('btnSave').click();
                        }

                    }
                    else if (subtab == "Upload & View Photos") {
                        if ($('.clsIframe')?.contents()[5]?.all?.namedItem('UploadViewPhotos')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[5]?.all?.namedItem('UploadViewPhotos')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[5].all.namedItem('UploadViewPhotos').children[0].contentDocument.all.namedItem('btnSave').click();
                        }
                    }
                    else if (subtab == "Body Image") {
                        if ($('.clsIframe')?.contents()[5]?.all?.namedItem('BodyImage')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[5]?.all?.namedItem('BodyImage')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[5].all.namedItem('BodyImage').children[0].contentDocument.all.namedItem('btnSave').click();
                        }

                    }
                    //if (localStorage.getItem("bSave") == "true") {
                    //    paneID = $(event.target).attr('href');
                    //    src = $(paneID).attr('data-src');
                    //    $(paneID + " iframe").attr("src", src);
                    //}
                    //else {
                    //    bCancel = true;
                    //    sessionStorage.setItem("EncCancel", "true");
                    //    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                    //    PrevTab.tab('show');
                    //    return;
                    //}
                }
                else if (PrevTab[0].innerText == "TEST") {
                    var subtab = localStorage.getItem("PrevSubTab");
                    paneID = $(event.target).attr('href');
                    sessionStorage.setItem("Enc_PaneId", paneID);
                    sessionStorage.setItem("Enc_Src", $(paneID).attr('data-src'));
                    if (subtab == "Mini Mental Status Exam") {
                        if ($('.clsIframe')?.contents()[6]?.all?.namedItem('MiniMentalStatusExam')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[6]?.all?.namedItem('MiniMentalStatusExam')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[6].all.namedItem('MiniMentalStatusExam').children[0].contentDocument.all.namedItem('btnSave').click();
                        }
                    }
                    else if (subtab == "Depression Screening") {
                        if ($('.clsIframe')?.contents()[6]?.all?.namedItem('DepressionScreening')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[6]?.all?.namedItem('DepressionScreening')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[6].all.namedItem('DepressionScreening').children[0].contentDocument.all.namedItem('btnSave').click();
                        }
                    }
                    else if (subtab == "Spiritual Care Assessment") {
                        if ($('.clsIframe')?.contents()[6]?.all?.namedItem('SpiritualCareAssessment')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[6]?.all?.namedItem('SpiritualCareAssessment')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[6].all.namedItem('SpiritualCareAssessment').children[0].contentDocument.all.namedItem('btnSave').click();
                        }
                    }
                    //if (localStorage.getItem("bSave") == "true") {
                    //    paneID = $(event.target).attr('href');
                    //    src = $(paneID).attr('data-src');
                    //    $(paneID + " iframe").attr("src", src);
                    //}
                    //else {
                    //    bCancel = true;
                    //    sessionStorage.setItem("EncCancel", "true");
                    //    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                    //    PrevTab.tab('show');
                    //    return;
                    //}
                }
                else if (PrevTab[0].innerText == "ASSESSMENT") {
                    paneID = $(event.target).attr('href');
                    sessionStorage.setItem("Enc_PaneId", paneID);
                    sessionStorage.setItem("Enc_Src", $(paneID).attr('data-src'));
                    //CAP 304 Cannot read properties of null (reading click) SOURCE: 
                    if ($('.clsIframe')?.contents()[7]?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[7]?.all?.namedItem('btnSave') != undefined) {
                        $('.clsIframe').contents()[7].all.namedItem('btnSave').click();
                    }

                    //if (localStorage.getItem("bSave") == "true") {
                    //    paneID = $(event.target).attr('href');
                    //    src = $(paneID).attr('data-src');
                    //    $(paneID + " iframe").attr("src", src);
                    //}
                    //else {
                    //    bCancel = true;
                    //    sessionStorage.setItem("EncCancel", "true");
                    //    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                    //    PrevTab.tab('show');
                    //    return;
                    //}
                }
                else if (PrevTab[0].innerText == "ORDERS") {
                    var subtab = localStorage.getItem("PrevSubTab");
                    paneID = $(event.target).attr('href');
                    sessionStorage.setItem("Enc_PaneId", paneID);
                    sessionStorage.setItem("Enc_Src", $(paneID).attr('data-src'));
                    //CAP 305 Uncaught TypeError: Cannot read properties of null (reading click) SOURCE: 
                    if (subtab == "Diagnostic Order") {
                        if ($('.clsIframe')?.contents()[8]?.all?.namedItem('DiagnosticOrder')?.children[0]?.contentDocument?.all?.namedItem('btnOrderSubmit') != null && $('.clsIframe')?.contents()[8]?.all?.namedItem('DiagnosticOrder')?.children[0]?.contentDocument?.all?.namedItem('btnOrderSubmit') != undefined) {
                            $('.clsIframe').contents()[8].all.namedItem('DiagnosticOrder').children[0]?.contentDocument.all.namedItem('btnOrderSubmit').click();
                        }

                    }
                    else if (subtab == "Referral Order") {
                        if ($('.clsIframe')?.contents()[8]?.all?.namedItem('ReferralOrder')?.children[0]?.contentDocument?.all?.namedItem('btnAddRefOrder') != null && $('.clsIframe')?.contents()[8]?.all?.namedItem('ReferralOrder')?.children[0]?.contentDocument?.all?.namedItem('btnAddRefOrder') != undefined) {
                            $('.clsIframe').contents()[8].all.namedItem('ReferralOrder').children[0].contentDocument.all.namedItem('btnAddRefOrder').click();
                        }

                    }
                    else if (subtab == "Immunization/Injection") {
                        if ($('.clsIframe')?.contents()[8]?.all.namedItem('Immunization')?.children[0]?.contentDocument?.all?.namedItem('btnAdd') != null && $('.clsIframe')?.contents()[8]?.all?.namedItem('Immunization')?.children[0]?.contentDocument?.all?.namedItem('btnAdd') != undefined) {
                            $('.clsIframe').contents()[8].all.namedItem('Immunization').children[0].contentDocument.all.namedItem('btnAdd').click();
                        }

                    }
                    else if (subtab == "Procedures") {
                        if ($('.clsIframe')?.contents()[8]?.all.namedItem('InHouseProcedure')?.children[0]?.contentDocument?.all?.namedItem('btnAdd') != null && $('.clsIframe')?.contents()[8]?.all?.namedItem('InHouseProcedure')?.children[0]?.contentDocument?.all?.namedItem('btnAdd') != undefined) {
                            $('.clsIframe').contents()[8].all.namedItem('InHouseProcedure').children[0].contentDocument.all.namedItem('btnAdd').click();
                        }

                    }
                    else if (subtab == "DME Order") {
                        if ($('.clsIframe')?.contents()[8]?.all?.namedItem('DMEOrder')?.children[0]?.contentDocument?.all?.namedItem('btnAdd') != null && $('.clsIframe')?.contents()[8]?.all?.namedItem('DMEOrder')?.children[0]?.contentDocument?.all?.namedItem('btnAdd') != undefined) {
                            $('.clsIframe').contents()[8].all.namedItem('DMEOrder').children[0].contentDocument.all.namedItem('btnAdd').click();
                        }

                    }
                    //if (localStorage.getItem("bSave") == "true") {
                    //    paneID = $(event.target).attr('href');
                    //    src = $(paneID).attr('data-src');
                    //    $(paneID + " iframe").attr("src", src);
                    //}
                    //else {
                    //    bCancel = true;
                    //    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                    //    PrevTab.tab('show');
                    //    return;
                    //}
                }
                else if (PrevTab[0].innerText == "eRx") {
                    paneID = $(event.target).attr('href');
                    sessionStorage.setItem("Enc_PaneId", paneID);
                    sessionStorage.setItem("Enc_Src", $(paneID).attr('data-src'));
                    var prvTab = PrevTab.attr('href');
                    if (prvTab.match("#tbEPrescription") != null) {
                        $.ajax({
                            type: "POST",
                            url: "frmEncounter.aspx/DownloadRcoipa",
                            contentType: "application/json;charset=utf-8",
                            dataType: "json",
                            async: true,
                            success: function (data) {
                                reloadSummary();
                            },
                            error: function OnError(xhr) {
                                var log = JSON.parse(xhr.responseText);
                                console.log(log);
                                if (xhr.status == 999)
                                    window.location = "/frmSessionExpired.aspx";
                                else
                                    alert("USER MESSAGE:\n" +
                                        ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                                        "Message: " + log.Message);
                                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                            }
                        });
                    }
                    //CAP-625: Handle Click event
                    if ($('.clsIframe')?.contents()[9]?.all?.namedItem('btnsave') != null && $('.clsIframe')?.contents()[9]?.all?.namedItem('btnsave') != undefined) {
                        $('.clsIframe').contents()[9].all.namedItem('btnsave').click();
                    }

                    //if (localStorage.getItem("bSave") == "true" && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value == "false") {
                    //    paneID = $(event.target).attr('href');
                    //    src = $(paneID).attr('data-src');
                    //    $(paneID + " iframe").attr("src", src);
                    //}
                    //else {
                    //    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                    //    bCancel = true;
                    //    PrevTab.tab('show');
                    //    return;
                    //}
                }
                else if (PrevTab[0].innerText == "SERV./PROC. CODES") {

                    //BugID:51562 
                    var prvTab = PrevTab.attr('href');
                    if (prvTab.match("#tbEandM") != null) {
                        //CAP-625: Handle Click event         
                        if ($(top.window.document)?.find(".in")?.find(".close") != null && $(top.window.document)?.find(".in")?.find(".close") != undefined && $(top.window.document)?.find(".in")?.find(".close")?.length > 1) {
                            $(top.window.document).find(".in").find(".close")[0].click();
                        }
                        //if ($(top.window.document).find(".in").find(".close") != null && $(top.window.document).find(".in").find(".close") != undefined && $(top.window.document).find(".in").find(".close").length > 1)
                    }

                    paneID = $(event.target).attr('href');
                    sessionStorage.setItem("Enc_PaneId", paneID);
                    sessionStorage.setItem("Enc_Src", $(paneID).attr('data-src'));
                    //CAP-625: Handle Click event
                    if ($('.clsIframe')?.contents()[11]?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[11]?.all?.namedItem('btnSave') != undefined) {
                        $('.clsIframe').contents()[11].all.namedItem('btnSave').click();
                    }
                    //if ($('.clsIframe').contents()[11].all.namedItem('btnSave') != null && $('.clsIframe').contents()[11].all.namedItem('btnSave') != undefined)
                    //    $('.clsIframe').contents()[11].all.namedItem('btnSave').click();

                    //if (localStorage.getItem("bSave") == "true" && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value == "false") {
                    //    paneID = $(event.target).attr('href');
                    //    src = $(paneID).attr('data-src');
                    //    $(paneID + " iframe").attr("src", src);
                    //}
                    //else {
                    //    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                    //    bCancel = true;
                    //    sessionStorage.setItem("EncCancel", "true");
                    //    PrevTab.tab('show');
                    //    return;
                    //}
                }
                else if (PrevTab[0].innerText == "PLAN") {
                    var subtab = localStorage.getItem("PrevSubTab");
                    paneID = $(event.target).attr('href');
                    sessionStorage.setItem("Enc_PaneId", paneID);
                    sessionStorage.setItem("Enc_Src", $(paneID).attr('data-src'));
                    if (subtab == "General Plan") {
                        //CAP-625: Handle Click event
                        if ($('.clsIframe')?.contents()[10]?.all?.namedItem('generalplan')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[10]?.all?.namedItem('generalplan')?.children[0]?.contentDocument.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[10].all.namedItem('generalplan').children[0].contentDocument.all.namedItem('btnSave').click();
                        }
                    }
                    else if (subtab == "Individualized CarePlan") {
                        //CAP-625: Handle Click event
                        if ($('.clsIframe')?.contents()[10]?.all?.namedItem('IndividualCarePlan')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[10]?.all?.namedItem('IndividualCarePlan')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[10].all.namedItem('IndividualCarePlan').children[0].contentDocument.all.namedItem('btnSave').click();
                        }
                    }
                    else if (subtab == "Preventive Screen Plan") {
                        //CAP-625: Handle Click event
                        if ($('.clsIframe')?.contents()[10]?.all?.namedItem('PreventiveScreen')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[10]?.all?.namedItem('PreventiveScreen')?.children[0]?.contentDocument?.all?.namedItem('btnSave') != undefined) {
                            $('.clsIframe').contents()[10].all.namedItem('PreventiveScreen').children[0].contentDocument.all.namedItem('btnSave').click();
                        }
                    }
                    //if (localStorage.getItem("bSave") == "true") {
                    //    paneID = $(event.target).attr('href');
                    //    src = $(paneID).attr('data-src');
                    //    $(paneID + " iframe").attr("src", src);
                    //}
                    //else {
                    //    bCancel = true;
                    //    sessionStorage.setItem("EncCancel", "true");
                    //    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                    //    PrevTab.tab('show');
                    //    return;
                    //}
                }
                else if (PrevTab[0].innerText == "SUMMARY") {
                    //CAP-625: Handle Click event
                    if ($('.clsIframe')?.contents()[12]?.all?.namedItem('btnSave') != null && $('.clsIframe')?.contents()[12]?.all?.namedItem('btnSave') != undefined) {
                        $('.clsIframe').contents()[12].all.namedItem('btnSave').click();
                    }
                    paneID = $(event.target).attr('href');
                    src = $(paneID).attr('data-src');
                    $(paneID + " iframe").attr("src", src);
                }

                return;

                //$(top.window.document).find("body").append("<div id='dvdialog' style='min-height: 65px !important; width: auto; max-height: none; height: auto; display: none;'>" +
                //               "<p style='font-family: Verdana,Arial,sans-serif; font-size: 13.5px;'>There are unsaved changes.Do you want to save them?</p></div>");
                //dvdialog = window.parent.parent.parent.parent.document.getElementsByTagName('div').namedItem('dvdialog');
                //event.preventDefault();
                // {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
                //$(dvdialog).dialog({
                //    modal: true,
                //    title: "Capella EHR",
                //    position: {
                //        my: 'left' + " " + 'center',
                //        at: 'center' + " " + 'center + 100px'

                //    },
                //    buttons: {
                //        "Yes": function () {

                //            event.preventDefault();
                //            $(dvdialog).dialog("close");
                //            $(dvdialog).remove();
                //            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
                //            sessionStorage.setItem("EncCancel", "false");
                //            sessionStorage.setItem("EncPrevTabText", PrevTab[0].innerText);
                //            sessionStorage.setItem("EncAutoSave", "true");
                //            if (PrevTab[0].innerText == "CC / HPI") {
                //                paneID = $(event.target).attr('href');
                //                sessionStorage.setItem("Enc_PaneId", paneID);
                //                sessionStorage.setItem("Enc_Src", $(paneID).attr('data-src'));
                //                $('.clsIframe').contents()[0].all.namedItem('btnAdd').click();
                //                //if (localStorage.getItem("bSave") == "true" && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value == "false") {
                //                //    paneID = $(event.target).attr('href');
                //                //    src = $(paneID).attr('data-src');
                //                //    $(paneID + " iframe").attr("src", src);
                //                //}
                //                //else {
                //                //    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                //                //    bCancel = true;
                //                //    sessionStorage.setItem("EncCancel", "true");
                //                //    PrevTab.tab('show');
                //                //    return;
                //                //}
                //            }
                //            else if (PrevTab[0].innerText == "SCREENING") {
                //                var subtab = localStorage.getItem("PrevSubTab");
                //                paneID = $(event.target).attr('href');
                //                sessionStorage.setItem("Enc_PaneId", paneID);
                //                sessionStorage.setItem("Enc_Src", $(paneID).attr('data-src'));
                //                if (subtab == "General") {
                //                    $('.clsIframe').contents()[1].all.namedItem('General').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "TB Risk Assessment") {
                //                    $('.clsIframe').contents()[1].all.namedItem('TBRiskAssessment').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "Diabetic Foot Screening") {
                //                    $('.clsIframe').contents()[1].all.namedItem('DiabeticFootScreening').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "Sleep Screening") {
                //                    $('.clsIframe').contents()[1].all.namedItem('SleepScreening').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "Sleep") {
                //                    $('.clsIframe').contents()[1].all.namedItem('Sleep').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "Pulmonary") {
                //                    $('.clsIframe').contents()[1].all.namedItem('Pulmonary').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "Epworth Sleep Score") {
                //                    $('.clsIframe').contents()[1].all.namedItem('EpworthSleepScore').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "Pulmonary/Sleep Exam") {
                //                    $('.clsIframe').contents()[1].all.namedItem('PulmonarySleepExam').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "Dermatology Questionnaire") {
                //                    $('.clsIframe').contents()[1].all.namedItem('DermatologyQuestionnaire').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "Asthma Control Test") {
                //                    $('.clsIframe').contents()[1].all.namedItem('AsthmaControlTest').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "PHQ-9 Screening") {
                //                    $('.clsIframe').contents()[1].all.namedItem('PHQ-9Screening').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "Depression Test") {
                //                    $('.clsIframe').contents()[1].all.namedItem('DepressionTest').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "Neck Disability Index") {
                //                    $('.clsIframe').contents()[1].all.namedItem('NeckDisabilityIndex').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "Oswestry Disability Index") {
                //                    $('.clsIframe').contents()[1].all.namedItem('OswestryDisabilityIndex').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "Development") {
                //                    $('.clsIframe').contents()[1].all.namedItem('Development').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }

                //                else if (subtab == "Chronic Cough Scale") {
                //                    $('.clsIframe').contents()[1].all.namedItem('ChronicCoughScale').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "Gynecological") {
                //                    $('.clsIframe').contents()[1].all.namedItem('Gynecological').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "Pediatric Sleep Questionnaire") {
                //                    $('.clsIframe').contents()[1].all.namedItem('PediatricSleepQuestionnaire').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "Sleep Short") {
                //                    $('.clsIframe').contents()[1].all.namedItem('SleepShort').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "Sleep Tendency Scale") {
                //                    $('.clsIframe').contents()[1].all.namedItem('SleepTendencyScale').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "Functional Assessment") {
                //                    $('.clsIframe').contents()[1].all.namedItem('FunctionalAssessment').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "Karnofsky") {
                //                    $('.clsIframe').contents()[1].all.namedItem('Karnofsky').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "Mini Mental") {
                //                    $('.clsIframe').contents()[1].all.namedItem('MiniMental').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "Nutritional Screening") {
                //                    $('.clsIframe').contents()[1].all.namedItem('NutritionalScreening').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "Safety Guidelines") {
                //                    $('.clsIframe').contents()[1].all.namedItem('SafetyGuidelines').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "Support Needs") {
                //                    $('.clsIframe').contents()[1].all.namedItem('SupportNeeds').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "Pain Assessment") {
                //                    $('.clsIframe').contents()[1].all.namedItem('PainAssessment').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "CAT Screening") {
                //                    $('.clsIframe').contents()[1].all.namedItem('CATScreening').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "COPD Breathe Well Program") {
                //                    $('.clsIframe').contents()[1].all.namedItem('COPDBreatheWellProgram').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "Fall Risk Screening") {
                //                    $('.clsIframe').contents()[1].all.namedItem('FallRiskScreening').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "Cognitive Screening") {
                //                    $('.clsIframe').contents()[1].all.namedItem('CognitiveScreening').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "Pain Screening") {
                //                    $('.clsIframe').contents()[1].all.namedItem('PainScreening').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "PHQ-9 Screening") {
                //                    $('.clsIframe').contents()[1].all.namedItem('PHQ-9Screening').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "Home Safety") {
                //                    $('.clsIframe').contents()[1].all.namedItem('HomeSafety').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "Katz Index Screening") {
                //                    $('.clsIframe').contents()[1].all.namedItem('KatzIndexScreening').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "ADL Screening") {
                //                    $('.clsIframe').contents()[1].all.namedItem('ADLScreening').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "Get Up and Go") {
                //                    $('.clsIframe').contents()[1].all.namedItem('GetUpandGo').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "Lawton Screening") {
                //                    $('.clsIframe').contents()[1].all.namedItem('LawtonScreening').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "Anxiety Screening") {
                //                    $('.clsIframe').contents()[1].all.namedItem('AnxietyScreening').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "Spine Intake") {
                //                    $('.clsIframe').contents()[1].all.namedItem('SpineIntake').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "Monofilament Foot Exam") {
                //                    $('.clsIframe').contents()[1].all.namedItem('MonofilamentFootExam').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "Cervical Spine") {
                //                    $('.clsIframe').contents()[1].all.namedItem('CervicalSpine').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "Lumbar Spine") {
                //                    $('.clsIframe').contents()[1].all.namedItem('LumbarSpine').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "Urinalysis") {
                //                    $('.clsIframe').contents()[1].all.namedItem('Urinalysis').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "Staying Healthy Assessment") {
                //                    $('.clsIframe').contents()[1].all.namedItem('StayingHealthyAssessment').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "AUA BPH Symptom") {
                //                    $('.clsIframe').contents()[1].all.namedItem('AUABPHSymptom').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                //if (localStorage.getItem("bSave") == "true") {
                //                //    paneID = $(event.target).attr('href');
                //                //    src = $(paneID).attr('data-src');
                //                //    $(paneID + " iframe").attr("src", src);
                //                //}
                //                //else {
                //                //    bCancel = true;
                //                //    sessionStorage.setItem("EncCancel", "true");
                //                //    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                //                //    PrevTab.tab('show');
                //                //    return;
                //                //}
                //            }





                //            else if (PrevTab[0].innerText == "PFSH") {
                //                var subtab = localStorage.getItem("PrevSubTab");
                //                paneID = $(event.target).attr('href');
                //                sessionStorage.setItem("Enc_PaneId", paneID);
                //                sessionStorage.setItem("Enc_Src", $(paneID).attr('data-src'));
                //                if (subtab == "Past Medical History") {
                //                    $('.clsIframe').contents()[2].all.namedItem('pastmedHis').children[0].contentDocument.all.namedItem('btnSave').click();


                //                }
                //                else if (subtab == "Surg./Proc.") {
                //                    $('.clsIframe').contents()[2].all.namedItem('surgproc').children[0].contentDocument.all.namedItem('btnAdd').click();
                //                }
                //                else if (subtab == "Hospitalization History") {
                //                    $('.clsIframe').contents()[2].all.namedItem('hospHis').children[0].contentDocument.all.namedItem('btnAdd').click();
                //                }
                //                else if (subtab == "Family History") {
                //                    $('.clsIframe').contents()[2].all.namedItem('famHis').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "Social History") {
                //                    $('.clsIframe').contents()[2].all.namedItem('socialHistory').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "Non Drug Allergy") {
                //                    $('.clsIframe').contents()[2].all.namedItem('NDA').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "Immunization History") {
                //                    $('.clsIframe').contents()[2].all.namedItem('immHis').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "AD") {
                //                    $('.clsIframe').contents()[2].all.namedItem('ad').children[0].contentDocument.all.namedItem('btnPFSHAutoSave').click();
                //                }
                //                else if (subtab == "Rx History") {
                //                    $('.clsIframe').contents()[2].all.namedItem('RxH').children[0].contentDocument.all.namedItem('btnAdd').click();
                //                }
                //                //if (localStorage.getItem("bSave") == "true") {
                //                //    paneID = $(event.target).attr('href');
                //                //    src = $(paneID).attr('data-src');
                //                //    $(paneID + " iframe").attr("src", src);
                //                //}
                //                //else {
                //                //    bCancel = true;
                //                //    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                //                //    PrevTab.tab('show');
                //                //    return;
                //                //}
                //            }
                //            else if (PrevTab[0].innerText == "ROS") {
                //                paneID = $(event.target).attr('href');
                //                sessionStorage.setItem("Enc_PaneId", paneID);
                //                sessionStorage.setItem("Enc_Src", $(paneID).attr('data-src'));
                //                $('.clsIframe').contents()[3].all.namedItem('btnSave').click();
                //            }
                //            else if (PrevTab[0].innerText == "VITALS") {
                //                //  var subtab = localStorage.getItem("PrevSubTab");
                //                paneID = $(event.target).attr('href');
                //                sessionStorage.setItem("Enc_PaneId", paneID);
                //                sessionStorage.setItem("Enc_Src", $(paneID).attr('data-src'));
                //                $('.clsIframe').contents()[4].all.namedItem('btnSaveVitals').click();
                //                paneID = $(event.target).attr('href');
                //                src = $(paneID).attr('data-src');
                //                $(paneID + " iframe").attr("src", src);
                //            }
                //            else if (PrevTab[0].innerText == "EXAM") {
                //                var subtab = localStorage.getItem("PrevSubTab");
                //                paneID = $(event.target).attr('href');
                //                sessionStorage.setItem("Enc_PaneId", paneID);
                //                sessionStorage.setItem("Enc_Src", $(paneID).attr('data-src'));
                //                if (subtab == "General With Specialty") {
                //                    $('.clsIframe').contents()[5].all.namedItem('generalwithspeciality').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "Focused") {
                //                    $('.clsIframe').contents()[5].all.namedItem('Focused').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "Upload & View Photos") {
                //                    $('.clsIframe').contents()[5].all.namedItem('UploadViewPhotos').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "Body Image") {
                //                    $('.clsIframe').contents()[5].all.namedItem('BodyImage').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                //if (localStorage.getItem("bSave") == "true") {
                //                //    paneID = $(event.target).attr('href');
                //                //    src = $(paneID).attr('data-src');
                //                //    $(paneID + " iframe").attr("src", src);
                //                //}
                //                //else {
                //                //    bCancel = true;
                //                //    sessionStorage.setItem("EncCancel", "true");
                //                //    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                //                //    PrevTab.tab('show');
                //                //    return;
                //                //}
                //            }
                //            else if (PrevTab[0].innerText == "TEST") {
                //                var subtab = localStorage.getItem("PrevSubTab");
                //                paneID = $(event.target).attr('href');
                //                sessionStorage.setItem("Enc_PaneId", paneID);
                //                sessionStorage.setItem("Enc_Src", $(paneID).attr('data-src'));
                //                if (subtab == "Mini Mental Status Exam") {
                //                    $('.clsIframe').contents()[6].all.namedItem('MiniMentalStatusExam').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "Depression Screening") {
                //                    $('.clsIframe').contents()[6].all.namedItem('DepressionScreening').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "Spiritual Care Assessment") {
                //                    $('.clsIframe').contents()[6].all.namedItem('SpiritualCareAssessment').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                //if (localStorage.getItem("bSave") == "true") {
                //                //    paneID = $(event.target).attr('href');
                //                //    src = $(paneID).attr('data-src');
                //                //    $(paneID + " iframe").attr("src", src);
                //                //}
                //                //else {
                //                //    bCancel = true;
                //                //    sessionStorage.setItem("EncCancel", "true");
                //                //    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                //                //    PrevTab.tab('show');
                //                //    return;
                //                //}
                //            }
                //            else if (PrevTab[0].innerText == "ASSESSMENT") {
                //                paneID = $(event.target).attr('href');
                //                sessionStorage.setItem("Enc_PaneId", paneID);
                //                sessionStorage.setItem("Enc_Src", $(paneID).attr('data-src'));
                //CAP 304
                //                $('.clsIframe').contents()[7]?.all?.namedItem('btnSave')?.click();
                //                //if (localStorage.getItem("bSave") == "true") {
                //                //    paneID = $(event.target).attr('href');
                //                //    src = $(paneID).attr('data-src');
                //                //    $(paneID + " iframe").attr("src", src);
                //                //}
                //                //else {
                //                //    bCancel = true;
                //                //    sessionStorage.setItem("EncCancel", "true");
                //                //    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                //                //    PrevTab.tab('show');
                //                //    return;
                //                //}
                //            }
                //            else if (PrevTab[0].innerText == "ORDERS") {
                //                var subtab = localStorage.getItem("PrevSubTab");
                //                paneID = $(event.target).attr('href');
                //                sessionStorage.setItem("Enc_PaneId", paneID);
                //                sessionStorage.setItem("Enc_Src", $(paneID).attr('data-src'));
                //if (subtab == "Diagnostic Order") {
                //if ($('.clsIframe').contents()[8].all.namedItem('DiagnosticOrder').children[0].contentDocument.all.namedItem('btnOrderSubmit') != null && $('.clsIframe').contents()[8].all.namedItem('DiagnosticOrder').children[0].contentDocument.all.namedItem('btnOrderSubmit') != undefined) {
                // $('.clsIframe').contents()[8]?.all?.namedItem('DiagnosticOrder').children[0]?.contentDocument.all?.namedItem('btnOrderSubmit')?.click();
                // }

                //}
                //                else if (subtab == "Referral Order") {
                //                    $('.clsIframe').contents()[8].all.namedItem('ReferralOrder').children[0].contentDocument.all.namedItem('btnAddRefOrder').click();
                //                }
                //                else if (subtab == "Immunization/Injection") {
                //                    $('.clsIframe').contents()[8].all.namedItem('Immunization').children[0].contentDocument.all.namedItem('btnAdd').click();
                //                }
                //                else if (subtab == "Procedures") {
                //                    $('.clsIframe').contents()[8].all.namedItem('InHouseProcedure').children[0].contentDocument.all.namedItem('btnAdd').click();
                //                }
                //                else if (subtab == "DME Order") {
                //                    $('.clsIframe').contents()[8].all.namedItem('DMEOrder').children[0].contentDocument.all.namedItem('btnAdd').click();
                //                }
                //                //if (localStorage.getItem("bSave") == "true") {
                //                //    paneID = $(event.target).attr('href');
                //                //    src = $(paneID).attr('data-src');
                //                //    $(paneID + " iframe").attr("src", src);
                //                //}
                //                //else {
                //                //    bCancel = true;
                //                //    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                //                //    PrevTab.tab('show');
                //                //    return;
                //                //}
                //            }
                //            else if (PrevTab[0].innerText == "eRx") {
                //                paneID = $(event.target).attr('href');
                //                sessionStorage.setItem("Enc_PaneId", paneID);
                //                sessionStorage.setItem("Enc_Src", $(paneID).attr('data-src'));
                //                var prvTab = PrevTab.attr('href');
                //                if (prvTab.match("#tbEPrescription") != null) {
                //                    $.ajax({
                //                        type: "POST",
                //                        url: "frmEncounter.aspx/DownloadRcoipa",
                //                        contentType: "application/json;charset=utf-8",
                //                        dataType: "json",
                //                        async: true,
                //                        success: function (data) {
                //                            reloadSummary();
                //                        },
                //                        error: function OnError(xhr) {
                //                            var log = JSON.parse(xhr.responseText);
                //                            console.log(log);
                //                            if (xhr.status == 999)
                //                                window.location = xhr.statusText;
                //                            else
                //                                alert("USER MESSAGE:\n" +
                //                     ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                //                    "Message: " + log.Message);
                //                             {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
                //                        }
                //                    });
                //                }
                //                $('.clsIframe').contents()[9].all.namedItem('btnsave').click();
                //                //if (localStorage.getItem("bSave") == "true" && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value == "false") {
                //                //    paneID = $(event.target).attr('href');
                //                //    src = $(paneID).attr('data-src');
                //                //    $(paneID + " iframe").attr("src", src);
                //                //}
                //                //else {
                //                //    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                //                //    bCancel = true;
                //                //    PrevTab.tab('show');
                //                //    return;
                //                //}
                //            }
                //            else if (PrevTab[0].innerText == "SERV./PROC. CODES") {

                //                //BugID:51562 
                //                var prvTab = PrevTab.attr('href');
                //                if (prvTab.match("#tbEandM") != null) {
                //                    if ($(top.window.document).find(".in").find(".close") != undefined && $(top.window.document).find(".in").find(".close").length > 1)
                //                        $(top.window.document).find(".in").find(".close")[0].click();
                //                }

                //                paneID = $(event.target).attr('href');
                //                sessionStorage.setItem("Enc_PaneId", paneID);
                //                sessionStorage.setItem("Enc_Src", $(paneID).attr('data-src'));

                //                if ($('.clsIframe').contents()[11].all.namedItem('btnSave') != null && $('.clsIframe').contents()[11].all.namedItem('btnSave')!=undefined)
                //                $('.clsIframe').contents()[11].all.namedItem('btnSave').click();
                //                //if (localStorage.getItem("bSave") == "true" && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value == "false") {
                //                //    paneID = $(event.target).attr('href');
                //                //    src = $(paneID).attr('data-src');
                //                //    $(paneID + " iframe").attr("src", src);
                //                //}
                //                //else {
                //                //    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                //                //    bCancel = true;
                //                //    sessionStorage.setItem("EncCancel", "true");
                //                //    PrevTab.tab('show');
                //                //    return;
                //                //}
                //            }
                //            else if (PrevTab[0].innerText == "PLAN") {
                //                var subtab = localStorage.getItem("PrevSubTab");
                //                paneID = $(event.target).attr('href');
                //                sessionStorage.setItem("Enc_PaneId", paneID);
                //                sessionStorage.setItem("Enc_Src", $(paneID).attr('data-src'));
                //                if (subtab == "General Plan") {
                //                    $('.clsIframe').contents()[10].all.namedItem('generalplan').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "Individualized CarePlan") {
                //                    $('.clsIframe').contents()[10].all.namedItem('IndividualCarePlan').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                else if (subtab == "Preventive Screen Plan") {
                //                    $('.clsIframe').contents()[10].all.namedItem('PreventiveScreen').children[0].contentDocument.all.namedItem('btnSave').click();
                //                }
                //                //if (localStorage.getItem("bSave") == "true") {
                //                //    paneID = $(event.target).attr('href');
                //                //    src = $(paneID).attr('data-src');
                //                //    $(paneID + " iframe").attr("src", src);
                //                //}
                //                //else {
                //                //    bCancel = true;
                //                //    sessionStorage.setItem("EncCancel", "true");
                //                //    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                //                //    PrevTab.tab('show');
                //                //    return;
                //                //}
                //            }
                //            else if (PrevTab[0].innerText == "SUMMARY") {
                //                $('.clsIframe').contents()[12].all.namedItem('btnSave').click();
                //                paneID = $(event.target).attr('href');
                //                src = $(paneID).attr('data-src');
                //                $(paneID + " iframe").attr("src", src);
                //            }

                //            return;
                //        },
                //        "No": function () {
                //            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
                //            localStorage.setItem("bSave", "true");
                //            sessionStorage.setItem("EncCancel", "false");
                //            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";

                //            var prvTab = PrevTab.attr('href');
                //            if (prvTab.match("#tbEPrescription") != null) {
                //                $.ajax({
                //                    type: "POST",
                //                    url: "frmEncounter.aspx/DownloadRcoipa",
                //                    contentType: "application/json;charset=utf-8",
                //                    dataType: "json",
                //                    async: true,
                //                    success: function (data) {
                //                        reloadSummary();
                //                    },
                //                    error: function OnError(xhr) {
                //                        var log = JSON.parse(xhr.responseText);
                //                        console.log(log);
                //                        if (xhr.status == 999)
                //                            window.location = xhr.statusText;
                //                        else
                //                            alert("USER MESSAGE:\n" +
                //                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                //                   "Message: " + log.Message);
                //                         {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
                //                    }
                //                });
                //            }

                //            //BugID:51562 
                //            if (prvTab.match("#tbEandM") != null) {
                //                if ($(top.window.document).find(".in").find(".close") != undefined && $(top.window.document).find(".in").find(".close").length > 1)
                //                    $(top.window.document).find(".in").find(".close")[0].click();
                //            }

                //            $(dvdialog).dialog("close");
                //            $(dvdialog).remove();
                //            paneID = $(event.target).attr('href');
                //            // version inclusion for htmlPages
                //            if(AspxPages.indexOf(paneID)==-1)
                //            {
                //                var HtmlVersion = $(paneID)[0].attributes.getNamedItem('data-src').nodeValue;
                //                if (HtmlVersion.indexOf('?') > -1) {
                //                    if (HtmlVersion.split('?')[1].split("=")[1] != sessionStorage.getItem("ScriptVersion")) {
                //                        var HtmlVersion = $(paneID)[0].attributes.getNamedItem('data-src').nodeValue.split('?')[0] + "?version=" + sessionStorage.getItem("ScriptVersion");
                //                    }
                //                }
                //                else
                //                    var HtmlVersion = $(paneID)[0].attributes.getNamedItem('data-src').nodeValue + "?version=" + sessionStorage.getItem("ScriptVersion");

                //                $(paneID).attr('data-src', HtmlVersion);
                //            }
                //            src = $(paneID).attr('data-src');
                //            // if the iframe hasn't already been loaded once
                //            $(paneID + " iframe").attr("src", src);
                //        },
                //        "Cancel": function () {
                //            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
                //            event.preventDefault();
                //            bCancel = true;
                //            sessionStorage.setItem("EncCancel", "true");
                //            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                //            $(dvdialog).dialog("close");
                //            $(dvdialog).remove();
                //            PrevTab.tab('show');
                //             {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
                //            return;

                //        }
                //    }
                //});
            }
            else {
                if ($(".ui-dialog").is(":visible")) {
                    $(dvdialog).dialog("close");
                    $(dvdialog).remove();
                }
                paneID = $(event.target).attr('href');
                // version inclusion for htmlPages
                if (AspxPages.indexOf(paneID) == -1)
                {
                    var HtmlVersion = $(paneID)[0].attributes.getNamedItem('data-src').nodeValue;
                    if (HtmlVersion.indexOf('?') > -1) {
                        if (HtmlVersion.split('?')[1].split("=")[1] != sessionStorage.getItem("ScriptVersion")) {
                            var HtmlVersion = $(paneID)[0].attributes.getNamedItem('data-src').nodeValue.split('?')[0] + "?version=" + sessionStorage.getItem("ScriptVersion");
                        }
                    }
                    else
                        var HtmlVersion = $(paneID)[0].attributes.getNamedItem('data-src').nodeValue + "?version=" + sessionStorage.getItem("ScriptVersion");

                    $(paneID).attr('data-src', HtmlVersion);
                }

                //BugID:51562 
                var prvTab = PrevTab.attr('href');
                if (prvTab.match("#tbEandM") != null) {
                    //CAP-625: Handle Click event
                    if ($(top.window.document).find(".in").find(".close") != null && $(top.window.document).find(".in").find(".close") != undefined && $(top.window.document).find(".in").find(".close").length > 1)
                        $(top.window.document).find(".in").find(".close")[0].click();
                }

                var prvTab = PrevTab.attr('href');
                if (prvTab.match("#tbEPrescription") != null) {
                    $.ajax({
                        type: "POST",
                        url: "frmEncounter.aspx/DownloadRcoipa",
                        contentType: "application/json;charset=utf-8",
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            reloadSummary();
                            if (JSON.parse(sessionStorage.getItem("EncCancel")) == false) {
                                src = $(paneID).attr('data-src');
                                $(paneID + " iframe").attr("src", src);
                            } else {
                                localStorage.setItem("bSave", "false");
                                window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
                                bCancel = false;
                                sessionStorage.setItem("EncCancel", "false");
                            }
                        },
                        error: function OnError(xhr) {
                            var log = JSON.parse(xhr.responseText);
                            console.log(log);
                            if (xhr.status == 999)
                                window.location = "/frmSessionExpired.aspx";
                            else
                                alert("USER MESSAGE:\n" +
                                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                                    "Message: " + log.Message);
                            {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
                        }
                    });
                }
                else {
                    if (JSON.parse(sessionStorage.getItem("EncCancel")) == false) {
                        src = $(paneID).attr('data-src');
                        $(paneID + " iframe").attr("src", src);
                        //if ($(paneID + " iframe").attr("src") == "") {
                        //    $(paneID + " iframe").attr("src", src);
                        //}
                    } else {
                        localStorage.setItem("bSave", "false");
                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
                        bCancel = false;
                        sessionStorage.setItem("EncCancel", "false");
                    }
                }
                //if (bCancel == false) {


            }


            paneID = $(event.target).attr('href');
            // version inclusion for htmlPages
            if (AspxPages.indexOf(paneID) == -1){
                var HtmlVersion = $(paneID)[0].attributes.getNamedItem('data-src').nodeValue;
                if (HtmlVersion.indexOf('?') > -1) {
                    if (HtmlVersion.split('?')[1].split("=")[1] != sessionStorage.getItem("ScriptVersion")) {
                        var HtmlVersion = $(paneID)[0].attributes.getNamedItem('data-src').nodeValue.split('?')[0] + "?version=" + sessionStorage.getItem("ScriptVersion");
                    }
                }
                else
                    var HtmlVersion = $(paneID)[0].attributes.getNamedItem('data-src').nodeValue + "?version=" + sessionStorage.getItem("ScriptVersion");

                $(paneID).attr('data-src', HtmlVersion);
            }


        }
        else
            sessionStorage.setItem("Encounter_PrevTabRevert", false);
    });

</script>
