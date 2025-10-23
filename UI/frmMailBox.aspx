<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmMailBox.aspx.cs" Inherits="Acurus.Capella.UI.frmMailBox" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Mail Box</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
        .style1 {
            height: 35px;
        }

        .style2 {
            height: 35px;
            width: 138px;
        }

        .style3 {
            height: 35px;
            width: 192px;
        }
    </style>
    <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function OpenMail(sender, eventArgs) {
            var index = eventArgs._itemIndexHierarchical;
            var grid = $find('grdMailBox');
            var MasterTable = grid.get_masterTableView();
            var row = MasterTable.get_dataItems()[index];
            var cellFrom = MasterTable.getCellByColumnUniqueName(row, "From");
            var cellSubject = MasterTable.getCellByColumnUniqueName(row, "Subject");
            var cellDate = MasterTable.getCellByColumnUniqueName(row, "Date");
            var cellToAddress = MasterTable.getCellByColumnUniqueName(row, "ToAddress");
            var cellBody = MasterTable.getCellByColumnUniqueName(row, "Body");
            var cellDateTime = MasterTable.getCellByColumnUniqueName(row, "DateSent");

            var MessageNumber = MasterTable.getCellByColumnUniqueName(row, "MessageNumber");
            var FileName = MasterTable.getCellByColumnUniqueName(row, "Filename");
            var txtMessage = "\r\n";
            //txtMessage +="\r\n";
            txtMessage += "\r\n---------------------------------------------------------";

            if (document.getElementById('rdbtnSentitems').checked) {
                txtMessage += "\r\n" + "From :  " + cellToAddress.innerHTML;
                txtMessage += "\r\n" + "To :  " + cellFrom.innerHTML;
            }
            if (document.getElementById('rdbtnInbox').checked) {
                txtMessage += "\r\n" + "From :  " + cellFrom.innerHTML;
                txtMessage += "\r\n" + "To :  " + cellToAddress.innerHTML;
            }



            txtMessage += "\r\n" + "Message Date&Time :  " + cellDateTime.innerHTML;
            txtMessage += "\r\n" + "Subject :  " + cellSubject.innerHTML.replace("&nbsp;", "\r\n");
            
                txtMessage += "\r\n" + "Body :  " + cellBody.innerHTML.replace("&nbsp;", "\r\n");
           

            var PatientID = document.getElementById('hdnPatientID').value;
            var EmailID = document.getElementById('hdnEmailID').value;
            var EncounterID = document.getElementById('hdnEncounterID').value;
            var Role = document.getElementById("hdnRole").value;
            var pateintporatal = document.getElementById("hdnIsPatientPortal").value;
            //PageMethods.CreateSessionViaJavascript(txtMessage);
            var code = window.btoa(unescape(encodeURIComponent(txtMessage)));
       
            var obj = new Array();
            obj.push("PatientID=" + PatientID);
            obj.push("EmailID=" + EmailID);
            obj.push("EncounterID=" + EncounterID);
            if (MessageNumber.innerHTML.trim() == "0") {
                obj.push("BodyMessage=" + code);
            }
            obj.push("FileName=" + FileName.innerHTML);
            obj.push("Role=" + Role)
            obj.push("IS_Patient_Portal=" + pateintporatal);
            obj.push("MsgId=" + MessageNumber.innerHTML);
            setTimeout(function () { GetRadWindow().BrowserWindow.openModal("frmMailMessage.aspx", 485, 650, obj, "MessageWindow"); }, 0);

        }
        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }

    </script>
