<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="guildView.aspx.cs" Inherits="TheSoulGMTool.kr.guildView" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function chageCreatUser(idx) {
            $("#idx").val(idx);
            document.forms[0].submit();
        }
    </script>
    <div class="span9">
        <table class="table table-bordered">
            <tbody>
                <tr>
                    <th><asp:Label ID="guildName" runat="server"></asp:Label></th>
                </tr>
            </tbody>
        </table>
        <asp:HiddenField ID="idx" runat="server" />
        <asp:GridView ID="dataList" CssClass="table table-bordered" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="isJoinerAttend" HeaderText="<%$ Resources:languageResource ,lang_grade %>" />
                <asp:BoundField DataField="JoinerName" HeaderText="<%$ Resources:languageResource ,lang_userName %>" />
                <asp:BoundField DataField="Lastconntime" HeaderText="<%$ Resources:languageResource ,lang_connectDate %>" />
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <%# (long)Eval("joinerAID") > 0 ? "<button type=\"button\" onclick=\"chageCreatUser(" + Eval("joinerAID") + ")\" class=\"btn\">"+GetGlobalResourceObject("languageResource","lang_guildEntrust")+"</button>" : "" %>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>