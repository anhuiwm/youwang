<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="guildList.aspx.cs" Inherits="TheSoulGMTool.kr.guildList" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="span9">
        <table class="table table-bordered">
            <tbody>
                <tr>
                    <th><asp:Label ID="Label1" Text="<%$ Resources:languageResource ,lang_searchType %>" runat="server" ></asp:Label></th>
                    <td><asp:Label ID="Label2" Text="Guild" runat="server" ></asp:Label> :
                        <asp:TextBox ID="guildname" runat="server"></asp:TextBox> <asp:Button ID="Button1" CssClass="btn" Text="<%$ Resources:languageResource ,lang_btn_search %>" OnClick="Button1_Click" runat="server" />
                    </td>
                </tr>
            </tbody>
        </table>
        <asp:HiddenField ID="idx" runat="server" />
        <asp:GridView ID="dataList" CssClass="table table-bordered" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:TemplateField HeaderText="<%$ Resources:languageResource ,lang_guildName %>">
                    <ItemTemplate>
                        <a href="guildView.aspx?gid=<%# Eval("GuildID") %>&ca2=<%=Master.ca2%>&select_server=<%=Master.serverID%>"><%#Eval("GuildName") %></a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="GuildCreateDate" HeaderText="<%$ Resources:languageResource ,lang_creationdate %>" />
                <asp:BoundField DataField="GuildCreateUserName" HeaderText="<%$ Resources:languageResource ,lang_guildLeader %>" />
                <asp:BoundField DataField="GuildMark" HeaderText="<%$ Resources:languageResource ,lang_mark %>" />
                <asp:BoundField DataField="GuildLevel" HeaderText="<%$ Resources:languageResource ,lang_level %>" />
                <asp:BoundField DataField="GuildWithdrawPoint" HeaderText="<%$ Resources:languageResource ,lang_ranking %>" />
                <asp:BoundField DataField="GuildRankingPoint" HeaderText="<%$ Resources:languageResource ,lang_enPoint %>" />
                <asp:BoundField DataField="GuildExp" HeaderText="<%$ Resources:languageResource ,lang_exp %>" />
                <asp:BoundField DataField="GuildWaitingCount" HeaderText="<%$ Resources:languageResource ,lang_guildJoinerCount %>" />
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
