<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Account_init.aspx.cs" Inherits="TheSoulCheatServer.Account_init" %>

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
        Daily Check Initialization <input type="checkbox" name="attend" value="attend" /><br />
        MailBox Initialization <input type="checkbox" name="mailbox" value="mailbox" /><br />
        Achievements Initialization <input type="checkbox" name="achievement" value="achievement" /><br />
        무신의 선물 치트 <input type="checkbox" name="missionWorld" value="missionWorld" /><br />
        Shop Buy Time Initialization <input type="checkbox" name="dungeonShop" value="dungeonShop" /><br />
        <input type="Submit" value=" Submit "/>
    </div>
    </form>
</body>
</html>
