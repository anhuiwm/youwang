<%@ Page MasterPageFile="~/Main.Master" Language="C#" AutoEventWireup="true" CodeBehind="gachaForm.aspx.cs" Inherits="TheSoulGMTool.management.gachaForm" %>
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
                var index = 0;
                $("#rewardTable > tbody").children('tr').each(function () {
                    index = index + 1
                });
                var rewardCntCheck = true;
                $('input[id="maxCnt"]').each(function () {
                    if ($(this).val() == "" || $(this).val() == "0") {
                        rewardCntCheck = false;
                    }
                });
                var rewardProbCheck = true;
                $('input[id="prob"]').each(function () {
                    if ($(this).val() == "" || $(this).val() == "0") {
                        rewardProbCheck = false;
                    }
                });
                if (index <= 4) {
                    alert("<%=HttpContext.GetGlobalResourceObject("languageResource","lang_msg_gachaCount") %>");
                    return false;
                }
                else if ($("#mainSoulID").val() == "-1" || $("#subSoulID1").val() == "-1" || $("#subSoulID2").val() == "-1" || $("#subSoulID3").val() == "-1") {
                    alert("<%=HttpContext.GetGlobalResourceObject("languageResource","lang_msg_gachaSoul") %>");
                    return false;
                }
                else if ($("#gachaCash").val() == "" || $("#gachaCash").val() == "0" || !rewardCntCheck || !rewardProbCheck) {
                    alert("<%=HttpContext.GetGlobalResourceObject("languageResource","lang_msg_gachaPrice") %>");
                    return false;
                }
                else {
                    if ($("input:hidden[id=mode]").val() != 0) {
                        $("input:hidden[id=mode]").val(1);
                    }
                    document.forms[0].submit();
                }
            }
        }

        function tableRowAdd() {
            if ($("#idx").val() == "0") {
                for (var i = 0; i < 3; i++) {
                    tableAdd('rewardTable');
                }
            }
        }
    </script>
    <div class="span9">
        <table class="table table-bordered">
            <tr>
                <th>
                    <asp:Label ID="lang_server" Text="<%$ Resources:languageResource ,lang_server %>" runat="server"></asp:Label></th>
                <td><span id="change_server" runat="server"></span></td>
            </tr>
        </table>
        <asp:HiddenField ID="idx" runat="server" />
        <asp:HiddenField ID="mode" runat="server" />
        <table class="table table-bordered">
            <tbody>
                <tr>
                    <th style="width: 25%;">
                        <asp:Label ID="Label10" Text="<%$ Resources:languageResource ,lang_startdate %>" runat="server"></asp:Label></th>
                    <td style="width: 75%;">
                        <asp:TextBox ID="startDay" TextMode="Date" Width="150" runat="server"></asp:TextBox>
                        <asp:DropDownList ID="startHour" Width="80" runat="server"></asp:DropDownList>
                        <asp:DropDownList ID="startMin" Width="80" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="Label1" Text="<%$ Resources:languageResource ,lang_enddate %>" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="endDay" TextMode="Date" Width="150" runat="server"></asp:TextBox>
                        <asp:DropDownList ID="endHour" Width="80" runat="server"></asp:DropDownList>
                        <asp:DropDownList ID="endMin" Width="80" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="Label2" Text="<%$ Resources:languageResource ,lang_gachaCash %>" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="gachaCash" TextMode="Number" CssClass="onlyNumber" Width="110" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="Label3" Text="<%$ Resources:languageResource ,lang_mainSoul %>" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList Width="50%" ID="mainSoulID" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="Label4" Text="<%$ Resources:languageResource ,lang_subSoul %>" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList Width="30%" ID="subSoulID1" runat="server">
                        </asp:DropDownList>&nbsp;
                        <asp:DropDownList Width="30%" ID="subSoulID2" runat="server">
                        </asp:DropDownList>&nbsp;
                        <asp:DropDownList Width="30%" ID="subSoulID3" runat="server">
                        </asp:DropDownList>&nbsp;
                    </td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="Label5" Text="<%$ Resources:languageResource ,lang_reward %>" runat="server"></asp:Label></th>
                    <td>
                        <button type="button" class="btn" onclick="tableAdd('rewardTable')">
                            <asp:Label ID="Label20" Text="<%$ Resources:languageResource ,lang_btn_add %>" runat="server"></asp:Label></button>
                        <table id="rewardTable" class="table table-bordered" runat="server">
                            <tr>
                                <th style="width: 40%;">
                                    <asp:Label ID="Label9" Text="<%$ Resources:languageResource ,lang_soul %>" runat="server"></asp:Label></th>
                                <th style="width: 20%;">
                                    <asp:Label ID="Label12" Text="<%$ Resources:languageResource ,lang_soulCount %>" runat="server"></asp:Label></th>
                                <th style="width: 20%;">
                                    <asp:Label ID="Label13" Text="<%$ Resources:languageResource ,lang_gachaRate %>" runat="server"></asp:Label>(%)</th>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList Width="100%" ID="rewardID" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="maxCnt" runat="server"></asp:TextBox></td>
                                <td>
                                    <asp:TextBox Width="90%" TextMode="Number" CssClass="onlyNumber" ID="prob" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
        <%if (idx.Value == "0")
          {%>
        <span id="Span1">
            <button type="button" onclick="formSend()" class="btn">
                <asp:Label ID="Label6" Text="<%$ Resources:languageResource ,lang_btn_ok %>" runat="server"></asp:Label></button></span>
        <%}
          else
          {
        %>
        <span id="buttonGubun">
            <button type="button" onclick="formSend()" class="btn">
                <asp:Label ID="Label8" Text="<%$ Resources:languageResource ,lang_btn_update %>" runat="server"></asp:Label></button>
            <button type="button" onclick="defaultSubmit('<%=mode.ClientID %>',2)" class="btn">
                <asp:Label ID="Label7" Text="<%$ Resources:languageResource ,lang_btn_delete %>" runat="server"></asp:Label></button></span>
        <%} %>
    </div>
    <script type="text/javascript">
        tableRowAdd();
    </script>
</asp:Content>

