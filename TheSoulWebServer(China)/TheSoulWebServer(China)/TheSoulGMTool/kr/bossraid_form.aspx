<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="bossraid_form.aspx.cs" Inherits="TheSoulGMTool.kr.bossraid_form" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#username").bind("keydown keyup keypress blur", function () {
                var userNameLength = getByteLength($(this).val());
                if (userNameLength > 30) {
                    $(this).val($(this).val().substring(0, $(this).val().length-1));
                }
            });
        });
    </script>
    <div class="span9">
        <table class="table table-bordered">
            <tbody>
                <tr>
                    <th><asp:Label ID="Label2" Text="Name" runat="server"></asp:Label></th>
                    <td><asp:TextBox ID="username" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <th>Boss</th>
                    <td>
                        <asp:DropDownList ID="boss" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="Label1" Text="Open Time" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="startDay" runat="server" TextMode="Date" Width="150"></asp:TextBox>
                        <asp:DropDownList ID="sHour" runat="server" Width="80">
                        </asp:DropDownList>
                        <asp:DropDownList ID="sMin" runat="server" Width="80">
                        </asp:DropDownList> 
                        &nbsp;~
                        <asp:TextBox ID="endDay" runat="server" TextMode="Date" Width="150"></asp:TextBox>
                        <asp:DropDownList ID="eHour" runat="server" Width="80">
                        </asp:DropDownList>
                        <asp:DropDownList ID="eMin" runat="server" Width="80">
                        </asp:DropDownList>
                    </td>
                </tr>
            </tbody>
        </table>
        <asp:Button ID="Button1" CssClass="btn" Text="<%$ Resources:languageResource ,lang_btn_ok %>" OnClientClick="submit();" runat="server" />
    </div>
</asp:Content>
