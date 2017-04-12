//using System.Collections.Generic;

namespace mSeed.mDBTxnBlock
{
    public interface IDBEndpoint
    {
        string Host { get; }
        string Database { get; }
    }

    public class DBEndpoint : IDBEndpoint
    {
        public const string DefaultDatabase = "common";
        public const string DefaultHost = "localhost";
        public const int DefaultIdleTimeOutSecs = 240; //default connection timeout is 300
        public const string DefaultUserID = "root";
        public const string DefaultUserPW = "";
        public string SetDBAlias = "";

        public DBEndpoint()
        {
            Host = DefaultHost;
            Database = DefaultDatabase;
            UserID = DefaultUserID;
            UserPW = DefaultUserPW;

            ConnectTimeout = 0;
            IdleTimeOutSecs = DefaultIdleTimeOutSecs;
        }

        public DBEndpoint(string host, string database, string id = "", string password = "", string alias = "")
            : this()
        {
            this.Host = host;
            this.Database = database;
            this.UserID = id;
            this.UserPW = password;
            this.SetDBAlias = string.IsNullOrEmpty(alias) ? database : alias;
        }

        public string Host { get; set; }
        public string Database { get; set; }
        public string UserID { get; set; }
        public string UserPW { get; set; }
        public int ConnectTimeout { get; set; }
        public int SendTimeout { get; set; }
        public int ReceiveTimeout { get; set; }
        public int IdleTimeOutSecs { get; set; }
        public bool RequiresAuth { get { return !string.IsNullOrEmpty(UserPW); } }
        public string NamespacePrefix { get; set; }

        protected bool Equals(DBEndpoint other)
        {
            return string.Equals(Host, other.Host)
                && string.Equals(Database, other.Database)
                && string.Equals(UserID, other.UserID)
                && string.Equals(UserPW, other.UserPW)
                && ConnectTimeout == other.ConnectTimeout
                && SendTimeout == other.SendTimeout
                && ReceiveTimeout == other.ReceiveTimeout
                && IdleTimeOutSecs == other.IdleTimeOutSecs
                && string.Equals(NamespacePrefix, other.NamespacePrefix);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DBEndpoint)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Host != null ? Host.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Database.GetHashCode();
                hashCode = (hashCode * 397) ^ ConnectTimeout;
                hashCode = (hashCode * 397) ^ SendTimeout;
                hashCode = (hashCode * 397) ^ ReceiveTimeout;
                hashCode = (hashCode * 397) ^ IdleTimeOutSecs;
                hashCode = (hashCode * 397) ^ (NamespacePrefix != null ? NamespacePrefix.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
