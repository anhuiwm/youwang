<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="user_loglevel.aspx.cs" Inherits="TheSoulGMTool.kr.user_loglevel" %>
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
                    <th><asp:Label ID="Label1" Text="<%$ Resources:languageResource ,lang_logDate %>" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="logDay" runat="server" TextMode="Date" Width="150"></asp:TextBox>
                        <asp:DropDownList ID="logHour" runat="server" Width="80">
                        </asp:DropDownList>
                        <asp:DropDownList ID="logMin" runat="server" Width="80">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="Label3" Text="<%$ Resources:languageResource ,lang_logLevel %>" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="logLevel" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="Label4" Text="<%$ Resources:languageResource ,lang_logCS %>" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="csLogLevel" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
            </tbody>
        </table>
        <asp:Button ID="Button1" CssClass="btn" Text="<%$ Resources:languageResource ,lang_btn_ok %>" OnClientClick="submit();" runat="server" />
    </div>
</asp:Content>
