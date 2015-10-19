using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace LotterySpider.Business.LotteryInfo
{
    public class LotterySSQ : ILotteryFactory
    {
        public List<LotterySerialNo> CreateSerialNo(LotteryBasicInfo info)
        {
            List<LotterySerialNo> serialNos = new List<LotterySerialNo>();
            DateTime startTime = string.IsNullOrEmpty(info.StartSaleTime) ? DateTime.Parse("2003-02-23 09:15:00") : DateTime.Parse(info.StartSaleTime);
            int i = 0;
            for (DateTime time = startTime; time < DateTime.Now; time = time.AddDays(1))
            {
                if (time.Year > startTime.Year)
                {
                    startTime = time;
                    i = 0;
                }
                if (info.OpenTimeOfWeek.ToList().Contains((int)time.DayOfWeek))
                {
                    i += 1;
                    LotterySerialNo no = new LotterySerialNo()
                    {
                        LotteryTypeID = info.LotteryTypeID,
                        OpenTime = time.ToString(),
                    };
                    no.SerailNo = time.Year.ToString() + "" + i.ToString().PadLeft(3, '0');
                    serialNos.Add(no);
                }
            }
            return serialNos;
        }
        public void AnalyseOriginData(List<LotteryOriginData> originDataList)
        {
            foreach (var originData in originDataList)
            {
                Dictionary<string, object> dataDict = JsonHelper.JsonToDictionary(originData.OriginData);
                Dictionary<int, int> redDict = new Dictionary<int, int>();
                Dictionary<int, int> blueDict = new Dictionary<int, int>();
                if (dataDict != null)
                {
                    LotterySSQBaseInfo SSQInfo = new LotterySSQBaseInfo();
                    EntityHelper.setPropertiseValue<LotterySSQBaseInfo>(dataDict,SSQInfo);
                    LotterySSQDetailInfo SSQDetailInfo = new LotterySSQDetailInfo();
                    EntityHelper.setPropertiseValue<LotterySSQDetailInfo>(SSQInfo.data,SSQDetailInfo);
                    ArrayList result = (ArrayList)SSQDetailInfo.result["result"];
                    foreach (Dictionary<string,object> i in result)
                    {
                        string ballColor = i["key"].ToString();
                        ArrayList data = (ArrayList)i["data"];
                    }
                }
            }
        }
    }
    public class LotterySSQBaseInfo
    {
        public int code { get; set; }
        public Dictionary<string, object> data { get; set; }
        public string message { get; set; }
        public bool LHC_SUPER_PRIV { get; set; }
        public string USER_SOURCE_DEFAULT { get; set; }    
    }
    public class LotterySSQDetailInfo
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
    public class LotteryPrizeInfo
    {

    }
}
