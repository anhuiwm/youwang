<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="userRestore.aspx.cs" Inherits="TheSoulGMTool.kr.userRestore" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="span9">
        <table class="table table-bordered">
            <tbody>
               <tr>
                    <th><asp:Label ID="Label2" Text="<%$ Resources:languageResource ,lang_searchType %>" runat="server" ></asp:Label></th>
                    <td><asp:Label ID="Label3" Text="<%$ Resources:languageResource ,lang_userNick %>" runat="server" ></asp:Label> :
                        <asp:TextBox ID="username" runat="server"></asp:TextBox><br />
                        <asp:Label ID="Label5" Text="<%$ Resources:languageResource ,lang_platfromID %>" runat="server" ></asp:Label> :
                        <asp:TextBox ID="uid" runat="server"></asp:TextBox><br />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="Button1" CssClass="btn" Text="<%$ Resources:languageResource ,lang_btn_search %>" OnClick="Button1_Click" runat="server" />
                    </td>
                </tr>
            </tbody>
        </table>
        <asp:GridView ID="dataList" CssClass="table table-bordered" DataKeyNames="user_account_idx, platform_user_id, platform_type" runat="server" AutoGenerateColumns="false">
            <Columns>
                
                <asp:TemplateField ItemStyle-Width="15%" HeaderText="Platform Type">
                    <ItemTemplate>
                        <%# Enum.GetName(typeof(TheSoul.DataManager.Global.Global_Define.ePlatformType),Convert.ToInt32(Eval("platform_type"))).Replace("EPlatformType_", "") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="reg_date" HeaderText="<%$ Resources:languageResource ,lang_editDate %>" />
                <asp:BoundField DataField="platform_user_id" HeaderText="Platform ID" />                
                <asp:BoundField DataField="play_server" HtmlEncode="false" HeaderText="<%$ Resources:languageResource ,lang_userServerInfo %>" />
                <asp:TemplateField HeaderText="<%$ Resources:languageResource ,lang_state %>">
                    <ItemTemplate>
                        <asp:Button ID="account_restore" CssClass="btn" Text ="<%$ Resources:languageResource ,lang_restore %>" OnClick="account_restore_Click" runat="server" Visible='<%# Convert.ToInt32(Eval("user_account_status")) > 0 ?false:true%>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>        
    </div>
</asp:Content>