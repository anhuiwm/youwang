<%@ Page MasterPageFile="~/Main.Master" Language="C#" AutoEventWireup="true" CodeBehind="globalnotice_form.aspx.cs" Inherits="TheSoulGMTool.management.globalnotice_form" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <script type="text/javascript" src="/ckeditor/ckeditor.js"></script>
    <div class="span9">
        <table border="0" style="background-color:#ffffff;width:100%">
                <tr>
                    <td>
                        <asp:HiddenField ID="idx" runat="server" />
                        <asp:HiddenField ID="mode" runat="server" />
                        <table class="table table-bordered">
                            <tr>
                                <th>PlatformType</th>
                                <td>
                                    <asp:CheckBoxList ID="platformType" CssClass="table-unbordered" RepeatDirection="Horizontal" runat="server">
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                            <tr>
                                <th>Server Version</th>
                                <td>
                                    <asp:TextBox ID="Version1" Width="50" MaxLength="2" runat="server"></asp:TextBox>.
                                    <asp:TextBox ID="Version2" Width="50" MaxLength="2" runat="server"></asp:TextBox>.
                                    <asp:TextBox ID="Version3" Width="50" MaxLength="2" runat="server"></asp:TextBox>.
                                    <asp:TextBox ID="Version4" Width="50" MaxLength="3" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th><asp:Label ID="Label1" Text="<%$ Resources:languageResource ,lang_noticeTag %>" runat="server" ></asp:Label></th>
                                <td>
                                    <asp:DropDownList ID="notice_tag" runat="server"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <th><asp:Label ID="Label2" Text="<%$ Resources:languageResource ,lang_noticeType %>" runat="server" ></asp:Label></th>
                                <td>
                                    <asp:DropDownList ID="notice_type" runat="server"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <th><asp:Label ID="Label3" Text="<%$ Resources:languageResource ,lang_on %>" runat="server" ></asp:Label>/<asp:Label ID="Label4" Text="<%$ Resources:languageResource ,lang_off %>" runat="server" ></asp:Label></th>
                                <td>
                                    <asp:RadioButtonList CssClass="table-unbordered" ID="notice_active" RepeatDirection="Horizontal" runat="server">
                                        <asp:ListItem Value="0">X</asp:ListItem>
                                        <asp:ListItem Value="1" Selected="True">O</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <th><asp:Label ID="Label5" Text="<%$ Resources:languageResource ,lang_sortNum %>" runat="server" ></asp:Label></th>
                                <td>
                                    <asp:TextBox TextMode="Number" CssClass="onlyNumber" ID="ordernum" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th><asp:Label ID="Label6" Text="<%$ Resources:languageResource ,lang_noticeDate %>" runat="server" ></asp:Label></th>
                                <td style="vertical-align:middle;">
                                    <asp:TextBox TextMode="Date" ID="sDate" Width="130" runat="server"></asp:TextBox> <asp:DropDownList Width="80" ID="sHour" runat="server"></asp:DropDownList> <asp:DropDownList ID="sMin" Width="80" runat="server"></asp:DropDownList> ~
                                    <asp:TextBox TextMode="Date" ID="eDate" Width="130" runat="server"></asp:TextBox> <asp:DropDownList Width="80" ID="eHour" runat="server"></asp:DropDownList> <asp:DropDownList ID="eMin" Width="80" runat="server"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <th><asp:Label ID="Label7" Text="<%$ Resources:languageResource ,lang_title %>" runat="server" ></asp:Label></th>
                                <td>
                                    <asp:TextBox ID="title" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <CKEditor:CKEditorControl ID="ctlCkeditor" runat="server"></CKEditor:CKEditorControl> 
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="text-align:right">
                        <%if(idx.Value == "0") {%>
                        <span id="Span1"><button type="button" onclick="defaultSubmit('<%=mode.ClientID %>','0')"  class="btn" ><asp:Label ID="Label8" Text="<%$ Resources:languageResource ,lang_btn_ok %>" runat="server" ></asp:Label></button></span>
                        <%}
                          else{
                        %>
                       <span id="buttonGubun">
                           <button type="button" onclick="defaultSubmit('<%=mode.ClientID %>',1)" class="btn" ><asp:Label ID="Label9" Text="<%$ Resources:languageResource ,lang_btn_update %>" runat="server" ></asp:Label></button> 
                           <button type="button"  onclick="defaultSubmit('<%=mode.ClientID %>',2)" class="btn" ><asp:Label ID="Label10" Text="<%$ Resources:languageResource ,lang_btn_delete %>" runat="server" ></asp:Label></button></span>
                        <%} %>
                    </td>
                </tr>
            </table>

    </div>
</asp:Content>