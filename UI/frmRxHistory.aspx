<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmRxHistory.aspx.cs" Inherits="Acurus.Capella.UI.RxHistory" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<%--<head runat="server">
    <title></title>
    <style>
        #tblrx {
            height:100%;
            width: 100%;
        }

        #row1 {
            height: 250px;
            width: 100%;
        }

        .panelleft{
            float:left;
            width:27%;
            height:100%;
        }
        .panelright{
            float:right;
             width:73%;
            height:100%;
        }
    </style>
</head>
<body onload=" {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}">
    <form id="form1" runat="server" style="height:567px;width:100%;">
        <div>
                <asp:Panel ID="PanelRx" runat="server" GroupingText="Rx Details" Width="100%" Height="340px">
                       <asp:Panel ID="panelleft"  runat="server" CssClass="panelleft">
                            <asp:ListBox ID="MedDtls" runat="server" Style="width: 100%; height:94%"></asp:ListBox>
                        </asp:Panel>
                       <asp:Panel ID="panelright" runat="server" CssClass="panelright">
                        <table id="table1" style="height:100%;width:95%">
                            <tr>
                                <td>
                                    <asp:Label ID="DrugName" Text="Drug Name" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDrugName" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="RouteOfAdmininstration" runat="server" Text="Route of Administration"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlRouteOfAdmininstration" runat="server" ></asp:DropDownList>
                                </td>
                            </tr>
                             <tr>
                                <td>
                                    <asp:Label ID="Strength" Text="Strength" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtStrength" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="Frequency" runat="server" Text="Frequency"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlFrequency" runat="server" ></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="FromDate" Text="From Date" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFromDate" runat="server" ></asp:TextBox>
                                </td>
                            </tr>
                              <tr>
                                  <td>
                                      <asp:CheckBox ID="chkDate" runat="server" />
                                  </td>
                                <td>
                                    <asp:Label ID="CurrentDate" Text="Current / To Date" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCurrentDate" runat="server" ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                <asp:Label ID="Notes" Text="Notes" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:Panel ID="pnlDLC" runat="server" Height="100%" Width="100%" BackColor="White"
                                            Font-Size="Small" Font-Bold="false">
                                            <DLC:DLC ID="DLC" runat="server" TextboxHeight="80px" TextboxWidth="500px" Enable="True"
                                                Value="Medication Notes" />
                                        </asp:Panel>
                                </td>
                            </tr>
                        </table>
                   </asp:Panel>
                    </asp:Panel>
                        <asp:Panel ID="rxhstry" GroupingText="Rx History" runat="server" Height="220px">
                            <asp:DataGrid ID="rxHistorytble" runat="server" Width="100%" Height="98%">
                                <Columns>
                                    <asp:EditCommandColumn  HeaderText="Edit" HeaderImageUrl="~/Resources/edit.gif" ></asp:EditCommandColumn>
                                    <asp:ButtonColumn  HeaderText="Del" HeaderImageUrl="~/Resources/close_small_pressed.png" ></asp:ButtonColumn>
                                    <asp:BoundColumn DataField="DrugName" HeaderText="Drug Name"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="Strength" HeaderText="Strength"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="Frequency" HeaderText="Frequency"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="FromDate" HeaderText="From Date"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="ToDate" HeaderText="To Date"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="Routeofadministration" HeaderText="Route of Administration"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="Notes" HeaderText="Notes"></asp:BoundColumn>
                                </Columns>
                            </asp:DataGrid>
                        </asp:Panel>
        </div>
    </form>
