<%@ Page MasterPageFile="~/Main.Master" Language="C#" AutoEventWireup="true" CodeBehind="eventGroupList.aspx.cs" Inherits="TheSoulGMTool.systemEvent.eventGroupList" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function eventGroupEdite(eventType, Index) {
            location.href = "/systemEvent/eventGroupEdite.aspx?ca2=<%=Master.ca2%>&select_server=<%=Master.serverID%>&group=<%=group %>&eventtype=" + eventType + "&idx=" + Index;
        }
</script>
    <div class="span9">
        <table class="table table-bordered" style="width:50%">  
            <tr>
                <th><a href="eventGroupList.aspx?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>&group=<%=group %>"><%if (group == 1) { %><asp:Label ID ="Label2" Text="<%$ Resources:languageResource ,lang_giftGroup %>" runat="server"></asp:Label><% } else {%><asp:Label ID ="Label1" Text="<%$ Resources:languageResource ,lang_eventGroup %>" runat="server"></asp:Label><%} %></a></th>
                <th><a href="eventList.aspx?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>&group=<%=group %>"><%if (group == 1) { %><asp:Label ID ="Label3" Text="<%$ Resources:languageResource ,lang_gift %>" runat="server"></asp:Label><% } else {%><asp:Label ID ="Label4" Text="<%$ Resources:languageResource ,lang_event %>" runat="server"></asp:Label><%} %></a></th>
            </tr>
        </table>
        <button type="button" class="btn" onclick="golink('eventGroupEdite.aspx','?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>&group=<%=group %>')"><asp:Label ID="Label8" Text="<%$ Resources:languageResource ,lang_btn_insert %>" runat="server" ></asp:Label></button>
        <asp:GridView id="groupList" CssClass="table table-bordered" runat="server" AutoGenerateColumns="false">
            <Columns>                
                <asp:BoundField DataField="order_index" HeaderText="<%$ Resources:languageResource ,lang_sortNum %>" /> 
                <asp:BoundField DataField="Event_Type" HeaderText="<%$ Resources:languageResource ,lang_eventType %>" />
                <asp:BoundField DataField="Event_Title" HeaderText="<%$ Resources:languageResource ,lang_title %>" /> 
                <asp:BoundField DataField="ActiveState" HeaderText="<%$ Resources:languageResource ,lang_isActive %>" /> 
                <asp:TemplateField HeaderText="<%$ Resources:languageResource ,lang_btn_update %>">
                    <ItemTemplate>
                        <button type="button" class="btn" onclick="eventGroupEdite(<%# Eval("Event_Group_Type")  %>,<%# Eval("Event_Index")  %>);"><asp:Label ID="Label3" Text="<%$ Resources:languageResource ,lang_btn_update %>" runat="server" ></asp:Label></button>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
