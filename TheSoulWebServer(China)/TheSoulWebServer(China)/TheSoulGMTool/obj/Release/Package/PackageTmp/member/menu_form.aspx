<%@ Page MasterPageFile="~/Main.Master" Language="C#" AutoEventWireup="true" CodeBehind="menu_form.aspx.cs" Inherits="TheSoulGMTool.member.menu_form" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="span9">
        <table border="0" style="background-color: #ffffff; width: 100%">
            <tr>
                <td>
                    <asp:HiddenField ID="idx" runat="server" />

                    <table class="table table-bordered">
                        <tr>
                            <th>
                                <asp:Label ID="Label5" Text="<%$ Resources:languageResource ,lang_group %>" runat="server"></asp:Label>
                            </th>
                            <td style="vertical-align: bottom;">
                                <asp:DropDownList ID="largeMenu" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <asp:Label ID="Label1" Text="Menu Name" runat="server"></asp:Label>
                            </th>
                            <td style="vertical-align: bottom;">
                                <asp:TextBox MaxLength="50" ID="menu" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <asp:Label ID="Label2" Text="<%$ Resources:languageResource ,lang_sortNum %>" runat="server"></asp:Label>
                            </th>
                            <td style="vertical-align: bottom;">
                                <asp:TextBox TextMode="Number" CssClass="onlyNumber" MaxLength="100" ID="orderNum" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                <asp:Label ID="Label7" Text="<%$ Resources:languageResource ,lang_service %>" runat="server"></asp:Label></th>
                            <td>
                                <asp:RadioButtonList CssClass="table-unbordered" RepeatDirection="Horizontal" ID="active" runat="server">
                                    <asp:ListItem Text="<%$ Resources:languageResource ,lang_notapply %>" Value="N" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:languageResource ,lang_apply %>" Value="Y"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <button type="submit" class="btn">
                        <asp:Label ID="Label8" Text="<%$ Resources:languageResource ,lang_btn_update %>" runat="server"></asp:Label></button>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
