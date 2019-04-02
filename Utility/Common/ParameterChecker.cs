using System;
using System.Collections.Generic;
using System.Text;

namespace Utility
{
    /// <summary>
    /// 
    /// </summary>
    public static class ParameterChecker
    {
        /// <summary>
        /// NullOrEmptyFormat
        /// </summary>
        const string NullOrEmptyFormat = "A parameter of {0} is null or empty.";
        /// <summary>
        /// NullFormat
        /// </summary>
        const string NullFormat = "A parameter of {0} is null.";
        /// <summary>
        /// RangeFormat
        /// </summary>
        const string RangeFormat = "A parameter of {0} is out of range.";

        /// <summary>
        /// Check a string
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="paraName">Name of the para.</param>
        /// <param name="paraValue">The para value.</param>
        public static void CheckNullOrEmpty(string method, string paraName, string paraValue)
        {
            if (string.IsNullOrEmpty(paraValue))
                throw new ArgumentNullException(paraName, string.Format(NullOrEmptyFormat, method));
        }

        /// <summary>
        /// Check an object
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="paraName">Name of the para.</param>
        /// <param name="paraValue">The para value.</param>
        public static void CheckNull(string method, string paraName, object paraValue)
        {
            if (paraValue == null)
                throw new ArgumentNullException(paraName, string.Format(NullFormat, method));
        }

        /// <summary>
        /// Check an array
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="paraName">Name of the para.</param>
        /// <param name="paraValue">The para value.</param>
        public static void CheckNullOrEmpty(string method, string paraName, System.Array paraValue)
        {
            CheckNullOrEmpty(method, paraName, paraValue, false);
        }

        /// <summary>
        /// Check an array
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="paraName">Name of the para.</param>
        /// <param name="paraValue">The para value.</param>
        /// <param name="deep">if set to <c>true</c> [deep].</param>
        public static void CheckNullOrEmpty(string method, string paraName, System.Array paraValue, bool deep)
        {
            if (paraValue == null || paraValue.Length == 0)
                throw new ArgumentNullException(paraName, string.Format(NullOrEmptyFormat, method));

            if (deep)
            {
                for (int i = 0, j = paraValue.Length; i < j; i++)
                {
                    if (paraValue.GetValue(i) == null)
                        throw new ArgumentNullException(paraName, string.Format(NullFormat, method+".Items"));
                }
            }
        }

        /// <summary>
        /// Check an ArrayList
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="paraName">Name of the para.</param>
        /// <param name="paraValue">The para value.</param>
        /// <param name="deep">if set to <c>true</c> [deep].</param>
        public static void CheckNullOrEmpty(string method, string paraName, System.Collections.ArrayList paraValue, bool deep)
        {
            if (paraValue == null || paraValue.Count == 0)
                throw new ArgumentNullException(paraName, string.Format(NullOrEmptyFormat, method));

            if (deep)
            {
                for (int i = 0, j = paraValue.Count; i < j; i++)
                {
                    if (paraValue[i] == null)
                        throw new ArgumentNullException(paraName, string.Format(NullFormat, method + ".Items"));
                }
            }
        }

        /// <summary>
        /// Check an ArrayList
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="paraName">Name of the para.</param>
        /// <param name="paraValue">The para value.</param>
        public static void CheckNullOrEmpty(string method, string paraName, System.Collections.ArrayList paraValue)
        {
            CheckNullOrEmpty(method, paraName, paraValue, false);
        }



        /// <summary>
        /// Check an int value
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="paraName">Name of the para.</param>
        /// <param name="paraValue">The para value.</param>
        /// <param name="minValue">The min value.</param>
        /// <param name="maxValue">The max value.</param>
        public static void CheckRange(string method, string paraName, int paraValue, int minValue, int maxValue)
        {
            if (paraValue < minValue || paraValue > maxValue)
                throw new ArgumentOutOfRangeException(paraName, paraValue, string.Format(RangeFormat, method));
        }

        /// <summary>
        /// Check an int value
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="paraName">Name of the para.</param>
        /// <param name="paraValue">The para value.</param>
        /// <param name="minValue">The min value.</param>
        public static void CheckRange(string method, string paraName, int paraValue, int minValue)
        {
            CheckRange(method, paraName, paraValue, minValue, int.MaxValue);
        }

        /// <summary>
        /// Check an decimal value
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="paraName">Name of the para.</param>
        /// <param name="paraValue">The para value.</param>
        /// <param name="minValue">The min value.</param>
        /// <param name="maxValue">The max value.</param>
        public static void CheckRange(string method, string paraName, decimal paraValue, decimal minValue, decimal maxValue)
        {
            if (paraValue < minValue || paraValue > maxValue)
                throw new ArgumentOutOfRangeException(paraName, paraValue, string.Format(RangeFormat, method));
        }

        /// <summary>
        /// Check an decimal value
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="paraName">Name of the para.</param>
        /// <param name="paraValue">The para value.</param>
        /// <param name="minValue">The min value.</param>
        public static void CheckRange(string method, string paraName, decimal paraValue, decimal minValue)
        {
            CheckRange(method, paraName, paraValue, minValue, decimal.MaxValue);
        }        
    }
}
