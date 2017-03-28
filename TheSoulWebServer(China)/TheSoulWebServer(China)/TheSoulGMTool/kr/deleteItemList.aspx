<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="deleteItemList.aspx.cs" Inherits="TheSoulGMTool.kr.deleteItemList" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function itemActive(idx,log,table) {
            $("#idx").val(idx);
            $("#log_idx").val(log);
            $("#table").val(table);
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
        <asp:HiddenField ID="idx" runat="server" />
        <asp:HiddenField ID="log_idx" runat="server" />
        <asp:HiddenField ID="table" runat="server" />
        <asp:HiddenField ID="pg" runat="server" />
        <asp:GridView ID="dataList" CssClass="table table-bordered" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="inven_seq" HeaderText="idx" />
                <asp:BoundField DataField="reg_date" HeaderText="<%$ Resources:languageResource ,lang_deleteDate %>" />
                <asp:BoundField DataField="equipposition" HeaderText="<%$ Resources:languageResource ,lang_character %>" />
                <asp:BoundField DataField="itemName" HeaderText="<%$ Resources:languageResource ,lang_Item %>" />
                <asp:BoundField DataField="grade" HeaderText="Grade" />
                <asp:BoundField DataField="level" HeaderText="Level" />
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <%# Eval("status").ToString() == "0" ? "<button type=\"button\" class=\"btn\" onclick=\"itemActive("+Eval("inven_seq")+", "+Eval("log_idx")+", '"+Eval("tableName")+"')\">"+GetGlobalResourceObject("languageResource","lang_restore")+"</button>" : "" %>
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
