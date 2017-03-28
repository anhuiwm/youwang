<%@ Page MasterPageFile="~/Main.Master" Language="C#" AutoEventWireup="true" CodeBehind="lineNoticeForm.aspx.cs" Inherits="TheSoulGMTool.management.lineNoticeForm" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(function () {
            $("#displayTime").bind('input keyup keydown paste change', function () {
                if ($(this).val() > 120) {
                    $(this).val(120);
                }
                else if ($(this).val() < 0) {
                    $(this).val(0);
                }
            });
        });
    </script>
    <div class="span9">
        <table border="0" style="background-color:#ffffff;width:100%">
                <tr>
                    <td>
                        <asp:HiddenField ID="idx" runat="server" />
                        <asp:HiddenField ID="mode" runat="server" />
                        <table class="table table-bordered">
                            <tr>
                                <th><asp:Label ID="Label1" Text="<%$ Resources:languageResource ,lang_server %>" runat="server" ></asp:Label></th>
                                <td><span id="change_server" runat="server"></span></td>
                            </tr>
                            <tr>
                                <th><asp:Label ID="Label2" Text="<%$ Resources:languageResource ,lang_postDate %>" runat="server" ></asp:Label></th>
                                <td style="vertical-align:middle;">
                                    <asp:TextBox TextMode="Date" ID="sDate" runat="server"></asp:TextBox> <asp:DropDownList Width="80" ID="sHour" runat="server"></asp:DropDownList> <asp:DropDownList ID="sMin" Width="80" runat="server"></asp:DropDownList> ~
                                    <asp:TextBox TextMode="Date" ID="eDate" runat="server"></asp:TextBox> <asp:DropDownList Width="80" ID="eHour" runat="server"></asp:DropDownList> <asp:DropDownList ID="eMin" Width="80" runat="server"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <th><asp:Label ID="Label3" Text="<%$ Resources:languageResource ,lang_displayTerm %>" runat="server" ></asp:Label></th>
                                <td style="vertical-align:middle;">
                                    <%--<asp:Label ID="labTimeTootip" Font-Bold="true" Text="<%$ Resources:languageResource ,lang_lineMsg %>" runat="server"></asp:Label>
                                    <br />--%>
                                    <asp:RadioButtonList CssClass="table-unbordered" ID="type" RepeatDirection="Horizontal" runat="server">
                                        <asp:ListItem Value="1" Text="<%$ Resources:languageResource ,lang_oneCount %>" Selected="True"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="<%$ Resources:languageResource ,lang_repeatMin %>"></asp:ListItem>
                                    </asp:RadioButtonList>
                                    <br />
                                    <asp:TextBox TextMode="Number" CssClass="onlyNumber" ID="displayTime" runat="server"></asp:TextBox> <asp:Label ID="Label4" Text="<%$ Resources:languageResource ,lang_repeatMin %>" runat="server" ></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <asp:Label ID="Label5" Text="<%$ Resources:languageResource ,lang_contents %>" runat="server" ></asp:Label>
                                </th>
                                <td style="vertical-align:bottom;">
                                    <asp:TextBox TextMode="MultiLine" Rows="3" MaxLength="100" ID="contents" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="text-align:right">
                        <%if(idx.Value == "0") {%>
                        <span id="Span1"><button type="submit" class="btn" ><asp:Label ID="Label6" Text="<%$ Resources:languageResource ,lang_btn_ok %>" runat="server" ></asp:Label></button></span>
                        <%}
                          else{
                        %>
                       <span id="buttonGubun">
                           <button type="button" onclick="defaultSubmit('<%=mode.ClientID %>',1)" class="btn" ><asp:Label ID="Label8" Text="<%$ Resources:languageResource ,lang_btn_update %>" runat="server" ></asp:Label></button> 
                           <button type="button"  onclick="defaultSubmit('<%=mode.ClientID %>',2)" class="btn" ><asp:Label ID="Label7" Text="<%$ Resources:languageResource ,lang_btn_delete %>" runat="server" ></asp:Label></button></span>
                        <%} %>
                    </td>
                </tr>
            </table>
    </div>
</asp:Content>