using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mSeed.RedisManager
{
    public interface IRedisEndPoint
    {
        string Host { get; }
        string Port { get; }
        string Alias { get; }
    }

    public class RedisEndpoint : IRedisEndPoint
    {
        public const string DefaultHost = "localhost";
        public const string DefaultPort = "6379";
        public const string DefaulAlias = "main";
        public const int DefaultIdleTimeOutSecs = 240; //default connection timeout is 60
        public const string DefaultUserID = "root";
        public const string DefaultUserPW = "";
        public string UserID { get; set; }
        public string UserPW { get; set; }

        public string Host { get; set; }
        public string Port { get; set; }
        public string Alias { get; set; }
        public int ConnectTimeout { get; set; }

        public RedisEndpoint()
        {
            Host = DefaultHost;
            Port = DefaultPort;
            UserID = DefaultUserID;
            UserPW = DefaultUserPW;
            Alias = DefaulAlias;
            ConnectTimeout = DefaultIdleTimeOutSecs;
        }

        public RedisEndpoint(string host, string port = DefaultPort, string alias = DefaulAlias,string id = "", string password = "")
            : this()
        {
            this.Host = host;
            this.Port = port;
            this.Alias = alias;
            this.UserID = id;
            this.UserPW = password;
        }

        protected bool Equals(RedisEndpoint other)
        {
            return string.Equals(Host, other.Host)
                && string.Equals(Port, other.Port)
                && string.Equals(UserID, other.UserID)
                && string.Equals(UserPW, other.UserPW)
                && ConnectTimeout == other.ConnectTimeout
                && string.Equals(Alias, other.Alias);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((RedisEndpoint)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Host != null ? Host.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Port.GetHashCode();
                hashCode = (hashCode * 397) ^ Alias.GetHashCode();
                hashCode = (hashCode * 397) ^ ConnectTimeout;
                return hashCode;
            }
        }
    }

}
