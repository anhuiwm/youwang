<%@ Page MasterPageFile="~/Main.Master" Language="C#" AutoEventWireup="true" CodeBehind="shopBuyOnceOnOff.aspx.cs" Inherits="TheSoulGMTool.systemEvent.shopBuyOnceOnOff" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var isFormSubmit;
        isFormSubmit = false;

        function activeChange(active, item) {
            if (isFormSubmit) {
                alert("<%=HttpContext.GetGlobalResourceObject("languageResource","lang_msgDoubleClick") %>");
                return false;
            }
            else {
                isFormSubmit = true;
                $("input:hidden[id=itemid]").val(item);
                $("input:hidden[id=active]").val(active);
                document.forms[0].submit();
            }
            
        }
    </script>
    <div class="span9">
        <table class="table table-bordered">
            <tr>
                <th><asp:Label ID="Label1" Text="<%$ Resources:languageResource ,lang_server %>" runat="server" ></asp:Label></th>
                <td><span id="change_server" runat="server"></span></td>
            </tr>
        </table>
        <asp:HiddenField ID="itemid" runat="server" />
        <asp:HiddenField ID="active" runat="server" />
        <asp:GridView ID="dataList" CssClass="table table-bordered" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="15">
            <Columns>
                <asp:BoundField DataField="NameCN2" HeaderText="<%$ Resources:languageResource ,lang_ItemName %>" />
                <asp:BoundField DataField="Shop_Goods_ID" HeaderText="<%$ Resources:languageResource ,lang_ItemID %>" />
                <asp:TemplateField HeaderText="<%$ Resources:languageResource ,lang_isActive %>">
                    <ItemTemplate>
                        <%# System.Convert.ToInt32(Eval("SaleEndTime")) > 0 ? "O" : "X"%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="on/off">
                    <ItemTemplate>
                        <button type="button" class="btn" onclick="activeChange(<%# System.Convert.ToInt32(Eval("SaleEndTime")) > 0 ? "0" : "1"%>,<%#Eval("Shop_Goods_ID")%>)"><asp:Label ID="Label1" Text="<%$ Resources:languageResource ,lang_change %>" runat="server" ></asp:Label></button>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources:languageResource ,lang_buyInit %>">
                    <ItemTemplate>
                        <button type="button" class="btn" onclick="activeChange(-1,<%#Eval("Shop_Goods_ID")%>)"><asp:Label ID="Label1" Text="<%$ Resources:languageResource ,lang_init %>" runat="server" ></asp:Label></button>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
