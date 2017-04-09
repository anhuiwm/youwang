<%@ Page MasterPageFile="~/Main.Master" Language="C#" AutoEventWireup="true" CodeBehind="blackMarketMain.aspx.cs" Inherits="TheSoulGMTool.management.blackMarketMain" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="span9">
        <table class="table table-bordered">
            <tr>
                <th>
                    <asp:Label ID="lang_server" Text="<%$ Resources:languageResource ,lang_server %>" runat="server"></asp:Label></th>
                <td><span id="change_server" runat="server"></span></td>
            </tr>
        </table>
        <table class="table table-bordered">
            <tbody>
                <tr>
                    <th><asp:Label ID="Label6" Text="<%$ Resources:languageResource,lang_resetCash %>" runat="server"></asp:Label></th>
                    <td><asp:TextBox TextMode="Number" CssClass="onlyNumber" ID="resetCash" runat="server"></asp:TextBox></td>
                    <td>
                        <button type="submit" class="btn"><asp:Label ID="Label7" Text="<%$ Resources:languageResource ,lang_btn_update %>" runat="server" ></asp:Label></button>
                    </td>
                </tr>
            </tbody>
        </table>
        <table class="table table-bordered">
            <tbody>
                <tr>
                    <th><asp:Label ID="Label8" Text="<%$ Resources:languageResource,lang_slotID %>" runat="server"></asp:Label></th>
                    <th><asp:Label ID="Label9" Text="<%$ Resources:languageResource,lang_slotName %>" runat="server"></asp:Label></th>
                    <th><asp:Label ID="Label10" Text="<%$ Resources:languageResource,lang_btn_manage %>" runat="server"></asp:Label></th>
                </tr>
                <tr>
                    <th>1</th>
                    <td><asp:Label ID="Label11" Text="<%$ Resources:languageResource,langslotID1 %>" runat="server"></asp:Label></td>
                    <td>
                        <button type="button" class="btn" onclick="golink('blackmarketList.aspx','?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>&slotid=1')"><asp:Label ID ="Label1" Text="<%$ Resources:languageResource ,lang_btn_manage %>" runat="server"></asp:Label></button>
                    </td>
                </tr>
                <tr>
                    <th>2</th>
                    <td><asp:Label ID="Label12" Text="<%$ Resources:languageResource,langslotID2 %>" runat="server"></asp:Label></td>
                    <td>
                        <button type="button" class="btn" onclick="golink('blackmarketList.aspx','?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>&slotid=2')"><asp:Label ID ="Label2" Text="<%$ Resources:languageResource ,lang_btn_manage %>" runat="server"></asp:Label></button>
                    </td>
                </tr>
                <tr>
                    <th>3</th>
                    <td><asp:Label ID="Label13" Text="<%$ Resources:languageResource,langslotID3 %>" runat="server"></asp:Label></td>
                    <td>
                        <button type="button" class="btn" onclick="golink('blackmarketList.aspx','?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>&slotid=3')"><asp:Label ID ="Label3" Text="<%$ Resources:languageResource ,lang_btn_manage %>" runat="server"></asp:Label></button>
                    </td>
                </tr>
                <tr>
                    <th>4</th>
                    <td><asp:Label ID="Label14" Text="<%$ Resources:languageResource,langslotID4 %>" runat="server"></asp:Label></td>
                    <td>
                        <button type="button" class="btn" onclick="golink('blackmarketList.aspx','?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>&slotid=4')"><asp:Label ID ="Label4" Text="<%$ Resources:languageResource ,lang_btn_manage %>" runat="server"></asp:Label></button>
                    </td>
                </tr>
                <tr>
                    <th>5</th>
                    <td><asp:Label ID="Label15" Text="<%$ Resources:languageResource,langslotID5 %>" runat="server"></asp:Label></td>
                    <td>
                        <button type="button" class="btn" onclick="golink('blackmarketList.aspx','?ca2=<%=Master.ca2 %>&select_server=<%=Master.serverID%>&slotid=5')"><asp:Label ID ="Label5" Text="<%$ Resources:languageResource ,lang_btn_manage %>" runat="server"></asp:Label></button>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</asp:Content>