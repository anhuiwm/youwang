<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Encrypt_Decrypt.aspx.cs" Inherits="Encrypt_Decrypt" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div><BR>
        암호화 key : <asp:textbox id="encryptkey" runat="server" size="50"></asp:textbox> <BR>
        암호화 : <asp:textbox id="encryptdata" runat="server" size="100"></asp:textbox> <BR><BR>
        복호화 key : <asp:textbox id="decryptkey" runat="server" size="50"></asp:textbox> <BR>
        복호화 : <asp:textbox id="decryptdata" runat="server" size="100"></asp:textbox> <BR><BR>
        <input type="Submit" value=" Submit "/>
    </div>
    </form>
</body>
</html>