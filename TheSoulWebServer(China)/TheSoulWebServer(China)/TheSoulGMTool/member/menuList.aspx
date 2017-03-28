<%@ Page MasterPageFile="~/Main.Master" Language="C#" AutoEventWireup="true" CodeBehind="menuList.aspx.cs" Inherits="TheSoulGMTool.member.menuList" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="span9">
        <asp:GridView ID="dataList" CssClass="table table-bordered" runat="server" AutoGenerateColumns="false">
            <Columns>                
                <asp:BoundField DataField="linkurl" HeaderText="<%$ Resources:languageResource ,lang_group %>" /> 
                 <asp:BoundField DataField="menuname" HeaderText="Menu Name" /> 
                <asp:TemplateField>
                    <ItemTemplate>                        
                        <button class="btn" type="button" onclick="golink('menu_form.aspx','?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>&idx=<%# Eval("idx") %>')"><asp:Label ID="Label1" Text="<%$ Resources:languageResource ,lang_btn_update %>" runat="server" ></asp:Label></button>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
