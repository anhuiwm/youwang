<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="member_register.aspx.cs" Inherits="TheSoulGMTool.member_register" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>[TheSoul] Operational Tool Register </title>
    <link href="/css/bootstrap.css" rel="stylesheet">
    <style type="text/css">
        body {
            padding-top: 60px;
            padding-bottom: 40px;
        }

        .sidebar-nav {
            padding: 9px 0;
        }

        .nav .active {
            font-weight: bold;
        }

        .row-fluid .span4 {
            min-height: 130px;
        }

        h3 {
            margin-top: 30px;
        }
    </style>
    <link rel="stylesheet" type="text/css" href="/css/jquery.datepick.css">
    <link href="/css/bootstrap-responsive.css" rel="stylesheet">
</head>
<body>
    <div class="container-fluid">
        <div class="row-fluid">
            <div class="span9">
                <div class="bs-docs-example">
                    <table class="table table-bordered">
                        <form name="frmadd" runat="server" method="post">
                            <tr>
                                <td colspan="2">Register</td>
                            </tr>
                            <tr>
                                <td width="30%">ID</td>
                                <td>
                                    <asp:TextBox id="userid" MaxLength="16" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="checkID" ErrorMessage="<%$ Resources:languageResource ,lang_MsgInputID %>" Display="Dynamic" SetFocusOnError="True" ControlToValidate ="userid" runat="server"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>Password</td>
                                <td>
                                    <asp:TextBox ID="userpass" TextMode="Password" maxlength="16" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="checkPW" ErrorMessage="<%$ Resources:languageResource, lang_MsgPass %>" Display="Dynamic" SetFocusOnError="True" ControlToValidate ="userpass" runat="server"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>Password Confirm</td>
                                <td>
                                    <asp:TextBox ID="userpass2" TextMode="Password" maxlength="16" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="checkRePW" ErrorMessage="<%$ Resources:languageResource ,lang_MsgOneMorePass %>" Display="None" SetFocusOnError="True" ControlToValidate ="userpass2" runat="server"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="PasswordConfirm" runat="server" ControlToCompare="userpass2" ControlToValidate="userpass" ErrorMessage="<%$ Resources:languageResource ,lang_MsgPassWrong %>" Display="Dynamic" SetFocusOnError="True"></asp:CompareValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>Name</td>
                                <td>
                                    <asp:TextBox ID="username" maxlength="16" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="checkName" ErrorMessage="<%$ Resources:languageResource ,lang_MsgInputName %>" Display="Dynamic" SetFocusOnError="True" ControlToValidate ="username" runat="server"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>Phone</td>
                                <td>
                                    <asp:TextBox ID="phone" maxlength="16" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="checkPhone" ErrorMessage="<%$ Resources:languageResource ,lang_MsgInputTel %>" Display="Dynamic" SetFocusOnError="True" ControlToValidate ="phone" runat="server"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>E-mail</td>
                                <td>
                                    <asp:TextBox ID="usermail" maxlength="64" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="checkEmail" ErrorMessage="<%$ Resources:languageResource ,lang_kind %>이메일을입력하세요." Display="Dynamic" SetFocusOnError="True" ControlToValidate ="usermail" runat="server"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>Department</td>
                                <td>
                                    <asp:TextBox ID="part" maxlength="32" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Position</td>
                                <td>
                                    <asp:TextBox ID="rank" maxlength="32" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Button ID="submit" Text="<%$ Resources:languageResource ,lang_btn_ok %>" runat="server" OnClick="submit_Click" />
                                </td>
                            </tr>
                        </form>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <hr>
    <footer>
        <p>&copy; wannaplay 2016</p>
    </footer>
</body>
</html>
