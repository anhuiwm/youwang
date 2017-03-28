<%@ Page MasterPageFile="~/Main.Master" Language="C#" AutoEventWireup="true" CodeBehind="sevenDayEvent_Form.aspx.cs" Inherits="TheSoulGMTool.systemEvent.sevenDayEvent_Form" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="span9">
        <asp:HiddenField ID="idx" runat="server" />
        <table class="table table-bordered">
            <tbody>
                <tr>
                    <th><asp:Label ID="lang_server" Text="<%$ Resources:languageResource ,lang_server %>" runat="server" ></asp:Label> </th>
                    <td colspan="4"><span id="change_server" runat="server"></span></td>
                </tr>
                <tr>
                    <th><asp:Label ID="lang_eventTitle" Text="<%$ Resources:languageResource ,lang_eventTitle %>" runat="server"></asp:Label></th>
                    <td colspan="4">
                        <asp:TextBox ID="eventTitle" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <th><asp:Label ID="Label10" Text="<%$ Resources:languageResource ,lang_tootip %>" runat="server"></asp:Label></th>
                    <td colspan="4">
                        <asp:TextBox ID="eventTooltip" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <th><asp:Label ID="lang_clear_trigger1" Text="<%$ Resources:languageResource ,lang_clear_trigger %>" runat="server"></asp:Label>1</th>
                    <td colspan="4">
                        <select name="clear_trigger1" ID="clear_trigger1" runat="server"></select>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lang_clear_trigger_value1" Text="<%$ Resources:languageResource ,lang_clear_trigger_value %>" runat="server"></asp:Label>1</th>
                    <td style="width:30%;border-right-style:none;">
                        Value1 : <asp:TextBox Width="75%" TextMode="Number" CssClass="onlyNumber" ID="clear1_1" runat="server"></asp:TextBox><br />
                        Value2 : <asp:TextBox Width="75%" TextMode="Number" CssClass="onlyNumber" ID="clear1_2" runat="server"></asp:TextBox><br />
                        Value3 : <asp:TextBox Width="75%" TextMode="Number" CssClass="onlyNumber" ID="clear1_3" runat="server"></asp:TextBox><br />
                    </td>
                    <td style="border-left-style:none;"><span ID="clear1Desc1"></span></td>
                    <td style="border-left-style:none;"><span ID="clear1Desc2"></span></td>
                    <td style="border-left-style:none;"><span ID="clear1Desc3"></span></td>
                </tr>
                <tr>
                    <th><asp:Label ID="lang_clear_trigger2" Text="<%$ Resources:languageResource ,lang_clear_trigger %>" runat="server"></asp:Label>2</th>
                    <td colspan="4">
                        <select name="clear_trigger1" ID="clear_trigger2" runat="server"></select>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lang_clear_trigger_value2" Text="<%$ Resources:languageResource ,lang_clear_trigger_value %>" runat="server"></asp:Label>2</th>
                    <td style="width:30%;border-right-style:none;">
                        Value1 : <asp:TextBox Width="75%" TextMode="Number" CssClass="onlyNumber" ID="clear2_1" runat="server"></asp:TextBox><br />
                        Value2 : <asp:TextBox Width="75%" TextMode="Number" CssClass="onlyNumber" ID="clear2_2" runat="server"></asp:TextBox><br />
                        Value3 : <asp:TextBox Width="75%" TextMode="Number" CssClass="onlyNumber" ID="clear2_3" runat="server"></asp:TextBox><br />
                    </td>
                    <td style="border-left-style:none;"><span ID="clear2Desc1"></span></td>
                    <td style="border-left-style:none;"><span ID="clear2Desc2"></span></td>
                    <td style="border-left-style:none;"><span ID="clear2Desc3""></span></td>
                </tr>
                <tr>
                    <th><asp:Label ID="lang_allReward" Text="<%$ Resources:languageResource ,lang_allReward %>" runat="server"></asp:Label></th>
                    <td colspan="4">
                        <table style="width: 100%;" class="table table-bordered">
                            <tr>
                                <th style="width:30%;"><asp:Label ID="lang_rewad1" Text="<%$ Resources:languageResource ,lang_reward %>" runat="server"></asp:Label></th>
                                <th style="width:20%;"><asp:Label ID="Label9" Text="<%$ Resources:languageResource ,lang_grade %>" runat="server"></asp:Label></th>
                                <th style="width:20%;"><asp:Label ID="lang_count" Text="<%$ Resources:languageResource ,lang_count %>" runat="server"></asp:Label></th>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList  Width="100%" ID="all_reward1" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td><asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="all_grade1" runat="server"></asp:TextBox></td>
                                <td><asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="all_rewardcnt1" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList Width="100%" ID="all_reward2" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td><asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="all_grade2" runat="server"></asp:TextBox></td>
                                <td>
                                    <asp:TextBox  Width="90%" TextMode="Number" CssClass="onlyNumber" ID="all_rewardcnt2" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList Width="100%" ID="all_reward3" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td><asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="all_grade3" runat="server"></asp:TextBox></td>
                                <td>
                                    <asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="all_rewardcnt3" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            </table>
                    </td>
                </tr>
            </tbody>
        </table>
        <button type="submit" class="btn"><asp:Label ID="lang_btn_ok" Text="<%$ Resources:languageResource , lang_btn_ok%>" runat="server"></asp:Label></button>
    </div>
</asp:Content>
<asp:Content ID="Content1" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .auto-style1 {
            height: 50px;
        }
    </style>
</asp:Content>

