using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using LotterySpider.DataBase;
using System.Collections;

namespace LotterySpider.Business.UtilTools
{
    public static class LotteryDataUtils
    {
        public static void InsertLotteryOriginDataListToDB(List<LotteryOriginData> dataList)
        {
            using (SQLiteTransaction tran = DBHelper.SQLConn.BeginTransaction())
            {
                foreach (var i in dataList)
                {
                    SQLiteCommand cmd = new SQLiteCommand(DBHelper.SQLConn);
                    cmd.Transaction = tran;
                    cmd.CommandText = "insert into LotteryOriginData values(@RowID,@LotteryTypeID,@OriginData,@DataUrl,@Time,@SerialNo)";
                    cmd.Parameters.AddRange(new[]{
                    new SQLiteParameter("@RowID",null),
                    new SQLiteParameter("@LotteryTypeID",i.LotteryTypeID),
                    new SQLiteParameter("@OriginData",i.OriginData),
                    new SQLiteParameter("@DataUrl",i.DataUrl),
                    new SQLiteParameter("@Time",i.Time),
                    new SQLiteParameter("@SerialNo",i.SerialNo),
                    });
                    cmd.ExecuteNonQuery();
                }
                tran.Commit();
            }
        }
        public static void InsertLotterySerialNoListToDB(List<LotterySerialNo> serials)
        {
            using (SQLiteTransaction tran = DBHelper.SQLConn.BeginTransaction())
            {
                foreach (var i in serials)
                {
                    SQLiteCommand cmd = new SQLiteCommand(DBHelper.SQLConn);
                    cmd.Transaction = tran;
                    cmd.CommandText = "insert into LotterySerialNo values(@RowID,@SerialID,@LotteryTypeID,@OpenTime)";
                    cmd.Parameters.AddRange(new[]{
                    new SQLiteParameter("@RowID",null),
                    new SQLiteParameter("@SerialID",i.SerailNo),
                    new SQLiteParameter("@LotteryTypeID",i.LotteryTypeID),
                    new SQLiteParameter("@OpenTime",i.OpenTime),
                    });
                    cmd.ExecuteNonQuery();
                }
                tran.Commit();
            }
        }
        public static List<LotteryOriginData> LoadLotteryOriginData(LotteryBasicInfo info, List<LotteryOriginData> dataList)
        {
            SQLiteDataReader Reader = DBHelper.Query("select * from LotteryOriginData where lotterytypeid = " + info.LotteryTypeID);
            while (Reader.Read())
            {
                LotteryOriginData data = new LotteryOriginData()
                {
                    RowID = Reader.GetInt32(0),
                    LotteryTypeID = Reader.GetInt32(1),
                    OriginData = Reader.GetString(2),
                    DataUrl = Reader.GetString(3),
                    SerialNo = Reader.GetString(4),
                    Time = Reader.GetString(5),
                };
                dataList.Add(data);
            }
            return dataList;
        }
        public static List<LotteryBaseInfo> ConvertOriginDataToLotteryBaseInfo(List<LotteryOriginData> originDataList)
        {
            List<LotteryBaseInfo> BaseInfoList = new List<LotteryBaseInfo>();
            foreach (var originData in originDataList)
            {
                Dictionary<string, object> dataDict = JsonHelper.JsonToDictionary(originData.OriginData);
                if (dataDict != null)
                {
                    LotteryBaseInfo BaseInfo = new LotteryBaseInfo();
                    EntityHelper.setPropertiseValue<LotteryBaseInfo>(dataDict, BaseInfo);
                    EntityHelper.setPropertiseValue<LotteryDetailInfo>(BaseInfo.data, BaseInfo.DetailInfo = new LotteryDetailInfo());
                    BaseInfoList.Add(BaseInfo);
                }
            }
            return BaseInfoList;
        }
        public static int GetRandomInt(int seed, int maxValue)
        {
            Random rand = new Random(seed);
            int num = rand.Next(maxValue);
            return num;
        }
    }
}
