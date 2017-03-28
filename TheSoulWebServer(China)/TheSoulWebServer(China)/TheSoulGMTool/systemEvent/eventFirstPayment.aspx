<%@ Page MasterPageFile="~/Main.Master" Language="C#" AutoEventWireup="true" CodeBehind="eventFirstPayment.aspx.cs" Inherits="TheSoulGMTool.systemEvent.eventFirstPayment" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="span9">
        <table class="table table-bordered">
            <tr>
                <th>
                    <asp:Label ID="lang_server" Text="<%$ Resources:languageResource ,lang_server %>" runat="server"></asp:Label></th>
                <td><span id="change_server" runat="server"></span></td>
            </tr>
        </table>
        <table class="table table-bordered">
            <tr>
                <th>
                    <asp:Label ID="Label1" Text="<%$ Resources:languageResource ,lang_isActive %>" runat="server"></asp:Label></th>
                <td>
                    <asp:RadioButtonList CssClass="table-unbordered" ID="activeValue" RepeatDirection="Horizontal" runat="server">
                        <asp:ListItem Value="1" Text="On"></asp:ListItem>
                        <asp:ListItem Value="0" Text="Off"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="Label2" Text="<%$ Resources:languageResource ,lang_reward %>" runat="server"></asp:Label></th>
                <td>
                    <asp:Label ID="labReward" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="Label3" Text="<%$ Resources:languageResource ,lang_rewardTitle %>" runat="server"></asp:Label></th>
                <td>
                    <asp:TextBox ID="title" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="Label4" Text="<%$ Resources:languageResource ,lang_rewardTitle %>" runat="server"></asp:Label></th>
                <td>
                    <asp:TextBox ID="contents" runat="server"></asp:TextBox></td>
            </tr>
        </table>
        <button type="submit" class="btn">
            <asp:Label ID="Label5" Text="<%$ Resources:languageResource ,lang_btn_ok %>" runat="server"></asp:Label></button>
    </div>
</asp:Content>
