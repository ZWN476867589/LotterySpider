using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LotterySpider.Business
{
    public interface ILotteryFactory
    {
        List<LotterySerialNo> CreateSerialNo(LotteryBasicInfo info);
    }
}
