using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LotterySpider.Business
{
    /// <summary>
    ///彩票基础信息
    /// </summary>
    public class LotteryBasicInfo
    {
        /// <summary>
        /// 彩票ID
        /// </summary>
        public int LotteryID
        {
            get;
            set;
        }
        /// <summary>
        /// 彩票类型ID
        /// </summary>
        public int LotteryTypeID
        {
            get;
            set;
        }
        /// <summary>
        /// 彩票名称
        /// </summary>
        public string LotteryName
        {
            get;
            set;
        }
        /// <summary>
        /// 彩票代码
        /// </summary>
        public string LotteryShortCode
        {
            get;
            set;
        }
        /// <summary>
        /// 彩票每周开奖时间
        /// </summary>
        public int[] OpenTimeOfWeek
        {
            get;
            set;
        }
        /// <summary>
        /// 彩票开售时间,主要指能在百度乐彩网站上能查到记录的最早时间
        /// </summary>
        public string StartSellTime
        {
            get;
            set;
        }
        public void SetOpenTimeOfWeek(string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                string[] weeks = value.Split(',');
                List<int> Weeks = new List<int>();
                foreach (var i in weeks)
                {
                    if (!string.IsNullOrEmpty(i))
                    {
                        Weeks.Add(int.Parse(i));
                    }
                }
                this.OpenTimeOfWeek = Weeks.ToArray();
            }
        }
    }
}
