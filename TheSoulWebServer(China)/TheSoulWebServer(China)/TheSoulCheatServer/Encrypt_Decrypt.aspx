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
       encrypt key : <asp:textbox id="encryptkey" runat="server" size="50"></asp:textbox> <BR>
        encrypt Data : <asp:textbox id="encryptdata" runat="server" size="100"></asp:textbox> <BR><BR>
        decrypt key : <asp:textbox id="decryptkey" runat="server" size="50"></asp:textbox> <BR>
        decrypt Data : <asp:textbox id="decryptdata" runat="server" size="100"></asp:textbox> <BR><BR>
        <input type="Submit" value=" Submit "/>
    </div>
    </form>
</body>
</html>