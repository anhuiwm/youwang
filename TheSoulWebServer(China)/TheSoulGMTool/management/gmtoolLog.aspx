<%@ Page MasterPageFile="~/Main.Master" Language="C#" AutoEventWireup="true" CodeBehind="gmtoolLog.aspx.cs" Inherits="TheSoulGMTool.management.gmtoolLog" %>
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
                <asp:BoundField DataField="server_id" HeaderText="<%$ Resources:languageResource ,lang_controlServer %>" /> 
                <asp:BoundField DataField="adminID" HeaderText="<%$ Resources:languageResource ,lang_Gm %>" /> 
                <asp:BoundField DataField="adminName" HeaderText="<%$ Resources:languageResource ,lang_GMName %>" /> 
                <asp:BoundField DataField="targetName" HeaderText="<%$ Resources:languageResource ,lang_controlID %>" /> 
                <asp:TemplateField HeaderText="<%$ Resources:languageResource ,lang_controlpattern %>">
                    <ItemTemplate>
                        <%# TheSoulGMTool.GMResult_Define.ControlType_List[(TheSoulGMTool.GMResult_Define.ControlType)Eval("controlType")] %>
                    </ItemTemplate>
                </asp:TemplateField> 
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>