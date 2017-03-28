<%@ Page MasterPageFile="~/Main.Master" Language="C#" AutoEventWireup="true" CodeBehind="serverState.aspx.cs" Inherits="TheSoulGMTool.management.serverState" %>

<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="span9">
        <script type="text/javascript">
            var isFormSubmit;
            isFormSubmit = false;

            function formSend() {
                if (isFormSubmit) {
                    alert("<%=HttpContext.GetGlobalResourceObject("languageResource","lang_msgDoubleClick") %>");
                return false;
            }
            else {
                isFormSubmit = true;
                document.forms[0].submit();
            }
        }
        </script>
        <button type="button" class="btn" onclick="golink('serverStateForm.aspx','?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>')">
            <asp:Label ID="Label4" Text="<%$ Resources:languageResource ,lang_btn_manage %>" runat="server"></asp:Label></button>
        <asp:GridView ID="dataList" DataKeyNames="server_group_id" OnRowCommand="dataList_RowCommand" CssClass="table table-bordered" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="15" OnPageIndexChanging="dataList_PageIndexChanging">
            <Columns>
                <asp:BoundField DataField="server_group_name" HeaderText="Server" />
                <asp:TemplateField HeaderText="Server">
                    <ItemTemplate>
                        <%# Eval("server_group_name") %> (<%# Eval("user_account_idx") %>)
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources:languageResource ,lang_serverState %>">
                    <ItemTemplate>
                        <%#(TheSoul.DataManager.Global.Global_Define.eServerStatus)System.Convert.ToInt32(Eval("server_group_status"))%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources:languageResource ,lang_serverStateChange %>">
                    <ItemTemplate>
                        <asp:DropDownList ID="serverState" runat="server">
                            <asp:ListItem Text="Select" Value="-1"></asp:ListItem>
                            <asp:ListItem Text="Normal" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Hot" Value="11"></asp:ListItem>
                            <asp:ListItem Text="Recommand" Value="12"></asp:ListItem>
                            <asp:ListItem Text="New" Value="13"></asp:ListItem>
                            <asp:ListItem Text="Maintenance" Value="14"></asp:ListItem>
                            <asp:ListItem Text="Hide" Value="15"></asp:ListItem>
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:ButtonField ButtonType="Button" ControlStyle-CssClass="btn" Text="<%$ Resources:languageResource ,lang_btn_manage %>" CommandName="UpdateState" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
