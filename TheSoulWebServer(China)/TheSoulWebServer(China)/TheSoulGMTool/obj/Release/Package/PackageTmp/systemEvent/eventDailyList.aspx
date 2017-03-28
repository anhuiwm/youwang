<%@ Page MasterPageFile="~/Main.Master" Language="C#" AutoEventWireup="true" CodeBehind="eventDailyList.aspx.cs" Inherits="TheSoulGMTool.systemEvent.eventDailyList" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var isFormSubmit;
        isFormSubmit = false;

        function popup_reward(id) {
            var url = "/systemEvent/popReward.aspx?select_server=<%=Master.serverID%>&reward=" + id;
            window.open(url, "Window", "width=300, height=400, menubar=no,status=yes,scrollbars=no");
        }

        function setConst() {
            if (isFormSubmit) {
                alert("<%=HttpContext.GetGlobalResourceObject("languageResource","lang_msgDoubleClick") %>");
                return false;
            }
            else {
                isFormSubmit = true;
                $("#mode").val("update");
                document.forms[0].submit();
            }
        }

        

    </script>

    <div class="span9">
        <table class="table table-bordered">
            <tr>
                <th><asp:Label ID="lang_server" Text="<%$ Resources:languageResource ,lang_server %>" runat="server" ></asp:Label></th>
                <td><span id="change_server" runat="server"></span></td>
            </tr>
        </table>
        <table class="table">
            <tr>
                <td style="border:none;">
                    <asp:HiddenField ID="mode" runat="server" />
                    <table class="table table-bordered">
                        <tbody>
                            <tr>
                                <th><asp:Label ID="Label1" Text="<%$ Resources:languageResource ,lang_content_type %>" runat="server" ></asp:Label></th>
                                <th><asp:Label ID="Label2" Text="<%$ Resources:languageResource ,lang_isData %>" runat="server" ></asp:Label></th>
                                <th><asp:Label ID="Label3" Text="<%$ Resources:languageResource ,lang_changeData %>" runat="server" ></asp:Label></th>
                            </tr>
                            <tr>
                                <th><asp:Label ID="Label4" Text="<%$ Resources:languageResource ,lang_dailyCount %>" runat="server" ></asp:Label></th>
                                <td>
                                    <asp:Label ID="labOverDay" runat="server"></asp:Label></td>
                                <td>
                                    <asp:TextBox TextMode="Number" CssClass="onlyNumber" ID="ovuerDay" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th><asp:Label ID="Label5" Text="<%$ Resources:languageResource ,lang_dailyNeedRuby %>" runat="server" ></asp:Label></th>
                                <td>
                                    <asp:Label ID="labOverRuby" runat="server"></asp:Label></td>
                                <td>
                                    <asp:TextBox TextMode="Number" CssClass="onlyNumber" ID="ovuerRuby" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th><asp:Label ID="Label6" Text="<%$ Resources:languageResource ,lang_dailyActive %>" runat="server" ></asp:Label></th>
                                <td>
                                    <asp:Label ID="labActive" runat="server"></asp:Label></td>
                                <td>
                                    <asp:RadioButtonList CssClass="table-unbordered" RepeatDirection="Horizontal" ID="dailyOnOff" runat="server">
                                        <asp:ListItem Value="1" Text="On"></asp:ListItem>
                                        <asp:ListItem Value="0" Text="Off"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="padding-right: 15px; text-align: right; border:none;">
                    <button type="button" onclick="setConst();" class="btn"><asp:Label ID="Label7" Text="<%$ Resources:languageResource ,lang_btn_update %>" runat="server" ></asp:Label></button></td>
            </tr>
        </table>

        <asp:GridView ID="dataList" CssClass="table table-bordered" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="15" OnPageIndexChanging="dataList_PageIndexChanging">
            <Columns>
                <asp:BoundField DataField="Event_DevName" HeaderText="<%$ Resources:languageResource ,lang_eventName %>" />
                <asp:BoundField DataField="Event_Daily_Type" HeaderText="<%$ Resources:languageResource ,lang_dailyEventType %>" />
                <asp:TemplateField HeaderText="<%$ Resources:languageResource ,lang_loop_state %>">
                    <ItemTemplate>
                        <%# System.Convert.ToInt32(Eval("Event_Loop")) > 0 ? "O" : "X"%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Reward_Mail_Subject_CN" HeaderText="<%$ Resources:languageResource ,lang_loop %>" />
                <asp:TemplateField HeaderText="<%$ Resources:languageResource ,lang_reward %>">
                    <ItemTemplate>
                        <button type="button" class="btn" onclick="popup_reward(<%#Eval("Reward_Box1ID") %>)"><asp:Label ID="Label4" Text="<%$ Resources:languageResource ,lang_viewReward %>" runat="server" ></asp:Label></button>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources:languageResource ,lang_btn_manage %>">
                    <ItemTemplate>
                        <button type="button" class="btn" onclick="golink('eventDaily_Form.aspx','?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>&idx=<%#Eval("Event_Daily_ID") %>')"><asp:Label ID="Label4" Text="<%$ Resources:languageResource ,lang_btn_manage %>" runat="server" ></asp:Label></button>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
