<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="coupon_list.aspx.cs" Inherits="WebPlatformTool.coupon.coupon_list" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="span9">
        <asp:HiddenField ID="pg" runat="server" />
        <asp:HiddenField ID="idx" runat="server" />
        <table style="border:0px none #ffffff; width:100%">
            <tr>
                <th>search</th>
                <td><asp:TextBox ID="search" runat="server"></asp:TextBox> <button type="submit" class="btn"><%=GetGlobalResourceObject("StringResource", "lang_btn_search") %></button></td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:GridView ID="dataList" CssClass="table table-bordered" runat="server" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField ControlStyle-Width="15" DataField="gameName" HeaderText="<%$ Resources:StringResource ,lang_gameName %>" />
                            <asp:BoundField ControlStyle-Width="10%" DataField="strCoupon_Type" HeaderText="<%$ Resources:StringResource ,lang_makeType %>" />
                            <asp:TemplateField ControlStyle-Width="25%" HeaderText="<%$ Resources:StringResource ,lang_eventName %>">
                                <ItemTemplate>
                                    <a href="javascript:void(0);" onclick="golink('/coupon/coupon_veiw.aspx?pg=<%=pg.Value%>&search=<%=search.Text%>&idx=<%#Eval("Coupon_Group_ID")%>')"><%#Eval("Coupon_Title")%></a>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ControlStyle-Width="20%" HeaderText="<%$ Resources:StringResource ,lang_expirationDate %>">
                                <ItemTemplate>
                                    <%#System.Convert.ToDateTime(Eval("startDate")).ToString("yyyy-MM-dd")%>~<%#System.Convert.ToDateTime(Eval("endDate")).ToString("yyyy-MM-dd")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField ControlStyle-Width="15%" DataField="Reg_date" HeaderText="<%$ Resources:StringResource ,lang_regDate %>" />
                            <asp:BoundField ControlStyle-Width="15%" DataField="Reg_id" HeaderText="<%$ Resources:StringResource ,lang_regGM %>" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:DataList RepeatDirection="Horizontal" runat="server" ID="dlPager" OnItemCommand="dlPager_ItemCommand">
                        <ItemTemplate>
                            <asp:LinkButton Enabled='<%#Eval("Enabled") %>' runat="server" ID="lnkPageNo" Text='<%#Eval("Text") %>' CommandArgument='<%#Eval("Value") %>' CommandName="PageNo"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:DataList>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>