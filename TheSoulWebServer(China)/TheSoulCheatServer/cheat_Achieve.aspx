<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cheat_Achieve.aspx.cs" Inherits="TheSoulCheatServer.cheat_Achieve" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        UserName : <input type="text" name="username" id="username" runat="server" size="20"/> </br>
        <asp:GridView id="DataGrid1" runat="server" AutoGenerateColumns="false">
            <Columns>
                
                <asp:BoundField DataField="Description" HeaderText="Achieve" /> 
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <input type="hidden" name="AchieveID" value="<%# Eval("AchieveID")  %>" />
                        <input type="text" name="<%# Eval("AchieveID")  %>" size="20" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <input type="Submit" value=" Submit "/>
    </div>
    </form>
</body>
</html>
