<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="globalnotice_list.aspx.cs" Inherits="TheSoulGMTool.management.globalnotice_list" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="span9">
        <table class="table table-bordered">
            <tbody>
                <tr>
                    <th><asp:Label ID="Label2" Text="<%$ Resources:languageResource ,lang_searchType %>" runat="server" ></asp:Label></th>
                    <td>Active : <asp:RadioButtonList CssClass="table-unbordered" ID="notice_active" RepeatDirection="Horizontal" runat="server">
                                        <asp:ListItem Value="-1"  Selected="True">All</asp:ListItem>
                                        <asp:ListItem Value="0">X</asp:ListItem>
                                        <asp:ListItem Value="1">O</asp:ListItem>
                                    </asp:RadioButtonList><br />
                        PlatformType : <asp:CheckBoxList ID="platformType" CssClass="table-unbordered" RepeatDirection="Horizontal" runat="server"></asp:CheckBoxList><br />
                        Server Version : <asp:TextBox ID="Version1" Width="50" MaxLength="2" runat="server"></asp:TextBox>.
                                    <asp:TextBox ID="Version2" Width="50" MaxLength="2" runat="server"></asp:TextBox>.
                                    <asp:TextBox ID="Version3" Width="50" MaxLength="2" runat="server"></asp:TextBox>.
                                    <asp:TextBox ID="Version4" Width="50" MaxLength="3" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="Button1" CssClass="btn" Text="<%$ Resources:languageResource ,lang_btn_search %>" runat="server" />
                    </td>
                </tr>
            </tbody>
        </table>
        <button type="button" class="btn" onclick="golink('globalnotice_form.aspx','?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>')"><asp:Label ID="Label1" Text="<%$ Resources:languageResource ,lang_btn_insert %>" runat="server" ></asp:Label></button>
        <asp:GridView ID="dataList" CssClass="table table-bordered" runat="server"  AutoGenerateColumns="false" AllowPaging="true" PageSize="15" OnPageIndexChanging="dataList_PageIndexChanging">
            <Columns>
                <asp:BoundField DataField="orderNumber" HeaderText="<%$ Resources:languageResource ,lang_sortNum %>" />
                <asp:BoundField DataField="noticeTag" HeaderText="<%$ Resources:languageResource ,lang_noticeTag %>" />
                <asp:BoundField DataField="noticeStyle" HeaderText="<%$ Resources:languageResource ,lang_noticeType %>" />
                <asp:BoundField DataField="title" HeaderText="<%$ Resources:languageResource ,lang_title %>" /> 
                <asp:BoundField DataField="startDate" HeaderText="<%$ Resources:languageResource ,lang_startdate %>" /> 
                <asp:BoundField DataField="endDate" HeaderText="<%$ Resources:languageResource ,lang_enddate %>" />                 
                <asp:BoundField DataField="regid" HeaderText="<%$ Resources:languageResource ,lang_regGM %>" /> 
                <asp:BoundField DataField="regdate" HeaderText="<%$ Resources:languageResource ,lang_regdate %>" /> 
                <asp:BoundField DataField="editID" HeaderText="platformType" /> 
                <asp:TemplateField HeaderText="Version">
                    <ItemTemplate>                        
                        <%#String.Format("{0:000000000}", System.Convert.ToInt32(Eval("target_version"))).Insert(6,".").Insert(4,".").Insert(2,".")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>                        
                        <button class="btn" type="button" onclick="golink('globalnotice_form.aspx','?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>&idx=<%# Eval("idx") %>')"><asp:Label ID="Label1" Text="<%$ Resources:languageResource ,lang_btn_update %>" runat="server" ></asp:Label></button>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>