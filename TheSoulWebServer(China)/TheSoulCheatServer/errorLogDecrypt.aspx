<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="errorLogDecrypt.aspx.cs" Inherits="TheSoulCheatServer.errorLogDecrypt" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div><BR>
        encrypt Data : <asp:textbox id="encryptdata" runat="server" size="100"></asp:textbox><br />
        <button type="submit">submit</button>
        <br /><br />
        DecryptString :<br />
        <asp:Label ID="decrypt" runat="server"></asp:Label>
    </div>
    </form>
</body>
</html>