<%@ Page MasterPageFile="~/Main.Master" Language="C#" AutoEventWireup="true" CodeBehind="blackMarket.aspx.cs" Inherits="TheSoulGMTool.management.blackMarket" %>
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
                            <tr>
                                <th><asp:Label ID="Label2" Text="<%$ Resources:languageResource ,lang_sellIitemName %>" runat="server" ></asp:Label></th>
                                <td style="vertical-align:middle;">
                                    <asp:TextBox ID="name" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th><asp:Label ID="Label4" Text="<%$ Resources:languageResource ,lang_sellItemID %>" runat="server" ></asp:Label></th>
                                <td style="vertical-align:middle;">
                                    <asp:DropDownList Width="50%" ID="item" runat="server"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <th><asp:Label ID="Label6" Text="<%$ Resources:languageResource ,lang_amount %>" runat="server" ></asp:Label></th>
                                <td><asp:TextBox TextMode="Number" CssClass="onlyNumber" ID="itema" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <th><asp:Label ID="Label5" Text="<%$ Resources:languageResource ,lang_sellItemPrice %>" runat="server" ></asp:Label></th>
                                <td style="vertical-align:middle;">
                                    <asp:TextBox ID="price" TextMode="Number" CssClass="onlyNumber" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th><asp:Label ID="Label7" Text="<%$ Resources:languageResource ,lang_itemProb %>" runat="server" ></asp:Label></th>
                                <td><asp:TextBox TextMode="Number" CssClass="onlyNumber" ID="itemprob" runat="server"></asp:TextBox></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="text-align:right">
                       
                           <button type="submit" class="btn" ><asp:Label ID="Label8" Text="<%$ Resources:languageResource ,lang_btn_ok %>" runat="server" ></asp:Label></button> 
                    </td>
                </tr>
            </table>

    </div>
</asp:Content>