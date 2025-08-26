<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmOrderQuestionSetAOE.aspx.cs"
    Inherits="Acurus.Capella.UI.frmOrderQuestionSetAOE" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>OrderQuestionSetAOE</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
        .style1
        {
            width: 82%;
            height: 103px;
        }
        .style2
        {
            height: 64px;
        }
        .style3
        {
            width: 100%;
        }
        .RadButton_Default.rbSkinnedButton
        { +background-image:url( 'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png');}
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
        .style4
        {
            height: 30px;
        }
        #frmOrderQuestionSetAOE
        {
            width: 527px;
            height: 176px;
        }
        .style6
        {
            height: 30px;
            width: 212px;
        }
        .DisplayNone
        {
            display: none;
        }
    </style>
</head>
<body style="width: 521px">
    <form id="frmOrderQuestionSetAOE" runat="server">
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="151px" Width="522px">
        <div>
            <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
            </telerik:RadScriptManager>
            <table class="style1">
                <tr>
                    <td class="style2">
                        <asp:Panel ID="Panel1" runat="server">
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="Panel2" runat="server" Height="35px" Width="515px">
                            <table class="style3">
                                <tr>
                                    <td class="style6">
                                    </td>
                                    <td class="style4">
                                        <telerik:RadButton ID="btnOK" runat="server" OnClick="btnOK_Click" Style="top: -1px;
                                            left: 0px; height: 20px; width: 100px" Text="OK" Width="100%">
                                        </telerik:RadButton>
                                    </td>
                                    <td class="style4">
                                        <telerik:RadButton ID="btnClearAll" runat="server" OnClientClicked="ClearAllAOE" OnClick="btnClearAll_Click"
                                            Style="top: -1px; left: 0px; width: 100px" Text="Clear All" Width="100%">
                                        </telerik:RadButton>
                                    </td>
                                    <td class="style4">
                                        <telerik:RadButton ID="btnCancel" runat="server" OnClientClicked="CloseQuestionSetAOE"
                                            Style="top: -1px; left: 0px; width: 100px; height: 20px;" Text="Cancel" Width="100%">
                                        </telerik:RadButton>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </div>
        <asp:HiddenField ID="hdnLocalTime" runat="server" />
        <asp:Button ID="btnClearAllAOE" runat="server" onclick="btnClearAllAOE_Click" CssClass="DisplayNone" />
<asp:PlaceHolder ID="PlaceHolder1" runat="server">
        <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

        <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

        <script src="JScripts/JSOrdersQuestionSetsAFP.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

        <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
   
   </asp:PlaceHolder>
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
