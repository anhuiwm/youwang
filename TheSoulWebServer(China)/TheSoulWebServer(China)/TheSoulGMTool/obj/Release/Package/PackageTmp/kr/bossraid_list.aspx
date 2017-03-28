<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="bossraid_list.aspx.cs" Inherits="TheSoulGMTool.kr.bossraid_list" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="span9">
        <button type="button" class="btn" onclick="golink('bossraid_form.aspx','?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>')"><asp:Label ID="Label1" Text="<%$ Resources:languageResource ,lang_btn_insert %>" runat="server" ></asp:Label></button>
        <asp:GridView ID="dataList" CssClass="table table-bordered" DataKeyNames="BossRaidID" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="BossRaidID" HeaderText="EventID" />
                <asp:BoundField DataField="CreaterNick" HeaderText="Event" />
                <asp:BoundField DataField="DoReward" HeaderText="Boss" />
                <asp:BoundField DataField="PublicDate" HeaderText="Start Date" />
                <asp:BoundField DataField="ExpireDate" HeaderText="End Date" />
                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <%# TheSoul.DataManager.BossRaid_Define.BossRaidStatus.First(item=>item.Value == Eval("Status").ToString()).Key %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="CreationDate" HeaderText="Reg Date" />
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <asp:Button ID="stop" CssClass="btn" Text ="<%$ Resources:languageResource ,lang_stop %>" OnClick="stop_Click" runat="server" Visible='<%# Eval("Status").ToString() == "I" ?true:false%>' />
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