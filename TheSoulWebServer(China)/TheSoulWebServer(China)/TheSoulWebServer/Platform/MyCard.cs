using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheSoul.DataManager;

namespace TheSoulWebServer.Platform
{
    public class MyCard
    {
        public MyCard()
        {

        }
        public static bool IsMyCard(Shop_Define.eBillingType BillingType)
        {
             if (BillingType == Shop_Define.eBillingType.Global_aOS_MyCard
                || BillingType == Shop_Define.eBillingType.Global_iOS_MyCard
                || BillingType == Shop_Define.eBillingType.mfun_aOS_Mycard
                || BillingType == Shop_Define.eBillingType.mfun_iOS_Mycard)
             {
                 return true;
             }
             return false;
        }
    }
}