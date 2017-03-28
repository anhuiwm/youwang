<%@ Page MasterPageFile="~/Main.Master" Language="C#" AutoEventWireup="true" CodeBehind="halfpagckage_Form.aspx.cs" Inherits="TheSoulGMTool.systemEvent.halfpagckage_Form" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="span9">
        <asp:HiddenField ID="idx" runat="server" />
        <table class="table table-bordered">
            <tbody>
                <tr>
                    <th><asp:Label ID="lang_server" Text="<%$ Resources:languageResource ,lang_server %>" runat="server" ></asp:Label> </th>
                    <td><span id="change_server" runat="server"></span></td>
                </tr>
                <tr>
                    <th><asp:Label ID="Label1" Text="<%$ Resources:languageResource ,lang_ItemName %>" runat="server" ></asp:Label></th>
                    <td><asp:TextBox ID="NameCN1" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <th><asp:Label ID="Label2" Text="<%$ Resources:languageResource ,lang_kind %>" runat="server" ></asp:Label></th>
                    <td>
                        Ruby
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="Label3" Text="<%$ Resources:languageResource ,lang_price %>" runat="server" ></asp:Label></th>
                    <td><asp:TextBox ID="payValue" TextMode="Number" CssClass="onlyNumber" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <th><asp:Label ID="Label5" Text="<%$ Resources:languageResource ,lang_vipPoint %>" runat="server" ></asp:Label></th>
                    <td><asp:TextBox ID="vipPoint" TextMode="Number" CssClass="onlyNumber" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <th><asp:Label ID="Label6" Text="<%$ Resources:languageResource ,lang_grade %>" runat="server" ></asp:Label></th>
                    <td>
                        <asp:RadioButtonList CssClass="table-unbordered" RepeatDirection="Horizontal" ID="grade" runat="server">
                            <asp:ListItem Text="white" Value="1" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="green" Value="2" ></asp:ListItem>
                            <asp:ListItem Text="blue" Value="3"></asp:ListItem>
                            <asp:ListItem Text="violet" Value="4"></asp:ListItem>
                            <asp:ListItem Text="orange" Value="5"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="Label7" Text="<%$ Resources:languageResource ,lang_service %>" runat="server" ></asp:Label></th>
                    <td>
                        <asp:RadioButtonList CssClass="table-unbordered" RepeatDirection="Horizontal" ID="active" runat="server">
                            <asp:ListItem Text="<%$ Resources:languageResource ,lang_notapply %>" Value="0" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:languageResource ,lang_apply %>" Value="1" ></asp:ListItem>
                        </asp:RadioButtonList>
                        
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="Label8" Text="<%$ Resources:languageResource ,lang_tootip %>" runat="server" ></asp:Label></th>
                    <td><asp:TextBox ID="descCN" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <th><asp:Label ID="Label11" Text="<%$ Resources:languageResource ,lang_maxbuycount %>" runat="server" ></asp:Label></th>
                    <td><asp:TextBox ID="maxCnt" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <th><asp:Label ID="Label12" Text="<%$ Resources:languageResource ,lang_allItem %>" runat="server" ></asp:Label></th>
                    <td>
                        <button type="button" class="btn" onclick="tableAdd('allitem')"><asp:Label ID="Label20" Text="<%$ Resources:languageResource ,lang_btn_add %>" runat="server" ></asp:Label></button>
                        <table id="allitem" class="table table-bordered" runat="server">
                            <tr>
                                <th><asp:Label ID="Label13" Text="<%$ Resources:languageResource ,lang_Item %>" runat="server" ></asp:Label></th>
                                <th><asp:Label ID="Label14" Text="<%$ Resources:languageResource ,lang_grade %>" runat="server" ></asp:Label></th>
                                <th><asp:Label ID="Label32" Text="<%$ Resources:languageResource ,lang_amount %>" runat="server" ></asp:Label></th>
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
                    <th><asp:Label ID="Label15" Text="<%$ Resources:languageResource ,lang_pc1Item %>" runat="server" ></asp:Label></th>
                    <td>
                        <button type="button" class="btn" onclick="tableAdd('warrior_itemtable')"><asp:Label ID="Label19" Text="<%$ Resources:languageResource ,lang_btn_add %>" runat="server" ></asp:Label></button>
                        <table id="warrior_itemtable" class="table table-bordered" runat="server">
                            <tr>
                                <th><asp:Label ID="Label16" Text="<%$ Resources:languageResource ,lang_Item %>" runat="server" ></asp:Label></th>
                                <th><asp:Label ID="Label18" Text="<%$ Resources:languageResource ,lang_grade %>" runat="server" ></asp:Label></th>
                                <th><asp:Label ID="Label17" Text="<%$ Resources:languageResource ,lang_level %>" runat="server" ></asp:Label></th>                                
                            </tr>
                            <tr>

                                <td><select name="warrior_item" id="warrior_item" runat="server"></select></td>
                                <td><input type="text" name="warriorgrade" ID="warriorgrade" size="50" runat="server" /></td>
                                <td><input type="text" name="warriorlevel" ID="warriorlevel" size="50" runat="server" /></td>                                
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="Label21" Text="<%$ Resources:languageResource ,lang_pc2Item %>" runat="server" ></asp:Label></th>
                    <td>
                        <button type="button" class="btn" onclick="tableAdd('sword_itemtable')"><asp:Label ID="Label22" Text="<%$ Resources:languageResource ,lang_btn_add %>" runat="server" ></asp:Label></button>
                        <table id="sword_itemtable" class="table table-bordered" runat="server">
                            <tr>
                                <th><asp:Label ID="Label23" Text="<%$ Resources:languageResource ,lang_Item %>" runat="server" ></asp:Label></th>
                                <th><asp:Label ID="Label25" Text="<%$ Resources:languageResource ,lang_grade %>" runat="server" ></asp:Label></th>
                                <th><asp:Label ID="Label24" Text="<%$ Resources:languageResource ,lang_level %>" runat="server" ></asp:Label></th>                                
                            </tr>
                            <tr>
                                <td><select name="sword_item" id="sword_item" runat="server"></select></td>
                                <td><input type="text" name="swordgrade" ID="swordgrade" size="50" runat="server" /></td>
                                <td><input type="text" name="swordlevel" ID="swordlevel" size="50" runat="server" /></td>                                
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="Label26" Text="<%$ Resources:languageResource ,lang_pc3Item %>" runat="server" ></asp:Label></th>
                    <td>
                        <button type="button" class="btn" onclick="tableAdd('taoist_itemtable')"><asp:Label ID="Label28" Text="<%$ Resources:languageResource ,lang_btn_add %>" runat="server" ></asp:Label></button>
                        <table id="taoist_itemtable" class="table table-bordered" runat="server">
                            <tr>
                                <th><asp:Label ID="Label27" Text="<%$ Resources:languageResource ,lang_Item %>" runat="server" ></asp:Label></th>
                                <th><asp:Label ID="Label30" Text="<%$ Resources:languageResource ,lang_grade %>" runat="server" ></asp:Label></th>
                                <th><asp:Label ID="Label29" Text="<%$ Resources:languageResource ,lang_level %>" runat="server" ></asp:Label></th>                                
                            </tr>
                            <tr>
                                <td><select name="taoist_item" id="taoist_item" runat="server"></select></td>
                                <td><input type="text" name="taoistgrade" ID="taoistgrade" size="50" runat="server" /></td>
                                <td><input type="text" name="taoistlevel" ID="taoistlevel" size="50" runat="server" /></td>                                
                            </tr>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
        <button type="submit" class="btn"><asp:Label ID="Label31" Text="<%$ Resources:languageResource ,lang_btn_ok %>" runat="server" ></asp:Label></button>
    </div>
</asp:Content>