<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmAddPhysician.aspx.cs" Inherits="Acurus.Capella.UI.frmAddPhysician" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="aspx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add Provider</title>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
        .col1 {
            width: 15%;
        }

        .col2 {
            width: 35%;
        }


        .col3 {
            width: 15%;
        }

        .col4 {
            width: 35%;
        }

        #frmFindReferralPhysician {
            width: 100%;
        }

        .HideOverflow {
            overflow: hidden;
        }

        .riTextBox {
            width: 96% !important;
        }
    </style>
    <link href="~/CSS/CommonStyle.css" rel="Stylesheet" type="text/css" />
</head>
<body onload="OnLoadPhysician();" >
    <form id="frmAddPhysician" runat="server">
        <div style="width: 100%">
            <aspx:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableViewState="false">
            </aspx:ToolkitScriptManager>
            <div>
                <asp:Panel ID="pnlSearchParameters" runat="server" Font-Size="Small" Width="100%">
                    <table style="width: 100%;">
                        <tr>
                            <td class="col1">
                                <asp:Label ID="lblLastName" EnableViewState="false" ForeColor="Red" runat="server" Text="Physician Last Name*" CssClass="Editabletxtbox" mand="Yes"></asp:Label>
                            </td>
                            <td class="col2">
                                <asp:TextBox ID="txtLastName" EnableViewState="false" runat="server" Width="262px" CssClass="Editabletxtbox"
                                    MaxLength="35"></asp:TextBox>
                            </td>
                            <td class="col3">
                                <asp:Label ID="lblFirstName" EnableViewState="false" runat="server" ForeColor="Red" Text="Physician First Name*" CssClass="Editabletxtbox"  mand="Yes"></asp:Label>
                            </td>
                            <td class="col4">
                                <asp:TextBox ID="txtFirstName" EnableViewState="false" runat="server" Width="262px" CssClass="Editabletxtbox"
                                    MaxLength="50"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="col1">
                                <asp:Label ID="lblSpecialty" EnableViewState="false" runat="server" ForeColor="Red" Text="Specialty*" CssClass="Editabletxtbox"  mand="Yes"></asp:Label>
                            </td>
                            <td class="col2">
                                <asp:DropDownList ID="ddlSpecialty" runat="server" Width="262px" CssClass="Editabletxtbox">
                                </asp:DropDownList>
                            </td>
                            <td class="col3">
                                <asp:Label ID="lblNPI" runat="server" EnableViewState="false" Text="NPI" CssClass="Editabletxtbox"></asp:Label>
                            </td>
                            <td class="col4">
                                <asp:TextBox ID="txtNPI" runat="server" EnableViewState="false" Width="262px" MaxLength="10" CssClass="Editabletxtbox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="col1">
                                <asp:Label ID="lblFacility" runat="server" EnableViewState="false" Text="Facility" CssClass="Editabletxtbox"></asp:Label>
                            </td>
                            <td class="col2 HideOverflow">
                                <asp:DropDownList ID="ddlFacility" runat="server" Width="262px" CssClass="Editabletxtbox">
                                </asp:DropDownList>
                            </td>
                            <td class="col3">
                                <asp:Label ID="lblAddress" runat="server" EnableViewState="false" Text="Physician Address" CssClass="Editabletxtbox"></asp:Label>
                            </td>
                            <td class="col4">
                                <asp:TextBox ID="txtAddress" runat="server" EnableViewState="false" Width="262px" CssClass="Editabletxtbox"
                                    MaxLength="55"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="col1">
                                <asp:Label ID="lblCity" runat="server" EnableViewState="false" Text="Physician City" CssClass="Editabletxtbox"></asp:Label>
                            </td>
                            <td class="col2">
                                <asp:TextBox ID="txtCity" runat="server" EnableViewState="false" Width="262px" onkeypress="return LettersWithSpaceOnly(event)" CssClass="Editabletxtbox"
                                    MaxLength="35"></asp:TextBox>
                            </td>
                            <td class="col3">
                                <asp:Label ID="lblState" runat="server" EnableViewState="false" Text="Physician State" CssClass="Editabletxtbox"></asp:Label>
                            </td>
                            <td class="col4">
                                <asp:TextBox ID="txtState" runat="server" EnableViewState="false" Width="262px" MaxLength="2" CssClass="Editabletxtbox"
                                    onkeypress="return LettersWithSpaceOnly(event)"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="col1">
                                <asp:Label ID="lblZip" runat="server" EnableViewState="false" Text="Zip" CssClass="Editabletxtbox"></asp:Label>
                            </td>
                            <td class="col2">
                                <telerik:RadMaskedTextBox ID="msktxtZip" runat="server" EnableViewState="false" Mask="#####-####" CssClass="Editabletxtbox"
                                    Width="100%">
                                    <InvalidStyle Resize="None" />
                                    <FocusedStyle Resize="None" />
                                    <EmptyMessageStyle Resize="None" />
                                    <HoveredStyle Resize="None" />
                                    <DisabledStyle Resize="None" />
                                    <EnabledStyle Resize="None" />
                                    <ReadOnlyStyle Resize="None" />
                                </telerik:RadMaskedTextBox>
                            </td>
                            <td class="col3">
                                <asp:Label ID="lblPhoneNum" runat="server" Text="Phone #" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                            </td>
                            <td class="col4">
                                <telerik:RadMaskedTextBox ID="msktxtPhoneNum" runat="server" CssClass="Editabletxtbox"
                                    Mask="(###) ###-####" Width="100%" EnableViewState="false">
                                </telerik:RadMaskedTextBox></td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
            <div>
                <asp:Panel ID="pnlButtons" runat="server" Font-Size="Small" Width="100%">
                    <table style="width: 100%;">
                        <tr>
                            <td class="col1">
                                <asp:Label ID="Label1" runat="server" Text="Fax #" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                            </td>
                            <td class="col2">
                                <telerik:RadMaskedTextBox ID="msktxtFaxNumber" runat="server" CssClass="Editabletxtbox"
                                    Mask="(###) ###-####" Width="100%" EnableViewState="false">
                                </telerik:RadMaskedTextBox>
                            </td>
                            <td style="float: right; text-align: right; width: 50%;">
                                <asp:Button ID="btnAddToLibrary" runat="server" Text="Add To Library"  OnClientClick="return validationInformation();" OnClick="btnAddToLibrary_Click" 
                                        CssClass="aspresizedbluebutton"  />
                                <asp:Button ID="btnClearAll" runat="server" Text="Clear All "  OnClick="btnClearAll_Click" 
                                       CssClass="aspresizedredbutton" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>

            <asp:HiddenField ID="hdnLastPageNo" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnAddPhysician" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnPhyID" runat="server" EnableViewState="false" />
            
        </div>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <script src="JScripts/jquery-1.11.3.min.js" type="text/javascript"></script>
            <script src="JScripts/jquery-ui.min1.10.2.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/bootstrap.min.js" type="text/javascript"></script>
            <script src="JScripts/JSLibraries.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSFindReferralPhysician.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <link href="CSS/ElementStyles.css" rel="stylesheet" type="text/css" />
        </asp:PlaceHolder>
    </form>
</body>
</html>
