<%@ Page  Async="true" Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="frmVitals.aspx.cs"
    Inherits="Acurus.Capella.UI.frmVitals"  ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
<meta http-equiv='X-UA-Compatible' content='IE=7' />
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">

    <title>Vitals</title>
  
<%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>

    <base target="_self" />
    <style type="text/css">
        .datepicker-years .picker-switch {
            cursor: default !important;
            background: inherit !important;
        }

        .close {
            float: right;
            font-size: 21px;
            font-weight: 700;
            line-height: 1;
            color: #000;pnlVitals
            text-shadow: 0 1px 0 #fff;
            filter: alpha(opacity=20);
            opacity: .2;
        }

       
        .displayprop{
            display:none;
        }
        .style7 {
            width: 304px;
            height: 26px;
        }

      

       

      

        .style16 {
            width: 210px;
            height: 26px;
        }

        .displayNone {
            display: none;
        }

        #frmVitals {
            margin-bottom: 8px;
            height: 643px;
            width: 100%;
        }

        #tblVitals {
            width:1180px;
            height: 40px;
        }

        .style17 {
            width: 85px;
        }
        #tblVitalControls {
            overflow-y:auto;
        }
        

        .modal {
            position: fixed;
            top: 0;
            left: 0;
            background-color: White;
            z-index: 99;
            opacity: 0.8;
            filter: alpha(opacity=80);
            -moz-opacity: 0.8;
            min-height: 100%;
            width: 100%;
        }



        .style21 {
            width: 638px;
        }

        .underline {
            text-decoration: underline;
        }

        .myGridStyle {
            border-collapse: collapse;
        }

        .GridviewScrollHeader TH, .GridviewScrollHeader TD {
            padding: 5px;
            font-weight: bold;
            white-space: nowrap;
            border-right: 1px solid #AAAAAA;
            border-bottom: 1px solid #AAAAAA;
            background-color: #7fade4;
            text-align: left;
            vertical-align: bottom;
            height: 2px;
        }

        .GridviewScrollItem TD {
            padding: 5px;
            white-space: nowrap;
            border-right: 1px solid #AAAAAA;
            border-bottom: 1px solid #AAAAAA;
            background-color: #FFFFFF;
        }

        .GridviewScrollPager {
            border-top: 1px solid #AAAAAA;
            background-color: #FFFFFF;
        }

            .GridviewScrollPager TD {
                padding-top: 3px;
                font-size: 14px;
                padding-left: 5px;
                padding-right: 5px;
            }

            .GridviewScrollPager A {
                color: #666666;
            }

            .GridviewScrollPager SPAN {
                font-size: 16px;
                font-weight: bold;
            }

        div#gridPanel {
            width: 900px;
            overflow: scroll;
            position: relative;
        }


            div#gridPanel th {
                top: expression(document.getElementById("gridPanel").scrollTop-2);
                left: expression(parentNode.parentNode.parentNode.parentNode.scrollLeft);
                position: relative;
                z-index: 20;
            }

        #tag:hover {
            text-decoration: underline;
        }
    </style>
     <link href="CSS/bootstrap.min3.3.5.css" rel="stylesheet" />
    <link href="CSS/bootstrap.min.css" rel="stylesheet" />
    <link href="~/CSS/font-awesome.css" rel="stylesheet" type="text/css" />
      <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />


