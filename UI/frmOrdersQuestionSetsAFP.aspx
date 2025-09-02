<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmOrdersQuestionSetsAFP.aspx.cs"
    Inherits="Acurus.Capella.UI.frmOrdersQuestionSetsAFP" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Orders Question Sets AFP</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
        .style1
        {
            width: 99%;
            height: 595px;
        }
        .style2
        {
            height: 94px;
        }
        .style3
        {
            width: 100%;
            height: 61px;
        }
        .style4
        {
            width: 176px;
        }
        .RadPicker
        {
            vertical-align: middle;
        }
        .RadPicker .rcTable
        {
            table-layout: auto;
        }
        .RadPicker .RadInput
        {
            vertical-align: baseline;
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
        .riSingle .riTextBox
        {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
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
        .style7
        {
            width: 196px;
        }
        .style8
        {
            width: 103px;
        }
        .style9
        {
            width: 136px;
        }
        .style10
        {
            height: 183px;
        }
        .style11
        {
            width: 100%;
            height: 280px;
        }
        .style14
        {
            height: 30px;
        }
        .style15
        {
            height: 48px;
        }
        .style17
        {
            height: 66px;
        }
        .style18
        {
            height: 30px;
            width: 205px;
        }
        .style19
        {
            height: 48px;
            width: 205px;
        }
        .style20
        {
            height: 66px;
            width: 205px;
        }
        .style23
        {
            height: 30px;
        }
        .style24
        {
            height: 48px;
        }
        .style25
        {
            height: 66px;
            width: 64px;
        }
        .style26
        {
            width: 64px;
        }
        .style27
        {
            height: 30px;
        }
        .style28
        {
            height: 48px;
        }
        .style29
        {
            height: 66px;
            width: 55px;
        }
        .style30
        {
            width: 55px;
        }
        .style31
        {
            height: 30px;
            width: 131px;
        }
        .style32
        {
            height: 48px;
            width: 131px;
        }
        .style33
        {
            height: 66px;
            width: 131px;
        }
        .style34
        {
            width: 131px;
        }
        .style39
        {
            height: 30px;
            width: 59px;
        }
        .style41
        {
            height: 66px;
            width: 59px;
        }
        .style42
        {
            width: 59px;
        }
        .style43
        {
            width: 205px;
        }
        .style44
        {
            height: 42px;
        }
        .style45
        {
            width: 100%;
        }
        .style46
        {
            width: 198px;
        }
        .style47
        {
            width: 66px;
        }
        .style48
        {
            width: 60px;
        }
        .style50
        {
            width: 58px;
        }
        .style51
        {
            width: 114px;
        }
        .style52
        {
            height: 99px;
        }
        .style56
        {
            height: 28px;
        }
        .style57
        {
            height: 28px;
            width: 194px;
        }
        .style59
        {
            height: 28px;
        }
        .style61
        {
        }
        .style62
        {
            width: 8px;
        }
        .style63
        {
            height: 28px;
            width: 8px;
        }
        .style64
        {
            height: 28px;
            width: 4px;
        }
        .style65
        {
            height: 28px;
        }
        .style66
        {
            height: 28px;
            width: 58px;
        }
        .style68
        {
            height: 28px;
            width: 68px;
        }
        .style69
        {
            height: 28px;
            width: 60px;
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
        .style72
        {
            width: 199px;
            height: 35px;
        }
        .style73
        {
            width: 124px;
            height: 35px;
        }
        .style74
        {
            width: 146px;
            height: 35px;
        }
        .style75
        {
            width: 631px;
            height: 35px;
        }
        .style76
        {
            width: 538px;
            height: 35px;
        }
        .style77
        {
            width: 45px;
            height: 35px;
        }
        .style79
        {
            width: 97px;
            height: 35px;
        }
        .style80
        {
            width: 885px;
            height: 35px;
        }
        .style81
        {
            width: 103px;
            height: 35px;
        }
        .style82
        {
            height: 35px;
        }
        .style83
        {
            width: 191px;
        }
        .style84
        {
            width: 61px;
        }
    </style>
</head>
<body style="background-color: #BFDBFF">
    <form id="frmOrdersQuestionSetsAFP" runat="server">
    <telerik:RadAjaxPanel ID="AjxQuestionSetAFP" runat="server" >
    <telerik:RadWindowManager ID="WindowMngr" runat="server" VisibleStatusbar="false"
            EnableViewState="false">
            <Windows>
                <telerik:RadWindow ID="MessageWindow" runat="server" Behaviors="Close" Title="Add/Update Keywords">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
    <div style="height: 670px; width: 1040px;">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <asp:Panel ID="pnlMaternalSerumAFP" runat="server" GroupingText="Maternal Serum Screening &amp; Aminoitic Fluid Protein"
            Height="662px" Width="1030px" Font-Size="Small">
            <asp:Panel ID="pnlAfp" runat="server" BackColor="White">
                <table class="style1">
                    <tr>
                        <td class="style2" colspan="10">
                            <asp:Panel ID="pnlGACalculation" runat="server" Height="84px" GroupingText="Gestational Age Calculation">
                                <table class="style3">
                                    <tr>
                                        <td class="style4">
                                            <asp:Label ID="lblGACalculationMethod" runat="server" Text="GA Calculation Method"></asp:Label>
                                        </td>
                                        <td>
                                            <telerik:RadComboBox ID="cboGACalculationMethod" runat="server" AutoPostBack="True"
                                                OnSelectedIndexChanged="cboGACalculationMethod_SelectedIndexChanged">
                                            </telerik:RadComboBox>
                                        </td>
                                        <td class="style7" colspan="2">
                                            <asp:Label ID="lblGADateofCalculation" runat="server" Text="GA Date of Calculation"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <telerik:RadDateTimePicker ID="dtpGADateofCalculation" runat="server" Culture="English (United States)"
                                                Width="100%" OnSelectedDateChanged="dtpGADateofCalculation_SelectedDateChanged">
                                                <Calendar UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False" ViewSelectorText="x">
                                                </Calendar>
                                                <TimeView CellSpacing="-1">
                                                </TimeView>
                                                <TimePopupButton HoverImageUrl="" ImageUrl="" Visible="false" />
                                                <DatePopupButton HoverImageUrl="" ImageUrl="" />
                                                <DateInput DateFormat="dd-MMM-yyyy" DisplayDateFormat="dd-MMM-yyyy" DisplayText=""
                                                    LabelWidth="40%" type="text" value="">
                                                </DateInput></telerik:RadDateTimePicker>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style4">
                                            <asp:Label ID="lblGADate" runat="server" Text="GA Date"></asp:Label>
                                        </td>
                                        <td>
                                            <telerik:RadDateTimePicker ID="dtpGADate" runat="server" AutoPostBack="True" AutoPostBackControl="Both"
                                                Culture="English (United States)" OnSelectedDateChanged="dtpGADate_SelectedDateChanged">
                                                <Calendar UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False" ViewSelectorText="x">
                                                </Calendar>
                                                <TimeView CellSpacing="-1">
                                                </TimeView>
                                                <TimePopupButton HoverImageUrl="" ImageUrl="" Visible="false" />
                                                <DatePopupButton HoverImageUrl="" ImageUrl="" />
                                                <DateInput DateFormat="dd-MMM-yyyy" DisplayDateFormat="dd-MMM-yyyy" DisplayText=""
                                                    LabelWidth="40%" type="text" value="" AutoPostBack="True">
                                                </DateInput></telerik:RadDateTimePicker>
                                        </td>
                                        <td class="style8">
                                            <asp:Label ID="lblGADays" runat="server" Text="GA Days"></asp:Label>
                                        </td>
                                        <td class="style9">
                                            <telerik:RadTextBox ID="txtGADays" runat="server" oncopy="return false" onpaste="return false"
                                                AutoPostBack="true" oncut="return false" LabelWidth="55px" Width="100px" OnTextChanged="txtGADays_TextChanged"
                                                MaxLength="1" onkeypress="return IsNumeric(event);">
                                            </telerik:RadTextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblGAWeeks" runat="server" Text="GA Weeks" Width="100px"></asp:Label>
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtGAWeeks" runat="server" LabelWidth="55px" Width="100px"
                                                AutoPostBack="true" OnTextChanged="txtGADays_TextChanged" MaxLength="2" onkeypress="return IsNumeric(event);">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td class="style10" colspan="10">
                            <asp:Panel ID="Panel1" runat="server" Height="314px" BorderWidth="1px">
                                <table class="style11">
                                    <tr>
                                        <td class="style18">
                                            <asp:Label ID="lblInsulinDependent" runat="server" Text="Insulin Dependent"></asp:Label>
                                        </td>
                                        <td class="style26">
                                            <asp:CheckBox ID="chkYesInsDependent" runat="server" Height="22px" Style="margin-left: 0px" onclick="IsCheckBox('chkNoInsDependent');"
                                                OnCheckedChanged="chkYesInsDependent_CheckedChanged" Text="Yes" Width="100%"
                                                AutoPostBack="true" ValidationGroup="InsulinDependent" />
                                        </td>
                                        <td class="style27">
                                            <asp:CheckBox ID="chkNoInsDependent" runat="server" Text="No" Width="100%" OnCheckedChanged="chkYesInsDependent_CheckedChanged"
                                                AutoPostBack="true"  ValidationGroup="InsulinDependent" onclick="IsCheckBox('chkYesInsDependent');"/>
                                        </td>
                                        <td class="style31">
                                            <asp:Label ID="lblOtherIndications" runat="server" Text="Other Indications"></asp:Label>
                                        </td>
                                        <td class="style27">
                                            <asp:CheckBox ID="chkYesOtherIndications" runat="server" Height="22px" Style="margin-left: 0px"
                                                Text="Yes" Width="100%" OnCheckedChanged="chkYesInsDependent_CheckedChanged" onclick="IsCheckBox('chkNoOtherIndications');"
                                                AutoPostBack="true" />
                                        </td>
                                        <td class="style39">
                                            <asp:CheckBox ID="chkNoOtherIndications" runat="server" Text="No" Width="100%" OnCheckedChanged="chkYesInsDependent_CheckedChanged"
                                                AutoPostBack="true" onclick="IsCheckBox('chkYesOtherIndications');"/>
                                        </td>
                                        <td class="style14">
                                            <asp:Label ID="lblFHXNTD" runat="server" Text="FHX NTD"></asp:Label>
                                        </td>
                                        <td class="style14">
                                            <asp:CheckBox ID="chkYesFHXNTD" runat="server" Height="22px" Style="margin-left: 0px"
                                                Text="Yes" Width="100%" OnCheckedChanged="chkYesInsDependent_CheckedChanged" onclick="IsCheckBox('chkNoFHXNTD');"
                                                AutoPostBack="true" />
                                        </td>
                                        <td class="style14">
                                            <asp:CheckBox ID="chkNoFHXNTD" runat="server" Text="No" Width="100%" OnCheckedChanged="chkYesInsDependent_CheckedChanged"
                                                AutoPostBack="true" onclick="IsCheckBox('chkYesFHXNTD');" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style18">
                                            <asp:Label ID="lblAdditionalInformation" runat="server" Text="Additional Information"></asp:Label>
                                        </td>
                                        <td class="style23" colspan="5">
                                            <telerik:RadTextBox ID="txtAdditionalInfo" runat="server" LabelWidth="55px" Width="96%"
                                                OnTextChanged="txtGADays_TextChanged" AutoPostBack="true">
                                            </telerik:RadTextBox>
                                        </td>
                                        <td class="style14">
                                            <asp:Label ID="lblNumberOfFetuses" runat="server" Text="Number of Fetuses"></asp:Label>
                                        </td>
                                        <td class="style14" colspan="2">
                                            <telerik:RadTextBox ID="txtNumberofFetuses" runat="server" LabelWidth="55px" Width="100px"
                                                MaxLength="1" onkeypress="return IsNumeric(event);" OnTextChanged="txtGADays_TextChanged"
                                                AutoPostBack="true">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style18">
                                            <asp:Label ID="lblDonorEgg" runat="server" Text="Donor Egg"></asp:Label>
                                        </td>
                                        <td class="style23">
                                            <asp:CheckBox ID="chkYesDonorEgg" runat="server" Height="22px" Style="margin-left: 0px"
                                                Text="Yes" Width="100%" OnCheckedChanged="chkYesInsDependent_CheckedChanged"
                                                AutoPostBack="true" onclick="IsCheckBox('chkNoDonorEgg');"/>
                                        </td>
                                        <td class="style27">
                                            <asp:CheckBox ID="chkNoDonorEgg" runat="server" Text="No" Width="100%" OnCheckedChanged="chkYesInsDependent_CheckedChanged"
                                                AutoPostBack="true" onclick="IsCheckBox('chkYesDonorEgg');"/>
                                        </td>
                                        <td class="style31">
                                            <asp:Label ID="lblAgeofEggDonor" runat="server" Text="Age of Egg Donor"></asp:Label>
                                        </td>
                                        <td class="style27" colspan="2">
                                            <telerik:RadTextBox ID="txtAgeofEggDonor" runat="server" LabelWidth="55px" Width="100px"
                                                OnTextChanged="txtGADays_TextChanged" AutoPostBack="true">
                                            </telerik:RadTextBox>
                                        </td>
                                        <td class="style14">
                                        </td>
                                        <td class="style14">
                                        </td>
                                        <td class="style14">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style19">
                                            <asp:Label ID="lblUltrasoundCRLLength" runat="server" Text="Ultrasound Measurement Crown  Rump Length(mm)"
                                                Width="200px"></asp:Label>
                                        </td>
                                        <td class="style24" colspan="2">
                                            <telerik:RadTextBox ID="txtUltrasoundCRLLength" runat="server" MaxLength="2" LabelWidth="55px"
                                                Width="100px" OnTextChanged="txtGADays_TextChanged" onkeypress="return IsNumeric(event);"
                                                AutoPostBack="true">
                                            </telerik:RadTextBox>
                                        </td>
                                        <td class="style32">
                                            <asp:Label ID="lblUltrasoundCRLLengthTwinB" runat="server" Text="Ultrasound Measurement Crown  Rump Length for Twin B(mm)"
                                                Width="200px"></asp:Label>
                                        </td>
                                        <td class="style28" colspan="2">
                                            <telerik:RadTextBox ID="txtUltrasoundCRLLengthTwinB" runat="server" MaxLength="2"
                                                LabelWidth="55px" Width="100px" OnTextChanged="txtGADays_TextChanged" onkeypress="return IsNumeric(event);"
                                                AutoPostBack="true">
                                            </telerik:RadTextBox>
                                        </td>
                                        <td class="style15">
                                            <asp:Label ID="lblUltrasoundCRLLengthDate" runat="server" Text="Ultrasound Measurement Crown  Rump Length Date"
                                                Width="200px"></asp:Label>
                                        </td>
                                        <td class="style15" colspan="2">
                                            <telerik:RadDateTimePicker ID="dtpUltrasoundCRLLengthDate" runat="server" Height="19px"
                                                Width="115px" Culture="English (United States)" OnSelectedDateChanged="dtpUltrasoundCRLLengthDate_SelectedDateChanged">
                                                <Calendar UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False" ViewSelectorText="x">
                                                </Calendar>
                                                <TimeView CellSpacing="-1">
                                                </TimeView>
                                                <TimePopupButton HoverImageUrl="" ImageUrl="" Visible="false" />
                                                <DatePopupButton HoverImageUrl="" ImageUrl="" />
                                                <DateInput DateFormat="dd-MMM-yyyy" DisplayDateFormat="dd-MMM-yyyy" DisplayText=""
                                                    Height="19px" LabelWidth="40%" type="text" value="">
                                                </DateInput></telerik:RadDateTimePicker>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style18">
                                            <asp:Label ID="lblNuchalTranslucency" runat="server" Text="Nuchal Translucency (mm)"></asp:Label>
                                        </td>
                                        <td class="style23" colspan="2">
                                            <telerik:RadTextBox ID="txtNuchalTranslucency" runat="server" LabelWidth="55px" MaxLength="2"
                                                Width="100px" OnTextChanged="txtGADays_TextChanged" onkeypress="return IsNumeric(event);"
                                                AutoPostBack="true">
                                            </telerik:RadTextBox>
                                        </td>
                                        <td class="style31">
                                            <asp:Label ID="lblNuchalTranslucencyTwinB" runat="server" Text="Nuchal Translucency for Twin B (mm)"
                                                Width="200px"></asp:Label>
                                        </td>
                                        <td class="style27" colspan="2">
                                            <telerik:RadTextBox ID="txtNuchalTranslucencyTwinB" runat="server" MaxLength="2"
                                                LabelWidth="55px" Width="100px" OnTextChanged="txtGADays_TextChanged" onkeypress="return IsNumeric(event);"
                                                AutoPostBack="true">
                                            </telerik:RadTextBox>
                                        </td>
                                        <td class="style14">
                                        </td>
                                        <td class="style14">
                                        </td>
                                        <td class="style14">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style20">
                                            <asp:Label ID="lblPriorDownSyndromeCurrentPregnancy" runat="server" Text="Prior Down Syndrome / ONTD Screening during current pregnancy"
                                                Width="200px"></asp:Label>
                                        </td>
                                        <td class="style25">
                                            <asp:CheckBox ID="chkYesPriorDownSyndromeCurrentPregnancy" runat="server" Height="22px"
                                                Style="margin-left: 0px" Text="Yes" Width="100%" OnCheckedChanged="chkYesInsDependent_CheckedChanged"
                                                AutoPostBack="true" onclick="IsCheckBox('chkNoPriorDownSyndromeCurrentPregnancy');"/>
                                        </td>
                                        <td class="style29">
                                            <asp:CheckBox ID="chkNoPriorDownSyndromeCurrentPregnancy" runat="server" Text="No"
                                                Width="100%" OnCheckedChanged="chkYesInsDependent_CheckedChanged" AutoPostBack="true"  onclick="IsCheckBox('chkYesPriorDownSyndromeCurrentPregnancy');"/>
                                        </td>
                                        <td class="style33">
                                            <asp:Label ID="lblPriorFirstTrimesterTesting" runat="server" Text="Prior First Trimester Testing"
                                                Width="200px"></asp:Label>
                                        </td>
                                        <td class="style29">
                                            <asp:CheckBox ID="chkYesPriorFirstTrimesterTesting" runat="server" Height="22px"
                                                Style="margin-left: 0px" Text="Yes" Width="100%" OnCheckedChanged="chkYesInsDependent_CheckedChanged"
                                                AutoPostBack="true" onclick="IsCheckBox('chkNoPriorFirstTrimesterTesting');"/>
                                        </td>
                                        <td class="style41">
                                            <asp:CheckBox ID="chkNoPriorFirstTrimesterTesting" runat="server" Text="No" Width="100%"
                                                OnCheckedChanged="chkYesInsDependent_CheckedChanged" AutoPostBack="true" onclick="IsCheckBox('chkYesPriorFirstTrimesterTesting');"/>
                                        </td>
                                        <td class="style17">
                                            <asp:Label ID="lblPriorSecondTrimesterTesting" runat="server" Text="Prior Second Trimester Testing"
                                                Width="200px"></asp:Label>
                                        </td>
                                        <td class="style17">
                                            <asp:CheckBox ID="chkYesPriorSecondTrimesterTesting" runat="server" Height="22px"
                                                Style="margin-left: 0px" Text="Yes" Width="100%" OnCheckedChanged="chkYesInsDependent_CheckedChanged"
                                                AutoPostBack="true" onclick="IsCheckBox('chkNoPriorSecondTrimesterTesting');"/>
                                        </td>
                                        <td class="style17">
                                            <asp:CheckBox ID="chkNoPriorSecondTrimesterTesting" runat="server" Text="No" Width="100%"
                                                OnCheckedChanged="chkYesInsDependent_CheckedChanged" AutoPostBack="true" onclick="IsCheckBox('chkYesPriorSecondTrimesterTesting');"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style43">
                                            <asp:Label ID="lblPreviouslyElevatedAFP" runat="server" Text="Previously Elevated AFP"></asp:Label>
                                        </td>
                                        <td class="style26">
                                            <asp:CheckBox ID="chkYesPreviouslyElevatedAFP" runat="server" Height="22px" Style="margin-left: 0px"
                                                Text="Yes" Width="100%" OnCheckedChanged="chkYesInsDependent_CheckedChanged"
                                                AutoPostBack="true" onclick="IsCheckBox('chkNoPreviouslyElevatedAFP');"/>
                                        </td>
                                        <td class="style30">
                                            <asp:CheckBox ID="chkNoPreviouslyElevatedAFP" runat="server" Text="No" Width="100%"
                                                OnCheckedChanged="chkYesInsDependent_CheckedChanged" AutoPostBack="true" onclick="IsCheckBox('chkYesPreviouslyElevatedAFP');"/>
                                        </td>
                                        <td class="style34">
                                            <asp:Label ID="lblPriorPregnancyDownSyndrome" runat="server" Text="Prior Pregnancy with Down Syndrome"
                                                Width="200px"></asp:Label>
                                        </td>
                                        <td class="style30">
                                            <asp:CheckBox ID="chkYesPriorPregnancyDownSyndrome" runat="server" Height="22px"
                                                Style="margin-left: 0px" Text="Yes" Width="100%" OnCheckedChanged="chkYesInsDependent_CheckedChanged"
                                                AutoPostBack="true" onclick="IsCheckBox('chkNoPriorPregnancyDownSyndrome');"/>
                                        </td>
                                        <td class="style42">
                                            <asp:CheckBox ID="chkNoPriorPregnancyDownSyndrome" runat="server" Text="No" Width="100%"
                                                OnCheckedChanged="chkYesInsDependent_CheckedChanged" AutoPostBack="true" onclick="IsCheckBox('chkYesPriorPregnancyDownSyndrome');"/>
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
                        <td class="style44" colspan="10">
                            <asp:Panel ID="pnlChorionicity" runat="server"  GroupingText="Chorionicity  (twins only)">
                                <table class="style45">
                                    <tr>
                                        <td class="style46">
                                            <asp:Label ID="lblMonochorionic" runat="server" Text="Monochorionic" Width="180px"></asp:Label>
                                        </td>
                                        <td class="style47">
                                            <asp:CheckBox ID="chkYesMonochorionic" runat="server" Height="22px" Style="margin-left: 0px"
                                                Text="Yes" Width="100%" OnCheckedChanged="chkYesInsDependent_CheckedChanged"
                                                AutoPostBack="true" onclick="IsCheckBox('chkNoMonochorionic');"/>
                                        </td>
                                        <td class="style48">
                                            <asp:CheckBox ID="chkNoMonochorionic" runat="server" Text="No" Width="100%" OnCheckedChanged="chkYesInsDependent_CheckedChanged"
                                                AutoPostBack="true" onclick="IsCheckBox('chkYesMonochorionic');"/>
                                        </td>
                                        <td class="style83">
                                            <asp:Label ID="lblDichorionic" runat="server" Text="Dichorionic" Width="180px"></asp:Label>
                                        </td>
                                        <td class="style84">
                                            <asp:CheckBox ID="chkYesDichorionic" runat="server" Height="22px" Style="margin-left: 0px"
                                                Text="Yes" Width="100%" OnCheckedChanged="chkYesInsDependent_CheckedChanged"
                                                AutoPostBack="true" onclick="IsCheckBox('chkNoDichorionic');"/>
                                        </td>
                                        <td class="style50">
                                            <asp:CheckBox ID="chkNoDichorionic" runat="server" Text="No" Width="100%" OnCheckedChanged="chkYesInsDependent_CheckedChanged"
                                                AutoPostBack="true" onclick="IsCheckBox('chkYesDichorionic');"/>
                                        </td>
                                        <td class="style51">
                                            <asp:Label ID="lblUnknown" runat="server" Text="Unknown" Width="180px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkYesUnknown" runat="server" Height="22px" Style="margin-left: 0px"
                                                Text="Yes" Width="100%" OnCheckedChanged="chkYesInsDependent_CheckedChanged"
                                                AutoPostBack="true" onclick="IsCheckBox('chkNoUnknown');"/>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkNoUnknown" runat="server" Text="No" Width="100%" OnCheckedChanged="chkYesInsDependent_CheckedChanged"
                                                AutoPostBack="true" onclick="IsCheckBox('chkYesUnknown');"/>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td class="style52" colspan="10">
                            <asp:Panel ID="pnlSonographerDetails" runat="server" Height="126px" GroupingText="Sonographer Details">
                                <table class="style45">
                                    <tr>
                                        <td class="style57">
                                            <asp:Label ID="lblSonographerLastName" runat="server" Text="Sonographer Last Name"
                                                Width="200px"></asp:Label>
                                        </td>
                                        <td class="style61" colspan="2">
                                            <telerik:RadTextBox ID="txtSonographerLastName" runat="server" AutoPostBack="true"
                                                LabelWidth="55px" Width="100px" Height="16px" OnTextChanged="txtGADays_TextChanged">
                                            </telerik:RadTextBox>
                                        </td>
                                        <td class="style62">
                                            <asp:Label ID="lblSonographerFirstName" runat="server" Text="Sonographer First Name"
                                                Width="200px"></asp:Label>
                                        </td>
                                        <td class="style65" colspan="2">
                                            <telerik:RadTextBox ID="txtSonographerFirstName" runat="server" Height="16px" LabelWidth="55px"
                                                Width="100px" OnTextChanged="txtGADays_TextChanged" AutoPostBack="true">
                                            </telerik:RadTextBox>
                                        </td>
                                        <td class="style64">
                                            <asp:Label ID="lblSonographerIDNumber" runat="server" Text="Sonographer ID Number"
                                                Width="200px"></asp:Label>
                                        </td>
                                        <td class="style56" colspan="2">
                                            <telerik:RadTextBox ID="txtSonographerIDNumber" runat="server" LabelWidth="55px"
                                                Width="100px" OnTextChanged="txtGADays_TextChanged" AutoPostBack="true">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style57">
                                            <asp:Label ID="lblCredentialedbyNTQR" runat="server" Text="Credentialed by-NTQR"
                                                Width="180px"></asp:Label>
                                        </td>
                                        <td class="style68">
                                            <asp:CheckBox ID="chkYesCredentialedbyNTQR" runat="server" Height="22px" Style="margin-left: 0px"
                                                Text="Yes" Width="100%" OnCheckedChanged="chkYesInsDependent_CheckedChanged"
                                                AutoPostBack="true" onclick="IsCheckBox('chkNoCredentialedbyNTQR');"/>
                                        </td>
                                        <td class="style69">
                                            <asp:CheckBox ID="chkNoCredentialedbyNTQR" runat="server" Text="No" Width="100%"
                                                OnCheckedChanged="chkYesInsDependent_CheckedChanged" AutoPostBack="true" onclick="IsCheckBox('chkYesCredentialedbyNTQR');"/>
                                        </td>
                                        <td class="style62">
                                            <asp:Label ID="lblReadingPhysicianID" runat="server" Text="Credentialed by-FMF" Width="130px"></asp:Label>
                                        </td>
                                        <td class="style56">
                                            <asp:CheckBox ID="chkYesCredentialedbyFMF" runat="server" Height="22px" Style="margin-left: 0px"
                                                Text="Yes" Width="50px" OnCheckedChanged="chkYesInsDependent_CheckedChanged"
                                                AutoPostBack="true" onclick="IsCheckBox('chkNoCredentialedbyFMF');"/>
                                        </td>
                                        <td class="style66">
                                            <asp:CheckBox ID="chkNoCredentialedbyFMF" runat="server" Text="No" Width="50px" OnCheckedChanged="chkYesInsDependent_CheckedChanged"
                                                AutoPostBack="true" onclick="IsCheckBox('chkYesCredentialedbyFMF');" />
                                        </td>
                                        <td class="style64">
                                            <asp:Label ID="lblCredentialedbyOther" runat="server" Text="Credentialed by-Other Organization"
                                                Width="200px"></asp:Label>
                                        </td>
                                        <td class="style56">
                                            <asp:CheckBox ID="chkYesCredentialedbyOther" runat="server" Height="22px" Style="margin-left: 0px"
                                                Text="Yes" Width="100%" OnCheckedChanged="chkYesInsDependent_CheckedChanged"
                                                AutoPostBack="true" onclick="IsCheckBox('chkNoCredentialedbyOther');" />
                                        </td>
                                        <td class="style56">
                                            <asp:CheckBox ID="chkNoCredentialedbyOther" runat="server" Text="No" Width="100%"
                                                OnCheckedChanged="chkYesInsDependent_CheckedChanged" AutoPostBack="true" onclick="IsCheckBox('chkYesCredentialedbyOther');" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style57">
                                            <asp:Label ID="lblSiteNumber" runat="server" Text="Site Number" Width="200px"></asp:Label>
                                        </td>
                                        <td class="style59" colspan="2">
                                            <telerik:RadTextBox ID="txtSiteNumber" runat="server" Height="16px" LabelWidth="55px"
                                                Width="100px" OnTextChanged="txtGADays_TextChanged" AutoPostBack="true">
                                            </telerik:RadTextBox>
                                        </td>
                                        <td class="style63">
                                            <asp:Label ID="lblSonographerLastName2" runat="server" Text="Reading Physician ID"
                                                Width="200px"></asp:Label>
                                        </td>
                                        <td class="style65" colspan="2">
                                            <telerik:RadTextBox ID="txtReadingPhysicianID" runat="server" LabelWidth="55px" Width="100px"
                                                Height="16px" OnTextChanged="txtGADays_TextChanged" AutoPostBack="true">
                                            </telerik:RadTextBox>
                                        </td>
                                        <td class="style64">
                                        </td>
                                        <td class="style56">
                                        </td>
                                        <td class="style56">
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td class="style72">
                        </td>
                        <td class="style73">
                        </td>
                        <td class="style74">
                        </td>
                        <td class="style75">
                        </td>
                        <td class="style76">
                        </td>
                        <td class="style77">
                        </td>
                        <td class="style80">
                        </td>
                        <td class="style79">
                            <telerik:RadButton ID="btnOK" runat="server" Style="top: -1px; left: 0px; height: 20px;
                                width: 100px" Text="OK" Width="100%" OnClick="btnOK_Click" OnClientClicked="CheckChange">
                            </telerik:RadButton>
                        </td>
                        <td class="style81">
                            <telerik:RadButton ID="btnClearAll" runat="server" Style="top: -1px; left: 0px; width: 100px"
                                Text="Clear All" Width="100%" OnClientClicked="ClearAll">
                            </telerik:RadButton>
                        </td>
                        <td class="style82">
                            <telerik:RadButton ID="btnCancel" runat="server" Style="top: -1px; left: 0px; height: 20px;
                                width: 100px" Text="Cancel" Width="100%" OnClientClicked="CloseQuestionSet">
                            </telerik:RadButton>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </asp:Panel>
    </div>
    <asp:HiddenField ID="hdnLocalTime" runat="server" />
    <asp:HiddenField ID="hdnMessageType" runat="server" />
     <asp:Button ID="btnMessageType" runat="server" Text="Button" Style="display: none" OnClientClick="CloseQuestionSet();"/>
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
