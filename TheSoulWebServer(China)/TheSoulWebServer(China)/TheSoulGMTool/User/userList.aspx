<%@ Page MasterPageFile="~/Main.Master" Language="C#" AutoEventWireup="true"  CodeBehind="userList.aspx.cs" Inherits="TheSoulGMTool.userList" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function userdetail(aid) {
            location.href = "/User/userDetail.aspx?ca2=<%=Master.ca2%>&select_server=<%=Master.serverID%>&platformid=<%=platform.Text%>&username=<%=username.Text%>&slevel=<%=sLevel.SelectedValue%>&elevel=<%=eLevel.SelectedValue%>&sdate=<%=sDate.Text%>&edate=<%=eDate.Text%>&aid=" + aid;
        }
    </script>
    <div class="span9">
        
        <table class="table table-bordered">
            <tbody>
                <tr>
                    <th><asp:Label ID="Label1" Text="<%$ Resources:languageResource ,lang_searchType %>" runat="server" ></asp:Label></th>
                    <td><asp:Label ID="Label2" Text="<%$ Resources:languageResource ,lang_userNick %>" runat="server" ></asp:Label> :
                        <asp:TextBox ID="username" runat="server"></asp:TextBox><br />
                        <asp:Label ID="Label5" Text="<%$ Resources:languageResource ,lang_snailID %>" runat="server" ></asp:Label> :
                        <asp:TextBox ID="platform" runat="server"></asp:TextBox><br />
                        <asp:Label ID="Label3" Text="<%$ Resources:languageResource ,lang_level %>" runat="server" ></asp:Label> :
                        <asp:DropDownList ID="sLevel" runat="server">
                        </asp:DropDownList>
                        ~  
                            <asp:DropDownList ID="eLevel" runat="server">
                            </asp:DropDownList><br />
                        <asp:Label ID="Label4" Text="<%$ Resources:languageResource ,lang_creationdate %>" runat="server" ></asp:Label> :
                        <asp:TextBox TextMode="Date" ID="sDate" runat="server"></asp:TextBox>
                        ~
                        <asp:TextBox ID="eDate" TextMode="Date" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="Button1" CssClass="btn" Text="<%$ Resources:languageResource ,lang_btn_search %>" OnClientClick="submit();" runat="server" />
                        <asp:Button ID="Button2" CssClass="btn" Text="<%$ Resources:languageResource ,lang_excel %>" OnClick="Button2_Click" runat="server" />
                    </td>
                </tr>
            </tbody>
        </table>
        
        <asp:GridView ID="dataList" CssClass="table table-bordered" AutoGenerateColumns="false" AllowPaging="true" PageSize="15" OnPageIndexChanging="dataList_PageIndexChanging" runat="server">
            <Columns>
                <asp:BoundField DataField="UserName" HeaderText="<%$ Resources:languageResource ,lang_userNick %>" />
                <asp:BoundField DataField="AID" HeaderText="GameID(AID)" />
                <asp:BoundField DataField="SNO" HeaderText="UID" />
                <asp:BoundField DataField="VIPLevel" HeaderText="<%$ Resources:languageResource ,lang_viplelve %>" />
                <asp:TemplateField HeaderText="<%$ Resources:languageResource, lang_cashAndEventcash %>">
                    <ItemTemplate>
                        <%#Eval("Cash") %>/<%#Eval("EventCash") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources:languageResource ,lang_equipCharacter %>">
                    <ItemTemplate>
                        <%# (int)Eval("equipclass") > 0 ? TheSoul.DataManager.Character_Define.ClassEnumToType[(TheSoul.DataManager.Character_Define.SystemClassType)Eval("equipclass")]:"" %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="stageInfo" HeaderText="<%$ Resources:languageResource ,lang_mission %>" />
                <asp:BoundField DataField="CreationDate" HeaderText="<%$ Resources:languageResource ,lang_userCreationDate %>" />
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <button type="button" class="btn" onclick="userdetail(<%# Eval("AID")  %>)"><asp:Label ID="Label1" Text="<%$ Resources:languageResource ,lang_detail %>" runat="server" ></asp:Label></button>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
