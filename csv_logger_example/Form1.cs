using AIM.Modules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace csv_logger_example
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        csv_logger csv_log = new csv_logger("log.csv", csv_logger.RollingInterval.Minute);
        private void button1_Click(object sender, EventArgs e)
        {
            var records = new List<dynamic>();
            dynamic record = new ExpandoObject();
            record.Id = 1;
            record.Name = "one";
            records.Add(record);

            csv_log.WriteRecord(records);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.backgroundWorker1.RunWorkerAsync();
            this.backgroundWorker2.RunWorkerAsync();
            this.backgroundWorker3.RunWorkerAsync();
        }
        csv_logger bg1_csv = new csv_logger(@".\logs\bg1_log.csv", i_rollingInterval: csv_logger.RollingInterval.Minute);
        csv_logger bg2_csv = new csv_logger(@".\logs\bg2_log.csv", i_rollingInterval: csv_logger.RollingInterval.Minute);
        csv_logger bg3_csv = new csv_logger(@".\logs\bg3_log.csv", i_rollingInterval: csv_logger.RollingInterval.Minute);

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            int count = 0;
            while (true)
            {
                Thread.Sleep(50);
                dynamic record = new ExpandoObject();
                record.Id = count;
                record.time = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                record.count_1 = count - 1;
                double FAT = (double)count / 100.0; ;
                record.FAT = FAT;
                record.FAT_2 = FAT * FAT;
                record.FAT = Math.Log(FAT);
                this.bg1_csv.WriteRecord(record);

                count++;
            }
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            int count = 0;
            while (true)
            {
                Thread.Sleep(50);
                dynamic record = new ExpandoObject();
                record.Id = count;
                record.time = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                record.count_1 = count - 1;
                double FAT = (double)count / 100.0; ;
                record.FAT = FAT;
                record.FAT_2 = FAT * FAT;
                record.FAT = Math.Log(FAT);
                this.bg2_csv.WriteRecord(record);

                count++;
            }
        }

        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            int count = 0;
            while (true)
            {
                Thread.Sleep(50);
                dynamic record = new ExpandoObject();
                record.Id = count;
                record.time = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                record.count_1 = count - 1;
                double FAT = (double)count / 100.0; ;
                record.FAT = FAT;
                record.FAT_2 = FAT * FAT;
                record.FAT = Math.Log(FAT);
                this.bg3_csv.WriteRecord(record);

                count++;
            }
        }
    }
}
