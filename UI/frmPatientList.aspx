<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmPatientList.aspx.cs"  MasterPageFile="~/DemoGraphicsEmpty.Master" Inherits="Acurus.Capella.UI.frmPatientList" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>



<asp:Content ID="Demographics" ContentPlaceHolderID="C5POBody" runat="server">
  
<head>
    <title></title>

    <style>
        #falogout:hover {
            color: #FC0303 !important;
            cursor: pointer;
        }

        .aspresizedbluebutton {
            color: #fff !important;
            background-color: #428bca !important;
            border-color: #357ebd !important;
            display: inline-block !important;
            margin-bottom: 0 !important;
            font-weight: 400 !important;
            text-align: center !important;
            vertical-align: middle !important;
            cursor: pointer !important;
            background-image: none !important;
            border: 1px solid transparent !important;
            white-space: nowrap !important;
            padding: 4px 12px !important;
            line-height: 1.42857143 !important;
            border-radius: 6px !important;
            font-size: 12px !important;
            font-family: "Helvetica Neue",Helvetica,Arial,sans-serif !important;
            height: 25px !important;
        }


        .aspresizedredbutton {
            color: #fff !important;
            background-color: #c9302c !important;
            border-color: #ac2925;
            display: inline-block !important;
            margin-bottom: 0 !important;
            font-weight: 400 !important;
            text-align: center !important;
            vertical-align: middle !important;
            cursor: pointer !important;
            background-image: none !important;
            border: 1px solid transparent !important;
            white-space: nowrap !important;
            padding: 4px 12px !important;
            font-size: 14px !important;
            line-height: 1.42857143 !important;
            border-radius: 6px !important;
            font-size: 12px !important;
            font-family: "Helvetica Neue",Helvetica,Arial,sans-serif !important;
            height: 25px !important;
        }

        .aspbluebutton {
            color: #fff !important;
            background-color: #428bca !important;
            border-color: #357ebd !important;
            display: inline-block !important;
            margin-bottom: 0 !important;
            font-weight: 400 !important;
            text-align: center !important;
            vertical-align: middle !important;
            cursor: pointer !important;
            background-image: none !important;
            border: 1px solid transparent !important;
            white-space: nowrap !important;
            padding: 6px 12px !important;
            font-size: 14px !important;
            line-height: 1.42857143 !important;
            border-radius: 4px !important;
            font-size: 15px !important;
            font-family: "Helvetica Neue",Helvetica,Arial,sans-serif !important;
        }

        .aspresizedgreenbutton {
            color: #fff !important;
            background-color: #5cb85c !important;
            border-color: #4cae4c !important;
            display: inline-block !important;
            margin-bottom: 0 !important;
            font-weight: 400 !important;
            text-align: center !important;
            vertical-align: middle !important;
            cursor: pointer !important;
            background-image: none !important;
            border: 1px solid transparent !important;
            white-space: nowrap !important;
            padding: 4px 12px !important;
            font-size: 14px !important;
            line-height: 1.42857143 !important;
            border-radius: 6px !important;
            font-size: 12px !important;
            font-family: "Helvetica Neue",Helvetica,Arial,sans-serif;
            height: 25px !important;
        }



        .aspredbutton {
            color: #fff !important;
            background-color: #c9302c !important;
            border-color: #ac2925 !important;
            display: inline-block !important;
            margin-bottom: 0 !important;
            font-weight: 400 !important;
            text-align: center !important;
            vertical-align: middle !important;
            cursor: pointer !important;
            background-image: none !important;
            border: 1px solid transparent !important;
            white-space: nowrap !important;
            padding: 6px 12px !important;
            font-size: 14px !important;
            line-height: 1.42857143 !important;
            border-radius: 4px !important;
            font-size: 15px !important;
            font-family: "Helvetica Neue",Helvetica,Arial,sans-serif !important;
        }

        .header-center {
            text-align: center !important;
        }

        .Header-align {
            height: 49px;
            border: 10px;
            border-color: #E7E7FF;
        }

        .form-control {
            display: block;
            width: 100%;
            height: 34px;
            padding: 6px 12px;
            font-size: 14px;
            line-height: 1.42857143;
            color: #555;
            background-color: #fff;
            background-image: none;
            border: 1px solid #ccc;
            border-radius: 4px;
            -webkit-box-shadow: inset 0 1px 1px rgba(0,0,0,.075);
            box-shadow: inset 0 1px 1px rgba(0,0,0,.075);
            -webkit-transition: border-color ease-in-out .15s,-webkit-box-shadow ease-in-out .15s;
            -o-transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;
            transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;
        }

        .vl {
            border-left: 2px solid green;
            height: 100px;
            position: absolute;
            margin-left: 25px;
            top: 34px;
        }

        .MandLabelstyle {
            color: red !important;
            font-family: "Helvetica Neue",Helvetica,Arial,sans-serif;
            font-size: 13px;
            border-style: none;
            font-weight: normal;
        }
        Gridheaderstyle {
    /*border: 1px solid #bfdbff !important;
    border-right: 1px solid #bfdbff !important;
    border-left: 1px solid #bfdbff !important;*/
    background-color: #bfdbff !important;
    background-image: none !important;
    font-weight: bold;
    font-family: "Helvetica Neue",Helvetica,Arial,sans-serif;
    font-size: 13px;
    height:60px !important;
}
        .Gridbodystyle {
    border: 1px solid #f0f0f0 !important;
    color: #3a3a3a;
    font-family: "Helvetica Neue",Helvetica,Arial,sans-serif;
    font-size: 13px;
}
        #ctl00_C5POBody_grdEncounters_ctl00_Header thead tr
        {
            height:60px !important;
        }
          #ctl00_C5POBody_grdEncounters_ctl00_Header thead tr th
        {
            text-align:center !important;
        }
    </style>

     <link href="~/CSS/style.css" rel="stylesheet" type="text/css" />
       
        <link href="CSS/jquery-ui.min.css" rel="stylesheet" />
          <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="JScripts/jquery-2.1.3.js"></script>
        <script type="text/javascript" src="JScripts/jquery-ui.min.js"></script>
     <link href="CSS/fontawesomenew.css" rel="stylesheet" />
   
    <link href="CSS/font-awesome.min4.7.0.css" rel="stylesheet" />
      <%--CAP-413: Restrict the Capella EHR access in multiple tabs --%>
    <script src="JScripts/JsRestrictMultipleTab.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
