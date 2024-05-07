<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmPhysicianLibray.aspx.cs" Inherits="Acurus.Capella.UI.frmPhysicianLibray" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
        ::-webkit-scrollbar {
            width: 8px;
        }

        ::-webkit-scrollbar-track {
            background-color: #c3bfbf;
        }

        ::-webkit-scrollbar-thumb {
            background-color: #707070;
        }

            ::-webkit-scrollbar-thumb:hover {
                background-color: #3d3c3a;
            }

        fieldset.scheduler-border {
            border: 1px groove #ddd !important;
            padding: 0 1.4em 1.4em 1.4em !important;
            margin: 0 0 1.5em 0 !important;
            -webkit-box-shadow: 0px 0px 0px 0px #000;
            box-shadow: 0px 0px 0px 0px #000;
        }
        .reset {
    all: revert;
        }
        input:focus::-webkit-input-placeholder {
             opacity: 0;
        }
        legend {
            font-size: 13px !important;
            font-weight: 700 !important;
        }
        /*#tblPhysicianDetails.element.style {
    width: 781px;
    data-item-original-width:"779"
}*/
    </style>
    <link href="CSS/style.css" rel="stylesheet" type="text/css" />
    <link href="CSS/font-awesome.css" rel="stylesheet" type="text/css" />
      <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <link href="CSS/bootstrap.min.css" rel="stylesheet" />
    <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
    <script src="JScripts/jquery-ui.min1.10.2.js" type="text/javascript"></script>
    <script src="JScripts/bootstrap.min.js" type="text/javascript"></script>
