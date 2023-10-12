<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmLogin.aspx.cs" Inherits="Acurus.Capella.UI.frmLogin" EnableEventValidation="false" %>

<!DOCTYPE html>

<html lang="en">
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width,height=device-height initial-scale=1">
    <title>Login</title>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <link href="CSS/font-awesome.4.4.0.css" rel="stylesheet" />

    <script src="JScripts/jquery-1.11.3.min.js" type="text/javascript"></script>

    <%--<script type="text/javascript">
        //$("#btnOk").click(function (e) {
        //    return true;
        //});
    
        var version = '<%=ConfigurationManager.AppSettings["VersionConfiguration"]%>';
        sessionStorage.setItem("ScriptVersion", version.split('-')[1].trim());

        var ProjectName = '<%#ConfigurationManager.AppSettings["ProjectName"]%>';
        sessionStorage.setItem("Projname", ProjectName.trim().toUpperCase());

        var ReportPath = '<%#ConfigurationManager.AppSettings["Reportpath"]%>';
        sessionStorage.setItem("ReportPath", ReportPath);
        var LoginHeader = '<%#ConfigurationManager.AppSettings["LoginHeader"]%>';
       
        var versionkey = '<%#ConfigurationManager.AppSettings["versionkey"]%>';
        sessionStorage.setItem("versionkey", versionkey);

        var vEVServiceLink = '<%#ConfigurationManager.AppSettings["EVServiceLink"]%>';
        sessionStorage.setItem("EVWebServiceLink", vEVServiceLink);

        var vEVProjectName = '<%#ConfigurationManager.AppSettings["EVProjectName"]%>';
        sessionStorage.setItem("EVProjectName", vEVProjectName);

    </script>--%>
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
    <%--<link href="CSS/font-awesome.css" rel="stylesheet" type="text/css" />--%>
    <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
</head>
<body style="width: 100%;">
    <%--<script type="text/javascript">!function (e, t, n) {
    function a() {
        var e = t.getElementsByTagName("script")[0], n = t.createElement("script"); n.type = "text/javascript", n.async = !0,
        n.src = "https://beacon-v2.helpscout.net", e.parentNode.insertBefore(n, e)
    } if (e.Beacon = n = function (t, n, a) { e.Beacon.readyQueue.push({ method: t, options: n, data: a }) },
        n.readyQueue = [], "complete" === t.readyState) return a();
    e.attachEvent ? e.attachEvent("onload", a) : e.addEventListener("load", a, !1)
}(window, document, window.Beacon || function () { });</script>--%>

    <%--<script type="text/javascript">
        window.Beacon('init', '24e48c05-b068-4f15-8892-fcc0880802b4');
        //var cookies = document.cookie.split(';');
        //var sUsername = "";
        //var sUserrole = "";
        //var sPersonname = "";
        //var sEMailAddress = "";
        //for (var l = 0; l < cookies.length; l++) {
        //    if (cookies[l].indexOf("CPersonName") > -1) {
        //        sPersonname = cookies[l].split("=")[1];
        //    }
        //    else if (cookies[l].indexOf("EMailAddress") > -1) {
        //        sEMailAddress = cookies[l].split("=")[1];
        //    }
        //    else if (cookies[l].indexOf("CUserName") > -1) {
        //        sUsername = cookies[l].split("=")[1];
        //    }
        //    else if (cookies[l].indexOf("CUserRole") > -1) {
        //        sUserrole = cookies[l].split("=")[1];
        //    }

        //}
        //alert(sUsername + " - " + sPersonname + " - " + sEMailAddress + " - " + sUserrole);

        window.Beacon('identify', {
            name: '',
            login: '',
            email: '',
            usertype: '',
        });
        //if (sEMailAddress == "") {
        window.Beacon('prefill', { name: '', login: '', usertype: '',text:'', subject: '', email: '' })
        //}

