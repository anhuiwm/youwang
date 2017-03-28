<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cheat_openTime.aspx.cs" Inherits="TheSoulCheatServer.cheat_openTime" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        BossRaid Rate <asp:Label ID="bossRate" runat="server"></asp:Label>&nbsp;&nbsp;<input type="text" name="bossraid" size="20" /><br /><br />
        Gold Expedition Matching <asp:Label ID="Label1" runat="server"></asp:Label>&nbsp;&nbsp;<input type="text" name="matching" size="20" /><br /><br />
        Free PVP 1st Time : <select name="pvp_start_hour1">
                    <option value="">select</option>
                    <% 
                        for (int i = 0; i <= 24; i++)
                        {
                            if(i.ToString().Length == 1)
                                Response.Write("<option value=\"0" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                            else
                                Response.Write("<option value=\"" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                        }
                    %>
                    </select>Hour
                    <select name="pvp_start_min1">
                    <option value="">select</option>
                    <% 
                        for (int i = 0; i <= 59; i++)
                        {
                            if (i.ToString().Length == 1)
                                Response.Write("<option value=\"0" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                            else
                                Response.Write("<option value=\"" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                        }
                    %>
                    </select>Min ~ 
                    <select name="pvp_end_hour1">
                    <option value="">select</option>
                    <% 
                        for (int i = 0; i <= 24; i++)
                        {
                            if (i.ToString().Length == 1)
                                Response.Write("<option value=\"0" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                            else
                                Response.Write("<option value=\"" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                        }
                    %>
                    </select>Hour
                    <select name="pvp_end_min1">
                    <option value="">select</option>
                    <% 
                        for (int i = 0; i <= 59; i++)
                        {
                            if (i.ToString().Length == 1)
                                Response.Write("<option value=\"0" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                            else
                                Response.Write("<option value=\"" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                        }
                    %>
                    </select>Min<br />
        Free PVP 2nd Time : <select name="pvp_start_hour2">
                    <option value="">select</option>
                    <% 
                        for (int i = 0; i <= 24; i++)
                        {
                            if (i.ToString().Length == 1)
                                Response.Write("<option value=\"0" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                            else
                                Response.Write("<option value=\"" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                        }
                    %>
                    </select>Hour
                    <select name="pvp_start_min2">
                    <option value="">select</option>
                    <% 
                        for (int i = 0; i <= 59; i++)
                        {
                            if (i.ToString().Length == 1)
                                Response.Write("<option value=\"0" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                            else
                                Response.Write("<option value=\"" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                        }
                    %>
                    </select>Min ~ 
                    <select name="pvp_end_hour2">
                    <option value="">select</option>
                    <% 
                        for (int i = 0; i <= 24; i++)
                        {
                            if (i.ToString().Length == 1)
                                Response.Write("<option value=\"0" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                            else
                                Response.Write("<option value=\"" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                        }
                    %>
                    </select>Hour
                    <select name="pvp_end_min2">
                    <option value="">select</option>
                    <% 
                        for (int i = 0; i <= 59; i++)
                        {
                            if (i.ToString().Length == 1)
                                Response.Write("<option value=\"0" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                            else
                                Response.Write("<option value=\"" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                        }
                    %>
                    </select>Min<br /><br />
        Gladiator 1st Time : <select name="rubyPVP_start_hour1">
                    <option value="">select</option>
                    <% 
                        for (int i = 0; i <= 24; i++)
                        {
                            if (i.ToString().Length == 1)
                                Response.Write("<option value=\"0" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                            else
                                Response.Write("<option value=\"" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                        }
                    %>
                    </select>Hour
                    <select name="rubyPVP_start_min1">
                    <option value="">select</option>
                    <% 
                        for (int i = 0; i <= 59; i++)
                        {
                            if (i.ToString().Length == 1)
                                Response.Write("<option value=\"0" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                            else
                                Response.Write("<option value=\"" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                        }
                    %>
                    </select>Min ~ 
                    <select name="rubyPVP_end_hour1">
                    <option value="">select</option>
                    <% 
                        for (int i = 0; i <= 24; i++)
                        {
                            if (i.ToString().Length == 1)
                                Response.Write("<option value=\"0" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                            else
                                Response.Write("<option value=\"" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                        }
                    %>
                    </select>Hour
                    <select name="rubyPVP_end_min1">
                    <option value="">select</option>
                    <% 
                        for (int i = 0; i <= 59; i++)
                        {
                            if (i.ToString().Length == 1)
                                Response.Write("<option value=\"0" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                            else
                                Response.Write("<option value=\"" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                        }
                    %>
                    </select>Min<br />
        Gladiator 2nd Time : <select name="rubyPVP_start_hour2">
                    <option value="">select</option>
                    <% 
                        for (int i = 0; i <= 24; i++)
                        {
                            if (i.ToString().Length == 1)
                                Response.Write("<option value=\"0" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                            else
                                Response.Write("<option value=\"" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                        }
                    %>
                    </select>Hour
                    <select name="rubyPVP_start_min2">
                    <option value="">select</option>
                    <% 
                        for (int i = 0; i <= 59; i++)
                        {
                            if (i.ToString().Length == 1)
                                Response.Write("<option value=\"0" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                            else
                                Response.Write("<option value=\"" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                        }
                    %>
                    </select>Min ~ 
                    <select name="rubyPVP_end_hour2">
                    <option value="">select</option>
                    <% 
                        for (int i = 0; i <= 24; i++)
                        {
                            if (i.ToString().Length == 1)
                                Response.Write("<option value=\"0" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                            else
                                Response.Write("<option value=\"" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                        }
                    %>
                    </select>Hour
                    <select name="rubyPVP_end_min2">
                    <option value="">select</option>
                    <% 
                        for (int i = 0; i <= 59; i++)
                        {
                            if (i.ToString().Length == 1)
                                Response.Write("<option value=\"0" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                            else
                                Response.Write("<option value=\"" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                        }
                    %>
                    </select>Min<br /><br />
        1VS1 PVP Open Time : <select name="1vs1PVP_start_hour1">
                    <option value="">select</option>
                    <% 
                        for (int i = 0; i <= 24; i++)
                        {
                            if (i.ToString().Length == 1)
                                Response.Write("<option value=\"0" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                            else
                                Response.Write("<option value=\"" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                        }
                    %>
                    </select>Hour
                    <select name="1vs1PVP_start_min1">
                    <option value="">select</option>
                    <% 
                        for (int i = 0; i <= 59; i++)
                        {
                            if (i.ToString().Length == 1)
                                Response.Write("<option value=\"0" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                            else
                                Response.Write("<option value=\"" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                        }
                    %>
                    </select>Min ~ 
                    <select name="1vs1PVP_end_hour1">
                    <option value="">select</option>
                    <% 
                        for (int i = 0; i <= 24; i++)
                        {
                            if (i.ToString().Length == 1)
                                Response.Write("<option value=\"0" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                            else
                                Response.Write("<option value=\"" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                        }
                    %>
                    </select>Hour
                    <select name="1vs1PVP_end_min1">
                    <option value="">select</option>
                    <% 
                        for (int i = 0; i <= 59; i++)
                        {
                            if (i.ToString().Length == 1)
                                Response.Write("<option value=\"0" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                            else
                                Response.Write("<option value=\"" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                        }
                    %>
                    </select>Min<br />
        1VS! PVP Bonus Time : <select name="1vs1PVP_start_hour2">
                    <option value="">select</option>
                    <% 
                        for (int i = 0; i <= 24; i++)
                        {
                            if (i.ToString().Length == 1)
                                Response.Write("<option value=\"0" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                            else
                                Response.Write("<option value=\"" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                        }
                    %>
                    </select>Hour
                    <select name="1vs1PVP_start_min2">
                    <option value="">select</option>
                    <% 
                        for (int i = 0; i <= 59; i++)
                        {
                            if (i.ToString().Length == 1)
                                Response.Write("<option value=\"0" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                            else
                                Response.Write("<option value=\"" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                        }
                    %>
                    </select>Min ~ 
                    <select name="1vs1PVP_end_hour2">
                    <option value="">select</option>
                    <% 
                        for (int i = 0; i <= 24; i++)
                        {
                            if (i.ToString().Length == 1)
                                Response.Write("<option value=\"0" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                            else
                                Response.Write("<option value=\"" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                        }
                    %>
                    </select>Hour
                    <select name="1vs1PVP_end_min2">
                    <option value="">select</option>
                    <% 
                        for (int i = 0; i <= 59; i++)
                        {
                            if (i.ToString().Length == 1)
                                Response.Write("<option value=\"0" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                            else
                                Response.Write("<option value=\"" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                        }
                    %>
                    </select>Min<br />
        GuildWar Open Time : <select name="guild_start_hour1">
                    <option value="">select</option>
                    <% 
                        for (int i = 0; i <= 24; i++)
                        {
                            if (i.ToString().Length == 1)
                                Response.Write("<option value=\"0" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                            else
                                Response.Write("<option value=\"" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                        }
                    %>
                    </select>Hour
                    <select name="guild_start_min1">
                    <option value="">select</option>
                    <% 
                        for (int i = 0; i <= 59; i++)
                        {
                            if (i.ToString().Length == 1)
                                Response.Write("<option value=\"0" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                            else
                                Response.Write("<option value=\"" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                        }
                    %>
                    </select>Min ~ 
                    <select name="guild_end_hour1">
                    <option value="">select</option>
                    <% 
                        for (int i = 0; i <= 24; i++)
                        {
                            if (i.ToString().Length == 1)
                                Response.Write("<option value=\"0" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                            else
                                Response.Write("<option value=\"" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                        }
                    %>
                    </select>Hour
                    <select name="guild_end_min1">
                    <option value="">select</option>
                    <% 
                        for (int i = 0; i <= 59; i++)
                        {
                            if (i.ToString().Length == 1)
                                Response.Write("<option value=\"0" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                            else
                                Response.Write("<option value=\"" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                        }
                    %>
                    </select>Min<br />
        GuildWar Bonus Time : <select name="guild_start_hour2">
                    <option value="">select</option>
                    <% 
                        for (int i = 0; i <= 24; i++)
                        {
                            if (i.ToString().Length == 1)
                                Response.Write("<option value=\"0" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                            else
                                Response.Write("<option value=\"" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                        }
                    %>
                    </select>Hour
                    <select name="guild_start_min2">
                    <option value="">select</option>
                    <% 
                        for (int i = 0; i <= 59; i++)
                        {
                            if (i.ToString().Length == 1)
                                Response.Write("<option value=\"0" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                            else
                                Response.Write("<option value=\"" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                        }
                    %>
                    </select>Min ~ 
                    <select name="guild_end_hour2">
                    <option value="">select</option>
                    <% 
                        for (int i = 0; i <= 24; i++)
                        {
                            if (i.ToString().Length == 1)
                                Response.Write("<option value=\"0" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                            else
                                Response.Write("<option value=\"" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                        }
                    %>
                    </select>Hour
                    <select name="guild_end_min2">
                    <option value="">select</option>
                    <% 
                        for (int i = 0; i <= 59; i++)
                        {
                            if (i.ToString().Length == 1)
                                Response.Write("<option value=\"0" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                            else
                                Response.Write("<option value=\"" + i.ToString() + "\">" + i.ToString() + "</option>\n");
                        }
                    %>
                    </select>Min<br />
        <input type="Submit" value=" Submit "/>
    </div>
    </form>
</body>
</html>
