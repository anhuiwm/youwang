<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="user_eventList.aspx.cs" Inherits="TheSoulGMTool.kr.user_eventList" %>
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
                        <asp:Label ID="Label4" Text="Date" runat="server" ></asp:Label> :
                        <asp:TextBox TextMode="Date" ID="sDate" runat="server"></asp:TextBox>
                        ~
                        <asp:TextBox ID="eDate" TextMode="Date" runat="server"></asp:TextBox><br />
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
                <asp:BoundField DataField="Event_Type" HeaderText="<%$ Resources:languageResource ,lang_eventType %>" />
                <asp:BoundField DataField="Event_Dev_Name" HeaderText="<%$ Resources:languageResource ,lang_eventTitle %>" />
                 <asp:TemplateField HeaderText="<%$ Resources:languageResource ,lang_eventPeriod %>" >
                    <ItemTemplate>
                        <%#Eval("Event_StartTime")%>~<%#Eval("Event_EndTime")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="StartTime" HeaderText="<%$ Resources:languageResource ,lang_startdate %>" />
                <asp:BoundField DataField="ClearFlag" HeaderText="<%$ Resources:languageResource ,lang_progression %>" />
                <asp:BoundField DataField="RewardFlag" HeaderText="<%$ Resources:languageResource ,lang_rewardFlag %>" />
                <asp:TemplateField HeaderText="" >
                    <ItemTemplate>
                        <button type="button" class="btn" onclick="golink('user_eventForm.aspx','?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>&username=<%=username.Text%>&uid=<%=uid.Text%>&sdate=<%=sDate.Text%>&edate=<%=eDate.Text%>&idx=<%#Eval("User_Event_ID") %>')"><asp:Label ID ="Label7" Text="<%$ Resources:languageResource ,lang_btn_update %>" runat="server"></asp:Label></button>
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
