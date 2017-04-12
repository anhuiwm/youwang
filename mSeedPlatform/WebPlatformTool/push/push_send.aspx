<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="push_send.aspx.cs" Inherits="WebPlatformTool.push.push_send" %>

<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="span9">
    <table class="table table-bordered">
        <tbody>
            <tr>
                <th>
                    <asp:Label ID="Label5" Text="Game" runat="server"></asp:Label></th>
                <td>
                    <asp:DropDownList ID="game_id" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>Push Type</th>
                <td>
                    <asp:RadioButtonList ID="pushType" CssClass="table-unbordered" RepeatDirection="Horizontal" runat="server">
                        <asp:ListItem Value="0" Text="개발용开发用" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="1" Text="배포용分发用"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="Label2" Text="<%$ Resources:StringResource ,lang_sendTime %>" runat="server"></asp:Label></th>
                <td>
                    <asp:TextBox ID="date" TextMode="Date" Width="150" runat="server"></asp:TextBox>
                    <asp:DropDownList ID="hour" Width="80" runat="server"></asp:DropDownList>
                    <asp:DropDownList ID="min" Width="80" runat="server"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th style="width:100px;">
                    <asp:Label ID="Label1" Text="<%$ Resources:StringResource ,lang_title %>" runat="server"></asp:Label></th>
                <td>
                    <asp:TextBox ID="txtTitle" Width="70%" MaxLength="16" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="Label3" Text="<%$ Resources:StringResource ,lang_message %>" runat="server"></asp:Label></th>
                <td>
                    <asp:TextBox ID="txtMessage" Width="80%" MaxLength="110" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="Label4" Text="<%$ Resources:StringResource ,lang_reason %>" runat="server"></asp:Label></th>
                <td>
                    <asp:TextBox TextMode="MultiLine" ID="txtReason" runat="server"></asp:TextBox>
                </td>
            </tr>
        </tbody>
    </table>
    <button type="submit" class="btn">
        <asp:Label ID="Label31" Text="확인确认" runat="server"></asp:Label></button>
    </div>
</asp:Content>
