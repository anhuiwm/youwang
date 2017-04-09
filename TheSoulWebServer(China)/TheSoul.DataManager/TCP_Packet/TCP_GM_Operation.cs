using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mSeed.RedisManager;

namespace TheSoul.DataManager.TCP_Packet
{
    public class TCP_GM_Operation
    {
        public enum GM_CS_OPERATION
        {
            GM_CS_OP_NONE,
            GM_CS_OP_LOGOUT_ALL = 1,//모든 유저 강제 종료
            GM_CS_OP_RESTRICT_CHAT = 2,//특정 유저 제한  JSON: {aid=88, restricttime}
            GM_CS_OP_RESTRICT_LOGIN = 3,//특정 유저 제한

            GM_CS_OP_USER_LOG = 4,// 추가 특정 유저 로그 남김

            GM_CS_OP_MAX
        };

        private static readonly Dictionary<GM_CS_OPERATION, string> opname_set = new Dictionary<GM_CS_OPERATION,string>()
        {
            { GM_CS_OPERATION.GM_CS_OP_LOGOUT_ALL, "GM_CS_OP_LOGOUT_ALL" },
            { GM_CS_OPERATION.GM_CS_OP_RESTRICT_CHAT, "GM_CS_OP_RESTRICT_CHAT" },
            { GM_CS_OPERATION.GM_CS_OP_RESTRICT_LOGIN, "GM_CS_OP_RESTRICT_LOGIN" },
            { GM_CS_OPERATION.GM_CS_OP_USER_LOG, "GM_CS_OP_USER_LOG" },
        };

        public enum CS_LOG_LEVEL    //CS에서 자체적으로 사용함. (다른 정의를 사용하고자 할경우는 따로 알려주세요)
        {
            CS_LOG_LEVEL_NONE = 0,	//이값으로 오면 로그 남기는 것을 해제함.
            CS_LOG_LEVEL_01 = 1,	//특정 부분만 남김CS에서 사용.
            CS_LOG_LEVEL_02,
            CS_LOG_LEVEL_03,
            CS_LOG_LEVEL_04,
            CS_LOG_LEVEL_05,

            CS_LOG_LEVEL_MAX,
        };

        const int GMConnectTimeOut = 500;

        private static bool SendPacket<T>(string ipaddr, int port, int timeoutMilliseconds, GM_CS_OPERATION setOp, T setObj)
        {
            StoSGMOperation setData = new StoSGMOperation();
            if (opname_set.TryGetValue(setOp, out setData.operatorName))
            {
                Session setSession = new Session();
                setSession.TcpConnect(ipaddr, port, timeoutMilliseconds);
                if (setSession.TcpConnected)
                {
                    setData.op = (UInt32)setOp;
                    setData.jsonParam = mJsonSerializer.ToJsonString(setObj);
                    setSession.TcpSend(Packet.FromProtocol(Protocol.ID.CS_GM_OPERATION, setData, Packet.Tag.TcpSnd));
                    setSession.TcpClose();
                    return true;
                }
            }
            return false;
        }

        private static bool SendPacket(string ipaddr, int port, int timeoutMilliseconds, GM_CS_OPERATION setOp)
        {
            StoSGMOperation setData = new StoSGMOperation();
            if (opname_set.TryGetValue(setOp, out setData.operatorName))
            {
                Session setSession = new Session();
                setSession.TcpConnect(ipaddr, port, timeoutMilliseconds);
                if (setSession.TcpConnected)
                {
                    setData.op = (UInt32)setOp;
                    setData.jsonParam = "";
                    setSession.TcpSend(Packet.FromProtocol(Protocol.ID.CS_GM_OPERATION, setData, Packet.Tag.TcpSnd));
                    setSession.TcpClose();
                    return true;
                }
            }
            return false;
        }

