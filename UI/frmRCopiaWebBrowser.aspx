 <%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmRCopiaWebBrowser.aspx.cs"
    Inherits="Acurus.Capella.UI.frmRCopiaWebBrowser" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
      <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
   <script type="text/javascript">
       function AllowNumbers(evt) {
           evt = (evt) ? evt : window.event;
           $("#btnsave")[0].disabled = false;
           window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
           localStorage.setItem("bSave", "false");
           var charCode = (evt.which) ? evt.which : evt.keyCode;
           if (charCode > 31 && (charCode < 48 || charCode > 57)) {
               return false;
           }
           return true;
       }


       function LoadAddendum() {
           $("[id*=pbDropdown]").addClass('pbDropdownBackground');
           $("span[mand=Yes]").addClass('MandLabelstyle');

           $("span[mand=Yes]").each(function () {
               $(this).html($(this).html().replace("*", "<span class='manredforstar'>*</span>"));
           });
       }
   </script>
    <base target="_self" />
    <title>E-Prescription</title>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
        .style1
        {
            height: 570px;
        }
        *
        {
            font-size: 13px;
            font-family: Microsoft Sans Serif;
        }
         #tag:hover {
            text-decoration: underline;
        }
    </style>
</head>
<body onload="LoadAddendum(); OpenNotificationPopUp('ERX');">
    <form id="form1" runat="server">
    <div>
        <table class="style1" width="100%">
            <tr>
                <td style="text-align: right; width: 100%" colspan="4">
                    <iframe id="radBrowser" runat="server" width="100%" height="1000px"></iframe>
                </td>
            </tr>
          
             <tr style="height: 25px" id="btnSet1" runat="server">
                <td style=" width: 73%; margin-left:10px;" 
                    align="left">
                     <fieldset id="pnl" runat="server" style="width:99%;background-color:white" class="Editabletxtbox">
                        <legend>Manual Prescriptions and Medication Documentation Details</legend>
                        <span id="lblcount" runat="server" enableviewstate="false" >No of Medication Orders Filled  in Paper </span>
                        <input type="text" id="txtcount" runat="server" style="width:50px;" maxlength="3" onkeypress="return AllowNumbers(event)" enableviewstate="true" />
                         <br /><br />
                         <input type="checkbox" runat="server" enableviewstate="true" id="chkCurrentMedicationDocumented" onclick="this.form.submit();" onserverchange="chkCurrentMedicationDocumented_ServerChange"/><span id="txtCurrent"  runat="server"  mand="Yes">Current Medication Documented/Updated/Reviewed *</span>
                         &nbsp;&nbsp;&nbsp;&nbsp;
                         <label id="lblCurrentMedicationDocumented" style="color:red;" runat="server" enableviewstate="false">Reason Not Performed *</label>
                         <select id="cboCurrentMedicationDocumented" runat="server">
                         </select>
                        <button type="button" id="btnsave" runat="server" style="width:50px;" onserverclick="btnsave_Click"  class="aspresizedgreenbutton" disabled>Save</button>
                    </fieldset>
                  
                </td>
                <td style="width: 27%" align="center">
                    <fieldset id="Panel2" runat="server" style="width:75%;background-color:white">
                        <legend>Is medication reconciliation done</legend><br /><br />
                        <button type="button" id="btnMedReview" runat="server" style="width:140px;" onserverclick="btnMedReview_Click" class="aspresizedbluebutton">Review Medication</button>
                         <br /><br />
                    </fieldset>
               
                </td>
            </tr>
            <tr style="height: 25px" id="btnSet2" runat="server">
                <td style="width: 40%" align="left">
                    <input type="checkbox" runat="server" enableviewstate="true" id="chkMove" onclick="javascript: form1.submit();"  onserverchange="chkMove_CheckedChanged"/><span id="tXt" runat="server">Move to MA to fill</span>
               
               </td>
                <td style="width:49%;" align="right">
                         <asp:Button ID="btnpatientChart1" runat="server" OnClientClick="return btnpatientChart_Click();" Text="Open Patient Chart"
                                            Width="150px" Visible="true" CssClass="aspresizedbluebutton" />
                             
                    <button type="button" id="btnMove" runat="server"  style="width:155px;" onserverclick="btnMove_Click" onclick="btnMoveClientClick();" visible="false" class="aspresizedbluebutton" >
                        Move To Next Process</button>
                  
                    &nbsp;
                    <button type="button" id="btnClose" runat="server" style="padding-left: 7px;margin-right: 4px;margin-bottom: -6px;width:50px;"   onclick="RadWindowClosepopup();" class="aspresizedredbutton">Close</button>
                     <%--onserverclick="btnClose_Click"--%> 
                   
                </td>
            </tr>
               
        </table>
    </div>
         <div style="width: 100%; margin-top: 30px; height: 50px;">
                <div style="float: left; height: 47px; padding-left: 5px; color: white; margin-top: -5px; text-align: center;">
                    <img src="Resources/measure_logo.png" alt="" onclick="OpenMeasurePopup('eRx');" style="height: 48px; width: 45px; padding-left: 2px; margin-left: -8px;" />
                </div>
                <div id="tag" style="margin-top: -13px; margin-left: -99px; font-size: 19px; height: 48px; width: 303px; font-weight: bold; color: #6504d0; border-radius: 7px; cursor: pointer; font-family: source sans pro;" onclick="OpenMeasurePopup('eRx');">
                    Measure Booster<img src="Resources/measure_info.png" alt="" style="width: 16px; height: 15px; margin-left: 4px;" />
                </div>
            </div>
        <input type="hidden" id="hdnLocalTime" runat="server" />
        <input type="hidden" id="hdnHumanID" runat="server" />
        <input type="hidden" id="hdnLabResult" runat="server" />
        <asp:HiddenField ID="EnableSave" Value="false" runat="server" />
    <asp:PlaceHolder ID="PlaceHolder1" runat="server">
    <script src="JScripts/JSMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
           <script src="JScripts/jquery-1.11.3.min.js" type="text/javascript"></script>
        <script src="JScripts/JSRcopiaWebBrowser.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
