<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmSendHealthRecord.aspx.cs"
    Inherits="Acurus.Capella.UI.frmSendHealthRecord" ValidateRequest="false" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="aspx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <title>Send Health Record</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
        .style1 {
            border-style: solid;
            border-color: #B6B6B4;
            border-width: 1px;
        }
    </style>
</head>
<body onload="Message();" style="margin: 0px; padding: 0px;">
    <form id="Form1" style="font-family: Microsoft Sans Serif; font-size: 8.5pt;" runat="server">
        <telerik:RadWindowManager ID="WindowMngr" runat="server">
            <Windows>
                <telerik:RadWindow ID="MessageWindo" ShowContentDuringLoad="false" runat="server"
                    Behaviors="Close" IconUrl="Resources/16_16.ico">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <telerik:RadScriptManager ID="RadScriptManager2" runat="server" EnableViewState="false">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
            </Scripts>
        </telerik:RadScriptManager>
        <div style="background-color: White; margin: 0px; padding: 0px;">
            <table style="width: 100%;  margin: 0px; padding: 0px;" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <asp:Panel ID="pnlSend" BackColor="White" Font-Bold="true" runat="server" GroupingText="Send"
                            CssClass="Editabletxtbox">
                            <table style="width: 100%; height: 270px;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFrom" Font-Bold="false" runat="server" Text="From" CssClass="Editabletxtbox"></asp:Label>

                                    </td>
                                    <td colspan="2">
                                        <telerik:RadComboBox ID="cboFrom"  OnClientSelectedIndexChanged="EnableSave" runat="server"
                                            Width="330px" Height="100px"  CssClass="Editabletxtbox" onchange="enableprovider()" />
                                    </td>

                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblSendto" Font-Bold="false" runat="server" Text="Send to" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <div id="divprovider" runat="server" class="style1" style="width: 436px!important;">
                                            <%-- <asp:Panel ID="pnlProvider" runat="server" GroupingText="-" > --%>
                                            <table>
                                                <td>
                                                     <asp:RadioButton ID="rdbtnProvider" GroupName="sendMail" Text="Provider" Font-Bold="false"
                                                        runat="server"  onchange="Providerchange()" Width="90px" CssClass="Editabletxtbox" />
                                                </td>
                                                <td>
                                                    <telerik:RadComboBox ID="cboProvider"  OnClientSelectedIndexChanged="cmboproviderchange" runat="server"
                                                        Width="330px" Height="100px"  CssClass="Editabletxtbox" />
                                                </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblDirectAddress" runat="server" Text="Address" Font-Bold="false" CssClass="Editabletxtbox"> </asp:Label>
                                        <%--<asp:RadioButton ID="rbtDirectAddress" GroupName="sendMail" Text="Direct Address" Font-Bold="false"
                                        runat="server" AutoPostBack="true" OnCheckedChanged="rdbtnDirectAddress_CheckedChanged" />--%></td>
                                    <td>
                                        <telerik:RadTextBox ID="txtDirectAddress" onkeypress="EnableSave();" runat="server"
                                            Height="19px" Width="330px" Enabled="false" CssClass="Editabletxtbox">
                                        </telerik:RadTextBox>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                             
        </div>
      
                            </td>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:RadioButton ID="rdbtnSentTo" GroupName="sendMail" runat="server" Text="Other Recipient"  onchange="DisableControls()"
                                        Font-Bold="false" CssClass="Editabletxtbox" Width="112px" />

                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtSentTo" onkeypress="EnableSave();" runat="server"
                                        Height="19px" Width="240px" CssClass="Editabletxtbox">
                                    </telerik:RadTextBox>
                                </td>
                                <td></td>
                            </tr>
        <tr>
            <td>
                <asp:Label ID="lblSubject" runat="server" Font-Bold="false" Text="Subject" CssClass="Editabletxtbox"></asp:Label>
            </td>
            <td colspan="2">
                <telerik:RadTextBox ID="txtSubject" onkeypress="EnableSave();" runat="server" Width="440px" CssClass="Editabletxtbox" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblMessage" Font-Bold="false" Style="font-weight: normal;" runat="server" Text="Message*" CssClass="Editabletxtbox" mand="Yes"></asp:Label>
            </td>
            <td colspan="2">
                <telerik:RadTextBox ID="txtMessage" onkeypress="EnableSave();" runat="server" Width="440px"
                    Height="95px" TextMode="MultiLine" CssClass="Editabletxtbox">
                </telerik:RadTextBox>
            </td>
            <td>&nbsp;
            </td>
        </tr>
        <tr>
            <td></td>
            <td colspan="3">
                <asp:Panel ID="pnlAttachment" runat="server" Width="100%" CssClass="Editabletxtbox">
                    <table>
                         <tr>
                            <td style="width: 360px">
                                
                                <asp:RadioButton ID="rdbEncrypted" GroupName="Encrypt" Text="Encrypted" Font-Bold="false" Checked="true"
                                    runat="server" Width="90px" CssClass="Editabletxtbox"  onchange="EnableSave();"/>
                            
                                <asp:RadioButton ID="rdbUnEncrypted" GroupName="Encrypt" Text="Unencrypted" Font-Bold="false"
                                    runat="server"  Width="95px" CssClass="Editabletxtbox" onchange="EnableSave();" />

                            
                                <telerik:RadButton ID="btnSend" runat="server" Text="Send" Width="75px" OnClientClicked="sendvalidation"
                                    OnClick="btnSend_Click" ButtonType="LinkButton" CssClass="greenbutton" />
                            </td>
                           <%-- <td>
                                <telerik:RadButton ID="btnCancel" runat="server" Text="Cancel" AutoPostBack="false"
                                    Width="75px" OnClientClicked="OnSendCancel" ButtonType="LinkButton" CssClass="redbutton" />
                               </td>--%>
                        </tr>
                        <tr>
                                    <td>
                                        <span id="lblattachfile" runat="server" class="Editabletxtbox">Attachments <i class="icon-ok-sign" style="color: blue; ">(File Formats supported:*.TIFF , *.PDF , *.PNG , *.JPG , *.GIF , *.TXT , *.DOC , *.XML , *.DOCX , *.ZIP)</i></span>
                                    </td>
                                </tr>
                        <tr>
                                    <td>
                                       <asp:Label ID="lblforwardattachment" runat="server"></asp:Label>
                                    </td>
                                </tr>

                        <tr>
                            <td>
                                
                                <div style="height: 90px; width: 100%; overflow: auto;">
                                      <telerik:RadAsyncUpload ID="UploadImage" runat="server" UploadedFilesRendering="BelowFileInput"
                                        MultipleFileSelection="Automatic"
                                        InitialFileInputsCount="0"   OnClientFileSelected="OnFileSelected"
                        OnClientFileUploadFailed="OnFileUploadFailed"
                        OnClientFilesUploaded="OnFilesUploaded"
                        OnClientProgressUpdating="OnProgressUpdating"
                        OnClientFileUploaded="OnFileUploaded"
                        OnClientFileUploadRemoved="OnFileUploadRemoved" OnClientFileUploadRemoving="OnFileRemove" >
                                        <Localization Select="Browse" />
                                        <FileFilters>
                                            <telerik:FileFilter Description="Documents(pdf;tif;png;jpg;jpeg;gif;txt;doc;docx;xml;zip)" Extensions="pdf,txt,tif,png,jpg,jpeg,gif,doc,docx,xml,zip" />
                                        </FileFilters>
                                    </telerik:RadAsyncUpload>
                                </div>
                            </td>
                            </tr>
                        <tr>
                            <td style="width: 360px">
                                <asp:Label ID="lblAttachment" runat="server" Font-Bold="false" Style="font-family: Microsoft Sans Serif; font-size: 8.5pt;"
                                    ForeColor="#000099" CssClass="Editabletxtbox"></asp:Label><br />
                                 <asp:Label ID="lblAttachment1" runat="server" Font-Bold="false" Style="font-family: Microsoft Sans Serif; font-size: 8.5pt;"
                                    ForeColor="#000099" CssClass="Editabletxtbox"></asp:Label><br />
                            </td>
                        </tr>
                       
                    </table>
                </asp:Panel>
            </td>
        </tr>
        </table>
                    </asp:Panel>
                    
                </td>
            </tr>
           <%-- <tr>
                <td>--%>
        <%--<asp:Panel ID ="pnlSentHistory" runat="server" GroupingText ="Sent History" BackColor="White" Height="180px" Width="530px" Font-Bold="true">
                    <telerik:RadGrid ID="grdSentHistory" runat="server" AutoGenerateColumns="False" CellSpacing="0"
                GridLines="None" ClientSettings-EnablePostBackOnRowClick="false" Skin="Vista"
                Width="530px" Height="392px">
                <ClientSettings EnablePostBackOnRowClick="false">
                    <Resizing AllowColumnResize="false" />
                    <Scrolling AllowScroll="True" UseStaticHeaders="true"/>
                    <Resizing AllowResizeToFit="true" />
                </ClientSettings>
                <MasterTableView NoMasterRecordsText="No Records Found" ShowHeadersWhenNoRecords="true" TableLayout="Fixed">
                    <Columns>
                        <telerik:GridTemplateColumn FilterControlAltText="Filter SentDate column" HeaderText="SentDate" DataField="SentDate"
                            UniqueName="SentDate">
                            <HeaderStyle Width="140px"/>
                            <ItemStyle Width="140px" HorizontalAlign ="Center"/>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn FilterControlAltText="Filter SentTo column" HeaderText="SentTo"
                            UniqueName="SentTo" DataField="SentTo">
                            <HeaderStyle Width="180px" />
                            <ItemStyle Width="180px" HorizontalAlign ="Center"/>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn FilterControlAltText="Filter Subject column"
                            HeaderText="Subject" UniqueName="Subject" DataField="Subject">
                             <HeaderStyle Width="200px" />
                            <ItemStyle Width="200px" />
                        </telerik:GridBoundColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
                    </asp:Panel>--%>
        <%--     </td>
            </tr>--%>
        </table>
    </div>
    <asp:Button ID="btnMessageType" runat="server" Text="Button" Style="display: none" OnClientClick="return OnSendCancel();" />
        <asp:HiddenField ID="hdnMailtype" runat="server" />
        <asp:HiddenField ID="hdnMessageType" runat="server" Value="" />
        <asp:HiddenField ID="hdnType" runat="server" Value="" />
        <asp:HiddenField ID="hdnHumanID" runat="server" />
        <asp:HiddenField ID="hdnLocalTime" EnableViewState="false" runat="server" />
        <asp:HiddenField ID="hdnScreenMode" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnRole" runat="server" EnableViewState="false" />
          <asp:HiddenField ID="Hdnfilename" runat="server" EnableViewState="false" />
         <asp:HiddenField ID="hdnIsSSOLogin" runat="server" EnableViewState="false" />
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <script src="JScripts/JSPatientPortal.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSMailBox.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSClinicalSummary.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

        </asp:PlaceHolder>
    </form>
</body>
</html>
