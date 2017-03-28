<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cheat_Guild_Member.aspx.cs" Inherits="TheSoulCheatServer.cheat_Guild_Member" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        GuildName : <asp:textbox id="guildname" runat="server"></asp:textbox> </br>
        Guild Member Count : <asp:textbox id="gcount" runat="server"></asp:textbox> </br>
        <input type="Submit" value=" Submit "/>
    </div>
    </form>
</body>
</html>
