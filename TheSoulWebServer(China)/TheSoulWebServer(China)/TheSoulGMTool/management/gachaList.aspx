<%@ Page MasterPageFile="~/Main.Master" Language="C#" AutoEventWireup="true" CodeBehind="gachaList.aspx.cs" Inherits="TheSoulGMTool.management.gachaList" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <script type="text/javascript">
            function popup_gacha(id) {
                var url = "/management/popReward.aspx?select_server=<%=Master.serverID%>&reward=" + id;
            window.open(url, "Window", "width=300, height=400, menubar=no,status=yes,scrollbars=no");
        }
    </script>
    <div class="span9">
        <table class="table table-bordered">
            <tr>
                <th><asp:Label ID="lang_server" Text="<%$ Resources:languageResource ,lang_server %>" runat="server" ></asp:Label></th>
                <td><span id="change_server" runat="server"></span></td>
            </tr>
        </table>
        
        <table class="table table-bordered">
            <tbody>
                <tr>
                    <th style="width:25%"><asp:Label ID="Label6" Text="<%$ Resources:languageResource,lang_bestGacha %>" runat="server"></asp:Label> On/Off</th>
                    <td>
                        <asp:RadioButtonList CssClass="table-unbordered" RepeatDirection="Horizontal" ID="gachaonoff" runat="server">
                            <asp:ListItem Value="1" Text="On"></asp:ListItem>
                            <asp:ListItem Value="0" Text="Off"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td style="width:25%">
                        <button type="submit" class="btn"><asp:Label ID="Label7" Text="<%$ Resources:languageResource ,lang_btn_update %>" runat="server" ></asp:Label></button>
                    </td>
                </tr>
            </tbody>
        </table>
        <button type="button" class="btn" onclick="golink('gachaForm.aspx','?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>')"><asp:Label ID="Label4" Text="<%$ Resources:languageResource ,lang_btn_add %>" runat="server" ></asp:Label></button>
        <asp:GridView ID="dataList" CssClass="table table-bordered" runat="server" OnRowDataBound="dataList_RowDataBound" AutoGenerateColumns="false" AllowPaging="true" PageSize="15" OnPageIndexChanging="dataList_PageIndexChanging">
            <Columns>
                <asp:BoundField HeaderText="no." />
                <asp:BoundField DataField="StartDate" HeaderText="<%$ Resources:languageResource ,lang_startdate %>" />
                <asp:BoundField DataField="EndDate" HeaderText="<%$ Resources:languageResource ,lang_enddate %>" />
                <asp:TemplateField HeaderText="<%$ Resources:languageResource ,lang_reward %>">
                    <ItemTemplate>
                        <button type="button" class="btn" onclick="popup_gacha(<%#Eval("GachaIndex") %>)"><asp:Label ID="Label4" Text="<%$ Resources:languageResource ,lang_viewReward %>" runat="server" ></asp:Label></button>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources:languageResource ,lang_btn_manage %>">
                    <ItemTemplate>
                        <button type="button" class="btn" onclick="golink('gachaForm.aspx','?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>&idx=<%#Eval("GachaIndex") %>')"><asp:Label ID="Label4" Text="<%$ Resources:languageResource ,lang_btn_manage %>" runat="server" ></asp:Label></button>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
