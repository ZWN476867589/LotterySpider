using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LotterySpider.Business
{
    public class LotteryOriginData
    {
        public int RowID { get; set; }
        public int LotteryTypeID { get; set; }
        public string OriginData { get; set; }
        public string DataUrl { get; set; }
        public string Time { get; set; }
        public string SerialNo { get; set; }
    }
}
