<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmHistoryFamily.aspx.cs" EnableEventValidation="false"
    Inherits="Acurus.Capella.UI.frmHistoryFamily"  ValidateRequest="false"  %>

<%@ Register Src="~/UserControls/CustomDLCNew.ascx" TagName="DLC" TagPrefix="DLC" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Family History</title>
        <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>

  <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>


    <style type="text/css">
        .style2
        {
            height: 75px;
        }
        .style3
        {
            height: 75px;
            width: 126px;
        }
        .style4
        {
            width: 126px;
        }
        .style5
        {
            width: 612px;
        }
        .displayNone
        {
        	
            display: none;
        }
        .modal
        {
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
        .RadInput_Default .riTextBox, html body .RadInputMgr_Default {
            background: #e3e3e3;
        }
        input[type="text"]:disabled {
                background: #e3e3e3;
            }

        .underline
        {
            text-decoration: underline;
        }
    </style>
 <link href="~/CSS/style.css" rel="stylesheet" type="text/css" />
    <link href="~/CSS/font-awesome.css" rel="stylesheet" type="text/css" />
      <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
   
</head>
<body  onload="HistoryFamily_Load()" onkeypress="return PageKeyPress();">
    <form id="frmFamilyHistory" runat="server" style="background-color: White; font-family: Microsoft Sans Serif;
    height:100%; margin-bottom: 0px; width: 100%">
    <telerik:RadWindowManager ID="WindowMngr" runat="server" IconUrl="Resources/16_16.ico">
        <windows>
            <telerik:RadWindow ID="MessageWindow" runat="server"  Title="Family History"
                IconUrl="Resources/16_16.ico">
            </telerik:RadWindow>
        </windows>
    </telerik:RadWindowManager>
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Width="100%">
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" IconUrl="Resources/16_16.ico" EnableViewState="false">
        </telerik:RadWindowManager>
        <telerik:RadScriptManager ID="RadScriptManager1" EnableViewState="false" runat="server" >
            <scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js">
                </asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js">
                </asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js">
                </asp:ScriptReference>
            </scripts>
        </telerik:RadScriptManager>
        <div>
            <table style="height: 100%; width: 90%">
            <tr >
                <td valign="top" width="100%">
                   <asp:Panel ID="MenuPanel" runat="server" Width="100%"  Height="100%" style="display:none;" Font-Size="8.5pt" Font-Bold="true" BackColor="White">
                      <table style="height: 100%; width: 100%;">
                        <tr style="height: 100%; width: 100%;">
                                    <td style="width:8%; text-align:left">
                                    <asp:Label ID="lblRelationship" runat="server" Text="Relationship" class="LabelStyleBold"   EnableViewState="false" ></asp:Label>
                                    </td>
                                    <td style="width: 12%; text-align:left">
                                     <asp:Label ID="lblSelectRelationship" runat="server" Text="Select Relationship" class="LabelStyleBold" EnableViewState="false"></asp:Label>
                                    </td>
                                    <td style="width: 10%; text-align:left">
                                     <asp:Label ID="lblAge" runat="server" Text="Age" class="LabelStyleBold" EnableViewState="false"></asp:Label>
                                    </td>
                                    <td style="width: 20%; text-align:left">
                                     <asp:Label ID="lblFamilDisease" runat="server" Text="Family Disease" class="LabelStyleBold" EnableViewState="false"></asp:Label>
                                    </td>
                                    <td style="width: 1%; text-align:left">
                                     <asp:Label ID="lblEmpty" runat="server" Text=" " class="LabelStyleBold" EnableViewState="false"></asp:Label>
                                    </td>
                                    <td style="width: 10%; text-align:left">
                                     <asp:Label ID="lblStatus" runat="server" Text="Status" class="LabelStyleBold" EnableViewState="false" ></asp:Label>
                                    </td>
                                    <td style="width: 20%; text-align:left">
                                     <asp:Label ID="lblCauseOfDeath" runat="server" Text="Cause of Death" class="LabelStyleBold" EnableViewState="false"></asp:Label>
                                    </td>
                            </tr>
                         </table>
                       </asp:Panel>
                    <asp:Panel ID="ScreenPanel" runat="server" Width="100%"  Height="100%" style="display:none;" Font-Size="8.5pt" Font-Bold="true" BackColor="White">
                      <table style="height: 100%; width: 100%;">
                        <tr style="height: 100%; width: 100%;">
                                    <td style="width:5%; text-align:left">
                                    <asp:Label ID="Label1" runat="server" Text="Relationship" class="LabelStyleBold"   EnableViewState="false" ></asp:Label>
                                    </td>
                                    <td style="width: 10%; text-align:left">
                                     <asp:Label ID="Label2" runat="server" Text="Select Relationship" class="LabelStyleBold" EnableViewState="false"></asp:Label>
                                    </td>
                                    <td style="width: 7%; text-align:left">
                                     <asp:Label ID="Label3" runat="server" Text="Age" class="LabelStyleBold" EnableViewState="false"></asp:Label>
                                    </td>
                                    <td style="width: 17%; text-align:left">
                                     <asp:Label ID="Label4" runat="server" Text="Family Disease" class="LabelStyleBold" EnableViewState="false"></asp:Label>
                                    </td>
                                    <td style="width: 10%; text-align:left">
                                     <asp:Label ID="Label5" runat="server" Text="Status" class="LabelStyleBold" EnableViewState="false" ></asp:Label>
                                    </td>
                                    <td style="width: 20%; text-align:left">
                                     <asp:Label ID="Label6" runat="server" Text="Cause of Death" class="LabelStyleBold" EnableViewState="false"></asp:Label>
                                    </td>
                            </tr>
                         </table>
                       </asp:Panel>
                  </td>
                </tr>
                <tr >
                    <td valign="top">
                         <div id="divFamilyHistory" class="cssfamilyhistory" runat="server" style="border-style: inset; height:370px;
                            background-color: White; border-width: thin; overflow: scroll; position: relative;
                            top: 0px; left: 0px;">
                        </div>
                    </td>
                </tr>
                <tr >
                    <td valign="top">
                        <asp:Panel ID="pnlGeneralNotes" class="generalnotes" runat="server" GroupingText="General Notes" CssClass="Editabletxtbox" Width="100%"
                            Height="100%"  >
                            <table style="width: 100%; height: 100%;">
                                <tr>
                                    <td class="style3" valign="top">
                                        <asp:Label ID="lblGeneralNotes"  class="notesbottom" runat="server" Text="Notes"  EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td class="style2" valign="top">
                                    <asp:Panel ID="pnlDLC" runat="server" Height="100%" Width="100%" BackColor="White"
                                        Font-Size="Small" Font-Bold="false">
                                        <DLC:DLC ID="DLC" runat="server" TextboxHeight="65px" TextboxWidth="630px" Value="FAMILY HISTORY NOTES"
                                            Enable="True" />
                                            </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style4">
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr valign="top">
                    <td align="right" valign="top" style="height:100%; width:100%;">
                        <asp:Panel ID="pnlSaveandClearAll" runat="server" Width="100%" Height="100%" Font-Size="Small"
                            Font-Bold="false">
                            <table style="width: 100%; height: 100%; font-family: Microsoft Sans Serif;">
                                <tr style="width: 100%; height: 100%;" valign="top" >
                                    <td style="width: 20%;" valign="top">
                                    </td>
                                    <td style="width: 25%;">
                                    </td>
                                    <td style="width: 43%;">
                                    </td>
                                    <td align="right" valign="top" style="width: 4%; height:100%;">
                                        <telerik:RadButton ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click"
                                            OnClientClicked="btnSave_Clicked" AccessKey="S" 
                                            Style="text-align: center; position: static; -moz-border-radius: 3px; -webkit-border-radius: 3px; 
                                            position: relative; height: 31px !important; width: 55px; font-size: 13px !important;"  ButtonType="LinkButton" CssClass="greenbutton teleriknormalbuttonstyle">
                                            <ContentTemplate>
                                                <span>S</span>ave
                                            </ContentTemplate>
                                        </telerik:RadButton>
                                    </td>
                                    <td align="right" valign="top" style="width: 8%; height:100%;">
                                        <telerik:RadButton ID="btnClearAll" runat="server" Text="Clear All"
                                            OnClientClicked="btnClearAll_Clicked" EnableViewState="false" AccessKey="l" 
                                            Style="text-align: center; position: static; -moz-border-radius: 3px; -webkit-border-radius: 3px;
                                            position: relative; height: 30px !important; width: 75px; font-size: 13px !important;" ButtonType="LinkButton" CssClass="redbutton teleriknormalbuttonstyle" >
                                            <ContentTemplate>
                                                C<span >l</span>ear All
                                            </ContentTemplate>
                                        </telerik:RadButton>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </div>
        <%--<div id="divLoading" class="modal" runat="server" style="text-align: center; display: none">
            <asp:Panel ID="Panel3" runat="server">
                <br />
                <br />
                <br />
                <br />
                <center>
                    <asp:Label ID="Label1" Text=" " runat="server"></asp:Label></center>
                <br />
                <img src="Resources/wait.ico" title="[Please wait while the page is loading...]"
                    alt="Loading..." />
                <br />
            </asp:Panel>
        </div>--%>
    </telerik:RadAjaxPanel>
    <asp:PlaceHolder ID="PlaceHolder1" runat="server">
     <script src="JScripts/JSHistoryFamily.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

    <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

    <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

    <script src="JScripts/JSCustomDLC.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

    <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

    </asp:PlaceHolder>
    <asp:Button ID="InvisibleButton" runat="server" OnClick="InvisibleButton_Click" CssClass="displayNone" />
    <asp:HiddenField ID="Hidden1" runat="server" EnableViewState="false" />
     <asp:HiddenField ID="hdnDeleteItem" runat="server" EnableViewState="false" />
     <asp:HiddenField ID="hdnLocalTime" runat="server" EnableViewState="false"/>
     <asp:HiddenField ID="hdnUserRole" runat="server" EnableViewState="false"/>
     <asp:HiddenField ID="hdnLibraryEnable" runat="server" EnableViewState="false"/>
    <asp:Button ID="btnUnCheckCheckBox" runat="server" CssClass="displayNone" OnClick="btnUnCheckCheckBox_Click" />
    <asp:Button ID="btnUnCheckCheckBoxCancel" runat="server" CssClass="displayNone" OnClick="btnUnCheckCheckBoxCancel_Click" />
        <%--<td style="width:1%;" align="left">
                                    <asp:Label ID="lblRelationship" runat="server" Text="Relationship" class="LabelStyleBold"   EnableViewState="false" ></asp:Label>
                                    </td>
                                    
                                    <td style="width: 3%;" align="right">
                                     <asp:Label ID="Label2" runat="server" Text="Select Relationship" class="LabelStyleBold" EnableViewState="false"></asp:Label>
                                    </td>
                                    
                                    <td style="width: 1%; padding-left:58px; ">
                                     <asp:Label ID="Label3" runat="server" Text="Age" class="LabelStyleBold" EnableViewState="false"></asp:Label>
                                    </td>
                                    
                                    <td style="width: 4%; padding-left:60px;text-align:left;padding-right:82px ">
                                     <asp:Label ID="Label4" runat="server" Text="Family Disease" class="LabelStyleBold" EnableViewState="false"></asp:Label>
                                    </td>
                                    
                                    <td style="width: 7%; padding-left:54px;padding-right:32px">
                                     <asp:Label ID="Label5" runat="server" Text="Status" class="LabelStyleBold" EnableViewState="false" ></asp:Label>
                                    </td>
                                    
                                    <td style="width: 9%; padding-left:0px; text-align:left;padding-right:152px">
                                     <asp:Label ID="Label6" runat="server" Text="Cause of Death" class="LabelStyleBold" EnableViewState="false"></asp:Label>
                                    </td>--%>
    </form>
</body>
</html>
