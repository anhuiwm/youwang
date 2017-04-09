<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cheat_character_levelup.aspx.cs" Inherits="TheSoulCheatServer.cheat_character_levelup" %>

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
        Class Select : <select name="CLASS"><option value="1">무사</option><option value="2">검객</option></select> </br>
        Level : <input type="text" name="level" size="20" /> </br>
        EXP : <input type="text" name="exp" size="20" /> </br>
        PassiveSoulEXP : <input type="text" name="passive_exp" size="20" /> </br>
        <input type="Submit" value=" Submit "/>
    </div>
    </form>
</body>
</html>
