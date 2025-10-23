<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmDecryption.aspx.cs" Inherits="Acurus.Capella.UI.frmDecryption" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Decryption</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
     <style type="text/css">
     .displayNone
        {
            display: none;
        }
        </style>
</head>
<body onload=" {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}">
    <form id="form1" runat="server" style="background-color: #BFDBFF;  font-family: Microsoft Sans Serif;width: 705px; height: 170px; margin-bottom: 1px">
    <div style="height: 160px" >
        <table style="font-family: Microsoft Sans Serif ;font-size:small;">
            <tr>
                <td style="width:30%;" valign="top">
                    <asp:Label ID="lblSelectedEncryptedLocation" runat="server"  Font-Bold="false" Font-Size="Small" 
                        Text="Select a  Encrypted File*"></asp:Label>
                </td>
                <td style="width:55%;" valign="top">
                    <input id="txtEncryptedLocation" type="text" runat="server" name="txtEncryptedLocation" style="width:100%;height:100%;" />
                </td>
                <td style="width:15%;" valign="top">
                    <input id="fuImport" runat="server"   enableviewstate="true" type="file" onchange="EncryptedLocation();"/>
                    
                </td>
            </tr>
            <tr>
                 <td style="width:30%;" valign="top">
                    <asp:Label ID="lblEncryptedFile" runat="server"  Font-Bold="false" Font-Size="Small" 
                         Text="Select a Decrypted File Location*"></asp:Label> 
                </td>
                 <td style="width:55%;" valign="top">
                     <input id="txtEncryptedFile" name="txtEncryptedFile" runat="server"  type="text" style="width:100%;height:100%;" />
                </td>
                <td>
                     <input id="fulEncryptedFile" runat="server"   enableviewstate="true" type="file" onchange="EncryptedFile();"/>
                </td>
            </tr>
            <tr>
                 <td style="width:30%;" valign="top">
                    <asp:Label ID="lblPassword" runat="server" Text="Password*"  Font-Bold="false" Font-Size="Small" ></asp:Label> 
                </td>
                <td style="width:55%;" valign="top">
                     <input id="txtPassword" name="txtPassword" runat="server" type="text" style="width:100%;height:100%;" />
                </td>
                <td >
                     <input id="btnDecryptFile" type="button" value="Decrypt File"  runat="server"
                         style="width:100%;height:25px;" onserverclick="btnDecryptFile_Click" />
                </td>
            </tr>
            <tr>
               <td style="width:30%;" valign="top">
                    <asp:Label ID="lblEncryptorDecrypt" runat="server" Text="Encrypt/Decrypt*"  Font-Bold="false" Font-Size="Small" ></asp:Label>
                </td>
               <td style="width:55%;">
                   <asp:Panel ID="Panel1" runat="server" Width="100%" Height="100%">
                       <table style="width: 100%; height:100%;">
                           <tr>
                               <td style="width:25%;" valign="top">
                                   <select id="cboAlgorithm" name="cboAlgorithm" runat="server" style="width:100%;height:100%;" onchange="changeTest()">
                                       <option>AES
                                       </option>
                                        <option>DES
                                       </option>
                                   </select>
                               </td>
                               <td style="width:30%;" valign="top">
                                   <asp:Label ID="lblKeySize" runat="server" Text="Key Size*"  Font-Bold="false" Font-Size="Small" ></asp:Label>
                               </td>
                               <td style="width:50%;" valign="top">
                                   <select id="cboKeySize" name="cboKeySize" runat="server" style="width:100%;height:100%;">
                                       <option></option>
                                   </select>
                               </td>
                           </tr>
                          
                       </table>
                   </asp:Panel>
                </td>
                <td valign="top">
                     <input id="btnClose" name="btnClose" type="button" value="Close"  style="width:100%;height:25px;"  runat="server" onclick="btnClose_Clicked();"/>
                </td>
            </tr>
        </table>
    </div>
     <asp:Button ID="InvisibleEncryptedLocation"  runat="server" CssClass="displayNone" 
        OnClick="InvisibleEncryptedLocation_Click1"  />
      <asp:Button ID="InvisibleEncryptedFile" runat="server" CssClass="displayNone" 
        onclick="InvisibleEncryptedFile_Click1" />
        
         <asp:Button ID="InvisibleCbo"  runat="server"  CssClass="displayNone" 
        onclick="InvisibleCbo_Click" />
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
<script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
    <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSDescryption.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    </asp:PlaceHolder>
    </form>
</body>
</html>
