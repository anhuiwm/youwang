<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="TheSoulGMTool.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>[Dark Blaze] Operational Tool </title>
    <meta charset="UTF-8" />
    <link rel="stylesheet" href="/css/anchovy.css" type="text/css">
    <script type="text/javascript">

        function TnGoLogin(frm) {
            if (frm.uid.value == "") {
                alert("input id!");
                document.loginfrm.uid.focus();
            }
            else if (frm.upw.value == "") {
                alert("input password!");
                document.loginfrm.upw.focus();
            }
            else {
                document.loginfrm.submit();
            }
        }

    </script>
</head>
<body>
    <br />
    <br />
    <br />
    <br />
    <table border="0" cellpadding="0" cellspacing="1" bgcolor="#808080" align="center" width="400" height="500">
        <tr>
            <td bgcolor="#FFFFFF">
                <table border="0" cellpadding="0" cellspacing="0" width="100%" height="100%">
                    <tr>
                        <td align="center">
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td height="50" align="center"><b>刃無雙运营管理工具</b></td>
                                </tr>
                                <tr>
                                    <td>
                                        <img src="/img/login_bg.jpg" width="380" height="214" alt=""></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <form method="post" name="loginfrm" id="loginfrm" runat="server">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td align="left"><font color="red">ID</font>: </td>
                                        <td>&nbsp;<input type="text" name="uid" class="input_kor" style="height: 20px; width: 100px;" onkeypress="if (event.keyCode == 13) loginfrm.upw.focus();"></td>
                                    </tr>
                                    <tr>
                                        <td align="left"><font color="red">PASS</font>: </td>
                                        <td>&nbsp;<input type="password" name="upw" class="input_kor" style="height: 20px; width: 100px;" onkeypress="if (event.keyCode == 13) TnGoLogin(loginfrm);"></td>
                                    </tr>
                                    <tr>
                                        <td align="left"><font color="red">Language</font>: </td>
                                        <td>&nbsp;<asp:DropDownList ID="gm_lang" runat="server"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center" height="40">
                                            <asp:Button CssClass="wbutton" OnClientClick="TnGoLogin(loginfrm)" Text="Login" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center" height="40"><a href="/member/member_register.aspx">Register</a></td>
                                    </tr>
                                </table>
                            </form>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>
