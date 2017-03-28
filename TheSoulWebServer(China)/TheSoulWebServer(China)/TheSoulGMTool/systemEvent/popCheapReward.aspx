<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="popCheapReward.aspx.cs" Inherits="TheSoulGMTool.systemEvent.popCheapReward" %>

<html lang="en">
<head>
    <meta charset="utf-8">
    <title>Operational Tool</title>


    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta name="description" content="" />
    <meta name="author" content="" />
    <!-- Le styles -->
    <link href="/css/bootstrap.css" rel="stylesheet" />
    <style type="text/css">
        body {
            padding-top: 60px;
            padding-bottom: 40px;
        }

        .sidebar-nav {
            padding: 9px 0;
        }

        .nav .active {
            font-weight: bold;
        }

        .row-fluid .span4 {
            min-height: 130px;
        }

        h3 {
            margin-top: 30px;
        }
    </style>
    <link rel="stylesheet" type="text/css" href="/css/jquery.datepick.css" />
    <link href="/css/bootstrap-responsive.css" rel="stylesheet" />
</head>
    <body>
        <form id="form1" runat="server">
            <div>
                <h3>
                    <asp:Label ID="Label1" Text="<%$ Resources:languageResource ,lang_1yuanReward %>" runat="server"></asp:Label></h3>
                <br />
                <br />
                <asp:GridView ID="dataList" CssClass="table table-bordered" runat="server" AutoGenerateColumns="false">
                    <columns>
                        <asp:BoundField DataField="Item_TargetType" HeaderText="<%$ Resources:languageResource ,lang_Item %>" />
                        <asp:BoundField DataField="Item_Grade" HeaderText="<%$ Resources:languageResource ,lang_grade %>" />
                        <asp:BoundField DataField="Item_Num" HeaderText="<%$ Resources:languageResource ,lang_count %>" />
                    </columns>
                </asp:GridView>
                <h3>
                    <asp:Label ID="Label2" Text="<%$ Resources:languageResource ,lang_3yuanReward %>" runat="server"></asp:Label></h3>
                <br />
                <br />
                <asp:GridView ID="GridView1" CssClass="table table-bordered" runat="server" AutoGenerateColumns="false">
                    <columns>
                        <asp:BoundField DataField="Item_TargetType" HeaderText="<%$ Resources:languageResource ,lang_Item %>" />
                        <asp:BoundField DataField="Item_Grade" HeaderText="<%$ Resources:languageResource ,lang_grade %>" />
                        <asp:BoundField DataField="Item_Num" HeaderText="<%$ Resources:languageResource ,lang_count %>" />
                    </columns>
                </asp:GridView>
                <button type="button" class="btn" onclick="self.close">
                    <asp:Label ID="Label3" Text="<%$ Resources:languageResource ,lang_close %>" runat="server"></asp:Label></button>
            </div>
        </form>
    </body>
</html>
