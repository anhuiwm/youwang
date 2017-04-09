using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Json;

namespace TheSoulCheatServer
{
    public partial class Soul_Test : System.Web.UI.Page
    {
        private SqlConnection DB_common = null;
        private SqlConnection DB_sharding = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            JsonObjectCollection res = new JsonObjectCollection();
            if (Request["username"] == null || Request["username"] == "")
            {
                res.Add(new JsonNumericValue("resultcode", 1));
                res.Add(new JsonStringValue("message", "Post 값 전달 실패"));
                string json_text = res.ToString();
                Response.Write(json_text);
            }
            else
            {
                try
                {
                    string UserName = System.Convert.ToString(Request.Params["username"]);
                    string str_item399002001 = System.Convert.ToString(Request.Params["399002001"]);
                    string str_item399002002 = System.Convert.ToString(Request.Params["399002002"]);
                    string str_item399002003 = System.Convert.ToString(Request.Params["399002003"]);
                    string str_item399002004 = System.Convert.ToString(Request.Params["399002004"]);
                    string str_item399991001 = System.Convert.ToString(Request.Params["399991001"]);
                    string str_item399991002 = System.Convert.ToString(Request.Params["399991002"]);
                    string str_item399991003 = System.Convert.ToString(Request.Params["399991003"]);
                    string str_item399991004 = System.Convert.ToString(Request.Params["399991004"]);
                    string str_item399991005 = System.Convert.ToString(Request.Params["399991005"]);
                    string str_item399991011 = System.Convert.ToString(Request.Params["399991011"]);
                    string str_item399991012 = System.Convert.ToString(Request.Params["399991012"]);
                    string str_item399991016 = System.Convert.ToString(Request.Params["399991016"]);
                    string str_item399991017 = System.Convert.ToString(Request.Params["399991017"]);
                    string str_item399991018 = System.Convert.ToString(Request.Params["399991018"]);
                    string str_item399991019 = System.Convert.ToString(Request.Params["399991019"]);
                    string str_item399991023 = System.Convert.ToString(Request.Params["399991023"]);
                    string str_item399991024 = System.Convert.ToString(Request.Params["399991024"]);
                    string str_item399991025 = System.Convert.ToString(Request.Params["399991025"]);
                    string str_item399991026 = System.Convert.ToString(Request.Params["399991026"]);
                    string str_item399991027 = System.Convert.ToString(Request.Params["399991027"]);
                    string str_item399991034 = System.Convert.ToString(Request.Params["399991034"]);
                    string str_item399991037 = System.Convert.ToString(Request.Params["399991037"]);
                    string str_item399991038 = System.Convert.ToString(Request.Params["399991038"]);
                    string str_item399991039 = System.Convert.ToString(Request.Params["399991039"]);
                    string str_item399991040 = System.Convert.ToString(Request.Params["399991040"]);
                    string str_item399991041 = System.Convert.ToString(Request.Params["399991041"]);
                    string str_PassiveSoulPoint = System.Convert.ToString(Request.Params["PassiveSoulPoint"]);
                    string str_PassiveSoulEXP = System.Convert.ToString(Request.Params["PassiveSoulEXP"]);

                    int item399002001 = 0;
                    if (!string.IsNullOrEmpty(str_item399002001))
                    {
                        item399002001 = System.Convert.ToInt32(str_item399002001);
                    }
                    int item399002002 = 0;
                    if (!string.IsNullOrEmpty(str_item399002002))
                    {
                        item399002002 = System.Convert.ToInt32(str_item399002002);
                    }
                    int item399002003 = 0;
                    if (!string.IsNullOrEmpty(str_item399002003))
                    {
                        item399002003 = System.Convert.ToInt32(str_item399002003);
                    }
                    int item399002004 = 0;
                    if (!string.IsNullOrEmpty(str_item399002004))
                    {
                        item399002004 = System.Convert.ToInt32(str_item399002004);
                    }
                    int item399991001 = 0;
                    if (!string.IsNullOrEmpty(str_item399991001))
                    {
                        item399991001 = System.Convert.ToInt32(str_item399991001);
                    }
                    int item399991002 = 0;
                    if (!string.IsNullOrEmpty(str_item399991002))
                    {
                        item399991002 = System.Convert.ToInt32(str_item399991002);
                    }
                    int item399991003 = 0;
                    if (!string.IsNullOrEmpty(str_item399991003))
                    {
                        item399991003 = System.Convert.ToInt32(str_item399991003);
                    }
                    int item399991004 = 0;
                    if (!string.IsNullOrEmpty(str_item399991004))
                    {
                        item399991004 = System.Convert.ToInt32(str_item399991004);
                    }
                    int item399991005 = 0;
                    if (!string.IsNullOrEmpty(str_item399991005))
                    {
                        item399991005 = System.Convert.ToInt32(str_item399991005);
                    }
                    int item399991011 = 0;
                    if (!string.IsNullOrEmpty(str_item399991011))
                    {
                        item399991011 = System.Convert.ToInt32(str_item399991011);
                    }
                    int item399991012 = 0;
                    if (!string.IsNullOrEmpty(str_item399991012))
                    {
                        item399991012 = System.Convert.ToInt32(str_item399991012);
                    }
                    int item399991016 = 0;
                    if (!string.IsNullOrEmpty(str_item399991016))
                    {
                        item399991016 = System.Convert.ToInt32(str_item399991016);
                    }
                    int item399991017 = 0;
                    if (!string.IsNullOrEmpty(str_item399991017))
                    {
                        item399991017 = System.Convert.ToInt32(str_item399991017);
                    }
                    int item399991018 = 0;
                    if (!string.IsNullOrEmpty(str_item399991018))
                    {
                        item399991018 = System.Convert.ToInt32(str_item399991018);
                    }
                    int item399991019 = 0;
                    if (!string.IsNullOrEmpty(str_item399991019))
                    {
                        item399991019 = System.Convert.ToInt32(str_item399991019);
                    }
                    int item399991023 = 0;
                    if (!string.IsNullOrEmpty(str_item399991023))
                    {
                        item399991023 = System.Convert.ToInt32(str_item399991023);
                    }
                    int item399991024 = 0;
                    if (!string.IsNullOrEmpty(str_item399991024))
                    {
                        item399991024 = System.Convert.ToInt32(str_item399991024);
                    }
                    int item399991025 = 0;
                    if (!string.IsNullOrEmpty(str_item399991025))
                    {
                        item399991025 = System.Convert.ToInt32(str_item399991025);
                    }
                    int item399991026 = 0;
                    if (!string.IsNullOrEmpty(str_item399991026))
                    {
                        item399991026 = System.Convert.ToInt32(str_item399991026);
                    }
                    int item399991027 = 0;
                    if (!string.IsNullOrEmpty(str_item399991027))
                    {
                        item399991027 = System.Convert.ToInt32(str_item399991027);
                    }
                    int item399991034 = 0;
                    if (!string.IsNullOrEmpty(str_item399991034))
                    {
                        item399991034 = System.Convert.ToInt32(str_item399991034);
                    }
                    int item399991037 = 0;
                    if (!string.IsNullOrEmpty(str_item399991037))
                    {
                        item399991037 = System.Convert.ToInt32(str_item399991037);
                    }
                    int item399991038 = 0;
                    if (!string.IsNullOrEmpty(str_item399991038))
                    {
                        item399991038 = System.Convert.ToInt32(str_item399991038);
                    }
                    int item399991039 = 0;
                    if (!string.IsNullOrEmpty(str_item399991039))
                    {
                        item399991039 = System.Convert.ToInt32(str_item399991039);
                    }
                    int item399991040 = 0;
                    if (!string.IsNullOrEmpty(str_item399991040))
                    {
                        item399991040 = System.Convert.ToInt32(str_item399991040);
                    }
                    int item399991041 = 0;
                    if (!string.IsNullOrEmpty(str_item399991041))
                    {
                        item399991041 = System.Convert.ToInt32(str_item399991041);
                    }
                    int PassiveSoulPoint = 0;
                    if (!string.IsNullOrEmpty(str_PassiveSoulPoint))
                    {
                        PassiveSoulPoint = System.Convert.ToInt32(str_PassiveSoulPoint);
                    }
                    int PassiveSoulEXP = 0;
                    if (!string.IsNullOrEmpty(str_PassiveSoulEXP))
                    {
                        PassiveSoulEXP = System.Convert.ToInt32(str_PassiveSoulEXP);
                    }

                    string DBconString = "";
                    string savePath = Request.PhysicalApplicationPath;
                    TheSoulDBcon.GetInstance().GetIniFileLoad(savePath, ref DBconString);
                    DB_common = new SqlConnection(DBconString);

                    //DB연결 정의
                    string connSharding = "";
                    connSharding = TheSoulDBcon.GetInstance().GetShardingDB(1);
                    DB_sharding = new SqlConnection(connSharding);

                    //DB분산 정보 가져오기
                    DB_sharding.Open();
                    var indexcommand = new SqlCommand { CommandType = CommandType.StoredProcedure, Connection = DB_sharding, CommandText = "GetAccountSearch" };
                    var outputDB_INDEX = new SqlParameter("@RetDB_INDEX", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var outputResultAID = new SqlParameter("@RetAID", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
                    indexcommand.Parameters.Add("@USERNAME", SqlDbType.NVarChar, 32).Value = UserName;
                    indexcommand.Parameters.Add(outputDB_INDEX);
                    indexcommand.Parameters.Add(outputResultAID);
                    indexcommand.ExecuteNonQuery();
                    int retDB_INDEX = int.Parse(outputDB_INDEX.Value.ToString());
                    ulong retAID = System.Convert.ToUInt64(outputResultAID.Value.ToString());
                    DB_sharding.Close();

                    //계정 정보 가져오기
                    DB_sharding.Open();
                    var command = new SqlCommand { CommandType = CommandType.StoredProcedure, Connection = DB_sharding, CommandText = "SoulTestCheat" };
                    var outputResult = new SqlParameter("@Result", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    command.Parameters.Add("@AID", SqlDbType.BigInt).Value = retAID;
                    command.Parameters.Add("@item399002001", SqlDbType.Int).Value = item399002001;
                    command.Parameters.Add("@item399002002", SqlDbType.Int).Value = item399002002;
                    command.Parameters.Add("@item399002003", SqlDbType.Int).Value = item399002003;
                    command.Parameters.Add("@item399002004", SqlDbType.Int).Value = item399002004;
                    command.Parameters.Add("@item399991001", SqlDbType.Int).Value = item399991001;
                    command.Parameters.Add("@item399991002", SqlDbType.Int).Value = item399991002;
                    command.Parameters.Add("@item399991003", SqlDbType.Int).Value = item399991003;
                    command.Parameters.Add("@item399991004", SqlDbType.Int).Value = item399991004;
                    command.Parameters.Add("@item399991005", SqlDbType.Int).Value = item399991005;
                    command.Parameters.Add("@item399991011", SqlDbType.Int).Value = item399991011;
                    command.Parameters.Add("@item399991012", SqlDbType.Int).Value = item399991012;
                    command.Parameters.Add("@item399991016", SqlDbType.Int).Value = item399991016;
                    command.Parameters.Add("@item399991017", SqlDbType.Int).Value = item399991017;
                    command.Parameters.Add("@item399991018", SqlDbType.Int).Value = item399991018;
                    command.Parameters.Add("@item399991019", SqlDbType.Int).Value = item399991019;
                    command.Parameters.Add("@item399991023", SqlDbType.Int).Value = item399991023;
                    command.Parameters.Add("@item399991024", SqlDbType.Int).Value = item399991024;
                    command.Parameters.Add("@item399991025", SqlDbType.Int).Value = item399991025;
                    command.Parameters.Add("@item399991026", SqlDbType.Int).Value = item399991026;
                    command.Parameters.Add("@item399991027", SqlDbType.Int).Value = item399991027;
                    command.Parameters.Add("@item399991034", SqlDbType.Int).Value = item399991034;
                    command.Parameters.Add("@item399991037", SqlDbType.Int).Value = item399991037;
                    command.Parameters.Add("@item399991038", SqlDbType.Int).Value = item399991038;
                    command.Parameters.Add("@item399991039", SqlDbType.Int).Value = item399991039;
                    command.Parameters.Add("@item399991040", SqlDbType.Int).Value = item399991040;
                    command.Parameters.Add("@item399991041", SqlDbType.Int).Value = item399991041;
                    command.Parameters.Add("@PassiveSoulPoint", SqlDbType.Int).Value = PassiveSoulPoint;
                    command.Parameters.Add("@PassiveSoulEXP", SqlDbType.Int).Value = PassiveSoulEXP;
                    command.Parameters.Add(outputResult);
                    command.ExecuteNonQuery();
                    int retCode = int.Parse(outputResult.Value.ToString());
                    DB_sharding.Close();

                    res.Add(new JsonNumericValue("resultcode", retCode));
                    string json_text = res.ToString();
                    Response.Write(json_text);
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }
            }
        }
    }
}