</head>
<body>
        <script id="ze-snippet" src="<%=ConfigurationManager.AppSettings["ZendeskWidgetURL"].ToString()%>"> </script>
    <script type="text/javascript">
        window.zESettings = {
            webWidget: {
                launcher: {
                    label: {
                        'en-US': ' '
                    }
                }
            }
        };
    </script>
<script type="text/javascript">
    var cookies = document.cookie.split(';');
    //var sUsername = "";
    //var sUserrole = "";
    var sPersonname = "";
    var sEMailAddress = "";
    var bReadOnly = false;
    for (var l = 0; l < cookies.length; l++) {
        if (cookies[l].indexOf("CPersonName") > -1) {
            sPersonname = cookies[l].split("=")[1];
        }
        else if (cookies[l].indexOf("CEMailAddress") > -1) {
            sEMailAddress = cookies[l].split("=")[1];
            if (sEMailAddress != "")
                bReadOnly = true;
        }
        //else if (cookies[l].indexOf("CUserName") > -1) {
        //    sUsername = cookies[l].split("=")[1];
        //}
        //else if (cookies[l].indexOf("CUserRole") > -1) {
        //    sUserrole = cookies[l].split("=")[1];
        //}

    }
    zE('webWidget', 'prefill', {
        name: {
            value: sPersonname,
            readOnly: true // optional
        },
        email: {
            value: sEMailAddress,
            readOnly: bReadOnly // optional
        }
        //phone: {
        //    value: '61431909749',
        //    readOnly: true // optional
        //}
    });
