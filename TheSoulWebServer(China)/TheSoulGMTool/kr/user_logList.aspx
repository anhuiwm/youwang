<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="user_logList.aspx.cs" Inherits="TheSoulGMTool.kr.user_logList" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function logChage(idx) {
            $("#idx").val(idx);
            document.forms[0].submit();
        }
    </script>
    <div class="span9">
        <asp:HiddenField ID="idx" runat="server" />
        <button type="button" class="btn" onclick="golink('user_loglevel.aspx','?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>')"><asp:Label ID="Label4" Text="<%$ Resources:languageResource ,lang_userLogSetting %>" runat="server" ></asp:Label></button>
        <asp:GridView ID="dataList" CssClass="table table-bordered" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="aid" HeaderText="AID" />
                <asp:BoundField DataField="username" HeaderText="<%$ Resources:languageResource ,lang_userName %>" />
                <asp:BoundField DataField="log_level" HeaderText="<%$ Resources:languageResource ,lang_logLevel %>" />
                <%--<asp:BoundField DataField="log_level" HeaderText="<%$ Resources:languageResource ,lang_logCS %>" />--%>
                <asp:BoundField DataField="enddate" HeaderText="<%$ Resources:languageResource ,lang_enddate %>" />
                <asp:BoundField DataField="regdate" HeaderText="<%$ Resources:languageResource ,lang_regdate %>" />
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <button type="button" class="btn"  onclick="logChage(<%#Eval("aid") %>)"><asp:Label ID ="Label1" Text="해제" runat="server"></asp:Label></button>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
