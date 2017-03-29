<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="coupon_veiw.aspx.cs" Inherits="WebPlatformTool.coupon.coupon_veiw" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="span9">
        <script type="text/javascript">
            
            function activeSumit() {
                $("#active").val(1);
                document.forms[0].submit();
            }
    </script>
        <asp:HiddenField ID="idx" runat="server" />
        <asp:HiddenField ID="pg" runat="server" />
        <asp:HiddenField ID="search" runat="server" />
        <asp:HiddenField ID="active" runat="server" />
        <table class="table table-bordered">
            <tr>
                <th><%=GetGlobalResourceObject("StringResource", "lang_makeType") %></th>
                <td colspan="3"><asp:Label ID="couponType" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <th><%=GetGlobalResourceObject("StringResource", "lang_eventName") %></th>
                <td colspan="3"><asp:Label ID="title" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <th><%=GetGlobalResourceObject("StringResource", "lang_couponCode") %></th>
                <td colspan="3">
                    <asp:Label ID="couponCode" runat="server"></asp:Label>
                    <asp:Button ID="excel" OnClick="excel_Click" CssClass="btn" runat="server" Text="<%$ Resources:StringResource ,lang_exportExcel %>" />
                </td>
            </tr>
            <tr>
                <th><%=GetGlobalResourceObject("StringResource", "lang_makeCount") %></th>
                <td><asp:Label ID="couponNum" runat="server"></asp:Label></td>
                <th><%=GetGlobalResourceObject("StringResource", "lang_useCouponCount") %></th>
                <td><asp:Label ID="couponUse" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <th><%=GetGlobalResourceObject("StringResource", "lang_rewardItem") %></th>
                <td colspan="3">
                    <table class="table table-bordered allreward" style="width:90%" id="allreward" runat="server">
                    </table>
                </td>
            </tr>
            <tr>
                <th><%=GetGlobalResourceObject("StringResource", "lang_memo") %></th>
                <td colspan="3"><asp:Label ID="memo" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <th><%=GetGlobalResourceObject("StringResource", "lang_regDate") %></th>
                <td><asp:Label ID="reg_date" runat="server"></asp:Label></td>
                <th><%=GetGlobalResourceObject("StringResource", "lang_regGM") %></th>
                <td><asp:Label ID="reg_id" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <th><%=GetGlobalResourceObject("StringResource", "lang_expirationDate") %></th>
                <td><asp:Label ID="coupon_active" runat="server"></asp:Label></td>
                <th><%=GetGlobalResourceObject("StringResource", "lang_couponStopDate") %></th>
                <td><asp:Label ID="discontinue_date" runat="server"></asp:Label></td>
            </tr>
        </table>
        <button type="button" onclick="activeSumit()" class="btn"><%=GetGlobalResourceObject("StringResource", "lang_btn_couponStop") %></button> <button type="button" onclick="golink('/coupon/coupon_list.aspx?pg=<%=pg.Value %>&search=<%=search.Value %>');" class="btn"><%=GetGlobalResourceObject("StringResource", "lang_btn_ok") %></button>
    </div>
</asp:Content>