</script>
    <telerik:RadWindowManager ID="PatientListModalWindowMngt" runat="server" EnableViewState="false">
            <Windows>
                <telerik:RadWindow ID="PatListModalWindow" runat="server" VisibleOnPageLoad="false" VisibleStatusbar="false" Behaviors="Close"
                    IconUrl="Resources/16_16.ico" EnableViewState="false">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
    <form id="form1">
        

        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" EnableScriptCombine="true">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
            </Scripts>
        </telerik:RadScriptManager>
        <div>
            <table>
                <tr>
                    <td style="width: 100%;text-align: end;">
                        <div style="margin-top: -5px;"> <%--margin-left: 1170px; --%>
                            <asp:Label ID="lblLogged" runat="server" Style="text-align:right; font-weight: bold; font-style: italic; white-space: nowrap;font-family: Segoe UI, Arial, Helvetica, sans-serif;
    font-size: 12px;"></asp:Label>
                        </div>
                    </td>
                    <td style="width: 0%;">
                        <i class="fa fa-power-off" style="font-size: 23px; color: #fe5c00; " onclick="OpenModal('Logout');" title="Logout" id="falogout"></i> <%--margin-left: 321px; margin-top: -19px;--%>
                    </td>
                </tr>
            </table>
            <div style="margin-top: 15px; margin-left: -11px;">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 8%">
                            <asp:Label ID="lblPayerName" class="MandLabelstyle" runat="server" Text="Pri. Carrier*" Style="font-size: 15px; font-weight: 400; font-family: inherit;margin-left:13px;"></asp:Label>
                        </td>

                        <td style="width: 18%">
                            <%--<asp:DropDownList ID="ddlPayerName" runat="server" AutoPostBack="True" Width="74%" Style="margin-left: 15px;" OnSelectedIndexChanged="ddlPayerName_SelectedIndexChanged" onchange="LoadingImage();"></asp:DropDownList>--%>
                            <select id="ddlPayerName" runat="server" style="width: 74%; margin-left: 15px;" onchange="PayerNameChange();" onserverchange="ddlPayerName_SelectedIndexChanged">
                            </select>
                            <%-- OnSelectedIndexChanged="ddlPayerName_SelectedIndexChanged"--%>
                        </td>

                        <td style="width: 8%">
                            <asp:Label ID="lblPlan" class="MandLabelstyle" runat="server" Text="Pri. Plan*" Style="font-size: 15px; font-weight: 400; font-family: inherit;"></asp:Label>
                        </td>

                        <td style="width: 18%">
                            <asp:DropDownList ID="ddlPlan" runat="server" Width="74%" Style="margin-left: 11px;"></asp:DropDownList>
                            <%-- OnSelectedIndexChanged="ddlPayerName_SelectedIndexChanged"--%>
                        </td>


                        <td style="width: 8%">
                            <asp:Label ID="lblMemberId" runat="server" Text="Member ID" Style="font-size: 15px; font-weight: 400; font-family: inherit; white-space: nowrap;"></asp:Label>
                        </td>

                        <td style="width: 18%">
                            <input type="text" id="txtMemberId" class="form-control" runat="server" maxlength="25" style="width: 70%; border-color: black; height: 8px; margin-left: 19px;" />
                        </td>

                        <td style="width: 8%">
                            <asp:Button runat="server" ID="btnOK" CssClass="aspresizedgreenbutton" Text="Generate" OnClick="btnOK_Click" OnClientClick="LoadingImage();" Width="100px" Style="margin-left: 10px;" />
                        </td>
                        <%--<td align="left">
                         <asp:Label ID="lblMatchFound" runat="server" Text="Match Found!" style="font-size: 15px; font-weight: 400;color:#5cb85c !important;;font-family: inherit;white-space: nowrap; margin-left: 30px;"></asp:Label>
                     </td>--%>
                        <td style="width: 18%">
                            <div class="vl"></div>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 8%">
                            <asp:Label ID="lblLastName" runat="server" Text="Pt. Last Name" Style="font-size: 15px; margin-left: 13px; font-weight: 400; font-family: inherit; white-space: nowrap;"></asp:Label>
                        </td>

                        <td style="width: 18%">
                            <input type="text" id="txtPatientLastName" class="form-control" runat="server" maxlength="50" style="width: 65%; border-color: black; height: 8px; margin-left: 15px;"/>
                        </td>
                        <td style="width: 8%">
                            <asp:Label ID="lblFirstName" runat="server" Text="Pt. First Name" Style="font-size: 15px; margin-left: 0px; font-weight: 400; font-family: inherit; white-space: nowrap;"></asp:Label>
                        </td>

                        <td style="width: 18%">
                            <input type="text" id="txtPatientFirstName" class="form-control" runat="server" maxlength="50" style="width: 65%; border-color: black; height: 8px; margin-left: 11px;"/>
                        </td>
                        <td style="width: 8%">
                            <asp:Label ID="lblDOB" runat="server" Text="DOB(Format: 01-Jan-1987)" Style="font-size: 15px; font-weight: 400; font-family: inherit;"></asp:Label>
                        </td>

                        <td style="width: 18%">
                        <%--    <telerik:RadDatePicker ID="dtpPatientDOB" runat="server" Culture="English (United States)" Style="margin-left: 19px;"
                                Skin="Web20" onkeypress="EnableSaveButton(this);" Height="21px"
                                Width="241px" MinDate="1900-01-01">
                                <Calendar EnableWeekends="True" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                                    ShowRowHeaders="false" Skin="Web20">
                                    <SpecialDays>
                                        <telerik:RadCalendarDay Repeatable="Today">
                                            <ItemStyle CssClass="rcToday" />
                                        </telerik:RadCalendarDay>
                                    </SpecialDays>
                                </Calendar>
                                <DatePopupButton HoverImageUrl="" ImageUrl="" />
                                <DateInput DateFormat="yyyy-MM-dd" DisplayDateFormat="yyyy-MM-dd" LabelWidth="40%"
                                    Height="21px">
                                    <EmptyMessageStyle Resize="None" />
                                    <FocusedStyle Resize="None" />
                                    <DisabledStyle Resize="None" />
                                    <InvalidStyle Resize="None" />
                                    <HoveredStyle Resize="None" />
                                    <EnabledStyle Resize="None" />
                                </DateInput>
                                <ClientEvents OnPopupOpening="EnableSaveButton" OnDateSelected="ValidateDate" />
                            </telerik:RadDatePicker>--%>

                             <telerik:RadMaskedTextBox ID="dtpPatientDOB" runat="server" Mask="##-Lll-####" Width="210" style="margin-left:7%"  CssClass="form-control">
                                                    <ClientEvents OnValueChanged="QPCDateValidation" />
                                                    <InvalidStyle Resize="None" />
                                                    <FocusedStyle Resize="None" BackColor="White" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <HoveredStyle Resize="None" BackColor="White" />
                                                    <DisabledStyle Resize="None" />
                                                    <EnabledStyle Resize="None" BackColor="White" />
                                                    <ReadOnlyStyle Resize="None" />
                                                </telerik:RadMaskedTextBox>
                        </td>
                        <td style="width: 8%">
                            <asp:Button runat="server" ID="btnClear" CssClass="aspresizedredbutton" Text="Clear All" OnClick="btnClear_Click" Width="100px" Style="margin-left: 10px;" />
                        </td>

                    </tr>
                    <tr>
                        <td style="width: 8%">
                            <asp:Label ID="lblPatientAccNo" runat="server" Text="Pt. Acc. #" onkeypress="return isNumberKey(event)" Style="font-size: 15px; margin-left: 13px; font-weight: 400; font-family: inherit; white-space: nowrap;"></asp:Label>
                        </td>

                        <td style="width: 18%">
                            <input type="text" id="txtPatientAccNo" class="form-control" runat="server" maxlength="10" style="width: 65%; border-color: black; height: 8px; margin-left: 15px;" onkeypress="return isNumberKey(this);" />
                        </td>

                        <td style="width: 8%">
                            <asp:Label ID="lblFromDOS" class="MandLabelstyle" runat="server" Text="DOS - From*" Style="font-size: 15px; font-weight: 400; font-family: inherit; white-space: nowrap; "></asp:Label>
                        </td>

                        <td style="width: 18%;">
                            <%--<telerik:RadDatePicker ID="dtpFromDOS" runat="server" Culture="English (United States)" EnableViewState="false" DateInput-MinDate="1/1/1900 12:00:00 AM"
                                Skin="Web20" Height="21px" Style="margin-left: 11px;"
                                Width="84%" onkeypress="EnableSaveButton(this);">
                                <Calendar UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False" EnableWeekends="True"
                                    ShowRowHeaders="false" Skin="Web20" ViewSelectorText="x" RangeMinDate="1/1/1900 12:00:00 AM">
                                    <SpecialDays>
                                        <telerik:RadCalendarDay Repeatable="Today">
                                            <ItemStyle CssClass="rcToday" />
                                        </telerik:RadCalendarDay>
                                    </SpecialDays>
                                </Calendar>
                                <DatePopupButton HoverImageUrl="" ImageUrl="" />
                                <DateInput DateFormat="yyyy-MM-dd" DisplayDateFormatdtpToDOS="yyyy-MM-dd" LabelWidth="40%"
                                    Height="21px" type="text" value="" MinDate="1/1/1900 12:00:00 AM">
                                    <EmptyMessageStyle Resize="None" />
                                    <FocusedStyle Resize="None" />
                                    <DisabledStyle Resize="None" />
                                    <InvalidStyle Resize="None" />
                                    <HoveredStyle Resize="None" />
                                    <EnabledStyle Resize="None" />
                                </DateInput>
                                <%--<ClientEvents OnPopupOpening="EnableSaveButton" OnDateSelected="EnableSaveButton" />
                            </telerik:RadDatePicker>--%>
                             <telerik:RadMaskedTextBox ID="dtFromDOS" runat="server" Mask="##-Lll-####" Width="201px" style="margin-left:4%" CssClass="form-control" >
                                                   
                                  <ClientEvents OnValueChanged="QPCDateValidation" />
                                                    <InvalidStyle Resize="None" />
                                                    <FocusedStyle Resize="None" BackColor="White" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <HoveredStyle Resize="None" BackColor="White" />
                                                    <DisabledStyle Resize="None" />
                                                    <EnabledStyle Resize="None" BackColor="White" />
                                                    <ReadOnlyStyle Resize="None" />
                                                </telerik:RadMaskedTextBox>
                        </td>
                        

                        <td style="width: 8%">
                            <asp:Label ID="lblToDOS" class="MandLabelstyle" runat="server" Text="DOS - To*" Style="font-size: 15px; font-weight: 400; font-family: inherit; white-space: nowrap; margin-left: 0px;"></asp:Label>
                        </td>

                        <td style="width: 18%;">
                           <%-- <telerik:RadDatePicker ID="dtpToDOS" runat="server" Culture="English (United States)"
                                Skin="Web20" onkeypress="EnableSaveButton(this);" Height="21px" Style="margin-left: 19px;"
                                Width="89%">
                                <Calendar EnableWeekends="True" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                                    ShowRowHeaders="false" Skin="Web20">
                                    <SpecialDays>
                                        <telerik:RadCalendarDay Repeatable="Today">
                                            <ItemStyle CssClass="rcToday" />
                                        </telerik:RadCalendarDay>
                                    </SpecialDays>
                                </Calendar>
                                <DatePopupButton HoverImageUrl="" ImageUrl="" />
                                <DateInput DateFormat="yyyy-MM-dd" DisplayDateFormat="yyyy-MM-dd" LabelWidth="40%"
                                    Height="21px">
                                    <EmptyMessageStyle Resize="None" />
                                    <DisabledStyle Resize="None" />
                                    <InvalidStyle Resize="None" />
                                    <HoveredStyle Resize="None" />
                                    <EnabledStyle Resize="None" />
                                </DateInput>
                                <ClientEvents OnPopupOpening="EnableSaveButton" OnDateSelected="EnableSaveButton" />
                            </telerik:RadDatePicker>--%>


                             <telerik:RadMaskedTextBox ID="dtToDOS" runat="server" Mask="##-Lll-####" Width="210" style="margin-left:7%" CssClass="form-control" >
                                                   
                                  <ClientEvents OnValueChanged="QPCDateValidation" />
                                                    <InvalidStyle Resize="None" />
                                                    <FocusedStyle Resize="None" BackColor="White" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <HoveredStyle Resize="None" BackColor="White" />
                                                    <DisabledStyle Resize="None" />
                                                    <EnabledStyle Resize="None" BackColor="White" />
                                                    <ReadOnlyStyle Resize="None" />
                                                </telerik:RadMaskedTextBox>
                        </td>


                        <td style="width: 8%">
                            <asp:Button runat="server" ID="btnExporttoExcel" CssClass="aspresizedbluebutton" Text="Export to Excel" Width="100px" Style="margin-left: 10px;" OnClick="btnExporttoExcel_Click" />
                        </td>


                        <td align="left" style="width: 8%">
                            <asp:Label ID="lblNoofResults" runat="server" Text="" Style="font-size: 15px; font-weight: 400; font-family: inherit; white-space: nowrap; margin-left: 30px;"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>

            <div style="margin-top: 22px;">
                <hr style="width: 100%; text-align: left; margin-left: 0">
            </div>
            <div>
                <table style="margin-top: 10px;">
                    <tr>
                        <td style="height: 50%;">
                            <telerik:RadGrid ID="grdEncounters" runat="server" AutoGenerateColumns="False" CellSpacing="0"
                                GridLines="None" Width="1326px"
                                ClientSettings-EnablePostBackOnRowClick="false" AutoGenerateSelectButton="True"
                                EmptyDataText="No Records" Font-Size="Small"  EnableTheming="False"  EnableViewState="true" HeaderStyle-Font-Bold="true"
                                CellPadding="3"
                                OnItemCommand="grdEncounters_ItemCommand"
                               AllowSorting="true" CssClass="Gridbodystyle" MasterTableView-AllowNaturalSort="false" OnNeedDataSource="grdEncounters_NeedDataSource">
                               
                                
                                <HeaderStyle Font-Bold="true" CssClass="Gridheaderstyle"  />
                                 <ClientSettings AllowKeyboardNavigation="true" Scrolling-UseStaticHeaders="true"
                                    EnablePostBackOnRowClick="false">
                                    <Selecting AllowRowSelect="True" />
                                    <Scrolling AllowScroll="True" ScrollHeight="440px" UseStaticHeaders="True" />
                                    <KeyboardNavigationSettings EnableKeyboardShortcuts="true"></KeyboardNavigationSettings>
                                </ClientSettings>
                                <MasterTableView Width="100%">
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="Patient Name" HeaderText="Patient Name">
                                            <HeaderStyle Width="30px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="DOB" HeaderText="DOB">
                                            <HeaderStyle Width="15px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Pt. Acc. #" HeaderText="Pt. Acc. #" UniqueName="PatientAcc">
                                            <HeaderStyle Width="15px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Member ID" HeaderText="Member ID">
                                            <HeaderStyle Width="15px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="DOS" HeaderText="DOS" DataType="System.DateTime">
                                            <HeaderStyle Width="20px" />

                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Enc. Provider" HeaderText="Enc. Provider">
                                            <HeaderStyle Width="30px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Pri. Carrier" HeaderText="Pri. Carrier">
                                            <HeaderStyle Width="30px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Pri. Plan" HeaderText="Pri. Plan">
                                            <HeaderStyle Width="30px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Type of Visit" HeaderText="Type of Visit">
                                            <HeaderStyle Width="20px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Facility" HeaderText="Facility">
                                            <HeaderStyle Width="20px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Encounter ID" HeaderText="Encounter ID" UniqueName="EncID" Display="false" FilterControlAltText="Filter column10 column">
                                            <%-- <HeaderStyle Width="1px" />
                                            <ItemStyle HorizontalAlign="Center" CssClass="displayNone" />--%>
                                        </telerik:GridBoundColumn>
                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="ViewSummary" Text="View"
                                            HeaderText="View" FilterControlAltText="Filter View column" UniqueName="View"
                                            ImageUrl="~/Resources/Down.bmp">
                                            <HeaderStyle Width="10px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridButtonColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </td>
                    </tr>
                </table>
            </div>


            <div id="ModalProgressNotes" class="modal fade">
                <div class="modal-dialog" style="height: 100%; width: 1050px; margin-left: 7%; display: none;">
                    <div class="modal-content" style="height: 99%; width: 100%">
                        <div class="modal-header" style="padding-top: 0px; padding-bottom: 0px;">
                            <button type="button" id="btnClosewindowNotes" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                            <h5 id="ModalTtleNotes" style="font-weight: bold;"></h5>
                        </div>
                        <div class="modal-body" style="height: 100%; width: 100%">
                            <iframe style="width: 100%; height: 100%; border: none; overflow: hidden; position: relative" id="ProcessiFrameNotes"></iframe>
                        </div>
                    </div>
                </div>
            </div>

        </div>

        <asp:Button ID="btnlogout" runat="server" Text="Button" OnClick="btnlogout_Click" style="display:none;" CssClass="displayNone" />
        <asp:HiddenField ID="hdnEncID" runat="server" />
        <asp:HiddenField ID="hdnLocalTime" runat="server" EnableViewState="false" />

        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
         <%--   <script src="JScripts/jquery.datetimepicker.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>--%>
            <script src="JScripts/JSPatientList.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
         <%--   <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
            <script type="text/javascript" src="JScripts/jquery.datetimepicker.js"></script>
            <script src="Jscripts/bootstrap.min.js"></script>--%>
        </asp:PlaceHolder>
    </form>
</body>
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
</asp:Content>
