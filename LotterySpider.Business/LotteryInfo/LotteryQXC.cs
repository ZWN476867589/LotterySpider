using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LotterySpider.Business.LotteryInfo
{
    public class LotteryQXC:ILotteryFactory
    {
        public List<LotterySerialNo> CreateSerialNo(LotteryBasicInfo info)
        {
            List<LotterySerialNo> serialNos = new List<LotterySerialNo>();
            DateTime startTime = string.IsNullOrEmpty(info.StartSaleTime) ? DateTime.Parse("2004-07-27 00:00:00") : DateTime.Parse(info.StartSaleTime);
            int i = 129;
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
                    no.SerailNo = time.Year.ToString().Substring(2, 2).PadLeft(2, '0') + "" + i.ToString().PadLeft(3, '0');
                    serialNos.Add(no);
                }
            }
            return serialNos;
        }
    }
}
