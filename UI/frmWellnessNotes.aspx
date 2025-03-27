<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmWellnessNotes.aspx.cs" Inherits="Acurus.Capella.UI.frmWellnessNotes" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
   
        <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.2/jquery.min.js"></script>
  
     <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.1.1/js/bootstrap.min.js"></script>
  
        <script src="JScripts/JSSummaryNew.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript" ></script>
   <script src="JScripts/JSSessionExpiry.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>"
                type="text/javascript" enableviewstate="false"></script>
    <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
</head>
<body onload="{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }" >
    <form id="form1" runat="server">
    <div>
     <%--<button type="button" class="btn btn-primary btn-md" runat="server" onserverclick="btnWord_Click" id="btnword" style="display:none">Print Wellness Note</button>--%>
   <button type="button" class="btn btn-primary btn-md" runat="server" onclick="StartLoadingForNotes();" onserverclick="btnWordwellness_Click" style="display:none" id="btnWellness">Print Wellness Note</button>
        
         </div>
    </form>
</body>
</html>
