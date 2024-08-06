<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmRCopiaMergePatientBar.aspx.cs" Inherits="Acurus.Capella.UI.frmRCopiaMergePatientBar" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" style="height:100%">
<head runat="server">
    <title>RCopiaMergePatientBar</title>
     <link href="CSS/CommonStyle.css?version=1.2" rel="stylesheet" type="text/css" />
    <link href="CSS/fontawesomenew.css" rel="stylesheet" />
    <link href="CSS/bootstrap.min.css" rel="Stylesheet" />
    <link href="CSS/jquery-ui.css" rel="Stylesheet" />

    <link href="CSS/bootstrap.min3.1.1.css" rel="stylesheet" />
            <link href="CSS/bootstrap.min3.3.7.css" rel="stylesheet" />
    <style>
        .btn-default:hover {
            color: darkseagreen !important;
            background-color: white !important;
        }
    </style>
</head>
<body style="height:100%">
    <form id="frmRCopiaMergePatientBar" runat="server" style="height:100%" >
        <label id="lblPatientStrip" runat="server" class="newStyle1 pnlBarGroup" style="width: 100%; height: 11px;" ></label>
        <div>
            <input type="button" id="btnMedication" onclick="OnTabClick();" class="btn btn-default btncolorMyQ" value="Medication" />
            <input type="button" id="btnAllergies" onclick="OnTabClick();" class="btn btn-default" value="Allergies" />
        </div>
        <div style="background-color: #bfdbff; height: 25px; width: 100%; margin-top: 5px; border: 1px solid #909090;">
            <label id ="lblScreenDis" runat="server"></label>
        </div>
        <iframe runat="server" id="ifrmRcopiaDuplicateScreen" style="height:85%; width :100%;"></iframe>

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
              </asp:PlaceHolder>
    </form>
</body>
</html>
