<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="coupon_make.aspx.cs" Inherits="OperationTool.coupon_make" %>

<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="span9">
        <table class="table table-bordered">
            <tbody>
                <tr>
                    <th style="width: 100px;">
                        <asp:Label ID="Label10" Text="<%$ Resources:StringResource ,lang_makeType %>" runat="server"></asp:Label></th>
                    <td>
                        <asp:RadioButtonList ID="couponType" CssClass="table-unbordered" RepeatDirection="Horizontal" runat="server">
                            <asp:ListItem Value="1" Text="<%$ Resources:StringResource ,lang_couponType1 %>"></asp:ListItem>
                            <asp:ListItem Value="2" Text="<%$ Resources:StringResource ,lang_couponType2 %>"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <th style="">
                        <asp:Label ID="Label1" Text="<%$ Resources:StringResource ,lang_eventName %>" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="eventName" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="Label2" Text="<%$ Resources:StringResource ,lang_expirationDate %>" runat="server"></asp:Label></th>
                    <td>
                        <asp:RadioButtonList ID="checkDate" RepeatDirection="Horizontal" runat="server">
                            <asp:ListItem Value="1" Text="<%$ Resources:StringResource ,lang_unlimited %>"></asp:ListItem>
                            <asp:ListItem Value="2" Text="<%$ Resources:StringResource ,lang_inputDate %>"></asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:TextBox ID="startDay" TextMode="Date" Width="150" runat="server"></asp:TextBox>
                        <asp:TextBox ID="endDay" TextMode="Date" Width="150" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="Label3" Text="<%$ Resources:StringResource ,lang_makeCount %>" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="couponCount" TextMode="Number" CssClass="onlyNumber" Width="110" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="Label6" Text="<%$ Resources:StringResource ,lang_couponCode %>" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="couponCode" Width="110" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="Label12" Text="<%$ Resources:StringResource ,lang_allItem %>" runat="server"></asp:Label></th>
                    <td>
                        <button type="button" class="btn" onclick="tableAdd('allitem')">
                            <asp:Label ID="Label20" Text="<%$ Resources:StringResource ,lang_btn_add %>" runat="server"></asp:Label></button>
                        <table id="allitem" class="table table-bordered" runat="server">
                            <colgroup>
                                <col width="40%" />
                                <col width="30%" />
                                <col width="30%" />
                            </colgroup>
                            <tr>
                                <th>
                                    <asp:Label ID="Label13" Text="<%$ Resources:StringResource ,lang_Item %>" runat="server"></asp:Label></th>
                                <th>
                                    <asp:Label ID="Label14" Text="<%$ Resources:StringResource ,lang_grade %>" runat="server"></asp:Label></th>
                                <th>
                                    <asp:Label ID="Label32" Text="<%$ Resources:StringResource ,lang_amount %>" runat="server"></asp:Label></th>
                            </tr>
                            <tr>
                                <td>
                                    <select name="all_item" id="all_item" runat="server"></select>
                                </td>
                                <td>
                                    <input type="text" name="all_itemgrade" id="all_itemgrade" runat="server" />
                                </td>
                                <td>
                                    <input type="text" name="all_itemcnt" id="all_itemcnt" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="Label15" Text="<%$ Resources:StringResource ,lang_pc1Item %>" runat="server"></asp:Label></th>
                    <td>
                        <button type="button" class="btn" onclick="tableAdd('warrior_itemtable')">
                            <asp:Label ID="Label19" Text="<%$ Resources:StringResource ,lang_btn_add %>" runat="server"></asp:Label></button>
                        <table id="warrior_itemtable" class="table table-bordered" runat="server">
                            <tr>
                                <th>
                                    <asp:Label ID="Label16" Text="<%$ Resources:StringResource ,lang_Item %>" runat="server"></asp:Label></th>
                                <th>
                                    <asp:Label ID="Label18" Text="<%$ Resources:StringResource ,lang_grade %>" runat="server"></asp:Label></th>
                                <th>
                                    <asp:Label ID="Label17" Text="<%$ Resources:StringResource ,lang_level %>" runat="server"></asp:Label></th>
                            </tr>
                            <tr>

                                <td>
                                    <select name="warrior_item" id="warrior_item" runat="server"></select></td>
                                <td>
                                    <input type="text" name="warriorgrade" id="warriorgrade" size="50" runat="server" /></td>
                                <td>
                                    <input type="text" name="warriorlevel" id="warriorlevel" size="50" runat="server" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="Label21" Text="<%$ Resources:StringResource ,lang_pc2Item %>" runat="server"></asp:Label></th>
                    <td>
                        <button type="button" class="btn" onclick="tableAdd('sword_itemtable')">
                            <asp:Label ID="Label22" Text="<%$ Resources:StringResource ,lang_btn_add %>" runat="server"></asp:Label></button>
                        <table id="sword_itemtable" class="table table-bordered" runat="server">
                            <tr>
                                <th>
                                    <asp:Label ID="Label23" Text="<%$ Resources:StringResource ,lang_Item %>" runat="server"></asp:Label></th>
                                <th>
                                    <asp:Label ID="Label25" Text="<%$ Resources:StringResource ,lang_grade %>" runat="server"></asp:Label></th>
                                <th>
                                    <asp:Label ID="Label24" Text="<%$ Resources:StringResource ,lang_level %>" runat="server"></asp:Label></th>
                            </tr>
                            <tr>
                                <td>
                                    <select name="sword_item" id="sword_item" runat="server"></select></td>
                                <td>
                                    <input type="text" name="swordgrade" id="swordgrade" size="50" runat="server" /></td>
                                <td>
                                    <input type="text" name="swordlevel" id="swordlevel" size="50" runat="server" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="Label26" Text="<%$ Resources:StringResource ,lang_pc3Item %>" runat="server"></asp:Label></th>
                    <td>
                        <button type="button" class="btn" onclick="tableAdd('taoist_itemtable')">
                            <asp:Label ID="Label28" Text="<%$ Resources:StringResource ,lang_btn_add %>" runat="server"></asp:Label></button>
                        <table id="taoist_itemtable" class="table table-bordered" runat="server">
                            <tr>
                                <th>
                                    <asp:Label ID="Label27" Text="<%$ Resources:StringResource ,lang_Item %>" runat="server"></asp:Label></th>
                                <th>
                                    <asp:Label ID="Label30" Text="<%$ Resources:StringResource ,lang_grade %>" runat="server"></asp:Label></th>
                                <th>
                                    <asp:Label ID="Label29" Text="<%$ Resources:StringResource ,lang_level %>" runat="server"></asp:Label></th>
                            </tr>
                            <tr>
                                <td>
                                    <select name="taoist_item" id="taoist_item" runat="server"></select></td>
                                <td>
                                    <input type="text" name="taoistgrade" id="taoistgrade" size="50" runat="server" /></td>
                                <td>
                                    <input type="text" name="taoistlevel" id="taoistlevel" size="50" runat="server" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="Label4" Text="<%$ Resources:StringResource ,lang_memo %>" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox TextMode="MultiLine" ID="memo" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </tbody>
        </table>
        <button type="submit" class="btn"><asp:Label ID="Label31" Text="확인" runat="server" ></asp:Label></button>
    </div>
</asp:Content>

