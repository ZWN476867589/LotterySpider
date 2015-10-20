using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LotterySpider.Business
{
    public interface ILotteryFactory
    {
        List<LotterySerialNo> CreateSerialNo(LotteryBasicInfo info);
        List<Dictionary<int, int>> GetLotteryDataMaxFrequency(List<LotteryBaseInfo> baseInfoList);
        List<Dictionary<int, int>> GetLotteryDataMinFrequency(List<LotteryBaseInfo> baseInfoList);
        List<Dictionary<int, int>> GetLotteryDataRandom(List<LotteryBaseInfo> baseInfoList);
    }
}
