<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="paylog.aspx.cs" Inherits="TheSoulGMTool.User.paylog" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
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
    <!-- HTML5 shim, for IE6-8 support of HTML5 elements -->
    <!--[if lt IE 9]>
<script src="http://html5shim.googlecode.com/svn/trunk/html5.js"></script>
<![endif]-->
    <!-- Fav and touch icons -->
    <link rel="apple-touch-icon-precomposed" sizes="144x144" href="ico/apple-touch-icon-144-precomposed.png" />
    <link rel="apple-touch-icon-precomposed" sizes="114x114" href="ico/apple-touch-icon-114-precomposed.png" />
    <link rel="apple-touch-icon-precomposed" sizes="72x72" href="ico/apple-touch-icon-72-precomposed.png" />
    <link rel="apple-touch-icon-precomposed" href="ico/apple-touch-icon-57-precomposed.png" />
    <link rel="shortcut icon" href="ico/favicon.png" />
    <!--script src="/js/jquery.js"></script-->
    <script src="/js/jquery-1.11.2.min.js"></script>
    <script src="/js/jquery-migrate-1.2.1.min.js"></script>
    <script src="/js/jquery.datepick.js"></script>
    <script src="/js/jquery.datepick-ko.js" charset="utf-8"></script>
    <script src="/js/ui.js"></script>
    <script src="/js/jquery-ui.js"></script>
    <script src="/js/form.js"></script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h3>결제 상세정보</h3>
            <br />
            <table class="table table-bordered">
                <tr>
                    <th>AID</th>
                    <td><asp:Label id="AID" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <th>CID</th>
                    <td><asp:Label ID="cid" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <th>Billing Token</th>
                    <td><asp:Label ID="uid" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <th>Platform Type</th>
                    <td><asp:Label ID="platform" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <th>Reg Date</th>
                    <td><asp:Label ID="regdate" runat="server"></asp:Label></td>
                </tr>
            </table>
            <button type="button" class="btn" onclick="self.close();"><%=HttpContext.GetGlobalResourceObject("languageResource","lang_btn_ok") %></button>
        </div>
    </form>
</body>
</html>