<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pop_itemCharge.aspx.cs" Inherits="TheSoulGMTool.User.pop_itemCharge" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
            <h3><%=HttpContext.GetGlobalResourceObject("languageResource","lang_Item") %></h3>
            <br />
            <table class="table table-bordered">
                <tr>
                    <th><%=HttpContext.GetGlobalResourceObject("languageResource","lang_Item") %> ID</th>
                    <td><asp:TextBox ID="itemid" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <th><%=HttpContext.GetGlobalResourceObject("languageResource","lang_grade") %></th>
                    <td><asp:TextBox TextMode="Number" CssClass="onlyNumber" ID="grade" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <th><%=HttpContext.GetGlobalResourceObject("languageResource","lang_level") %></th>
                    <td><asp:TextBox TextMode="Number" CssClass="onlyNumber" ID="level" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <th><%=HttpContext.GetGlobalResourceObject("languageResource","lang_count") %></th>
                    <td><asp:TextBox TextMode="Number" CssClass="onlyNumber" ID="itema" runat="server"></asp:TextBox></td>
                </tr>
            </table>
            <button type="submit" class="btn"><%=HttpContext.GetGlobalResourceObject("languageResource","lang_btn_ok") %></button>
        </div>
    </form>
</body>
</html>
