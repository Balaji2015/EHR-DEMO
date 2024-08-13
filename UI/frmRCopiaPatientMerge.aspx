<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmRCopiaPatientMerge.aspx.cs" Inherits="Acurus.Capella.UI.frmRCopiaPatientMerge" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <link href="CSS/jquery-ui.css" rel="stylesheet" />
    <link href="CSS/bootstrap.min.css" rel="stylesheet" />
    <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <link href="CSS/ElementStyles.css" rel="stylesheet" type="text/css" />
    <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
    <script src="JScripts/bootstrap.min.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/jquery-ui.min1.11.4.js" type="text/javascript"></script>
    <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSRCopiaPatientMerge.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

    <style>
        .kepp-account {
            margin-top: 30px;
            display: flex;
        }

        .buttons {
            display: flex;
            justify-content: end;
            margin-top: 30px;
            margin-bottom: 50px;
            padding-right: 20px;
            gap: 15px;
        }

        .button-duplicate {
            border-radius: 1px;
            border: 2px solid #07077a;
        }

        .ui-autocomplete {
            -webkit-margin-before: 3px !important;
            max-height: 150px;
            overflow-y: auto;
        }

        .ui-state-focus {
            color: #808080;
            background-color: #bbe2f1 !important;
            outline: none;
            border: 0px !important;
        }

        .inactive, .inactive.ui-state-focus {
            background-color: #aaa !important;
        }

        .deceased, .deceased.ui-state-focus {
            background-color: black !important;
            color: white !important;
        }

        .disabled, .disabled.ui-state-focus {
            background-color: white !important;
        }

        .ui-dialog-titlebar-close {
            display: none;
        }
    </style>
    <title></title>
</head>
<body>
    <button type="button" class="close" data-dismiss="modal" aria-label="Close" style="font-size: 25px; color: black; opacity: 1;" onclick="patientMergeClose();">
        <span aria-hidden="true" style="color: black;">×</span>
    </button>
    <div style="padding: 10px;">
        <div>
            <label id="lblcount" mand="Yes" class="Editabletxtboxcc Editabletxtbox" style="color: #0d6de9; font-weight: bold;">Note: Please enter ONLY ONE MERGE account to merge records at given time</label>
        </div>
        <div style="width: 100%;">
            <iframe runat="server" style="width: 100%; height: 500px; border: none" id="radBrowser"></iframe>
        </div>
        <div class="panel panelborderbox" style="height: 225px; width: 100%; float: left; clear: none;">
            <div class="divgroupstyle" style="height: 25px!important">Download Merged Data</div>
            <div id="fileThumbs" style="width: 100%; overflow-y: auto;">
                <div style="padding-left: 20px;">
                    <div class="kepp-account">
                        <span mand="Yes" class="MandLabelstyle" style="width: 170px;">Select Keep Account<span class="manredforstar">*</span></span>
                        <input type="text" id="txtKeepAccount" class="LabelStyleBold" data-human-id="0" data-human-details="" placeholder="Type minimum 3 characters of Last or First or Middle name or DOB as dd-mmm-yyyy or Acc# or Ext.Acc # or MR # and follow it by a space.." style="width: 100%;" />
                        <img id="imgClearKeepAccount" src="Resources/Delete-Blue.png" alt="X" title="Click to clear the text field." style="position: sticky; right: 20px; top: 33px; cursor: pointer; width: 20px; height: 20px;" />
                    </div>
                    <div class="kepp-account">
                        <span mand="Yes" class="MandLabelstyle" style="width: 170px;">Select Merge Account<span class="manredforstar">*</span></span>
                        <input type="text" id="txtMergeAccount" class="LabelStyleBold" data-human-id="0" data-human-details="" placeholder="Type minimum 3 characters of Last or First or Middle name or DOB as dd-mmm-yyyy or Acc# or Ext.Acc # or MR # and follow it by a space.." style="width: 100%;" />
                        <img id="imgCleartxtMergeAccount" src="Resources/Delete-Blue.png" alt="X" title="Click to clear the text field." style="position: sticky; right: 20px; top: 33px; cursor: pointer; width: 20px; height: 20px;" />
                    </div>
                    <div class="buttons">
                        <%--<button style="width: 12%" class="btn btn-primary button-duplicate" type="button">Show Duplicates</button>--%>
                        <button style="width: 12%" class="btn btn-success" type="button" onclick="download();" id="btnDownload" disabled="disabled">Download</button>
                    </div>
                </div>
            </div>
        </div>
        <%--<div>
            <button id="btnCloseModal" class="btn btn-danger" type="button" onclick="patientMergeClose();" style="float: right;">Close</button>
        </div>--%>
    </div>
    <input type="hidden" id="hdnLocalTime" runat="server" />
</body>
</html>