</body>--%>
<head>
    <title>Medication</title>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="CSS/bootstrap.min.css" rel="Stylesheet" />
    <link href="CSS/font-awesome.css" rel="stylesheet" type="text/css" />
    <link href="CSS/jquery-ui.css" rel="Stylesheet" />
      <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />



    <style>
        select {
  text-align: initial;
  text-align-last: inherit;
  /* webkit*/
}
option {
  text-align: left;
  /* reset to left*/
}
         .highlight {
            background-color: #b2def6;
        }
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

        input::-webkit-calendar-picker-indicator {
            display: none;
        }
        .panel {
            margin-bottom: 5px !important;
        }
        #tblMedicationDetails th, #tblMedicationDetails td {
            border-top: none !important;
            border-left: none !important;
        }

        .panel-heading {
            padding: 3px 7px;
            background-color: #BFDBFF !important;
            color: #000000 !important;
        }

        .panel-info {
            border: 1px solid #808080;
        }
        .txtheight
        {
            height:35px !important;
        }
        /*table#tRxMed th {
            text-align: center;
            background-color: #BFDBFF !important;
            border-color: #808080;
            color: #4E4E4E;
            vertical-align: middle;
            border-top: 1px solid #808080 !important;
        }*/

        tbody#tbodyRxMed td, table#tRxMed td {
            text-align: center;
            border: 1px solid #808080;
            color: #4E4E4E;
            padding: 6px 2px 3px 2px;
        }
        table#tRxMed tbody {
    height: 250px;       /* Just for the demo          */
    overflow-y: auto;    /* Trigger vertical scroll    */
    overflow-x: hidden;  /* Hide the horizontal scroll */
}
        /*table#tblRxHistory tr{
              width: 100%;
              display:inline-table;
            table-layout: fixed;
        }*/

        /*#tbodyRxMed {
            height: 190px;
            position: absolute;
        }*/
        #pnlDrugs .panel-body {
            /*padding: 5px !important;*/
            border: 1px solid #808080;
            font-size: smaller !important;
        }

        #pnlDrugs p {
            padding: 5px;
            margin-bottom: 0px;
            border-bottom: 1px solid #808080;
            background-color: #ffffff;
        }

            #pnlDrugs p:hover {
                /*border: 1px solid #fbe086;*/
                background-color: #fdfef6;
            }

        span, p, a:hover {
            cursor: pointer;
        }

        li:hover {
            background-color: #DEF;
        }

        /*.panel {
            margin-bottom: 12px !important;
        }*/
        /*body {
            font-size:13px!important;
        }*/

        .custom-combobox {
            position: relative;
            display: inline-block;
            width: 100%;
        }
        table{
            margin-bottom:0px !important;
        }
        .custom-combobox-toggle {
            padding: 8px 6px !important;
            display: table-cell !important;
            width: 30px;
            font-size: 10px !important;
        }

        .custom-combobox-input {
            border: 1px solid #ccc !important;
            border-radius: 3px;
            margin: 1px 0px;
            height: 29px !important;
            padding: 5px 10px;
        }

        .ui-autocomplete {
            max-height: 300px;
            overflow-y: auto;
            overflow-x: hidden;
        }
      .custom-combobox
      {
          width:41% !important;
      }
      .form-control{
          display: inline-block !important;
      }
      /*.custom-combobox-strength
      {
          width:100% !important;
      }*/
        .commonstyle
        {
            font-size: 13px; line-height: 1.42857143; color: #555; background-color: #fff; background-image: none; border: 1px solid #ccc; border-radius: 4px; height: 30px;
        }
    </style>
    <script src="JScripts/jquery-1.11.3.min.js"></script>
    <script src="JScripts/jquery-ui.min1.10.2.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>"></script>
    <script src="JScripts/bootstrap.min.js"></script>
      <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
