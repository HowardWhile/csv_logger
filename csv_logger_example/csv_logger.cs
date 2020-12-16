using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIM.Modules
{   
    class csv_logger
    {
        public enum RollingInterval
        {
            //
            // 摘要:
            //     The log file will never roll; no time period information will be appended to
            //     the log file name.
            Infinite = 0,
            //
            // 摘要:
            //     Roll every year. Filenames will have a four-digit year appended in the pattern
            //     yyyy
            //     .
            Year = 1,
            //
            // 摘要:
            //     Roll every calendar month. Filenames will have
            //     yyyyMM
            //     appended.
            Month = 2,
            //
            // 摘要:
            //     Roll every day. Filenames will have
            //     yyyyMMdd
            //     appended.
            Day = 3,
            //
            // 摘要:
            //     Roll every hour. Filenames will have
            //     yyyyMMddHH
            //     appended.
            Hour = 4,
            //
            // 摘要:
            //     Roll every minute. Filenames will have
            //     yyyyMMddHHmm
            //     appended.
            Minute = 5
        }

        public csv_logger(
            string i_filename,
            RollingInterval i_rollingInterval = RollingInterval.Infinite,
            int i_retainedFileCountLimit = 10 // 
            )
        {
            //this.filename = i_filename;
            var fullpath = Path.GetFullPath(i_filename);
            this.file_name = Path.GetFileNameWithoutExtension(fullpath);
            this.file_ext = Path.GetExtension(fullpath);
            this.file_path = Path.GetDirectoryName(fullpath);

            this.search_pattern = $"{file_name}*{file_ext}";

            this.rollingInterval = i_rollingInterval;

            this.retainedFileCountLimit = i_retainedFileCountLimit;

            switch (i_rollingInterval)
            {
                case RollingInterval.Infinite:
                    this.filenames_appended = "";
                    break;
                case RollingInterval.Year:
                    this.filenames_appended = "yyyy";
                    break;
                case RollingInterval.Month:
                    this.filenames_appended = "yyyyMMdd";
                    break;
                case RollingInterval.Day:
                    this.filenames_appended = "yyyyMMddHHmm";
                    break;
                case RollingInterval.Hour:
                    this.filenames_appended = "yyyyMMddHH";
                    break;
                case RollingInterval.Minute:
                    this.filenames_appended = "yyyyMMddHHmm";
                    break;
                default:
                    break;
            }
        }

        private string file_path;
        private string file_name;
        private string file_ext;
        private string search_pattern;

        private RollingInterval rollingInterval;
        private string filenames_appended;

        private int retainedFileCountLimit;

        public void WriteRecords(List<dynamic> iRecords)
        {
            string path = this.rollingInterval == RollingInterval.Infinite ?
                $"{this.file_path}\\{this.file_name}{this.file_ext}":
                $"{this.file_path}\\{this.file_name}-{System.DateTime.Now.ToString(this.filenames_appended)}{this.file_ext}";

            bool file_exists = File.Exists(path);
            if (!file_exists)
                Directory.CreateDirectory(this.file_path);

            try
            {
                using (var stream = File.Open(path, FileMode.Append))
                using (var writer = new StreamWriter(stream))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.Configuration.HasHeaderRecord = !file_exists; // Don't write the header again. if file exit
                    csv.WriteRecords(iRecords);
                }
            }
            catch (Exception ex)
            {

                
            }
            

            this.check_retaine();
        }
        public void WriteRecord(dynamic iRecord)
        {
            var records = new List<dynamic>();
            records.Add(iRecord);
            this.WriteRecords(records);
        }

        private void check_retaine()
        {
            // 確認保留檔案的數量

            DirectoryInfo di = new DirectoryInfo(this.file_path);
            FileInfo[] fi = di.GetFiles(this.search_pattern);
            if (fi.Length > this.retainedFileCountLimit)
            {
                try
                {
                    FileInfo[] f_info_sort = fi.OrderBy(f => f.LastWriteTime).ToArray();
                    File.Delete(f_info_sort[0].FullName);
                }
                catch (Exception ex)
                {

                    System.Console.WriteLine(
                        $">>{ex.Message}\r\n" +
                        $"{ex.ToString()}");
                }
            }
        }
    }
}
