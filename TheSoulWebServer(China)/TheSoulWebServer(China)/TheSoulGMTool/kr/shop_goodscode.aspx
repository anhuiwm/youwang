<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="shop_goodscode.aspx.cs" Inherits="TheSoulGMTool.kr.shop_goodscode" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="span9">
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
    </div>
</asp:Content>
