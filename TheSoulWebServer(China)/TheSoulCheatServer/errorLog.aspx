<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="errorLog.aspx.cs" Inherits="TheSoulCheatServer.errorLog" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<style type="text/css">
    .classTD tbody tr td {
        white-space:normal;
        word-break: break-all;
    }
</style>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        AID : <asp:textbox id="aid" runat="server"></asp:textbox> </br>
        ErrorCode : <asp:textbox id="errorcode" runat="server"></asp:textbox> </br>
        op : <asp:textbox id="op" runat="server"></asp:textbox> </br>
        <input type="Submit" value=" Submit "/><br />
        <br />
        <asp:GridView ID="loglist" CssClass="classTD" AutoGenerateColumns="false"  Width="100%" runat="server">
            <Columns>
                <asp:BoundField ControlStyle-Width="20%" DataField="RequestParams" HtmlEncode="false" HeaderText="RequestParams" />
                <asp:BoundField ControlStyle-Width="20%" DataField="ResponseResult" HtmlEncode="false" HeaderText="ResponseResult" />
                <asp:BoundField ControlStyle-Width="20%" DataField="BaseJson" HtmlEncode="false" HeaderText="BaseJson" />
                <asp:BoundField ControlStyle-Width="30%" DataField="DetailDBLog" HtmlEncode="false" HeaderText="DetailDBLog" />
                <asp:BoundField ControlStyle-Width="10%" DataField="regdate" HtmlEncode="false" HeaderText="RegDate" />
            </Columns>
        </asp:GridView>
    </div>
    </form>
</body>
</html>