</head>
<body >
    <form id="form1" style="font-family: Microsoft Sans Serif; font-size: 8.5pt; position: static;"
        runat="server">
        <div style="height: 392px; width: 850px">
            <table style="width: 100%; height: 386px;">
                <tr>
                    <td class="style2">
                        <asp:RadioButton ID="rdbtnInbox" GroupName="MailBox" Text="Inbox" AutoPostBack="true"
                            runat="server" class="Editabletxtbox" OnCheckedChanged="rdbtnInbox_CheckedChanged" />
                    </td>
                    <td class="style3">
                        <asp:RadioButton ID="rdbtnSentitems" Text="Sent items" GroupName="MailBox" runat="server"
                            AutoPostBack="true" class="Editabletxtbox" OnCheckedChanged="rdbtnSentitems_CheckedChanged" />
                    </td>
                    <td class="style1">
                        <asp:RadioButton ID="rdbtnCompose" Text="Compose" GroupName="MailBox" runat="server"
                            AutoPostBack="true" class="Editabletxtbox" OnCheckedChanged="rdbtnCompose_CheckedChanged" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">&nbsp;
                    <asp:Panel ID="pnlMailBox" runat="server" Height="500px"
                        Style="margin-top: 0px;">
                        <%-- <asp:GridView ID="gvEmails" runat="server" AutoGenerateColumns="false">
                            <Columns>
                                <asp:BoundField HeaderText="From" DataField="From" />
                                <asp:BoundField HeaderText="Subject"  DataField="Subject" />
                                <asp:BoundField HeaderText="Date" DataField="DateSent" />
                                  <asp:BoundField HeaderText="Message" DataField="Message" />
                            </Columns>
                        </asp:GridView>--%>

                        <telerik:RadGrid ID="grdMailBox" GridLines="Both" runat="server" class="Editabletxtbox" AutoGenerateColumns="False"
                            CellSpacing="0" Width="100%" CssClass="Gridbodystyle" Height="450px">
                            <FilterMenu EnableImageSprites="False">
                            </FilterMenu>
                            <HeaderStyle Font-Bold="true" CssClass="Gridheaderstyle" />
                            <ClientSettings>
                                <Scrolling AllowScroll="True" UseStaticHeaders="true" />
                                <Selecting AllowRowSelect="true" />
                                <ClientEvents OnRowDblClick="OpenMail" />
                            
                                <%-- <ClientEvents OnRowDblClick="OnRowDblClick" OnColumnHidden="#e1e3e4" OnRowClick="OnRowClick" />--%>
                            </ClientSettings>

                            <MasterTableView>
                                <CommandItemSettings ExportToPdfText="Export to PDF" />
                                <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                                </RowIndicatorColumn>
                                <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                                </ExpandCollapseColumn>
                                <Columns>
                                    <telerik:GridBoundColumn DataField="From" ItemStyle-CssClass="Editabletxtbox" FilterControlAltText="Filter From column"
                                        HeaderText="From" UniqueName="From" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                                        <%--<HeaderStyle Width="200px"  />
                                        <ItemStyle Width="230px" />--%>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Subject" ItemStyle-CssClass="Editabletxtbox" FilterControlAltText="Filter Subject column"
                                        HeaderText="Subject" UniqueName="Subject" HeaderStyle-Width="300px" ItemStyle-Width="300px">
                                        <%--<HeaderStyle Width="250px"  />
                                        <ItemStyle Width="300px" />--%>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Date" ItemStyle-CssClass="Editabletxtbox" FilterControlAltText="Filter Date column"
                                        HeaderText="Date" UniqueName="Date">
                                        <HeaderStyle Width="150px" />
                                        <ItemStyle Width="70px" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="ToAddress" FilterControlAltText="Filter ToAddress column" ItemStyle-CssClass="Editabletxtbox"
                                        HeaderText="ToAddress" Display="false" UniqueName="ToAddress">
                                        <HeaderStyle Width="150px" />
                                        <ItemStyle Width="150px" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Message" FilterControlAltText="Filter Body column" ItemStyle-CssClass="Editabletxtbox"
                                        HeaderText="Body" Display="false" UniqueName="Body">
                                        <HeaderStyle Width="150px" />
                                        <ItemStyle Width="150px" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="DateSent" ItemStyle-CssClass="Editabletxtbox" FilterControlAltText="Filter DateTime column"
                                        HeaderText="DateTime" Display="false" UniqueName="DateSent">
                                        <HeaderStyle Width="150px" />
                                        <ItemStyle Width="150px" />
                                    </telerik:GridBoundColumn>
                                      <telerik:GridBoundColumn DataField="MsgId" ItemStyle-CssClass="Editabletxtbox" FilterControlAltText="Filter DateTime column"
                                        HeaderText="MsgId" Display="false" UniqueName="MsgId">
                                        <HeaderStyle Width="150px" />
                                        <ItemStyle Width="150px" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="MessageNumber" ItemStyle-CssClass="Editabletxtbox" FilterControlAltText="Filter DateTime column"
                                        HeaderText="MessageNumber" Display="false" UniqueName="MessageNumber">
                                        <HeaderStyle Width="150px" />
                                        <ItemStyle Width="150px" />
                                    </telerik:GridBoundColumn>
                                     <telerik:GridBoundColumn DataField="Filename" ItemStyle-CssClass="Editabletxtbox" FilterControlAltText="Filter DateTime column"
                                        HeaderText="Filename" Display="false" UniqueName="Filename">
                                        <HeaderStyle Width="150px" />
                                        <ItemStyle Width="150px" />
                                    </telerik:GridBoundColumn>
                                </Columns>
                                <EditFormSettings>
                                    <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                    </EditColumn>
                                </EditFormSettings>
                            </MasterTableView><AlternatingItemStyle BorderStyle="None" />
                        </telerik:RadGrid>
                        <iframe id="ifrmCompose" runat="server" src="" visible="false" frameborder="0"></iframe>
                    </asp:Panel>
                    </td>
                </tr>
            </table>
  <telerik:RadWindowManager EnableViewState="false" ID="WindowMngr" runat="server">
                <Windows>
                    <telerik:RadWindow ID="MessageWindo" IconUrl="~/Resources/16_16.ico" Height="380px"
                        Width="650px" VisibleStatusbar="false" Behaviors="Close" Title="Message Detail"
                        Style="display: none; position: absolute; text-align: left;" Overlay="true" Modal="true"
                        runat="server">
                        <ContentTemplate>
                        </ContentTemplate>
                    </telerik:RadWindow>
                </Windows>
            </telerik:RadWindowManager>
            <telerik:RadScriptManager ID="RadScriptManager1" EnableViewState="false" runat="server" ScriptMode="Release" LoadScriptsBeforeUI="true" EnablePageMethods="true">
            </telerik:RadScriptManager>
        </div>
        <asp:Button ID="btnMessageType" runat="server" Text="Button" Style="display: none" OnClientClick="return btnCancel_Clicked();" />
        <asp:HiddenField ID="hdnToEnableSave" runat="server" />
        <asp:HiddenField ID="hdnMessageType" runat="server" Value="" />
        <asp:HiddenField ID="hdnPatientID" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnEmailID" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnEncounterID" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnRole" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnInboxCnt" runat="server" EnableViewState="false" /><%--BugID:48547--%>
         <asp:HiddenField ID="hdnIsPatientPortal" runat="server" EnableViewState="false" />
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
            <script src="JScripts/JSMailBox.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
        </asp:PlaceHolder>
    </form>
    <p>
        &nbsp;
    </p>
</body>
</html>