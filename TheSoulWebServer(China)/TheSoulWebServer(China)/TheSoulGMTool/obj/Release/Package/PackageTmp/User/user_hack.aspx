<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="user_hack.aspx.cs" Inherits="TheSoulGMTool.User.user_hack" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function submit() {
            isRunnig = false;
            document.forms[0].submit();
        }
    </script>
    <table class="table table-bordered">
            <tbody>
                <tr>
                    <th><asp:Label ID="Label1" Text="<%$ Resources:languageResource ,lang_detectCount %>" runat="server" ></asp:Label></th>
                    <td><asp:TextBox id="check_count" runat="server"></asp:TextBox> <asp:Button ID="Button2" CssClass="btn" Text="<%$ Resources:languageResource ,lang_btn_search %>" runat="server" /></td>
                </tr>
            </tbody>
        </table>
    <asp:GridView ID="dataList" CssClass="table table-bordered" OnRowCommand="dataList_RowCommand" DataKeyNames="aid" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="username" HeaderText="<%$ Resources:languageResource ,lang_userNick %>" />
                <asp:BoundField DataField="total_detect_count" HeaderText="Total Count" />
                <asp:BoundField DataField="today_detect_count" HeaderText="Today Count" />
                <asp:BoundField DataField="reg_date" HeaderText="Update Date" />
                <asp:ButtonField ButtonType="Button" ControlStyle-CssClass="btn" Text="<%$ Resources:languageResource ,lang_userRestrict %>" CommandName="restrict" />
                <asp:ButtonField ButtonType="Button" ControlStyle-CssClass="btn" Text="<%$ Resources:languageResource ,lang_lift %>" CommandName="delete" /> 
            </Columns>
        </asp:GridView>
</asp:Content>