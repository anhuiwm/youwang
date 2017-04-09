<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cheat_init.aspx.cs" Inherits="TheSoulCheatServer.cheat_init" %>

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
        Game Play Count Initialization<br />
        pvp : <input type="checkbox" name="co_op2" value="co_op" /><br />
        Game Play Reset Count Initialization<br />
        pvp : <input type="checkbox" name="co_op" value="co_op" /><br />
        Dark Passage :<input type="checkbox" name="bark" value="bark" /><br />
        Gold Expedition:<input type="checkbox" name="expedition" value="expedition" /><br />
        Mission :<input type="checkbox" name="mission" value="mission" /><br />
        Gold Expedition Mercenary Initialization<br />
        Register :<input type="checkbox" name="hero" value="hero" /><br />
        Employ :<input type="checkbox" name="hier" value="hier" /><br />
        Shop Initialization :<input type="checkbox" name="shop" value="shop" /><br />
        VIP Level : <input type="text" name="level" size="20" /> </br><br />
        Mission Cheat <br />
        World : <input type="text" name="world" size="20" /> </br>
        Stage : <input type="text" name="stage" size="20" /> </br>
        Mission Init : <input type="checkbox" name="del_mission" value="del_mission" /><br />
        <input type="Submit" value=" Submit "/>
    </div>
    </form>
</body>
</html>
