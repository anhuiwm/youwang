using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheSoul.DataManager;

namespace TheSoulWebServer.Platform
{
    public class MOL
    {
        public MOL()
        {

        }

        public static bool IsMOL(Shop_Define.eBillingType BillingType)
        {
            if (
                BillingType == Shop_Define.eBillingType.Global_aOS_MOL
               ||BillingType == Shop_Define.eBillingType.Global_aOS_MOLPin
               ||BillingType == Shop_Define.eBillingType.Global_iOS_MOL
               ||BillingType == Shop_Define.eBillingType.Global_iOS_MOLPin
               ||BillingType == Shop_Define.eBillingType.mfun_aOS_MOL
               ||BillingType == Shop_Define.eBillingType.mfun_aOS_MOLPin
               ||BillingType == Shop_Define.eBillingType.mfun_iOS_MOL
               ||BillingType == Shop_Define.eBillingType.mfun_iOS_MOLPin
                )
            {
                return true;
            }
            return false;
        }
    }
}