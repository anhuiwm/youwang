using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace mSeed.RedisManager
{
    public class Retry
    {
        private static void ErrorLog(string error)
        {
            Debug.WriteLine(error);
        }

        /// <summary>
        /// Retry calling of a method if it fails
        /// </summary>
        /// <typeparam name="T">Return data type</typeparam>
        /// <param name="method">Method</param>
        /// <param name="numRetries">Number of Retries</param>
        /// <param name="secondsToWaitBeforeRetry"></param>
        /// <returns>T</returns>
        public static T RetryMethod<T>(Func<T> method, int numRetries, float secondsToWaitBeforeRetry)
        {
            if (method == null)
                throw new ArgumentNullException("method");
            T retval = default(T);
            do
            {
                try
                {
                    retval = method();
                    return retval;
                }
                catch (Exception ex)
                {
                    ErrorLog(ex.ToString());
                    ErrorLog("Retrying... Count down: " + numRetries);
                    if (numRetries <= 0) throw;                    
                    else Thread.Sleep((int)(secondsToWaitBeforeRetry * 1000));
                }
            } while (numRetries-- > 0);
            return retval;
        }

        /// <summary>
        /// Retry calling of a method if it fails
        /// </summary>
        /// <typeparam name="T">Return data type</typeparam>
        /// <param name="method">Method</param>
        /// <param name="numRetries">Number of Retries</param>
        /// <param name="secondsToWaitBeforeRetry"></param>
        /// <returns>T</returns>
        public static void RetryVoidMethod(Action method, int numRetries, float secondsToWaitBeforeRetry)
        {
            if (method == null)
                throw new ArgumentNullException("method");
            do
            {
                try
                {
                    method();
                    return;
                }
                catch (Exception ex)
                {
                    ErrorLog(ex.ToString());
                    ErrorLog("Retrying... Count down: " + numRetries);
                    if (numRetries <= 0) throw;
                    else Thread.Sleep((int)(secondsToWaitBeforeRetry * 1000));
                }
            } while (numRetries-- > 0);
            return;
        }

        /// <summary>
        /// Retry calling of an Action if it fails
        /// </summary>
        /// <typeparam name="T">Return data type</typeparam>
        /// <param name="method">Method</param>
        /// <param name="numRetries">Number of Retries</param>
        /// <param name="secondsToWaitBeforeRetry"></param>
        public static void RetryAction(Action action, int numRetries, float secondsToWaitBeforeRetry)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            do
            {
                try { action(); return; }
                catch (Exception ex)
                {
                    ErrorLog(ex.ToString());
                    ErrorLog("Retrying... Count down: " + numRetries);
                    if (numRetries <= 0) throw;
                    else Thread.Sleep((int)(secondsToWaitBeforeRetry * 1000));
                }
            } while (numRetries-- > 0);
        }
    }
}
