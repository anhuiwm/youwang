<%@ Page MasterPageFile="~/Main.Master" Language="C#" AutoEventWireup="true" CodeBehind="member_list.aspx.cs" Inherits="TheSoulGMTool.member.member_list" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="span9">
        <asp:GridView ID="dataList" CssClass="table table-bordered" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="15" OnPageIndexChanging="dataList_PageIndexChanging">
            <Columns>                
                <asp:BoundField DataField="name" HeaderText="<%$ Resources:languageResource ,lang_GMName %>" /> 
                <asp:TemplateField HeaderText="ID">
                    <ItemTemplate>
                        <a href="member_edite.aspx?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>&idx=<%#Eval("idx") %>"><%#Eval("userid") %></a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="part" HeaderText="<%$ Resources:languageResource ,lang_Department %>" /> 
                <asp:BoundField DataField="rank" HeaderText="<%$ Resources:languageResource ,lang_Position %>" /> 
                <asp:BoundField DataField="phone" HeaderText="<%$ Resources:languageResource ,lang_phoneNumber %>" /> 
                <asp:BoundField DataField="email" HeaderText="<%$ Resources:languageResource ,lang_Email %>" /> 
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>