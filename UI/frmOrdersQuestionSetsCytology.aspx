<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmOrdersQuestionSetsCytology.aspx.cs"
    Inherits="Acurus.Capella.UI.frmOrdersQuestionSetsCytology" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Orders Question Sets Cytology</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
        #frmOrdersQuestionSetsCytology
        {
            width: 699px;
            height: 770px;
        }
        .style2
        {
            height: 19px;
        }
        .style3
        {
            width: 100%;
            height: 32px;
        }
        .style4
        {
            width: 145px;
        }
        .style5
        {
            width: 162px;
        }
        .RadInput_Default
        {
            font: 12px "segoe ui" ,arial,sans-serif;
        }
        .riSingle
        {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }
        .riSingle
        {
            position: relative;
            display: inline-block;
            white-space: nowrap;
            text-align: left;
        }
        .RadInput
        {
            vertical-align: middle;
        }
        .RadInput_Default
        {
            font: 12px "segoe ui" ,arial,sans-serif;
        }
        .riSingle
        {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }
        .riSingle
        {
            position: relative;
            display: inline-block;
            white-space: nowrap;
            text-align: left;
        }
        .RadInput
        {
            vertical-align: middle;
        }
        .RadInput_Default
        {
            font: 12px "segoe ui" ,arial,sans-serif;
        }
        .riSingle
        {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }
        .riSingle
        {
            position: relative;
            display: inline-block;
            white-space: nowrap;
            text-align: left;
        }
        .RadInput
        {
            vertical-align: middle;
        }
        .RadInput_Default
        {
            font: 12px "segoe ui" ,arial,sans-serif;
        }
        .riSingle
        {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }
        .riSingle
        {
            position: relative;
            display: inline-block;
            white-space: nowrap;
            text-align: left;
        }
        .RadInput
        {
            vertical-align: middle;
        }
        .RadInput_Default
        {
            font: 12px "segoe ui" ,arial,sans-serif;
        }
        .riSingle
        {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }
        .riSingle
        {
            position: relative;
            display: inline-block;
            white-space: nowrap;
            text-align: left;
        }
        .RadInput
        {
            vertical-align: middle;
        }
        .RadInput_Default
        {
            font: 12px "segoe ui" ,arial,sans-serif;
        }
        .riSingle
        {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }
        .riSingle
        {
            position: relative;
            display: inline-block;
            white-space: nowrap;
            text-align: left;
        }
        .RadInput
        {
            vertical-align: middle;
        }
        .RadInput_Default
        {
            font: 12px "segoe ui" ,arial,sans-serif;
        }
        .riSingle
        {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }
        .riSingle
        {
            position: relative;
            display: inline-block;
            white-space: nowrap;
            text-align: left;
        }
        .RadInput
        {
            vertical-align: middle;
        }
        .RadInput_Default
        {
            font: 12px "segoe ui" ,arial,sans-serif;
        }
        .riSingle
        {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }
        .riSingle
        {
            position: relative;
            display: inline-block;
            white-space: nowrap;
            text-align: left;
        }
        .RadInput
        {
            vertical-align: middle;
        }
        .RadInput
        {
            vertical-align: middle;
        }
        .riSingle
        {
            position: relative;
            display: inline-block;
            white-space: nowrap;
            text-align: left;
        }
        .riSingle
        {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }
        .RadInput_Default
        {
            font: 12px "segoe ui" ,arial,sans-serif;
        }
        .RadInput
        {
            vertical-align: middle;
        }
        .riSingle
        {
            position: relative;
            display: inline-block;
            white-space: nowrap;
            text-align: left;
        }
        .riSingle
        {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }
        .RadInput_Default
        {
            font: 12px "segoe ui" ,arial,sans-serif;
        }
        .riSingle .riTextBox
        {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }
        .riSingle .riTextBox
        {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }
        .riSingle .riTextBox
        {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }
        .riSingle .riTextBox
        {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }
        .riSingle .riTextBox
        {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }
        .riSingle .riTextBox
        {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }
        .riSingle .riTextBox
        {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }
        .riSingle .riTextBox
        {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }
        .riSingle .riTextBox
        {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }
        .riSingle .riTextBox
        {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }
        .RadPicker
        {
            vertical-align: middle;
        }
        .RadPicker .rcTable
        {
            table-layout: auto;
        }
        .RadPicker_Default .rcCalPopup
        {
            background-position: 0 0;
        }
        .RadPicker_Default .rcCalPopup
        {
            background-image: url(  'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Calendar.sprite.gif' );
        }
        .RadPicker .rcCalPopup
        {
            display: block;
            overflow: hidden;
            width: 22px;
            height: 22px;
            background-color: transparent;
            background-repeat: no-repeat;
            text-indent: -2222px;
            text-align: center;
        }
        .RadPicker td a
        {
            position: relative;
            outline: none;
            z-index: 2;
            margin: 0 2px;
            text-decoration: none;
        }
        .RadPicker_Default .rcTimePopup
        {
            background-position: 0 -100px;
        }
        .RadPicker_Default .rcTimePopup
        {
            background-image: url(  'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Calendar.sprite.gif' );
        }
        .RadPicker .rcTimePopup
        {
            display: block;
            overflow: hidden;
            width: 22px;
            height: 22px;
            background-color: transparent;
            background-repeat: no-repeat;
            text-indent: -2222px;
            text-align: center;
        }
        div.RadPicker table.rcSingle .rcInputCell
        {
            padding-right: 0;
        }
        .RadPicker table.rcTable .rcInputCell
        {
            padding: 0 4px 0 0;
        }
        .style8
        {
            width: 99%;
            height: 83px;
        }
        .style15
        {
            width: 54px;
        }
        .style18
        {
            width: 136px;
        }
        .RadButton_Default.rbSkinnedButton
        {
            background-image: url(  'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }
        .RadButton_Default.rbSkinnedButton
        {
            background-image: url(  'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }
        .RadButton_Default.rbSkinnedButton
        {
            background-image: url(  'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }
        .RadButton_Default.rbSkinnedButton
        {
            background-image: url(  'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }
        .RadButton_Default.rbSkinnedButton
        {
            background-image: url(  'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }
        .RadButton_Default.rbSkinnedButton
        {
            background-image: url(  'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }
        .RadButton_Default.rbSkinnedButton
        {
            background-image: url(  'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }
        .RadButton_Default.rbSkinnedButton
        {
            background-image: url(  'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }
        .RadButton_Default.rbSkinnedButton
        {
            background-image: url(  'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }
        .RadButton_Default.rbSkinnedButton
        {
            background-image: url(  'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }
        .RadButton
        {
            cursor: pointer;
        }
        .rbSkinnedButton
        {
            vertical-align: top;
        }
        .rbSkinnedButton
        {
            display: inline-block;
            position: relative;
            background-color: transparent;
            background-repeat: no-repeat;
            border: 0 none;
            height: 22px;
            text-align: center;
            text-decoration: none;
            white-space: nowrap;
            background-position: right 0;
            padding-right: 4px;
            vertical-align: top;
        }
        .RadButton
        {
            font-size: 12px;
            font-family: "Segoe UI" ,Arial,Helvetica,sans-serif;
        }
        .RadButton
        {
            cursor: pointer;
        }
        .rbSkinnedButton
        {
            vertical-align: top;
        }
        .rbSkinnedButton
        {
            display: inline-block;
            position: relative;
            background-color: transparent;
            background-repeat: no-repeat;
            border: 0 none;
            height: 22px;
            text-align: center;
            text-decoration: none;
            white-space: nowrap;
            background-position: right 0;
            padding-right: 4px;
            vertical-align: top;
        }
        .RadButton
        {
            font-size: 12px;
            font-family: "Segoe UI" ,Arial,Helvetica,sans-serif;
        }
        .RadButton
        {
            cursor: pointer;
        }
        .rbSkinnedButton
        {
            vertical-align: top;
        }
        .rbSkinnedButton
        {
            display: inline-block;
            position: relative;
            background-color: transparent;
            background-repeat: no-repeat;
            border: 0 none;
            height: 22px;
            text-align: center;
            text-decoration: none;
            white-space: nowrap;
            background-position: right 0;
            padding-right: 4px;
            vertical-align: top;
        }
        .RadButton
        {
            font-size: 12px;
            font-family: "Segoe UI" ,Arial,Helvetica,sans-serif;
        }
        .RadButton
        {
            cursor: pointer;
        }
        .rbSkinnedButton
        {
            vertical-align: top;
        }
        .rbSkinnedButton
        {
            display: inline-block;
            position: relative;
            background-color: transparent;
            background-repeat: no-repeat;
            border: 0 none;
            height: 22px;
            text-align: center;
            text-decoration: none;
            white-space: nowrap;
            background-position: right 0;
            padding-right: 4px;
            vertical-align: top;
        }
        .RadButton
        {
            font-size: 12px;
            font-family: "Segoe UI" ,Arial,Helvetica,sans-serif;
        }
        .RadButton
        {
            cursor: pointer;
        }
        .rbSkinnedButton
        {
            vertical-align: top;
        }
        .rbSkinnedButton
        {
            display: inline-block;
            position: relative;
            background-color: transparent;
            background-repeat: no-repeat;
            border: 0 none;
            height: 22px;
            text-align: center;
            text-decoration: none;
            white-space: nowrap;
            background-position: right 0;
            padding-right: 4px;
            vertical-align: top;
        }
        .RadButton
        {
            font-size: 12px;
            font-family: "Segoe UI" ,Arial,Helvetica,sans-serif;
        }
        .RadButton
        {
            cursor: pointer;
        }
        .rbSkinnedButton
        {
            vertical-align: top;
        }
        .rbSkinnedButton
        {
            display: inline-block;
            position: relative;
            background-color: transparent;
            background-repeat: no-repeat;
            border: 0 none;
            height: 22px;
            text-align: center;
            text-decoration: none;
            white-space: nowrap;
            background-position: right 0;
            padding-right: 4px;
            vertical-align: top;
        }
        .RadButton
        {
            font-size: 12px;
            font-family: "Segoe UI" ,Arial,Helvetica,sans-serif;
        }
        .RadButton
        {
            font-size: 12px;
            font-family: "Segoe UI" ,Arial,Helvetica,sans-serif;
        }
        .rbSkinnedButton
        {
            display: inline-block;
            position: relative;
            background-color: transparent;
            background-repeat: no-repeat;
            border: 0 none;
            height: 22px;
            text-align: center;
            text-decoration: none;
            white-space: nowrap;
            background-position: right 0;
            padding-right: 4px;
            vertical-align: top;
        }
        .rbSkinnedButton
        {
            vertical-align: top;
        }
        .RadButton
        {
            cursor: pointer;
        }
        .RadButton
        {
            font-size: 12px;
            font-family: "Segoe UI" ,Arial,Helvetica,sans-serif;
        }
        .rbSkinnedButton
        {
            display: inline-block;
            position: relative;
            background-color: transparent;
            background-repeat: no-repeat;
            border: 0 none;
            height: 22px;
            text-align: center;
            text-decoration: none;
            white-space: nowrap;
            background-position: right 0;
            padding-right: 4px;
            vertical-align: top;
        }
        .rbSkinnedButton
        {
            vertical-align: top;
        }
        .RadButton
        {
            cursor: pointer;
        }
        .RadButton
        {
            font-size: 12px;
            font-family: "Segoe UI" ,Arial,Helvetica,sans-serif;
        }
        .rbSkinnedButton
        {
            display: inline-block;
            position: relative;
            background-color: transparent;
            background-repeat: no-repeat;
            border: 0 none;
            height: 22px;
            text-align: center;
            text-decoration: none;
            white-space: nowrap;
            background-position: right 0;
            padding-right: 4px;
            vertical-align: top;
        }
        .rbSkinnedButton
        {
            vertical-align: top;
        }
        .RadButton
        {
            cursor: pointer;
        }
        .RadButton
        {
            font-size: 12px;
            font-family: "Segoe UI" ,Arial,Helvetica,sans-serif;
        }
        .rbSkinnedButton
        {
            display: inline-block;
            position: relative;
            background-color: transparent;
            background-repeat: no-repeat;
            border: 0 none;
            height: 22px;
            text-align: center;
            text-decoration: none;
            white-space: nowrap;
            background-position: right 0;
            padding-right: 4px;
            vertical-align: top;
        }
        .rbSkinnedButton
        {
            vertical-align: top;
        }
        .RadButton
        {
            cursor: pointer;
        }
        .RadButton_Default .rbDecorated
        {
            background-image: url(  'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }
        .RadButton_Default .rbDecorated
        {
            background-image: url(  'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }
        .RadButton_Default .rbDecorated
        {
            background-image: url(  'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }
        .RadButton_Default .rbDecorated
        {
            background-image: url(  'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }
        .RadButton_Default .rbDecorated
        {
            background-image: url(  'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }
        .RadButton_Default .rbDecorated
        {
            background-image: url(  'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }
        .RadButton_Default .rbDecorated
        {
            background-image: url(  'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }
        .RadButton_Default .rbDecorated
        {
            background-image: url(  'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }
        .RadButton_Default .rbDecorated
        {
            background-image: url(  'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }
        .RadButton_Default .rbDecorated
        {
            background-image: url(  'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }
        .rbDecorated
        {
            display: block;
            height: 22px;
            padding-right: 6px;
            padding-left: 10px;
            border: 0;
            text-align: center;
            background-position: left -22px;
            overflow: visible;
            background-color: transparent;
            outline: none;
            cursor: pointer;
            -webkit-border-radius: 0;
        }
        .rbDecorated
        {
            font-size: 12px;
            font-family: "Segoe UI" ,Arial,Helvetica,sans-serif;
        }
        .rbDecorated
        {
            display: block;
            height: 22px;
            padding-right: 6px;
            padding-left: 10px;
            border: 0;
            text-align: center;
            background-position: left -22px;
            overflow: visible;
            background-color: transparent;
            outline: none;
            cursor: pointer;
            -webkit-border-radius: 0;
        }
        .rbDecorated
        {
            font-size: 12px;
            font-family: "Segoe UI" ,Arial,Helvetica,sans-serif;
        }
        .rbDecorated
        {
            display: block;
            height: 22px;
            padding-right: 6px;
            padding-left: 10px;
            border: 0;
            text-align: center;
            background-position: left -22px;
            overflow: visible;
            background-color: transparent;
            outline: none;
            cursor: pointer;
            -webkit-border-radius: 0;
        }
        .rbDecorated
        {
            font-size: 12px;
            font-family: "Segoe UI" ,Arial,Helvetica,sans-serif;
        }
        .rbDecorated
        {
            display: block;
            height: 22px;
            padding-right: 6px;
            padding-left: 10px;
            border: 0;
            text-align: center;
            background-position: left -22px;
            overflow: visible;
            background-color: transparent;
            outline: none;
            cursor: pointer;
            -webkit-border-radius: 0;
        }
        .rbDecorated
        {
            font-size: 12px;
            font-family: "Segoe UI" ,Arial,Helvetica,sans-serif;
        }
        .rbDecorated
        {
            display: block;
            height: 22px;
            padding-right: 6px;
            padding-left: 10px;
            border: 0;
            text-align: center;
            background-position: left -22px;
            overflow: visible;
            background-color: transparent;
            outline: none;
            cursor: pointer;
            -webkit-border-radius: 0;
        }
        .rbDecorated
        {
            font-size: 12px;
            font-family: "Segoe UI" ,Arial,Helvetica,sans-serif;
        }
        .rbDecorated
        {
            display: block;
            height: 22px;
            padding-right: 6px;
            padding-left: 10px;
            border: 0;
            text-align: center;
            background-position: left -22px;
            overflow: visible;
            background-color: transparent;
            outline: none;
            cursor: pointer;
            -webkit-border-radius: 0;
        }
        .rbDecorated
        {
            font-size: 12px;
            font-family: "Segoe UI" ,Arial,Helvetica,sans-serif;
        }
        .rbDecorated
        {
            font-size: 12px;
            font-family: "Segoe UI" ,Arial,Helvetica,sans-serif;
        }
        .rbDecorated
        {
            display: block;
            height: 22px;
            padding-right: 6px;
            padding-left: 10px;
            border: 0;
            text-align: center;
            background-position: left -22px;
            overflow: visible;
            background-color: transparent;
            outline: none;
            cursor: pointer;
            -webkit-border-radius: 0;
        }
        .rbDecorated
        {
            font-size: 12px;
            font-family: "Segoe UI" ,Arial,Helvetica,sans-serif;
        }
        .rbDecorated
        {
            display: block;
            height: 22px;
            padding-right: 6px;
            padding-left: 10px;
            border: 0;
            text-align: center;
            background-position: left -22px;
            overflow: visible;
            background-color: transparent;
            outline: none;
            cursor: pointer;
            -webkit-border-radius: 0;
        }
        .rbDecorated
        {
            font-size: 12px;
            font-family: "Segoe UI" ,Arial,Helvetica,sans-serif;
        }
        .rbDecorated
        {
            display: block;
            height: 22px;
            padding-right: 6px;
            padding-left: 10px;
            border: 0;
            text-align: center;
            background-position: left -22px;
            overflow: visible;
            background-color: transparent;
            outline: none;
            cursor: pointer;
            -webkit-border-radius: 0;
        }
        .rbDecorated
        {
            font-size: 12px;
            font-family: "Segoe UI" ,Arial,Helvetica,sans-serif;
        }
        .rbDecorated
        {
            display: block;
            height: 22px;
            padding-right: 6px;
            padding-left: 10px;
            border: 0;
            text-align: center;
            background-position: left -22px;
            overflow: visible;
            background-color: transparent;
            outline: none;
            cursor: pointer;
            -webkit-border-radius: 0;
        }
        .style22
        {
            width: 598px;
            height: 39px;
        }
        .style29
        {
            width: 115px;
        }
        .style30
        {
            width: 63px;
        }
        .style31
        {
            width: 61px;
        }
        .style32
        {
            width: 58px;
        }
        .style33
        {
            width: 56px;
        }
        .style34
        {
            width: 55px;
        }
        .style35
        {
            width: 62px;
        }
        .style39
        {
            width: 60px;
        }
        .style47
        {
            height: 39px;
        }
        .style49
        {
            width: 64px;
            margin-left: 40px;
        }
        .style50
        {
            width: 60px;
            margin-left: 40px;
        }
        .style51
        {
            width: 62px;
            margin-left: 40px;
        }
        .style52
        {
            width: 59px;
        }
        .style53
        {
            width: 115px;
            height: 26px;
        }
        .style55
        {
            width: 63px;
            height: 26px;
        }
        .style56
        {
            width: 136px;
            height: 26px;
        }
        .style57
        {
            width: 65px;
            margin-left: 40px;
            height: 26px;
        }
        .style59
        {
            width: 145px;
            height: 26px;
        }
        .style60
        {
            width: 61px;
            height: 26px;
        }
        .style61
        {
            width: 65px;
            margin-left: 40px;
        }
        .style62
        {
            width: 67px;
            height: 26px;
        }
        .style63
        {
            width: 67px;
        }
        .style64
        {
        }
        .style70
        {
            width: 121px;
            height: 26px;
        }
        .style71
        {
            width: 121px;
        }
        .style74
        {
            height: 42px;
        }
        .style78
        {
            height: 80px;
        }
        .style79
        {
            height: 34px;
        }
        .style82
        {
            height: 66px;
        }
        .style86
        {
            width: 86px;
            height: 26px;
        }
        .style87
        {
            width: 86px;
        }
        .style89
        {
            width: 69px;
        }
        .style90
        {
            width: 122px;
        }
        .style92
        {
            width: 103px;
        }
        .style95
        {
            width: 57px;
        }
        .style96
        {
            width: 123px;
        }
        .style97
        {
            width: 105px;
        }
        .style98
        {
            width: 104px;
        }
        .style99
        {
            width: 163px;
        }
        .style100
        {
            width: 142px;
        }
    </style>

    <script language="javascript" type="text/javascript">
    
    </script>

</head>
<body>
    <form runat="server">
    <telerik:RadAjaxPanel ID="AjxQuestionSetAFP" runat="server">
    <telerik:RadWindowManager ID="WindowMngr" runat="server" VisibleStatusbar="false"
            EnableViewState="false">
            <Windows>
                <telerik:RadWindow ID="MessageWindow" runat="server" Behaviors="Close" Title="Add/Update Keywords">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <div style="height: 100%; width: 100%">
            <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
            </telerik:RadScriptManager>
            <asp:Panel ID="Panel1" runat="server" Height="100%" Width="100%">
                <table>
                    <tr>
                        <td colspan="5">
                            <asp:Panel ID="Panel2" runat="server">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblLMPDate" runat="server" Text="LMP-Meno Date" Width="120px"></asp:Label>
                                        </td>
                                        <td>
                                            <telerik:RadDatePicker ID="dtpLMPDate" runat="server" OnSelectedDateChanged="dtpLMPDate_SelectedDateChanged">
                                                <Calendar UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False" ViewSelectorText="x">
                                                </Calendar>
                                                <DatePopupButton HoverImageUrl="" ImageUrl="" />
                                                <DateInput DateFormat="dd-MMM-yyyy" DisplayDateFormat="dd-MMM-yyyy" DisplayText=""
                                                    LabelWidth="40%" type="text" value="">
                                                </DateInput></telerik:RadDatePicker>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <asp:Panel ID="pnlGynSource" runat="server" GroupingText="Gyn Source">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblCervical" runat="server" Text="Cervical" Width="122%"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkYesCervical" runat="server" Height="22px" onclick="CheckChangeCyt('chkNoCervical');"
                                                Style="margin-left: 0px" Text="Yes" Width="100%" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkNoCervical" runat="server" Text="No" Width="100%" onclick="CheckChangeCyt('chkYesCervical');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblEndocervical" runat="server" Text="Endocervical" Width="114%"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkYesEndocervical" runat="server" Text="Yes" Width="100%" onclick="CheckChangeCyt('chkNoEndocervical');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkNoEndocervical" runat="server" Text="No" Width="100%" onclick="CheckChangeCyt('chkYesEndocervical');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblLabiaVulva" runat="server" Text="Labia-Vulva" Width="100%"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkYesLabiaVulva" runat="server" Text="Yes" Width="55px" onclick="CheckChangeCyt('chkNoLabiaVulva');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkNoLabiaVulva" runat="server" Text="No" Width="100%" onclick="CheckChangeCyt('chkYesLabiaVulva');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblVaginal" runat="server" Text="Vaginal" Width="100%"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkYesVaginal" runat="server" Text="Yes" Width="100%" onclick="CheckChangeCyt('chkNoVaginal');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkNoVaginal" runat="server" Text="No" Width="100%" onclick="CheckChangeCyt('chkYesVaginal');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblEndometrial" runat="server" Style="margin-top: 12px; margin-right: 3px;"
                                                Text="Endometrial" Width="119%"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkYesEndometrial" runat="server" Text="Yes" Width="100%" onclick="CheckChangeCyt('chkNoEndometrial');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkNoEndometrial" runat="server" Style="margin-right: 1px" Text="No"
                                                Width="100%" onclick="CheckChangeCyt('chkYesEndometrial');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblHysterectomySupracervical" runat="server" Text="Hysterectomy, Supracervical"
                                                Width="100%"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkYesHysterectomySupracervical" runat="server" Text="Yes" Width="55px"
                                                onclick="CheckChangeCyt('chkNoHysterectomySupracervical');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkNoHysterectomySupracervical" runat="server" Text="No" Width="100%"
                                                onclick="CheckChangeCyt('chkYesHysterectomySupracervical');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <asp:Panel ID="pnlCollectiontechnique" runat="server" GroupingText="Collection technique">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblSwabSpatula" runat="server" Text="Swab-Spatula" Width="122%"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkYesSwabSpatula" runat="server" Height="22px" Style="margin-left: 0px"
                                                Text="Yes" Width="100%" onclick="CheckChangeCyt('chkNoSwabSpatula');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkNoSwabSpatula" runat="server" Text="No" Width="100%" onclick="CheckChangeCyt('chkYesSwabSpatula');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblBrushSpatula" runat="server" Text="Brush-Spatula" Width="114%"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkYesBrushSpatula" runat="server" Text="Yes" Width="100%" onclick="CheckChangeCyt('chkNoBrushSpatula');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkNoBrushSpatula" runat="server" Text="No" Width="100%" onclick="CheckChangeCyt('chkYesBrushSpatula');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblSpatulaAlone" runat="server" Text="Spatula-Alone" Width="100%"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkYesSpatulaAlone" runat="server" Text="Yes" Width="100%" onclick="CheckChangeCyt('chkNoSpatulaAlone');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkNoSpatulaAlone" runat="server" Text="No" Width="100%" onclick="CheckChangeCyt('chkYesSpatulaAlone');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblBrushAlone" runat="server" Text="Brush-Alone" Width="100%"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkYesBrushAlone" runat="server" Text="Yes" Width="100%" onclick="CheckChangeCyt('chkNoBrushAlone');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkNoBrushAlone" runat="server" Text="No" Width="100%" onclick="CheckChangeCyt('chkYesBrushAlone');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblBroomAlone" runat="server" Style="margin-top: 12px; margin-right: 3px;"
                                                Text="Broom-Alone" Width="119%"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkYesBroomAlone" runat="server" Text="Yes" Width="100%" onclick="CheckChangeCyt('chkNoBroomAlone');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkNoBroomAlone" runat="server" Text="No" Width="100%" Style="margin-right: 1px"
                                                onclick="CheckChangeCyt('chkYesBroomAlone');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblOtheCollectionTechnique" runat="server" Text="Other Collection Technique "
                                                Width="100%"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkYesOtheCollectionTechnique" runat="server" Text="Yes" Width="100%"
                                                onclick="CheckChangeCyt('chkNoOtheCollectionTechnique');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkNoOtheCollectionTechnique" runat="server" Text="No" Width="100%"
                                                onclick="CheckChangeCyt('chkYesOtheCollectionTechnique');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <asp:Panel ID="pnlPreviousTreatment" runat="server" GroupingText="Previous Treatment">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblNone" runat="server" Text="None" Width="100%"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkYesNone" runat="server" Height="22px" Style="margin-left: 0px"
                                                Text="Yes" Width="100%" onclick="CheckChangeCyt('chkNoNone');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkNoNone" runat="server" Text="No" Width="100%" onclick="CheckChangeCyt('chkYesNone');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblHyst" runat="server" Text="Hyst" Width="100%"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkYesHyst" runat="server" Text="Yes" Width="100%" onclick="CheckChangeCyt('chkNoHyst');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkNoHyst" runat="server" Text="No" Width="100%" onclick="CheckChangeCyt('chkYesHyst');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblConiza" runat="server" Text="Coniza" Width="100%"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkYesConiza" runat="server" Text="Yes" Width="100%" onclick="CheckChangeCyt('chkNoConiza');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkNoConiza" runat="server" Text="No" Width="100%" onclick="CheckChangeCyt('chkYesConiza');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblColpBX" runat="server" Text="Colp-BX" Width="100%"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkYesColpBX" runat="server" Text="Yes" Width="100%" onclick="CheckChangeCyt('chkNoColpBX');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkNoColpBX" runat="server" Text="No" Width="100%" Style="margin-right: 16px"
                                                onclick="CheckChangeCyt('chkYesColpBX');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblLaserVap" runat="server" Style="margin-top: 12px" Text="Laser-Vap"
                                                Width="100%"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkYesLaserVap" runat="server" Text="Yes" Width="100%" onclick="CheckChangeCyt('chkNoLaserVap');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkNoLaserVap" runat="server" Text="No" Width="100%" onclick="CheckChangeCyt('chkYesLaserVap');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCyro" runat="server" Text="Cyro" Width="100%"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkYesCyro" runat="server" Text="Yes" Width="100%" onclick="CheckChangeCyt('chkNoCyro');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkNoCyro" runat="server" Text="No" Width="100%" onclick="CheckChangeCyt('chkYesCyro');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblRadiation" runat="server" Text="Radiation" Width="100%"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkYesRadiation" runat="server" Text="Yes" Width="100%" onclick="CheckChangeCyt('chkNoRadiation');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkNoRadiation" runat="server" Text="No" Width="100%" onclick="CheckChangeCyt('chkYesRadiation');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <asp:Panel ID="gbPreviousCytologyInformation" runat="server" GroupingText="Previous Cytology Information">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblNegative" runat="server" Text="Negative" Width="100%"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkYesNegative" runat="server" Height="22px" Style="margin-left: 0px;
                                                margin-right: 0px;" Text="Yes" Width="100%" onclick="CheckChangeCyt('chkNoNegative');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkNoNegative" runat="server" Text="No" Width="100%" onclick="CheckChangeCyt('chkYesNegative');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblAtypical" runat="server" Text="Atypical" Width="100%"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkyesAtypical" runat="server" Text="Yes" Width="100%" onclick="CheckChangeCyt('chkNoAtypical');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkNoAtypical" runat="server" Text="No" Width="100%" onclick="CheckChangeCyt('chkyesAtypical');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblDysplasia" runat="server" Text="Dysplasia" Width="100%"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkYesDysplasia" runat="server" Text="Yes" Width="100%" onclick="CheckChangeCyt('chkNoDysplasia');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkNoDysplasia" runat="server" Text="No" Width="100%" onclick="CheckChangeCyt('chkYesDysplasia');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblCaInSitu" runat="server" Text="Ca-In-Situ" Width="100%"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkYesCaInSitu" runat="server" Text="Yes" Width="100%" onclick="CheckChangeCyt('chkNoCaInSitu');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkNoCaInSitu" runat="server" Text="No" Width="100%" onclick="CheckChangeCyt('chkYesCaInSitu');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblInvasive" runat="server" Style="margin-top: 12px" Text="Invasive"
                                                Width="100%"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkYesInvasive" runat="server" Text="Yes" Width="100%" onclick="CheckChangeCyt('chkNoInvasive');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkNoInvasive" runat="server" Text="No" Width="100%" onclick="CheckChangeCyt('chkYesInvasive');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblOthePreviousInformation" runat="server" Text="Other Previous Information"
                                                Width="100%"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkYesOthePreviousInformation" runat="server" Text="Yes" Width="100%"
                                                onclick="CheckChangeCyt('chkNoOthePreviousInformation');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkNoOthePreviousInformation" runat="server" Text="No" Width="100%"
                                                onclick="CheckChangeCyt('chkYesOthePreviousInformation');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblDatesResults" runat="server" Text="Dates-Results" Width="130%"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <telerik:RadTextBox ID="txtDatesResults" runat="server" Height="18px" Width="100%">
                                            </telerik:RadTextBox>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <asp:Panel ID="pnlOtherPatientInformation" runat="server" GroupingText="Other Patient Information">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblPregnant" runat="server" Text="Pregnant" Width="100%" Style="margin-top: 0px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkYesPregnant" runat="server" Height="22px" Style="margin-left: 0px"
                                                Text="Yes" Width="100%" onclick="CheckChangeCyt('chkNoPregnant');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkNoPregnant" runat="server" Text="No" Width="100%" Style="margin-right: 0px"
                                                onclick="CheckChangeCyt('chkYesPregnant');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblLactating" runat="server" Text="Lactating" Width="100%"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkYesLactating" runat="server" Text="Yes" Width="100%" onclick="CheckChangeCyt('chkNoLactating');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkNoLactating" runat="server" Text="No" Width="100%" onclick="CheckChangeCyt('chkYesLactating');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblOralContraceptives" runat="server" Text="Oral Contraceptives" Width="100%"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkYesOralContraceptives" runat="server" Text="Yes" Width="100%"
                                                onclick="CheckChangeCyt('chkNoOralContraceptives');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkNoOralContraceptives" runat="server" Text="No" Width="100%"
                                                onclick="CheckChangeCyt('chkYesOralContraceptives');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblMenopausal" runat="server" Text="Menopausal" Width="100%"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkYesMenopausal" runat="server" Text="Yes" Width="100%" onclick="CheckChangeCyt('chkNoMenopausal');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkNoMenopausal" runat="server" Text="No" Width="100%" onclick="CheckChangeCyt('chkYesMenopausal');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblEstroRX" runat="server" Style="margin-top: 12px" Text="Estro-RX"
                                                Width="100%"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkYesEstroRX" runat="server" Text="Yes" Width="100%" onclick="CheckChangeCyt('chkNoEstroRX');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkNoEstroRX" runat="server" Text="No" Width="100%" onclick="CheckChangeCyt('chkYesEstroRX');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblPMPBleeding" runat="server" Text="PMP-Bleeding" Width="100%"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkYesPMPBleeding" runat="server" Text="Yes" Width="100%" onclick="CheckChangeCyt('chkNoPMPBleeding');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkNoPMPBleeding" runat="server" Text="No" Width="100%" onclick="CheckChangeCyt('chkYesPMPBleeding');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblPostPart" runat="server" Text="Post-Part" Width="100%"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkYesPostPart" runat="server" Text="Yes" Width="100%" onclick="CheckChangeCyt('chkNoPostPart');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkNoPostPart" runat="server" Text="No" Width="100%" onclick="CheckChangeCyt('chkYesPostPart');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblIUD" runat="server" Style="margin-top: 12px" Text="IUD" Width="100%"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkYesIUD" runat="server" Text="Yes" Width="100%" Height="22px"
                                                onclick="CheckChangeCyt('chkNoIUD');" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkNoIUD" runat="server" Text="No" Width="100%" onclick="CheckChangeCyt('chkYesIUD');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblAllOtherPat" runat="server" Style="margin-top: 12px" Text="All-Other-Pat"
                                                Width="100%"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkYesAllOtherPat" runat="server" Text="Yes" Width="100%" onclick="CheckChangeCyt('chkNoAllOtherPat');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkNoAllOtherPat" runat="server" Text="No" Width="100%" onclick="CheckChangeCyt('chkYesAllOtherPat');" OnCheckedChanged="chkYesCervical_CheckedChanged" AutoPostBack="true"/>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td style="width: 800px;">
                        </td>
                        <td>
                            <telerik:RadButton ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" OnClientClicked="GetUTCTime">
                            </telerik:RadButton>
                        </td>
                        <td>
                            <telerik:RadButton ID="btnClearAll" runat="server" OnClientClicked="ClearAllCytology"
                                Text="Clear All">
                            </telerik:RadButton>
                        </td>
                        <td>
                            <telerik:RadButton ID="btnCancel" runat="server" OnClientClicked="CloseQuestionSetCytology"
                                Text="Cancel">
                            </telerik:RadButton>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
         <asp:HiddenField ID="hdnLocalTime" runat="server" />
         <asp:HiddenField ID="hdnMessageType" runat="server" />
     <asp:Button ID="btnMessageType" runat="server" Text="Button" Style="display: none" OnClientClick="CloseQuestionSetCytology();"/>
    </telerik:RadAjaxPanel>
   
<asp:PlaceHolder ID="PlaceHolder1" runat="server">
    <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

    <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

    <script src="JScripts/JSOrdersQuestionSetsAFP.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

    <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
   </asp:PlaceHolder>
    </form>
</body>
</html>
