<%@ Page MasterPageFile="~/Main.Master" Language="C#" AutoEventWireup="true" CodeBehind="lineNoticeList.aspx.cs" Inherits="TheSoulGMTool.management.lineNoticeList" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="span9">
        <button type="button" class="btn" onclick="golink('lineNoticeForm.aspx','?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>')"><asp:Label ID="Label1" Text="<%$ Resources:languageResource ,lang_btn_insert %>" runat="server" ></asp:Label></button>
        <asp:GridView ID="dataList" CssClass="table table-bordered" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="15">
            <Columns>                
                <asp:BoundField DataField="sdate" HeaderText="<%$ Resources:languageResource ,lang_startdate %>" /> 
                <asp:BoundField DataField="edate" HeaderText="<%$ Resources:languageResource ,lang_enddate %>" /> 
                <asp:BoundField DataField="flrag" HeaderText="<%$ Resources:languageResource ,lang_state %>" /> 
                <asp:BoundField DataField="title" HeaderText="<%$ Resources:languageResource ,lang_contents %>" /> 
                <asp:BoundField DataField="regid" HeaderText="<%$ Resources:languageResource ,lang_regGM %>" /> 
                <asp:BoundField DataField="regdate" HeaderText="<%$ Resources:languageResource ,lang_regdate %>" /> 
                <asp:BoundField DataField="editeID" HeaderText="<%$ Resources:languageResource ,lang_editGM %>" /> 
                <asp:BoundField DataField="editeDate" HeaderText="<%$ Resources:languageResource ,lang_editDate %>" /> 
                <asp:TemplateField>
                    <ItemTemplate>
                        <button class="btn" type="button" onclick="golink('lineNoticeForm.aspx','?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>&idx=<%#Eval("idx") %>')"><asp:Label ID="Label1" Text="<%$ Resources:languageResource ,lang_btn_update %>" runat="server" ></asp:Label></button>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>