<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="mailLog.aspx.cs" Inherits="TheSoulGMTool.kr.mailLog" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function mailActive(idx) {
            $("#mailidx").val(idx);
            document.forms[0].submit();
        }
    </script>
    <div class="span9">
        <table class="table table-bordered">
            <tbody>
                <tr>
                    <th><asp:Label ID="Label1" Text="<%$ Resources:languageResource ,lang_searchType %>" runat="server" ></asp:Label></th>
                    <td><asp:Label ID="Label2" Text="<%$ Resources:languageResource ,lang_userNick %>" runat="server" ></asp:Label> :
                        <asp:TextBox ID="username" runat="server"></asp:TextBox><br />
                        <asp:Label ID="Label5" Text="<%$ Resources:languageResource ,lang_platfromID %>" runat="server" ></asp:Label> :
                        <asp:TextBox ID="uid" runat="server"></asp:TextBox><br />
                        <asp:Label ID="Label4" Text="Date" runat="server" ></asp:Label> :
                        <asp:TextBox TextMode="Date" ID="sDate" runat="server"></asp:TextBox>
                        ~
                        <asp:TextBox ID="eDate" TextMode="Date" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="Button1" CssClass="btn" Text="<%$ Resources:languageResource ,lang_btn_search %>" OnClick="Button1_Click" runat="server" />
                    </td>
                </tr>
            </tbody>
        </table>
        <asp:HiddenField ID="mailidx" runat="server" />
        <asp:GridView ID="dataList" CssClass="table table-bordered" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="mailseq" ItemStyle-Width="15%" HeaderText="idx" />
                <asp:BoundField DataField="reg_date" ItemStyle-Width="15%" HeaderText="<%$ Resources:languageResource ,lang_regdate %>" />
                <asp:BoundField DataField="closedate" ItemStyle-Width="15%" HeaderText="<%$ Resources:languageResource ,lang_expireDate %>" />
                <asp:BoundField DataField="title" ItemStyle-Width="23%" HeaderText="<%$ Resources:languageResource ,lang_title %>" />
                <asp:BoundField DataField="bodytext" ItemStyle-Width="19%" HtmlEncode="false" HeaderText="<%$ Resources:languageResource ,lang_Item %>" />
                <asp:BoundField DataField="readflag" ItemStyle-Width="8%" HeaderText="<%$ Resources:languageResource ,lang_readFlag %>" />
                <asp:BoundField DataField="delflag"  ItemStyle-Width="12%" HeaderText="<%$ Resources:languageResource ,lang_receiveFlag %>" />
                <asp:TemplateField HeaderText=""  ItemStyle-Width="8%">
                    <ItemTemplate>
                        <button type="button" class="btn" onclick="mailActive(<%#Eval("mailseq")%>)"><%#Eval("delflag").ToString() == "Y" ? GetGlobalResourceObject("languageResource","lang_restore") : GetGlobalResourceObject("languageResource","lang_btn_delete") %></button>
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
</asp:Content>
