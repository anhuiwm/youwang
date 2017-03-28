<%@ Page MasterPageFile="~/Main.Master" Language="C#" AutoEventWireup="true" CodeBehind="packageCheap_Form.aspx.cs" Inherits="TheSoulGMTool.systemEvent.packageCheap_Form" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var isFormSubmit;
        isFormSubmit = false;

        function formSend() {
            if (isFormSubmit) {
                alert("<%=HttpContext.GetGlobalResourceObject("languageResource","lang_msgDoubleClick") %>");
                return false;
            }
            else {
                isFormSubmit = true;
                var itemCount1 = 0;
                var itemCount2 = 0;
                if ($("#reward1_1 option:selected").val() != "-1") {
                    itemCount1 += 1;
                }
                if ($("#reward1_2 option:selected").val() != "-1") {
                    itemCount1 += 1;
                }
                if ($("#reward1_3 option:selected").val() != "-1") {
                    itemCount1 += 1;
                }
                if ($("#reward1_4 option:selected").val() != "-1") {
                    itemCount1 += 1;
                }
                if ($("#reward1_5 option:selected").val() != "-1") {
                    itemCount1 += 1;
                }
                if ($("#reward2_1 option:selected").val() != "-1") {
                    itemCount2 += 1;
                }
                if ($("#reward2_2 option:selected").val() != "-1") {
                    itemCount2 += 1;
                }
                if ($("#reward2_3 option:selected").val() != "-1") {
                    itemCount2 += 1;
                }
                if ($("#reward2_4 option:selected").val() != "-1") {
                    itemCount2 += 1;
                }
                if ($("#reward2_5 option:selected").val() != "-1") {
                    itemCount2 += 1;
                }
                if (itemCount1 == 0 && itemCount2 == 0) {
                    alert("<%=HttpContext.GetGlobalResourceObject("languageResource","lang_msgCheapMinCount") %>");
                    return false;
                }
                else {
                    document.forms[0].submit();
                }
            }
        }
    </script>
    <div class="span9">
        <asp:HiddenField ID="idx" runat="server" />
        <asp:HiddenField ID="idx2" runat="server" />
        <asp:HiddenField ID="edit" runat="server" />
        <table class="table table-bordered">
            <tbody>
                <tr>
                    <th><asp:Label ID="lang_server" Text="<%$ Resources:languageResource ,lang_server %>" runat="server" ></asp:Label> </th>
                    <td><span id="change_server" runat="server"></span></td>
                </tr>
                <tr>
                    <th>Billing PlatForm</th>
                    <td><span id="span_billing" runat="server"></span></td>
                </tr>
                <tr>
                    <th><asp:Label ID="Label10" Text="<%$ Resources:languageResource ,lang_validtime %>" runat="server" ></asp:Label></th>
                    <td>
                        <asp:TextBox ID="startDay" TextMode="Date" Width="110" runat="server"></asp:TextBox>
                        <asp:DropDownList ID="startHour" Width="80" runat="server"></asp:DropDownList>
                        <asp:DropDownList ID="startMin" Width="80" runat="server"></asp:DropDownList>
                        ~
                        <asp:TextBox ID="endDay" TextMode="Date" Width="110" runat="server"></asp:TextBox>
                        <asp:DropDownList ID="endHour" Width="80" runat="server"></asp:DropDownList>
                        <asp:DropDownList ID="endMin" Width="80" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lang_loop_type" Text="<%$ Resources:languageResource ,lang_loop_type %>" runat="server"></asp:Label></th>
                    <td colspan="4">
                        <asp:DropDownList ID="loopType" runat="server">
                            <asp:ListItem Value="0" Text="None"></asp:ListItem>
                            <asp:ListItem Value="2" Text="Day"></asp:ListItem>
                            <asp:ListItem Value="3" Text="Week"></asp:ListItem>
                            <asp:ListItem Value="4" Text="Month"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="Label6" Text="<%$ Resources:languageResource ,lang_1yuanReward %>" runat="server"></asp:Label></th>
                    <td>
                        <table style="width: 100%;" class="table table-bordered">
                            <tr>
                                <th><asp:Label ID="Label15" Text="<%$ Resources:languageResource ,lang_ItemName %>" runat="server" ></asp:Label></th>
                                <td colspan="2"><asp:TextBox ID="title1"  runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <th><asp:Label ID="Label11" Text="<%$ Resources:languageResource ,lang_price %>" runat="server" ></asp:Label></th>
                                <td colspan="2"><asp:DropDownList ID="payValue1" runat="server"></asp:DropDownList></td>
                            </tr>
                            <tr>
                                <th style="width:30%;"><asp:Label ID="lang_rewad1" Text="<%$ Resources:languageResource ,lang_reward %>" runat="server"></asp:Label></th>
                                <th style="width:20%;"><asp:Label ID="Label9" Text="<%$ Resources:languageResource ,lang_grade %>" runat="server"></asp:Label></th>
                                <th style="width:20%;"><asp:Label ID="lang_count" Text="<%$ Resources:languageResource ,lang_count %>" runat="server"></asp:Label></th>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList CssClass="rewardItem" Width="100%" ID="reward1_1" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td><asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="grade1_1" runat="server"></asp:TextBox></td>
                                <td><asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="rewardcnt1_1" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList CssClass="rewardItem" Width="100%" ID="reward1_2" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td><asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="grade1_2" runat="server"></asp:TextBox></td>
                                <td>
                                    <asp:TextBox  Width="90%" TextMode="Number" CssClass="onlyNumber" ID="rewardcnt1_2" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList CssClass="rewardItem" Width="100%" ID="reward1_3" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td><asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="grade1_3" runat="server"></asp:TextBox></td>
                                <td>
                                    <asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="rewardcnt1_3" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList CssClass="rewardItem" Width="100%" ID="reward1_4" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td><asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="grade1_4" runat="server"></asp:TextBox></td>
                                <td>
                                    <asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="rewardcnt1_4" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList CssClass="rewardItem" Width="100%" ID="reward1_5" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td><asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="grade1_5" runat="server"></asp:TextBox></td>
                                <td>
                                    <asp:TextBox  Width="90%" TextMode="Number" CssClass="onlyNumber" ID="rewardcnt1_5" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th><asp:Label ID="Label1" Text="<%$ Resources:languageResource ,lang_maxbuycount %>" runat="server" ></asp:Label></th>
                                <td colspan="2"><asp:TextBox TextMode="Number" ID="maxCnt" CssClass="onlyNumber" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <th><asp:Label ID="Label14" Text="VIP Point" runat="server" ></asp:Label></th>
                                <td colspan="2"><asp:TextBox TextMode="Number" ID="vip_point1" CssClass="onlyNumber" runat="server"></asp:TextBox></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="Label7" Text="<%$ Resources:languageResource ,lang_3yuanReward %>" runat="server"></asp:Label></th>
                    <td>
                        <table style="width: 100%;" class="table table-bordered">
                             <tr>
                                <th><asp:Label ID="Label16" Text="<%$ Resources:languageResource ,lang_ItemName %>" runat="server" ></asp:Label></th>
                                <td colspan="2"><asp:TextBox ID="title2"  runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <th><asp:Label ID="Label12" Text="<%$ Resources:languageResource ,lang_price %>" runat="server" ></asp:Label></th>
                                <td colspan="2"><asp:DropDownList ID="payValue2" runat="server"></asp:DropDownList></td>
                            </tr>
                            <tr>
                                <th style="width:30%;"><asp:Label ID="Label2" Text="<%$ Resources:languageResource ,lang_reward %>" runat="server"></asp:Label></th>
                                <th style="width:20%;"><asp:Label ID="Label3" Text="<%$ Resources:languageResource ,lang_grade %>" runat="server"></asp:Label></th>
                                <th style="width:20%;"><asp:Label ID="Label4" Text="<%$ Resources:languageResource ,lang_count %>" runat="server"></asp:Label></th>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList CssClass="rewardItem" Width="100%" ID="reward2_1" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td><asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="grade2_1" runat="server"></asp:TextBox></td>
                                <td><asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="rewardcnt2_1" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList CssClass="rewardItem" Width="100%" ID="reward2_2" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td><asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="grade2_2" runat="server"></asp:TextBox></td>
                                <td>
                                    <asp:TextBox  Width="90%" TextMode="Number" CssClass="onlyNumber" ID="rewardcnt2_2" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList CssClass="rewardItem" Width="100%" ID="reward2_3" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td><asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="grade2_3" runat="server"></asp:TextBox></td>
                                <td>
                                    <asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="rewardcnt2_3" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList CssClass="rewardItem" Width="100%" ID="reward2_4" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td><asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="grade2_4" runat="server"></asp:TextBox></td>
                                <td>
                                    <asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="rewardcnt2_4" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList CssClass="rewardItem" Width="100%" ID="reward2_5" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td><asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="grade2_5" runat="server"></asp:TextBox></td>
                                <td>
                                    <asp:TextBox  Width="90%" TextMode="Number" CssClass="onlyNumber" ID="rewardcnt2_5" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th><asp:Label ID="Label5" Text="<%$ Resources:languageResource ,lang_maxbuycount %>" runat="server" ></asp:Label></th>
                                <td colspan="2"><asp:TextBox TextMode="Number" ID="maxCnt2" CssClass="onlyNumber" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <th><asp:Label ID="Label13" Text="VIP Point" runat="server" ></asp:Label></th>
                                <td colspan="2"><asp:TextBox TextMode="Number" ID="vip_point2" CssClass="onlyNumber" runat="server"></asp:TextBox></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
        <button type="button" onclick="formSend();" class="btn">
            <%if(idx2.Value == "0"){ %>
            <asp:Label ID="Label31" Text="<%$ Resources:languageResource ,lang_btn_ok %>" runat="server" ></asp:Label>
            <%} else{ %>
            <asp:Label ID="Label8" Text="<%$ Resources:languageResource ,lang_btn_update %>" runat="server" ></asp:Label>
            <%} %>
        </button>
    </div>
</asp:Content>