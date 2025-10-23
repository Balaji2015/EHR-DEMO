<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="webfrmRegistration.aspx.cs" Inherits="Acurus.Capella.UI.webfrmRegistration" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <style type="text/css">
        .style2
        {
            width: 280px;
        }
        .style3
        {
            width: 668px;
            margin-left: 40px;
        }
        .style4
        {
            height: 18px;
            font-size: small;
        }
        .style5
        {
            width: 55px;
        }
    </style>
    <style type="text/css">
        div#centered {
    background-color: #eee; 
    border: 1px solid #ddd; 
    position: absolute; 
    left: 50%; 
    top: 50%;  
    width: 523px;  
    height: 228px; 
    margin-left: -200px;  
    margin-top: -150px;
}
        .style6
        {
            height: 18px;
            font-size: medium;
            text-align: left;
        }
        .style7
        {
            width: 103px;
            margin-left: 40px;
        }
        .style8
        {
            width: 55px;
            height: 26px;
        }
        .style9
        {
            width: 668px;
            height: 26px;
            margin-left: 40px;
        }
        .style10
        {
            width: 280px;
            height: 26px;
        }
        .style11
        {
            width: 55px;
            height: 42px;
        }
        .style12
        {
            width: 668px;
            margin-left: 40px;
            height: 42px;
        }
        .style13
        {
            width: 280px;
            height: 42px;
        }
        .style14
        {
            width: 55px;
            height: 33px;
        }
        .style15
        {
            width: 668px;
            margin-left: 40px;
            height: 33px;
        }
        .style16
        {
            width: 280px;
            height: 33px;
        }
 </style>
</head>
<body>
    <form id="form1" runat="server">
     <table>
    <tr><td align =center>
    <div id="centered">
    
                                        <table border="0" cellpadding="4" cellspacing="0" 
                                            style="border-collapse:collapse; height: 220px;">
                                            <tr>
                                                <td align="center">
                                                    <table border="0" cellpadding="0" style="height:222px; width:519px;">
                                                        <tr>
                                                            <td align="center" colspan="2" 
                                                                style="color:White;background-color:#507CD1;font-weight:bold;" 
                                                                class="style4">
                                                                &nbsp;</td>
                                                            <td class="style6" 
                                                                style="color:White;background-color:#507CD1;font-weight:bold;">
                                                                Registration</td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" class="style11">
                                                                </td>
                                                            <td align="right" class="style12">
                                                                <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="EMail">E-Mail ID</asp:Label>
                                                            </td>
                                                            <td class="style13">
                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:TextBox ID="EMail" runat="server" Font-Size="0.8em" Width="266px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" class="style8">
                                                                </td>
                                                            <td align="right" class="style9">
                                                                <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="DateofBirth">Date 
                                                                of Birth: (Ex: 15-Feb-1947)</asp:Label>
                                                            </td>
                                                            <td class="style10" align="left">
                                                                <asp:TextBox ID="DateofBirth" runat="server" Font-Size="0.8em" Width="266px"></asp:TextBox>
                                                                <br />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" class="style5">
                                                                &nbsp;</td>
                                                            <td align="right" class="style3">
                                                                <asp:Label ID="PasswordLabel2" runat="server" AssociatedControlID="PIN">Pin 
                                                                (Enter last 4 digits of SSN):    </asp:Label>
                                                            </td>
                                                            <td class="style2">
                                                                <asp:TextBox ID="PIN" runat="server" Font-Size="0.8em" Width="266px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" class="style5">
                                                                &nbsp;</td>
                                                            <td align="right" class="style3">
                                                                <asp:Label ID="PasswordLabel3" runat="server" AssociatedControlID="Password">Enter Password :</asp:Label>
                                                            </td>
                                                            <td class="style2">
                                        <asp:TextBox ID="Password" runat="server" Font-Size="0.8em" TextMode="Password" 
                                            Width="266px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" class="style14">
                                                                </td>
                                                            <td align="right" class="style15">
                                                                <asp:Label ID="PasswordLabel4" runat="server" 
                                                                    AssociatedControlID="ConfirmPassword">Confirm Password: </asp:Label>
                                                            </td>
                                                            <td class="style16">
                                        <asp:TextBox ID="ConfirmPassword" runat="server" Font-Size="0.8em" TextMode="Password" 
                                            Width="266px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" class="style5">
                                                                &nbsp;</td>
                                                            <td align="right" class="style7">
                                                                <asp:Label ID="lblAlertText" runat="server" 
                                                                    AssociatedControlID="ConfirmPassword" style="color: #FF3300"></asp:Label>
                                                            </td>
                                                            <td class="style2">
                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:Button ID="LoginButton" runat="server" BackColor="White" 
                                                                    BorderColor="#507CD1" BorderStyle="Solid" BorderWidth="1px" CommandName="Login" 
                                                                    Font-Names="Verdana" Font-Size="0.8em" ForeColor="#284E98" 
                                                                    onclick="LoginButton_Click" Text="Register" 
                                                                    ValidationGroup="Registration1" style="height: 18px" />
                                                            </td>
                                                        </tr>
                                                        </table>
                                                </td>
                                            </tr>
                                        </table>
                                    <br />
        <br />
        <br />
        <br />
    
    </div>
    </tr>>
    </table>
    </form>
</body>
</html>
