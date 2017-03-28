<%@ Page MasterPageFile="~/Main.Master" Language="C#" AutoEventWireup="true" CodeBehind="serverVersion.aspx.cs" Inherits="TheSoulGMTool.management.serverVersion" %>
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
                        <asp:Label ID="Label1" Text="PlatformType" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="PlatformType" Width="90%" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="Label2" Text="Version" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="Version1" Width="50" MaxLength="2" runat="server"></asp:TextBox>.
                        <asp:TextBox ID="Version2" Width="50" MaxLength="2" runat="server"></asp:TextBox>.
                        <asp:TextBox ID="Version3" Width="50" MaxLength="2" runat="server"></asp:TextBox>.
                        <asp:TextBox ID="Version4" Width="50" MaxLength="3" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </tbody>
        </table>
        <button type="button" onclick="formSend()" class="btn"><asp:Label ID="Label6" Text="<%$ Resources:languageResource ,lang_btn_ok %>" runat="server"></asp:Label></button>
    </div>
</asp:Content>
