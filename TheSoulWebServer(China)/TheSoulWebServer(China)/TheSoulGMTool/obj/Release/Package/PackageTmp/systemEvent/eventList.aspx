<%@ Page MasterPageFile="~/Main.Master" Language="C#" AutoEventWireup="true" CodeBehind="eventList.aspx.cs" Inherits="TheSoulGMTool.systemEvent.eventList" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function eventGroupEdite(eventType, Index) {
            location.href = "/systemEvent/eventGroupEdite.aspx?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>&eventtype=" + eventType + "&idx=" + Index;
        }

        function popup_reward(id) {
            var url = "/systemEvent/popReward.aspx?select_server=<%=Master.serverID%>&reward=" + id;
            window.open(url, "Window", "width=300, height=400, menubar=no,status=yes,scrollbars=no");
        }
    </script>
    <div class="span9">
        <table class="table table-bordered" style="width: 50%">
            <tr>
                <th><a href="eventGroupList.aspx?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>&group=<%=group %>"><%if (group == 1) { %><asp:Label ID ="Label2" Text="<%$ Resources:languageResource ,lang_giftGroup %>" runat="server"></asp:Label><% } else {%><asp:Label ID ="Label1" Text="<%$ Resources:languageResource ,lang_eventGroup %>" runat="server"></asp:Label><%} %></a></th>
                <th><a href="eventList.aspx?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>&group=<%=group %>"><%if (group == 1) { %><asp:Label ID ="Label3" Text="<%$ Resources:languageResource ,lang_gift %>" runat="server"></asp:Label><% } else {%><asp:Label ID ="Label4" Text="<%$ Resources:languageResource ,lang_event %>" runat="server"></asp:Label><%} %></a></th>
            </tr>
        </table>

        <table class="table table-bordered">
            <tr>
                <%if (group == 1) { %>
                <th><asp:Label ID ="Label5" Text="<%$ Resources:languageResource ,lang_giftGroup2 %>" runat="server"></asp:Label></th>
                <%}
                  else{
                %>
                <th><asp:Label ID ="Label9" Text="<%$ Resources:languageResource ,lang_eventgroup2 %>" runat="server"></asp:Label></th>
                <%} %>
                <td>
                    <asp:DropDownList ID="eventType" runat="server"></asp:DropDownList>
                    <button type="submit" class="btn"><asp:Label ID ="Label6" Text="<%$ Resources:languageResource ,lang_btn_search %>" runat="server"></asp:Label></button>
                </td>
            </tr>
        </table>
        <%if(!string.IsNullOrEmpty(eventType.SelectedValue)){ %>
        <button type="button" class="btn" onclick="golink('eventForm.aspx','?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>&group=<%=group %>&eventtype=<%=eventType.SelectedValue %>')"><asp:Label ID="Label8" Text="<%$ Resources:languageResource ,lang_btn_insert %>" runat="server" ></asp:Label></button>
        <%} %>
        <asp:GridView ID="groupList" CssClass="table table-bordered" runat="server" AutoGenerateColumns="False" AllowPaging="True" PageSize="15" OnPageIndexChanging="groupList_PageIndexChanging">
            <Columns>
                <asp:BoundField DataField="Event_ID" HeaderText="ID" />
                <asp:BoundField DataField="Event_Type" HeaderText="<%$ Resources:languageResource ,lang_eventType %>" />
                <asp:BoundField DataField="Event_Tooltip" HeaderText="<%$ Resources:languageResource ,lang_eventTitle %>" />
                <asp:BoundField DataField="Event_StartTime" HeaderText="<%$ Resources:languageResource ,lang_startdate %>" />
                <asp:BoundField DataField="Event_EndTime" HeaderText="<%$ Resources:languageResource ,lang_enddate %>" />
                <asp:BoundField DataField="Event_Loop" HeaderText="<%$ Resources:languageResource ,lang_loop_state %>" />
                <asp:BoundField DataField="Event_LoopType" HeaderText="<%$ Resources:languageResource ,lang_loop_type %>" />
                <asp:BoundField DataField="ActiveTriggerType1" HeaderText="<%$ Resources:languageResource ,lang_active_trigger%>" />
                <asp:BoundField DataField="ActiveTriggerType1_Value1" HeaderText="value1" />
                <asp:BoundField DataField="ActiveTriggerType1_Value2" HeaderText="value2" />
                <asp:BoundField DataField="ActiveTriggerType1_Value3" HeaderText="value3" />
                <asp:BoundField DataField="ActiveTriggerType2" HeaderText="<%$ Resources:languageResource ,lang_active_trigger %>" />
                <asp:BoundField DataField="ActiveTriggerType2_Value1" HeaderText="value1" />
                <asp:BoundField DataField="ActiveTriggerType2_Value2" HeaderText="value2" />
                <asp:BoundField DataField="ActiveTriggerType2_Value3" HeaderText="value3" />
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
                <asp:TemplateField HeaderText="<%$ Resources:languageResource ,lang_pc1Reward %>">
                    <ItemTemplate>
                        <%# (int)Eval("Reward_Box2ID") == 0 ? "" : "<button type=\"button\" class=\"btn\" onclick=\"popup_reward("+Eval("Reward_Box2ID")+")\">"+GetGlobalResourceObject("languageResource","lang_viewReward")+"</button>"%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources:languageResource ,lang_pc2Reward %>">
                    <ItemTemplate>
                        <%# (int)Eval("Reward_Box3ID") == 0 ? "" : "<button type=\"button\" class=\"btn\" onclick=\"popup_reward("+Eval("Reward_Box3ID")+")\">"+GetGlobalResourceObject("languageResource","lang_viewReward")+"</button>"%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources:languageResource ,lang_pc3Reward %>">
                    <ItemTemplate>
                        <%# (int)Eval("Reward_Box4ID") == 0 ? "" : "<button type=\"button\" class=\"btn\" onclick=\"popup_reward("+Eval("Reward_Box4ID")+")\">"+GetGlobalResourceObject("languageResource","lang_viewReward")+"</button>"%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <%if(group == 1){ %>
                        <button type="button" class="btn"  onclick="golink('eventTimeForm.aspx','?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>&eventtype=<%=eventType.SelectedValue %>&idx=<%#Eval("Event_ID") %>')"><asp:Label ID ="Label1" Text="<%$ Resources:languageResource ,lang_btn_update %>" runat="server"></asp:Label></button>
                        <%}
                          else {
                        %>
                        <button type="button" class="btn" onclick="golink('eventForm.aspx','?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>&eventtype=<%=eventType.SelectedValue %>&idx=<%#Eval("Event_ID") %>')"><asp:Label ID ="Label7" Text="<%$ Resources:languageResource ,lang_btn_update %>" runat="server"></asp:Label></button>
                        <%} %>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

    </div>
</asp:Content>
