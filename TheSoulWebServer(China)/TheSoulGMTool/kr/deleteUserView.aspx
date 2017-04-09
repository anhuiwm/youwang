<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="deleteUserView.aspx.cs" Inherits="TheSoulGMTool.kr.deleteUserView" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="span9">
        <asp:GridView ID="dataList" CssClass="table table-bordered" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField ItemStyle-Width="20%" DataField="server_group_name" HeaderText="server" />
                <asp:BoundField ItemStyle-Width="20%" DataField="user_server_nickname" HeaderText="nickname" />
                <asp:BoundField ItemStyle-Width="20%" DataField="user_server_status" HeaderText="level" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
