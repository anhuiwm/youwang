<%@ Page MasterPageFile="~/Main.Master" Language="C#" AutoEventWireup="true" CodeBehind="member_edite.aspx.cs" Inherits="TheSoulGMTool.member.member_edite" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="span9">
        <div class="reply">
            <asp:HiddenField ID="idx" runat="server" />
            <table class="table table-bordered">
                <tbody>
                    <tr>
                        <th>ID</th>
                        <td><asp:TextBox ID="user_id" MaxLength="16" CssClass="span9" runat="server" ReadOnly="true"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th><%=HttpContext.GetGlobalResourceObject("languageResource","lang_password") %></th>
                        <td><asp:TextBox ID="userpw" TextMode="Password" MaxLength="16" CssClass="span9" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th><%=HttpContext.GetGlobalResourceObject("languageResource","lang_GMName") %></th>
                        <td><asp:TextBox ID="username" MaxLength="16" CssClass="span9" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th><%=HttpContext.GetGlobalResourceObject("languageResource","lang_Department") %></th>
                        <td><asp:TextBox ID="userpart" MaxLength="32" CssClass="span9" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th><%=HttpContext.GetGlobalResourceObject("languageResource","lang_Position") %></th>
                        <td><asp:TextBox ID="userrank" MaxLength="32" CssClass="span9" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th><%=HttpContext.GetGlobalResourceObject("languageResource","lang_phoneNumber") %></th>
                        <td><asp:TextBox ID="phone" MaxLength="16" CssClass="span9" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th><%=HttpContext.GetGlobalResourceObject("languageResource","lang_Email") %></th>
                        <td><asp:TextBox ID="usermail" CssClass="span9" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th><%=HttpContext.GetGlobalResourceObject("languageResource","lang_serverAuth") %></th>
                        <td><span id="serverAuth" runat="server"></span></td>
                    </tr>
                    <tr>
                        <th><%=HttpContext.GetGlobalResourceObject("languageResource","lang_auth") %></th>
                        <td><span id="authlist" runat="server"></span></td>
                    </tr>
                    <tr>
                        <th><%=HttpContext.GetGlobalResourceObject("languageResource","lang_GMUseCheck") %></th>
                        <td><asp:CheckBox ID="isusing" runat="server" /> (<%=HttpContext.GetGlobalResourceObject("languageResource","lang_gmUsing") %>)</td>
                    </tr>
                </tbody>
            </table>
            
            
            <div class="controls">
                <span class="span2"></span>
                <span class="span9" style="text-align: right; margin: 0;">
                    <asp:Button ID="Button1" Text="<%$ Resources:languageResource ,lang_btn_ok %>" OnClientClick="submit();" runat="server" />
                </span>
            </div>
        </div>
    </div>
</asp:Content>
