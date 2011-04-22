<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>TailSpin</title>
    <meta name="HandheldFriendly" content="true" />
    <meta name="MobileOptimized" content="300" />
    <meta name="viewport" content="width=300" />
</head>
<body>
    <div style="width:300px; height:200px;text-align:center; vertical-align:middle;">
        <img src="<%:Url.Content("~/Content/Img/tailspin-logo.png")%>" alt="Powered by TailSpin" width="200" height="80" align="top" />
        <br />
        <br />
        <span style="font-size:16px;">TailSpin has a specific version for Windows Phone!</span>
        <br />
        <br />
        <br />
        <span style="font-size:25px;">
            <a href="http://mobile.microsoft.com/windowsmobile">Get TailSpin Application!</a>
        </span>
    </div>
</body>
</html>
