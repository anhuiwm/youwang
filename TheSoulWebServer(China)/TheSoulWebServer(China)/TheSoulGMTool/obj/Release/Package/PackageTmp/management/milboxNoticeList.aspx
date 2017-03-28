<%@ Page MasterPageFile="~/Main.Master" Language="C#" AutoEventWireup="true" CodeBehind="milboxNoticeList.aspx.cs" Inherits="TheSoulGMTool.management.milboxNoticeList" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="span9">
        <button type="button" class="btn" onclick="golink('mailboxNotice.aspx','?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>')"><asp:Label ID="Label1" Text="<%$ Resources:languageResource ,lang_btn_insert %>" runat="server" ></asp:Label></button>
        <asp:GridView ID="dataList" CssClass="table table-bordered" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="15">
            <Columns>                
                <asp:BoundField DataField="startDate" HeaderText="<%$ Resources:languageResource ,lang_startdate %>" /> 
                <asp:BoundField DataField="endDate" HeaderText="<%$ Resources:languageResource ,lang_enddate %>" /> 
                <asp:TemplateField HeaderText="<%$ Resources:languageResource ,lang_mailType %>" >
                    <ItemTemplate>
                        <%# System.Convert.ToInt32(Eval("mailtype")) == 0 ? HttpContext.GetGlobalResourceObject("languageResource","lang_mailType1") : HttpContext.GetGlobalResourceObject("languageResource","lang_mailType2")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="title" HeaderText="<%$ Resources:languageResource ,lang_title %>" /> 
                <asp:BoundField DataField="regid" HeaderText="<%$ Resources:languageResource ,lang_regGM %>" /> 
                <asp:BoundField DataField="regdate" HeaderText="<%$ Resources:languageResource ,lang_regdate %>" /> 
                <asp:TemplateField>
                    <ItemTemplate>
                        <button class="btn" type="button" onclick="golink('mailboxNotice.aspx','?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>&idx=<%#Eval("idx") %>')"><asp:Label ID="Label2" Text="<%$ Resources:languageResource ,lang_btn_manage %>" runat="server" ></asp:Label></button>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>