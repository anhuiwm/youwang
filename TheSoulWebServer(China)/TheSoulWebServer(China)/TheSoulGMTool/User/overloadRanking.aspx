<%@ Page MasterPageFile="~/Main.Master" Language="C#" AutoEventWireup="true" CodeBehind="overloadRanking.aspx.cs" Inherits="TheSoulGMTool.User.overloadRanking" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="span9">
        <table class="table table-bordered" style="width: 70%;">
            <tr>
                <th>
                    <a href="/User/characterRanking.aspx?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>"><asp:Label ID="Label1" Text="<%$ Resources:languageResource ,lang_characterRanking %>" runat="server" ></asp:Label></a><a>
                </th>
                <th style="background-color:#EAEAEA;">
                    <a href="/User/overloadRanking.aspx?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>"><asp:Label ID="Label2" Text="<%$ Resources:languageResource ,lang_overload %>" runat="server" ></asp:Label></a>
                </th>
                <th>
                    <a href="/User/rankingList.aspx?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>&rank=1"><asp:Label ID="Label3" Text="<%$ Resources:languageResource ,lang_freePVP %>" runat="server" ></asp:Label></a>
                </th>
                <th>
                    <a href="/User/rankingList.aspx?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>&rank=2"><asp:Label ID="Label4" Text="<%$ Resources:languageResource ,lang_1vs1PVP %>" runat="server" ></asp:Label></a>
                </th>
                <th>
                    <a href="/User/guildRanking.aspx?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>&rank=20"><asp:Label ID="Label5" Text="<%$ Resources:languageResource ,lang_guild %>" runat="server" ></asp:Label></a>
                </th>
                <th>
                    <a href="/User/guildRanking.aspx?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>&rank=3"><asp:Label ID="Label6" Text="<%$ Resources:languageResource ,lang_guildWar %>" runat="server" ></asp:Label></a>
                </th>
            </tr>
        </table>
        <asp:HiddenField ID="userid" runat="server" />
        <table class="table table-bordered">
            <tbody>
                <tr>
                    <th><asp:Label ID="Label7" Text="<%$ Resources:languageResource ,lang_userNick %>" runat="server" ></asp:Label></th>
                    <td>
                        <asp:TextBox ID="username" runat="server"></asp:TextBox>
                        <asp:Button ID="Button1" CssClass="btn" Text="<%$ Resources:languageResource ,lang_btn_search %>" OnClientClick="submit();" runat="server" />
                    </td>
                </tr>
            </tbody>
        </table>
        <asp:GridView ID="dataList" CssClass="table table-bordered" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="rank" HeaderText="<%$ Resources:languageResource ,lang_ranking %>" />
                <asp:BoundField DataField="username" HeaderText="<%$ Resources:languageResource ,lang_userName %>" />
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
