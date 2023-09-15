<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmHistorySurgical.aspx.cs"
    Inherits="Acurus.Capella.UI.frmHistorySurgical" EnableEventValidation="false"  ValidateRequest="false" %>

<%@ Register Src="~/UserControls/CustomDLCNew.ascx" TagName="DLC" TagPrefix="DLC" %>
<%@ Register Src="~/UserControls/CustomDateTimePicker.ascx" TagName="CustomDatePicker"
    TagPrefix="UC" %>
<%@ Register Src="~/UserControls/PageNavigator.ascx" TagName="PageNavigator" TagPrefix="PageNavigator" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Surgical History</title>
  
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>

    <style type="text/css">
        .style124 {
            height: 23px;
            width: 3%;
        }

        .underline {
            text-decoration: underline;
        }

        p:first-letter {
            text-decoration: underline;
        }

        .style130 {
            height: 23px;
            width: 100px;
        }

        .displayNone {
            display: none;
        }

        .RadPicker {
            vertical-align: middle;
        }

            .RadPicker .rcTable {
                table-layout: auto;
            }

        .RadPicker_Default .rcCalPopup {
            background-position: 0 0;
        }

        .RadPicker_Default .rcCalPopup {
            background-image: url( 'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Calendar.sprite.gif' );
        }

        .RadPicker .rcCalPopup {
            display: block;
            overflow: hidden;
            width: 22px;
            height: 22px;
            background-color: transparent;
            background-repeat: no-repeat;
            text-indent: -2222px;
            text-align: center;
        }

        .style136 {
            height: 23px;
        }

        .style139 {
            height: 23px;
            width: 72px;
        }

        .style142 {
            width: 15%;
        }

        .style143 {
            width: 14%;
            font-family:Open Sans;
            font-size:14px;
        }

        

        .RadGrid_Default {
            border: 1px solid #828282;
            background: #fff;
            color: #333;
        }

           

        .style155 {
            width: 342px;
        }

        #form1 {
            width: 926px;
        }

        .style156 {
            height: 92px;
            
        }

        .modal {
            position: fixed;
            top: 0;
            left: 0;
            background-color: White;
            z-index: 99;
            -moz-opacity: 0.8;
            min-height: 100%;
            width: 100%;
        }

        .style157 {
            width: 206px;
        }

        .style158 {
            width: 67px;
        }

        .underline {
            text-decoration: underline;
        }

        body {
            zoom: 1.0 !important;
            -moz-transform: scale(1) !important;
            -moz-transform-origin: 0 0 !important;
        }
    </style>
    
    <%--<script type="text/javascript">
     $('button[accesskey]').each( //selects only those a elements with an accesskey attribute
    function(){
    alert('sfg');
        var aKey = $(this).attr('accesskey'); // finds the accesskey value of the current a
        var text = $(this).text(); // finds the text of the current a
        var newHTML = text.replace(aKey,'<span class="access">' + aKey + '</span>');
        // creates the new html having wrapped the accesskey character with a span
        $(this).html(newHTML); // sets the new html of the current link with the above newHTML variable
    });
     </script>--%>
    <link href="~/CSS/style.css" rel="stylesheet" type="text/css" />
    <link href="~/CSS/font-awesome.css" rel="stylesheet" type="text/css" />
    <link href="~/CSS/CommonStyle.css" rel="Stylesheet" type="text/css" />

    <script type="text/javascript" id="telerikClientEvents1">
        //<![CDATA[


        //]]>
    </script>

    <script type="text/javascript" id="telerikClientEvents2">
        //<![CDATA[


        //]]>
    </script>

    <script type="text/javascript" id="telerikClientEvents3">
        //<![CDATA[


        //]]>
    </script>

