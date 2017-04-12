using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using mSeed.Common;
using mSeed.mDBTxnBlock;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;


namespace mSeed.Platform
{
    public partial class UserManager
    {
        private static user_guest_auth_id GetGuestUserID(ref TxnBlock TB, string acc_token, string auth_token)
        {
            user_guest_auth_id retObj = null;
            bool bMake = string.IsNullOrEmpty(acc_token);

            string query = bMake ?
                                string.Format(@"SELECT TOP 1 * FROM {0} WITH(NOLOCK) WHERE client_auth_token = @p2", DB_Define.DBTables[DB_Define.eDBTables.user_guest_auth_id]) :
                                string.Format(@"SELECT * FROM {0} WITH(NOLOCK) WHERE auth_md5_id = @p1 AND client_auth_token = @p2", DB_Define.DBTables[DB_Define.eDBTables.user_guest_auth_id]);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = query;
            if (!bMake)
                cmd.Parameters.AddWithValue("@p1", acc_token);
            cmd.Parameters.AddWithValue("@p2", auth_token);
            retObj = GenericFetch.FetchFromDB<user_guest_auth_id>(ref TB, cmd);
            cmd.Dispose();
            return retObj;
        }

        private static user_guest_auth_id CreateGuestUser(ref TxnBlock TB, string acc_token, string auth_token, mSeed.Common.ePlatformType platform_type = ePlatformType.EPlatformType_Guest)
        {
            user_guest_auth_id retObj = null;
            using (MD5 md5Hash = MD5.Create())
            {
                string server_token = platform_type == mSeed.Common.ePlatformType.EPlatformType_Guest_Editer ? "editer_token" : Convert.ToBase64String(Guid.NewGuid().ToByteArray());

                // TODO : remove after test
                if (string.IsNullOrEmpty(auth_token))
                    auth_token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                string server_md5 = MD5Tool.GetMd5Hash(md5Hash, server_token);
                string client_md5 = MD5Tool.GetMd5Hash(md5Hash, auth_token);

                acc_token = server_md5 + client_md5;

                string setQuery = string.Format(@"
                                                MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON auth_md5_id = @p1
                                                WHEN MATCHED THEN
                                                   UPDATE SET 
                                                    auth_md5_id = @p1,
                                                    server_auth_token = @p2,
                                                    client_auth_token = @p3,
                                                    server_auth_md5 = @p4,
                                                    client_auth_md5 = @p5,
                                                    reg_date = GETDATE()
                                                WHEN NOT MATCHED THEN
                                                   INSERT (auth_md5_id, server_auth_token, client_auth_token, server_auth_md5, client_auth_md5, reg_date)
                                                   VALUES (@p1, @p2, @p3, @p4, @p5, GETDATE());
                                    ", DB_Define.DBTables[DB_Define.eDBTables.user_guest_auth_id]
                                 );

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = setQuery;
                cmd.Parameters.AddWithValue("@p1", acc_token);
                cmd.Parameters.AddWithValue("@p2", server_token);
                cmd.Parameters.AddWithValue("@p3", auth_token);
                cmd.Parameters.AddWithValue("@p4", server_md5);
                cmd.Parameters.AddWithValue("@p5", client_md5);

                bool isSuccess = TB.ExcuteSqlCommand(ref cmd);

                if (isSuccess)
                    retObj = new user_guest_auth_id(acc_token, server_token, auth_token, server_md5, client_md5);
            }
            return retObj;
        }
    }
}
