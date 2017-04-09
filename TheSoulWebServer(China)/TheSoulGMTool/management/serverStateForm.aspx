<%@ Page MasterPageFile="~/Main.Master" Language="C#" AutoEventWireup="true" CodeBehind="serverStateForm.aspx.cs" Inherits="TheSoulGMTool.management.serverStateForm" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var isFormSubmit;
        isFormSubmit = false;

        function formSend() {
            if (isFormSubmit) {
                alert("<%=HttpContext.GetGlobalResourceObject("languageResource","lang_msgDoubleClick") %>");
                return false;
            }
            else {
                isFormSubmit = true;
                document.forms[0].submit();
            }
        }
    </script>
    <div class="span9">
        <table class="table table-bordered">
            <tbody>
                <tr>
                    <th style="width: 25%;">
                        <asp:Label ID="Label10" Text="Server" runat="server"></asp:Label></th>
                    <td style="width: 75%;">
                        <span id="change_server" runat="server"></span>
                    </td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="Label1" Text="<%$ Resources:languageResource ,lang_serverStateChange %>" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="serverState" runat="server">
                            <asp:ListItem Text="Select" Value="-1"></asp:ListItem>
                            <asp:ListItem Text="Normal" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Hot" Value="11"></asp:ListItem>
                            <asp:ListItem Text="Recommand" Value="12"></asp:ListItem>
                            <asp:ListItem Text="New" Value="13"></asp:ListItem>
                            <asp:ListItem Text="Maintenance" Value="14"></asp:ListItem>
                            <asp:ListItem Text="Hide" Value="15"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </tbody>
        </table>
        <button type="button" onclick="formSend()" class="btn"><asp:Label ID="Label6" Text="<%$ Resources:languageResource ,lang_btn_ok %>" runat="server"></asp:Label></button>
    </div>
</asp:Content>
