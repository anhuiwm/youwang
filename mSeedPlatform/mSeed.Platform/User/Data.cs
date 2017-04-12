using mSeed.Common;

namespace mSeed.Platform
{
    public class IData // 单利构造函数加数据进去
    {
        public IData() {  }

        //protected const bool isTestMode = false;
        public const string iosMolReturnUrl    = "http://107.150.101.9:5100/iosMolBilling.aspx";
        public const string iosMolPinReturnUrl = "http://107.150.101.9:5100/iosMolBilling.aspx";

    }

    public class Singleton<T> where T : class,new()
    {
        private static T _instance;
        private static readonly object syslock = new object();  
        public static T GetInstance()
        {
            lock (syslock)
            {
                if (_instance == null)
                {
                    _instance = new T();
                }
            }
            return _instance;
        }
    }
}