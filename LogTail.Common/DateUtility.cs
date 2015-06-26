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
        public readonly static String DU_DATETIME = "yyyy-MM-dd HH:mm:ss.fff";//
        public static readonly String DU_DATE = "yyyyMMdd";
        private static CultureInfo CI = CultureInfo.InvariantCulture;//setting Culture設定文化特性(各國的系統時間表示法)

        public string PatternDateTime { get; set; }
        public string PatternDate { get; set; }

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

        public bool ValidDateTimeStr(string strDateTime)
        {
            DateTime dt;
            return (this.TryGetDateTimeByStr(strDateTime, out dt));
        }

        public bool ValidDateStr(string strDate)
        {
            DateTime dt;
            return (this.TryGetDateByStr(strDate, out dt));
        }

        public void ResetToDefault()
        {
            this.PatternDate = DateUtility.DU_DATE;
            this.PatternDateTime = DateUtility.DU_DATETIME;
        }

        public long GetTotMillisecs(DateTime start, DateTime end)
        {
            return (long)((end - start).TotalMilliseconds);
        }

        public long GetTotMillisecsNow(DateTime start)
        {
            return this.GetTotMillisecs(start, DateTime.Now);
        }

        public bool TimeIsUp(DateTime expired)
        {
            return this.GetTotMillisecsNow(expired) > 0;
        }
    }
}
