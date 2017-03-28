<%@ Page MasterPageFile="~/Main.Master" Language="C#" AutoEventWireup="true" CodeBehind="requestLogList.aspx.cs" Inherits="TheSoulGMTool.User.requestLogList" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
    .classTD tbody tr td {
        white-space:normal;
        word-break: break-all;
    }
</style>
    <div class="span9">
        
        <table class="table table-bordered">
            <tbody>
                <tr>
                    <th><asp:Label ID="Label1" Text="<%$ Resources:languageResource ,lang_searchType %>" runat="server" ></asp:Label></th>
                    <td>AID : <asp:TextBox ID="AID" runat="server"></asp:TextBox><br />
                        Operation :<asp:TextBox ID="op" runat="server"></asp:TextBox><br />
                        ErrorCode :<asp:TextBox ID="errorCode" runat="server"></asp:TextBox><br />
                        Reg Date :
                        <asp:TextBox TextMode="Date" ID="sDate" runat="server"></asp:TextBox>
                        ~
                        <asp:TextBox ID="eDate" TextMode="Date" runat="server"></asp:TextBox><br />
                        <input type="checkbox" id="field1" value="CID" runat="server" /> CID <input type="checkbox" id="field2" value="RequestURL" runat="server" /> RequestURL 
                        <input type="checkbox" id="field3" value="RequestParams" runat="server" /> RequestParams <input type="checkbox" id="field4" value="ResponseResult" runat="server" /> ResponseResult 
                        <input type="checkbox" id="field5" value="BaseJson" runat="server" /> BaseJson <input type="checkbox" id="field6" value="DetailDBLog" runat="server" /> DetailDBLog 
                        <input type="checkbox" id="field7" value="requesttime" runat="server" /> requesttime <input type="checkbox" id="field8" value="regdate" runat="server" /> regdate 
                        <br />
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
        
        <asp:GridView ID="dataList" CssClass="table table-bordered classTD" AutoGenerateColumns="false" AllowPaging="true" PageSize="15" OnPageIndexChanging="dataList_PageIndexChanging" runat="server">
            <EmptyDataTemplate>

            </EmptyDataTemplate>
        </asp:GridView>
    </div>
</asp:Content>