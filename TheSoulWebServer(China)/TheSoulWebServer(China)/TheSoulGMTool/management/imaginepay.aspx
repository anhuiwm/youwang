<%@ Page MasterPageFile="~/Main.Master" Language="C#" AutoEventWireup="true" CodeBehind="imaginepay.aspx.cs" Inherits="TheSoulGMTool.management.imaginepay" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var isFormSubmit;
        isFormSubmit = false;

        function shopSend(aid) {
            if (isFormSubmit) {
                alert("<%=HttpContext.GetGlobalResourceObject("languageResource","lang_msgDoubleClick") %>");
                return false;
            }
            else {
                isFormSubmit = true;
                var goodsID = $("#shop_" + aid).val();
                $("#userid").val(aid);
                $("#goodsID").val(goodsID);

                document.forms[0].submit();
            }
        }
    </script>
    <div class="span9">
        <asp:HiddenField ID="userid" runat="server" />
        <asp:HiddenField ID="goodsID" runat="server" />
        <table class="table table-bordered">
            <tbody>
                <tr>
                    <th><asp:Label ID="Label2" Text="<%$ Resources:languageResource ,lang_userNick %>" runat="server" ></asp:Label></th>
                    <td>
                        <asp:TextBox ID="username" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="Label5" Text="<%$ Resources:languageResource ,lang_snailID %>" runat="server" ></asp:Label></th>
                    <td><asp:TextBox id="uid" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="Button1" CssClass="btn" Text="<%$ Resources:languageResource ,lang_btn_search %>" OnClientClick="submit();" runat="server" />
                    </td>
                </tr>
            </tbody>
        </table>
        
        <asp:GridView ID="dataList" CssClass="table table-bordered" OnRowCreated="dataList_RowCreated" AutoGenerateColumns="false" AllowPaging="true" PageSize="15" OnPageIndexChanging="dataList_PageIndexChanging" DataKeyNames="AID" runat="server">
            <Columns>
                <asp:BoundField DataField="platform_idx" HeaderText="<%$ Resources:languageResource ,lang_snailIndex %>" />
                <asp:BoundField DataField="platform_user_id" HeaderText="<%$ Resources:languageResource ,lang_snailID %>" />
                <asp:BoundField DataField="UserName" HeaderText="<%$ Resources:languageResource ,lang_userNick %>" />
                <asp:TemplateField HeaderText="<%$ Resources:languageResource ,lang_ItemID %>" >
                    <ItemTemplate>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <button type="button" class="btn" onclick="shopSend(<%# Eval("AID")  %>)"><asp:Label ID="Label1" Text="<%$ Resources:languageResource ,lang_imaginepay %>" runat="server" ></asp:Label></button>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