</head>
<body onload=" {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}">
    <form id="tab" runat="server" style="font-size: 8px;">
        <div id="divMain" style="height: 275px !important;">
            <%--<div class="panel-head Editabletxtbox">Contact Details</div>--%>
            <div class="panel-body" id="divContactDetails" style="height:100%;">

                <table style="width: 100%;" class="Editabletxtbox">
                    <tr>
                        <td style="width: 15%; padding-bottom: 0.5%"><span class="MandLabelstyle">Category*</span></td>
                        <td style="width: 35%; padding-bottom: 0.5%">
                            <select class="Editabletxtbox" name="Category" id="ddlCategory" style="width: 96%" onchange="ddlCategory_Change()"></select></td>
                        <td style="width: 15%; padding-bottom: 0.5%">
                            <label class="Editabletxtbox" style="display: none;">Physician Type</label></td>
                        <td style="width: 35%; padding-bottom: 0.5%">
                            <select class="Editabletxtbox" name="PhysicianType" id="ddlPhysicianType" style="width: 96%; display:none;">
                                <option value=""></option>
                                <option value="RENDERING">RENDERING</option>
                                <option value="READING">READING</option>
                            </select></td>
                    </tr>
                    <tr>
                        <td style="width: 15%; padding-bottom: 0.5%">
                            <label class="Editabletxtbox">Prefix.</label>
                            <select name="Prefix" id="ddlPrefix" class="Editabletxtbox" style="width: 45%">
                                <option value=""></option>
                                <option value="Dr">Dr</option>
                                <option value="Mr">Mr</option>
                                <option value="Mrs">Mrs</option>
                            </select>
                        </td>
                        <!--<td style="width: 30%; padding-bottom: 0.5%"></td>-->
                        <td style="width: 35%; padding-bottom: 0.5%">
                            <span id="lblLastName" class="MandLabelstyle">Last Name*</span>
                            <input type="text" id="txtLastName" maxlength="35" style="width: 45%" value="" onkeypress="AvoidSpecailCharacter(event)" class="Editabletxtbox"/>
                            <label class="Editabletxtbox">MI</label>
                            <input type="text" id="txtMI" maxlength="25" style="width: 16%" value="" class="Editabletxtbox" onkeypress="AvoidSpecailCharacter(event)"/>
                        </td>
                        <!--<td style="width: 30%; padding-bottom: 0.5%"></td>-->
                        <td style="width: 15%; padding-bottom: 0.5%">
                            <span id="lblFirstName" class="MandLabelstyle">First Name*</span>
                        </td>
                        <!--<td style="width: 30%; padding-bottom: 0.5%"></td>-->
                        <td style="width: 35%; padding-bottom: 0.5%">
                            <input type="text" id="txtFirstName" maxlength="50" style="width: 58%" value="" onkeypress="AvoidSpecailCharacter(event)" class="Editabletxtbox"/>
                            <label id="lblSuffix" class="Editabletxtbox">Suffix</label>
                            <input type="text" id="txtSuffix" onkeypress="AvoidSpecailCharacter(event)" style="width: 20%" value="" class="Editabletxtbox"/>
                        </td>
                    </tr>
                    <tr>
                        <!--<td style="width: 15%; padding-bottom: 0.5%"><label>Facility</label></td>
                        <td style="width: 35%; padding-bottom: 0.5%"><select name="Facility" id="ddlFacility" style="width: 96%" class="input-xlarge"></select></td>-->
                        <!--<td style="width: 15%; padding-bottom: 0.5%"><label>Specialty*</label></td>
                        <td style="width: 35%; padding-bottom: 0.5%"><select name="Specialty" id="ddlSpecialty" style="width: 96%" class="input-xlarge"><option value=""></option><option value="Diagnosis">Diagnosis</option></select></td>-->
                        <td style="width: 15%; padding-bottom: 0.5%">
                            <span id="lblNPI"  class="Editabletxtbox">NPI</span></td>
                        <td style="width: 35%; padding-bottom: 0.5%">
                            <input type="text" id="txtNPI" onkeypress="if(!isNumberKey(event)) return false;" maxlength="10" style="width: 96%" value="" class="Editabletxtbox"/></td>
                        <td style="width: 15%; padding-bottom: 0.5%">
                            <label id="lblCompany" class="Editabletxtbox">Company</label></td>
                        <td style="width: 35%; padding-bottom: 0.5%">
                            <input type="text" id="txtCompany" onkeypress="AvoidSpecailCharacter(event)" style="width: 96%" value="" class="Editabletxtbox"/></td>
                    </tr>
                    <tr>
                        <td style="width: 15%; padding-bottom: 0.5%">
                            <label id="lblAddressLine1" class="Editabletxtbox">Address Line1</label></td>
                        <td style="width: 35%; padding-bottom: 0.5%">
                            <input type="text" id="txtAddressLine1" style="width: 96%" maxlength="100" value="" class="Editabletxtbox"/></td>
                        <td style="width: 15%; padding-bottom: 0.5%">
                            <label id="lblAddressLine2" class="Editabletxtbox">Address Line2</label></td>
                        <td style="width: 35%; padding-bottom: 0.5%">
                            <input type="text" id="txtAddressLine2" style="width: 96%" maxlength="100"  value="" class="Editabletxtbox"/></td>
                    </tr>
                    <tr>
                        <td style="width: 15%; padding-bottom: 0.5%">
                            <label id="lblCity" class="Editabletxtbox">City</label></td>
                        <td style="width: 35%; padding-bottom: 0.5%">
                            <input type="text" id="txtCity" onkeypress="AvoidSpecailCharacter(event)" style="width: 96%" value="" maxlength="35" class="Editabletxtbox"/></td>
                        <td style="width: 15%; padding-bottom: 0.5%">
                            <label id="lblState" class="Editabletxtbox">State</label></td>
                        <td style="width: 35%; padding-bottom: 0.5%">
                            <input type="text" id="txtState" onkeypress="AllowCharacter(event)" maxlength="2" style="width: 96%" value="" class="Editabletxtbox"/></td>
                    </tr>
                    <tr>
                        <td style="width: 15%; padding-bottom: 0.5%">
                            <label class="Editabletxtbox">Zip</label></td>
                        <td style="width: 35%; padding-bottom: 0.5%">
                            <input type="text" id="txtZip" placeholder="_____-____" maxlength="11" style="width: 96%" value="" class="Editabletxtbox"/></td>
                        <td style="width: 15%; padding-bottom: 0.5%">
                            <label class="Editabletxtbox">Phone#</label></td>
                        <td style="width: 35%; padding-bottom: 0.5%">
                            <input type="text" id="txtPhone" style="width: 96%" maxlength="20" value="" class="Editabletxtbox"/></td>
                    </tr>
                    <tr>
                        <td style="width: 15%; padding-bottom: 0.5%">
                            <label class="Editabletxtbox" id="lblFax">Fax#</label></td>
                        <td style="width: 35%; padding-bottom: 0.5%">
                            <input type="text" id="txtFax" style="width: 96%" value="" class="Editabletxtbox"/></td>
                        <td style="width: 15%; padding-bottom: 0.5%">
                            <label id="lblEmail" class="Editabletxtbox">Email</label></td>
                        <td style="width: 35%; padding-bottom: 0.5%">
                            <input type="text" id="txtEmail" onkeypress="AllowMailCharacter(event)" style="width: 96%" maxlength="100" value="" class="Editabletxtbox"/></td>
                    </tr>
                    <tr>
                        <td style="width: 50%; padding-bottom: 0.5%;" colspan="2">
                            <!--<div class="panel-head Editabletxtbox" style=" width: 98%;">Specialities</div>-->
                            <table style="width: 97%;">
                                <tr>
                                    <td>
                                        <fieldset class="scheduler-border" style="display:none;">
                                            <legend id="lgndSpecialities" class="legendschedulerborderPhysician">Specialties</legend>
                                            <div id="divSpecialities" class="Editabletxtbox">
                                            </div>
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>

                        </td>
                        <td style="width: 50%; padding-bottom: 0.5%" colspan="2">
                            <table style="width: 97%;">
                                <tr>
                                    <td>
                                        <fieldset class="scheduler-border" style="display:none;">
                                            <legend id="lgndFacilityName" class="legendschedulerborderPhysician">Facility Name</legend>
                                            <div id="divFacility" class="Editabletxtbox">
                                            </div>
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 15%; padding-bottom: 0.5%"></td>
                        <td style="width: 35%; padding-bottom: 0.5%"></td>
                        <td style="width: 15%; padding-bottom: 0.5%"></td>
                        <td style="width: 35%; padding-bottom: 0.5%; text-align: right;">
                            <button id="btnSave" style="margin-right: 16px; margin-top: 12px; width: 35%;" class="aspgreenbutton" onclick="AddProvider(); return false;">Add</button>
                            <button id="btnClearAll" style="margin-right: 16px; margin-top: 12px; width: 35%;" onclick="ClearAll(); return false;" class="aspredbutton">Clear All</button></td>
                    </tr>
                </table>
            </div>
            
            <div id="divSearchParameters" style="width: 783px;">  
                <fieldset class="reset">
                    <legend class="reset">Search Parameters</legend>
                    <table>
                        <tbody>
                         <tr>
                         <td style="padding:8px;">
                       <label class="Editabletxtbox">Category</label>
                        <select id="cboCategory" name="Category" onchange="SearchByTextandCategory()" class="Editabletxtbox" style="width: 245px;margin-left:5px;" >
                            <option value=""></option>
                        </select>
                         </td>
                        <td style="padding:8px;">
                        <label class="Editabletxtbox" style="margin-left:30px;">Search</label>
                         <input type="text" id="txtSearch" onkeyup="SearchByTextandCategory()" placeholder="Search by Name or NPI" class="Editabletxtbox" style="width:260px;margin-left:5px;"/>  
                <img id="imgClearProviderText" runat="server" src="Resources/Delete-Blue.png" alt="X" title="Click to clear the text field." onclick="ClearSearch();" style="position: absolute; margin-left: 10px; top: 312px !important; cursor: pointer; width: 12px; height: 12px;" />
            </td>
           </tr>
        </tbody>
            </table>
            </fieldset>
            
            </div>
            <div id="divgrd" style="overflow-y: auto; height:100%; margin-top: 20px;">
                <table id="tblPhysicianDetails" class='table table-bordered' style="width:781px;">
                    <thead class="Gridheaderstyle" style="width:100%;">
                       <%-- <tr>
                            <th style="width: 5%;">Edit</th>
                            <th style="width: 15%;">Category</th>
                            <th style="width: 15%;">Name</th>
                            <th style="width: 28%; display:none;">Specialty</th>
                            <th style="width: 10%;">NPI</th>
                            <th style="width: 25%; display:none;">Facility</th>
                            <!--<th style="display:none;">Physician Library ID</th> class="table table-sm table-striped table-bordered"-->
                        </tr>--%>
                         <tr>
                            <th style="width: 5%;text-align: center;">Edit</th>
                            <th style="width: 16%;text-align: center;">Category</th>
                            <th style="width: 21%;text-align: center;">Name</th>
                            <th style="width: 28%;text-align: center; display:none;">Specialty</th>
                            <th style="width: 11.5%;text-align: center;">NPI</th>
                            <th style="width: 25%;text-align: center; display:none;">Facility</th>
                        </tr>
                    </thead>
                    <tbody class="Editabletxtbox" style="width:100%;"></tbody>
                </table>
            </div>
        </div>
        <input type="hidden" style="display: none;" id="hdnPhysicianId" value="" />
         <button type="button" id="btnClose" style="display: none;" class="btn btn-default" onclick="closewindow();" data-dismiss="modal"></button>
        <script type="text/javascript">
            var JSFiles = ["JScripts/JSErrorMessage.js", "JScripts/JSMask.Min.js", "JScripts/JSPhysicianLibrary.js", "JScripts/JSAvoidRightClick.js", "JScripts/JSCustomDLC.js"];
            for (var i = 0; i < JSFiles.length; i++) {
                var JSLink = JSFiles[i] + "?version=" + sessionStorage.getItem("ScriptVersion");
                var JSElement = document.createElement('script');
                JSElement.src = JSLink;
                document.getElementsByTagName('body')[0].appendChild(JSElement);
            }
        </script>
    </form>
</body>
</html>
