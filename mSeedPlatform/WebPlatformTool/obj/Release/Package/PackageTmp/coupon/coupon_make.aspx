<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="coupon_make.aspx.cs" Inherits="WebPlatformTool.coupon.coupon_make" %>

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
                        <asp:ListItem Text="DarkBlaze" Value="1"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th style="width: 100px;">
                    <asp:Label ID="Label10" Text="<%$ Resources:StringResource ,lang_makeType %>" runat="server"></asp:Label></th>
                <td>
                    <asp:RadioButtonList ID="couponType" CssClass="table-unbordered" RepeatDirection="Horizontal" runat="server">
                        <asp:ListItem Value="1" Text="<%$ Resources:StringResource ,lang_couponType1 %>"></asp:ListItem>
                        <asp:ListItem Value="2" Text="<%$ Resources:StringResource ,lang_couponType2 %>"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <th style="">
                    <asp:Label ID="Label1" Text="<%$ Resources:StringResource ,lang_eventName %>" runat="server"></asp:Label></th>
                <td>
                    <asp:TextBox ID="eventName" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="Label2" Text="<%$ Resources:StringResource ,lang_expirationDate %>" runat="server"></asp:Label></th>
                <td>
                    <asp:RadioButtonList ID="checkDate" RepeatDirection="Horizontal" runat="server">
                        <asp:ListItem Value="1" Text="<%$ Resources:StringResource ,lang_unlimited %>"></asp:ListItem>
                        <asp:ListItem Value="2" Text="<%$ Resources:StringResource ,lang_inputDate %>"></asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:TextBox ID="startDay" TextMode="Date" Width="150" runat="server"></asp:TextBox>
                    <asp:TextBox ID="endDay" TextMode="Date" Width="150" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="Label3" Text="<%$ Resources:StringResource ,lang_makeCount %>" runat="server"></asp:Label></th>
                <td>
                    <asp:TextBox ID="couponCount" TextMode="Number" CssClass="onlyNumber" Width="110" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="Label6" Text="<%$ Resources:StringResource ,lang_couponCode %>" runat="server"></asp:Label></th>
                <td>
                    <asp:TextBox ID="couponCode" Width="110" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="Label12" Text="<%$ Resources:StringResource ,lang_allItem %>" runat="server"></asp:Label></th>
                <td>
                    <button type="button" class="btn" onclick="tableAdd('allreward')"><asp:Label ID="Label20" Text="<%$ Resources:StringResource ,lang_btn_add %>" runat="server"></asp:Label></button>
                    <table class="table table-bordered allreward" style="width:90%" id="allreward" runat="server">
                    </table>
                </td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="Label4" Text="<%$ Resources:StringResource ,lang_memo %>" runat="server"></asp:Label></th>
                <td>
                    <asp:TextBox TextMode="MultiLine" ID="memo" runat="server"></asp:TextBox>
                </td>
            </tr>
        </tbody>
    </table>
    <button type="submit" class="btn">
        <asp:Label ID="Label31" Text="확인" runat="server"></asp:Label></button>
    </div>
</asp:Content>
