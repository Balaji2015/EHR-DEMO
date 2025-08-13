<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmError.aspx.cs" Inherits="Acurus.Capella.UI.frmError" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title>
     Validation Error
   </title>
        <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <script src="JScripts/jquery-1.11.3.min.js" type="text/javascript"></script>

    <link href="CSS/bootstrap.min.css" rel="stylesheet" />
  <script src="JScripts/bootstrap.min.css.3.1.1.js" type="text/javascript"></script>
    <script src="JScripts/bootstrap.min3.1.1.js" type="text/javascript"></script>

  <script src="JScripts/bootstrap.min3.3.7.js" type="text/javascript"></script>
 
    
    <style type="text/css">
        ::-webkit-scrollbar {
            width: 8px;
        }

        ::-webkit-scrollbar-track {
            background-color: #c3bfbf;
        }

        ::-webkit-scrollbar-thumb {
            background-color: #707070;
        }

            ::-webkit-scrollbar-thumb:hover {
                background-color: #3d3c3a;
            }

        .highlight {
            background: yellow;
        }
          .rowspace {
            margin-top: 0.5%;
            margin-bottom: 0.5%;
           
        }
         
    </style>

</head>
<body >
    <form id="form1" runat="server">
      
      
        <div  class="rowspace">
        </div>
        <div id="xslFrame" style="margin: 0px 8px 0px 8px; border: 1px solid #92A9C5; height: 570px; display: block;overflow-y:scroll;overflow-x:scroll;" runat="server">
            <div style="margin: 3px 3px 3px 3px;">
                <asp:Xml ID="DownloadFrame"  runat="server" />
            </div>
            
        </div>
       
         <div class="rowspace">
        </div>
         <div style="float:right;padding-right:10px">
                <asp:Button ID="btnprint" runat="server" Text="Print" OnClick="btnprint_Click" />

            </div>

      
    </form>
</body>
</html>

