using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LotterySpider.Business.LotteryInfo
{
    public class LotteryFactory : ILotteryFactory
    {
        public List<LotterySerialNo> CreateSerialNo(LotteryBasicInfo info)
        {
            List<LotterySerialNo> serialNos = new List<LotterySerialNo>();
            return serialNos;
        }
        public List<Dictionary<int, int>> GetLotteryDataMaxFrequency(List<LotteryBaseInfo> baseInfoList)
        {
            List<Dictionary<int, int>> maxList = new List<Dictionary<int, int>>();
            Dictionary<int, int> maxDict = new Dictionary<int, int>();
            maxList.Add(maxDict);
            return maxList;
        }
        public List<Dictionary<int, int>> GetLotteryDataMinFrequency(List<LotteryBaseInfo> baseInfoList)
        {
            List<Dictionary<int, int>> minList = new List<Dictionary<int, int>>();
            Dictionary<int, int> minDict = new Dictionary<int, int>();
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
