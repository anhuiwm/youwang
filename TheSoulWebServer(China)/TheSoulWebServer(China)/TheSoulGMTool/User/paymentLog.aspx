<%@ Page MasterPageFile="~/Main.Master" Language="C#" AutoEventWireup="true" CodeBehind="paymentLog.aspx.cs" Inherits="TheSoulGMTool.User.paymentLog" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function openPop(idx, tablename) {
            window.open('/User/paylog.aspx?select_server=<%=Master.serverID%>&idx=' + idx , '_blank', 'width=600, height=500, toolbar=no, menubar=no, scrollbars=2, resizable=no, copyhistory=no');
        }
    </script>
    <div class="span9">
        <table class="table table-bordered">
            <tbody>
                <tr>
                    <th><asp:Label ID="Label1" Text="<%$ Resources:languageResource ,lang_userNick %>" runat="server" ></asp:Label></th>
                    <td><asp:TextBox id="username" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <th><asp:Label ID="Label5" Text="<%$ Resources:languageResource ,lang_snailID %>" runat="server" ></asp:Label></th>
                    <td><asp:TextBox id="uid" runat="server"></asp:TextBox></td>
                </tr>
                <tr style="display:none;">
                    <th><asp:Label ID="Label2" Text="<%$ Resources:languageResource ,lang_chargeType %>" runat="server" ></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="shoptype" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="Label6" Text="<%$ Resources:languageResource ,lang_payResult %>" runat="server" ></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="payResult" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="Label3" Text="<%$ Resources:languageResource ,lang_term %>" runat="server" ></asp:Label></th>
                    <td>
                        <asp:TextBox TextMode="Date" ID="sDate" runat="server"></asp:TextBox>
                        ~
                        <asp:TextBox TextMode="Date" ID="eDate" runat="server"></asp:TextBox>
                        <asp:Button ID="Button1" CssClass="btn" Text="<%$ Resources:languageResource ,lang_btn_search %>" OnClick="Button1_Click" runat="server" />
                        <asp:Button ID="Button2" CssClass="btn" Text="<%$ Resources:languageResource ,lang_excel %>" OnClick="csvSave" runat="server" />
                    </td>
                </tr>
            </tbody>
        </table>
        <asp:HiddenField ID="billingindex" runat="server" />
        <asp:GridView ID="payList" CssClass="table table-bordered" runat="server" AutoGenerateColumns="false">
            <Columns>                
                <asp:BoundField DataField="regdate" HeaderText="<%$ Resources:languageResource ,lang_paydate %>" /> 
                <asp:BoundField DataField="payResult" HeaderText="<%$ Resources:languageResource ,lang_payResult %>" /> 
                <asp:BoundField DataField="ErrorCode" HeaderText="ErrorCode" />
                <%--<asp:BoundField DataField="platform_idx" HeaderText="<%$ Resources:languageResource ,lang_snailIndex %>" />--%>
                <asp:BoundField DataField="platform_user_id" HeaderText="<%$ Resources:languageResource ,lang_snailID %>" />
                <asp:BoundField DataField="UserName" HeaderText="<%$ Resources:languageResource ,lang_userNick %>" />
                <asp:BoundField DataField="Shop_Goods_ID" HeaderText="<%$ Resources:languageResource ,lang_ItemID %>" />
                <asp:BoundField DataField="goodsName" HeaderText="<%$ Resources:languageResource ,lang_ItemName %>" />                
                <asp:BoundField DataField="shopType" HeaderText="<%$ Resources:languageResource ,lang_chargeType %>" />
                <asp:BoundField DataField="ItemDay" HeaderText="<%$ Resources:languageResource ,lang_dailyDate %>" /> 
                <asp:BoundField DataField="ItemNum" HeaderText="<%$ Resources:languageResource ,lang_baseRuby %>" /> 
                <asp:BoundField DataField="Bonus_ItemNum" HeaderText="<%$ Resources:languageResource ,lang_bousRuby %>" /> 
                <asp:BoundField DataField="Buy_PriceValue" HeaderText="<%$ Resources:languageResource ,lang_buyPrice %>" /> 
                <asp:TemplateField HeaderText="<%$ Resources:languageResource ,lang_btn_manage %>" > 
                    <ItemTemplate>
                        <%#
                            System.Convert.ToInt32(Eval("Billing_Status")) == (int)TheSoul.DataManager.Shop_Define.eBillingStatus.Error || System.Convert.ToInt32(Eval("Billing_Status")) == (int)TheSoul.DataManager.Shop_Define.eBillingStatus.Fail?
                        "<button type=\"button\" class=\"btn\" onclick=\"openPop("+Eval("BillingIndex")+")\">"+GetGlobalResourceObject("languageResource","lang_details")+"</asp:Label></button>" :
                        ""
                        %>
                        <%#
                            System.Convert.ToInt32(Eval("Billing_Status")) == (int)TheSoul.DataManager.Shop_Define.eBillingStatus.Error || System.Convert.ToInt32(Eval("Billing_Status")) == (int)TheSoul.DataManager.Shop_Define.eBillingStatus.Fail?
                        "<button type=\"button\" class=\"btn\" onclick=\"blilingStatus("+Eval("BillingIndex")+")\">"+GetGlobalResourceObject("languageResource","lang_btn_manage")+"</asp:Label></button>" :
                        ""
                        %>
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