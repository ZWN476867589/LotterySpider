using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LotterySpider.Business
{
    public class LotteryBaseInfo
    {
        public int code { get; set; }
        public Dictionary<string, object> data { get; set; }
        public string message { get; set; }
        public bool LHC_SUPER_PRIV { get; set; }
        public string USER_SOURCE_DEFAULT { get; set; }
        public LotteryDetailInfo DetailInfo { get; set; }
    }
}
