<%@ Page MasterPageFile="~/Main.Master" Language="C#" AutoEventWireup="true" CodeBehind="blackmarketList.aspx.cs" Inherits="TheSoulGMTool.management.blackmarketList" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var isFormSubmit;
        isFormSubmit = false;
        function activeChange(item) {
            if (isFormSubmit) {
                alert("<%=HttpContext.GetGlobalResourceObject("languageResource","lang_msgDoubleClick") %>");
                return false;
            }
            else {
                isFormSubmit = true;
                $("input:hidden[id=goodsID]").val(item);
                document.forms[0].submit();
            }

        }
    </script>
    <div class="span9">
        <asp:HiddenField ID="slotID" runat="server" />
        <asp:HiddenField ID="goodsID" runat="server" />
        <table class="table table-bordered">
            <tr>
                <th><asp:Label ID="Label1" Text="<%$ Resources:languageResource ,lang_server %>" runat="server" ></asp:Label></th>
                <td><span id="change_server" runat="server"></span></td>
            </tr>
        </table>
        <asp:GridView ID="dataList" CssClass="table table-bordered" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="15" OnPageIndexChanging="dataList_PageIndexChanging">
            <Columns>
                <asp:BoundField DataField="ItemID" HeaderText="<%$ Resources:languageResource ,lang_sellItemID %>" />
                <asp:BoundField DataField="NameCN1" HeaderText="<%$ Resources:languageResource ,lang_sellIitemName %>" />
                <asp:BoundField DataField="ItemNum" HeaderText="<%$ Resources:languageResource ,lang_amount %>" />
                <asp:BoundField DataField="Buy_PriceValue" HeaderText="<%$ Resources:languageResource ,lang_sellItemPrice %>" />
                <asp:BoundField DataField="ItemProb" HeaderText="<%$ Resources:languageResource ,lang_itemProb %>" />
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <button type="button" class="btn" onclick="activeChange(<%#Eval("Shop_Goods_ID")%>)"><asp:Label ID="Label4" Text="<%$ Resources:languageResource ,lang_btn_delete %>" runat="server" ></asp:Label></button>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <table class="table-unbordered" style="width:100%">
            <tr>
                <td style="width:70%"></td>
                <td style="text-align:right;padding-right:10px;"><button type="button" class="btn" onclick="golink('blackMarket.aspx','?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>&slotid=<%=slotID.Value %>')"><asp:Label ID="Label4" Text="<%$ Resources:languageResource ,lang_btn_add %>" runat="server" ></asp:Label></button></td>
                <td><button type="button" class="btn" onclick="golink('blackMarketMain.aspx','?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>')"><asp:Label ID="Label2" Text="<%$ Resources:languageResource ,lang_list %>" runat="server" ></asp:Label></button></td>
            </tr>
        </table>
    </div>
</asp:Content>