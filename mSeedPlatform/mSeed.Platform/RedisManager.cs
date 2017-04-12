using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mSeed.RedisManager;

namespace mSeed.Platform
{
    public class RedisManager
    {
        // singleton Server_Cache instance
        private static RedisManager _redisinstance = new RedisManager();
        /// <summary> get _syscache Instance </summary>
        public static RedisManager GetRedisInstance()
        {
            return _redisinstance;
        }

        private RedisManager()
        {            
            SetRedisInstance();
        }

        private static mRedis _redisController = null;

        private void SetRedisInstance()
        {
            if(_redisController == null)
                _redisController = new mRedis();
            _redisController.Dispose();
            _redisController.RedisClose();
            _redisController.RedisConn();
        }

        public mRedis GetRedisController()
        {
            if (_redisController == null)
                SetRedisInstance();
            return _redisController;
        }
    }

}
