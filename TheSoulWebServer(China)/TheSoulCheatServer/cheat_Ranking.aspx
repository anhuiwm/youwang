<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cheat_Ranking.aspx.cs" Inherits="TheSoulCheatServer.cheat_Ranking" %>

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
        Free PvP : <input type="text" name="pvp_free" size="20" /> </br>
        1vs1 PvP : <input type="text" name="pvp_1VS1" size="20" /> </br>
        <br />
        GuildName : <input type="text" name="guildname" size="20" /> </br>
        Guild Ranking : <input type="text" name="guildranking" size="20" /> </br>
        <!--Guild War Ranking : <input type="text" name="guildwar" size="20" /> </br>-->
        
        <input type="Submit" value=" Submit "/>
    </div>
    </form>
</body>
</html>
