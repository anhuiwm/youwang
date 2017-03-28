<%@ Page MasterPageFile="~/Main.Master" Language="C#" AutoEventWireup="true" CodeBehind="eventTimeForm.aspx.cs" Inherits="TheSoulGMTool.systemEvent.eventTimeForm" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="span9">
        <asp:HiddenField ID="idx" runat="server" />
        <table class="table table-bordered">
            <tbody>
                <tr>
                    <th><asp:Label ID="lang_server" Text="<%$ Resources:languageResource ,lang_server %>" runat="server" ></asp:Label> </th>
                    <td>
                        <span id="change_server" runat="server"></span>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="Label1" Text="<%$ Resources:languageResource ,lang_eventType %>" runat="server" ></asp:Label> </th>
                    <td>
                        <asp:Label ID="labeventType" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="Label2" Text="<%$ Resources:languageResource ,lang_eventTitle %>" runat="server" ></asp:Label> </th>
                    <td>
                        <asp:TextBox ID="eventTitle" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="Label3" Text="<%$ Resources:languageResource ,lang_startDay %>" runat="server" ></asp:Label> </th>
                    <td>
                        <asp:TextBox ID="startDay" TextMode="Date" Width="110" runat="server"></asp:TextBox>
                        <asp:DropDownList ID="startHour" Width="80" runat="server"></asp:DropDownList>
                        <asp:DropDownList ID="startMin" Width="80" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="Label4" Text="<%$ Resources:languageResource ,lang_endday %>" runat="server" ></asp:Label> </th>
                    <td>
                        <asp:TextBox ID="endDay" TextMode="Date" Width="110" runat="server"></asp:TextBox>
                        <asp:DropDownList ID="endHour" Width="80" runat="server"></asp:DropDownList>
                        <asp:DropDownList ID="endMin" Width="80" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lang_allReward" Text="<%$ Resources:languageResource ,lang_allReward %>" runat="server"></asp:Label></th>
                    <td colspan="4">
                        <table style="width: 100%;" class="table table-bordered">
                            <tr>
                                <th style="width:30%;"><asp:Label ID="lang_viplelve1" Text="<%$ Resources:languageResource ,lang_viplelve %>" runat="server"></asp:Label></th>
                                <th style="width:30%;"><asp:Label ID="lang_rewad1" Text="<%$ Resources:languageResource ,lang_reward %>" runat="server"></asp:Label></th>
                                <th style="width:20%;"><asp:Label ID="Label14" Text="<%$ Resources:languageResource ,lang_grade %>" runat="server"></asp:Label></th>
                                <th style="width:20%;"><asp:Label ID="lang_count" Text="<%$ Resources:languageResource ,lang_count %>" runat="server"></asp:Label></th>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList Width="100%" ID="all_vipLevel1" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList CssClass="rewardItem"  Width="100%" ID="all_reward1" onchange="checkRewardCount(this.id);" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td><asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="all_grade1" runat="server"></asp:TextBox></td>
                                <td><asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="all_rewardcnt1" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList Width="100%" ID="all_vipLevel2" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList CssClass="rewardItem" Width="100%" ID="all_reward2" onchange="checkRewardCount(this.id);" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td><asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="all_grade2" runat="server"></asp:TextBox></td>
                                <td>
                                    <asp:TextBox  Width="90%" TextMode="Number" CssClass="onlyNumber" ID="all_rewardcnt2" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList Width="100%" ID="all_vipLevel3" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList CssClass="rewardItem" Width="100%" ID="all_reward3" onchange="checkRewardCount(this.id);" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td><asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="all_grade3" runat="server"></asp:TextBox></td>
                                <td>
                                    <asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="all_rewardcnt3" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList Width="100%" ID="all_vipLevel4" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList CssClass="rewardItem" Width="100%" ID="all_reward4" onchange="checkRewardCount(this.id);" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td><asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="all_grade4" runat="server"></asp:TextBox></td>
                                <td>
                                    <asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="all_rewardcnt4" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList Width="100%" ID="all_vipLevel5" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList CssClass="rewardItem" Width="100%" ID="all_reward5" onchange="checkRewardCount(this.id);" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td><asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="all_grade5" runat="server"></asp:TextBox></td>
                                <td>
                                    <asp:TextBox  Width="90%" TextMode="Number" CssClass="onlyNumber" ID="all_rewardcnt5" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lang_pc1Reward" Text="<%$ Resources:languageResource ,lang_pc1Reward %>" runat="server"></asp:Label></th>
                    <td colspan="4">
                        <table class="table table-bordered">
                            <tr>
                                <th style="width:20%;"><asp:Label ID="lang_viplelve2" Text="<%$ Resources:languageResource ,lang_viplelve %>" runat="server"></asp:Label></th>
                                <th style="width:40%;"><asp:Label ID="lang_reward2" Text="<%$ Resources:languageResource ,lang_reward %>" runat="server"></asp:Label></th>
                                <th style="width:20%;"><asp:Label ID="lang_grade1" Text="<%$ Resources:languageResource ,lang_grade %>" runat="server"></asp:Label></th>
                                <th style="width:20%;"><asp:Label ID="lang_level1" Text="<%$ Resources:languageResource ,lang_level %>" runat="server"></asp:Label></th>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList Width="100%" ID="warrior_vipLevel1" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList CssClass="rewardItem" Width="100%" ID="warrior_reward1" onchange="checkRewardCount(this.id);" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="warrior_grade1" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="warrior_level1" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList Width="100%" ID="warrior_vipLevel2" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList CssClass="rewardItem" Width="100%" ID="warrior_reward2" onchange="checkRewardCount(this.id);" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="warrior_grade2" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="warrior_level2" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList Width="100%" ID="warrior_vipLevel3" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td><asp:DropDownList CssClass="rewardItem" Width="100%" ID="warrior_reward3" onchange="checkRewardCount(this.id);" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="warrior_grade3" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="warrior_level3" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList Width="100%" ID="warrior_vipLevel4" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList CssClass="rewardItem" Width="100%" ID="warrior_reward4" onchange="checkRewardCount(this.id);" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="warrior_grade4" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="warrior_level4" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList Width="100%" ID="warrior_vipLevel5" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList CssClass="rewardItem" Width="100%" ID="warrior_reward5" onchange="checkRewardCount(this.id);" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="warrior_grade5" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="warrior_level5" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lang_pc2Reward" Text="<%$ Resources:languageResource ,lang_pc2reward %>" runat="server"></asp:Label></th>
                    <td colspan="4">
                        <table class="table table-bordered">
                            <tr>
                                <th style="width:20%;"><asp:Label ID="Label6" Text="<%$ Resources:languageResource ,lang_viplelve %>" runat="server"></asp:Label></th>
                                <th style="width:40%;"><asp:Label ID="Label7" Text="<%$ Resources:languageResource ,lang_reward %>" runat="server"></asp:Label></th>
                                <th style="width:20%;"><asp:Label ID="Label9" Text="<%$ Resources:languageResource ,lang_grade %>" runat="server"></asp:Label></th>
                                <th style="width:20%;"><asp:Label ID="Label8" Text="<%$ Resources:languageResource ,lang_level %>" runat="server"></asp:Label></th>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList Width="100%" ID="sword_vipLevel1" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList CssClass="rewardItem" Width="100%" ID="sword_reward1" onchange="checkRewardCount(this.id);" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="sword_grade1" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="sword_level1" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList Width="100%" ID="sword_vipLevel2" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList CssClass="rewardItem" Width="100%" ID="sword_reward2" onchange="checkRewardCount(this.id);" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="sword_grade2" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="sword_level2" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList Width="100%" ID="sword_vipLevel3" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList CssClass="rewardItem" Width="100%" ID="sword_reward3" onchange="checkRewardCount(this.id);" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="sword_grade3" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="sword_level3" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList Width="100%" ID="sword_vipLevel4" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList CssClass="rewardItem" Width="100%" ID="sword_reward4" onchange="checkRewardCount(this.id);" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="sword_grade4" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="sword_level4" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList Width="100%" ID="sword_vipLevel5" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList CssClass="rewardItem" Width="100%" ID="sword_reward5" onchange="checkRewardCount(this.id);" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="sword_grade5" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="sword_level5" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lang_pc3Reward" Text="<%$ Resources:languageResource ,lang_pc3Reward %>" runat="server"></asp:Label></th>
                    <td colspan="4">
                        <table class="table table-bordered">
                            <tr>
                                <th style="width:20%;"><asp:Label ID="Label10" Text="<%$ Resources:languageResource ,lang_viplelve %>" runat="server"></asp:Label></th>
                                <th style="width:40%;"><asp:Label ID="Label11" Text="<%$ Resources:languageResource ,lang_reward %>" runat="server"></asp:Label></th>
                                <th style="width:20%;"><asp:Label ID="Label13" Text="<%$ Resources:languageResource ,lang_grade %>" runat="server"></asp:Label></th>
                                <th style="width:20%;"><asp:Label ID="Label12" Text="<%$ Resources:languageResource ,lang_level %>" runat="server"></asp:Label></th>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList Width="100%" ID="taoist_vipLevel1" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList CssClass="rewardItem" Width="100%" ID="taoist_reward1" onchange="checkRewardCount(this.id);" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="taoist_grade1" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="taoist_level1" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList Width="100%" ID="taoist_vipLevel2" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList CssClass="rewardItem" Width="100%" ID="taoist_reward2" onchange="checkRewardCount(this.id);" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="taoist_grade2" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="taoist_level2" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList Width="100%" ID="taoist_vipLevel3" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList CssClass="rewardItem" Width="100%" ID="taoist_reward3" onchange="checkRewardCount(this.id);" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="taoist_grade3" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="taoist_level3" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList Width="100%" ID="taoist_vipLevel4" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList CssClass="rewardItem"  Width="100%" ID="taoist_reward4" onchange="checkRewardCount(this.id);" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="taoist_grade4" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="taoist_level4" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList Width="100%" ID="taoist_vipLevel5" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList CssClass="rewardItem" Width="100%" ID="taoist_reward5" onchange="checkRewardCount(this.id);" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="taoist_grade5" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="taoist_level5" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
        <button class="btn" type="submit"><asp:Label ID="Label5" Text="<%$ Resources:languageResource ,lang_btn_update %>" runat="server" ></asp:Label></button>
</div>
</asp:Content>
