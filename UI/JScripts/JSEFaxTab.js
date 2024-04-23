$(document).ready(function () {   
    $('#faxtab a:first').tab('show');
    $("#tabfax_Compose").on('click', function () {       
        var sPath = "";
        //Cap - 1918
        //sPath = "frmEFax.aspx";
        sPath = "frmEFax.aspx?Mode='MenuLevelEFax'";
        $("#TabComposeFrame")[0].contentDocument.location.href = sPath;
    });
    $("#tabfax_Outbox").on('click', function () {      
        var sPath = ""
        //CAP-1831 - eFax Outbox - Introduce filter
        sPath = "HtmlEFaxOutbox.html?version=" + sessionStorage.getItem("ScriptVersion");
        $("#TabOutboxFrame")[0].contentDocument.location.href = sPath;
    });
});

