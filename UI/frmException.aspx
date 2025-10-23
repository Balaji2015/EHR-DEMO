<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmException.aspx.cs" EnableEventValidation="false" Inherits="Acurus.Capella.UI.frmException" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/UserControls/CustomDLCNew.ascx" TagName="DLC" TagPrefix="DLC" %>
<%@ Register Src="~/UserControls/PageNavigator.ascx" TagName="PageNavigator" TagPrefix="PageNavigator" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Exception</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <base target="_self" />
    <link href="~/CSS/style.css" rel="stylesheet" type="text/css" />
    <link href="~/CSS/font-awesome.css" rel="stylesheet" type="text/css" />
      <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    
    <style type="text/css">
        .displayNone {
            display: none;
        }
       

        .RadInput {
            vertical-align: middle;
        }

        .RadInput_Default {
            font: 12px "segoe ui",arial,sans-serif;
        }

        .riSingle {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
            position: relative;
            display: inline-block;
            white-space: nowrap;
            text-align: left;
        }


            .riSingle .riTextBox {
                box-sizing: border-box;
                -moz-box-sizing: border-box;
                -ms-box-sizing: border-box;
                -webkit-box-sizing: border-box;
                -khtml-box-sizing: border-box;
            }

        .Panel legend {
            font-weight: bold;
        }

        .modaldiv {
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

        .modal-open {
            overflow: hidden;
        }

        .modal {
            position: fixed;
            top: 0;
            right: 0;
            bottom: 0;
            left: 0;
            z-index: 1050;
            display: none;
            overflow: hidden;
            -webkit-overflow-scrolling: touch;
            outline: 0;
        }

            .modal.fade .modal-dialog {
                -webkit-transition: -webkit-transform .3s ease-out;
                -o-transition: -o-transform .3s ease-out;
                transition: transform .3s ease-out;
                -webkit-transform: translate(0, -25%);
                -ms-transform: translate(0, -25%);
                -o-transform: translate(0, -25%);
                transform: translate(0, -25%);
            }

            .modal.in .modal-dialog {
                -webkit-transform: translate(0, 0);
                -ms-transform: translate(0, 0);
                -o-transform: translate(0, 0);
                transform: translate(0, 0);
            }

        .modal-open .modal {
            overflow-x: hidden;
            overflow-y: auto;
        }

        .modal-dialog {
            position: relative;
            width: auto;
            margin: 10px;
        }

        .modal-content {
            position: relative;
            background-color: #fff;
            -webkit-background-clip: padding-box;
            background-clip: padding-box;
            border: 1px solid #999;
            border: 1px solid rgba(0, 0, 0, .2);
            border-radius: 6px;
            outline: 0;
            -webkit-box-shadow: 0 3px 9px rgba(0, 0, 0, .5);
            box-shadow: 0 3px 9px rgba(0, 0, 0, .5);
        }

        .modal-backdrop {
            position: fixed;
            top: 0;
            right: 0;
            bottom: 0;
            left: 0;
            z-index: 1040;
            background-color: #000;
        }

            .modal-backdrop.fade {
                filter: alpha(opacity=0);
                opacity: 0;
            }

            .modal-backdrop.in {
                filter: alpha(opacity=50);
                opacity: .5;
            }

        .modal-header {
            padding: 15px;
            border-bottom: 1px solid #e5e5e5;
        }

            .modal-header .close {
                margin-top: -2px;
            }

        .modal-title {
            margin: 0;
            line-height: 1.42857143;
        }

        .modal-body {
            position: relative;
            padding: 15px;
        }

        .modal-footer {
            padding: 15px;
            text-align: right;
            border-top: 1px solid #e5e5e5;
        }

            .modal-footer .btn + .btn {
                margin-bottom: 0;
                margin-left: 5px;
            }

            .modal-footer .btn-group .btn + .btn {
                margin-left: -1px;
            }

            .modal-footer .btn-block + .btn-block {
                margin-left: 0;
            }

        .modal-scrollbar-measure {
            position: absolute;
            top: -9999px;
            width: 50px;
            height: 50px;
            overflow: scroll;
        }

        @media (min-width: 768px) {
            .modal-dialog {
                width: 600px;
                margin: 30px auto;
            }

            .modal-content {
                -webkit-box-shadow: 0 5px 15px rgba(0, 0, 0, .5);
                box-shadow: 0 5px 15px rgba(0, 0, 0, .5);
            }

            .modal-sm {
                width: 300px;
            }
        }

        @media (min-width: 992px) {
            .modal-lg {
                width: 900px;
            }
        }

        .modal-header:before,
        .modal-header:after,
        .modal-footer:before,
        .modal-footer:after {
            display: table;
            content: " ";
        }

        .modal-header:after,
        .modal-footer:after {
            clear: both;
        }

        .close {
            float: right;
            font-size: 21px;
            font-weight: bold;
            line-height: 1;
            color: #000;
            text-shadow: 0 1px 0 #fff;
            filter: alpha(opacity=20);
            opacity: .2;
        }

            .close:hover,
            .close:focus {
                color: #000;
                text-decoration: none;
                cursor: pointer;
                filter: alpha(opacity=50);
                opacity: .5;
            }

        button.close {
            -webkit-appearance: none;
            padding: 0;
            cursor: pointer;
            background: transparent;
            border: 0;
        }
    </style>
</head>
<body  onload="LoadException();">
    <form id="form11" runat="server">
        <telerik:RadAjaxPanel ID="pnlAjaxException" runat="server">
            <telerik:RadWindowManager ID="WindowMngr" runat="server">
                <Windows>
                    <telerik:RadWindow ID="MessageWindow" runat="server" Behaviors="Close" Title="Exception"
                        IconUrl="Resources/16_16.ico">
                    </telerik:RadWindow>
                </Windows>
            </telerik:RadWindowManager>
            <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
                <Scripts>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
                </Scripts>
            </telerik:RadScriptManager>
            <div>
                <asp:Panel ID="pnlException" runat="server" CssClass="LabelStyleBold" GroupingText="Create Exception">
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 100%">
                                <asp:Panel ID="pnlPatientDetails" runat="server" CssClass="LabelStyleBold" GroupingText="Patient Details">
                                    <table style="width: 100%; height: 184px;" class="Editabletxtbox">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblEncounterID" Width="103px" runat="server" Font-Bold="False" Text="Encounter ID"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <telerik:RadTextBox ID="txtEncounterID" Style="border-color: black; border-width: 1px;" runat="server" Height="21px"
                                                    ReadOnly="True" Width="120px" CssClass="nonEditabletxtbox"
                                                    AutoCompleteType="Disabled">
                                                </telerik:RadTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblPatientAccount" runat="server" Font-Bold="False" Text="Patient Account #"
                                                    Width="103px"></asp:Label>
                                            </td>
                                            <td>
                                                <telerik:RadTextBox ID="txtPatientAccount" runat="server" Style="border-color: black; border-width: 1px;" Height="18px"
                                                    ReadOnly="True" Width="120px" CssClass="nonEditabletxtbox"
                                                    AutoCompleteType="Disabled">
                                                </telerik:RadTextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblPatientName" Width="80px" runat="server" Font-Bold="False" Text="Patient Name"></asp:Label>
                                            </td>
                                            <td>
                                                <telerik:RadTextBox ID="txtPatientName" runat="server" Height="18px" Style="border-color: black; border-width: 1px;"
                                                    ReadOnly="True" Width="230px" CssClass="nonEditabletxtbox"
                                                    AutoCompleteType="Disabled">
                                                </telerik:RadTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblProviderID" Width="80px" runat="server" Font-Bold="False" Text="Provider ID"></asp:Label>
                                            </td>
                                            <td>
                                                <telerik:RadTextBox ID="txtProviderID" runat="server" Style="border-color: black; border-width: 1px;" Height="18px"
                                                    ReadOnly="True" Width="120px" CssClass="nonEditabletxtbox"
                                                    AutoCompleteType="Disabled">
                                                </telerik:RadTextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblProviderName" runat="server" Font-Bold="False" Text="Provider Name"></asp:Label>
                                            </td>
                                            <td>
                                                <telerik:RadTextBox ID="txtProviderName" runat="server" Style="border-color: black; border-width: 1px;" Height="18px"
                                                    ReadOnly="True" Width="230px" CssClass="nonEditabletxtbox"
                                                    AutoCompleteType="Disabled">
                                                </telerik:RadTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblIssues" runat="server" Width="100px" mand="Yes"
                                                    Text="Issues*"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtIssues" Style="resize: none" runat="server" Height="90px" Width="550px"
                                                    AutoCompleteType="Disabled" TextMode="MultiLine">
                                                </asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="pnlReviewException" runat="server">
                                            <td>
                                                <asp:Label ID="lblFeedback" Width="100px" runat="server" Font-Bold="False" ForeColor="Red"
                                                    Text="Feedback*"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <DLC:DLC ID="DLCfeedback" runat="server" style="position: inherit" TextboxHeight="90px"
                                                    TextboxWidth="550px" Enable="True" Value="Feedback Notes" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100%" align="right" colspan="4">
                                                <telerik:RadButton ID="btnAdd" runat="server" Text="Add" Style="position: static" ButtonType="LinkButton" CssClass="greenbutton"
                                                    OnClick="btnAdd_Click" Width="70px" OnClientClicked="btnAdd_Clicked">
                                                </telerik:RadButton>
                                                &nbsp;
                                           <%-- <telerik:RadButton ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click"
                                                OnClientClicked="Clearall" Width="50px" AutoPostBack="False">
                                            </telerik:RadButton>--%>
                                                <telerik:RadButton ID="btnClear" runat="server" Text="Clear" ButtonType="LinkButton"
                                                    OnClientClicked="Clearall" Width="70px" AutoPostBack="False" CssClass="redbutton">
                                                </telerik:RadButton>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100%">
                                <asp:Panel ID="pnlGrid" runat="server" 
                                    GroupingText="Questions for the Encounter" CssClass="LabelStyleBold">
                                    <telerik:RadGrid ID="grdException" runat="server" AutoGenerateColumns="False"
                                        CellSpacing="0" GridLines="None" OnItemCommand="grdException_ItemCommand"
                                        Height="250px" Width="800px" CssClass="Gridbodystyle">
                                       <HeaderStyle Font-Bold="false" CssClass="Gridheaderstyle" />
                                        <ClientSettings>
                                            <Scrolling AllowScroll="True" ScrollHeight="220px" SaveScrollPosition="false" UseStaticHeaders="True" />
                                        </ClientSettings>
                                        <FilterMenu EnableImageSprites="False">
                                        </FilterMenu>
                                        <MasterTableView>
                                            <CommandItemSettings ExportToPdfText="Export to PDF" />
                                            <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                                            </RowIndicatorColumn>
                                            <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                                            </ExpandCollapseColumn>
                                            <Columns>
                                                <telerik:GridButtonColumn ButtonType="ImageButton" ItemStyle-Width="50px" HeaderStyle-Width="50px"
                                                    CommandArgument="EditRow" CommandName="EditRow" FilterControlAltText="Filter EditRow column"
                                                    HeaderText="Edit" ImageUrl="~/Resources/edit.gif" UniqueName="EditRow" Text="Edit">
                                                    <ItemStyle Width="30px"      CssClass="Gridheaderstyle"/>
                                               
                                                </telerik:GridButtonColumn>
                                                <telerik:GridButtonColumn ButtonType="ImageButton" ItemStyle-Width="50px" HeaderStyle-Width="50px"
                                                    CommandArgument="Del" CommandName="Del" FilterControlAltText="Filter Del column"
                                                    HeaderText="Del" Text="Del" ImageUrl="~/Resources/close_small_pressed.png" UniqueName="Del">
                                                    <ItemStyle Width="30px"      CssClass="Gridheaderstyle"/>
                                                </telerik:GridButtonColumn>
                                                <telerik:GridBoundColumn FilterControlAltText="Filter Issues column" HeaderText="Issues"
                                                    UniqueName="Issues" DataField="Issues">
                                                    <ItemStyle CssClass="Gridbodystyle" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn FilterControlAltText="Filter Feedback column" HeaderText="Feedback"
                                                    UniqueName="Feedback" DataField="Feedback">
                                                    <ItemStyle CssClass="Gridbodystyle" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn FilterControlAltText="Filter EncounterID column" HeaderText="EncounterID"
                                                    UniqueName="EncounterID" Display="false" DataField="EncounterID">
                                                      <ItemStyle CssClass="Gridbodystyle" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn FilterControlAltText="Filter Version column" HeaderText="Version"
                                                    UniqueName="Version" Display="false" DataField="Version">
                                                      <ItemStyle CssClass="Gridbodystyle" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="CreatedBy" FilterControlAltText="Filter CreatedBy column"
                                                    HeaderText="CreatedBy" UniqueName="CreatedBy" Visible="False">
                                                      <ItemStyle CssClass="Gridbodystyle" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="CreatedDateTime" FilterControlAltText="Filter CreatedDateTime column"
                                                    HeaderText="CreatedDateTime" UniqueName="CreatedDateTime" Visible="False">
                                                      <ItemStyle CssClass="Gridbodystyle" />
                                                </telerik:GridBoundColumn>

                                            </Columns>
                                            <EditFormSettings>
                                                <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                                </EditColumn>
                                            </EditFormSettings>
                                        </MasterTableView>
                                        <HeaderStyle Font-Bold="True" />
                                    </telerik:RadGrid>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100%">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 50%">
                                            <PageNavigator:PageNavigator ID="mpnException" runat="server" OnFirst="FirstPageNavigator" class="LabelStyleBold" />
                                        </td>
                                        <td style="width: 50%" align="right">
                                            <telerik:RadButton ID="btnMovetoProvider" runat="server" OnClick="btnMovetoProvider_Click" ButtonType="LinkButton"
                                                Text="Move To Provider" Width="183px" OnClientClicked="btnMovetoProvider" CssClass="bluebutton">
                                            </telerik:RadButton>
                                            &nbsp;&nbsp;&nbsp;
                                        <telerik:RadButton ID="btnClose" runat="server" Text="Close" Width="90px" AutoPostBack="false" ButtonType="LinkButton"
                                            OnClientClicked="btnCloseClicked" CssClass="redbutton">
                                        </telerik:RadButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
            <div id="divLoading" class="modaldiv" runat="server" style="text-align: center; display: none">
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
            <asp:Button ID="InvisibleButton" runat="server" CssClass="displayNone" OnClick="InvisibleButton_Click" />
            <asp:Button ID="btnMessageType" runat="server" Text="Button" CssClass="displayNone"
                OnClientClick="return btnClose_Clicked();" />
            <asp:HiddenField ID="hdnMessageType" runat="server" />
        </telerik:RadAjaxPanel>
        <asp:HiddenField ID="hdnDelExceptionId" runat="server" />
        <asp:HiddenField ID="hdnLocalTime" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnsuccess" runat="server" />
        <asp:HiddenField ID="hdnEnableYesNo" runat="server" />
        <asp:HiddenField ID="hdnSourceScreen" runat="server" />
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">

            <script src="JScripts/jquery-1.11.3.min.js" type="text/javascript"></script>
            <script src="JScripts/bootstrap.min.js" type="text/javascript"></script>
            <script src="JScripts/JSCreateandReviewException.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <%--  <script src="JScripts/JSException.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>--%>

            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSCustomDLC.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
        </asp:PlaceHolder>
    </form>
</body>
</html>
