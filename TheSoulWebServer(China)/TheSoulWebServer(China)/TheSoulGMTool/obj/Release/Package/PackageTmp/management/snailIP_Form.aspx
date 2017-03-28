<%@ Page MasterPageFile="~/Main.Master" Language="C#" AutoEventWireup="true" CodeBehind="snailIP_Form.aspx.cs" Inherits="TheSoulGMTool.management.snailIP_Form" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">.

    <div class="span9">
        <table border="0" style="background-color: #ffffff; width: 100%">
            <tr>
                <td>
                    <table class="table table-bordered">
                        <tr>
                            <th>
                                <asp:Label ID="Label2" Text="IP" runat="server"></asp:Label>
                            </th>
                            <td style="vertical-align: bottom;">
                                <asp:TextBox TextMode="Number" CssClass="onlyNumber" MaxLength="3" ID="ip1" Width="50" runat="server"></asp:TextBox>.
                                <asp:TextBox TextMode="Number" CssClass="onlyNumber" MaxLength="3" ID="ip2" Width="50" runat="server"></asp:TextBox>.
                                <asp:TextBox TextMode="Number" CssClass="onlyNumber" MaxLength="3" ID="ip3" Width="50" runat="server"></asp:TextBox>.
                                <asp:TextBox TextMode="Number" CssClass="onlyNumber" MaxLength="3" ID="ip4" Width="50" runat="server"></asp:TextBox>
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
