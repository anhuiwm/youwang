<%@ Page MasterPageFile="~/Main.Master" Language="C#" AutoEventWireup="true" CodeBehind="mailboxNotice.aspx.cs" Inherits="TheSoulGMTool.management.mailboxNotice" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="span9">
        <asp:HiddenField ID="idx" runat="server" />
        <table class="table table-bordered">
            <tbody>
                <tr>
                    <th><asp:Label ID="Label3" Text="<%$ Resources:languageResource ,lang_server %>" runat="server" ></asp:Label></th>
                    <td><span id="change_server" runat="server"></span></td>
                </tr>
                <tr>
                    <th><asp:Label ID="lang_startDay" Text="<%$ Resources:languageResource ,lang_startDay %>" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="startDay" TextMode="Date" Width="150" runat="server"></asp:TextBox>
                        <asp:DropDownList ID="startHour" Width="80" runat="server"></asp:DropDownList>
                        <asp:DropDownList ID="startMin" Width="80" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lang_endday" Text="<%$ Resources:languageResource ,lang_endday %>" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="endDay" TextMode="Date" Width="150" runat="server"></asp:TextBox>
                        <asp:DropDownList ID="endHour" Width="80" runat="server"></asp:DropDownList>
                        <asp:DropDownList ID="endMin" Width="80" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="Label1" Text="<%$ Resources:languageResource ,lang_mailType %>" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="mailType" runat="server">
                            <asp:ListItem Value="0" Text="<%$ Resources:languageResource ,lang_mailType1 %>"></asp:ListItem>
                            <asp:ListItem Value="1" Text="<%$ Resources:languageResource ,lang_mailType2 %>"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><%=HttpContext.GetGlobalResourceObject("languageResource","lang_title") %></th>
                    <td><asp:TextBox ID="title" MaxLength="11" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <th><%=HttpContext.GetGlobalResourceObject("languageResource","lang_contents") %></th>
                    <td>
                        <asp:TextBox TextMode="MultiLine" ID="contents" runat="server"></asp:TextBox>
                        <span class="remaining">
                            <span class="count">0</span>/<span class="maxcount">2000</span>byte
                        </span>
                    </td>
                </tr>
                <tr>
                    <th><%=HttpContext.GetGlobalResourceObject("languageResource","lang_Item") %></th>
                    <td>
                        <table class="table table-bordered">
                            <tr>
                                <th style="width:35%;"><asp:Label ID="Label6" Text="<%$ Resources:languageResource ,lang_reward %>" runat="server" ></asp:Label></th>
                                <th style="width:20%;"><asp:Label ID="Label9" Text="<%$ Resources:languageResource ,lang_grade %>" runat="server"></asp:Label></th>
                                <th style="width:20%;"><asp:Label ID="Label2" Text="<%$ Resources:languageResource ,lang_level %>" runat="server"></asp:Label></th>
                                <th style="width:30%;"><asp:Label ID="Label7" Text="<%$ Resources:languageResource ,lang_count %>" runat="server" ></asp:Label></th>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList Width="100%" ID="reward1" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td><asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="grade1" runat="server"></asp:TextBox></td>
                                <td><asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="lelvel1" runat="server"></asp:TextBox></td>
                                <td>
                                    <asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="rewardcnt1" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList Width="100%" ID="reward2" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td><asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="grade2" runat="server"></asp:TextBox></td>
                                <td><asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="lelvel2" runat="server"></asp:TextBox></td>
                                <td>
                                    <asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="rewardcnt2" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList Width="100%" ID="reward3" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td><asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="grade3" runat="server"></asp:TextBox></td>
                                <td><asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="lelvel3" runat="server"></asp:TextBox></td>
                                <td>
                                    <asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="rewardcnt3" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList Width="100%" ID="reward4" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td><asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="grade4" runat="server"></asp:TextBox></td>
                                <td><asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="lelvel4" runat="server"></asp:TextBox></td>
                                <td>
                                    <asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="rewardcnt4" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList Width="100%" ID="reward5" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td><asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="grade5" runat="server"></asp:TextBox></td>
                                <td><asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="lelvel5" runat="server"></asp:TextBox></td>
                                <td>
                                    <asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="rewardcnt5" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
        <%if(idx.Value == "0") {%>
        <asp:Button ID="btn" CssClass="btn" Text="<%$ Resources:languageResource ,lang_btn_ok %>" OnClientClick="submit();" runat="server" />
        <%}else{ %>
        <asp:Button ID="Button1" CssClass="btn" Text="<%$ Resources:languageResource ,lang_btn_delete %>" OnClientClick="submit();" runat="server" />
        <%} %>
    </div>
</asp:Content>