<%@ Page MasterPageFile="~/Main.Master" Language="C#" AutoEventWireup="true" CodeBehind="eventForm.aspx.cs" Inherits="TheSoulGMTool.systemEvent.eventForm" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function formSend(mode) {
            var server = "";
            $('input:checkbox[name=serverid]:checked').each(function () {
                if (server == "")
                    server = $(this).val();
                else
                    server = server + "," + $(this).val();
            });
            var confirmMsg = "<%=GetGlobalResourceObject("languageResource","lang_msgEventReg")%>";
            if (mode == 1) {
                confirmMsg = "<%=GetGlobalResourceObject("languageResource","lang_msgEventIDEdit")%>";
            }
            if ($("#idx").val() != $("#event_id").val()) {
                if (confirm("<%=GetGlobalResourceObject("languageResource","lang_msgEventIDEdit")%>")) {
                    $.ajax({
                        type: "POST",
                        url: "/WebService.asmx/GetEventServerCheck",
                        data: '{ "EventID":"' + $("#event_id").val() + '", "checked_Server": "' + server + '" }',
                        contentType: "application/json; charset=utf-16",
                        dataType: "json",
                        success: function (msg) {
                            if (confirm(msg.d)) {
                                $("#mode").val(mode);
                                document.forms[0].submit();
                            }
                            else
                                return;
                        }
                    });
                }
                else {
                    return;
                }
            }
            else {
                $("#mode").val(mode);
                document.forms[0].submit();
            }
        }
        function trigger(name, type) {
            $.ajax({
                type: "POST",
                url: "/WebService.asmx/GetTrigerDesc",
                data: "{ 'trigerType': '"+type+"' }",
                contentType: "application/json; charset=utf-16",
                dataType: "json",
                success: function (msg) {
                    var data = msg.d;
                    if (name == "active1") {
                        $("#active1Desc1").html(data.value1);
                        $("#active1Desc2").html(data.value2);
                        $("#active1Desc3").html(data.value3);
                        $("#triggerid1").val(data.TriggerID);
                    }
                    else if (name == "active2") {
                        $("#active2Desc1").html(data.value1);
                        $("#active2Desc2").html(data.value2);
                        $("#active2Desc3").html(data.value3);
                        $("#triggerid2").val(data.TriggerID);
                    }
                    else if (name == "clear1") {
                        $("#clear1Desc1").html(data.value1);
                        $("#clear1Desc2").html(data.value2);
                        $("#clear1Desc3").html(data.value3);
                    }
                    if (name == "clear2") {
                        $("#clear2Desc1").html(data.value1);
                        $("#clear2Desc2").html(data.value2);
                        $("#clear2Desc3").html(data.value3);
                    }
                    makeorderID();
                }
            });
        }
        $(function () {
            var $input = $(".clearvalue", this);

            if ($("input:checkbox[id='chkAutoOrder']").is(":checked") == true) {
                $("#orderID").attr("readonly", true);
            }
            else
                $("#orderID").attr("readonly", false);

            $input.bind('input keyup keydown paste change', function () {
                makeorderID();
            });
        });
        
        function orderIDCheck() {
            if ($("input:checkbox[id='chkAutoOrder']").is(":checked") == true) {
                $("#orderID").attr("readonly", true);
            }
            else
                $("#orderID").attr("readonly", false);
        }

    </script> 
    <div class="span9">
        <asp:HiddenField ID="idx" runat="server" />
        <asp:HiddenField ID="mode" runat="server" />
        <asp:HiddenField ID="triggerid1" runat="server" />
        <asp:HiddenField ID="triggerid2" runat="server" />
        <table class="table table-bordered">
            <tbody>
                <tr>
                    <th><asp:Label ID="lang_server" Text="<%$ Resources:languageResource ,lang_server %>" runat="server" ></asp:Label> </th>
                    <td colspan="4"><span id="change_server" runat="server"></span></td>
                </tr>
                <tr>
                    <th><asp:Label ID="lang_eventType" Text="<%$ Resources:languageResource ,lang_eventType %>" runat="server"></asp:Label></th>
                    <td colspan="4">
                        <asp:DropDownList ID="eventType" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="Label14" Text="Event ID" runat="server"></asp:Label></th>
                    <td colspan="4">
                        <asp:TextBox ID="event_id" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <th><asp:Label ID="lang_eventTitle" Text="<%$ Resources:languageResource ,lang_eventTitle %>" runat="server"></asp:Label></th>
                    <td colspan="4">
                        <asp:TextBox ID="eventTitle" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <th><asp:Label ID="lang_startDay" Text="<%$ Resources:languageResource ,lang_startDay %>" runat="server"></asp:Label></th>
                    <td colspan="4">
                        <asp:TextBox ID="startDay" TextMode="Date" Width="150" runat="server"></asp:TextBox>
                        <asp:DropDownList ID="startHour" Width="80" runat="server"></asp:DropDownList>
                        <asp:DropDownList ID="startMin" Width="80" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lang_endday" Text="<%$ Resources:languageResource ,lang_endday %>" runat="server"></asp:Label></th>
                    <td colspan="4">
                        <asp:TextBox ID="endDay" TextMode="Date" Width="150" runat="server"></asp:TextBox>
                        <asp:DropDownList ID="endHour" Width="80" runat="server"></asp:DropDownList>
                        <asp:DropDownList ID="endMin" Width="80" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lang_loop_state" Text="<%$ Resources:languageResource ,lang_loop_state %>" runat="server"></asp:Label></th>
                    <td colspan="4">
                        <asp:RadioButtonList CssClass="table-unbordered" ID="eventloop" RepeatDirection="Horizontal" runat="server">
                            <asp:ListItem Value="0" Text="X"></asp:ListItem>
                            <asp:ListItem Value="1" Text="O"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lang_loop_type" Text="<%$ Resources:languageResource ,lang_loop_type %>" runat="server"></asp:Label></th>
                    <td colspan="4">
                        <asp:DropDownList ID="loopType" runat="server">
                            <asp:ListItem Value="0" Text="None"></asp:ListItem>
                            <asp:ListItem Value="1" Text="Repeat"></asp:ListItem>
                            <asp:ListItem Value="2" Text="Day"></asp:ListItem>
                            <asp:ListItem Value="3" Text="Week"></asp:ListItem>
                            <asp:ListItem Value="4" Text="Month"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="Label12" Text="<%$ Resources:languageResource ,lang_sortNum %>" runat="server"></asp:Label></th>
                    <td colspan="4">
                        <asp:TextBox ID="orderID" TextMode="Number" CssClass="onlyNumber" runat="server"></asp:TextBox> <input type="checkbox" id="chkAutoOrder" value="1" onclick="orderIDCheck()"  checked="checked" /> <asp:Label ID="Label13" Text="<%$ Resources:languageResource ,lang_auto_orderNum %>" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lang_active_trigger1" Text="<%$ Resources:languageResource ,lang_active_trigger %>" runat="server"></asp:Label>1</th>
                    <td colspan="4">
                        <select name="active_trigger1" ID="active_trigger1" onchange="trigger('active1',this.value)" runat="server"></select>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lang_active_trigger_value1" Text="<%$ Resources:languageResource ,lang_active_trigger_value %>" runat="server"></asp:Label>1</th>
                    <td style="width:20%;border-right-style:none;">
                        Value1 : <asp:TextBox Width="75%" TextMode="Number" CssClass="onlyNumber" ID="active1_1" runat="server"></asp:TextBox><br />
                        Value2 : <asp:TextBox Width="75%" TextMode="Number" CssClass="onlyNumber" ID="active1_2" runat="server"></asp:TextBox><br />
                        Value3 : <asp:TextBox Width="75%" TextMode="Number" CssClass="onlyNumber" ID="active1_3" runat="server"></asp:TextBox><br />
                    </td>
                    <td style="width:20%;border-left-style:none;"><span ID="active1Desc1"></span></td>
                    <td style="width:20%;border-left-style:none;"><span ID="active1Desc2"></span></td>
                    <td style="width:20%;border-left-style:none;"><span ID="active1Desc3"></span></td>
                </tr>
                <tr>
                    <th><asp:Label ID="lang_active_trigger2" Text="<%$ Resources:languageResource ,lang_active_trigger %>" runat="server"></asp:Label>2</th>
                    <td colspan="4">
                        <select name="active_trigger2" onchange="trigger('active2',this.value)" ID="active_trigger2" runat="server"></select>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lang_active_trigger_value2" Text="<%$ Resources:languageResource ,lang_active_trigger_value %>" runat="server"></asp:Label>2</th>
                    <td style="border-right-style:none;">
                        Value1 : <asp:TextBox Width="75%" TextMode="Number" CssClass="onlyNumber" ID="active2_1" runat="server"></asp:TextBox><br />
                        Value2 : <asp:TextBox Width="75%" TextMode="Number" CssClass="onlyNumber" ID="active2_2" runat="server"></asp:TextBox><br />
                        Value3 : <asp:TextBox Width="75%" TextMode="Number" CssClass="onlyNumber" ID="active2_3" runat="server"></asp:TextBox><br />
                    </td>
                    <td style="border-left-style:none;"><span ID="active2Desc1"></span></td>
                    <td style="border-left-style:none;"><span ID="active2Desc2"></span></td>
                    <td style="border-left-style:none;"><span ID="active2Desc3"></span></td>
                </tr>
                <tr>
                    <th><asp:Label ID="lang_clear_trigger1" Text="<%$ Resources:languageResource ,lang_clear_trigger %>" runat="server"></asp:Label>1</th>
                    <td colspan="4">
                        <select name="clear_trigger1" ID="clear_trigger1" onchange="trigger('clear1',this.value)" runat="server"></select>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lang_clear_trigger_value1" Text="<%$ Resources:languageResource ,lang_clear_trigger_value %>" runat="server"></asp:Label>1</th>
                    <td style="width:30%;border-right-style:none;">
                        Value1 : <asp:TextBox Width="75%" TextMode="Number" CssClass="onlyNumber clearvalue" ID="clear1_1" runat="server"></asp:TextBox><br />
                        Value2 : <asp:TextBox Width="75%" TextMode="Number" CssClass="onlyNumber clearvalue" ID="clear1_2" runat="server"></asp:TextBox><br />
                        Value3 : <asp:TextBox Width="75%" TextMode="Number" CssClass="onlyNumber clearvalue" ID="clear1_3" runat="server"></asp:TextBox><br />
                    </td>
                    <td style="border-left-style:none;"><span ID="clear1Desc1"></span></td>
                    <td style="border-left-style:none;"><span ID="clear1Desc2"></span></td>
                    <td style="border-left-style:none;"><span ID="clear1Desc3"></span></td>
                </tr>
                <tr>
                    <th><asp:Label ID="lang_clear_trigger2" Text="<%$ Resources:languageResource ,lang_clear_trigger %>" runat="server"></asp:Label>2</th>
                    <td colspan="4">
                        <select name="clear_trigger1" ID="clear_trigger2" onchange="trigger('clear2',this.value)" runat="server"></select>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lang_clear_trigger_value2" Text="<%$ Resources:languageResource ,lang_clear_trigger_value %>" runat="server"></asp:Label>2</th>
                    <td style="width:30%;border-right-style:none;">
                        Value1 : <asp:TextBox Width="75%" TextMode="Number" CssClass="onlyNumber clearvalue" ID="clear2_1" runat="server"></asp:TextBox><br />
                        Value2 : <asp:TextBox Width="75%" TextMode="Number" CssClass="onlyNumber clearvalue" ID="clear2_2" runat="server"></asp:TextBox><br />
                        Value3 : <asp:TextBox Width="75%" TextMode="Number" CssClass="onlyNumber clearvalue" ID="clear2_3" runat="server"></asp:TextBox><br />
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
                                <th style="width:30%;"><asp:Label ID="lang_viplelve1" Text="<%$ Resources:languageResource ,lang_viplelve %>" runat="server"></asp:Label></th>
                                <th style="width:30%;"><asp:Label ID="lang_rewad1" Text="<%$ Resources:languageResource ,lang_reward %>" runat="server"></asp:Label></th>
                                <th style="width:20%;"><asp:Label ID="Label9" Text="<%$ Resources:languageResource ,lang_grade %>" runat="server"></asp:Label></th>
                                <th style="width:20%;"><asp:Label ID="lang_count" Text="<%$ Resources:languageResource ,lang_count %>" runat="server"></asp:Label></th>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList Width="100%" ID="all_vipLevel1" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList CssClass="rewardItem"  Width="100%" ID="all_reward1" runat="server">
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
                                    <asp:DropDownList CssClass="rewardItem"  Width="100%" ID="all_reward2" runat="server">
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
                                    <asp:DropDownList CssClass="rewardItem"  Width="100%" ID="all_reward3" runat="server">
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
                                    <asp:DropDownList CssClass="rewardItem"  Width="100%" ID="all_reward4" runat="server">
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
                                    <asp:DropDownList CssClass="rewardItem"  Width="100%" ID="all_reward5" runat="server">
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
                                    <asp:DropDownList CssClass="rewardItem"  Width="100%" ID="warrior_reward1" runat="server">
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
                                    <asp:DropDownList CssClass="rewardItem"  Width="100%" ID="warrior_reward2" runat="server">
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
                                <td><asp:DropDownList CssClass="rewardItem"  Width="100%" ID="warrior_reward3" runat="server">
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
                                    <asp:DropDownList CssClass="rewardItem"  Width="100%" ID="warrior_reward4" runat="server">
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
                                    <asp:DropDownList CssClass="rewardItem"  Width="100%" ID="warrior_reward5" runat="server">
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
                                <th style="width:20%;"><asp:Label ID="Label1" Text="<%$ Resources:languageResource ,lang_viplelve %>" runat="server"></asp:Label></th>
                                <th style="width:40%;"><asp:Label ID="Label2" Text="<%$ Resources:languageResource ,lang_reward %>" runat="server"></asp:Label></th>
                                <th style="width:20%;"><asp:Label ID="Label4" Text="<%$ Resources:languageResource ,lang_grade %>" runat="server"></asp:Label></th>
                                <th style="width:20%;"><asp:Label ID="Label3" Text="<%$ Resources:languageResource ,lang_level %>" runat="server"></asp:Label></th>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList Width="100%" ID="sword_vipLevel1" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList CssClass="rewardItem"  Width="100%" ID="sword_reward1" runat="server">
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
                                    <asp:DropDownList CssClass="rewardItem"  Width="100%" ID="sword_reward2" runat="server">
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
                                    <asp:DropDownList CssClass="rewardItem"  Width="100%" ID="sword_reward3" runat="server">
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
                                    <asp:DropDownList CssClass="rewardItem"  Width="100%" ID="sword_reward4" runat="server">
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
                                    <asp:DropDownList CssClass="rewardItem"  Width="100%" ID="sword_reward5" runat="server">
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
                                <th style="width:20%;"><asp:Label ID="Label5" Text="<%$ Resources:languageResource ,lang_viplelve %>" runat="server"></asp:Label></th>
                                <th style="width:40%;"><asp:Label ID="Label6" Text="<%$ Resources:languageResource ,lang_reward %>" runat="server"></asp:Label></th>
                                <th style="width:20%;"><asp:Label ID="Label8" Text="<%$ Resources:languageResource ,lang_grade %>" runat="server"></asp:Label></th>
                                <th style="width:20%;"><asp:Label ID="Label7" Text="<%$ Resources:languageResource ,lang_level %>" runat="server"></asp:Label></th>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList Width="100%" ID="taoist_vipLevel1" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList CssClass="rewardItem"  Width="100%" ID="taoist_reward1" runat="server">
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
                                    <asp:DropDownList CssClass="rewardItem"  Width="100%" ID="taoist_reward2" runat="server">
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
                                    <asp:DropDownList CssClass="rewardItem"  Width="100%" ID="taoist_reward3" runat="server">
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
                                    <asp:DropDownList CssClass="rewardItem"  Width="100%" ID="taoist_reward4" runat="server">
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
                                    <asp:DropDownList CssClass="rewardItem"  Width="100%" ID="taoist_reward5" runat="server">
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
        <%if(idx.Value == "0") {%>
        <button type="button" onclick="formSend(0)" class="btn" ><asp:Label ID="lang_btn_ok" Text="<%$ Resources:languageResource , lang_btn_ok%>" runat="server"></asp:Label></button>
        <%}
            else{
        %>
        <span id="buttonGubun">
            <button type="button" onclick="formSend(1)" class="btn" ><asp:Label ID="Label10" Text="<%$ Resources:languageResource ,lang_btn_update %>" runat="server" ></asp:Label></button> 
            <button type="button"  onclick="defaultSubmit('<%=mode.ClientID %>',2)" class="btn" ><asp:Label ID="Label11" Text="<%$ Resources:languageResource ,lang_btn_delete %>" runat="server" ></asp:Label></button></span>
        <%} %>
    </div>
</asp:Content>
