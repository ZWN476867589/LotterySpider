using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Text;
using System.Windows;
using System.Data.SQLite;
using LotterySpider;
using LotterySpider.Business;
using LotterySpider.DataBase;
using LotterySpider.Business.LotteryInfo;
using LotterySpider.Business.UtilsTools;

namespace LotterySpider
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        public void GetSSQInfoByYearAndMonth(DateTime time)
        {
            string baseUrl = @"http://baidu.lecai.com/lottery/draw/ajax_get_detail.php?lottery_type=50&phase={0}";
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(baseUrl);
            request.Method = "GET";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.KeepAlive = true;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
            using (WebResponse wr = request.GetResponse())
            {
                Stream st = wr.GetResponseStream();
                StreamReader sr = new StreamReader(st, Encoding.UTF8);
                string result = sr.ReadToEnd();
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadLotteryBasicInfo();
        }
        List<LotteryBasicInfo> infos = new List<LotteryBasicInfo>();
        public void LoadLotteryBasicInfo()
        {
            DBHelper.LoadDB("Lottery.db");
            string sqlStr = "select * from lotterybasicinfo";
            SQLiteDataReader reader = DBHelper.Query(sqlStr);
            while (reader.Read())
            {
                LotteryBasicInfo info = new LotteryBasicInfo()
                {
                    LotteryID = reader.GetInt32(0),
                    LotteryTypeID = reader.GetInt32(1),
                    LotteryName = reader.GetString(2),
                    LotteryShortCode = reader.GetString(3),
                    StartSellTime = reader.GetString(5),
                };
                info.SetOpenTimeOfWeek(reader.GetString(4));
                infos.Add(info);
            }
            cmbLotteryName.ItemsSource = infos;
            cmbLotteryName.SelectedValuePath = "LotteryTypeID";
            cmbLotteryName.DisplayMemberPath = "LotteryName";
        }
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (cmbLotteryName.SelectedIndex == -1)
            {
                MessageBox.Show("请选择彩票.");
                return;
            }
            GetLotteryOriginData();
        }
        public void GetLotteryOriginData()
        {
            string LotteryID = cmbLotteryName.SelectedValue.ToString();
            LotteryBasicInfo info = infos.FirstOrDefault(p => p.LotteryTypeID.ToString() == LotteryID);
            if (info != null)
            {
                string baseUrl = @"http://baidu.lecai.com/lottery/draw/ajax_get_detail.php?lottery_type={0}&phase={1}";
                SQLiteDataReader Reader = DBHelper.Query("select * from LotterySerialNo where lotterytypeid = " + info.LotteryTypeID);
                List<LotterySerialNo> numList = new List<LotterySerialNo>();
                List<LotteryOriginData> dataList = new List<LotteryOriginData>();
                while (Reader.Read())
                {
                    LotterySerialNo num = new LotterySerialNo()
                    {
                        RowID = Reader.GetInt32(0),
                        SerailNo = Reader.GetString(1),
                        LotteryTypeID = Reader.GetInt32(2),
                        OpenTime = Reader.GetString(3),
                    };
                    numList.Add(num);
                }
                foreach (var num in numList)
                {
                    string url = String.Format(baseUrl, num.LotteryTypeID, num.SerailNo);
                    string res = NetHelper.GetByUrl(url);
                    if (!String.IsNullOrWhiteSpace(res))
                    {
                        LotteryOriginData data = new LotteryOriginData()
                        {
                            LotteryTypeID = num.LotteryTypeID,
                            OriginData = res,
                            DataUrl = url,
                            Time = num.OpenTime,
                            SerialNo = num.SerailNo,
                        };
                        dataList.Add(data);
                    }
                }
                InsertLotteryOriginDataList(dataList);
            }
        }
        public void CreateLotterySerialNo()
        {
            string LotteryID = cmbLotteryName.SelectedValue.ToString();
            LotteryBasicInfo info = infos.FirstOrDefault(p => p.LotteryTypeID.ToString() == LotteryID);
            if (info != null)
            {
                switch (info.LotteryShortCode)
                {
                    case "SSQ":
                        CreateSSQSerialNo(info);
                        break;
                    case "DLT":
                        CreateDLTSerialNo(info);
                        break;
                    case "QXC":
                        CreateQXCSerialNo(info);
                        break;
                    default:
                        break;
                }
            }
        }
        public void CreateSSQSerialNo(LotteryBasicInfo info)
        {
            LotterySSQ SSQ = new LotterySSQ();
            InsertLotterySerialNoList(SSQ.CreateSerialNo(info));
        }
        public void CreateDLTSerialNo(LotteryBasicInfo info)
        {
            LotteryDLT DLT = new LotteryDLT();
            InsertLotterySerialNoList(DLT.CreateSerialNo(info));
        }
        public void CreateQXCSerialNo(LotteryBasicInfo info)
        {
            LotteryQXC QXC = new LotteryQXC();
            InsertLotterySerialNoList(QXC.CreateSerialNo(info));
        }
        public void InsertLotterySerialNoList(List<LotterySerialNo> serials)
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
        public void InsertLotteryOriginDataList(List<LotteryOriginData> dataList)
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
        private void btnCreateSerialNo_Click(object sender, RoutedEventArgs e)
        {
            if (cmbLotteryName.SelectedIndex == -1)
            {
                MessageBox.Show("请选择彩票.");
                return;
            }
            CreateLotterySerialNo();
        }

        private void btnAnalyseData_Click(object sender, RoutedEventArgs e)
        {
            if (cmbLotteryName.SelectedIndex == -1)
            {
                MessageBox.Show("请选择彩票.");
                return;
            }
            AnslyseLotteryData();
        }
        public void AnslyseLotteryData()
        {
            string LotteryID = cmbLotteryName.SelectedValue.ToString();
            LotteryBasicInfo info = infos.FirstOrDefault(p => p.LotteryTypeID.ToString() == LotteryID);
            if (info != null)
            {
                List<LotteryOriginData> dataList = new List<LotteryOriginData>();
                dataList = LoadLotteryOriginData(info,dataList);
                switch (info.LotteryShortCode)
                {
                    case "SSQ":
                        LotterySSQ SSQ = new LotterySSQ();
                        SSQ.AnalyseOriginData(dataList);
                        break;
                    default:
                        break;
                }
            }
        }
        public List<LotteryOriginData> LoadLotteryOriginData(LotteryBasicInfo info, List<LotteryOriginData> dataList)
        {
            SQLiteDataReader Reader = DBHelper.Query("select * from LotteryOriginData where lotterytypeid = " + info.LotteryTypeID);
            while (Reader.Read())
            {
                LotteryOriginData data = new LotteryOriginData() { 
                RowID = Reader.GetInt32(0),
                LotteryTypeID = Reader.GetInt32(1),
                OriginData= Reader.GetString(2),
                DataUrl = Reader.GetString(3),
                Time = Reader.GetString(4),
                };
                dataList.Add(data);
            }
            return dataList;
        }
    }
}
