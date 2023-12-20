<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmFindPatient.aspx.cs"
    Inherits="Acurus.Capella.UI.frmFindPatient" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Capella Find Patient</title>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <base target="_self" />
    <style type="text/css">
        #frmFindPatient {
            width: 99%;
            height: 100%;
        }

        .displayNone {
            display: none;
        }

        .modal {
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

        .loading {
            font-family: Arial;
            font-size: 10pt;
            border: 5px solid #67CFF5;
            width: 200px;
            height: 100px;
            display: none;
            position: fixed;
            background-color: White;
            z-index: 999;
        }

        .ui-autocomplete {
            -webkit-margin-before: 3px !important;
            max-height: 150px;
            overflow-y: auto;
        }

        .ui-state-focus {
            color: #808080;
            background-color: #bbe2f1 !important;
            outline: none;
            border: 0px !important;
        }

        .inactive, .inactive.ui-state-focus {
            background-color: #aaa !important;
        }

        .deceased, .deceased.ui-state-focus {
            background-color: black !important;
            color: white !important;
        }

        .disabled, .disabled.ui-state-focus {
            background-color: white !important;
        }
    </style>
    <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
    <script src="JScripts/jquery-ui.min1.11.4.js" type="text/javascript"></script>
    <link href="CSS/jquery-ui.css" rel="stylesheet" />
    <link href="~/CSS/CommonStyle.css" rel="Stylesheet" type="text/css" />
      <link href="CSS/bootstrap.min.css" rel="stylesheet" />
</head>

<body class="bodybackground" onpagehide="RemoveSessions()">
    <form id="frmFindPatient" runat="server" name="frmFindPatient" style="margin-top:2%">
        <telerik:RadWindowManager ID="ModalWindowMngt" runat="server" EnableViewState="false">
            <Windows>
                <telerik:RadWindow ID="ModalWindow" runat="server" VisibleOnPageLoad="false" Height="625px"
                    IconUrl="Resources/16_16.ico" Width="1225px" EnableViewState="false" DestroyOnClose="true">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <telerik:RadScriptManager ID="radscriptmngr" runat="server"></telerik:RadScriptManager>
        <telerik:RadFormDecorator ID="RadDecorator" runat="server" DecoratedControls="All" EnableViewState="false" />
        <%--<div style="width: 100%; height: 100%; margin-top: 2%;">--%>
            <div style="width: 100%; height: 100%;">
            <div>
                <table style="width: 100%;">
                    <tr>
                        
                        <td style="width: 10%;">
                            <span class="LabelStyleBold">Original Search**</span>
                        </td>
                        <td style="width: 58%;">
                            <asp:TextBox ID="txtPatientSearch" CssClass="LabelStyleBold" runat="server" Width="99%" data-human-id="0" data-human-details="" placeholder="Type minimum 3 characters of Last or First or Middle name or DOB as dd-mmm-yyyy or Acc# or Ext.Acc # or MR # and follow it by a space.."></asp:TextBox>
                            <img id="imgClearPatientText" src="Resources/Delete-Blue.png" alt="X" title="Click to clear the text field." style="margin-top:-5px;"/>
                        </td>
                        <td style="width: 9%;">
                            <span class="LabelStyleBold">Quick Search***</span>
                        </td>
                        <td style="width: 23%;">
                            <asp:TextBox ID="txtPatientSearchQuick" CssClass="LabelStyleBold" runat="server" Width="98%" data-human-id="0" data-human-details="" placeholder="(YYYYLNM) - Enter Birth Year YYYY and atleast first 3 characters of Last name followed by a space.."></asp:TextBox>
                            <img id="imgClearPatientTextQuick" src="Resources/Delete-Blue.png" alt="X" title="Click to clear the text field." style="margin-top:-5px;"/>
                        </td>
                    </tr>                    
                    <tr>
                        <td></td>
                        <td colspan="3">
                            <table style="width: 100%;">
                                <tr>
                                    <td style="width: 50%;" class="Editabletxtbox">
                                        <asp:CheckBox ID="chkIncludeInactive" runat="server" onclick="TriggerAutocomplete()" Text="Include Inactive Patients"/>
                                        <asp:CheckBox ID="chkIncludeDeceased" runat="server" onclick="TriggerAutocomplete()" Text="Include Deceased Patients"/>
                                    </td>
                                    <td style="width: 50%; text-align: right;">
                                        <span  class="LabelStyleBold">Patient Type : </span>
                                        <asp:RadioButton ID="rdbRegular" Checked="false" Text="Regular" runat="server" GroupName="PatientType" onclick="TriggerAutocomplete()" CssClass="Editabletxtbox" />
                                        <asp:RadioButton ID="rdbWC" Checked="false" Text="WC" runat="server" GroupName="PatientType" onclick="TriggerAutocomplete()" CssClass="Editabletxtbox" />
                                        <asp:RadioButton ID="rdbAuto" Checked="false" Text="Auto" runat="server" GroupName="PatientType" onclick="TriggerAutocomplete()" CssClass="Editabletxtbox" />
                                        <asp:RadioButton ID="rdbAll" Checked="true" Text="All" runat="server" GroupName="PatientType" onclick="TriggerAutocomplete()" CssClass="Editabletxtbox" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>                    
                </table>
            </div>
            <div style="float: right; margin-top: 1%; margin-right: 1%;">
                <table style="width: 50%; float: right;">
                    <tr id="trAddOrModifyPatient">
                        <td align="right">
                            <input type="hidden" id="btnQuickPatientCreate" value="Quick Patient Create" onclick="return OpenQuickPatient();" runat="server" tabindex="15" style="width: 160px;display:none" disabled="disabled" class="aspresizedbluebutton" />
                        </td>
                        <td align="right">
                            <input type="button" id="btnAddpatient" runat="server" value="Add Patient" onclick="return OpenDemoForAdd();" tabindex="16" style="width: 160px" disabled="disabled"  class="aspresizedbluebutton" />
                        </td>
                        <td align="right">
                            <input type="button" id="btnModifiyPatient" value="Modify/View Patient" onclick="return openDemographicsRadGrid();" tabindex="17" style="width: 160px" disabled="disabled"  class="aspresizedbluebutton" />
                        </td>
                    </tr>
                    <tr style="height:5px">

                    </tr>
                    <tr>
                        <td align="right">
                            <input type="button" id="btnPrintFaceSheet" value="Print FaceSheet"  onclick="FaceSheetClick()" style="width: 160px" disabled="disabled" class="aspresizedbluebutton" />
                        </td>
                        <td align="right">
                            <input type="button" id="btnOk" value="Ok" onclick="return CloseFindPatientRadGrid();" tabindex="18" style="width: 160px" disabled="disabled"  class="aspresizedgreenbutton"  />
                        </td>
                        <td align="right">
                            <input type="button" id="btnCancel" value="Cancel" onclick="cancel();" tabindex="19" style="width: 160px"  class="aspresizedredbutton" />
                        </td>
                    </tr>
                </table>
                <table style="width:100%;margin-top: 72px;">
                    <tr>                        
                        <td style="width: 100%;" >
                            <span  class="LabelStyleBold"><i>** Type minimum 3 characters of Last or First or Middle name or DOB as dd-mmm-yyyy or Acc# or Ext.Acc# or MRN# and follow it by a space.</i></span>                        
                          </td>
                         </tr>
                        <tr>                        
                        <td style="width: 100%;" >
                            <span  class="LabelStyleBold"><i>*** Enter patient's birth year(YYYY) and atleast first 3 characters of Last name followed by a space. Eg.1958DOE</i></span>                        
                            <%--<span  class="LabelStyleBold"><i>** - Kindly search by a unique keyword to experience enhanced performance of patient search. Avoid common names like David, Mary etc. After keying in the first word and obtaining results, enter a second word to narrow down your search instead of going through the inconvenience of scrolling.</i></span>                        --%>
                        </td>
                    </tr>
                </table>
                <div id="divLoading" class="modal" runat="server" style="text-align: center; display: none">
                    <asp:Panel ID="Panel2" runat="server">
                        <br />
                        <br />
                        <br />
                        <br />
                        <center>
                            <asp:Label ID="Label1" Text="" EnableViewState="false" runat="server"></asp:Label></center>
                        <br />
                        <img src="Resources/wait.ico" title="[Please wait while the page is loading...]"
                            alt="Loading..." />
                        <br />
                    </asp:Panel>
                </div>
                <asp:Label ID="lblLoad" runat="server" Style="display: none"></asp:Label>
                <br />
            </div>
        </div>

        <asp:HiddenField ID="hdnHumanID" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnselected" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnFromScreen" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnDataHumanDetails" runat="server" />
        <asp:HiddenField ID="SelectedItem" runat="server" />
        <asp:Button ID="btnface" runat="server"  style="display:none;" OnClick="btnPrintFaceSheet_ServerClick" CssClass="radPreventDecorate" />
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
            <script src="JScripts/jquery-ui.min1.11.4.js" type="text/javascript"></script>
            <link href="CSS/jquery-ui.css" rel="stylesheet" />
              <script src="JScripts/bootstrap.min.js" type="text/javascript"></script>
            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSLibraries.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <link href="CSS/ElementStyles.css" rel="stylesheet" type="text/css" />
        </asp:PlaceHolder>
    </form>
</body>
</html>
