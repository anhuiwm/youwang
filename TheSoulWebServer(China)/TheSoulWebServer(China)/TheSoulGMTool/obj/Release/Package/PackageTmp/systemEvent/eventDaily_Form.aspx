<%@ Page MasterPageFile="~/Main.Master" Language="C#" AutoEventWireup="true" CodeBehind="eventDaily_Form.aspx.cs" Inherits="TheSoulGMTool.systemEvent.eventDaily_Form" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="span9">
        <table class="table table-bordered">
            <tr>
                <th><asp:Label ID="lang_server" Text="<%$ Resources:languageResource ,lang_server %>" runat="server" ></asp:Label></th>
                <td><span id="change_server" runat="server"></span></td>
            </tr>
        </table>
        <asp:HiddenField ID="eventIdx" runat="server" />
        <table class="table table-bordered">
            <tbody>
                <tr>
                    <th><asp:Label ID="Label10" Text="<%$ Resources:languageResource ,lang_eventName %>" runat="server" ></asp:Label></th>
                    <td><asp:Label ID="eventName" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <th><asp:Label ID="Label1" Text="<%$ Resources:languageResource ,lang_dailyEventType %>" runat="server" ></asp:Label></th>
                    <td><asp:Label ID="eventType" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <th><asp:Label ID="Label2" Text="<%$ Resources:languageResource ,lang_loop_state %>" runat="server" ></asp:Label></th>
                    <td>
                        <asp:Label ID="loopType" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="Label3" Text="<%$ Resources:languageResource ,lang_loop %>" runat="server" ></asp:Label></th>
                    <td>
                        <asp:Label ID="loop" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="Label4" Text="<%$ Resources:languageResource ,lang_dailyReward %>" runat="server" ></asp:Label></th>
                    <td>
                        <table class="table table-bordered">
                            <tr>
                                <th style="width:15%;"><asp:Label ID="Label5" Text="<%$ Resources:languageResource ,lang_viplelve %>" runat="server" ></asp:Label></th>
                                <th style="width:35%;"><asp:Label ID="Label6" Text="<%$ Resources:languageResource ,lang_reward %>" runat="server" ></asp:Label></th>
                                <th style="width:20%;"><asp:Label ID="Label9" Text="<%$ Resources:languageResource ,lang_grade %>" runat="server"></asp:Label></th>
                                <th style="width:30%;"><asp:Label ID="Label7" Text="<%$ Resources:languageResource ,lang_count %>" runat="server" ></asp:Label></th>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList Width="100%" ID="vipLevel1" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList Width="100%" ID="reward1" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td><asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="grade1" runat="server"></asp:TextBox></td>
                                <td>
                                    <asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="rewardcnt1" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList Width="100%" ID="vipLevel2" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList Width="100%" ID="reward2" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td><asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="grade2" runat="server"></asp:TextBox></td>
                                <td>
                                    <asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="rewardcnt2" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList Width="100%" ID="vipLevel3" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList Width="100%" ID="reward3" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td><asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="grade3" runat="server"></asp:TextBox></td>
                                <td>
                                    <asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="rewardcnt3" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList Width="100%" ID="vipLevel4" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList Width="100%" ID="reward4" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td><asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="grade4" runat="server"></asp:TextBox></td>
                                <td>
                                    <asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="rewardcnt4" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList Width="100%" ID="vipLevel5" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList Width="100%" ID="reward5" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td><asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="grade5" runat="server"></asp:TextBox></td>
                                <td>
                                    <asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="rewardcnt5" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
        <button type="submit" class="btn"><asp:Label ID="Label8" Text="<%$ Resources:languageResource ,lang_btn_update %>" runat="server" ></asp:Label></button>
    </div>
</asp:Content>