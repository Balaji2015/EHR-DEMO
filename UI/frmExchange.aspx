<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmExchange.aspx.cs" Inherits="Acurus.Capella.UI.frmExchange" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
        .buttonD {
            margin-left: 150px;
        }

        .Displaynone {
            display: none;
        }

        .responseDisplay {
            width: 98%;
        }

        fieldset {
            margin-bottom: 10px !important;
        }

    </style>
      <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <title></title>
</head>
<body oncontextmenu="return false" style="overflow: visible;" onload=" {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}">
    <form id="form1" runat="server" style="overflow: hidden;">
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        </telerik:RadAjaxManager>
        <telerik:RadWindowManager ID="WindowMngr" runat="server" EnableViewState="False">
            <Windows>
                <telerik:RadWindow ID="RadExchangeWindow" runat="server" Behaviors="Close"
                    VisibleStatusbar="false" IconUrl="Resources/16_16.ico">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <div id="ContainerDiv" runat="server" style="display: block;">
            <div style="width: 650px; display: inline-block;" id="queryDiv" runat="server">

                <table id="tblSummary" runat="server" style="width: 620px; height: 400px;">
                    <tr>
                        <td style="border: 0px!important;" colspan="2">
                            <asp:RadioButton ID="rbtnAck" Font-Size="Small" GroupName="Acknowledement" AutoPostBack="true" runat="server" onclick="Download(this);" Text="With Acknowledgement" CssClass="spanstyle"/>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                           <asp:RadioButton ID="rbtnWithoutAck" Font-Size="Small" GroupName="Acknowledement" AutoPostBack="true" runat="server" onclick="Download(this);" Text="Without Acknowledgement" CssClass="spanstyle" />
                        </td>
                    </tr>
                    <tr>
                        <td width="100%" style="border: 0px!important;" colspan="2">
                            <telerik:RadEditor ID="EditorSummary" runat="server" Width="100%" Height="380px"
                                ToolsWidth="0px" StripAbsoluteImagesPaths="true" AutoResizeWidth="true" Style="background-color: White; text-align: left; word-wrap: break-word !important;"
                                EnableViewState="false" ToolbarMode="ShowOnFocus" OnClientLoad="EditorSummary_Load" class="Editabletxtbox">
                                <Modules>
                                    <telerik:EditorModule Name="RadEditorDomInspector" Visible="false" />
                                    <telerik:EditorModule Name="RadEditorHtmlInspector" Visible="false" />
                                    <telerik:EditorModule Name="RadEditorNodeInspector" Visible="false" />
                                    <telerik:EditorModule Name="RadEditorStatistics" Visible="false" />
                                </Modules>
                                <TrackChangesSettings CanAcceptTrackChanges="False" />
                                <ContextMenus>
                                    <telerik:EditorContextMenu Enabled="true"></telerik:EditorContextMenu>
                                </ContextMenus>
                                <Content>
                        
                        


                                </Content>
                            </telerik:RadEditor>
                        </td>
                    </tr>
                    <tr>
                        <td style="display:none;border:0px!important;" runat="server" id="tdWarning">
                            <label id="lblErrMsg" class="spanstyle" style="color: red; font-weight: bold;">The Registry returned Error/Warning!</label>
                            <br />
                            <label id="lbldetailedErrMsg" class="spanstyle" runat="server" style="color: red;font-size:12px;"></label>
                        </td>
                        <td style="text-align: right; border: 0px!important;">
                            <telerik:RadButton ID="btnSubmit" runat="server" Style="top: 0px; left: -1px" Text="Submit"
                                Width="90px" ButtonType="LinkButton" Cssclass="greenbutton" OnClick="btnSubmit_Click" OnClientClicking="btnSubmit_ClientClick">
                            </telerik:RadButton>
                            &nbsp; &nbsp; &nbsp;
                            <telerik:RadButton ID="btnClose" runat="server" Style="top: 0px; left: -1px" Text="Close"
                                Width="90px" ButtonType="LinkButton" Cssclass="redbutton" OnClientClicked="btnClose_Click">
                            </telerik:RadButton>
                        </td>
                    </tr>
                </table>

            </div>
            <div style="width: 525px; display: none; height: 440px;" id="DatesList" runat="server">
                <fieldset>
                    <legend class="LabelStyleBold">History Query Responses Made on Dates</legend>
                    <div id="DatesListItems" style="height: 400px; overflow-y: auto;" runat="server">
                    </div>
                </fieldset>
            </div>
            <div style="width: 530px; display: none; height: 415px; overflow: auto; font-size: 12.5px!important;" id="responseDiv" runat="server">

                <fieldset>
                    <legend class="LabelStyleBold">Patient Information</legend>
                    <table id="patientInfo" class="responseDisplay Gridbodystyle">
                        <tr>
                            <th class="Gridheaderstyle">Patient Identifier</th>
                            <th class="Gridheaderstyle">Patient Name</th>
                            <th class="Gridheaderstyle">DOB</th>
                            <th class="Gridheaderstyle">Gender</th>
                        </tr>
                    </table>
                </fieldset>
                <fieldset>
                    <legend class="LabelStyleBold">Evaluated Immunization History and Immunization Forecast</legend>
                    <table id="immScheduleInfo" class="responseDisplay Gridbodystyle">
                        <tr>
                            <th class="Gridheaderstyle">Immunization Schedule Used</th>
                        </tr>
                    </table>
                </fieldset>
                <fieldset id="fldImmHis">
                    <legend class="LabelStyleBold">Evaluated Immunization History</legend>
                    <table id="immHistoryInfo" class="responseDisplay Gridbodystyle">
                        <tr>
                            <th class="Gridheaderstyle">Vaccine Group</th>
                            <th class="Gridheaderstyle">Vaccine Administered</th>
                            <th class="Gridheaderstyle">Date Administered</th>
                            <th class="Gridheaderstyle">Valid Dose</th>
                            <th class="Gridheaderstyle">Validity Reason</th>
                            <th class="Gridheaderstyle">Completion Status</th>
                        </tr>
                    </table>
                </fieldset>
                <fieldset id="fldImmForecast">
                    <legend class="LabelStyleBold">Immunization Forecast</legend>
                    <table id="immForcastInfo" class="responseDisplay Gridbodystyle">
                        <tr>
                            <th class="Gridheaderstyle">Vaccine Group</th>
                            <th class="Gridheaderstyle">Due Date</th>
                            <th class="Gridheaderstyle">Earliest Date To Give</th>
                            <th class="Gridheaderstyle" >Latest Date To Give</th>
                        </tr>
                    </table>
                </fieldset>

            </div>
        </div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnableViewState="false">
        </asp:ScriptManager>
        <asp:HiddenField ID="hdnXmlPath" runat="server" />
        <asp:Button ID="hdnDownload" runat="server" OnClick="hdnDownload_Click" CssClass="Displaynone" />
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSExchange.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

        </asp:PlaceHolder>
    </form>
</body>
</html>
