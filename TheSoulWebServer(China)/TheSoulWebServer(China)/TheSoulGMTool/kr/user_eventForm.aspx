<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="user_eventForm.aspx.cs" Inherits="TheSoulGMTool.kr.user_eventForm" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function sendForm() {
            var checkflage = true;
            var userValue1 = parseInt($("#clear1").val()) + parseInt($("#data_user_1").text());
            var userValue2 = parseInt($("#clear2").val()) + parseInt($("#data_user_2").text());
            
            if ($("#clear_value1").val() < userValue1 && checkflage) {
                checkflage = false;
            }
            if ($("#clear_value2").val() < userValue2 && checkflage) {
                checkflage = false;
            }
            if (checkflage) {
                document.forms[0].submit();
            }
            else {
                alert("<%=HttpContext.GetGlobalResourceObject("languageResource","lang_mgsTriggerClearValue") %>");
            }
        }
    </script>
    <div class="span9">
        <asp:HiddenField ID="idx" runat="server" />
        <table class="table table-bordered">
            <tbody>
                <tr>
                    <th class="auto-style1"><asp:Label ID="lang_eventTitle" Text="<%$ Resources:languageResource ,lang_eventTitle %>" runat="server"></asp:Label></th>
                    <td colspan="3"><asp:Label ID="data_title" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <th class="auto-style1"></th>

                    <th><asp:Label ID="Label1" Text="<%$ Resources:languageResource ,lang_clearCondition %>" runat="server" ></asp:Label></th>
                     <th><asp:Label ID="Label2" Text="<%$ Resources:languageResource ,lang_progression %>" runat="server" ></asp:Label></th>
                     <th><asp:Label ID="Label3" Text="<%$ Resources:languageResource ,lang_change %>" runat="server" ></asp:Label></th>   
                </tr>
                <tr>
                    <th class="auto-style1"><asp:Label ID="lang_clear_trigger1" Text="<%$ Resources:languageResource ,lang_clear_trigger %>" runat="server"></asp:Label>1</th>
                    <td><asp:Label ID="data_clear_1" runat="server"></asp:Label>
                        <asp:HiddenField ID="clear_value1" runat="server" />
                    </td>
                    <td><asp:Label ID="data_user_1" runat="server"></asp:Label></td>
                    <td><asp:TextBox TextMode="Number" CssClass="onlyNumber" ID="clear1" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <th class="auto-style1"><asp:Label ID="lang_clear_trigger2" Text="<%$ Resources:languageResource ,lang_clear_trigger %>" runat="server"></asp:Label>2</th>
                    <td><asp:Label ID="data_clear_2" runat="server"></asp:Label>
                        <asp:HiddenField ID="clear_value2" runat="server" />
                    </td>
                    <td><asp:Label ID="data_user_2" runat="server"></asp:Label></td>
                    <td><asp:TextBox TextMode="Number" CssClass="onlyNumber" ID="clear2" runat="server"></asp:TextBox></td>
                </tr>
            </tbody>
        </table>
        <button type="button" onclick="sendForm()" class="btn"><asp:Label ID="lang_btn_ok" Text="<%$ Resources:languageResource , lang_btn_update%>" runat="server"></asp:Label></button>
    </div>
</asp:Content>
<asp:Content ID="Content1" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .auto-style1 {
            width: 87px;
        }
    </style>
</asp:Content>
