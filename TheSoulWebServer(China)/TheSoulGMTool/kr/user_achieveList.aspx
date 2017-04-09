<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="user_achieveList.aspx.cs" Inherits="TheSoulGMTool.kr.user_achieveList" %>
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
                        <asp:Label ID="Label6" Text="Type" CssClass="table-unbordered" runat="server" ></asp:Label> :
                        <asp:RadioButtonList ID="achieveType" RepeatDirection="Horizontal" CssClass="table-unbordered" runat="server">
                            <asp:ListItem Text="<%$ Resources:languageResource ,lang_achieveType1 %>" Value="2" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:languageResource ,lang_achieveType2 %>" Value="5"></asp:ListItem>
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
                <asp:BoundField DataField="User_Event_ID" HeaderText="User Index" />
                <asp:BoundField DataField="Event_ID" HeaderText="<%$ Resources:languageResource ,lang_achievID %>" />
                <asp:BoundField DataField="Event_Dev_Name" HeaderText="<%$ Resources:languageResource ,lang_achieveName %>" />                
                <asp:BoundField DataField="StartTime" HeaderText="<%$ Resources:languageResource ,lang_startdate %>" />
                <asp:BoundField DataField="EndTime" HeaderText="<%$ Resources:languageResource ,lang_enddate %>" />
                 <asp:TemplateField HeaderText="<%$ Resources:languageResource ,lang_progressionType %>" >
                    <ItemTemplate>
                        <%#Eval("ClearTriggerType1_Value3")%> / <%#Eval("CurrentValue1")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ClearFlag" HeaderText="<%$ Resources:languageResource ,lang_progression %>" />
                <asp:BoundField DataField="RewardFlag" HeaderText="<%$ Resources:languageResource ,lang_rewardFlag %>" />
                <asp:TemplateField HeaderText="" >
                    <ItemTemplate>
                        <button type="button" class="btn" onclick="golink('user_achieve.aspx','?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>&username=<%=username.Text%>&uid=<%=uid.Text%>&achieveType=<%=achieveType.SelectedValue%>&idx=<%#Eval("User_Event_ID") %>')"><asp:Label ID ="Label7" Text="<%$ Resources:languageResource ,lang_btn_update %>" runat="server"></asp:Label></button>
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
