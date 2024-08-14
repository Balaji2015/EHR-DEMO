<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmRCopiaDuplicateMediations.aspx.cs" Inherits="Acurus.Capella.UI.frmRCopiaDuplicateMediations" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>RCopiaDuplicateMediations</title>
     <link href="CSS/CommonStyle.css?version=1.2" rel="stylesheet" type="text/css" />
    <link href="CSS/fontawesomenew.css" rel="stylesheet" />
    <link href="CSS/bootstrap.min.css" rel="Stylesheet" />
    <link href="CSS/jquery-ui.css" rel="Stylesheet" />

    <link href="CSS/bootstrap.min3.1.1.css" rel="stylesheet" />
            <link href="CSS/bootstrap.min3.3.7.css" rel="stylesheet" />
    <style>
        #RCopiaDuplicateMediationsTable th {
             text-align:center;
              font-weight:bold;
              background-color: #bfdbff;
        }
        #RCopiaDuplicateMediationsTable {
             text-align:center;
        }

    </style>
</head>
<body>
    <form id="frmRCopiaDuplicateMediations" runat="server" style="margin-top:4px;">
        <div>&emsp;<label  style="all: revert;" class="patientchartfontfamily">Medication </label> &emsp; <input type="text" id="txtSearcMedication" onkeyup="SearchMedication()" placeholder="Type your Medication here" style="width:433px;" /> <img id="imgClearProviderText" onclick="ClearAllSearch()" src="Resources/Delete-Blue.png" alt="X" title="Click to clear the text field." style="position: absolute; left: 545px; top: 12px; cursor: pointer; width: 12px; height: 12px;" /></div>
        <div id="divRCopiaDuplicateMediations" style="overflow:auto; height:450px;">
            <table id="RCopiaDuplicateMediationsTable" class='table table-bordered Gridbodystyle' style='table-layout: fixed;'>
              <thead>
                  <tr>
                      <th style='width:65px; border: 1px solid #909090;'>Select</th>
                      <th style='width: 335px; border: 1px solid #909090;'>Medication</th>
                      <th style="border: 1px solid #909090;">Patient Instruction</th>
                      <th style="border: 1px solid #909090;">Other Instruction</th>
                      <th style="border: 1px solid #909090;">Start Date</th>
                      <th style="border: 1px solid #909090;">Stop Date</th>
                      <th style="border: 1px solid #909090;">Created By</th>
                      <th style="border: 1px solid #909090;">Created Date And Time</th>
                      <th style="border: 1px solid #909090;">Status</th></tr>
              </thead>
               <tbody id="RCopiaDuplicateMediationsTableBody">
                </tbody>
            </table>
        </div>
        <input type="checkbox" name="ShowAll" id="chkShowAll" onclick='ShowAllMedication();' style="display:none" />
        <label for="ShowAll" style="display:none">ShowAll</label>
        <input type="button" id="btnDelete" onclick="DeleteMedication();" class="aspresizedredbutton" value="Delete" style="width:85px;float: right;" />

        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
        

        <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>"
                type="text/javascript" enableviewstate="false"></script>

            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>"
                type="text/javascript" enableviewstate="false"></script>

        <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>"
                type="text/javascript"></script>
            
             <script src="JScripts/jquery-1.11.3.min.js" type="text/javascript"></script>
            <script src="JScripts/jquery-ui.min1.10.2.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/bootstrap.min.js" type="text/javascript"></script>
        <script src="JScripts/jquery-1.11.3.min.js" type="text/javascript"></script>
            <script src="JScripts/JsRCopiaMergePatientBar.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>"
                type="text/javascript" enableviewstate="false"></script>
             <script src="JScripts/JsRCopiaDuplicateMediations.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>"
                type="text/javascript" enableviewstate="false"></script>
              </asp:PlaceHolder>
    </form>
</body>
</html>