<script src="JScripts/jquery-ui.min1.10.2.js" type="text/javascript"></script>
<script src="JScripts/jquery-3.2.1.js" type="text/javascript"></script>
   </asp:PlaceHolder>
    </form>
</body>
</html>
<script type="text/javascript">
    $(document).ready(function () {

        //if ($("#btnsave")[0] != undefined) {
        //    if (document.getElementById("EnableSave").value == "true") {
        //        $("#btnsave")[0].disabled = false;
        //        if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined) {
        //            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
        //            localStorage.setItem("bSave", "false");
        //        }
        //    }
        //    else
        //        $("#btnsave")[0].disabled = true;
        //}

        function EnableSave() {
            if ($("#btnsave")[0] != undefined) {
                $("#btnsave")[0].disabled = false;
            }
            if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined) {
                window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
                localStorage.setItem("bSave", "false");
            }
        }

        function DisableSave() {

            if ($("#btnsave")[0] != undefined) {
                $("#btnsave")[0].disabled = true;
            }
            if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined) {
                window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                localStorage.setItem("bSave", "true");
            }

        }



        if (($("#txtcount")[0] != undefined) && $("#txtcount")[0].disabled == true) {
            DisableSave();
        }

        $.ajax({
            type: "POST",
            url: "frmRCopiaWebBrowser.aspx/CheckMedReviewStatus",
            data: '',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (response.d == true) {
                    if (($("#btnMedReview")[0]) != undefined) {
                        btnMedReview.disabled = true;
                        btnMedReview.innerHTML = "Medication Reviewed";
                    }

                }
                else
                    if (($("#btnMedReview")[0]) != undefined) {
                        btnMedReview.disabled = false;
                    }
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            },
            error: function OnError(xhr) {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                var log = JSON.parse(xhr.responseText);
                console.log(log);
                if (xhr.status == 999)
                    window.location = "/frmSessionExpired.aspx";
                else
                    alert("USER MESSAGE:\n" +
                                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                                   "Message: " + log.Message);
            }
        });
        $("#chkCurrentMedicationDocumented").change(function () {
            $("#btnsave")[0].disabled = false;
            if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined) {
                window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
                localStorage.setItem("bSave", "false");
            }
            if ($("#chkCurrentMedicationDocumented")[0].checked) {
                $("#cboCurrentMedicationDocumented").attr('disabled', 'disabled');
                $("#lblCurrentMedicationDocumented")[0].innerText = $("#lblCurrentMedicationDocumented")[0].innerText.replace("*", " ").trim();
                $("#lblCurrentMedicationDocumented")[0].style.color = "black";
                $("#cboCurrentMedicationDocumented")[0].selectedIndex = 0;
            }
            else {
                $("#cboCurrentMedicationDocumented").attr('disabled', false);
                $("#lblCurrentMedicationDocumented")[0].innerText += "*";
                $("#lblCurrentMedicationDocumented")[0].style.color = "red";
            }
            $("#btnsave")[0].disabled = false;
            if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined) {
                window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
                localStorage.setItem("bSave", "false");
            }
        });
        $("#cboCurrentMedicationDocumented").change(function () {
            $("#btnsave")[0].disabled = false;
            if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined) {
                window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
                localStorage.setItem("bSave", "false");
            }
        });
        $("#btnsave").click(function () {
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        });
        $("#btnMedReview").click(function () {
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        });
        $("#chkMove").click(function () {
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        });


    });



</script>
