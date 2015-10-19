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
            DateTime startTime = DateTime.Parse(info.StartSaleTime);
            int i = info.SerialNoStartIndex;
            for (DateTime time = startTime; time < DateTime.Now; time = time.AddDays(1))
            {
                if (time.Year > startTime.Year)
                {
                    startTime = time;
                    i = 0;
                }
                if (info.OpenTimeOfWeek.ToList().Contains((int)time.DayOfWeek))
                {
                    LotterySerialNo no = new LotterySerialNo()
                    {
                        LotteryTypeID = info.LotteryTypeID,
                        OpenTime = time.ToString(),
                    };
                    no.SerailNo = time.Year.ToString().Substring(info.StartSerialNo.Length == 5 ? 2 : 0, info.StartSerialNo.Length - 3) + "" + i.ToString().PadLeft(3, '0');
                    serialNos.Add(no);
                    i += 1;
                }
            }
            return serialNos;
        }
    }
}
