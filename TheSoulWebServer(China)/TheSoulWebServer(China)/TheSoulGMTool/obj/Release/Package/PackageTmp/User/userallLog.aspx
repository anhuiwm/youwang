<%@ Page MasterPageFile="~/Main.Master" Language="C#" AutoEventWireup="true" CodeBehind="userallLog.aspx.cs" Inherits="TheSoulGMTool.User.userallLog" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
    .classTD tbody tr td {
        white-space:normal;
        word-break: break-all;
    }
</style>
    <script type="text/javascript">
        $(document).keypress(function (e) {
            if (e.keyCode == 13)
                e.preventDefault();
        });
        function openPop(idx, tablename) {
            window.open('/User/pop_LogDecrypt.aspx?select_server=<%=Master.serverID%>&idx=' + idx + '&table=' + tablename, '_blank', 'width=600, height=500, toolbar=no, menubar=no, scrollbars=2, resizable=no, copyhistory=no');
        }
        function search() {
            var Mindate = $("#minDate").val();
            var datecnt;
            if ($("#sDate").length > 0) {
                datecnt = DateDiff(Mindate,$("#sDate").val());
            }
            else {
                datecnt = 0;
            }

            if (datecnt > 0) {
                alert("<%=GetGlobalResourceObject("languageResource","lang_msgSearch15Day")%>");
                return false;
            }
            if (isRunnig) {
                alert("<%=GetGlobalResourceObject("languageResource","lang_msgDoubleClick")%>");
                return false;
            }
            else {
                isRunnig = true;
                $("#pg").val(1);
                document.forms[0].submit();
            }
        }

    </script>
    <div class="span9">
        <asp:HiddenField ID="pg" runat="server" />
        <asp:HiddenField ID="minDate" runat="server" />
        <table class="table table-bordered">
            <tbody>
                <tr>
                    <th><asp:Label ID="Label1" Text="<%$ Resources:languageResource ,lang_searchType %>" runat="server" ></asp:Label></th>
                    <td><asp:Label ID="Label2" Text="<%$ Resources:languageResource ,lang_userNick %>" runat="server" ></asp:Label> :
                        <asp:TextBox ID="username" runat="server"></asp:TextBox><br />
                        <asp:Label ID="Label3" Text="<%$ Resources:languageResource ,lang_snailID %>" runat="server" ></asp:Label> :
                        <asp:TextBox ID="platform" runat="server"></asp:TextBox><br />
                        Operation :
                        <asp:DropDownList ID="op1" runat="server"></asp:DropDownList>
                        <asp:DropDownList ID="op2" runat="server"></asp:DropDownList>
                        <asp:DropDownList ID="op3" runat="server"></asp:DropDownList><br />
                        Reg Date :
                        <asp:TextBox TextMode="Date" ID="sDate" runat="server"></asp:TextBox>
                        ~
                        <asp:TextBox ID="eDate" TextMode="Date" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <button type="button" class="btn" onclick="search();"><asp:Label ID="Label4" Text="<%$ Resources:languageResource ,lang_btn_search %>" runat="server" ></asp:Label></button>
                        <asp:Button ID="Button2" CssClass="btn" Text="<%$ Resources:languageResource ,lang_excel %>" OnClick="Button2_Click" runat="server" />
                    </td>
                </tr>
            </tbody>
        </table>
        
        <asp:GridView ID="dataList" CssClass="table table-bordered" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField ControlStyle-Width="10%" DataField="regdate" HeaderText="regdate" />
                <asp:BoundField ControlStyle-Width="5%" DataField="CID" HeaderText="CID" />
                <asp:BoundField ControlStyle-Width="5%" DataField="ErrorCode" HeaderText="ErrorCode" />
                <asp:BoundField ControlStyle-Width="10%" DataField="RequestURL" HeaderText="RequestURL" />
                <asp:BoundField ControlStyle-Width="10%" DataField="Operation" HeaderText="Operation" />
                <asp:BoundField ControlStyle-Width="10%" DataField="RequestParams" HeaderText="RequestParams" />                
                <asp:BoundField ControlStyle-Width="15%" DataField="BaseJson" HeaderText="BaseJson" />
                <asp:TemplateField HeaderText="ResponseResult">
                    <ItemTemplate>
                        <a href="javascript:openPop(<%#Eval("log_idx") %>,'<%#Eval("tableName") %>')"><%#Eval("ResponseResult") %></a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField ControlStyle-Width="15%" DataField="DetailDBLog" HeaderText="DetailDBLog" />
                <asp:BoundField ControlStyle-Width="10%" DataField="requesttime" HeaderText="requesttime" />
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