<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmRestrictMultipleTabs.aspx.cs" Inherits="Acurus.Capella.UI.frmRestrictMultipleTabs" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        #rcorners3 {
            border-radius: 25px;
            background-color: #a2bdca;
            padding: 20px;
            padding-top:35px;
            padding-bottom:15px;
            height: 46px;
            text-align: center;
            font-size: 25px;
            font-weight: bold;
        }
        img {
            float: right;
        }
        .divdat {
            padding-left: 150px;
            width: 1300px;
        }
        .Editabletxtbox {
            font-weight: normal;
            font-family: "Helvetica Neue",Helvetica,Arial,sans-serif;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="row">
                <div class="divdat">
                    <p id="rcorners3" class="Editabletxtbox">MULTIPLE TAB ACCESS WARNING</p>
                    <br />
                    <p style="font-size: 22px;margin:15px" class="Editabletxtbox">
                        <img src="Resources/multipletab.png" alt="Pineapple" style="width: 283px; height: 126px; margin-right: 15px;" />
                        You already have an active EHR session. Use "<a id="navigateWindowLink" href="#" onclick="navigateRestrictMultipleTabClick()">Windows</a>" in the active session to navigate to other open charts.
                    </p>
                </div>
            </div>
        </div>
    </form>
    <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>"></script>
</body>
</html>
