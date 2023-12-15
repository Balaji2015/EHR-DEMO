<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmRestrictedAccess.aspx.cs" Inherits="Acurus.Capella.UI.frmRestrictedAccess" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        #rcorners3 {
            font-size: 25px;
            font-weight: bold;
        }

        .divdat {
            padding-left: 150px;
            width: 1300px;
        }

        .Editabletxtbox {
            text-align: center;
            font-weight: normal;
            font-family: "Helvetica Neue",Helvetica,Arial,sans-serif;
        }
        #form1 .row {
           padding:30px 0;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="row align-items-center">
                <div class="col-sm-4"></div>
                <div class="col-sm-4">
                    <center><img src="Resources/permission-error.jpg" alt="Pineapple" style="width: 100px; height: 100px;" /></center>
                    <p id="rcorners3" class="Editabletxtbox">Access Restricted</p>
                    <p class="Editabletxtbox">You do not have permission to view this page.</p>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
