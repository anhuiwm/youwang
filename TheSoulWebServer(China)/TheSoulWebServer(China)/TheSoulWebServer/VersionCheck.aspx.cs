using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Json;

public partial class VersionCheck : System.Web.UI.Page
{
    private SqlConnection DB_common = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        JsonObjectCollection res = new JsonObjectCollection();
        try
        {
            string DBconString = "";
            string savePath = Request.PhysicalApplicationPath;
            TheSoulDBcon.GetCommonDB(savePath, ref DBconString);
            DB_common = new SqlConnection(DBconString);

            //MY ACCOUNTINDEX SEARCH
            DB_common.Open();
            var indexcommand = new SqlCommand { CommandType = CommandType.StoredProcedure, Connection = DB_common, CommandText = "ClientVersionCheck" };
            var outputVersion = new SqlParameter("@VERSION", SqlDbType.VarChar, 8) { Direction = ParameterDirection.Output };
            indexcommand.Parameters.Add(outputVersion);
            indexcommand.ExecuteNonQuery();
            string retClientVersion = System.Convert.ToString(outputVersion.Value);
            DB_common.Close();

            res.Add(new JsonStringValue("clientversion", retClientVersion));
            res.Add(new JsonStringValue("Android", "https://play.google.com/store/apps/details?id=com.mseedgames.ArcaneSoul"));
            res.Add(new JsonStringValue("IOS", "https://itunes.apple.com/us/app/arcanesoul/id892626405?l=ko&ls=1&mt=8"));
            string json_text = res.ToString();
            Response.Write(json_text);
        }
        catch (Exception ex)
        {
            if (DB_common != null)
                DB_common.Close();

            res.Add(new JsonNumericValue("resultcode", 97));
            res.Add(new JsonStringValue("message", ex.Message));
                res.Add(new JsonStringValue("message", ex.ToString()));
            string json_text = res.ToString();
            Response.Write(json_text);
        }
    }
}