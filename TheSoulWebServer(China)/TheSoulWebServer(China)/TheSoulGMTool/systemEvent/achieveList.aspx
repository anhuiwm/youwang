<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="achieveList.aspx.cs" Inherits="TheSoulGMTool.systemEvent.achieveList" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="span9">
        <asp:GridView ID="groupList" CssClass="table table-bordered" runat="server" AutoGenerateColumns="False" >
            <Columns>
                <asp:BoundField DataField="AchieveID" HeaderText="ID" />
                <asp:BoundField DataField="TaskCN" HeaderText="<%$ Resources:languageResource ,lang_eventTitle %>" />
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
