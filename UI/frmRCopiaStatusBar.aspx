<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmRCopiaStatusBar.aspx.cs" Inherits="Acurus.Capella.UI.frmRCopiaStatusBar" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <link href="CSS/jquery-ui.css" rel="stylesheet" />
    <link href="CSS/font-awesome.css" rel="stylesheet" type="text/css" />
    <link href="CSS/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.7.2/font/bootstrap-icons.css" />
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap" rel="stylesheet" />

    <style>
        .akidoboxstyle {
            /*padding: 10px;*/
            /*border: 1px solid #E0E0E0;*/
            font-size: 14px;
            font-family: 'Inter', sans-serif;
            display: flex;
           /* margin: 10px;*/
            /*border-radius: 5px;*/
            font-weight: 700;
            width: fit-content;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="akidoboxstyle">
            <div style="margin-right: 10px;">
                <span onclick="LoadRcopiaRxCount();" style="color: #0078d8 !important; cursor: pointer;" class="glyphicon glyphicon-refresh"></span>
            </div>
            <div style="margin-right: 15px;">
                <span>Refill: <span id="lblRefill">0</span></span>
            </div>
            <div>
                <span>Pending Rx: <span id="lblPendingRx">0</span></span>
            </div>
        </div>
        <input type="hidden" runat="server" id="hdnEmail"/>
        <input type="hidden" runat="server" id="hdnLegalOrg"/>
    </form>
    <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
    <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella-","") %>" type="text/javascript"></script>
    <script>
        $(document).ready(function () {
            LoadRcopiaRxCount();
        });

        function LoadRcopiaRxCount() {
            var email = $('#hdnEmail').val();
            var legalOrg = $('#hdnLegalOrg').val();
            var url = "frmRCopiaToolbar.aspx/LoadRCopiaNotification";
            var data = '';
            if (email) {
                url = "frmRCopiaStatusBar.aspx/LoadRCopiaNotification";
                data = '{email: "' + email + '",legalOrg: "' + legalOrg + '"}';
            }
            $.ajax({
                type: "POST",
                url: url,
                data: data,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: function () {
                    RefreshRcopia();
                },
                success: function (response) {
                    OnSuccessRCopia(response);
                },
                error: function OnError(xhr) {
                    if (xhr.status == 999)
                        window.location = "/frmSessionExpired.aspx";
                    else {
                        var log = JSON.parse(xhr.responseText);
                        console.log(log);
                        alert("USER MESSAGE:\n" + xhr.status + "-" + xhr.statusText +
                            ". \nCannot process request. Please Login again and retry. If issue persists, Please contact Support.\n\nEXCEPTION DETAILS: \nException Type" +
                            log.ExceptionType + " \nMessage: " + log.Message);
                    }
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                }
            });
        }

        function RefreshRcopia() {
            if (document.getElementById("lblRefill") != null && document.getElementById("lblRefill") != undefined) {
                document.getElementById("lblRefill").innerText = "Loading...";
            }
            if (document.getElementById("lblPendingRx") != null && document.getElementById("lblPendingRx") != undefined) {
                document.getElementById("lblPendingRx").innerText = "Loading...";
            }
            //loadRcopiaRxCount();
        }

        function OnSuccessRCopia(response) {
            var responseValues = response.d.split('#$%');
            var rxValues = "";
            if (responseValues == "") {
                document.getElementById("lblRefill").style.display = "none";
                document.getElementById("lblPendingRx").style.display = "none";
            }
            if (responseValues != null && responseValues.length > 1) {
                document.getElementById("lblRefill").innerText = responseValues[0].split(':')[1].trim();
                document.getElementById("lblPendingRx").innerText = responseValues[1].split(':')[1].trim();
                rxValues = document.getElementById("lblRefill").innerText + "$:$" + document.getElementById("lblPendingRx").innerText;
            }
            else {
                document.getElementById("lblRefill").innerText = "0";
                document.getElementById("lblPendingRx").innerText = "0";
                rxValues = document.getElementById("lblRefill").innerText + "$:$" + document.getElementById("lblPendingRx").innerText;
            }
            sessionStorage.setItem("RxCount", rxValues);
        }
    </script>
</body>
</html>
