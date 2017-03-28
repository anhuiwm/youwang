<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="popReward.aspx.cs" Inherits="TheSoulGMTool.systemEvent.popReward" %>

<html lang="en">
<head>
    <meta charset="utf-8">
    <title>Operational Tool</title>


    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="">
    <meta name="author" content="">
    <!-- Le styles -->
    <link href="/css/bootstrap.css" rel="stylesheet">
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
    <link rel="stylesheet" type="text/css" href="/css/jquery.datepick.css">
    <link href="/css/bootstrap-responsive.css" rel="stylesheet">
    <body>
        <form id="form1" runat="server">
            <div>
                <h3><asp:Label ID="Label1" Text="<%$ Resources:languageResource ,lang_viewReward %>" runat="server" ></asp:Label></h3>
                <br />
                <br />
                <table class="table table-bordered">
                    <tr>
                        <th><asp:Label ID="Label2" Text="<%$ Resources:languageResource ,lang_reward %>" runat="server" ></asp:Label></th>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="labReward" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <button type="button" class="btn" onclick="self.close"><asp:Label ID="Label3" Text="<%$ Resources:languageResource ,lang_close %>" runat="server" ></asp:Label></button>
            </div>
        </form>
    </body>
</html>
