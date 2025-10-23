<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmRoomIn.aspx.cs" Inherits="Acurus.Capella.UI.frmRoomIn" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <style type="text/css">
        *
        {
            font-size: small;
            
        }
        #tblPatientInformation tr td:nth-of-type(even)
        {
            border: solid 1px black;
        }
        .InlineStyle
        {
            display: inline;
        }
        #tblPatientInformation
        {
        }
      
    </style>
    <title>Room in</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <base target="_self" />
      <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server" style="  height: 197px;"> 
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server" EnableViewState="false">
          <Scripts>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js">
                    </asp:ScriptReference>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js">
                    </asp:ScriptReference>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js">
                    </asp:ScriptReference>
                </Scripts>
    </telerik:RadScriptManager>
    <asp:Panel runat="server" Height="174px">
        <asp:Panel ID="pnlPatientInformation" runat="server" class="LabelStyleBold" 
            GroupingText="Patient Information" BackColor="White">
     
                <table id="tblPatientInformation" runat="server" width="100%">
                    <tr>
                        <td style="width: 20%">
                            <asp:Label ID="lblPhyName" runat="server" Text="Physician Name" class="Editabletxtbox"></asp:Label>
                        </td>
                        <td style="width: 30%;" class="nonEditabletxtbox">
                            <asp:Label ID="lblPhysicianName" runat="server"  class="nonEditabletxtbox"></asp:Label>
                        </td>
                        <td style="width: 15%">
                            <asp:Label ID="lblPatAccountNumber" runat="server" Text="Patient Account #" class="Editabletxtbox"></asp:Label>
                        </td>
                        <td style="width: 35%;"  class="nonEditabletxtbox" >
                            <asp:Label ID="lblPatientAccountNumber" runat="server" class="nonEditabletxtbox"  ></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            <asp:Label ID="lblPatName" runat="server" Text="Patient Name" class="Editabletxtbox"></asp:Label>
                        </td>
                        <td class="style1 nonEditabletxtbox" >
                            <asp:Label ID="lblpatientName" runat="server" class="nonEditabletxtbox" ></asp:Label>
                        </td>
                        <td class="style1">
                            <asp:Label ID="lblPatDOB" runat="server" Text="Patient DOB" class="Editabletxtbox"></asp:Label>
                        </td>
                        <td class="style1 nonEditabletxtbox" >
                            <asp:Label ID="lblPatientDOB" runat="server" class="nonEditabletxtbox" ></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            <asp:Label ID="lblAppoinnmentDateTime" runat="server" Text="Appoinment Date Time" class="Editabletxtbox"></asp:Label>
                        </td>
                        <td class="style1 nonEditabletxtbox" >
                            <asp:Label ID="lblApptDateTime" runat="server" CssClass="ReadOnlyLable"  EnableViewState="true" 
                                class="nonEditabletxtbox" ></asp:Label>
                        </td>
                        <td class="style1">
                            <asp:Label ID="lblTypeVisit" runat="server" Text="Type Visit" class="Editabletxtbox"  EnableViewState="true"></asp:Label>
                        </td>
                        <td class="style1 nonEditabletxtbox" >
                            <asp:Label ID="lblPurposeofVisit" runat="server" CssClass="ReadOnlyLable" class="nonEditabletxtbox"></asp:Label>
                        </td>
                    </tr>
                    </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlRoomInInformation" runat="server" Font-Bold="true" class="LabelStyleBold"
            GroupingText="RoomIn Information"   BackColor="White">
                    <table id="Table1" runat="server" width="100%" height="30px">
                    <tr>
                        <td class="style2">
                            <asp:Label ID="lblExamRoom" runat="server" Text="Exam Room" class="Editabletxtbox"  Font-Bold="False"></asp:Label>
                        </td>
                        <td align="left" >
                            <telerik:RadComboBox ID="cboExamRoom" class="Editabletxtbox"  runat="server" Width="150px" Height="50px"   
                                Font-Bold="False" MaxHeight="40px" OnClientSelectedIndexChanged="EnableSave">
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    </table>
                    </asp:Panel>
                    <table id="Table2" runat="server" width="100%" style="margin-top: 1%;">
                    <tr>
                        <td  align="right">
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                        <td align="right">
                            &nbsp;<div style="display:inline;"> 
                                <telerik:RadButton  ID="btnOK" runat="server"  AccessKey="S" 
                                    ButtonType="LinkButton" CssClass="greenbutton teleriknormalbuttonstyle"
                                OnClick="btnOK_Click" Text="Save" OnClientClicked="StartLoadFromPatChart">
                                </telerik:RadButton></div>
                            <div style="display:inline;"> 
                                <telerik:RadButton CssClass="redbutton teleriknormalbuttonstyle" ButtonType="LinkButton" ID="btnCancel" runat="server"  AccessKey="C" 
                                    Text="Cancel" OnClientClicked="btnCancelclickRoomIn">
                                </telerik:RadButton></div>
                        </td>
                    </tr>
                </table>
                      <asp:HiddenField ID="hdnMessageType" runat="server"  EnableViewState="false"/>
       
    </asp:Panel>
    <asp:PlaceHolder ID="PlaceHolder1" runat="server">
        <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"  enableviewstate="false"></script>
        <script src="JScripts/JSRoomIn.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript" enableviewstate="false"></script>
         <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
   </asp:PlaceHolder>
   </form>
</body>
</html>
