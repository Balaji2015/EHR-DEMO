<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmFlowSheetManager.aspx.cs"
    Inherits="Acurus.Capella.UI.frmFlowSheetManager" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
      <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <title>Flow Sheet Manager</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <base target="_self" />
    <style type="text/css">
        #frmFlowSheetManager {
            width: 788px;
        }

        .style1 {
            width: 85%;
            height: 462px;
        }

        .style2 {
            height: 183px;
        }

        .style4 {
            width: 100%;
            height: 159px;
        }

        .style6 {
            height: 24px;
        }

        .style7 {
            height: 24px;
            width: 220px;
        }

        .RadInput_Default {
            font: 12px "segoe ui",arial,sans-serif;
        }

        .riSingle {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }

        .riSingle {
            position: relative;
            display: inline-block;
            white-space: nowrap;
            text-align: left;
        }

        .RadInput {
            vertical-align: middle;
        }

        .RadInput_Default {
            font: 12px "segoe ui",arial,sans-serif;
        }

        .riSingle {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }

        .riSingle {
            position: relative;
            display: inline-block;
            white-space: nowrap;
            text-align: left;
        }

        .RadInput {
            vertical-align: middle;
        }

        .RadInput_Default {
            font: 12px "segoe ui",arial,sans-serif;
        }

        .riSingle {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }

        .riSingle {
            position: relative;
            display: inline-block;
            white-space: nowrap;
            text-align: left;
        }

        .RadInput {
            vertical-align: middle;
        }

        .RadInput_Default {
            font: 12px "segoe ui",arial,sans-serif;
        }

        .riSingle {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }

        .riSingle {
            position: relative;
            display: inline-block;
            white-space: nowrap;
            text-align: left;
        }

        .RadInput {
            vertical-align: middle;
        }

        .RadInput_Default {
            font: 12px "segoe ui",arial,sans-serif;
        }

        .riSingle {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }

        .riSingle {
            position: relative;
            display: inline-block;
            white-space: nowrap;
            text-align: left;
        }

        .RadInput {
            vertical-align: middle;
        }

        .RadInput_Default {
            font: 12px "segoe ui",arial,sans-serif;
        }

        .riSingle {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }

        .riSingle {
            position: relative;
            display: inline-block;
            white-space: nowrap;
            text-align: left;
        }

        .RadInput {
            vertical-align: middle;
        }

        .RadInput_Default {
            font: 12px "segoe ui",arial,sans-serif;
        }

        .riSingle {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }

        .riSingle {
            position: relative;
            display: inline-block;
            white-space: nowrap;
            text-align: left;
        }

        .RadInput {
            vertical-align: middle;
        }

        .RadInput_Default {
            font: 12px "segoe ui",arial,sans-serif;
        }

        .riSingle {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }

        .riSingle {
            position: relative;
            display: inline-block;
            white-space: nowrap;
            text-align: left;
        }

        .RadInput {
            vertical-align: middle;
        }

        .RadInput {
            vertical-align: middle;
        }

        .riSingle {
            position: relative;
            display: inline-block;
            white-space: nowrap;
            text-align: left;
        }

        .riSingle {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }

        .RadInput_Default {
            font: 12px "segoe ui",arial,sans-serif;
        }

        .RadInput {
            vertical-align: middle;
        }

        .riSingle {
            position: relative;
            display: inline-block;
            white-space: nowrap;
            text-align: left;
        }

        .riSingle {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }

        .RadInput_Default {
            font: 12px "segoe ui",arial,sans-serif;
        }

        .riSingle .riTextBox {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }

        .riSingle .riTextBox {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }

        .riSingle .riTextBox {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }

        .riSingle .riTextBox {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }

        .riSingle .riTextBox {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }

        .riSingle .riTextBox {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }

        .riSingle .riTextBox {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }

        .riSingle .riTextBox {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }

        .riSingle .riTextBox {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }

        .riSingle .riTextBox {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }

        .style8 {
            height: 180px;
        }

        .style9 {
            height: 24px;
            width: 239px;
        }

        .style11 {
            height: 24px;
            width: 40px;
        }

        .style12 {
            width: 100%;
            height: 158px;
        }

        .RadButton_Default.rbSkinnedButton {
            background-image: url( 'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }

        .RadButton_Default.rbSkinnedButton {
            background-image: url( 'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }

        .RadButton_Default.rbSkinnedButton {
            background-image: url( 'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }

        .RadButton_Default.rbSkinnedButton {
            background-image: url( 'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }

        .RadButton_Default.rbSkinnedButton {
            background-image: url( 'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }

        .RadButton_Default.rbSkinnedButton {
            background-image: url( 'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }

        .RadButton_Default.rbSkinnedButton {
            background-image: url( 'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }

        .RadButton_Default.rbSkinnedButton {
            background-image: url( 'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }

        .RadButton_Default.rbSkinnedButton {
            background-image: url( 'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }

        .RadButton_Default.rbSkinnedButton {
            background-image: url( 'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }

        .RadButton_Default.rbSkinnedButton {
            background-image: url( 'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }

        .RadButton_Default.rbSkinnedButton {
            background-image: url( 'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }

        .RadButton_Default.rbSkinnedButton {
            background-image: url( 'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }

        .RadButton_Default.rbSkinnedButton {
            background-image: url( 'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }

        .RadButton {
            cursor: pointer;
        }

        .rbSkinnedButton {
            vertical-align: top;
        }

        .rbSkinnedButton {
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

        .RadButton {
            font-size: 12px;
            font-family: "Segoe UI",Arial,Helvetica,sans-serif;
        }

        .RadButton {
            cursor: pointer;
        }

        .rbSkinnedButton {
            vertical-align: top;
        }

        .rbSkinnedButton {
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

        .RadButton {
            font-size: 12px;
            font-family: "Segoe UI",Arial,Helvetica,sans-serif;
        }

        .RadButton {
            cursor: pointer;
        }

        .rbSkinnedButton {
            vertical-align: top;
        }

        .rbSkinnedButton {
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

        .RadButton {
            font-size: 12px;
            font-family: "Segoe UI",Arial,Helvetica,sans-serif;
        }

        .RadButton {
            cursor: pointer;
        }

        .rbSkinnedButton {
            vertical-align: top;
        }

        .rbSkinnedButton {
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

        .RadButton {
            font-size: 12px;
            font-family: "Segoe UI",Arial,Helvetica,sans-serif;
        }

        .RadButton {
            cursor: pointer;
        }

        .rbSkinnedButton {
            vertical-align: top;
        }

        .rbSkinnedButton {
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

        .RadButton {
            font-size: 12px;
            font-family: "Segoe UI",Arial,Helvetica,sans-serif;
        }

        .RadButton {
            cursor: pointer;
        }

        .rbSkinnedButton {
            vertical-align: top;
        }

        .rbSkinnedButton {
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

        .RadButton {
            font-size: 12px;
            font-family: "Segoe UI",Arial,Helvetica,sans-serif;
        }

        .RadButton {
            cursor: pointer;
        }

        .rbSkinnedButton {
            vertical-align: top;
        }

        .rbSkinnedButton {
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

        .RadButton {
            font-size: 12px;
            font-family: "Segoe UI",Arial,Helvetica,sans-serif;
        }

        .RadButton {
            cursor: pointer;
        }

        .rbSkinnedButton {
            vertical-align: top;
        }

        .rbSkinnedButton {
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

        .RadButton {
            font-size: 12px;
            font-family: "Segoe UI",Arial,Helvetica,sans-serif;
        }

        .RadButton {
            font-size: 12px;
            font-family: "Segoe UI",Arial,Helvetica,sans-serif;
        }

        .rbSkinnedButton {
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

        .rbSkinnedButton {
            vertical-align: top;
        }

        .RadButton {
            cursor: pointer;
        }

        .RadButton {
            font-size: 12px;
            font-family: "Segoe UI",Arial,Helvetica,sans-serif;
        }

        .rbSkinnedButton {
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

        .rbSkinnedButton {
            vertical-align: top;
        }

        .RadButton {
            cursor: pointer;
        }

        .RadButton {
            font-size: 12px;
            font-family: "Segoe UI",Arial,Helvetica,sans-serif;
        }

        .rbSkinnedButton {
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

        .rbSkinnedButton {
            vertical-align: top;
        }

        .RadButton {
            cursor: pointer;
        }

        .RadButton {
            font-size: 12px;
            font-family: "Segoe UI",Arial,Helvetica,sans-serif;
        }

        .rbSkinnedButton {
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

        .rbSkinnedButton {
            vertical-align: top;
        }

        .RadButton {
            cursor: pointer;
        }

        .RadButton {
            font-size: 12px;
            font-family: "Segoe UI",Arial,Helvetica,sans-serif;
        }

        .rbSkinnedButton {
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

        .rbSkinnedButton {
            vertical-align: top;
        }

        .RadButton {
            cursor: pointer;
        }

        .RadButton {
            font-size: 12px;
            font-family: "Segoe UI",Arial,Helvetica,sans-serif;
        }

        .rbSkinnedButton {
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

        .rbSkinnedButton {
            vertical-align: top;
        }

        .RadButton {
            cursor: pointer;
        }

        .RadButton_Default .rbDecorated {
            background-image: url( 'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }

        .RadButton_Default .rbDecorated {
            background-image: url( 'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }

        .RadButton_Default .rbDecorated {
            background-image: url( 'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }

        .RadButton_Default .rbDecorated {
            background-image: url( 'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }

        .RadButton_Default .rbDecorated {
            background-image: url( 'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }

        .RadButton_Default .rbDecorated {
            background-image: url( 'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }

        .RadButton_Default .rbDecorated {
            background-image: url( 'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }

        .RadButton_Default .rbDecorated {
            background-image: url( 'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }

        .RadButton_Default .rbDecorated {
            background-image: url( 'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }

        .RadButton_Default .rbDecorated {
            background-image: url( 'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }

        .RadButton_Default .rbDecorated {
            background-image: url( 'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }

        .RadButton_Default .rbDecorated {
            background-image: url( 'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }

        .RadButton_Default .rbDecorated {
            background-image: url( 'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }

        .RadButton_Default .rbDecorated {
            background-image: url( 'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }

        .rbDecorated {
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

        .rbDecorated {
            font-size: 12px;
            font-family: "Segoe UI",Arial,Helvetica,sans-serif;
        }

        .rbDecorated {
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

        .rbDecorated {
            font-size: 12px;
            font-family: "Segoe UI",Arial,Helvetica,sans-serif;
        }

        .rbDecorated {
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

        .rbDecorated {
            font-size: 12px;
            font-family: "Segoe UI",Arial,Helvetica,sans-serif;
        }

        .rbDecorated {
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

        .rbDecorated {
            font-size: 12px;
            font-family: "Segoe UI",Arial,Helvetica,sans-serif;
        }

        .rbDecorated {
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

        .rbDecorated {
            font-size: 12px;
            font-family: "Segoe UI",Arial,Helvetica,sans-serif;
        }

        .rbDecorated {
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

        .rbDecorated {
            font-size: 12px;
            font-family: "Segoe UI",Arial,Helvetica,sans-serif;
        }

        .rbDecorated {
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

        .rbDecorated {
            font-size: 12px;
            font-family: "Segoe UI",Arial,Helvetica,sans-serif;
        }

        .rbDecorated {
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

        .rbDecorated {
            font-size: 12px;
            font-family: "Segoe UI",Arial,Helvetica,sans-serif;
        }

        .rbDecorated {
            font-size: 12px;
            font-family: "Segoe UI",Arial,Helvetica,sans-serif;
        }

        .rbDecorated {
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

        .rbDecorated {
            font-size: 12px;
            font-family: "Segoe UI",Arial,Helvetica,sans-serif;
        }

        .rbDecorated {
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

        .rbDecorated {
            font-size: 12px;
            font-family: "Segoe UI",Arial,Helvetica,sans-serif;
        }

        .rbDecorated {
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

        .rbDecorated {
            font-size: 12px;
            font-family: "Segoe UI",Arial,Helvetica,sans-serif;
        }

        .rbDecorated {
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

        .rbDecorated {
            font-size: 12px;
            font-family: "Segoe UI",Arial,Helvetica,sans-serif;
        }

        .rbDecorated {
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

        .rbDecorated {
            font-size: 12px;
            font-family: "Segoe UI",Arial,Helvetica,sans-serif;
        }

        .rbDecorated {
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

        .style16 {
            height: 32px;
            width: 209px;
        }

        .style17 {
            height: 32px;
            width: 957px;
        }

        .style20 {
            height: 32px;
            width: 122px;
        }

        .style21 {
            height: 32px;
            width: 253px;
        }

        .style22 {
            height: 32px;
            width: 674px;
        }

        .style23 {
            height: 32px;
            width: 168px;
        }

        .style25 {
            width: 100%;
            height: 102px;
        }

        .style26 {
            height: 32px;
            width: 69px;
        }

        .style27 {
            height: 32px;
            width: 748px;
        }

        .style28 {
            height: 32px;
            width: 890px;
        }

        .style29 {
            height: 32px;
        }

        .style30 {
            width: 476px;
        }

        body {
            zoom: 1.0 !important;
            -moz-transform: scale(1) !important;
            -moz-transform-origin: 0 0 !important;
        }
    </style>
    <script type="text/javascript">
        function LoadFlowSheetManager() {
            $("span[mand=Yes]").addClass('MandLabelstyle');

            $("span[mand=Yes]").each(function () {
                $(this).html($(this).html().replace("*", "<span class='manredforstar'>*</span>"));
            });
        }
        </script>
</head>
<body onload="LoadFlowSheetManager();" oncontextmenu="return false">
    <form id="frmFlowSheetManager" runat="server">
        <telerik:RadWindowManager ID="ModalWindowMngt" runat="server">
            <Windows>
                <telerik:RadWindow ID="ModalWindow" runat="server" VisibleOnPageLoad="false" Height="625px"
                    IconUrl="Resources/16_16.ico" Width="1225px">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="pnlFlowsheetMngr" BackColor="White" runat="server">
            <div style="height: 558px; width: 786px">
                <asp:Panel ID="Panel1" runat="server" Width="784px">
                    <table class="style1">
                        <tr>
                            <td class="style2" colspan="10">
                                <asp:Panel ID="pnlFlowSheetDetails" Font-Bold="true" runat="server" Height="240px" GroupingText="Flow Sheet Details" CssClass="LabelStyleBold"
                                    Width="777px">
                                    <table class="style4">
                                        <tr>
                                            <td class="style7">
                                                <asp:Label ID="lblFlowSheetTemplateName" runat="server" Text="Flow Sheet Template Name *"
                                                    Width="100%" cssclass="spanstyle" mand="Yes"></asp:Label>
                                            </td>
                                            <td class="style9">
                                                <telerik:RadTextBox ID="txtTemplate" runat="server" Height="20px" Width="250px" cssclass="Editabletxtbox" onkeypress="EnableSave();"
                                                    AutoPostBack="false">
                                                </telerik:RadTextBox>
                                            </td>
                                            <td class="style11"></td>
                                            <td class="style11"></td>
                                            <td class="style11"></td>
                                            <td class="style6"></td>
                                        </tr>
                                        <tr>
                                            <td class="style8">
                                                <asp:Panel ID="pnlVitalItems" runat="server" GroupingText="Vital Items" Width="274px"
                                                    Height="182px" CssClass="LabelStyleBold">
                                                    <table class="style4">
                                                        <tr>
                                                            <td>
                                                                <telerik:RadListBox ID="chklstVitalsDataItems" Font-Bold="false" runat="server" CheckBoxes="True" Height="147px"
                                                                    SelectionMode="Multiple" Width="100%" OnClientSelectedIndexChanging="OnClientItemSelectedIndexChangingFlowSheet"
                                                                    OnClientItemChecked="EnableSave">
                                                                    <ButtonSettings TransferButtons="All" />
                                                                </telerik:RadListBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                            <td class="style8" colspan="5">
                                                <asp:Panel ID="pnlFrequentlyUsedResultItems" runat="server" GroupingText="Frequently Used Result Items"
                                                    Width="482px" Height="182px"  CssClass="LabelStyleBold">
                                                    <table class="style12">
                                                        <tr>
                                                            <td class="style30" rowspan="6">
                                                                <telerik:RadListBox ID="chklstResultItems" Font-Bold="false" runat="server" CheckBoxes="True" Height="147px"
                                                                    SelectionMode="Multiple" Width="100%" OnClientSelectedIndexChanging="OnClientItemSelectedIndexChangingFlowSheet"
                                                                    OnClientItemChecked="EnableSave">
                                                                    <ButtonSettings TransferButtons="All" />
                                                                </telerik:RadListBox>
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="pbSearchAllResults" runat="server" Height="23px" ImageUrl="~/Resources/Database Inactive.jpg"
                                                                    Width="18px" OnClientClick="return OpenSearchAllResults();" ToolTip="Search Library" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td class="style17"></td>
                            <td class="style22"></td>
                            <td class="style16"></td>
                            <td class="style23"></td>
                            <td class="style28"></td>
                            <td class="style20"></td>
                            <td class="style27"></td>
                            <td align="right">
                                <asp:Button ID="btnAddandGenerate" runat="server" Style="top: -2px; left: -1px; width: 147px"
                                    Text="Add and Generate" Width="100%" CssClass="aspresizedbluebutton"
                                    OnClick="btnAddandGenerate_Click" OnClientClick="return ValidationMngr();" />
                            </td>
                            <td>
                                <asp:Button ID="btnAdd" runat="server" Style="top: -2px; left: 2px; width: 100px" CssClass="aspresizedgreenbutton"
                                    Text="Add" Width="100%" OnClick="btnAdd_Click" OnClientClick="return ValidationMngr();" />
                            </td>
                            <td>
                                <asp:Button ID="btnClearAll" runat="server" OnClick="btnClearAll_Click" Style="top: -1px; left: 0px; width: 100px" CssClass="aspresizedredbutton"
                                    Text="Clear All" Width="100%" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="10">
                                <asp:Panel ID="pnlFlowSheet" runat="server" GroupingText="Flow Sheet(s)" CssClass="LabelStyleBold">
                                    <table class="style25">
                                        <tr>
                                            <td>
                                                <telerik:RadGrid ID="grdFlowSheet" CssClass="Gridbodystyle" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                    CellSpacing="0" GridLines="None" Height="200px" OnItemCommand="grdFlowSheet_ItemCommand" OnItemCreated="grdFlowSheet_ItemCreated">
                                                    <FilterMenu EnableImageSprites="False">
                                                    </FilterMenu>
                                                    <ClientSettings>
                                                        <Selecting AllowRowSelect="True" />
                                                        <Scrolling AllowScroll="True" UseStaticHeaders="True" />
                                                        <ClientEvents OnCommand="OnItemCommand" />
                                                    </ClientSettings>
                                                    <HeaderStyle cssclass="Gridheaderstyle" HorizontalAlign="Center" />
                                                    <MasterTableView>
                                                        <CommandItemSettings ExportToPdfText="Export to PDF" />
                                                        <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                                                        </RowIndicatorColumn>
                                                        <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                                                        </ExpandCollapseColumn>
                                                        <Columns>
                                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="EditDel"
                                                                DataTextField="Edit" FilterControlAltText="Filter Edit column" HeaderText="Edit"
                                                                ImageUrl="~/Resources/edit.gif" Text="Edit" UniqueName="Edit">
                                                                <HeaderStyle Width="35px" />
                                                                <ItemStyle Width="35px" />
                                                            </telerik:GridButtonColumn>
                                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Copy"
                                                                DataTextField="Copy" FilterControlAltText="Filter Copy column" HeaderText="Copy"
                                                                ImageUrl="~/Resources/Copy2.png" Text="Copy" UniqueName="Copy">
                                                                <HeaderStyle Width="40px" />
                                                                <ItemStyle Width="40px" />
                                                            </telerik:GridButtonColumn>
                                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Del"
                                                                DataTextField="Del" FilterControlAltText="Filter Del column" HeaderText="Del"
                                                                ImageUrl="~/Resources/close_small_pressed.png" Text="Del" UniqueName="Del">
                                                                <HeaderStyle Width="35px" />
                                                                <ItemStyle Width="35px" />
                                                            </telerik:GridButtonColumn>
                                                            <telerik:GridBoundColumn DataField="Flow SheetTemplateName" FilterControlAltText="Filter FlowSheetTemplateName column"
                                                                HeaderText="Flow SheetTemplateName" UniqueName="FlowSheetTemplateName">
                                                                <ItemStyle Font-Bold="false"/>
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="Data Items" FilterControlAltText="Filter DataItems column"
                                                                HeaderText="Data Items" UniqueName="DataItems">
                                                                <ItemStyle Font-Bold="false"/>
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="FlowID" FilterControlAltText="Filter FlowID column"
                                                                HeaderText="FlowID" UniqueName="FlowID" Display="false">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="Version" FilterControlAltText="Filter Version column"
                                                                HeaderText="Version" UniqueName="Version" Visible="False">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="Unit" FilterControlAltText="Filter Unit column"
                                                                HeaderText="Unit" UniqueName="Unit" Visible="False">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="Phy ID" FilterControlAltText="Filter PhyID column"
                                                                HeaderText="Phy ID" UniqueName="PhyID" Visible="False">
                                                                <HeaderStyle Width="15px" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="Result Code" FilterControlAltText="Filter ResultCode column"
                                                                HeaderText="Result Code" UniqueName="ResultCode" Display="false">
                                                                <HeaderStyle Width="15px" />
                                                            </telerik:GridBoundColumn>
                                                        </Columns>
                                                        <EditFormSettings>
                                                            <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                                            </EditColumn>
                                                        </EditFormSettings>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:HiddenField ID="hdnMessageType" runat="server" />
                <asp:HiddenField ID="hdnSearchResults" runat="server" />
                <asp:HiddenField ID="hdnAddResults" runat="server" />
                <asp:HiddenField ID="DelID" runat="server" />
                <asp:HiddenField ID="hdnSelectedIndex" runat="server" />
                <asp:Button ID="InvisibleButton" runat="server" CssClass="displaynonestyle" OnClick="InvisibleButton_Click" />
                <asp:Button ID="InvisibleButtonClear" runat="server" CssClass="displaynonestyle" OnClick="InvisibleButtonClear_Click" />
                <asp:Button ID="SearchButton" runat="server" CssClass="displaynonestyle" OnClick="SearchButton_Click" />
                <asp:Button ID="btnMessageType" runat="server" Text="Button" Style="display: none" OnClientClick="return FlowSheetManagerClearAll();" />
            </div>
        </telerik:RadAjaxPanel>
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <script src="JScripts/jquery-1.11.3.min.js" type="text/javascript"></script>
            <script src="JScripts/bootstrap.min.js" type="text/javascript"></script>
            <script src="JScripts/JSChart.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>


      <%--      <link href="CSS/ElementStyles.css" rel="stylesheet" type="text/css" />--%>

            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
        </asp:PlaceHolder>
        <script type="text/javascript">
            Telerik.Web.UI.RadListBox.prototype.saveClientState = function () {
                return "{" +
                            "\"isEnabled\":" + this._enabled +
                            ",\"logEntries\":" + this._logEntriesJson +
                           ",\"selectedIndices\":" + this._selectedIndicesJson +
                           ",\"checkedIndices\":" + this._checkedIndicesJson +
                           ",\"scrollPosition\":" + Math.round(this._scrollPosition) +
                       "}";
            }
        </script>
    </form>
</body>
</html>
