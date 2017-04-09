<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="user_restrrict_log.aspx.cs" Inherits="TheSoulGMTool.kr.user_restrrict_log" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="span9">
        <table class="table table-bordered" style="width: 50%">
            <tr>
                <th><a href="user_restrictList.aspx?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>"><%=GetGlobalResourceObject("languageResource", "lang_userLoginRestrict") %></a></th>
                <th><a href="user_restrrict_log.aspx?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>"><%=GetGlobalResourceObject("languageResource", "lang_userLoginRestrictLog") %></a></th>
            </tr>
        </table>
        <br />
        <table class="table table-bordered">
            <tbody>
                <tr>
                    <th>
                        <asp:Label ID="Label1" Text="<%$ Resources:languageResource ,lang_searchType %>" runat="server"></asp:Label></th>
                    <td>
                        <asp:Label ID="Label2" Text="<%$ Resources:languageResource ,lang_userNick %>" runat="server"></asp:Label>
                        :
                        <asp:TextBox ID="username" runat="server"></asp:TextBox><br />
                        <asp:Label ID="Label5" Text="AID" runat="server"></asp:Label>
                        :
                        <asp:TextBox ID="aid" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="Button1" CssClass="btn" Text="<%$ Resources:languageResource ,lang_btn_search %>" OnClick="Button1_Click" runat="server" />
                    </td>
                </tr>
            </tbody>
        </table>
        <asp:GridView ID="dataList" CssClass="table table-bordered" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="user_account_idx" HeaderText="AID" />
                <asp:BoundField DataField="userInfo" HtmlEncode="false" HeaderText="User Name" />
                <asp:BoundField DataField="login_restrict_enddate" HeaderText="<%$ Resources:languageResource ,lang_loginRestrict %>" />
                <asp:BoundField DataField="chat_restrict_endate" HeaderText="<%$ Resources:languageResource ,lang_chatRestrict %>" />
                <asp:BoundField DataField="memo" HeaderText="Memo" />
                <asp:BoundField DataField="regdate" HeaderText="<%$ Resources:languageResource ,lang_regdate %>" />
            </Columns>
        </asp:GridView>
        <br />
        <asp:DataList RepeatDirection="Horizontal" runat="server" ID="dlPager" OnItemCommand="dlPager_ItemCommand">
            <ItemTemplate>
                <asp:LinkButton Enabled='<%#Eval("Enabled") %>' runat="server" ID="lnkPageNo" Text='<%#Eval("Text") %>' CommandArgument='<%#Eval("Value") %>' CommandName="PageNo"></asp:LinkButton>
            </ItemTemplate>
        </asp:DataList>
    </div>
</asp:Content>
