<%@ Page Language="C#" MasterPageFile ="~/Main.Master" AutoEventWireup="true" CodeBehind="itemChargeLog.aspx.cs" Inherits="TheSoulGMTool.management.itemChargeLog" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="span9">
        <table class="table table-bordered">
            <tbody>
                <tr>
                    <th><asp:Label ID="Label1" Text="<%$ Resources:languageResource ,lang_searchDate %>" runat="server" ></asp:Label></th>
                    <td>
                        <asp:TextBox TextMode="Date" ID="sDate" runat="server"></asp:TextBox>
                        ~
                        <asp:TextBox TextMode="Date" ID="eDate" runat="server"></asp:TextBox>
                        <button type="submit" class="btn"><asp:Label ID="Label2" Text="<%$ Resources:languageResource ,lang_btn_search %>" runat="server" ></asp:Label></button>
                    </td>
                </tr>
            </tbody>
        </table>
        <asp:GridView ID="dataList" CssClass="table table-bordered" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="15" OnPageIndexChanging="dataList_PageIndexChanging">
            <Columns>                
                <asp:BoundField DataField="regdate" HeaderText="<%$ Resources:languageResource ,lang_controldate %>" /> 
                <asp:BoundField DataField="userNames" HeaderText="<%$ Resources:languageResource ,lang_userNick %>" /> 
                <asp:BoundField DataField="item_name_1" HeaderText="itemName1" /> 
                <asp:BoundField DataField="itemea_1" HeaderText="<%$ Resources:languageResource ,lang_count %>" /> 
                <asp:BoundField DataField="item_grade_1" HeaderText="<%$ Resources:languageResource ,lang_grade %>" /> 
                <asp:BoundField DataField="item_level_1" HeaderText="<%$ Resources:languageResource ,lang_level %>" /> 
                <asp:BoundField DataField="item_name_2" HeaderText="itemName2" /> 
                <asp:BoundField DataField="itemea_2" HeaderText="<%$ Resources:languageResource ,lang_count %>" /> 
                <asp:BoundField DataField="item_grade_2" HeaderText="<%$ Resources:languageResource ,lang_grade %>" /> 
                <asp:BoundField DataField="item_level_2" HeaderText="<%$ Resources:languageResource ,lang_level %>" /> 
                <asp:BoundField DataField="item_name_3" HeaderText="itemName3" /> 
                <asp:BoundField DataField="itemea_3" HeaderText="<%$ Resources:languageResource ,lang_count %>" /> 
                <asp:BoundField DataField="item_grade_3" HeaderText="<%$ Resources:languageResource ,lang_grade %>" /> 
                <asp:BoundField DataField="item_level_3" HeaderText="<%$ Resources:languageResource ,lang_level %>" /> 
                <asp:BoundField DataField="item_name_4" HeaderText="itemName4" /> 
                <asp:BoundField DataField="itemea_4" HeaderText="<%$ Resources:languageResource ,lang_count %>" /> 
                <asp:BoundField DataField="item_grade_4" HeaderText="<%$ Resources:languageResource ,lang_grade %>" /> 
                <asp:BoundField DataField="item_level_4" HeaderText="<%$ Resources:languageResource ,lang_level %>" /> 
                <asp:BoundField DataField="item_name_5" HeaderText="itemName5" /> 
                <asp:BoundField DataField="itemea_5" HeaderText="<%$ Resources:languageResource ,lang_count %>" /> 
                <asp:BoundField DataField="item_grade_5" HeaderText="<%$ Resources:languageResource ,lang_grade %>" /> 
                <asp:BoundField DataField="item_level_5" HeaderText="<%$ Resources:languageResource ,lang_level %>" /> 
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
