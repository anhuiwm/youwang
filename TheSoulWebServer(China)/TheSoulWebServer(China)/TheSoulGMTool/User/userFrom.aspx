<%@ Page MasterPageFile="~/Main.Master" Language="C#" AutoEventWireup="true" CodeBehind="userFrom.aspx.cs" Inherits="TheSoulGMTool.User.userFrom" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="span9">
        <asp:HiddenField ID="useridx" runat="server" />
        <table class="table table-bordered">
            <tbody>
                <tr>
                    <th>
                        <asp:Label ID="Label2" Text="<%$ Resources:languageResource ,lang_group %>" runat="server"></asp:Label></th>
                    <th>
                        <asp:Label ID="Label1" Text="<%$ Resources:languageResource ,lang_beforeData %>" runat="server"></asp:Label></th>
                    <th>
                        <asp:Label ID="Label3" Text="<%$ Resources:languageResource ,lang_changeData %>" runat="server"></asp:Label></th>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="Label4" Text="Tutorial On/Off" runat="server"></asp:Label></th>
                    <td>
                        <asp:Label ID="labTutorial" runat="server"></asp:Label></td>
                    <td>
                        <asp:RadioButtonList CssClass="table-unbordered" ID="tutorial" RepeatDirection="Horizontal" runat="server">
                            <asp:ListItem Value="0" Text="On"></asp:ListItem>
                            <asp:ListItem Value="1" Text="Off"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <th><%=HttpContext.GetGlobalResourceObject("languageResource", "lang_level")%></th>
                    <td>
                        <asp:Label ID="labPc1Level" runat="server"></asp:Label></td>
                    <td>
                        <asp:DropDownList ID="Pc1Level" runat="server"></asp:DropDownList></td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="Label17" Text="<%$ Resources:languageResource ,lang_viplelve %>" runat="server"></asp:Label></th>
                    <td>
                        <asp:Label ID="labVipLevel" runat="server"></asp:Label></td>
                    <td>
                        <asp:DropDownList ID="vipLevel" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th>VIP Point</th>
                    <td>
                        <asp:Label ID="labVipPoint" runat="server"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="vipPoint" TextMode="Number" CssClass="onlyNumber" runat="server"></asp:TextBox>/
                    </td>
                </tr>
                <tr>
                    <th><%=string.Format("{0}({1}/{2})",HttpContext.GetGlobalResourceObject("languageResource", "lang_ruby"),HttpContext.GetGlobalResourceObject("languageResource", "lang_charged"),HttpContext.GetGlobalResourceObject("languageResource", "lang_free"))%></th>
                    <td>
                        <asp:Label ID="labCash" runat="server"></asp:Label>/<asp:Label ID="labEventCash" runat="server"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="cash" TextMode="Number" CssClass="onlyNumber" runat="server"></asp:TextBox>/
                        <asp:TextBox ID="eventCash" TextMode="Number" CssClass="onlyNumber" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="Label5" Text="<%$ Resources:languageResource ,lang_gold %>" runat="server"></asp:Label></th>
                    <td>
                        <asp:Label ID="labGold" runat="server"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="gold" TextMode="Number" CssClass="onlyNumber" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="Label6" Text="<%$ Resources:languageResource ,lang_key %>" runat="server"></asp:Label></th>
                    <td>
                        <asp:Label ID="labKey" runat="server"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="key" TextMode="Number" CssClass="onlyNumber" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="Label7" Text="<%$ Resources:languageResource ,lang_ticket %>" runat="server"></asp:Label></th>
                    <td>
                        <asp:Label ID="labTicket" runat="server"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="ticket" TextMode="Number" CssClass="onlyNumber" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="Label8" Text="<%$ Resources:languageResource ,lang_Battlepoint %>" runat="server"></asp:Label></th>
                    <td>
                        <asp:Label ID="labBattlepoint" runat="server"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="Battlepint" TextMode="Number" CssClass="onlyNumber" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="Label9" Text="<%$ Resources:languageResource ,lang_Partypoint %>" runat="server"></asp:Label></th>
                    <td>
                        <asp:Label ID="labPartypoint" runat="server"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="Partypoint" TextMode="Number" CssClass="onlyNumber" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="Label10" Text="<%$ Resources:languageResource ,lang_Honorpoint %>" runat="server"></asp:Label></th>
                    <td>
                        <asp:Label ID="labHonorpoint" runat="server"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="Honorpoint" TextMode="Number" CssClass="onlyNumber" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="Label11" Text="<%$ Resources:languageResource ,lang_Donationpoint %>" runat="server"></asp:Label></th>
                    <td>
                        <asp:Label ID="labDonationpoint" runat="server"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="Donationpoint" TextMode="Number" CssClass="onlyNumber" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="Label12" Text="<%$ Resources:languageResource ,lang_Expeditionpoint %>" runat="server"></asp:Label></th>
                    <td>
                        <asp:Label ID="labExpeditionpoint" runat="server"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="Expeditionpoint" TextMode="Number" CssClass="onlyNumber" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="Label13" Text="<%$ Resources:languageResource ,lang_BlackMarketPoint %>" runat="server"></asp:Label></th>
                    <td>
                        <asp:Label ID="labBlackMarketPoint" runat="server"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="BlackMarketPoint" TextMode="Number" CssClass="onlyNumber" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="Label23" Text="<%$ Resources:languageResource ,lang_OverLoadPoint %>" runat="server"></asp:Label></th>
                    <td>
                        <asp:Label ID="labOverloadPoint" runat="server"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="OverloadPoint" TextMode="Number" CssClass="onlyNumber" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="Label15" Text="<%$ Resources:languageResource ,lang_soulStone %>" runat="server"></asp:Label></th>
                    <td>
                        <asp:Label ID="lab_stone" runat="server"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="stone" TextMode="Number" CssClass="onlyNumber" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="Label14" Text="<%$ Resources:languageResource ,lang_dungeonOpen %>" runat="server"></asp:Label></th>
                    <td>
                        <asp:Label ID="labMissionName" runat="server"></asp:Label>
                    </td>
                        
                    <td>
                        <asp:DropDownList ID="wordid" runat="server"></asp:DropDownList> - 
                        <asp:DropDownList ID="missionid" runat="server"></asp:DropDownList>
                    </td>
                </tr>
            </tbody>
        </table>
        <asp:Button CssClass="btn" Text="<%$ Resources:languageResource ,lang_btn_update %>" OnClientClick="subim();" runat="server" />
    </div>
</asp:Content>
