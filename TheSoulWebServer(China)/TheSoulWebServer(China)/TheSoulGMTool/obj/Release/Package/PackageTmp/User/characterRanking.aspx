<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="characterRanking.aspx.cs" Inherits="TheSoulGMTool.User.characterRanking" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var isFormSubmit;
        isFormSubmit = false;

        function rankingDelete2(idx, type) {
            if (isFormSubmit) {
                alert("<%=HttpContext.GetGlobalResourceObject("languageResource","lang_msgDoubleClick") %>");
                return false;
            }
            else {
                isFormSubmit = true;
                $("input:hidden[id=userid]").val(idx);
                $("#classType").val(type);
                document.forms[0].submit();
            }
        }
    </script>
    <div class="span9">
        <table class="table table-bordered" style="width: 70%;">
            <tr>
                <th style="background-color:#EAEAEA;">
                    <a href="/User/characterRanking.aspx?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>"><asp:Label ID="Label1" Text="<%$ Resources:languageResource ,lang_characterRanking %>" runat="server" ></asp:Label></a><a>
                </th>
                <th>
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
        <asp:HiddenField ID="classType" runat="server" />
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
                <asp:BoundField DataField="point" HeaderText="<%$ Resources:languageResource ,lang_point %>" />
                <asp:TemplateField HeaderText="<%$ Resources:languageResource ,lang_btn_delete %>">
                    <ItemTemplate>  
                        <button type="button" class="btn" onclick="rankingDelete2('<%#Eval("aid")%>', '<%#Eval("Class")%>');"><asp:Label ID="Label9" Text="<%$ Resources:languageResource ,lang_btn_delete %>" runat="server" ></asp:Label></button>
                    </ItemTemplate>
                </asp:TemplateField>
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
