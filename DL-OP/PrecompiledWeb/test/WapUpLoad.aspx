<%@ page language="C#" autoeventwireup="true" inherits="test_WapUpLoad, dlopwebdll" enableviewstate="false" %>

<?xml version="1.0"?>
<!DOCTYPE html PUBLIC "-//WAPFORUM//DTD XHTML Mobile 1.0//EN" "http://www.wapforum.org/DTD/xhtml-mobile10.dtd">
 

<html xmlns="http://www.w3.org/1999/xhtml">


    <head>
     <title>上传文件</title>
   </head>
   <body>     
    <form action="uploadfile.aspx" method="post" enctype="multipart/form-data">
       <p>
        <input name="myFile" type="file"/>
       </p>
       <p>
         <input type="submit" value="上传"/>
       </p>
     </form>
   </body>
</html>
