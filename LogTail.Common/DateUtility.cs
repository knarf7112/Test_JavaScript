using System;
//
using System.Globalization;

namespace LogTail.Common
{
    /// <summary>
    /// Date格式
    /// </summary>
    public class DateUtility : IDateUtility
    {
        /// <summary>
        /// 日期+時間
        /// </summary>
        public readonly static String DU_DATETIME = "yyyy-MM-dd HH:mm:ss.fff";//
        /// <summary>
        /// 日期格式
        /// </summary>
        public static readonly String DU_DATE = "yyyyMMdd";
        private static CultureInfo CI = CultureInfo.InvariantCulture;//setting Culture設定文化特性(各國的系統時間表示法)

        /// <summary>
        /// 日期+時間的format格式
        /// </summary>
        public string PatternDateTime { get; set; }
        /// <summary>
        /// 日期format格式
        /// </summary>
        public string PatternDate { get; set; }

        /// <summary>
        /// 日期轉換物件的Constructor
        /// </summary>
        public DateUtility()
        {
            //log ...
            this.PatternDate = DateUtility.DU_DATE;
            this.PatternDateTime = DateUtility.DU_DATETIME;
        }

        /// <summary>
        /// DateTime資料物件轉資料字串 => "yyyy-MM-dd HH:mm:ss.fff"
        /// </summary>
        /// <param name="dateTime">DateTime資料物件</param>
        /// <returns>日期+時間字串("yyyy-MM-dd HH:mm:ss.fff")</returns>
        public string GetStrByDateTime(DateTime dateTime)
        {
            return dateTime.ToString(this.PatternDateTime,CI);
        }
        /// <summary>
        /// get Datetime.Now by PatternDateTime format string
        /// </summary>
        /// <returns></returns>
        public string GetStrNow()
        {
            return GetStrByDateTime(DateTime.Now);
        }
        /// <summary>
        /// DateTime資料物件轉資料字串 => "yyyy-MM-dd"
        /// </summary>
        /// <param name="date">DateTime資料物件</param>
        /// <returns>日期字串("yyyyMMdd")</returns>
        public string GetStrByDate(DateTime date)
        {
            return date.ToString(this.PatternDate, CI);
        }

        /// <summary>
        /// get today string by pattern format string
        /// </summary>
        /// <returns></returns>
        public string GetStrToday()
        {
            return GetStrByDate(DateTime.Now);
        }

        /// <summary>
        /// 字串轉DateTime物件(日期)
        /// </summary>
        /// <param name="strDate">轉型前的資料字串</param>
        /// <param name="date">轉型成功的資料物件</param>
        /// <returns>成功/失敗</returns>
        public bool TryGetDateByStr(string strDate, out DateTime date)
        {
            return DateTime.TryParseExact(strDate, this.PatternDate, CI, DateTimeStyles.None, out date);
        }

        /// <summary>
        /// 字串轉DateTime物件(日期+時間)
        /// </summary>
        /// <param name="strDateTime">轉型前的資料字串</param>
        /// <param name="dateTime">轉型成功的資料物件</param>
        /// <returns>成功/失敗</returns>
        public bool TryGetDateTimeByStr(string strDateTime, out DateTime dateTime)
        {
            return DateTime.TryParseExact(strDateTime, this.PatternDateTime, CI, DateTimeStyles.None, out dateTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strDate"></param>
        /// <param name="diff"></param>
        /// <param name="strDiffDate"></param>
        /// <returns></returns>
        public bool TryGetDiffDateStr(string strDate, int diff, out string strDiffDate)
        {
            DateTime d;
            strDiffDate = null;
            if (this.TryGetDateByStr(strDate, out d))
            {
                d = d.AddDays(diff);
                strDiffDate = this.GetStrByDate(d);
                return true;
            };
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strDate"></param>
        /// <param name="diff"></param>
        /// <param name="strDiffDate"></param>
        /// <returns></returns>
        public bool TryGetDiffMonthStr(string strDate, int diff, out string strDiffDate)
        {
            DateTime d;
            strDiffDate = null;
            if (this.TryGetDateByStr(strDate, out d))
            {
                d = d.AddMonths(diff);
                strDiffDate = this.GetStrByDate(d);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strDate"></param>
        /// <param name="diff"></param>
        /// <param name="strDiffDate"></param>
        /// <returns></returns>
        public bool TryGetDiffYearStr(string strDate, int diff, out string strDiffDate)
        {
            DateTime d;
            strDiffDate = null;
            if (this.TryGetDateByStr(strDate, out d))
            {
                d = d.AddYears(diff);
                strDiffDate = this.GetStrByDate(d);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strDateTime"></param>
        /// <returns></returns>
        public bool ValidDateTimeStr(string strDateTime)
        {
            DateTime dt;
            return (this.TryGetDateTimeByStr(strDateTime, out dt));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strDate"></param>
        /// <returns></returns>
        public bool ValidDateStr(string strDate)
        {
            DateTime dt;
            return (this.TryGetDateByStr(strDate, out dt));
        }

        /// <summary>
        /// 日期和時間format格式使用預設值
        /// </summary>
        public void ResetToDefault()
        {
            this.PatternDate = DateUtility.DU_DATE;
            this.PatternDateTime = DateUtility.DU_DATETIME;
        }

        /// <summary>
        /// 取得時間差(ms)
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public long GetTotMillisecs(DateTime start, DateTime end)
        {
            return (long)((end - start).TotalMilliseconds);
        }

        /// <summary>
        /// 取得與現在的時間差
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        public long GetTotMillisecsNow(DateTime start)
        {
            return this.GetTotMillisecs(start, DateTime.Now);
        }

        /// <summary>
        /// 時間是否超過現在時間,true=未超時,false=超時
        /// </summary>
        /// <param name="expired"></param>
        /// <returns></returns>
        public bool TimeIsUp(DateTime expired)
        {
            return this.GetTotMillisecsNow(expired) > 0;
        }
    }
}
