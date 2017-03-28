using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using mSeed.RedisManager;
using mSeed.mDBTxnBlock;
using System.Data.SqlClient;
using System.Data;
using TheSoul.DataManager;
using TheSoul.DataManager.DBClass;
using TheSoul.DataManager.Tools;
using TheSoul.DataManager.Global;
using TheSoulWebServer.Tools;
using TheSoulGMTool.DBClass;

namespace TheSoulGMTool.User
{
    public partial class characterRanking : System.Web.UI.Page
    {
        protected override void InitializeCulture()
        {
            UICulture = GMDataManager.GetGmToolWebLanguageCode();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            WebQueryParam queryFetcher = new WebQueryParam(true);
            string userName = queryFetcher.QueryParam_Fetch(username.UniqueID, "");
            if (!Page.IsPostBack || !string.IsNullOrEmpty(userName))
                GetRankingList(1);
        }

        protected void GetRankingList(int pageIndex)
        {
            WebQueryParam queryFetcher = new WebQueryParam(true);
            queryFetcher.bDBLog = true;
            string retJson = "";
            long serverID = queryFetcher.QueryParam_FetchLong("select_server", 1);

            TxnBlock tb = new TxnBlock();
            {
                try
                {
                    GMDataManager.GetServerinit(ref tb, serverID);
                    string userName = queryFetcher.QueryParam_Fetch(username.UniqueID, "");
                    long deleteID = queryFetcher.QueryParam_FetchLong(userid.UniqueID);
                    int deleteClass = queryFetcher.QueryParam_FetchInt(classType.UniqueID);
                    long AID = 0;
                    long CID = 0;

                    int pageSize = 15;
                    int startNum = 0;
                    int endNum = pageSize - 1;

                    Result_Define.eResult retError = Result_Define.eResult.SUCCESS;

                    if (deleteID > 0 && deleteClass > 0)
                    {
                        List<Character> characterList = CharacterManager.GetCharacterList(ref tb, deleteID);
                        CID = characterList.Find(item => item.Class == deleteClass) == null ? 0 : characterList.Find(item => item.Class == deleteClass).cid;
                        if (CID > 0)
                        {
                            PvP_WarPoint setWarPoint = CharacterManager.GetCharacterWarpoint(ref tb, CID);
                            retError = CharacterManager.UpdateCharacterWarpoint(ref tb, deleteID, CID, 0);
                            if (retError == Result_Define.eResult.SUCCESS)
                                retError = GMDataManager.InsertGMControlLog(ref tb, GMResult_Define.TargetType.GAME_USER, deleteID, AccountManager.GetSimpleAccountInfo(ref tb, AID).username, GMResult_Define.ControlType.RANKING_DELETE, queryFetcher.GetReqParams(), serverID.ToString());
                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                PvPManager.SetUser_PvP_Warpoint(deleteID, CID, 0);
                                userid.Value = string.Empty;
                                classType.Value = string.Empty;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(userName))
                    {
                        AID = GMDataManager.GetSearchAID_BYUserName(ref tb, userName);
                    }
                    
                    long totalPlayer = 0;
                    if (AID > 0)
                    {
                        
                        List<Ret_PvP> rankList = new List<Ret_PvP>();
                        List<Character> characterList = CharacterManager.GetCharacterList(ref tb, AID);
                        characterList.ForEach(item =>
                        {
                            PvP_WarPoint setWarPoint = CharacterManager.GetCharacterWarpoint(ref tb, item.cid, true);
                            rankList.Add(PvPManager.GetUser_PvP_Warpoint_Rank_Info(ref tb, ref setWarPoint, AID, ref totalPlayer));
                        });
                        dataList.DataSource = rankList;
                        dataList.DataBind();
                        GMDataManager.PopulatePager(ref dlPager, totalPlayer, 1);
                    }
                    else
                    {
                        PvP_WarPoint setWarPoint = CharacterManager.GetCharacterWarpoint(ref tb, CID);
                        Ret_PvP my_PvP_Rank = PvPManager.GetUser_PvP_Warpoint_Rank_Info(ref tb, ref setWarPoint, AID, ref totalPlayer);
                        
                        if (pageIndex > 1) 
                        {
                            startNum = (pageIndex - 1) * pageSize;
                            endNum = (pageIndex * pageSize) - 1;
                        }
                        if (endNum > (totalPlayer - 1))
                            endNum = (int)totalPlayer - 1;

                        List<Ret_PvP> rankList = PvPManager.GetUser_PvP_Warpoint_Rank_List(ref tb, startNum, endNum).OrderBy(item => item.rank).ToList();
                        dataList.DataSource = rankList;
                        dataList.DataBind();
                        GMDataManager.PopulatePager(ref dlPager, totalPlayer, pageIndex);
                    }
                    queryFetcher.GM_Render(retError);
                }
                catch (Exception errorEx)
                {
                    queryFetcher.DBLog("StackTrace" + mJsonSerializer.ToJsonString(errorEx.StackTrace));
                    queryFetcher.DBLog(errorEx.Message);
                    retJson = queryFetcher.Render<ErrorReturnString>(new ErrorReturnString(errorEx.Message), Result_Define.eResult.System_Exception);
                }
                finally
                {

                    tb.EndTransaction(queryFetcher.Render_errorFlag);
                    string gmid = "";
                    if (Request.Cookies.Count > 0)
                        gmid = GMDataManager.GetUserCookies("userid");
                    queryFetcher.GMToolLogToDB(ref tb, gmid, GMData_Define.GmDBName);
                    tb.Dispose();
                }
            }
        }
        
        protected void dlPager_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "PageNo")
            {
                int page = System.Convert.ToInt32(e.CommandArgument);
                this.GetRankingList(page);
            }
        }
    }
}