<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="push_list.aspx.cs" Inherits="WebPlatformTool.push.push_list" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function pushConfirmed(game, idx, status) {
            $("input:hidden[id=game]").val(game);
            $("input:hidden[id=idx]").val(idx);
            $("input:hidden[id=status]").val(status);
            document.forms[0].submit();
        }
    </script>
    <div class="span9">
        <asp:HiddenField ID="game" runat="server" />
        <asp:HiddenField ID="idx" runat="server" />
        <asp:HiddenField ID="status" runat="server" />
        <table style="border:0px none #ffffff; width:100%">
            <tr>
                <td colspan="2" style="padding:10px 15px">
                    <button type="button" class="btn" onclick="golink('/push/push_send.aspx')" ><%=GetGlobalResourceObject("StringResource", "lang_btn_register") %></button>
                </td>
            </tr>
            <tr>
                <td style="width:15%; border:1px solid #dddddd;padding:10px;">검색조건搜索条件</td>
                <td style="border:1px solid #dddddd; padding:10px;"><asp:TextBox ID="sdate" TextMode="Date" runat="server"></asp:TextBox> ~<asp:TextBox ID="edate" TextMode="Date" runat="server"></asp:TextBox> <br /><asp:DropDownList ID="game_id" runat="server"></asp:DropDownList>
                    <button type="submit" class="btn"><%=GetGlobalResourceObject("StringResource", "lang_btn_search") %></button></td>
            </tr>
            <tr>
                <td colspan="2" style="padding-top:10px;">
                    <asp:GridView ID="dataList" CssClass="table table-bordered" runat="server" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField ControlStyle-Width="11%" DataField="game_name" HeaderText="<%$ Resources:StringResource ,lang_gameName %>" />
                            <asp:BoundField ControlStyle-Width="11%" DataField="reg_date" HeaderText="<%$ Resources:StringResource ,lang_regDate %>" />
                            <asp:TemplateField ControlStyle-Width="10%" HeaderText="Type">
                                <ItemTemplate>
                                    <%# System.Convert.ToInt32(Eval("push_type")) == 0 ? "개발开发" : "배포部署" %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField ControlStyle-Width="11%" DataField="send_reserv_date" HeaderText="<%$ Resources:StringResource ,lang_sendTime %>" />
                            <asp:BoundField ControlStyle-Width="10%" DataField="title" HeaderText="<%$ Resources:StringResource ,lang_title %>" />
                            <asp:BoundField ControlStyle-Width="17%" DataField="message" HeaderText="<%$ Resources:StringResource ,lang_message %>" />
                            <asp:BoundField ControlStyle-Width="10%" DataField="register" HeaderText="<%$ Resources:StringResource ,lang_regGM %>" />
                            <asp:BoundField ControlStyle-Width="10%" DataField="strStatus" HeaderText="<%$ Resources:StringResource ,lang_status %>" />
                            <asp:BoundField ControlStyle-Width="10%" DataField="push_reason" HeaderText="<%$ Resources:StringResource ,lang_reason %>" />
                            <asp:TemplateField ControlStyle-Width="10%">
                                <ItemTemplate>
                                    <%# System.Convert.ToInt32(Eval("push_status")) == (int)mSeed.Common.ePushStatus.Unconfirmed ? "<button type=\"button\" class=\"btn\" onclick=\"pushConfirmed(" + Eval("game_service_id") + "," + Eval("push_id") + ", 2)\">" + GetGlobalResourceObject("StringResource", "lang_btn_ok") + "</button>" : ""%>
                                    <%# (System.Convert.ToInt32(Eval("push_status")) < (int)mSeed.Common.ePushStatus.Finish && System.Convert.ToInt32(Eval("push_status")) != (int)mSeed.Common.ePushStatus.Stop) ? "<button type=\"button\" class=\"btn\" onclick=\"pushConfirmed(" + Eval("game_service_id") + "," + Eval("push_id") + ", 0)\">" + GetGlobalResourceObject("StringResource", "lang_btn_stop") + "</button>" : ""%>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:DataList RepeatDirection="Horizontal" runat="server" ID="dlPager" OnItemCommand="dlPager_ItemCommand">
                        <ItemTemplate>
                            <asp:LinkButton Enabled='<%#Eval("Enabled") %>' runat="server" ID="lnkPageNo" Text='<%#Eval("Text") %>' CommandArgument='<%#Eval("Value") %>' CommandName="PageNo"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:DataList>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>