<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Item_Charge2.aspx.cs" Inherits="TheSoulCheatServer.Item_Charge2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        UserName : <asp:textbox id="username" runat="server"></asp:textbox> </br>
        Class Select : <select name="CLASS"><option value="1">무사</option><option value="2">검객</option></select> </br>
        Passive Soul Creation <input type="checkbox" name="passive" value="passive" /><br />
        <table border="0">
            <tr>
                <td>ItemID</td>
                <td>Grade</td>
                <td>ItemEA</td>
            </tr>
            <tr>
                <td><asp:textbox id="ItemID" runat="server"></asp:textbox></td>
                <td><select name="Grade">
                    <option value="1">1성</option>
                    <option value="2">2성</option>
                    <option value="3">3성</option>
                    <option value="4">4성</option>
                    <option value="5">5성</option>
                    </select>
                </td>
                <td><asp:textbox id="ItemEA" runat="server"></asp:textbox> </td>
            </tr>
            <tr>
                <td><asp:textbox id="ItemID2" runat="server"></asp:textbox></td>
                <td><select name="Grade2">
                    <option value="1">1성</option>
                    <option value="2">2성</option>
                    <option value="3">3성</option>
                    <option value="4">4성</option>
                    <option value="5">5성</option>
                    </select>
                </td>
                <td><asp:textbox id="ItemEA2" runat="server"></asp:textbox> </td>
            </tr>
            <tr>
                <td><asp:textbox id="ItemID3" runat="server"></asp:textbox></td>
                <td><select name="Grade3">
                    <option value="1">1성</option>
                    <option value="2">2성</option>
                    <option value="3">3성</option>
                    <option value="4">4성</option>
                    <option value="5">5성</option>
                    </select>
                </td>
                <td><asp:textbox id="ItemEA3" runat="server"></asp:textbox> </td>
            </tr>
            <tr>
                <td><asp:textbox id="ItemID4" runat="server"></asp:textbox></td>
                <td><select name="Grade4">
                    <option value="1">1성</option>
                    <option value="2">2성</option>
                    <option value="3">3성</option>
                    <option value="4">4성</option>
                    <option value="5">5성</option>
                    </select>
                </td>
                <td><asp:textbox id="ItemEA4" runat="server"></asp:textbox> </td>
            </tr>
            <tr>
                <td><asp:textbox id="ItemID5" runat="server"></asp:textbox></td>
                <td><select name="Grade5">
                    <option value="1">1성</option>
                    <option value="2">2성</option>
                    <option value="3">3성</option>
                    <option value="4">4성</option>
                    <option value="5">5성</option>
                    </select>
                </td>
                <td><asp:textbox id="ItemEA5" runat="server"></asp:textbox> </td>
            </tr>
            <tr>
                <td><asp:textbox id="ItemID6" runat="server"></asp:textbox></td>
                <td><select name="Grade6">
                    <option value="1">1성</option>
                    <option value="2">2성</option>
                    <option value="3">3성</option>
                    <option value="4">4성</option>
                    <option value="5">5성</option>
                    </select>
                </td>
                <td><asp:textbox id="ItemEA6" runat="server"></asp:textbox> </td>
            </tr>
        </table>

        <br />
        
        <input type="Submit" value=" Submit "/>
    </div>
    </form>
</body>
</html>
