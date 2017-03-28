<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="errorLogDecrypt.aspx.cs" Inherits="TheSoulGMTool.management.errorLogDecrypt" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="span9">
         encrypt Data : <asp:textbox id="encryptdata" runat="server" size="100"></asp:textbox><br />
        <button type="submit">submit</button>
        <br /><br />
        DecryptString :<br />
        <asp:Label ID="decrypt" runat="server"></asp:Label>
    </div>
</asp:Content>