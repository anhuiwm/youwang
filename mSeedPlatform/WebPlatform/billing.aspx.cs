using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ServiceStack.Text;
using mSeed.Common;
using mSeed.Platform;
using WebPlatform.Tools;
using mSeed.RedisManager;
namespace WebPlatform
{
    public partial class billing : System.Web.UI.Page
    {
        private string[] ops = new string[] {
            "iab_validate",
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            WebQueryParam queryFetcher = new WebQueryParam();
            string requestOp = queryFetcher.QueryParam_Fetch("op");
            try
            {

                
                if (Array.IndexOf(ops, requestOp) >= 0)
                {
                    JsonObject json = new JsonObject();
                    queryFetcher.operation = requestOp;
                    Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
                    long service_access_id = queryFetcher.QueryParam_FetchLong("service_access_id");
                    string service_key = queryFetcher.QueryParam_Fetch("service_key");
                    eBillingType billing_type = (eBillingType)queryFetcher.QueryParam_FetchInt("billing_type");

                
                    if (requestOp.Equals("iab_validate"))
                    {
                        string purchaseToken = queryFetcher.QueryParam_Fetch("purchase_token");
                        string product_id = queryFetcher.QueryParam_Fetch("product_id");

                        game_service_info serviceInfo = GameServiceManager.GetGameServiceInfo(service_access_id, service_key, (int)billing_type);
                        //purchaseToken = "{\"orderId\":\"1299912121692121212121.1320812112121\",\"packageName\":\"com.test.runrunrun\",\"productId\":\"com.test.runrunrun.item_001\",\"purchaseTime\":1393377621833,\"purchaseState\":0,\"purchaseToken\":\"sekadakadaaxgjhsrLn61wGoyAl..._8-l8zsUhVpY7o7Zq08s\"}";
                        if (billing_type == eBillingType.Kr_aOS_Google || billing_type == eBillingType.Global_aOS_Google)
                        {
//                            List<string> testToken = new List<string>() {
//                                "jnbbkbobgecoapcfnndkdmoc.AO-J1Oy-U6G7P_U4MODbL9YP1Yi6SRrIMI8hnX9nrNJinlZdSUUiSjoGY5meVVxW1ilMSP8R_pzXEZ3oquqd8WOvmCAmHo2tYdcLqpUbDzHFVzHQq1G_VtOVhe5wcRcrsrKbdzu5Ysd2ju4qB5ME627bD8Qruoe7iw",
//"idhdponfcmnldcfmicopbihi.AO-J1OxcuDk2sIsSKmCQP4U9-5QELk4O5LdY62B9_TV19WVFGCftpl1JJObzA2Nx1rWlRQE9M4W9e7oftpizLis0v5fufst9vrGUa4f1Zc2_j6w0VhT7WR8l7KOwojgRAZiPw-TTNIgFjWpRRvCRoQxQikUo5-9lNQ",
//"eccdcehhjbfkalkjcopddklh.AO-J1OwZ0baL0c4KdvM_vwORrJviY167HDugaK8WT-krs2mWLX_QW4d0nFJ3zTuK2v9eJ06vM7-CXhANase69Q-wsbetJnGCjleLJpDY9B7WC4bkl-C5gaH9AmZAsm4th7wvr4XsPtEiFFhfQ5yx_324iyD45NvO8A",
//"jnolidfenpjdfcndenfbgihi.AO-J1OyVlDGu6DepX-NIlpgSi7Qo1TLy7gL1RN2a-mjl2MHWzZU-DiEpCKcF-QTPOrP9I-QH7HZ7FcdqEvZpN2gq_SnF_56FuuZCmUqlId4bH6loGjbc8BtdYMyHKvAPaoKzh4GA6E9vgQZQXks476-0Irrek3LL-w",
//"cjlkppecmckkmomndfndblna.AO-J1OzXJgeBQydCvo6-Msc7V9yWuf1O3MzfRYRxXTM8h85GQTnJjTtlQ-cmmA94Hv58e6RNR7FgZwrfgcEaS2Q-BY-jF3KnQO7yDng-FZHjk2m7XUSUCeCweEkxX_XijLR1VYo9cdhUQrfMMk1LE2YME4cbxK7OgA",
//"gmhneiejbglfbokabemgoiaa.AO-J1OzE-EwByapPn_mp25TwyjNpIkCcTiD-lQuGkxzFpYF7eqSRM713QTb-08w5zU_LZQNFevBY3A0Dedl9P9fl2Qgi8Byz4jmxvCa7-3qNN3Pwc5ZYzc1R0QrBmb1e-rbCpqtogadPhXP6HrKtit7kA6mrs33iRA",
//"hglagmijgaagikinmmimaapp.AO-J1OwnC3n5PHOYJzmrCxxYFJq5cCpBgHLzZehnIaSsgNr6MEN9-ba1jGR115w8OPEqR-8YO2zLnBVKDm8TP5RlYBwm6_eCzvJ9itAYx-qKyjJw7vhu1Dep3Uos4MGglLl2rnNOU03ZNYtcsSDjMvrdXfdGc-hWbg",
//"idgdcikgjalhobhonfmhohgj.AO-J1Oxf0wsZHUigMCB4cZ5EFVs7-zZ1i09xgudz4zVkvsMa1htE7BganLEecNLT7lDm7rqeAfkRdNcdGeTktS_kc95wOfYI3ocJoU6vZcBSA6VuRdCsymMcZK6vMYSk5SlL6ejxA8Gvb6FfRSUl32gzmrWSb76hOw",
//"dmjifkemdhldekljkiplcmjf.AO-J1Ox73Kqq-WLs8AX_KLUb5R3nfr4mwazn8KSOPaljoEPC0yt-z2TVG2D-bZcH01UweqGAxfxQSv5SlpZW8wrd34z8r9G75dg2Mrfo-eGyLKF4h3sztpWRhBwiWYc612Q8MsmtST55zvq0Babml9W9t8mCDtAjMw",
//"dhnejppcplkpjmbnicmndohg.AO-J1OyplKYy_krmalTYRIq9_oMrlSp8eGavks6qIdzJ40W-M1LGTRp-wPYKzWTFY6eW6VHHhkHUvrwbZ0Y2YQuB-XKjytEJDE3PBWyg43KtDyURp_mpIgT0fqnMdqhefzs2Nu4hCALorq_cLw_a6ocEMUISPtm1WQ",
//"bmllnbedeeinnglhbnhbgang.AO-J1Oz6FH5RVRA8KckOd4qe4mpjGV_pWyLLrveHBlV-pqGvuu_JnFMHrBDXSYz_Yomtn8Pc9TN64YW5CKenFXgauPq-zIOROLmNSRxFcgq4QpbLKNyAfbraZ4BvuiguhOWJNEyfEYOsEZuFOqqv_wjH3vWFoNVUPw",
//"hbphmdkhnalpeekpihiiekdo.AO-J1Oz6hN4WPquEqO8a6Q6eV6p_cMmyKYsVZDtz44rU3P2xQE5Idp1FiLcuJxrb1x1Ht7CSy3uJqjtzvbgywqHCujABmJzudbFKYGytIV8SexqiVUuDRSaBiAKSrk7cBTlVPJzVCq8y3EztMPmAG7V3VDreLcUZPw",
//"ilbkklkjdffocemdejhgnhfe.AO-J1Ow88urlnAUUIyKrT20d8Neto9-AnTO8KgBMWX_16XAXBY5jBFzzLvot50VpzPhjoKK8Ua4aD58pFUfpZvqPtV2DeztY749NXMYQx06KSSJrUqQT0zTKl-SAVOWvJ8DVuYOfV94tns9l9gaNLnQyfWcpaVbs7w",
//"abnhbgohckmjmfinkffhekli.AO-J1OwiyijTL85HQH14hYbRAHnGD3OFo_AtBAz_c5bMjpX13gw8yh3L3124mLoMzUee2sTUG8Ta86wHhdbJ7F9pOTZx0BM6LR8fbnHTSV0Anm822BpBeUcmExbHl4a0aBlTH8Yl2bkq8w9RbVCyZR28cmoswWB9kQ",
//"edhnmnbdggdafmmcgpoijopo.AO-J1Oxz9NxcsHJM8twCwg1MvacsIZzuyn-YngsIc5--xUEzjTDE4zmCl3RV9R_VIZ3M87D7XEPdJWyT-bMxmj6af939lXiyysU3y-bPYd9VD7lZ0SweC_MdmT4vAvyYhSFncRk_Hz4ngvqMKH__Ngd7leFaPqSSzw",
//"dnecoinpodjkhcdafodhdoaj.AO-J1OznNJxUhRDyrA4rnFgKxlmlyYuh6fmASGazHcjlbiq2-HEJPaIoR-7OBzPN07V8LRZfEzen9X2V_28iT0bacmpPuMD4tHP0VZ2QX8gbHpAFG4tk81Xkx24c97F3gVvnz4kXJR_kwvpiOhSUjez8xDDtIn1ocA",
//"nchgfimoddebapdikhhipoeh.AO-J1OwgAFZ4YF_K46m0m6wJYPxu5TNX2CoXfnb8_mTjdtPggYNRHEeZeNMd-CuPYjM81VpjVs1_pjBb60Ivuyk72zzMYkWMOJ6bZHWNspfVkzBSrGF04gQ0YOpnb9U3DN0NUkpHeo2Y4y3QfgJUw6n_s8887QMHTw",
//"njnhcheplnookpgnecppcpba.AO-J1OxdS2sWwI41KikfLc2MoMURz2_ldcRvkVTDdk5zYjuvX_WRsVtXaJItzOUljY77-mLoHEcKqLCNJgm4WzNC7UfnVbDooZmTpyxaFTT3KjGkM5870cSbpGwWKi5YinI0L_IZvNFZvolVcdX7nThNHLelTGeL2Q",
//"mbmnhebennngbbhfbfilbknb.AO-J1OwxsZAYpvsh5eSc1CVpzFdUklaKGtI9UvCuaPOmBBx6CD7wtCIK5s1C7J_Qop5-8YSOqHBxlW_5F5ZURbfkifzzr05SWA9goQHPkWi40CLp07dwvMCOvo2E8OYwrJpujpa15rqb0cf7WxyrX2XZMgMcIWZHpw",
//"cdompeppjhelokjmekmognnf.AO-J1OwoQC0LSumDMDxqnvjlmkW_wpMA4-ghSOXdw4F07hWzxNWaEXYRuvzxb9Beyd8XzDVUwH8eaZrXwG-ycvExk3HF26QHBimP5CxJQixdH66k3vH1IE3Y0sYFCMcGqczkw8srddlNQiz1_cA5EjXfCwIAlDK9kQ",
//                            };

//                            foreach (string setTest in testToken)
//                            {
//                                retError = GoogleJsonWebToken.GetGoogleJWTInstance().GoogleIABVerify(purchaseToken, product_id, serviceInfo.service_app_id);
//                            }


                            retError = GoogleJsonWebToken.GetGoogleJWTInstance().GoogleIABVerify(purchaseToken, product_id, serviceInfo.service_app_id);
                        }
                        else if (billing_type == eBillingType.Kr_iOS_Appstore || billing_type == eBillingType.Global_iOS_Appstore)
                        {
                            //string testToken = "ewoJInNpZ25hdHVyZSIgPSAiQXljR0JTSmsyYWs4bzhNS3hhdDVrK3pYcWVTaGRn\r\nYjEyc3hORGI5OFdpb3d3bUtDQ3ZSSXpMSit2OHYzOHdTWTNCZDZucTVQdnVGaUtq\r\ndDZLc3ZQQ3pBaWdBR3dka2RwMVNpVWpmb0RxZjFHT01wclpQNklXS0pjWHJKS0dh\r\naWthVkMyOXV6Z3pEWnFVT25RdjBNZ0lzck5TeEFrcWRNREZHNTVYeVk3eFZoQnI2\r\nRFdoWm01ME1xUHBaSVdzTk5Ic1JqRWdDZXBBWnV4TjJvUnJSUHNXbmlpQURRR0pB\r\ndzRQZTh3a3Q2ZTR3TklLOWVkOWFlR2NiQ3RjS2RWbkpJYkRxNDhyVC9YVnRMYXAz\r\nd3lKRjYwQlNqYWtkWDNvTnRRREJmRy9QOUFiRVJ0RGlKcm10Tm5ZUSt1TGpJTXFJ\r\nUDV0SXRSbTdvMEhWK1lGTEtLRjN6RmNkc0FBQVdBTUlJRmZEQ0NCR1NnQXdJQkFn\r\nSUlEdXRYaCtlZUNZMHdEUVlKS29aSWh2Y05BUUVGQlFBd2daWXhDekFKQmdOVkJB\r\nWVRBbFZUTVJNd0VRWURWUVFLREFwQmNIQnNaU0JKYm1NdU1Td3dLZ1lEVlFRTERD\r\nTkJjSEJzWlNCWGIzSnNaSGRwWkdVZ1JHVjJaV3h2Y0dWeUlGSmxiR0YwYVc5dWN6\r\nRkVNRUlHQTFVRUF3dzdRWEJ3YkdVZ1YyOXliR1IzYVdSbElFUmxkbVZzYjNCbGNp\r\nQlNaV3hoZEdsdmJuTWdRMlZ5ZEdsbWFXTmhkR2x2YmlCQmRYUm9iM0pwZEhrd0ho\r\nY05NVFV4TVRFek1ESXhOVEE1V2hjTk1qTXdNakEzTWpFME9EUTNXakNCaVRFM01E\r\nVUdBMVVFQXd3dVRXRmpJRUZ3Y0NCVGRHOXlaU0JoYm1RZ2FWUjFibVZ6SUZOMGIz\r\nSmxJRkpsWTJWcGNIUWdVMmxuYm1sdVp6RXNNQ29HQTFVRUN3d2pRWEJ3YkdVZ1Yy\r\nOXliR1IzYVdSbElFUmxkbVZzYjNCbGNpQlNaV3hoZEdsdmJuTXhFekFSQmdOVkJB\r\nb01Da0Z3Y0d4bElFbHVZeTR4Q3pBSkJnTlZCQVlUQWxWVE1JSUJJakFOQmdrcWhr\r\naUc5dzBCQVFFRkFBT0NBUThBTUlJQkNnS0NBUUVBcGMrQi9TV2lnVnZXaCswajJq\r\nTWNqdUlqd0tYRUpzczl4cC9zU2cxVmh2K2tBdGVYeWpsVWJYMS9zbFFZbmNRc1Vu\r\nR09aSHVDem9tNlNkWUk1YlNJY2M4L1cwWXV4c1FkdUFPcFdLSUVQaUY0MWR1MzBJ\r\nNFNqWU5NV3lwb041UEM4cjBleE5LaERFcFlVcXNTNCszZEg1Z1ZrRFV0d3N3U3lv\r\nMUlnZmRZZUZScjZJd3hOaDlLQmd4SFZQTTNrTGl5a29sOVg2U0ZTdUhBbk9DNnBM\r\ndUNsMlAwSzVQQi9UNXZ5c0gxUEttUFVockFKUXAyRHQ3K21mNy93bXYxVzE2c2Mx\r\nRkpDRmFKekVPUXpJNkJBdENnbDdaY3NhRnBhWWVRRUdnbUpqbTRIUkJ6c0FwZHhY\r\nUFEzM1k3MkMzWmlCN2o3QWZQNG83UTAvb21WWUh2NGdOSkl3SURBUUFCbzRJQjF6\r\nQ0NBZE13UHdZSUt3WUJCUVVIQVFFRU16QXhNQzhHQ0NzR0FRVUZCekFCaGlOb2RI\r\nUndPaTh2YjJOemNDNWhjSEJzWlM1amIyMHZiMk56Y0RBekxYZDNaSEl3TkRBZEJn\r\nTlZIUTRFRmdRVWthU2MvTVIydDUrZ2l2Uk45WTgyWGUwckJJVXdEQVlEVlIwVEFR\r\nSC9CQUl3QURBZkJnTlZIU01FR0RBV2dCU0lKeGNKcWJZWVlJdnM2N3IyUjFuRlVs\r\nU2p0ekNDQVI0R0ExVWRJQVNDQVJVd2dnRVJNSUlCRFFZS0tvWklodmRqWkFVR0FU\r\nQ0IvakNCd3dZSUt3WUJCUVVIQWdJd2diWU1nYk5TWld4cFlXNWpaU0J2YmlCMGFH\r\nbHpJR05sY25ScFptbGpZWFJsSUdKNUlHRnVlU0J3WVhKMGVTQmhjM04xYldWeklH\r\nRmpZMlZ3ZEdGdVkyVWdiMllnZEdobElIUm9aVzRnWVhCd2JHbGpZV0pzWlNCemRH\r\nRnVaR0Z5WkNCMFpYSnRjeUJoYm1RZ1kyOXVaR2wwYVc5dWN5QnZaaUIxYzJVc0lH\r\nTmxjblJwWm1sallYUmxJSEJ2YkdsamVTQmhibVFnWTJWeWRHbG1hV05oZEdsdmJp\r\nQndjbUZqZEdsalpTQnpkR0YwWlcxbGJuUnpMakEyQmdnckJnRUZCUWNDQVJZcWFI\r\nUjBjRG92TDNkM2R5NWhjSEJzWlM1amIyMHZZMlZ5ZEdsbWFXTmhkR1ZoZFhSb2Iz\r\nSnBkSGt2TUE0R0ExVWREd0VCL3dRRUF3SUhnREFRQmdvcWhraUc5Mk5rQmdzQkJB\r\nSUZBREFOQmdrcWhraUc5dzBCQVFVRkFBT0NBUUVBRGFZYjB5NDk0MXNyQjI1Q2xt\r\nelQ2SXhETUlKZjRGelJqYjY5RDcwYS9DV1MyNHlGdzRCWjMrUGkxeTRGRkt3TjI3\r\nYTQvdncxTG56THJSZHJqbjhmNUhlNXNXZVZ0Qk5lcGhtR2R2aGFJSlhuWTR3UGMv\r\nem83Y1lmcnBuNFpVaGNvT0FvT3NBUU55MjVvQVE1SDNPNXlBWDk4dDUvR2lvcWJp\r\nc0IvS0FnWE5ucmZTZW1NL2oxbU9DK1JOdXhUR2Y4YmdwUHllSUdxTktYODZlT2Ex\r\nR2lXb1IxWmRFV0JHTGp3Vi8xQ0tuUGFObVNBTW5CakxQNGpRQmt1bGhnd0h5dmoz\r\nWEthYmxiS3RZZGFHNllRdlZNcHpjWm04dzdISG9aUS9PamJiOUlZQVlNTnBJcjdO\r\nNFl0UkhhTFNQUWp2eWdhWndYRzU2QWV6bEhSVEJoTDhjVHFBPT0iOwoJInB1cmNo\r\nYXNlLWluZm8iID0gImV3b0pJbTl5YVdkcGJtRnNMWEIxY21Ob1lYTmxMV1JoZEdV\r\ndGNITjBJaUE5SUNJeU1ERTJMVEV3TFRFNUlEQTNPakk0T2pNMUlFRnRaWEpwWTJF\r\ndlRHOXpYMEZ1WjJWc1pYTWlPd29KSW5CMWNtTm9ZWE5sTFdSaGRHVXRiWE1pSUQw\r\nZ0lqRTBOelk0T0Rjek1UVTBOekFpT3dvSkluVnVhWEYxWlMxcFpHVnVkR2xtYVdW\r\neUlpQTlJQ0kxTjJNNVlXVmpaVE5tWkdNeE1tSmlZamd5WmpOaVlUZzFaV1l6TVdJ\r\neU1tRTRaVFJqT1RBNElqc0tDU0p2Y21sbmFXNWhiQzEwY21GdWMyRmpkR2x2Ymkx\r\ncFpDSWdQU0FpTXpjd01EQXdNVFEzTmpRd016Z3pJanNLQ1NKaWRuSnpJaUE5SUNJ\r\neUlqc0tDU0poY0hBdGFYUmxiUzFwWkNJZ1BTQWlNVEUxTWpJNE1qazBPU0k3Q2dr\r\naWRISmhibk5oWTNScGIyNHRhV1FpSUQwZ0lqTTNNREF3TURFME56WTBNRE00TXlJ\r\nN0Nna2ljWFZoYm5ScGRIa2lJRDBnSWpFaU93b0pJbTl5YVdkcGJtRnNMWEIxY21O\r\nb1lYTmxMV1JoZEdVdGJYTWlJRDBnSWpFME56WTRPRGN6TVRVME56QWlPd29KSW5W\r\ndWFYRjFaUzEyWlc1a2IzSXRhV1JsYm5ScFptbGxjaUlnUFNBaU1rRXlSa05CUVVJ\r\ndFF6YzNPQzAwTVRKQ0xUZzJORVl0UkRNeFFVTTFPVGd3UXpORUlqc0tDU0pwZEdW\r\ndExXbGtJaUE5SUNJeE1UVXpOVEF5TURBeklqc0tDU0oyWlhKemFXOXVMV1Y0ZEdW\r\neWJtRnNMV2xrWlc1MGFXWnBaWElpSUQwZ0lqZ3hPVE13TlRRME1TSTdDZ2tpY0hK\r\ndlpIVmpkQzFwWkNJZ1BTQWlkR2hsYzI5MWJHbHVkRjl3Y21salpWODFOVEF3SWpz\r\nS0NTSndkWEpqYUdGelpTMWtZWFJsSWlBOUlDSXlNREUyTFRFd0xURTVJREUwT2pJ\r\nNE9qTTFJRVYwWXk5SFRWUWlPd29KSW05eWFXZHBibUZzTFhCMWNtTm9ZWE5sTFdS\r\naGRHVWlJRDBnSWpJd01UWXRNVEF0TVRrZ01UUTZNamc2TXpVZ1JYUmpMMGROVkNJ\r\nN0Nna2lZbWxrSWlBOUlDSmpiMjB1YlhObFpXUm5ZVzFsY3k1MGFHVnpiM1ZzYVc1\r\nMElqc0tDU0p3ZFhKamFHRnpaUzFrWVhSbExYQnpkQ0lnUFNBaU1qQXhOaTB4TUMw\r\neE9TQXdOem95T0Rvek5TQkJiV1Z5YVdOaEwweHZjMTlCYm1kbGJHVnpJanNLZlE9\r\nPSI7CgkicG9kIiA9ICIzNyI7Cgkic2lnbmluZy1zdGF0dXMiID0gIjAiOwp9";
                            //UserManager.AppleIABVerify(testToken, true, serviceInfo.service_app_id, product_id);

                            purchaseToken = purchaseToken.Replace("\\r\\n", "\r\n");
                            retError = UserManager.AppleIABVerify(purchaseToken, serviceInfo.service_status > 0, serviceInfo.service_app_id, product_id);
                        }
                        else if (billing_type == eBillingType.Kr_aOS_OneStore)
                        {
                            string buydate = queryFetcher.QueryParam_Fetch("buy_date");
                            DateTime setDate;
                            if (!DateTime.TryParse(buydate, out setDate))
                                setDate = DateTime.Now;
                            retError = UserManager.OneStoreIABVerify(purchaseToken, serviceInfo.service_status > 0, serviceInfo.service_app_id, setDate);
                        }
                        else if (billing_type == eBillingType.Global_iOS_MOL || billing_type == eBillingType.mfun_iOS_MOL)
                        {
                            string buydate = queryFetcher.QueryParam_Fetch("buy_date");
                            string webIPandPort = queryFetcher.QueryParam_Fetch("webIPandPort");
                            string realToken = queryFetcher.QueryParam_Fetch("realToken");
                            string price = queryFetcher.QueryParam_Fetch("price");
                            DateTime setDate;
                            if (!DateTime.TryParse(buydate, out setDate))
                                setDate = DateTime.Now;
                            retError = UserManager.iOSMolIABVerify(ref json, purchaseToken, product_id, webIPandPort, realToken, price);
                            
                        }
                        else if (billing_type == eBillingType.Global_iOS_MOLPin || billing_type == eBillingType.mfun_iOS_MOLPin)
                        {
                            string buydate = queryFetcher.QueryParam_Fetch("buy_date");
                            string webIPandPort = queryFetcher.QueryParam_Fetch("webIPandPort");
                            string realToken = queryFetcher.QueryParam_Fetch("realToken");
                            string price = queryFetcher.QueryParam_Fetch("price");
                            DateTime setDate;
                            if (!DateTime.TryParse(buydate, out setDate))
                                setDate = DateTime.Now;
                            retError = UserManager.iOSMolPinIABVerify(ref json, purchaseToken, product_id, webIPandPort, realToken, price);
                        }
                        else if (billing_type == eBillingType.Global_aOS_MyCard || billing_type == eBillingType.mfun_aOS_Mycard || billing_type == eBillingType.Global_iOS_MyCard || billing_type == eBillingType.mfun_iOS_Mycard)
                        {                            
                            retError = UserManager.MyCardIABVerify(ref json, purchaseToken, product_id);
                        }

                    }

                    queryFetcher.Render(json, retError);
                }
            }
            catch (Exception errorEx)
            {
                JsonObject error = new JsonObject();
                error = mJsonSerializer.AddJson(error, "StackTrace", mJsonSerializer.ToJsonString(errorEx.StackTrace));
                error = mJsonSerializer.AddJson(error, "Message", mJsonSerializer.ToJsonString(errorEx.Message));
                mSeed.Common.mLogger.mLogger.Critical(error.ToJson(), "billing");
                queryFetcher.Render(Result_Define.eResult.System_Exception);
            }
        }
    }
}