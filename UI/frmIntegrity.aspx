<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmIntegrity.aspx.cs" Inherits="Acurus.Capella.UI.frmIntegrity" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Integrity</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
   <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
        .style1
        {
            width: 320px;
        }
        .style2
        {
            width: 239px;
        }
        #Select1
        {
            width: 58px;
        }
    </style>
   
<script language="javascript" type="text/javascript">
// <!CDATA[



// ]]>
</script>
</head>
<body onload=" {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}">
    <form id="form1" runat="server" 
    style="background-color: #BFDBFF;  font-family: Microsoft Sans Serif; width:750px; height:180px; margin-bottom: 1px;">
    <div style="height: 150px">
        <table width="100%" style="font-family: Microsoft Sans Serif ;font-size:small;">
            <tr>
                <td class="style1" valign="top">
                    <asp:Label ID="lblTypeTextHash" runat="server"  Font-Bold="false" Font-Size="Small" 
                        Text="Type Text to Hash"></asp:Label>
                </td>
                <td class="style2" valign="top">
                    <input id="txtTextToHash" name="txtTextToHash" runat="server" type="text" style="width:450px;height:100%;" />
                </td>
                <td style="width:15%;" valign="top">
                
                    
                     <input id="btnHashString" name="btnHashString" type="button" value="Hash String"  runat="server" onserverclick="btnHashString_Click"  style="width:100%;height:25px;" />
                      
                       
                </td>
            </tr>
            <tr>
                 <td class="style1" valign="top">
                    <asp:Label ID="lblSelectFiletoHash" runat="server" Font-Bold="false" Font-Size="Small" 
                         Text="Select File to Hash"></asp:Label> 
                </td>
                 <td class="style2" valign="top">
                    <asp:Panel ID="Panel1" runat="server" Width="400px">
                       <table width="100%">
                           <tr>
                           
                               <td style="width:100%;" valign="top">
                          <input id="fuImport" runat="server"   enableviewstate="true" type="file" style="width:100%;height:25px;" /></td>
                                   
                           </tr>
                          
                       </table>
                   </asp:Panel>
                   </td>
                <td>
                
                     <input id="btnHashFile" name="btnHashFile" type="button" value="Hash File"  runat="server"  onserverclick="btnHashFile_Click"
                         style="width:100%;height:25px;" />
                </td>
            </tr>
            <tr>
                 <td class="style1" valign="top">
                    <asp:Label ID="lblUse" runat="server" Text="Use 0 for MD5, 1 for SHA-1" Font-Bold="false" Font-Size="Small" ></asp:Label> 
                </td>
                <td class="style2" valign="top">
                    <select id="cboSHA" runat="server" name="cboSHA">
                        <option>0</option>
                        <option>1</option>
                    </select>
                
                     </td>
                <td valign="top">
                     <input id="btnClose" name="btnClose" type="button" value="Close"  style="width:100%;height:25px;"  runat="server" onclick="btnClose_Clicked();"/></td>
            </tr> 
            <tr>
               <td class="style1" valign="top">
                    <asp:Label ID="lblResultOfHash" runat="server" Text="Result of Hash" Font-Bold="false" Font-Size="Small"></asp:Label>
                </td>
               <td rowspan="2" class="style2" valign="top">
                   <textarea id="txtResultOfHash" runat="server" name="txtResultOfHash" enableviewstate="true" cols="53" rows="2"></textarea>
                   
                      </td>
                <td rowspan="2" valign="top">
                     </td>
            </tr>
            
             <tr>
               <td class="style1" valign="top">
                   
                </td>
            </tr>
        </table>
     
    </div>
   <asp:PlaceHolder ID="PlaceHolder1" runat="server">
       <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
     <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSIntegrity.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    
    </asp:PlaceHolder>
    </form>
</body>
</html>
