<%@ Page MasterPageFile="~/Main.Master" Language="C#" AutoEventWireup="true" CodeBehind="cdkeyLog.aspx.cs" Inherits="TheSoulGMTool.User.cdkeyLog" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
    .classTD tbody tr td {
        white-space:normal;
        word-break: break-all;
    }
</style>
    <div class="span9">
        
        <table class="table table-bordered">
            <tbody>
                <tr>
                    <th><asp:Label ID="Label1" Text="<%$ Resources:languageResource ,lang_searchType %>" runat="server" ></asp:Label></th>
                    <td>Reg Date :
                        <asp:TextBox TextMode="Date" ID="sDate" runat="server"></asp:TextBox>
                        ~
                        <asp:TextBox ID="eDate" TextMode="Date" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="Button1" CssClass="btn" Text="<%$ Resources:languageResource ,lang_btn_search %>" OnClientClick="submit();" runat="server" />
                        <asp:Button ID="Button2" CssClass="btn" Text="<%$ Resources:languageResource ,lang_excel %>" OnClick="Button2_Click" runat="server" />
                    </td>
                </tr>
            </tbody>
        </table>
        
        <asp:GridView ID="dataList" CssClass="table table-bordered" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField ControlStyle-Width="10%" DataField="AID" HeaderText="Game AID" />
                <asp:BoundField ControlStyle-Width="12%" DataField="platform_idx" HeaderText="<%$ Resources:languageResource ,lang_snailIndex %>" />
                <asp:BoundField ControlStyle-Width="15%" DataField="platform_user_id" HeaderText="<%$ Resources:languageResource ,lang_snailID %>" />
                <asp:BoundField ControlStyle-Width="15%" DataField="userName" HeaderText="<%$ Resources:languageResource ,lang_userNick %>" />
                <asp:BoundField ControlStyle-Width="15%" DataField="cdkey" HeaderText="coupon_key" />
                <asp:BoundField ControlStyle-Width="10%" DataField="mailseq" HeaderText="mailseq_json" />
                <asp:BoundField ControlStyle-Width="10%" DataField="stateflag" HeaderText="stateflag" />                
                <asp:BoundField ControlStyle-Width="13%" DataField="regdate" HeaderText="regdate" />
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