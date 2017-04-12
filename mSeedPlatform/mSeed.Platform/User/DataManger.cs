using mSeed.Common;
using System.Collections.Generic;

namespace mSeed.Platform
{
    public class DataManger : Singleton<DataManger>
    {
        //private Dictionary<eBillingType, string> dicReturnUrl = new Dictionary<eBillingType, string>();
        public DataManger() 
        {
            //dicReturnUrl.Add( eBillingType.Global_iOS_MOLPin, iosMolReturnUrl )
        }

        //public string getUrl(eBillingType);

        public string GetIosMolResUrl()
        {
            return IData.iosMolReturnUrl;
        }

        public string GetIosMolPinResUrl()
        {
            return IData.iosMolPinReturnUrl;
        }
    }
}