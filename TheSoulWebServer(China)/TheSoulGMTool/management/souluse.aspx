<%@ Page MasterPageFile="~/Main.Master" Language="C#" AutoEventWireup="true" CodeBehind="souluse.aspx.cs" Inherits="TheSoulGMTool.management.souluse" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="span9">
        <table border="0" style="background-color:#ffffff;width:100%">
                <tr>
                    <td>
                        <asp:HiddenField ID="slotID" runat="server" />
                        <table class="table table-bordered">
                            <tr>
                                <th><asp:Label ID="Label1" Text="<%$ Resources:languageResource ,lang_server %>" runat="server" ></asp:Label></th>
                                <td><span id="change_server" runat="server"></span></td>
                            </tr>
                        </table>
                        <table class="table table-unbordered">
                            <tr>
                                <td style="text-align:center;">
                                    Show Soul List<br />
                                    <asp:ListBox Height="150px" ID="showSoul" runat="server"></asp:ListBox>
                                </td>
                                <td style="text-align:center;vertical-align:middle;">
                                    <asp:Button id="addItem" CssClass="btn" Text="->" OnClick="addItem_Click" runat="server" /><br />
                                    <asp:Button id="removeItem" CssClass="btn" Text="<-" OnClick="removeItem_Click" runat="server" />
                                </td>
                                <td style="text-align:center;">
                                    Hide Soul List<br />
                                    <asp:ListBox Height="150px" ID="hideSoul" runat="server"></asp:ListBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="text-align:right">
                        <asp:Button id="submit" CssClass="btn" Text="<%$ Resources:languageResource ,lang_btn_ok %>" OnClick="submit_Click" runat="server" />
                    </td>
                </tr>
            </table>

    </div>
</asp:Content>