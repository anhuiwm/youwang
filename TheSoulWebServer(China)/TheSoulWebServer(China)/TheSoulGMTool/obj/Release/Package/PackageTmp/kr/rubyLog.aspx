<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="rubyLog.aspx.cs" Inherits="TheSoulGMTool.kr.rubyLog" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="span9">
        <asp:HiddenField id="pg" runat="server" />
        <table class="table table-bordered">
            <tbody>
                <tr>
                    <th><asp:Label ID="Label1" Text="<%$ Resources:languageResource ,lang_searchType %>" runat="server" ></asp:Label></th>
                    <td><asp:Label ID="Label2" Text="<%$ Resources:languageResource ,lang_userNick %>" runat="server" ></asp:Label> :
                        <asp:TextBox ID="username" runat="server"></asp:TextBox><br />
                        <asp:Label ID="Label5" Text="<%$ Resources:languageResource ,lang_platfromID %>" runat="server" ></asp:Label> :
                        <asp:TextBox ID="uid" runat="server"></asp:TextBox><br />
                        <asp:Label ID="Label4" Text="Date" runat="server" ></asp:Label> :
                        <asp:TextBox TextMode="Date" ID="sDate" runat="server"></asp:TextBox>
                        ~
                        <asp:TextBox ID="eDate" TextMode="Date" runat="server"></asp:TextBox><br />
                        <asp:Label ID="Label6" Text="<%$ Resources:languageResource ,lang_goodsType %>" CssClass="table-unbordered" runat="server" ></asp:Label> :
                        <asp:RadioButtonList ID="moneyType" RepeatDirection="Horizontal" CssClass="table-unbordered" runat="server">
                            <asp:ListItem Text="<%$ Resources:languageResource ,lang_ruby %>" Value="0" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:languageResource ,lang_gold %>" Value="1"></asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:Label ID="Label3" Text="<%$ Resources:languageResource ,lang_type %>" CssClass="table-unbordered" runat="server" ></asp:Label> :
                        <asp:RadioButtonList ID="eventType" RepeatDirection="Horizontal"  CssClass="table-unbordered" runat="server">
                            <asp:ListItem Text="<%$ Resources:languageResource ,lang_whole %>" Value="-1"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:languageResource ,lang_acquire %>" Value="0" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:languageResource ,lang_spend %>" Value="1"></asp:ListItem>
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
        <asp:HiddenField ID="mailidx" runat="server" />
        <asp:GridView ID="dataList" CssClass="table table-bordered" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="d_create" HeaderText="<%$ Resources:languageResource ,lang_regdate %>" />
                <asp:BoundField DataField="s_comment" HeaderText="<%$ Resources:languageResource ,lang_type %>" />
                <asp:BoundField DataField="s_event_id" HeaderText="<%$ Resources:languageResource ,lang_division %>" />
                <asp:BoundField DataField="n_money" HeaderText="<%$ Resources:languageResource ,lang_amount %>" />
                <asp:BoundField DataField="n_before" DataFormatString="{0:0,0}" HeaderText="<%$ Resources:languageResource ,lang_beforeCount %>" />
                <asp:BoundField DataField="n_after" DataFormatString="{0:0,0}" HeaderText="<%$ Resources:languageResource ,lang_afterCount %>" />
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
