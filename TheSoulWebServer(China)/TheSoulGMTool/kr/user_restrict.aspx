<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="user_restrict.aspx.cs" Inherits="TheSoulGMTool.kr.user_restrict" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="span9">
        <table class="table table-bordered">
            <tbody>
                <tr>
                    <th><asp:Label ID="Label2" Text="<%$ Resources:languageResource ,lang_userName %>" runat="server"></asp:Label></th>
                    <td><asp:TextBox ID="username" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <th>AID</th>
                    <td><asp:TextBox TextMode="Number" CssClass="onlyNumber" ID="userAID" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <th><asp:Label ID="Label1" Text="<%$ Resources:languageResource ,lang_login_restrict %>" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="loginDay" runat="server" TextMode="Date" Width="150"></asp:TextBox>
                        <asp:DropDownList ID="loginHour" runat="server" Width="80">
                        </asp:DropDownList>
                        <asp:DropDownList ID="loginMin" runat="server" Width="80">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="Label3" Text="<%$ Resources:languageResource ,lang_chat_restrict %>" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="chatDay" runat="server" TextMode="Date" Width="150"></asp:TextBox>
                        <asp:DropDownList ID="chatHour" runat="server" Width="80">
                        </asp:DropDownList>
                        <asp:DropDownList ID="chatMin" runat="server" Width="80">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th>Memo</th>
                    <td>
                        <asp:TextBox TextMode="MultiLine" ID="memo" Rows="3" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </tbody>
        </table>
        <asp:Button ID="Button1" CssClass="btn" Text="<%$ Resources:languageResource ,lang_btn_ok %>" OnClientClick="submit();" runat="server" />
    </div>
</asp:Content>
