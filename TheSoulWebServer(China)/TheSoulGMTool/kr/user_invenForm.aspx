<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="user_invenForm.aspx.cs" Inherits="TheSoulGMTool.kr.user_invenForm" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function sendForm() {
            
            var userValue1 = parseInt($("#item_count").val()) - parseInt($("#setData").val());

            if (userValue1 >= 0) {
                document.forms[0].submit();
            }
            else {
                alert("<%=GetGlobalResourceObject("languageResource","lang_msgDeleteItemCount")%>");
            }
        }
    </script>
    <div class="span9">
        <asp:HiddenField ID="idx" runat="server" />
        <table class="table table-bordered">
            <tbody>
                <tr>
                    <th><asp:Label ID="lang_clear_trigger1" Text="<%$ Resources:languageResource ,lang_Item %>" runat="server"></asp:Label></th>
                    <td><asp:Label ID="itemInfo" runat="server"></asp:Label>
                        <asp:HiddenField ID="item_count" runat="server" />
                    </td>
                    <td><asp:TextBox TextMode="Number" CssClass="onlyNumber" ID="setData" runat="server"></asp:TextBox></td>
                </tr>
            </tbody>
        </table>
        <button type="button" onclick="sendForm()" class="btn"><asp:Label ID="lang_btn_ok" Text="<%$ Resources:languageResource , lang_btn_update%>" runat="server"></asp:Label></button>
    </div>
</asp:Content>