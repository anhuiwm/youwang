<%@ Page MasterPageFile="~/Main.Master" Language="C#" AutoEventWireup="true" CodeBehind="sevenDayEvent.aspx.cs" Inherits="TheSoulGMTool.systemEvent.sevenDayEvent" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function popup_reward(id) {
            var url = "/systemEvent/popReward.aspx?select_server=<%=Master.serverID%>&rewardType=3&reward=" + id;
            window.open(url, "Window", "width=300, height=400, menubar=no,status=yes,scrollbars=no");
        }
    </script>
    <div class="span9">
        <asp:GridView ID="groupList" CssClass="table table-bordered" runat="server" AutoGenerateColumns="False">
            <Columns>
                <asp:BoundField DataField="Event_Type" HeaderText="<%$ Resources:languageResource ,lang_eventType %>" />
                <asp:BoundField DataField="Event_Dev_Name" HeaderText="<%$ Resources:languageResource ,lang_eventTitle %>" />
                <asp:BoundField DataField="ClearTriggerType1" HeaderText="<%$ Resources:languageResource ,lang_clear_trigger %>" />
                <asp:BoundField DataField="ClearTriggerType1_Value1" HeaderText="value1" />
                <asp:BoundField DataField="ClearTriggerType1_Value2" HeaderText="value2" />
                <asp:BoundField DataField="ClearTriggerType1_Value3" HeaderText="value3" />
                <asp:BoundField DataField="ClearTriggerType2" HeaderText="<%$ Resources:languageResource ,lang_clear_trigger %>" />
                <asp:BoundField DataField="ClearTriggerType2_Value1" HeaderText="value1" />
                <asp:BoundField DataField="ClearTriggerType2_Value2" HeaderText="value2" />
                <asp:BoundField DataField="ClearTriggerType2_Value3" HeaderText="value3" />
                <asp:TemplateField HeaderText="<%$ Resources:languageResource ,lang_allReward %>">
                    <ItemTemplate>
                        <%# (int)Eval("Reward_Box1ID") == 0 ? "" : "<button type=\"button\" class=\"btn\" onclick=\"popup_reward("+Eval("Reward_Box1ID")+")\">"+GetGlobalResourceObject("languageResource","lang_viewReward")+"</button>"%>
                        
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <button type="button" class="btn" onclick="golink('sevenDayEvent_Form.aspx','?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>&idx=<%#Eval("Event_ID") %>')"><asp:Label ID ="Label7" Text="<%$ Resources:languageResource ,lang_btn_update %>" runat="server"></asp:Label></button>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

    </div>
</asp:Content>
