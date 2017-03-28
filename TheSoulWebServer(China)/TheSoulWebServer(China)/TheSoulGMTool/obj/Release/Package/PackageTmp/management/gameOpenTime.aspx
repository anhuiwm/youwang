<%@ Page MasterPageFile="~/Main.Master" Language="C#" AutoEventWireup="true" CodeBehind="gameOpenTime.aspx.cs" Inherits="TheSoulGMTool.management.gameOpenTime" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <table class="table table-bordered">
        <tr>
            <th>
                <asp:Label ID="Label1" Text="<%$ Resources:languageResource ,lang_server %>" runat="server"></asp:Label></th>
            <td><span id="change_server" runat="server"></span></td>
        </tr>
    </table>
    <table class="table table-bordered">
        <tbody>
            <tr>
                <th>
                    <asp:Label ID="Label2" Text="<%$ Resources:languageResource ,lang_content_type %>" runat="server"></asp:Label></th>
                <th>
                    <asp:Label ID="Label3" Text="<%$ Resources:languageResource ,lang_isData %>" runat="server"></asp:Label></th>
                <th>
                    <asp:Label ID="Label4" Text="<%$ Resources:languageResource ,lang_changeData %>" runat="server"></asp:Label></th>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="Label5" Text="<%$ Resources:languageResource ,lang_freePVP %>" runat="server"></asp:Label>1st
                </th>
                <td>
                    <asp:Label ID="labPvPfree_1" runat="server"></asp:Label></td>
                <td>
                    <asp:DropDownList Width="80" ID="PvPfree_shour1" runat="server"></asp:DropDownList>
                    <asp:DropDownList Width="80" ID="PvPfree_smin1" runat="server"></asp:DropDownList>
                    ~
                    <asp:DropDownList Width="80" ID="PvPfree_ehour1" runat="server"></asp:DropDownList>
                    <asp:DropDownList Width="80" ID="PvPfree_emin1" runat="server"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="Label6" Text="<%$ Resources:languageResource ,lang_freePVP %>" runat="server"></asp:Label>2nd
                </th>
                <td>
                    <asp:Label ID="labPvPfree_2" runat="server"></asp:Label></td>
                <td>
                    <asp:DropDownList Width="80" ID="PvPfree_shour2" runat="server"></asp:DropDownList>
                    <asp:DropDownList Width="80" ID="PvPfree_smin2" runat="server"></asp:DropDownList>
                    ~
                    <asp:DropDownList Width="80" ID="PvPfree_ehour2" runat="server"></asp:DropDownList>
                    <asp:DropDownList Width="80" ID="PvPfree_emin2" runat="server"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="Label7" Text="<%$ Resources:languageResource ,lang1vs1OpenTiem %>" runat="server"></asp:Label>
                </th>
                <td>
                    <asp:Label ID="labPvP1vs1_1" runat="server"></asp:Label></td>
                <td>
                    <asp:DropDownList Width="80" ID="PvP1vs1_shour1" runat="server"></asp:DropDownList>
                    <asp:DropDownList Width="80" ID="PvP1vs1_smin1" runat="server"></asp:DropDownList>
                    ~
                    <asp:DropDownList Width="80" ID="PvP1vs1_ehour1" runat="server"></asp:DropDownList>
                    <asp:DropDownList Width="80" ID="PvP1vs1_emin1" runat="server"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="Label8" Text="<%$ Resources:languageResource ,lang1vs1BousTime %>" runat="server"></asp:Label>
                </th>
                <td>
                    <asp:Label ID="labPvP1vs1_2" runat="server"></asp:Label></td>
                <td>
                    <asp:DropDownList Width="80" ID="PvP1vs1_shour2" runat="server"></asp:DropDownList>
                    <asp:DropDownList Width="80" ID="PvP1vs1_smin2" runat="server"></asp:DropDownList>
                    ~
                    <asp:DropDownList Width="80" ID="PvP1vs1_ehour2" runat="server"></asp:DropDownList>
                    <asp:DropDownList Width="80" ID="PvP1vs1_emin2" runat="server"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="Label9" Text="<%$ Resources:languageResource ,lang_gladiator %>" runat="server"></asp:Label>1st
                </th>
                <td>
                    <asp:Label ID="labPvPruby_1" runat="server"></asp:Label></td>
                <td>
                    <asp:DropDownList Width="80" ID="PvPruby_shour1" runat="server"></asp:DropDownList>
                    <asp:DropDownList Width="80" ID="PvPruby_smin1" runat="server"></asp:DropDownList>
                    ~
                    <asp:DropDownList Width="80" ID="PvPruby_ehour1" runat="server"></asp:DropDownList>
                    <asp:DropDownList Width="80" ID="PvPruby_emin1" runat="server"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="Label10" Text="<%$ Resources:languageResource ,lang_gladiator %>" runat="server"></asp:Label>2nd
                </th>
                <td>
                    <asp:Label ID="labPvPruby_2" runat="server"></asp:Label></td>
                <td>
                    <asp:DropDownList Width="80" ID="PvPruby_shour2" runat="server"></asp:DropDownList>
                    <asp:DropDownList Width="80" ID="PvPruby_smin2" runat="server"></asp:DropDownList>
                    ~
                    <asp:DropDownList Width="80" ID="PvPruby_ehour2" runat="server"></asp:DropDownList>
                    <asp:DropDownList Width="80" ID="PvPruby_emin2" runat="server"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="Label11" Text="<%$ Resources:languageResource ,lang_guildWar %>" runat="server"></asp:Label>1st
                </th>
                <td>
                    <asp:Label ID="labPvPguild_1" runat="server"></asp:Label></td>
                <td>
                    <asp:DropDownList Width="80" ID="PvPguild_shour1" runat="server"></asp:DropDownList>
                    <asp:DropDownList Width="80" ID="PvPguild_smin1" runat="server"></asp:DropDownList>
                    ~
                    <asp:DropDownList Width="80" ID="PvPguild_ehour1" runat="server"></asp:DropDownList>
                    <asp:DropDownList Width="80" ID="PvPguild_emin1" runat="server"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="Label12" Text="<%$ Resources:languageResource ,lang_guildWar %>" runat="server"></asp:Label>2nd
                </th>
                <td>
                    <asp:Label ID="labPvPguild_2" runat="server"></asp:Label></td>
                <td>
                    <asp:DropDownList Width="80" ID="PvPguild_shour2" runat="server"></asp:DropDownList>
                    <asp:DropDownList Width="80" ID="PvPguild_smin2" runat="server"></asp:DropDownList>
                    ~
                    <asp:DropDownList Width="80" ID="PvPguild_ehour2" runat="server"></asp:DropDownList>
                    <asp:DropDownList Width="80" ID="PvPguild_emin2" runat="server"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="Label18" Text="<%$ Resources:languageResource ,lang_party %>" runat="server"></asp:Label>1st
                </th>
                <td>
                    <asp:Label ID="labPvpParty1" runat="server"></asp:Label></td>
                <td>
                    <asp:DropDownList Width="80" ID="PvPparty_shour1" runat="server"></asp:DropDownList>
                    <asp:DropDownList Width="80" ID="PvPparty_smin1" runat="server"></asp:DropDownList>
                    ~
                    <asp:DropDownList Width="80" ID="PvPparty_ehour1" runat="server"></asp:DropDownList>
                    <asp:DropDownList Width="80" ID="PvPparty_emin1" runat="server"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="Label22" Text="<%$ Resources:languageResource ,lang_party %>" runat="server"></asp:Label>2nd
                </th>
                <td>
                    <asp:Label ID="labPvpParty2" runat="server"></asp:Label></td>
                <td>
                    <asp:RadioButtonList ID="pvpPartyUse" CssClass="table-unbordered" RepeatDirection="Horizontal" runat="server">
                        <asp:ListItem Value="1" Text="<%$ Resources:languageResource ,lang_use %>"></asp:ListItem>
                        <asp:ListItem Value="0" Text="<%$ Resources:languageResource ,lang_notUse %>"></asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:DropDownList Width="80" ID="PvPparty_shour2" runat="server"></asp:DropDownList>
                    <asp:DropDownList Width="80" ID="PvPparty_smin2" runat="server"></asp:DropDownList>
                    ~
                    <asp:DropDownList Width="80" ID="PvPparty_ehour2" runat="server"></asp:DropDownList>
                    <asp:DropDownList Width="80" ID="PvPparty_emin2" runat="server"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="Label13" Text="<%$ Resources:languageResource ,lang_bossraid %>" runat="server"></asp:Label></th>
                <td>
                    <asp:Label ID="labBoss" runat="server"></asp:Label></td>
                <td>
                    <asp:TextBox TextMode="Number" CssClass="onlyNumber" ID="bossRate" runat="server"></asp:TextBox></td>
            </tr>
             <tr>
                <th>
                    <asp:Label ID="Label15" Text="<%$ Resources:languageResource ,lang_expeditionMacting %>" runat="server"></asp:Label></th>
                <td>
                    <asp:Label ID="labGold" runat="server"></asp:Label></td>
                <td>
                    <asp:TextBox TextMode="Number" CssClass="onlyNumber" ID="matching" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <th><asp:Label ID="Label16" Text="<%$ Resources:languageResource ,lang_shopNew %>" runat="server"></asp:Label></th>
                <td colspan="2">
                    <asp:RadioButtonList ID="shop_new" CssClass="table-unbordered" RepeatDirection="Horizontal" runat="server">
                        <asp:ListItem Value="1" Text="On"></asp:ListItem>
                        <asp:ListItem Value="0" Text="Off"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <th><asp:Label ID="Label17" Text="All Coupon On/Off" runat="server"></asp:Label></th>
                <td colspan="2">
                    <asp:RadioButtonList ID="coupon" CssClass="table-unbordered" RepeatDirection="Horizontal" runat="server">
                        <asp:ListItem Value="1" Text="On"></asp:ListItem>
                        <asp:ListItem Value="0" Text="Off"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <th><asp:Label ID="Label19" Text="IOS Coupon On/Off" runat="server"></asp:Label></th>
                <td colspan="2">
                    <asp:RadioButtonList ID="ios_coupon" CssClass="table-unbordered" RepeatDirection="Horizontal" runat="server">
                        <asp:ListItem Value="1" Text="On"></asp:ListItem>
                        <asp:ListItem Value="0" Text="Off"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <th><asp:Label ID="Label20" Text="SevenDay Event On/Off" runat="server"></asp:Label></th>
                <td colspan="2">
                    <asp:RadioButtonList ID="sevenday" CssClass="table-unbordered" RepeatDirection="Horizontal" runat="server">
                        <asp:ListItem Value="1" Text="On"></asp:ListItem>
                        <asp:ListItem Value="0" Text="Off"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <th><%=HttpContext.GetGlobalResourceObject("languageResource","lang_blackmarket") %><%=HttpContext.GetGlobalResourceObject("languageResource","lang_openDay") %></th>
                <td colspan="2">
                    <asp:CheckBoxList ID="blackOpenDay"  CssClass="table-unbordered" RepeatDirection="Horizontal" runat="server">
                        <asp:ListItem Value="1" Text="<%$ Resources:languageResource ,lang_monday %>"></asp:ListItem>
                        <asp:ListItem Value="2" Text="<%$ Resources:languageResource ,lang_tuesday %>"></asp:ListItem>
                        <asp:ListItem Value="4" Text="<%$ Resources:languageResource ,lang_wednesday %>"></asp:ListItem>
                        <asp:ListItem Value="8" Text="<%$ Resources:languageResource ,lang_thursday %>"></asp:ListItem>
                        <asp:ListItem Value="16" Text="<%$ Resources:languageResource ,lang_firday %>"></asp:ListItem>
                        <asp:ListItem Value="32" Text="<%$ Resources:languageResource ,lang_saturday %>"></asp:ListItem>
                        <asp:ListItem Value="64" Text="<%$ Resources:languageResource ,lang_sunday %>"></asp:ListItem>
                    </asp:CheckBoxList>
                </td>
            </tr>
            <tr>
                <th><%=HttpContext.GetGlobalResourceObject("languageResource","lang_blackmarket") %><%=HttpContext.GetGlobalResourceObject("languageResource","lang_openTime") %></th>
                <td>
                    <asp:Label ID="labBlackMarket" runat="server"></asp:Label></td>
                <td>
                    <asp:DropDownList Width="80" ID="black_shour" runat="server"></asp:DropDownList>
                    <asp:DropDownList Width="80" ID="black_smin" runat="server"></asp:DropDownList>
                    ~
                    <asp:DropDownList Width="80" ID="black_ehour" runat="server"></asp:DropDownList>
                    <asp:DropDownList Width="80" ID="black_emin" runat="server"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>EXP Hot Time Content</th>
                <td colspan="2">
                    <asp:CheckBoxList ID="hot_content"  CssClass="table-unbordered" RepeatDirection="Horizontal" RepeatColumns="4" runat="server">
                    </asp:CheckBoxList>
                </td>
            </tr>
            <tr>
                <th>EXP Hot Time</th>
                <td>
                    <asp:Label ID="labHotTime" runat="server"></asp:Label></td>
                <td>
                    <asp:DropDownList Width="80" ID="hot_shour" runat="server"></asp:DropDownList>
                    <asp:DropDownList Width="80" ID="hot_smin" runat="server"></asp:DropDownList>
                    ~
                    <asp:DropDownList Width="80" ID="hot_ehour" runat="server"></asp:DropDownList>
                    <asp:DropDownList Width="80" ID="hot_emin" runat="server"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>EXP Hot Time Rate</th>
                <td>
                    <asp:Label ID="labHotTimeRate" runat="server"></asp:Label></td>
                <td>
                    <asp:TextBox TextMode="Number" CssClass="onlyNumber" ID="hotTimeRate" runat="server"></asp:TextBox>
                </td>
            </tr>
        </tbody>
    </table>
    <button type="submit" class="btn"><asp:Label ID="Label14" Text="<%$ Resources:languageResource ,lang_btn_update %>" runat="server"></asp:Label></button>
</asp:Content>
