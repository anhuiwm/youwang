<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="user_all_logout.aspx.cs" Inherits="TheSoulGMTool.kr.user_all_logout" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var isFormSubmit;
        isFormSubmit = false;

        function formSend() {
            if(confirm("<%=HttpContext.GetGlobalResourceObject("languageResource","lang_msgAllLogout")%>")){
                if (isFormSubmit) {
                    alert("<%=HttpContext.GetGlobalResourceObject("languageResource","lang_msgDoubleClick") %>");
                    return false;
                }
                else {
                    isFormSubmit = true;
                    document.forms[0].submit();
                }
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
            </tbody>
        </table>
        <button type="button" onclick="formSend()" class="btn"><asp:Label ID="Label6" Text="All User LogOut" runat="server"></asp:Label></button>
    </div>
</asp:Content>
