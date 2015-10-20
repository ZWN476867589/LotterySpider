using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using LotterySpider.DataBase;
using LotterySpider.Business.UtilTools;

namespace LotterySpider.Business.LotteryInfo
{
    public class LotterySSQ : ILotteryFactory
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
            Dictionary<int, int> maxRedDict = new Dictionary<int, int>();
            Dictionary<int, int> maxBlueDict = new Dictionary<int, int>();
            Dictionary<int, int> redDict = new Dictionary<int, int>();
            Dictionary<int, int> blueDict = new Dictionary<int, int>();
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
                            if (ballColor == "red")
                            {
                                foreach (var num in ballData)
                                {
                                    if (redDict.Keys.Contains(int.Parse(num.ToString())))
                                    {
                                        redDict[int.Parse(num.ToString())] += 1;
                                    }
                                    else
                                    {
                                        redDict[int.Parse(num.ToString())] = 1;
                                    }
                                }
                            }
                            if (ballColor == "blue")
                            {
                                foreach (var num in ballData)
                                {
                                    if (blueDict.Keys.Contains(int.Parse(num.ToString())))
                                    {
                                        blueDict[int.Parse(num.ToString())] += 1;
                                    }
                                    else
                                    {
                                        blueDict[int.Parse(num.ToString())] = 1;
                                    }
                                }
                            }
                        }
                    }
                }
                maxRedDict = redDict.OrderByDescending(p=>p.Value).Take(6).OrderBy(p=>p.Key).ToDictionary(p=>p.Key,p=>p.Value);
                maxBlueDict = blueDict.OrderByDescending(p => p.Value).Take(1).OrderBy(p => p.Key).ToDictionary(p => p.Key, p => p.Value);
            }
            maxList.Add(maxRedDict);
            maxList.Add(maxBlueDict);
            return maxList;
        }
        public List<Dictionary<int, int>> GetLotteryDataMinFrequency(List<LotteryBaseInfo> baseInfoList)
        {
            List<Dictionary<int, int>> minList = new List<Dictionary<int, int>>();
            Dictionary<int, int> minRedDict = new Dictionary<int, int>();
            Dictionary<int, int> minBlueDict = new Dictionary<int, int>();
            Dictionary<int, int> redDict = new Dictionary<int, int>();
            Dictionary<int, int> blueDict = new Dictionary<int, int>();
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
                            if (ballColor == "red")
                            {
                                foreach (var num in ballData)
                                {
                                    if (redDict.Keys.Contains(int.Parse(num.ToString())))
                                    {
                                        redDict[int.Parse(num.ToString())] += 1;
                                    }
                                    else
                                    {
                                        redDict[int.Parse(num.ToString())] = 1;
                                    }
                                }
                            }
                            if (ballColor == "blue")
                            {
                                foreach (var num in ballData)
                                {
                                    if (blueDict.Keys.Contains(int.Parse(num.ToString())))
                                    {
                                        blueDict[int.Parse(num.ToString())] += 1;
                                    }
                                    else
                                    {
                                        blueDict[int.Parse(num.ToString())] = 1;
                                    }
                                }
                            }
                        }
                    }
                }
                minRedDict = redDict.OrderBy(p => p.Value).Take(6).OrderBy(p => p.Key).ToDictionary(p => p.Key, p => p.Value);
                minBlueDict = blueDict.OrderBy(p => p.Value).Take(1).OrderBy(p => p.Key).ToDictionary(p => p.Key, p => p.Value);
            }
            minList.Add(minRedDict);
            minList.Add(minBlueDict);
            return minList;
        }
        public List<Dictionary<int, int>> GetLotteryDataRandom(List<LotteryBaseInfo> baseInfoList)
        {
            List<Dictionary<int, int>> randomList = new List<Dictionary<int, int>>();
            Dictionary<int, int> redDict = new Dictionary<int, int>();
            Dictionary<int, int> blueDict = new Dictionary<int, int>();
            while (redDict.Count != 6)
            {
                int num = LotteryDataUtils.GetRandomInt(DateTime.Now.Millisecond, 33);
                if (!redDict.Keys.Contains(num) && num > 0)
                {
                    redDict.Add(num,num);
                }
            }
            while (blueDict.Count != 1)
            {
                int num = LotteryDataUtils.GetRandomInt(DateTime.Now.Millisecond, 16);
                if (!blueDict.Keys.Contains(num) && num>0)
                {
                    blueDict.Add(num, num);
                }
            }
            redDict = redDict.OrderBy(p => p.Key).ToDictionary(p=>p.Key,p=>p.Value);
            randomList.Add(redDict);
            randomList.Add(blueDict);
            return randomList;
        }        
    }
}
