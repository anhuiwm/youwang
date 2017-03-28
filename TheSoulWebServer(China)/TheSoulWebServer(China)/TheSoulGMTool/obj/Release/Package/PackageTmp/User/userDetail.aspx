<%@ Page MasterPageFile="~/Main.Master" Language="C#" AutoEventWireup="true" CodeBehind="userDetail.aspx.cs" Inherits="TheSoulGMTool.User.userDetail" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="userIdx" runat="server" />
    <table class="table table-bordered">
        <tbody>
            <tr>
                <th>
                    <asp:Label ID="Label24" Text="Tutorial On/Off" runat="server"></asp:Label></th>
                <td>
                    <asp:Label ID="labTutorial" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="Label26" Text="VIP Level(Point)" runat="server"></asp:Label></th>
                <td>
                    <asp:Label ID="labVipInfo" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <th><%=string.Format("{0}({1}/{2})",HttpContext.GetGlobalResourceObject("languageResource", "lang_ruby"),HttpContext.GetGlobalResourceObject("languageResource", "lang_charged"),HttpContext.GetGlobalResourceObject("languageResource", "lang_free"))%></th>
                <td>
                    <asp:Label ID="labCash" runat="server"></asp:Label>/<asp:Label ID="labEventCash" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="Label2" Text="<%$ Resources:languageResource ,lang_gold %>" runat="server"></asp:Label></th>
                <td>
                    <asp:Label ID="labGold" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="Label3" Text="<%$ Resources:languageResource ,lang_key %>" runat="server"></asp:Label></th>
                <td>
                    <asp:Label ID="labKey" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="Label4" Text="<%$ Resources:languageResource ,lang_ticket %>" runat="server"></asp:Label></th>
                <td>
                    <asp:Label ID="labTicket" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="Label5" Text="<%$ Resources:languageResource ,lang_Battlepoint %>" runat="server"></asp:Label></th>
                <td>
                    <asp:Label ID="labBattlepoint" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="Label6" Text="<%$ Resources:languageResource ,lang_Partypoint %>" runat="server"></asp:Label></th>
                <td>
                    <asp:Label ID="labPartypoint" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="Label7" Text="<%$ Resources:languageResource ,lang_Honorpoint %>" runat="server"></asp:Label></th>
                <td>
                    <asp:Label ID="labHonorpoint" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="Label8" Text="<%$ Resources:languageResource ,lang_Donationpoint %>" runat="server"></asp:Label></th>
                <td>
                    <asp:Label ID="labDonationpoint" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="Label9" Text="<%$ Resources:languageResource ,lang_Expeditionpoint %>" runat="server"></asp:Label></th>
                <td>
                    <asp:Label ID="labExpeditionpoint" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="Label10" Text="<%$ Resources:languageResource ,lang_BlackMarketPoint %>" runat="server"></asp:Label></th>
                <td>
                    <asp:Label ID="labBlackMarketPoint" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="Label23" Text="<%$ Resources:languageResource ,lang_OverLoadPoint %>" runat="server"></asp:Label></th>
                <td>
                    <asp:Label ID="labOverloadPoint" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="Label28" Text="<%$ Resources:languageResource ,lang_soulStone %>" runat="server"></asp:Label></th>
                <td>
                    <asp:Label ID="lab_stone" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <th><%=HttpContext.GetGlobalResourceObject("languageResource","lang_guildName") %></th>
                <td>
                    <asp:Label ID="labGuildName" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="Label27" Text="<%$ Resources:languageResource ,lang_dungeonOpen %>" runat="server"></asp:Label></th>
                <td>
                    <asp:Label ID="labMissionName" runat="server"></asp:Label>
                </td>
            </tr>
            <% if (labPc1.Text != "0")
               {%>
            <tr>
                <th>
                    <asp:Label ID="Label11" Text="<%$ Resources:languageResource ,lang_pc1info %>" runat="server"></asp:Label></th>
                <td>
                    <asp:Label ID="labPc1" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="Label14" Text="<%$ Resources:languageResource ,lang_pc1stat %>" runat="server"></asp:Label></th>
                <td>
                    <asp:Label ID="labPc1Stat" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="Label16" Text="<%$ Resources:languageResource ,lang_equipitem %>" runat="server"></asp:Label></th>
                <td>
                    <asp:Label ID="labPc1Item" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="Label20" Text="<%$ Resources:languageResource ,lang_equipSoul %>" runat="server"></asp:Label></th>
                <td>
                    <asp:Label ID="labPc1Soul" runat="server"></asp:Label></td>
            </tr>
            <%} %>
            <% if (labPc2.Text != "0")
               {%>
            <tr>
                <th>
                    <asp:Label ID="Label12" Text="<%$ Resources:languageResource ,lang_pc2info %>" runat="server"></asp:Label></th>
                <td>
                    <asp:Label ID="labPc2" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="Label15" Text="<%$ Resources:languageResource ,lang_pc2stat %>" runat="server"></asp:Label></th>
                <td>
                    <asp:Label ID="labPc2Stat" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="Label18" Text="<%$ Resources:languageResource ,lang_equipitem %>" runat="server"></asp:Label></th>
                <td>
                    <asp:Label ID="labPc2Item" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="Label21" Text="<%$ Resources:languageResource ,lang_equipSoul %>" runat="server"></asp:Label></th>
                <td>
                    <asp:Label ID="labPc2Soul" runat="server"></asp:Label></td>
            </tr>
            <%} %>
            <% if (labPc3.Text != "0")
               {%>
            <tr>
                <th>
                    <asp:Label ID="Label13" Text="<%$ Resources:languageResource ,lang_pc3info %>" runat="server"></asp:Label></th>
                <td>
                    <asp:Label ID="labPc3" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="Label17" Text="<%$ Resources:languageResource ,lang_pc3stat %>" runat="server"></asp:Label></th>
                <td>
                    <asp:Label ID="labPc3Stat" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="Label19" Text="<%$ Resources:languageResource ,lang_equipitem %>" runat="server"></asp:Label></th>
                <td>
                    <asp:Label ID="labPc3Item" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="Label22" Text="<%$ Resources:languageResource ,lang_equipSoul %>" runat="server"></asp:Label></th>
                <td>
                    <asp:Label ID="labPc3Soul" runat="server"></asp:Label></td>
            </tr>
            <%} %>
        </tbody>
    </table>
    <br />
    <button class="btn" type="button" onclick="golink('userFrom.aspx','?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>&platformid=<%=Request.Params["platformid"]%>&username=<%=Request.Params["username"]%>&slevel=<%=Request.Params["slevel"]%>&elevel=<%=Request.Params["elevel"]%>&sdate=<%=Request.Params["sdate"]%>&edate=<%=Request.Params["edate"]%>&aid=<%=userIdx.Value %>')">
        <asp:Label ID="Label25" Text="<%$ Resources:languageResource ,lang_btn_update %>" runat="server"></asp:Label></button>
    <h4>Item List</h4>
    <table class="table-bordered" style="width:95%">
        <tr style="height:31px">
            <th style="width:14%">Item ID</th>
            <th style="width:16%">Item Name</th>
            <th style="width:14%"><%=HttpContext.GetGlobalResourceObject("languageResource", "lang_equipCharacter")%></th>
            <th style="width:14%"><%=HttpContext.GetGlobalResourceObject("languageResource", "lang_amount")%></th>
            <th style="width:14%">Item Grade</th>
            <th style="width:13%">Item Level</th>
            <th style="width:14%">Creation Date</th>
        </tr>
    </table>
    <div style="height: 187px; width: 95%; overflow: auto;">
        <asp:GridView ID="itemlist" Width="105%" CssClass="table table-bordered" ShowHeader="false" AutoGenerateColumns="false" runat="server">
            <Columns>
                <asp:BoundField ItemStyle-Width="14%" DataField="itemid" HeaderText="Item ID" />
                <asp:BoundField ItemStyle-Width="16%" DataField="delflag" HeaderText="Item Name" />
                <asp:BoundField ItemStyle-Width="14%" DataField="newyn" HeaderText="<%$ Resources:languageResource ,lang_equipCharacter %>" />
                <asp:BoundField ItemStyle-Width="14%" DataField="itemea" HeaderText="<%$ Resources:languageResource ,lang_amount %>" />
                <asp:BoundField ItemStyle-Width="14%" DataField="grade" HeaderText="Item Grade" />
                <asp:BoundField ItemStyle-Width="13%" DataField="level" HeaderText="Item Level" />
                <asp:BoundField ItemStyle-Width="13%" DataField="creation_date" HeaderText="Creation Date" />
            </Columns>
        </asp:GridView>
    </div>
    <h4>Ultimate Item List</h4>
    <table class="table-bordered" style="width:95%">
        <tr style="height:31px">
            <th style="width:17%">Ultimate ID</th>
            <th style="width:22%">Item Name</th>
            <th style="width:14%"><%=HttpContext.GetGlobalResourceObject("languageResource", "lang_equipCharacter")%></th>
            <th style="width:14%">Item Step</th>
            <th style="width:13%">Item Level</th>
            <th style="width:19%">Creation Date</th>
        </tr>
    </table>
    <div style="height: 187px; width: 95%; overflow: auto;">
        <asp:GridView ID="ultimatelist" Width="105%" CssClass="table table-bordered" ShowHeader="false" AutoGenerateColumns="false" runat="server">
            <Columns>
                <asp:BoundField ItemStyle-Width="16%" DataField="item_id" HeaderText="Item ID" />
                <asp:BoundField ItemStyle-Width="22%" DataField="equipflag" HeaderText="Item Name" />
                <asp:TemplateField ItemStyle-Width="14%" HeaderText="<%$ Resources:languageResource ,lang_equipCharacter %>">
                    <ItemTemplate>
                        <%# (byte)Eval("class_type") > 0 ? TheSoul.DataManager.Character_Define.ClassEnumToType[(TheSoul.DataManager.Character_Define.SystemClassType)((byte)Eval("class_type"))]:"" %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField ItemStyle-Width="13%" DataField="step" HeaderText="Item Step" />
                <asp:BoundField ItemStyle-Width="13%" DataField="level" HeaderText="Item Level" />
                <asp:BoundField ItemStyle-Width="18%" DataField="creation_date" HeaderText="Creation Date" />
            </Columns>
        </asp:GridView>
    </div>
    
    <h4>ActiveSoul List</h4>
    <table class="table-bordered" style="width:95%">
        <tr style="height:31px">
            <th style="width:14%">Soul ID</th>
            <th style="width:16%">Soul Name</th>
            <th style="width:14%"><%=HttpContext.GetGlobalResourceObject("languageResource", "lang_soulPartCount")%></th>
            <th style="width:14%">Soul Grade</th>
            <th style="width:14%">Soul Level</th>
            <th style="width:13%"><%=HttpContext.GetGlobalResourceObject("languageResource", "lang_starLevel")%></th>
            <th style="width:14%">Creation Date</th>
        </tr>
    </table>
    <div id="div_soullist1" style="height: 187px; width: 95%; overflow: auto;" runat="server">
        <asp:GridView ID="soullist1" Width="105%" CssClass="table table-bordered" ShowHeader="false" AutoGenerateColumns="false" runat="server">
            <Columns>
                <asp:BoundField ItemStyle-Width="14%" DataField="soulid" HeaderText="Soul ID" />
                <asp:BoundField ItemStyle-Width="16%" DataField="delflag" HeaderText="Soul Name" />
                <asp:BoundField ItemStyle-Width="14%" DataField="soulparts_ea" HeaderText="<%$ Resources:languageResource ,lang_soulPartCount %>" />
                <asp:BoundField ItemStyle-Width="14%" DataField="grade" HeaderText="Soul Grade" />
                <asp:BoundField ItemStyle-Width="14%" DataField="level" HeaderText="Soul Level" />
                <asp:BoundField ItemStyle-Width="13%" DataField="starlevel" HeaderText="<%$ Resources:languageResource ,lang_starLevel %>" />
                <asp:BoundField ItemStyle-Width="13%" DataField="creation_date" HeaderText="Creation Date" />
            </Columns>
        </asp:GridView>
    </div>
    <h4>PassiveSoul List</h4>
    <table class="table-bordered" style="width:95%">
        <tr style="height:31px">
            <th style="width:20%">Soul ID</th>
            <th style="width:20%">Soul Name</th>
            <th style="width:20%"><%=HttpContext.GetGlobalResourceObject("languageResource", "lang_equipCharacter")%></th>
            <th style="width:20%">Soul Level</th>
            <th style="width:20%">Creation Date</th>
        </tr>
    </table>
    <div style="height: 187px; width: 95%; overflow: auto;">
        <asp:GridView ID="soullist2" Width="105%" CssClass="table table-bordered" ShowHeader="false" AutoGenerateColumns="false" runat="server">
            <Columns>
                <asp:BoundField ItemStyle-Width="20%" DataField="soulid" HeaderText="Soul ID" />
                <asp:BoundField ItemStyle-Width="20%" DataField="delflag" HeaderText="Soul Name" />
                <asp:BoundField ItemStyle-Width="20%" DataField="stateflag" HeaderText="<%$ Resources:languageResource ,lang_equipCharacter %>" />
                <asp:BoundField ItemStyle-Width="20%" DataField="level" HeaderText="Soul Level" />
                <asp:BoundField ItemStyle-Width="18%" DataField="creation_date" HeaderText="Creation Date" />
            </Columns>
        </asp:GridView>
    </div>
    <button class="btn" type="button" onclick="golink('userFrom.aspx','?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>&platformid=<%=Request.Params["platformid"]%>&username=<%=Request.Params["username"]%>&slevel=<%=Request.Params["slevel"]%>&elevel=<%=Request.Params["elevel"]%>&sdate=<%=Request.Params["sdate"]%>&edate=<%=Request.Params["edate"]%>&aid=<%=userIdx.Value %>')">
        <asp:Label ID="Label1" Text="<%$ Resources:languageResource ,lang_btn_update %>" runat="server"></asp:Label></button>
</asp:Content>