</head>
<body onload="OnVitalsLoad();OpenNotificationPopUp('VITALS');">
    <form id="frmVitals" runat="server">
        <table id="tblVitals" bgcolor="white">
            <tr>
                <td class="vitalheadertext1" >Vital Name
                </td>
                <td class="vitalheadertext2" align="left">Value
                </td>
                <td class="style16"></td>
               
                 <td class="vitalheadertext2" visible="false">&nbsp;Captured Date Time
                </td>
                
                
                <td align="left"  class="vitalheadertext3"> Comments / Reason for Not Performed
                </td>
            </tr>
            <tr>
                <td colspan="7">
                    <asp:Label ID="lblHeaderLine" runat="server" Font-Bold="True" Text="__________________________________________________________________________________________________________________________________________"
                        Width="1094px"></asp:Label>
                </td>
            </tr>
        </table>


        <div id="pnlVitals" runat="server" style="width: 100%; height: 544px; position: relative; font-family: Times New Roman; overflow-y: auto;overflow-x:hidden">

        </div>

        <asp:Panel ID="pnlButtons" runat="server" Width="95%">
            <table style="width: 100%; height: 28px; margin-top: 15px;" id="tblButtons" bgcolor="white">
                <tr>
                    <td align="right">
                        <div style="float: left; padding-top: 12px;">
                            <div style="width: 100%; height: 47px;">
                                <div style="float: left; height: 47px; padding-left: 5px; color: white; margin-top: -5px; text-align: center;">
                                    <img src="Resources/measure_logo.png" alt="" onclick="OpenMeasurePopup('Vitals');" style="height: 48px; width: 45px; padding-left: 2px; margin-left: -8px;" />
                                </div>
                                <div id="tag" style="margin-top: -13px; margin-left: -99px; font-size: 19px; height: 48px; width: 301px; font-weight: bold; color: #6504d0; border-radius: 7px; cursor: pointer; font-family: source sans pro;" onclick="OpenMeasurePopup('Vitals');">
                                    Measure Booster<img src="Resources/measure_info.png" alt="" style="width: 16px; height: 15px; margin-left: 4px;"/>
                                </div>
                            </div>
                        </div>
                        
                        <div style="float: right;">
                            <input type="button" runat="server" class="btn btn-primary" id="btnCopyCC" accesskey="v" value="View Past Vitals"
                                onclick="return OpenPastVitals();" />
                            <input type="button" runat="server" class="btn btn-success" id="btnSaveVitals" accesskey="s"
                                disabled="disabled" value="Save" />
                            <input type="button" runat="server" class="btn btn-danger" id="inpbtnClearall" accesskey="c"
                                value="Clear All" />
                        </div>
                    </td>
                   
                </tr>
            </table>
        </asp:Panel>
        <br />
        <div style="overflow: hidden; visibility: hidden; height: 21px;" id="DivHeaderRow">
        </div>

        <div style="overflow: scroll;width:1140px !important" onscroll="OnScrollDiv(this)" id="GridDiv" runat="server" >

            <asp:GridView ID="gvEnterDetails" runat="server" CellSpacing="0" GridLines="Vertical"
                AutoGenerateColumns="true" EnableViewState="true" 
                ShowFooter="true" Width="1130px" 
                Height="200px" OnRowDataBound="gvEnterDetails_RowCreated" CssClass="Gridbodystyle" >
                <HeaderStyle Wrap="false"  CssClass="Gridheaderstyle" />
            
                <Columns>
                    <asp:TemplateField HeaderText="Edit">

                        <ItemTemplate>
                            <asp:ImageButton ID="imgbtnEdit" CommandName="EditRow" runat="server" ImageUrl="~/Resources/edit.gif" ToolTip="Edit"
                                Height="20px" Width="20px" OnClientClick='<%# " return grdPastVitals_OnCommandEdit("+ Container.DataItemIndex +"," 
                                                 + Eval("EncounterID")+","+"editVariable"+");" %>' 
                                CommandArgument='<%# Container.DataItemIndex %>' OnClick="imgbtnEdit_Click" CssClass="Gridbodystyle"  />

                        </ItemTemplate>
                       
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Del" >
                        <ItemTemplate>
                            <asp:ImageButton ID="imgbtnDel" CommandName="Del" runat="server" ImageUrl="~/Resources/close_small_pressed.png" ToolTip="Delete" Height="20px" Width="20px"
                                OnClientClick='<%# " return grdPastVitals_OnCommandDelete("+ Container.DataItemIndex +"," 
                                                 + Eval("EncounterID")+","+"deleteVariable"+");" %>'
                                CommandArgument='<%# Container.DataItemIndex %>' OnClick="imgbtnDelete_Click" CssClass="Gridbodystyle" />

                        </ItemTemplate>

                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Captured Date Time">

                        <ItemTemplate>
                            <asp:Label ID="lblDate" runat="server" Text='<%#Eval("Entered Date Time") %>' />
                        </ItemTemplate>
                        <HeaderStyle width="40%" />
                        <ItemStyle Width="40%" />
                        <FooterStyle Width="40%" />

                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="EncounterID" Visible="false">

                        <ItemTemplate>
                            <asp:Label ID="lblDesg" runat="server" Text='<%#Eval("EncounterID") %>' />
                        </ItemTemplate>
                        <HeaderStyle width="40%"  />
                        <ItemStyle Width="40%" />
                        <FooterStyle Width="40%" />

                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="GroupID" Visible="false">

                        <ItemTemplate>
                            <asp:Label ID="lblDes" runat="server" Text='<%#Eval("GroupID") %>' />
                        </ItemTemplate>

                    </asp:TemplateField>

                </Columns>
            </asp:GridView>
           
        </div>
        <input type="button" runat="server" class="aspredbutton aspresizedredbutton " id="BtnClose1" accesskey="v" value="Close"
            onclick="btnClose_Clicked()" style="padding-top: 0px; float: right; display: none;" />


        <div id="divLoading" class="modal" runat="server" style="text-align: center; display: none">
            <asp:Panel ID="Panel2" runat="server">
                <br />
                <br />
                <br />
                <br />
                <center>
                    <asp:Label ID="Label1" Text="" runat="server"></asp:Label></center>
                <br />
                <img src="Resources/wait.ico" title="" alt="Loading..." />
                <br />
            </asp:Panel>
        </div>

        <div id="PastVitals" class="modal modal-wide fade" style="z-index: 5000; height: 520px; width: 1400px; background-color: rgba(0, 0, 0, 0.13)">
            <div id="ViewPastVitals" class="modal-dialog" style="margin-top: 90px; overflow: hidden; height: 520px; width: 820px; margin-top: 20px; margin-left: 150px">
                <div class=" modal-content" style="height: 100%">
                    <div class="modal-header" style="height: 40px">
                        <button type="button" class="close" id="btnClosed" style="font-size: 30px !important;margin-top:-8px;" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <h3 class="modal-title" style="font-family: sans-serif; font-weight: bold; font-size: medium" id="PastVitalsTitle"></h3>
                    </div>
                    <div class="modal-body" style="width: 100%; height: 100%; padding-top: 0px">
                        <iframe style="width: 100%; height: 455px; width: 800px; margin-top: 2px; border: none;" id="PastVitalsFrame"></iframe>
                    </div>
                </div>

            </div>
        </div>
        <asp:Button ID="btnClear" runat="server" CssClass="displayNone" OnClick="btnClear_Click" />
        <asp:Button ID="btnDelete" runat="server" Text="DisableLoad" CssClass="displayNone" OnClick="btnDelete_Click" />
        <asp:Button ID="InvisibleButton" runat="server" CssClass="displayNone" OnClick="InvisibleButton_Click" />
        <asp:Button ID="InvisibleButton1" runat="server" CssClass="displayNone" OnClick="InvisibleButton1_Click" />
        <asp:Button ID="btnLoadDefaultValues" runat="server" CssClass="displayNone" OnClick="btnLoadDefaultValues_Click" />
        <input type="button" runat="server" style="display: none" id="btnsavehidden" onserverclick="btnSave_Click" />
        <asp:HiddenField ID="hdnPicId" runat="server" />
         <asp:HiddenField ID="hdnVisittype" runat="server" />
         <asp:HiddenField ID="hdnBPValue" runat="server" />
        <asp:HiddenField ID="hdnBMI" runat="server" />
        <asp:HiddenField ID="hdnHbA1c" runat="server" />
         <asp:HiddenField ID="hdnHgb" runat="server" /><%--BugID:51648--%>
        <asp:HiddenField ID="hdnegfr" runat="server" />
        <asp:HiddenField ID="hdnegfrSecond" runat="server" />
        <asp:HiddenField ID="hdnBloodFasting" runat="server" />
        <asp:HiddenField ID="hdnBloodFastingSecond" runat="server" />
        <asp:HiddenField ID="hdnBloodPost" runat="server" />
        <asp:HiddenField ID="hdnBloodPostSecond" runat="server" />
        <asp:HiddenField ID="hdnLocalTime" runat="server" />
        <asp:HiddenField ID="hdnUrineforMicroalbumin" runat="server" />
        <asp:HiddenField ID="hdnABITest" runat="server" />
        <asp:HiddenField ID="hdnNotification" runat="server" />
        <asp:HiddenField ID="hdnOpeningFrom" runat="server" />
        <asp:HiddenField ID="hdnForMenuLevelCancel" runat="server" />
        <asp:HiddenField ID="hdnMessageType" runat="server" Value="" />
        <asp:HiddenField ID="hdnSystemTime" runat="server" />
        <asp:HiddenField ID="HiddenField1" runat="server" />
        <asp:HiddenField ID="hdnEnableYesNo" runat="server" />
        <asp:HiddenField ID="hdnbuttonName" Value="false" runat="server" />
        <asp:HiddenField ID="hdnRequestDate" Value="false" runat="server" />
        <asp:HiddenField ID="hdnEdit" Value="" runat="server" />
        <asp:HiddenField ID="hdnHeader" Value="" runat="server" />
        <asp:HiddenField ID="birthdate" Value="" runat="server" />
        <asp:HiddenField ID="hdnscript" Value="" runat="server" />
        <asp:HiddenField ID="hdnType" runat="server" />
          <asp:HiddenField ID="hdnreason" Value="" runat="server" />
        <asp:Button ID="btnMessageType" runat="server" Text="Button" Style="display: none" OnClientClick="return autosavevitals();" />
        <asp:HiddenField ID="hdnRowIndex" runat="server" EnableViewState="false" />
        
        <asp:HiddenField ID="hdnBP" Value="" runat="server" />
        <asp:HiddenField ID="hdnLabResults" Value="false" runat="server" />
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">

           
            <link href="CSS/bootstrap.min3.3.7.css" rel="stylesheet" />
            <link href="CSS/bootstrap.min.css" rel="stylesheet" />
            <script src="JScripts/jquery-1.11.3.min.js" type="text/javascript"></script>
            <link href="CSS/bootstrap.min3.3.7.css" rel="stylesheet" />

            <script src="JScripts/bootstrap.min3.3.7.js" type="text/javascript"></script>
            <script src="CSS/moment.js" type="text/javascript"></script>
            <script src="CSS/bootstrap.min.js" type="text/javascript"></script>
            <script src="CSS/datetimepicker.js" type="text/javascript"></script>
            <link href="CSS/bootstrap-datetimepicker.min.css" rel="stylesheet" />

            <script src="Jscripts/jquery-ui.js" type="text/javascript"></script>
            <link rel="stylesheet" href="CSS/jquery-ui.min.css">


            <script src="JScripts/jstat.min.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/lodash.min.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/moment.min.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/numeral.min.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/numeric.min.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/formula.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSCustomDLC.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSVitals.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
             <script src="JScripts/JSMaskedinput.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>



           
        </asp:PlaceHolder>

    </form>
</body>
</html>