</script>--%>

    <script id="ze-snippet" src="https://static.zdassets.com/ekr/snippet.js?key=115b8368-0fd6-4f41-bd95-8c6eddfd7ba7"> </script>

    <script type="text/javascript">
        window.zESettings = {
            webWidget: {
                launcher: {
                    label: {
                        'en-US': ' '
                    }
                }
            }
        };
    </script>

    <form id="frmLoginNew" runat="server" style="width: 100%; height: 900px; overflow-x: hidden; overflow-y: hidden;">
        <div class="container-fluid">
            <div >
                <div class="col-md-1 text-right">
                    <img id="imgleft" height="60px" alt="" width="100" />
                </div>
                <div class="col-md-8 align-bottom text-sm-left " style="margin-top: 22px;">
                    <label id="lblProduct" for="EHR" class="logoEHR">EHR</label>
                </div>
                <div id="#divproductname_version" class="col-md-3 text-sm-right" style="text-align: right">
                    <img id="imgright" height="60px" alt="" width="100" />
                </div>

                <div class="container">
                    <div>

                        <br />
                        <br />

                        <div class="row">
                            <%-- <div class="col-md-4 ">
                    <img src="Images/ACURUS Logo_Modified.png" width="200"   />
                </div>--%>

                            <div class="col-md-6 mobile-hide col-lg-6 col-sm-6">
                                <div  id="divpanelsucess" >
                                    <div id="dvpanelheading">
                                        <%--<img src="Images/rsz_note-icon.png" class="transparent" />--%>
                             Key Features
                                    </div>
                                    <div class="panel-body fixed-panel Editabletxtbox">
                                        <br />
                                        <ul id="ulFeatures">
                                        </ul>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-6 col-lg-6 col-sm-6">
                                <div class="card card-container">
                                    <div class="container">
                                        <div style="float: left;">
                                            <span  id="spnlogo" class="logoLogin" >LOGIN
                                            </span>
                                        </div>
                                    </div>
                                    <hr size="16">
                                    <form class="form-signin">
                                        <input type="text" id="txtUserName" runat="server" class="form-control Editabletxtbox" placeholder="UserName" autofocus="autofocus" maxlength="50" /><br />
                                        <%--<input type="text" id="txtUserName" class="form-control" placeholder="UserName" autofocus="autofocus" runat="server" />--%>
                                        <input type="password" id="txtPassword" class="form-control Editabletxtbox" placeholder="Password" runat="server" maxlength="50" /><br />
                                        <select id="ddlFacility" runat="server" class="form-control Editabletxtbox">
                                            <option value="0">Select Facility</option>
                                        </select>
                                        <div id="remember" class="checkbox">
                                            <%--<a href="" class="pagerLink">Forgot Password</a>--%>
                                            <%--<img src="Images/Support.png" id="imgSupport" class="support" style="padding-left:98%" title="Need Support?" />--%>
                                        </div>
                                        <div id="divsubmit">
                                            <button class="btn btn-lg btn-primary btn-block btn-signin btnclass htmlbtnstyle" runat="server" id="btnOk" onclick="return CheckMandatory();" onserverclick="btnOk_Click">Sign in</button>
                                            <%--<button class="button green bigrounded" id="btnOk" runat="server" style="font-size:small;" onclick="CheckMandatory();" onserverclick="btnOk_Click">Login</button>--%>
                                        </div>
                                    </form>
                                </div>
                            </div>


                        </div>
                    </div>
                    <br />

                    <div class="row">
                        <div class="col-md-4 mobile-hide col-lg-4 col-sm-4">
                            <div id="divpanelsucess1">
                                <div  id="dvpanelheading1" >
                                    <%--<img src="Images/System Messages.png" class="transparent" />--%>
                        System Messages
                                </div>
                                <div class="panel-body fixed-panel Editabletxtbox">
                                    <ul id="ulSystemMessages"  >
                                    </ul>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4 mobile-hide col-lg-4 col-sm-4">
                            <div id="divpanelsucess2">
                                <div id="dvpanelheading2">
                                    <%--<img src="Images/Contacts.png" />--%>
                        Contact Details
                                </div>
                                <div class="panel-body fixed-panel">
                                    <p id="pContactDetails" class="Editabletxtbox">
                                        <%-- <b>Address:</b><br />
                                    160 South Old Springs Road<br />
                                    Suite# 280<br />
                                    Anaheim Hills<br />
                                    CA 92808<br />
                                    USA.
                                    <br />
                                    <br />

                                    <b>Phone:</b> 714-221-6311<br />
                                    <br />

                                    <b>Fax:</b> 909-348-8194<br />
                                    <br />

                                    <b>Email:</b> capellasupport@acurussolutions.com--%>
                                    </p>
                                </div>
                            </div>
                        </div>


                        <div class="col-md-4 mobile-hide col-lg-4 col-sm-4">
                            <div id="divpanelsucess3">
                                <div id="dvpanelheading3">
                                    <%--<img src="Images/Knowledge center.png" />--%>
                            Knowledge Center
                                </div>
                                <div class="panel-body fixed-panel Editabletxtbox">
                                    <ul id="ulKnowledgecenter" >
                                    </ul>
                                </div>
                            </div>
                        </div>

                        <div id="dvSupportModal" class="modal fade">
                            <div class="modal-dialog">
                                <div class="modal-content">
                                    <div class="modal-header">

                                        <div class="row">
                                            <div class="col-xs-4">
                                                <%--<img class="modal-title" src="Images/Logo.png" style="padding-left: 5px" />--%>
                                            </div>
                                            <div class="col-xs-4">
                                                <h3>Need Support</h3>
                                            </div>
                                            <div class="col-xs-4">
                                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="modal-body">


                                        <div class="form-group">
                                            <div class="row">
                                                <span class="col-xs-3">Screen text :</span>
                                                <div class="col-xs-8">

                                                    <input type="text" class="form-control" id="txtScreentext" value="Login" disabled="disabled" />

                                                </div>

                                            </div>
                                            <br />
                                            <div class="row">
                                                <span class="col-xs-3">Date time :</span>
                                                <div class="col-xs-8">
                                                    <input type="text" class="form-control" id="txtDateTime" value="15-JUL-2015" disabled="disabled" />
                                                </div>
                                            </div>
                                            <br />
                                            <div class="row">
                                                <span class="col-xs-3">User Name <span style="color: red">*</span>:</span>
                                                <div class="col-xs-8">
                                                    <input type="text" class="form-control" id="inpUserName" placeholder="Please Enter UserName" />
                                                </div>
                                            </div>
                                            <br />
                                            <div class="row">
                                                <span class="col-xs-3">Email/Phone No :</span>
                                                <div class="col-xs-8">
                                                    <input type="text" class="form-control" id="iptxtEmail" placeholder="Email/Phone No" />
                                                </div>
                                            </div>
                                            <br />

                                            <div class="row">
                                                <span class="col-xs-3">Priority :</span>
                                                <div>
                                                    <div class="btn-group col-xs-8">

                                                        <select id="selPriority" class="form-control">
                                                            <option value="Select">Select a Priority</option>
                                                            <option value="High">High</option>
                                                            <option value="Medium">Medium</option>
                                                            <option value="Low">Low</option>
                                                        </select>

                                                    </div>
                                                </div>
                                            </div>
                                            <br />
                                            <div class="row">
                                                <span class="col-xs-3">Tell us more about the issue<span style="color: red">*</span> : </span>
                                                <div class="col-xs-8">
                                                    <textarea class="form-control custom-control" rows="4" style="resize: none" id="inputlg"></textarea>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                        <button type="button" class="btn btn-primary" id="btnsupportsubmit">Submit</button>
                                        <button type="button" class="btn btn-default" data-dismiss="modal" id="btncancel">Close</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>



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

                </div>
            </div>

            <%-- <div id='resultLoading' class='modal' style='text-align: center; display: none'>
       <br /><br /><br /><br />
        <center></center><br /><img src='Resources/wait.ico' title='[Please wait while the page is loading...]'alt='Loading...' /><br />

    </div>--%>
            <button id="hdnbtnLogin" runat="server" style="display: none;" onserverclick="hdnbtnLogin_Click">hdnLogin</button>
            <asp:HiddenField ID="hdnLocalTime" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnLocalDate" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnUniversaloffset" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnLocalDateAndTime" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnFollowsDayLightSavings" runat="server" Value="false" />
            <asp:HiddenField ID="hdnUserName" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnFacltyName" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnOkButton" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnVersion" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnProjectName" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnreportPath" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnLoginheader" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnVersionKey" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnServiceLink" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnPersonName" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnEvProjectName" runat="server" EnableViewState="false" />  <asp:HiddenField ID="hdnReportPathhttp" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnGroupId" runat="server" EnableViewState="false" />

            <asp:HiddenField ID="hdnroleLanding" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnRCopia_User_NameLanding" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnIs_RCopia_Notification_RequiredLanding" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnPhysician_Library_IDLanding" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnLanding_Screen_IDLanding" runat="server" Value="false" />
            <asp:HiddenField ID="hdnEMailAddress" runat="server" Value="false" />




            <asp:PlaceHolder ID="PlaceHolder1" runat="server">
                <link href="CSS/bootstrap.min3.1.1.css" rel="stylesheet" />
                <script src="JScripts/bootstrap.min3.1.1.max.js" type="text/javascript"></script>
                <script src="JScripts/jsLogin.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"]%>"></script>
                <script src="JScripts/jsErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"]%>"></script>
                <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>        
               <%-- <script src="JScripts/JsRestrictMultipleTab.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>--%>
            </asp:PlaceHolder>
        </div>


    </form>
    
</body>
</html>
