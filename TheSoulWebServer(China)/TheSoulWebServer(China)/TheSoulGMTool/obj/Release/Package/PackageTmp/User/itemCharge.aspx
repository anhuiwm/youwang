<%@ Page MasterPageFile="~/Main.Master" Language="C#" AutoEventWireup="true" CodeBehind="itemCharge.aspx.cs" Inherits="TheSoulGMTool.User.itemCharge" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(function () {
            $('.remaining').each(function () {
                var $maxcount = $('.maxcount', this);
                var $count = $('.count', this);
                var $input = $("#contents");

                var maximumByte = $maxcount.text() * 1;
                var update = function () {
                    var before = $count.text() * 1;
                    var str_len = $input.val().length;
                    var cbyte = 0;
                    var li_len = 0;
                    for (i = 0; i < str_len; i++) {
                        var ls_one_char = $input.val().charAt(i);
                        if (escape(ls_one_char).length > 4) {
                            cbyte += 4;
                        } else {
                            cbyte++;
                        }
                        if (cbyte <= maximumByte) {
                            li_len = i + 1;
                        }
                    }
                    if (parseInt(cbyte) > parseInt(maximumByte)) {
                        var str = $input.val();
                        var str2 = $input.val().substr(0, li_len);
                        $input.val(str2);
                        var cbyte = 0;
                        for (i = 0; i < $input.val().length; i++) {
                            var ls_one_char = $input.val().charAt(i);
                            if (escape(ls_one_char).length > 4) {
                                cbyte += 4;
                            } else {
                                cbyte++;
                            }
                        }
                    }
                    $count.text(cbyte);
                };
                $input.bind('input keyup keydown paste change', function () {
                    setTimeout(update, 0)
                });
                update();
            });
            $("#username").bind('input keyup keydown paste change', function () {
                data = $(this).val();
                maxCount = 1000;
                var stringArray = data.split(",");
                var usercnt = stringArray.length;
                if (usercnt > maxCount) {
                    stringArray.splice(maxCount, usercnt - maxCount);
                    data = stringArray.toString();
                    $(this).val(data);
                }
                $("#userCount").text(stringArray.length+"/"+maxCount);
            });
        });
    </script>
    <div class="span9">
        <table class="table table-bordered" style="width: 50%">
            <tr>
                <th><a href="/user/itemCharge.aspx?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>"><%=GetGlobalResourceObject("languageResource","lang_mailType2") %></a></th>
                <th><a href="/kr/user_group_mail.aspx?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>"><%=GetGlobalResourceObject("languageResource","lang_fileItemCharge") %></a></th>
            </tr>
        </table>
        <asp:HiddenField ID="useridx" runat="server" />
        <table class="table table-bordered">
            <tbody>
                <tr>
                    <th><%=HttpContext.GetGlobalResourceObject("languageResource","lang_title") %></th>
                    <td><asp:TextBox ID="title" MaxLength="11" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <th><%=HttpContext.GetGlobalResourceObject("languageResource","lang_contents") %></th>
                    <td>
                        <asp:TextBox TextMode="MultiLine" ID="contents" runat="server"></asp:TextBox>
                        <span class="remaining">
                            <span class="count">0</span>/<span class="maxcount">2000</span>byte
                        </span>
                    </td>
                </tr>
                <tr>
                    <th><%=HttpContext.GetGlobalResourceObject("languageResource","lang_userName") %></th>
                    <td>
                        <asp:TextBox TextMode="MultiLine" Rows="2" ID="username" runat="server"></asp:TextBox> 
                        <span id="userCount">0/1000</span>                       
                    </td>
                </tr>
                <tr>
                    <th><%=HttpContext.GetGlobalResourceObject("languageResource","lang_Item") %></th>
                    <td>
                        <asp:HiddenField ID="itemid" runat="server" />
                        <asp:HiddenField ID="itema" runat="server" />
                        <asp:HiddenField ID="level" runat="server" />
                        <asp:HiddenField ID="grade" runat="server" />
                        <button type="button" id="itemAddButton" onclick="openItemPop()" class="btn" ><%=HttpContext.GetGlobalResourceObject("languageResource","lang_btn_add") %></button>
                        <table id="itemtable"  class="table table-bordered">
                            <tr>
                                <th><%=HttpContext.GetGlobalResourceObject("languageResource","lang_Item") %></th>
                                <th><%=HttpContext.GetGlobalResourceObject("languageResource","lang_level") %></th>
                                <th><%=HttpContext.GetGlobalResourceObject("languageResource","lang_grade") %></th>
                                <th><%=HttpContext.GetGlobalResourceObject("languageResource","lang_count") %></th>
                                <th><%=HttpContext.GetGlobalResourceObject("languageResource","lang_btn_delete") %></th>
                            </tr>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
        <asp:Button ID="btn" CssClass="btn" Text="<%$ Resources:languageResource ,lang_btn_ok %>" OnClientClick="submit();" runat="server" />
    </div>
</asp:Content>