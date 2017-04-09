<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cheat_Ranking_orverload.aspx.cs" Inherits="TheSoulCheatServer.cheat_Ranking_orverload" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        UserName1 : <input type="text" name="username1" size="20" /> </br>
        UserName2 : <input type="text" name="username2" size="20" /> </br>
        <input type="radio" name="resultType" value="0" />userName1 Win <input type="radio" name="resultType" value="1" />userName2 Win <input type="radio" name="resultType" value="-1" checked />기록 안남김<br />
        <input type="Submit" value=" Submit "/>
    </div>
    </form>
</body>
</html>
