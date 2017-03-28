<%@ Page MasterPageFile="~/Main.Master" Language="C#" AutoEventWireup="true" CodeBehind="halfpagckage.aspx.cs" Inherits="TheSoulGMTool.systemEvent.halfpagckage" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="span9">
        <asp:GridView ID="dataList" CssClass="table table-bordered" OnPageIndexChanging="dataList_PageIndexChanging" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="15">
           <Columns>
                <asp:BoundField DataField="Package_ID" HeaderText="ID" />
                <asp:BoundField DataField="NameCN1" HeaderText="<%$ Resources:languageResource ,lang_ItemName %>" />
                <asp:BoundField DataField="Buy_PriceValue" HeaderText="<%$ Resources:languageResource ,lang_price %>" />
                <asp:BoundField DataField="Buy_Day" HeaderText="<%$ Resources:languageResource ,lang_EventDay %>" />
                <asp:BoundField DataField="Grade" HeaderText="<%$ Resources:languageResource ,lang_grade %>" />
                <asp:TemplateField HeaderText="<%$ Resources:languageResource ,lang_service %>">
                    <ItemTemplate>
                        <%# TheSoulGMTool.GMDataManager.GetActiveType(Eval("ActiveType").ToString()) %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="DetailCN" HeaderText="<%$ Resources:languageResource ,lang_detailTootip %>" />
                <asp:BoundField DataField="Max_Buy" HeaderText="<%$ Resources:languageResource ,lang_count %>" />
               <asp:TemplateField  HeaderText="<%$ Resources:languageResource ,lang_reward %>" >
                    <ItemTemplate>
                            <%# Eval("ToolTipCN") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                            <button type="button" class="btn" onclick="golink('halfpagckage_Form.aspx','?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>&idx=<%#Eval("Package_ID") %>')"><asp:Label ID="Label1" Text="<%$ Resources:languageResource ,lang_btn_manage %>" runat="server" ></asp:Label></button>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
