<%@ Page MasterPageFile="~/Main.Master" Language="C#" AutoEventWireup="true" CodeBehind="serverList.aspx.cs" Inherits="TheSoulGMTool.management.serverList" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="span9">
        
        <button type="button" class="btn" onclick="golink('serverVersion.aspx','?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>')"><asp:Label ID="Label4" Text="<%$ Resources:languageResource ,lang_btn_add %>" runat="server" ></asp:Label></button>
        <asp:GridView ID="dataList" CssClass="table table-bordered" runat="server" OnRowCommand="dataList_RowCommand" DataKeyNames="server_group_id,billing_platform_type,target_version" AutoGenerateColumns="false" AllowPaging="true" PageSize="15" OnPageIndexChanging="dataList_PageIndexChanging">
            <Columns>
                <asp:BoundField DataField="server_group_name" HeaderText="Server" />
                <asp:TemplateField HeaderText="PlatformType">
                    <ItemTemplate>
                        <%#(TheSoul.DataManager.Shop_Define.eBillingType)System.Convert.ToInt32(Eval("billing_platform_type"))%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Version">
                    <ItemTemplate>
                        <%#String.Format("{0:000000000}", System.Convert.ToInt32(Eval("target_version"))).Insert(6,".").Insert(4,".").Insert(2,".")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:ButtonField ButtonType="Button" ControlStyle-CssClass="btn" Text="<%$ Resources:languageResource ,lang_btn_delete %>" CommandName="deleteVersion" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>