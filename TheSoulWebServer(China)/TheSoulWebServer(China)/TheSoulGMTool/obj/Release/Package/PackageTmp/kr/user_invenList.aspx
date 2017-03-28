<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="user_invenList.aspx.cs" Inherits="TheSoulGMTool.kr.user_invenList" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <script type="text/javascript">
            function itemDelete(idx) {
                $("#idx").val(idx);
                document.forms[0].submit();
            }
    </script>
    <div class="span9">
         <table class="table table-bordered">
            <tbody>
                <tr>
                    <th><asp:Label ID="Label1" Text="<%$ Resources:languageResource ,lang_searchType %>" runat="server" ></asp:Label></th>
                    <td><asp:Label ID="Label2" Text="<%$ Resources:languageResource ,lang_userNick %>" runat="server" ></asp:Label> :
                        <asp:TextBox ID="username" runat="server"></asp:TextBox><br />
                        <asp:Label ID="Label5" Text="<%$ Resources:languageResource ,lang_platfromID %>" runat="server" ></asp:Label> :
                        <asp:TextBox ID="uid" runat="server"></asp:TextBox><br />
                        <asp:Label ID="Label6" Text="type" CssClass="table-unbordered" runat="server" ></asp:Label> :
                        <asp:RadioButtonList ID="itemType" RepeatDirection="Horizontal" CssClass="table-unbordered" runat="server">
                            <asp:ListItem Text="Item" Value="Item" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Soul Parts" Value="Soul_Parts"></asp:ListItem>
                            <asp:ListItem Text="Soul Equip" Value="Soul_Equip"></asp:ListItem>
                            <asp:ListItem Text="Orb" Value="ItemClass_Orb"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="Button1" CssClass="btn" Text="<%$ Resources:languageResource ,lang_btn_search %>" OnClick="Button1_Click" runat="server" />
                    </td>
                </tr>
            </tbody>
        </table>
        <asp:HiddenField ID="idx" runat="server" />
        <asp:GridView ID="dataList" CssClass="table table-bordered" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField ItemStyle-Width="10%" DataField="idx" HeaderText="Seq" />
                <asp:BoundField ItemStyle-Width="20%" DataField="Item_IndexID" HeaderText="Item ID" />
                <asp:BoundField ItemStyle-Width="25%" DataField="Name" HeaderText="<%$ Resources:languageResource ,lang_Item %>" />
                <asp:BoundField ItemStyle-Width="10%" DataField="grade" HeaderText="Grade" />
                <asp:BoundField ItemStyle-Width="10%" DataField="level" HeaderText="Level" />
                <asp:BoundField ItemStyle-Width="10%" DataField="itemea" HeaderText="<%$ Resources:languageResource ,lang_amount %>" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <button type="button" class="btn" onclick="golink('user_invenForm.aspx','?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>&username=<%=username.Text%>&uid=<%=uid.Text%>&itemtype=<%=itemType.SelectedValue%>&idx=<%#Eval("idx") %>')"><asp:Label ID ="Label7" Text="<%$ Resources:languageResource ,lang_btn_update %>" runat="server"></asp:Label></button>
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
