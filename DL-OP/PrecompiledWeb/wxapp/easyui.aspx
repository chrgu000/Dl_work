<%@ page language="C#" autoeventwireup="true" inherits="wxapp_easyui, dlopwebdll" enableviewstate="false" %>
    
<!DOCTYPE html>
    <html>
    <head>
        <meta charset="UTF-8">
        <meta name="viewport" content="initial-scale=1.0, maximum-scale=1.0, user-scalable=no">
        <title></title>

        <link href="../js/themes/mobile.css" rel="stylesheet" />
        <link href="../js/themes/icon.css" rel="stylesheet" />
        <link href="../js/themes/metro/easyui.css" rel="stylesheet" />
        <script src="../js/jquery.min.js"></script>
        <script src="../js/jquery.easyui.min.js"></script>
        <script src="../js/jquery.easyui.mobile.js"></script>
    </head>
    <body>
        <div class="easyui-navpanel" style="position:relative;padding:20px">
            <header>
                <div class="m-toolbar">
                    <div class="m-title">Basic Form</div>
                    <div class="m-right">
                        <a href="javascript:void(0)" class="easyui-linkbutton" onclick="$('#ff').form('reset')" style="width:60px">Reset</a>
                    </div>
                </div>
            </header>
            <form id="ff">
                <div>
                    <label>Full name</label>
                    <input class="easyui-textbox" prompt="Full name" style="width:100%">
                </div>
                <div>
                    <label>Birthday</label>
                    <input class="easyui-datebox" prompt="Birthday" data-options="editable:false,panelWidth:220,panelHeight:240,iconWidth:30" style="width:100%">
                </div>
                <div>
                    <label>Password</label>
                    <input class="easyui-textbox" type="password" prompt="Password" style="width:100%">
                </div>
                <div>
                    <label>Number</label>
                    <input class="easyui-numberbox" prompt="Number" style="width:100%">
                </div>
                <div>
                    <label>Volumn</label>
                    <input class="easyui-slider" value="10" style="width:100%">
                </div>
            </form>
        </div>
        <style scoped>
            form label{
                display: block;
                margin: 10px 0 5px 0;
            }
        </style>
    </body>
    </html>