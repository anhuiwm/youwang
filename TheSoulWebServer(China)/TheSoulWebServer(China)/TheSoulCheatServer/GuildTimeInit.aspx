<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GuildTimeInit.aspx.cs" Inherits="TheSoulCheatServer.GuildTimeInit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        Guild Name : <input type="text" name="guildName" size="20" /> </br>
        Today Attend Count : <input type="text" name="usercnt" size="20" /><br />
        YesterDay Attend Count : <input type="text" name="usercnt2" size="20" /><br />
        스킬 발동 시간 치트 <input type="checkbox" name="skillTime" value="skillTime" /><br />
        Guild introduction, notice Time Initialization<input type="checkbox" name="guildTime" value="guildTime" /><br />
        Guild Entrust Time Initialization <input type="checkbox" name="EntrustTime" value="guildTime" /><br />
        Guild Level : <input type="text" name="level" size="20" /><br />
        <br />
        UserName : <input type="text" name="username" size="20" /><br />
        Guild ReJoin Time Initialization <input type="checkbox" name="joinDate" value="joinDate" /><br />
        Guild Donation Time Initialization <input type="checkbox" name="DonationDate" value="DonationDate" /><br />
        <br />
        <input type="Submit" value=" Submit "/>
    </div>
    </form>
</body>
</html>
