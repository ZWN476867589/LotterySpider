using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace LotterySpider.Business.LotteryInfo
{
    public class LotteryQXC : ILotteryFactory
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
        public List<Dictionary<int, int>> GetLotteryDataMaxFrequency(List<LotteryBaseInfo> baseInfoList)
        {
            List<Dictionary<int, int>> maxList = new List<Dictionary<int, int>>();
            Dictionary<int, int> maxDict = new Dictionary<int, int>();
            Dictionary<int, int> dataDict = new Dictionary<int, int>();
            if (baseInfoList != null)
            {
                foreach (var baseinfo in baseInfoList)
                {
                    if (baseinfo.DetailInfo.result != null)
                    {
                        ArrayList result = (ArrayList)baseinfo.DetailInfo.result["result"];
                        foreach (var i in result)
                        {
                            Dictionary<string, object> ballDict = (Dictionary<string, object>)i;
                            string ballColor = ballDict["key"].ToString();
                            ArrayList ballData = (ArrayList)ballDict["data"];
                            foreach (var num in ballData)
                            {
                                if (dataDict.Keys.Contains(int.Parse(num.ToString())))
                                {
                                    dataDict[int.Parse(num.ToString())] += 1;
                                }
                                else
                                {
                                    dataDict[int.Parse(num.ToString())] = 1;
                                }
                            }
                        }
                    }
                }
                maxDict = dataDict.OrderByDescending(p => p.Value).Take(7).ToDictionary(p => p.Key, p => p.Value);
            }
            maxList.Add(maxDict);
            return maxList;
        }
        public List<Dictionary<int, int>> GetLotteryDataMinFrequency(List<LotteryBaseInfo> baseInfoList)
        {
            List<Dictionary<int, int>> minList = new List<Dictionary<int, int>>();
            Dictionary<int, int> minDict = new Dictionary<int, int>();
            Dictionary<int, int> dataDict = new Dictionary<int, int>();
            if (baseInfoList != null)
            {
                foreach (var baseinfo in baseInfoList)
                {
                    if (baseinfo.DetailInfo.result != null)
                    {
                        ArrayList result = (ArrayList)baseinfo.DetailInfo.result["result"];
                        foreach (var i in result)
                        {
                            Dictionary<string, object> ballDict = (Dictionary<string, object>)i;
                            string ballColor = ballDict["key"].ToString();
                            ArrayList ballData = (ArrayList)ballDict["data"];
                            foreach (var num in ballData)
                            {
                                if (dataDict.Keys.Contains(int.Parse(num.ToString())))
                                {
                                    dataDict[int.Parse(num.ToString())] += 1;
                                }
                                else
                                {
                                    dataDict[int.Parse(num.ToString())] = 1;
                                }
                            }
                        }
                    }
                }
                minDict = dataDict.OrderBy(p => p.Value).Take(7).ToDictionary(p => p.Key, p => p.Value);
            }
            minList.Add(minDict);
            return minList;
        }
        public List<Dictionary<int, int>> GetLotteryDataRandom(List<LotteryBaseInfo> baseInfoList)
        {
            List<Dictionary<int, int>> randomList = new List<Dictionary<int, int>>();
            return randomList;
        }
    }
}
