<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmLandingScreen.aspx.cs" Inherits="Acurus.Capella.UI.frmLandingScreen" EnableEventValidation="true" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="CSS/font-awesome.4.4.0.css" rel="stylesheet" />
    <script src="JScripts/jquery-1.11.3.min.js" type="text/javascript"></script>
     <style>
        @media screen and (min-width: 0px) and (max-width: 720px) {

            .mobile-hide {
                display: none;
            }
        }

        #carouselButtons {
            margin-left: 100px;
            position: absolute;
            bottom: 0px;
        }

        body, html {
            /*height: 100%;
            background-repeat: no-repeat;
            background-image: linear-gradient(rgb(104, 145, 162), rgb(12, 97, 33));*/
        }

        .left-inner-addon {
            position: relative;
        }

        .card-container.card {
            /*max-width: 360px;*/
            max-width: 600px;
            padding: 40px 40px;
        }

        .btn {
            font-weight: 700;
            height: 36px;
            -moz-user-select: none;
            -webkit-user-select: none;
            user-select: none;
            cursor: default;
        }

        /*
 * Card component
 */
        .card {
            background-color: #F7F7F7;
            /* just in case there no content*/
            padding: 20px 25px 30px;
            /*margin: 0 auto 25px;
            margin-top: 50px;*/
            /* shadows and rounded borders */
            -moz-border-radius: 2px;
            -webkit-border-radius: 2px;
            border-radius: 2px;
            -moz-box-shadow: 0px 2px 2px rgba(0, 0, 0, 0.3);
            -webkit-box-shadow: 0px 2px 2px rgba(0, 0, 0, 0.3);
            box-shadow: 0px 2px 2px rgba(0, 0, 0, 0.3);
        }

        .profile-img-card {
            width: 96px;
            height: 96px;
            margin: 0 auto 10px;
            display: block;
            -moz-border-radius: 50%;
            -webkit-border-radius: 50%;
            border-radius: 50%;
        }

        /*
 * Form styles
 */
        .profile-name-card {
            font-size: 16px;
            font-weight: bold;
            text-align: center;
            margin: 10px 0 0;
            min-height: 1em;
        }

        .reauth-email {
            display: block;
            color: #404040;
            line-height: 2;
            margin-bottom: 10px;
            font-size: 14px;
            text-align: center;
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
            -moz-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            box-sizing: border-box;
        }

        .form-signin #inptUsername,
        .form-signin #inptPassword {
            direction: ltr;
            height: 37px;
            font-size: 16px;
        }

        .form-signin input[type=email],
        .form-signin input[type=password],
        .form-signin input[type=text],
        .form-signin select,
        .form-signin button {
            width: 100%;
            display: block;
            margin-bottom: 10px;
            z-index: 1;
            position: relative;
            -moz-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            box-sizing: border-box;
        }

        .form-signin .form-control:focus {
            border-color: rgb(104, 145, 162);
            outline: 0;
            -webkit-box-shadow: inset 0 1px 1px rgba(0,0,0,.075),0 0 8px rgb(104, 145, 162);
            box-shadow: inset 0 1px 1px rgba(0,0,0,.075),0 0 8px rgb(104, 145, 162);
        }

        .rowspace {
            margin-top: 0.5%;
            margin-bottom: 0.5%;
        }

        .loginimage {
            height: 70px;
            width: 180px;
        }




        /*#ulKnowledgecenter li {
            background: url("./Images/1436347083_sign-in.png") no-repeat;
            list-style-type: none;
        }

        #ulSystemMessages li {
            background: url("./Images/1436347083_sign-in.png") no-repeat;
            list-style-type: none;
        }*/

        .fixed-panel {
            /*overflow-y: scroll;*/
            height: 340px;
        }


        .rcorners3 {
            border-radius: 5px;
            background: url('../Images/ACURUS Logo_Modified.png');
            /*background-position: left top;
            background-repeat: repeat;*/
            width: 380px;
            height: 343px;
            padding-left: 30px;
        }

        .transparent {
        }

        .support {
        }

        .Priority {
            font-size: 14px;
            border-radius: 3px;
            padding-top: 1px;
            margin-left: 3%;
        }

        .panel-success > .panel-heading {
            /*color: #0951AD !important;
            background-color: #bfdbff !important;
             border-color: #bfdbff !important;*/
            /*new*/
            /*background-color: #3aa04a !important;
            border-color: #3aa04a !important*/
        }

        .panel-success {
            /*border-color: #bfdbff !important;*/
            /*border-color: #3aa04a !important;*/
        }

        #ulSystemMessages li {
            list-style-type: disc !important;
        }

        .logocolor {
            height: 36px !important;
            background-color: white !important;
            font-family: Mongolian Baiti !important;
            font-weight: bold !important;
            font-size: xx-large !important;
            /*height: 36px; background-color: white; font-family: Mongolian Baiti; color: Red; font-weight: bold; font-size: xx-large;*/
        }

        .nopadding {
            padding: 0 !important;
            margin: 0 !important;
        }
        .centered{
            position: absolute;
            top: 11%;
            left: 50%;
            /*transform: translate(-50%, -50%);*/
            /*height: 100px;*/

        }

        .BackgroundImage{
            background: url(../Resources/maintenance.jpg);
            background-repeat: no-repeat;
            height:1000px;
            width:2000px;
            background-size:1700px 794px;


        }
    </style>
    <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>


             <div id="divErrorMessage" class="modal modal-wide fade" style="z-index: 50000000!important;">
                        <div class="modal-dialog" style="width: 30%; margin-top: 0%;">
                            <div class="modal-content">
                                <div class="modal-header" style="height: 46px;">
                                    <div class="row" style="margin-top: -25px;">
                                        <div class="col-xs-8">
                                            <h3>Capella -EHR</h3>
                                        </div>
                                        <div class="col-xs-4" style="margin-top: 17px;">
                                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                        </div>
                                    </div>
                                </div>
                                <div class="modal-body" id="divErrorMsgBody">
                                    <p style="font-size: small;" id="pErrorMsg"></p>

                                </div>
                                <div class="modal-footer" style="height: 50px;">
                                    <button id="btnErrorOk" style="margin-top: -10px; border: 1px solid !important;">Ok</button>
                                    <button id="btnErrorCancel" style="margin-top: -10px; border: 1px solid !important;">Cancel</button>
                                    <button id="btnErrorOkCancel" data-dismiss="modal" style="margin-top: -10px; border: 1px solid !important;">OK</button>
                                </div>
                            </div>
                        </div>
                    </div>

            <button id="hdnbtnLogin" runat="server" style="display: none;" onserverclick="hdnbtnLogin_Click">hdnLogin</button>
            <%--<asp:HiddenField ID="hdnLocalTime" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnLocalDate" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnUniversaloffset" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnLocalDateAndTime" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnFollowsDayLightSavings" runat="server" Value="false" />--%>
            <asp:HiddenField ID="hdnGroupId" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnPersonName" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnVersion" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnProjectName" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnreportPath" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnLoginheader" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnVersionKey" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnServiceLink" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="HiddenField1" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnEvProjectName" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnReportPathhttp" runat="server" EnableViewState="false" />

            <asp:HiddenField ID="hdnroleLanding" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnRCopia_User_NameLanding" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnIs_RCopia_Notification_RequiredLanding" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnPhysician_Library_IDLanding" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnLanding_Screen_IDLanding" runat="server" Value="false" />
            <asp:HiddenField ID="hdnEMailAddress" runat="server" Value="false" />

            <link href="CSS/bootstrap.min3.1.1.css" rel="stylesheet" />
            <script src="JScripts/bootstrap.min3.1.1.max.js" type="text/javascript"></script>
            <script src="JScripts/JSLandingScreen.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"]%>"></script>
            <script src="JScripts/jsErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"]%>"></script>
        </div>
    </form>
</body>
</html>