        public static bool LogOutAllUser(string ipaddr, int port, int timeoutMilliseconds = GMConnectTimeOut)
        {
            return SendPacket(ipaddr, port, timeoutMilliseconds, GM_CS_OPERATION.GM_CS_OP_LOGOUT_ALL);
            //StoSGMOperation setData = new StoSGMOperation();
            //GM_CS_OPERATION csOp = GM_CS_OPERATION.GM_CS_OP_LOGOUT_ALL;
            //if (opname_set.TryGetValue(csOp, out setData.operatorName))
            //{
            //    Session setSession = new Session();
            //    setSession.TcpConnect(ipaddr, port, timeoutMilliseconds);
            //    if (setSession.TcpConnected)
            //    {
            //        setData.operatorName = "GM_CS_OP_LOGOUT_ALL";
            //        setData.op = (UInt32)csOp;
            //        setData.jsonParam = "";
            //        setSession.TcpSend(Packet.FromProtocol(Protocol.ID.CS_GM_OPERATION, setData, Packet.Tag.TcpSnd));
            //        setSession.TcpClose();
            //        return true;
            //    }
            //}
            //return false;
        }

        public static bool User_ChatRestrict(string ipaddr, int port, long aid, long restrictMinute, int timeoutMilliseconds = GMConnectTimeOut)
        {
            RESTRICT_CHAT param = new RESTRICT_CHAT(aid, restrictMinute);
            return SendPacket<RESTRICT_CHAT>(ipaddr, port, timeoutMilliseconds, GM_CS_OPERATION.GM_CS_OP_RESTRICT_CHAT, param);

            //StoSGMOperation setData = new StoSGMOperation();
            //GM_CS_OPERATION csOp = GM_CS_OPERATION.GM_CS_OP_RESTRICT_CHAT;
            //if (opname_set.TryGetValue(csOp, out setData.operatorName))
            //{
            //    Session setSession = new Session();
            //    setSession.TcpConnect(ipaddr, port, timeoutMilliseconds);
            //    if (setSession.TcpConnected)
            //    {
            //        setData.operatorName = "GM_CS_OP_RESTRICT_CHAT";
            //        setData.op = (UInt32)csOp;
            //        setData.jsonParam = mJsonSerializer.ToJsonString(param);
            //        setSession.TcpSend(Packet.FromProtocol(Protocol.ID.CS_GM_OPERATION, setData, Packet.Tag.TcpSnd));
            //        setSession.TcpClose();
            //        return true;
            //    }
            //}
            //return false;
        }

        public static bool User_LoginRestrict(string ipaddr, int port, long aid, long restrictMinute, int timeoutMilliseconds = GMConnectTimeOut)
        {
            RESTRICT_CHAT param = new RESTRICT_CHAT(aid, restrictMinute);
            return SendPacket<RESTRICT_CHAT>(ipaddr, port, timeoutMilliseconds, GM_CS_OPERATION.GM_CS_OP_RESTRICT_LOGIN, param);
            //StoSGMOperation setData = new StoSGMOperation();
            //GM_CS_OPERATION csOp = GM_CS_OPERATION.GM_CS_OP_RESTRICT_LOGIN;
            //if (opname_set.TryGetValue(csOp, out setData.operatorName))
            //{
            //    Session setSession = new Session();
            //    setSession.TcpConnect(ipaddr, port, timeoutMilliseconds);
            //    if (setSession.TcpConnected)
            //    {
            //        setData.operatorName = "GM_CS_OP_RESTRICT_LOGIN";
            //        setData.op = (UInt32)csOp;
            //        RESTRICT_CHAT param = new RESTRICT_CHAT(aid, restrictMinute);
            //        setData.jsonParam = mJsonSerializer.ToJsonString(param);
            //        setSession.TcpSend(Packet.FromProtocol(Protocol.ID.CS_GM_OPERATION, setData, Packet.Tag.TcpSnd));
            //        setSession.TcpClose();
            //        return true;
            //    }
            //}
            //return false;
        }

        public static bool User_LogLevel(string ipaddr, int port, long aid, CS_LOG_LEVEL setLevel, long logtime, int timeoutMilliseconds = GMConnectTimeOut)
        {
            LOG_LEVEL param = new LOG_LEVEL(aid, (int)setLevel, logtime);
            return SendPacket<LOG_LEVEL>(ipaddr, port, timeoutMilliseconds, GM_CS_OPERATION.GM_CS_OP_USER_LOG, param);
        }
    }
}