</head>
<body onload="HistorySurgical_Load()">
    <form id="form1" runat="server" style="background-color: #bfdbfd ; height: 100%; margin-bottom: 0px; width: 100%;  font-family:Open Sans;  font-size:14px;">
        <telerik:RadWindowManager ID="WindowMngr" runat="server" EnableViewState="false" IconUrl="Resources/16_16.ico" >
            <Windows>
                <telerik:RadWindow ID="MessageWindow" runat="server" Behaviors="Close" Title="Surgical History"
                    IconUrl="Resources/16_16.ico" >
                </telerik:RadWindow>
                <telerik:RadWindow ID="MessageWindowLibrary" runat="server" Behaviors="Close" OnClientClose="ReloadOnClientClose"
                    IconUrl="Resources/16_16.ico">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" EnableViewState="false">
            </telerik:RadAjaxManager>
            <div>
                <asp:Panel ID="pnlHistory" runat="server" BorderWidth="1px" BackColor="White" Height="99%"
                    Width="100%">
                    <div style="height: 52%">
                        <asp:Panel ID="SocialHistory" BorderWidth="1px" GroupingText="Surgical History" runat="server"
                            Style="margin-left: 3px; margin-right: 3px; margin-top: 3px; margin-bottom: 3px;"
                            CssClass="Editabletxtbox" Font-Size="Small" BackColor="White" Font-Bold="true">
                            <table style="height: 225px">
                                <tr>
                                    <td class="style142">
                                        <asp:Label ID="lblSurgeryName" runat="server" Font-Bold="False" Text="Surgery Name" CssClass="Editabletxtbox"></asp:Label>
                                        
                                    </td>
                                    <td class="style143" align="right">
                                        <asp:ImageButton ID="pbDatabase" runat="server"
                                            ImageUrl="~/Resources/Database%20Inactive.jpg" OnClientClick="openAddorUpdate();"
                                            EnableViewState="False" />
                                    </td>
                                    <td></td>
                                    <td class="style157"></td>
                                    <td class="style130"></td>
                                    <td class="style136"></td>
                                    <td class="style139"></td>
                                    <td class="style124"></td>
                                </tr>
                                <tr>
                                    <td colspan="3" rowspan="3">
                                        <asp:Panel ID="pnlSurgeryNameLists" runat="server" Height="224px" Width="100%" >
                                            <telerik:RadListBox ID="lstSurgeryName" runat="server" Height="98%" Width="100%"
                                                AutoPostBack="false" OnClientSelectedIndexChanged="lstSurgeryName_SelectedIndexChanged"  
                                                CssClass="Editabletxtbox"
                                                Font-Bold="False">
                                                <ButtonSettings TransferButtons="All" />
                                            </telerik:RadListBox>
                                        </asp:Panel>
                                    </td>
                                    <td style="padding-left:26px;">
                                        <asp:Label ID="lblSurName" runat="server" Font-Bold="False" EnableViewState="false" Text="Surgery Name" Width="90px"
                                           CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                          <td class="style157">
                                        <div id="txtdiv" onkeydown="keyDownEvent(this)" runat="server">
                                        <telerik:RadTextBox ID="txtSurgeryName" runat="server" EnableViewState="false" MaxLength="255" Style="margin-right: 0px">
                                            <DisabledStyle Resize="None" />
                                            <InvalidStyle Resize="None" />
                                            <HoveredStyle Resize="None" />
                                            <ReadOnlyStyle Resize="None" />
                                            <EmptyMessageStyle Resize="None" />
                                            <ClientEvents OnKeyPress="txtSurgeryName_OnKeyPress" />
                                            <FocusedStyle Resize="None" />
                                            <EnabledStyle Resize="None" />
                                        </telerik:RadTextBox>
                                        </div>
                                        
                                    </td>
                                    <td align="center" width="40px">
                                        <asp:Label ID="lblDateOfSurgery" runat="server" EnableViewState="False"
                                            Font-Bold="False" Text="Date of Surgery"
                                            Width="80px" CssClass="Editabletxtbox"></asp:Label>

                                    </td>
                                    <td>
                                        <UC:CustomDatePicker ID="dtpDateOfSurgery" runat="server" />
                                    </td>
                                    <td align="left" colspan="2">
                                        <asp:Label ID="lblExDateFormate" runat="server" EnableViewState="false" Text="(Eg:2013-Jan-22)" Width="150px"
                                            Font-Bold="False" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="comboboxchange"  style="padding-left:26px;">
                                        <asp:Label ID="lblSurgeryNotes" runat="server" EnableViewState="false" Font-Bold="False" Text="Surgery Notes"  
                                            Width="90px" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td colspan="5" class="style156">
                                        <div id="dlcDiv" onkeydown="keyDownEvent(this)" runat="server">
                                        <asp:Panel ID="pnlDLC" runat="server" Height="100%" Width="100%" BackColor="White" 
                                            Font-Size="Small" Font-Bold="false">
                                            <DLC:DLC ID="DLC" runat="server" TextboxHeight="80px" TextboxWidth="500px" Enable="True" TextboxMaxLength="2000"
                                                Value="Surgery Notes" />
                                        </asp:Panel>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td class="style157"></td>
                                    <td></td>
                                    <td colspan="3">
                                        <asp:Panel ID="pnlSaveClearAll" runat="server" Width="100%" Height="100%">
                                            <table runat="server" width="100%">
                                                <tr style="width: 100%; height: 100%;">
                                                    <td style="width: 45%;">

                                                        <telerik:RadButton ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_Click"
                                                            OnClientClicked="SurgicalSave" AccessKey="A" 
                                                            Style="text-align: center;float:right; position: static; -moz-border-radius: 3px; -webkit-border-radius: 3px; position: relative; padding: 4px 12px !important; height: 28px !important; font-size: 13px !important; "  ButtonType="LinkButton" CssClass="greenbutton teleriknormalbuttonstyle" >
                                                            <ContentTemplate>
                                                                <span id="SpanAdd" runat="server" >A</span><span id="SpanAdditionalword" runat="server">dd</span>

                                                            </ContentTemplate>


                                                        </telerik:RadButton>

                                                    </td>
                                                    <td style="width: 13%;">
                                                        <telerik:RadButton ID="btnClearAll" runat="server" Text="Clear All"
                                                            AutoPostBack="false" OnClientClicked="btnClearAll_Clicked" AccessKey="C"
                                                            Style="text-align: center; position: static; -moz-border-radius: 3px; -webkit-border-radius: 3px; position: relative;  padding: 4px 12px !important; width: 72px; height: 27px !important;  font-size: 13px !important;"  ButtonType="LinkButton" CssClass="redbutton teleriknormalbuttonstyle">
                                                            <ContentTemplate>
                                                                <span id="SpanClear" runat="server" >C</span><span id="SpanClearAdditional" runat="server">lear All</span>
                                                            </ContentTemplate>
                                                        </telerik:RadButton>

                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                    <div  class="styleheight ">
                        <asp:Panel ID="pnlSurgeryDetails" runat="server" BorderWidth="1px" GroupingText="Surgery History Details"
                            Height="91%" Width="99%" Style="margin-left: 1px; margin-right: 1px; margin-top: 1px; margin-bottom: 1px;"
                            BackColor="White"  Font-Bold="true" CssClass="Editabletxtbox">
                            <telerik:RadGrid ID="grdSurgeryHistoryDetails" runat="server" AutoGenerateColumns="false"
                                EnableTheming="False" CellSpacing="0" GridLines="Both" OnItemCommand="grdSurgeryHistoryDetails_ItemCommand"
                                Width="100%"  Height="230px"   CssClass="Gridbodystyle" >
                                <HeaderStyle Font-Bold="true" CssClass="Gridheaderstyle"  />
                                <ClientSettings EnablePostBackOnRowClick="false">
                                    <Selecting AllowRowSelect="true" />

                                    <ClientEvents OnCommand="grdSurgeryHistoryDetails_OnCommand" />

                                    <Scrolling AllowScroll="True" ScrollHeight="270px" UseStaticHeaders="True" />

                                </ClientSettings>
                                <MasterTableView CssClass="Gridbodystyle" AutoGenerateColumns="false">
                                    <Columns>
                                        <telerik:GridButtonColumn HeaderText="Edit" HeaderStyle-Width="50px" ButtonType="ImageButton"
                                            CommandName="Edt" Text="Edit" UniqueName="EditRows" ImageUrl="~/Resources/edit.gif">
                                            <HeaderStyle Width="50px"  BackColor="#3aa04a" CssClass="Gridheaderstyle" VerticalAlign="Middle" />
                                            <ItemStyle BorderColor="#CCFFFF"  BorderStyle="Dotted"  />
                                        </telerik:GridButtonColumn>
                                        <telerik:GridButtonColumn ConfirmText="Are you sure you want to delete this Surgical History?"
                                            ConfirmDialogType="RadWindow" ConfirmTitle="Surgical History" HeaderText="Delete" HeaderStyle-Width="50px"
                                            ButtonType="ImageButton" CommandName="DeleteRows" Text="Delete" UniqueName="DeleteRows" ConfirmDialogHeight="155px"
                                            ImageUrl="~/Resources/close_small_pressed.png">
                                            <HeaderStyle Width="50px" />
                                            <ItemStyle  BorderStyle="Dotted"  />
                                        </telerik:GridButtonColumn>
                                        <telerik:GridBoundColumn DataField="DateOfSurgery" FilterControlAltText="Filter DateOfSurgery column"
                                            HeaderText="Date Of Surgery" UniqueName="DateOfSurgery" Resizable="False" Visible="true" >
                                            <HeaderStyle Width="18%" />
                                            <ItemStyle Font-Bold="false"  BorderStyle="Dotted" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="SurgeryName" FilterControlAltText="Filter SurgeryName column"
                                            HeaderText="Surgery Name" UniqueName="SurgeryName" Resizable="False" Visible="true" >
                                            <HeaderStyle Width="30%"/>
                                            <ItemStyle Font-Bold="false"   BorderStyle="Dotted" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="SurgeryNotes" FilterControlAltText="Filter SurgeryNotes column"
                                            HeaderText="Surgery Notes" UniqueName="SurgeryNotes" Resizable="False" Visible="true" >
                                            <HeaderStyle Width="40%" />
                                            <ItemStyle Font-Bold="false"   BorderStyle="Dotted" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="ID" FilterControlAltText="Filter ID column" HeaderText="ID"
                                            UniqueName="ID" Resizable="False" Display="false" Visible="true"  >
                                            <HeaderStyle Width="5%"  BorderStyle="Dotted" />
                                        </telerik:GridBoundColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </asp:Panel>
                        <%--<PageNavigator:PageNavigator ID="mpnHistorySurgical" runat="server" OnFirst="FirstPageNavigator"
                        Visible="true" />--%>
                    </div>
                </asp:Panel>
                <telerik:RadScriptManager ID="RadScriptManager1" runat="server" EnableViewState="false">
                    <Scripts>
                        <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                        <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                        <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
                    </Scripts>
                </telerik:RadScriptManager>
            </div>
            <%--<div id="divLoading" class="modal" runat="server" style="text-align: center; display: none">
            <asp:Panel ID="Panel1" runat="server">
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
            <%--<asp:Button ID="InvisibleClearAllButton" runat="server" CssClass="displayNone" OnClick="InvisibleClearAllButton_Click" />--%>
        </telerik:RadAjaxPanel>
        <asp:HiddenField ID="hdnDelSurgicalId" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="Hidden1" runat="server" EnableViewState="false" />

        <asp:HiddenField ID="Hiddenupdate" runat="server" />
        <asp:HiddenField ID="hdnLocalTime" runat="server" EnableViewState="false" />
        <asp:Button ID="InvisibleButton" runat="server" CssClass="displayNone" OnClick="InvisibleButton_Click" />
        <asp:Button ID="LibraryButton" runat="server" CssClass="displayNone" OnClick="LibraryButton_Click" />
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <script src="JScripts/JSHistorySurgical.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSCustomDLC.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSCustomDateTimePicker.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>


        </asp:PlaceHolder>
    </form>
    <script type="text/javascript">
        $('button[accesskey]').each( //selects only those a elements with an accesskey attribute
   function () {
       alert('sfg');
       var aKey = $(this).attr('accesskey'); // finds the accesskey value of the current a
       var text = $(this).text(); // finds the text of the current a
       var newHTML = text.replace(aKey, '<span class="access">' + aKey + '</span>');
       // creates the new html having wrapped the accesskey character with a span
       $(this).html(newHTML); // sets the new html of the current link with the above newHTML variable
   });
        //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        //function EndRequestHandler(sender, args) {
        //    if (args.get_error() != undefined) {
        //        args.set_errorHandled(true);
        //    }
        //}
        Telerik.Web.UI.RadListBox.prototype.saveClientState = function () {
            return "{" +
                        "\"isEnabled\":" + this._enabled +
                        ",\"logEntries\":" + this._logEntriesJson +
                       ",\"selectedIndices\":" + this._selectedIndicesJson +
                       ",\"checkedIndices\":" + this._checkedIndicesJson +
                       ",\"scrollPosition\":" + Math.round(this._scrollPosition) +
                   "}";
        }
    </script>
</body>
</html>
