<%@ Page MasterPageFile="~/Main.Master" Language="C#" AutoEventWireup="true" CodeBehind="snailIPList.aspx.cs" Inherits="TheSoulGMTool.management.snailIPList" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="span9">
        <asp:GridView ID="dataList" DataKeyNames="idx" OnRowCommand="dataList_RowCommand" CssClass="table table-bordered" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="15" OnPageIndexChanging="dataList_PageIndexChanging">
            <Columns>
                <asp:BoundField DataField="ip_address" HeaderText="IP" />
                <asp:ButtonField ButtonType="Button" ControlStyle-CssClass="btn" Text="<%$ Resources:languageResource ,lang_btn_delete %>" CommandName="deleteip" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
