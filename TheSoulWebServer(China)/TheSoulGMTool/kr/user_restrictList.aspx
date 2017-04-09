<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="user_restrictList.aspx.cs" Inherits="TheSoulGMTool.kr.user_restrictList" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function chageRestrict(idx, type) {
            $("#idx").val(idx);
            $("#restrictType").val(type);
            openLayer("reasonPopup", { top: 200 });
        }
        $(document).ready(function () {
            $("#reasonPopup").draggable();
        });
    </script>
    <div class="span9">
        <table class="table table-bordered" style="width: 50%">
            <tr>
                <th><a href="user_restrictList.aspx?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>"><%=GetGlobalResourceObject("languageResource", "lang_userLoginRestrict") %></a></th>
                <th><a href="user_restrrict_log.aspx?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>"><%=GetGlobalResourceObject("languageResource", "lang_userLoginRestrictLog") %></a></th>
            </tr>
        </table>
        <asp:HiddenField ID="idx" runat="server" />
        <asp:HiddenField ID="restrictType" runat="server" />
        <button type="button" class="btn" onclick="golink('user_restrict.aspx','?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>')"><asp:Label ID="Label4" Text="<%$ Resources:languageResource ,lang_userRestrict %>" runat="server" ></asp:Label></button>
        <asp:GridView ID="dataList" CssClass="table table-bordered" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="user_account_idx" HeaderText="AID" />
                <asp:BoundField DataField="login_restrict_enddate" HeaderText="<%$ Resources:languageResource ,lang_loginRestrict %>" />
                <asp:BoundField DataField="login_restrict_reg_date" HeaderText="<%$ Resources:languageResource ,lang_regdate %>" />
                <asp:BoundField DataField="chat_restrict_endate" HeaderText="<%$ Resources:languageResource ,lang_chatRestrict %>" />
                <asp:BoundField DataField="chat_restrict_reg_date" HeaderText="<%$ Resources:languageResource ,lang_regdate %>" />
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <%# System.Convert.ToInt32(Eval("loginActive")) > 0 ? "<button type=\"button\" onclick=\"chageRestrict(" + Eval("user_account_idx") + ", 11)\" class=\"btn\">"+GetGlobalResourceObject("languageResource", "lang_loginLift")+"</button>" : "" %>
                        <br />
                        <%# System.Convert.ToInt32(Eval("chatActive")) > 0 ? "<button type=\"button\" onclick=\"chageRestrict(" + Eval("user_account_idx") + ", 12)\" class=\"btn\">"+GetGlobalResourceObject("languageResource", "lang_chatLift")+"</button>" : "" %>
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
    <div id="reasonPopup" class="ui-widget-content ui-draggable" style="position: absolute; width: 50%; display: none;">
        <table border="0" style="background-color: #ffffff; width: 100%">
            <tr>
                <td>
                    <table class="table table-bordered">
                        <tr>
                            <td style="vertical-align: middle;">Memo :
                                <asp:TextBox ID="reason" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="Button1" CssClass="btn" Text="<%$ Resources:languageResource ,lang_btn_ok %>" runat="server" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