</head>
<body onload="loadRx_History()">
    <form id="frmRxMed" runat="server">
        <div class="panel panel-info" id="mainContainer">
            <div class="panel-head">Rx Details</div>
            <div style="border: 1px solid #808080;
           padding-bottom:0px;">
                <table style="width: 100%;  padding-bottom:0px;">
                    <tr>
                        <%--<td style="width: 30%; vertical-align: top; padding-right: 2%;">
                            <div class="panel panel-info" id="pnlDrugs">
                                <div class="panel-heading" style="display: none">Drugs<span class="glyphicon glyphicon-log-in" style="float: right; font-weight: bold; background-color: #BFDBFF !important" onclick="OpenFrequentUsedDrugs('Drug Name');" title="Click here to open Frequently Used Drugs"></span></div>
                                <div class="panel-body" style="overflow-y: auto; height: 245px;" id="DrugPanel" runat="server">
                                </div>
                            </div>
                        </td>--%>
                        <td style="width: 70%;">
                            <table class="table table-condensed" id="tblMedicationDetails">
                                <tbody>
                                    <tr>
                                        <td style="width:10%;"><span class="MandLabelstyle" >Drug Name</span><span class="manredforstar">*</span></td>
                                        <td style="width: 35%;">
                                            <input type="text" id="txtDrugName" class="form-control ui-autocomplete-input Editabletxtbox" style="width: 97%;" onchange="EnableSave();" maxlength="255" autocomplete="off"/><span role="status" aria-live="polite" class="ui-helper-hidden-accessible"></span>
                                        </td>
                                        <td class="spanstyle" style="width:10%">Strength</td>
                                        <td style="width: 35%;">
                                            <select id="selStrength"dir="ltr" class="Editabletxtbox" style="width: 88%; display: none;" onselect="EnableSave();">
                                            </select>
                                            
                                       <%-- <div style="display: inline-block;width: 10%;vertical-align: top;padding-top: 1%;" class="Editabletxtbox"> Dose</div>--%>
                                               
                                      <span class="spanstyle">Dose</span>
                                            <input type="text" id="txtQuantity" style="width: 17%;" class="commonstyle" onchange="EnableSave();" onkeypress="return isNumberKey(event);" maxlength="7" onblur="CalculateTotal();" />
                                            <select id="selquantity"  dir="ltr" style="width: 25%;" class=" form-control Editabletxtbox" onchange="setTolquantityunit();" name="D1">
                                           
                                              </select>
                                                            </td>                     

                                    </tr>
                                    <tr>
                                        <td class="Editabletxtbox" style="width:10%">Days Supply</td>
                                        <td style="width: 16%;">
                                          
                                         

                                            <select id="selDayssupply" style="width: 15%; " dir="ltr" class="commonstyle" onchange="CalculateTotal();">
                                            </select>

                                            <span class="Editabletxtbox">Refills</span>
                                            <select id="selrefill" dir="ltr" style="width: 15%;" class="commonstyle">
                                                </select>
                                            <span class="Editabletxtbox">Quantity</span>
                                             <input type="text" id="txttotalQuantity" style="width: 10%;" class="commonstyle"  onkeypress="return isNumberKey(event);" onchange="EnableSave();"/>
                                            <select id="selTotQuantity" dir="ltr" style="width: 29%;" class="Editabletxtbox form-control">
                                            </select>
                                            
                                        </td>
                                        <td class="Editabletxtbox" style="width:10%">Route of Administration
                                        </td>
                                        <td style="width: 35%;">
                                            <select id="selRouteOAdministration"  dir="ltr" class="Editabletxtbox" style="width: 88%; display: none;">
                                            </select>
                                            <%--<span class="Editabletxtbox">Direction</span>--%>
                                            
                                        <div style="display: inline-block;width:15%;vertical-align: top;padding-top: 2%;" class="Editabletxtbox"> Direction</div>
                                             <select id="selDirection" style="width: 38%; padding-top: 19px;" class="form-control Editabletxtbox">
                                            </select>
                                        </td>

                                    </tr>
                                    <tr>

                                        <td class="Editabletxtbox" style="width:10%">Frequency</td>
                                        <td style="width: 20%;">
                                            <select id="selfrequency"  dir="ltr" style="width: 20%; width: 55%;" class=" form-control Editabletxtbox" onchange="CalculateTotal();">
                                            </select>
                                            
                                           <span class="Editabletxtbox">Last Filled</span>
                                             <input type="text" id="txtlastfilled" style="width: 26%;"  class="datecontrol commonstyle" placeholder="yyyy-MMM-dd"/> 

                                            </td>

                                        <td class="Editabletxtbox" style="width:10%">Pharm.Directions</td>
                                        <td style="width: 35%;">
                                           
                                           <div style="display: inline-block;width: 32%;vertical-align:  inherit" class="Editabletxtbox">
                                                                       
                                             <select id="selPharmacist"  dir="ltr" style="width: 100%;height:30px!important" class="Editabletxtbox" onchange="settooltip()">
                                            </select></div>

                                        <div style="display: inline-block;width: 21%;vertical-align: top;padding-top: 1%;" class="Editabletxtbox"> Pharm.Notes</div>
                                         <div style="display: inline-block;width: 44%;"><textarea onchange="EnableSave()" onkeyup="EnableSave()" name="Pharmacist Notes" class="actcmpt form-control txtheight" textmode="MultiLine"
                                            id="txtNotesPhar" style="resize: none; width: 79%; position: static; font-family: Microsoft Sans Serif; font-size: 8.5pt; display: initial; vertical-align:initial;"></textarea>
                                        <div id="dvnotesPhae" class="col-6-btns" style="width: 35px; float: right !important; margin-top: 7px; text-align: right;">
                                            <a class=" fa fa-plus" style="margin-left: -5px; margin-right: 1  background: #6DABF7; color: #fff; font-size: 12px; border-radius: 2px; width: 23px; margin-top: -8px"
                                                id="pbnotesPhar" align="centre" font-bold="false" title="Drop down" onclick="callweb(this, 'Pharmacist Notes', 'txtNotesPhar');"></a>
                                        </div></div>
                                        </td>
                                    </tr>

                                    <tr>

                                        <td class="Editabletxtbox" style="width:10%">From Date</td>
                                        <td style="width: 15%;">
                                          
                                            <input type="text" id="dtpFromDate" class="datecontrol commonstyle" placeholder="yyyy-MMM-dd" style="width: 40%;"/>
                                           &nbsp;&nbsp;&nbsp; <span class="tdToDate"> To Date
                                            </span>
                                           <%--  <input type="checkbox" id="chkCurntDt" onchange="ChkCurrent();"/>--%>
                                           
                                            <input type="text" id="dtpToDate" placeholder="yyyy-MMM-dd" class="datecontrol commonstyle" style="width: 40%;" />

                                            
                                        </td>

                                        <td class="Editabletxtbox" style="width:10%">
                                          Directions to Patient
                                            </td>
                                     <td style="width: 30%;">
                                          
                                            <textarea onchange="EnableSave()" onkeyup="EnableSave()" name="Directions Patient" class="actcmpt form-control txtheight Editabletxtbox" textmode="MultiLine"
                                            id="txtAdditionalNotes" style="resize: none; width: 90%; position: static; display: initial;" ></textarea>
                                        <div id="dvaddnotes" class="col-6-btns" style="width: 30px; float: right !important; margin-top: 7px; text-align: right;">
                                            <a class=" fa fa-plus" style="margin-left: -10px; margin-right: 15px; text-decoration: none; padding: 4px 4px 4px 4px !important;  border-radius: 2px;"
                                                id="pbaddnotes" align="centre" font-bold="false" title="Drop down" onclick="callweb(this, 'Directions Patient', 'txtAdditionalNotes');"></a>
                                        </div> 
                                        
                                        
                                       
                                          
                                        </td>
                                    </tr>


                                    <tr>
                                        <td class="Editabletxtbox" style="width:10%">Notes </td>
                                        <td>
                                            <textarea onchange="EnableSave()" onkeyup="EnableSave()" name="Medication Notes" class="actcmpt form-control txtheight Editabletxtbox" textmode="MultiLine"
                                            id="txtNotes" style="resize: none; width: 90%; position: static; display: initial;" ></textarea>
                                        <div id="dvnotes" class="col-6-btns" style="width: 35px; float: right !important; margin-top: 7px; text-align: right;">
                                            <a class=" fa fa-plus" style="margin-left: -5px; margin-right: 15px; text-decoration: none; padding: 4px 4px 4px 4px !important; border-radius: 2px;"
                                                id="pbnotes" align="centre" font-bold="false" title="Drop down" onclick="callweb(this, 'Medication Notes', 'txtNotes');"></a>
                                        </div>
                                        </td>
                                         <td style="width: 10%;text-align:right" colspan="3">
                                        <label style="border:1px solid #ccc;background-color:#D4FBA8;height:20px;width:42px;border-radius:3px;vertical-align: bottom;"></label><span style="vertical-align: -webkit-baseline-middle;" class="Editabletxtbox">  On-going Medications</span>
                                        <label style="border:1px solid #ccc;background-color:#e0dede;height:20px;width:42px;border-radius:3px;margin-left:12px;vertical-align: bottom;" ></label><span style="vertical-align: -webkit-baseline-middle;" class="Editabletxtbox">  Stopped Medications</span>
                                         <input type="button" class="btn aspresizedbluebutton" value="Reconcile" id="btnReconcile" onclick="OpenReconcile();" />
                                            <input type="button" class="btn aspresizedgreenbutton btn-sm disabled" value="Add" id="btnAdd" onclick="Save();"/>
                                            <input type="button" class="btn aspresizedredbutton btn-sm" value="Clear All" id="btnClear" onclick="ClearValues();"/>
                                        </td>
                                       
                                    </tr>
                                   
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <%-- <div class="panel panel-info" style="height: 235px; width: 100%;">
            <div class="panel-heading">Rx History</div>
            <div class="panel-body">--%>
     <%--   <div id="divHeader">
            <table id="tblRxHistory" class="table table-bordered table-condensed" style="height: 245px; width: 99%">
                <thead>
                    <tr>
                        <th id="thEdit" style="width: 4%;">Edit</th>
                        <th id="thDelete" style="width: 4%;">Del.</th>
                        <th id="thDrugName" style="width: 15%;">Drug Name</th>
                        <th id="thStrength" style="width: 14%;">Strength</th>
                        <th id="thFrequency" style="width: 13%;">Frequency</th>
                        <th id="thFrmDt" style="width: 10%;">From Date</th>
                        <th id="thToDt" style="width: 10%;">To Date</th>
                        <th id="thRoa" style="width: 15%;">Route of Administration</th>
                        <th id="thNotes" style="width: 15%;">Notes</th>
                        <th id="thID" style="width: 1%; display: none"></th>
                        <th id="thVersion" style="width: 1%; display: none"></th>
                        <th id="thEncID" style="width: 1%; display: none"></th>
                    </tr>
                </thead>
            </table>
        </div>--%>
       
        <div id="divBody" style="height: 250px; width: 100%" class="Editabletxtbox">
            <table class="table table-bordered table-condensed" style="width: 100%;table-layout: fixed;overflow:scroll;height:100%" id="tRxMed">
               
            </table>
        </div>
        <div id="dvdialogRxHistory" style="min-height: 65px !important; width: auto; max-height: none; height: auto; display: none;">
            <p style="font-family: Verdana,Arial,sans-serif; font-size: 13.5px;">
                There are unsaved changes.Do you want to save them?
            </p>
        </div>
        <%-- </div>
        </div>--%>
        <input type="hidden" id="hdnMedID" />
        <input type="hidden" id="hdndel" />
        <input type="hidden" id="hdnVersion" />
        <input type="hidden" id="hdnEncID" />
        <input type="hidden" id="hdnCurEncounterID" runat="server" />
        <input type="hidden" id="hdnUserRole" runat="server" />
        <input type="hidden" id="hdnCurProcess" runat="server" />

    </form>
</body>
<asp:placeholder id="PlaceHolder1" runat="server">
 
 <script src="JScripts/JSHistoryRx.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript" enableviewstate="False"></script>
        
<script src="JScripts/jsReconciliationCommon.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript" enableviewstate="False">
</script>

<script src="JScripts/JSMaskedinput.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript" enableviewstate="False"></script>

 <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript" enableviewstate="False"></script>

<script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript" enableviewstate="False"></script>

<script src="JScripts/JSCustomDLC.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript" enableviewstate="False"></script>

<script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript" enableviewstate="False"></script>
    
<script src="JScripts/jsCustomAutoCompleteCombobox.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript" enableviewstate="False"></script>
   
     </asp:placeholder>
</html>
