<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cheat_Ranking2.aspx.cs" Inherits="TheSoulCheatServer.cheat_Ranking2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>

    <form id="form1" runat="server">
    <div>
        <asp:RadioButtonList ID="pvpType" RepeatDirection="Horizontal" runat="server">
            <asp:ListItem Value="free" Selected>Free PvP</asp:ListItem>
            <asp:ListItem Value="1vs1">1vs1 PvP</asp:ListItem>
            <asp:ListItem Value="guild">Guild</asp:ListItem>
        </asp:RadioButtonList>
        <asp:GridView id="DataGrid2" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="rank" HeaderText="Rank" /> 
                <asp:BoundField DataField="username" HeaderText="Name" />                 
                <asp:BoundField DataField="point" HeaderText="Rank Point" /> 
                <asp:TemplateField HeaderText="Point Edit">
                    <ItemTemplate>
                        <input type="text" name="<%# Eval("aid")  %>" size="20" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:GridView id="GridView1" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="rank" HeaderText="Rank" /> 
                <asp:BoundField DataField="guild_name" HeaderText="Name" />                 
                <asp:BoundField DataField="point" HeaderText="Rank Point" /> 
                <asp:TemplateField HeaderText="Point Edit">
                    <ItemTemplate>
                        <input type="text" name="<%# Eval("gid")  %>" size="20" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView><br />
        <input type="Submit" value=" Submit "/>
    </div>
    </form>
</body>
</html>
