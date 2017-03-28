<%@ Page MasterPageFile="~/Main.Master" Language="C#" AutoEventWireup="true" CodeBehind="eventGroupEdite.aspx.cs" Inherits="TheSoulGMTool.systemEvent.eventGroupEdite" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="span9">
        <asp:HiddenField ID="groupType" runat="server" />
        <asp:HiddenField ID="idx" runat="server" />
        <table class="table table-bordered">
            <tbody>
                <tr>
                    <th style="width:30%;"><asp:Label ID="lang_server" Text="<%$ Resources:languageResource ,lang_server %>" runat="server" ></asp:Label></th>
                    <td>
                        <span id="change_server" runat="server"></span>
                        
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="Label2" Text="<%$ Resources:languageResource ,lang_sortNum %>" runat="server" ></asp:Label></th>
                    <td><asp:TextBox TextMode="Number" CssClass="onlyNumber" ID="ordernum" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <th><asp:Label ID="Label6" Text="<%$ Resources:languageResource ,lang_eventType %>" runat="server" ></asp:Label></th>
                    <td><asp:TextBox ID="eventType" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <th><asp:Label ID="Label4" Text="<%$ Resources:languageResource ,lang_title %>" runat="server" ></asp:Label></th>
                    <td><asp:TextBox ID="title" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <th><asp:Label ID="Label5" Text="<%$ Resources:languageResource ,lang_tootip %>" runat="server" ></asp:Label></th>
                    <td><asp:TextBox ID="tootip" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <th><asp:Label ID="Label1" Text="<%$ Resources:languageResource ,lang_isActive %>" runat="server" ></asp:Label></th>
                    <td>
                        <asp:RadioButtonList CssClass="table-unbordered" ID="active" RepeatDirection="Horizontal" runat="server">
                            <asp:ListItem Value="O" Selected="True">O</asp:ListItem>
                            <asp:ListItem Value="X">X</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
            </tbody>
        </table>
        <button class="btn" type="submit"><asp:Label ID="Label3" Text="<%$ Resources:languageResource ,lang_btn_ok %>" runat="server" ></asp:Label></button>
        </div>
</asp:Content>
