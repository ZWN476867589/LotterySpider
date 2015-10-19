using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LotterySpider.Business
{
    public class LotteryDetailInfo
    {
        public int phasetype { get; set; }
        public string phase { get; set; }
        public string create_at { get; set; }
        public string time_startsale { get; set; }
        public string time_endsale { get; set; }
        public string time_endticket { get; set; }
        public string time_draw { get; set; }
        public string staus { get; set; }
        public int forsale { get; set; }
        public int is_current { get; set; }
        public Dictionary<string, object> result { get; set; }
        public Dictionary<string, object> result_detail { get; set; }
        public string pool_amount { get; set; }
        public string sale_amount { get; set; }
        public string ext { get; set; }
        public string fc3d_sjh { get; set; }
        public int terminal_status { get; set; }
        public int fordraw { get; set; }
        public string time_startsale_fixed { get; set; }
        public string time_endsale_fixed { get; set; }
        public string time_endsale_syndicate_fixed { get; set; }
        public string time_endsale_upload_fixed { get; set; }
        public string time_draw_fixed { get; set; }
        public int time_startsale_correction { get; set; }
        public int time_endsale_correction { get; set; }
        public int time_endsale_syndicate_correction { get; set; }
        public int time_endsale_upload_correction { get; set; }
        public int time_draw_correction { get; set; }
        public string time_exchange { get; set; }
        public string formatSaleAmount { get; set; }
        public string latest_pool_amount_str { get; set; }
    }
}
