<%@ Page MasterPageFile="~/Main.Master" Language="C#" AutoEventWireup="true" CodeBehind="userAccount.aspx.cs" Inherits="TheSoulGMTool.User.userAccount" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="span9">
        <table class="table table-bordered">
            <tbody>
                <tr>
                    <th><asp:Label ID="Label2" Text="<%$ Resources:languageResource ,lang_snailID %>" runat="server"></asp:Label></th>
                    <td><asp:TextBox ID="platform" runat="server"></asp:TextBox></td>
                </tr>
            </tbody>
        </table>
        <asp:Button CssClass="btn" Text="<%$ Resources:languageResource ,lang_btn_ok %>" OnClientClick="submit();" runat="server" />
    </div>
</asp:Content>
