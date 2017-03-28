<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="sendReward.aspx.cs" Inherits="TheSoulGMTool.kr.sendReward" %>
<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="con" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        날짜 입력 형식 2016-08-19 13:59<br />
        기간:<asp:TextBox ID="start" runat="server"></asp:TextBox>~<asp:TextBox ID="end" runat="server"></asp:TextBox>
        <button type="submit">보상보내기</button>
</asp:Content>