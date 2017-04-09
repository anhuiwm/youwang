<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="deleteUserList.aspx.cs" Inherits="TheSoulGMTool.kr.deleteUserList" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="span9">
        
        <asp:GridView ID="dataList" CssClass="table table-bordered" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField ItemStyle-Width="20%" DataField="reg_date" HeaderText="<%$ Resources:languageResource ,lang_withdrawDate %>" />
                <asp:TemplateField ItemStyle-Width="15%" HeaderText="Platform Type">
                    <ItemTemplate>
                        <%# Enum.GetName(typeof(TheSoul.DataManager.Global.Global_Define.ePlatformType),Convert.ToInt32(Eval("platform_type"))).Replace("EPlatformType_", "") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Platform ID">
                    <ItemTemplate>
                        <a href="deleteUserView.aspx?aid=<%# Eval("user_account_idx") %>&ca2=<%=Master.ca2%>&select_server=<%=Master.serverID%>"><%#Eval("platform_user_id") %></a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField ItemStyle-Width="15%" DataField="user_account_idx" HeaderText="AID" />
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
