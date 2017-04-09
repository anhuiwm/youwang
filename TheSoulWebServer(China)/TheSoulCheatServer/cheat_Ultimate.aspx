<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cheat_Ultimate.aspx.cs" Inherits="TheSoulCheatServer.cheat_Ultimate" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        UserName : <input type="text" name="username" size="20" /> </br>
        itemID : <input type="text" name="itemid" size="20" /> </br>
        level : <input type="text" name="level" size="20" /> </br>
        step : <input type="text" name="step" size="20" /> </br>
        Weapon All lock : <input type="checkbox" name="disarm" value="disarm" /><br />
        Weapon lock : <input type="checkbox" name="lock" value="lock" /><br />
        loginCount : <input type="text" name="login" size="20" /> </br>
        <input type="Submit" value=" Submit "/>
    </div>
    </form>
</body>
</html>
    