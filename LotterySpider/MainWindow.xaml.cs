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
            CreateLotterySerialNo();
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
    }
}
