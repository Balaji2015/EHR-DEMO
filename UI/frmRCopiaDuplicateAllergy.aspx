<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmRCopiaDuplicateAllergie.aspx.cs" Inherits="Acurus.Capella.UI.frmRCopiaDuplicateAllergie" EnableEventValidation="false"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="aspx" %>
<%@ Register Src="~/UserControls/CustomDLCNew.ascx" TagName="DLC" TagPrefix="DLC" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <!--<script src="https://logrocket.acurussolutions.io/LogRocket.js" ; crossorigin="anonymous"></script><script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>-->
    <script>document.write("<script src='JScripts/jquery.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <script>document.write("<script src='JScripts/jQueryAngular.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <script>document.write("<script src='JScripts/jquery-ui.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <link href="CSS/jquery-ui.css" rel="stylesheet" />
    <link href="CSS/bootstrap.min.css" rel="stylesheet" />
    <script src="JScripts/bootstrap.min.js"></script>
    <link href="CSS/font-awesome.css" rel="stylesheet" type="text/css" />
    <!--<link href="CSS/style.css" rel="stylesheet" type="text/css" />-->
    <link href="CSS/CommonStyle.css?v=37.00" rel="stylesheet" type="text/css" />
    <!--CAP-897 Dynamically add Javscript reference to the HTML files-->
   
     <script>
         ["JSErrorMessage.js", "JSModalWindow.js", "JSRCopiaDuplicateAllergy.js"].forEach(script => document.write(`<script src="JScripts/${script}?version=${sessionStorage.getItem("ScriptVersion")}"><\/script>`));
     </script>

    <title></title>
    <style>
         @media screen and (min-width: 0px) and (max-width: 7250px) {

            .mobile-hide {
                display: none;
            }
        }

        @media screen and (min-height: 0px) and (max-height: 720px) {

            .mobile-hide {
                display: none;
            }
        }

        .panel {
            color: #808080;
            margin-top: 5px;
            margin-bottom: 5px;
            border: 1px solid;
            margin-left: 2px;
            margin-right: 2px;
        }

        .panel-heading {
            border-right: 1px solid !important;
            border-bottom-left-radius: 3px !important;
            border-top-right-radius: 0px !important;
            padding: 3px 15px;
            float: left !important;
            width: 2% !important;
            height: 100%;
        }

        .panel-default {
            border: 1px solid !important;
            border-color: black !important;
        }

        .table {
            margin-top: 1px;
            margin-bottom: 5px;
            border: 1px solid !important;
            margin-left: 2px;
            margin-right: 2px;
        }
        body {
            font-size: 11px !important;
        }

        .table > tbody > tr > td, .table > tbody > tr > th, .table > tfoot > tr > td, .table > tfoot > tr > th, .table > thead > tr > td, .table > thead > tr > th {
            padding: 2px !important;
        }

        .panel-body {
            padding: 0.5%;
            width: 96.9% !important;
            float: right !important;
        }

        #tag:hover {
            text-decoration: underline;
        }
        </style>
</head>
<body>
    <form id="frmRCopiaDuplicateAllergie" runat="server">
        <div id="AngularDiv" ng-app="Allergieapp" ng-controller="AllergieCtrl">
             <div style="margin-top: 1%;margin-bottom: 1%;margin-left: -0.5%;">
                <table id="AllergyDetails" style="width: 1375px;margin-left: 0.5%;">
                    <tr>
                        <td style="width:0px;font-size: 14px;text-align:center">Allergy
                        </td>
                        <td style="width:550px;">
                            <input type="text" style="height: 30px;width: 433px;border: 1px solid;" id="txtAllergy" ng-keypress="AllergySearch()" ng-keydown="AllergySearchDown()" ng-keyup="AllergySearchDown()" class="Editabletxtbox" placeholder="Type your Allergy here" autocomplete="off" /><!--ng-keyup="LoadCPTDescription()"-->

                            <img id="imgClearAllergyText" runat="server" src="Resources/Delete-Blue.png" alt="X" title="Click to clear the text field." ng-click="AllergyClear();" style="position: absolute; margin-left: 10px; cursor: pointer; width: 13px; height: 15px;margin-top: 7px;" />
                        </td>
                       
                        </tr>
                    </table>
                 </div>
            <div id="divAlergyPanel" style="padding:0px;height: 400px;">
             <table id="tblAllergie" class="table Gridbodystyle" style="width: 1375px;margin-left: 0.5%;">

                    <thead id="AllergieHead" class="Gridheaderstyle">
                        <tr style="width:100%" class="Gridheaderstyle">
                            <th style="width:5%;text-align:center">Select</th>
                            <th style="width:25%;text-align:center">Allergy Name</th>
                            <th style="width:25%;text-align:center">Reaction</th>
                            <th style="width:15%;text-align:center">Onset Date</th>
                            <th style="width:7%;text-align:center">Created By</th>
                            <th style="width:15%;text-align:center;">Created Date Time</th>
                            <th style="width:15%;text-align:center;">Status</th>
                        </tr>
                    </thead>
                    <tbody id="AllergieGrid" class="Editabletxtbox">
                        <tr  ng-repeat="a in AllergieList" style="width:100%;color:{{a.Colour}}">
                            <td style="width:5%;text-align:center;"><input type="checkbox" ng-click="CheckChange()" name="DeleteCheck" value="{{a.RcopiaId}}" /></td>
                            <td style="width:25%;text-align:center">{{a.AlergyName}}</td>
                            <td style="width:25%;text-align:center">{{a.Reaction}}</td>
                            <td style="width:15%;text-align:center">{{a.OnsetDate}}</td>
                            <td style="width:10%;text-align:center">{{a.Createdby}}</td>
                            <td style="width:7%;text-align:center">{{a.CreatedDate}}</td>
                            <td style="width:7%;text-align:center">{{a.Status}}</td>
                            <td style="display:none">{{a.RcopiaId}}</td>                           
                        </tr>
                    </tbody>
                </table>
                </div>
            <div id="divcheckbox" style="float:left;margin-left: 2%;">
                <input type="checkbox" id="ShowAll" ng-click="ShowActive()"  style="font-size: 14px; display:none;" value=""  />
                  <lable style="font-size: 14px;font-weight: bold;display:none;">Check for all status</lable>
                </div>
              <div id="divButtonsPanelClose" style="float:right;margin-right: 2%;">
                 
             <button type="button"  id="btnDelete" ng-click="DeleteClick()" class="btn aspredbutton aspresizedredbutton" style="width: 75px;" runat="server" disabled="disabled">Delete</button>
        </div>
            </div>
        <asp:HiddenField  id="hdnDeleteIdList" runat="server" EnableViewState="false"/>     
        <asp:HiddenField ID="HdnGranular" runat="server" EnableViewState="false" />
    </form>  


</body>
</html>