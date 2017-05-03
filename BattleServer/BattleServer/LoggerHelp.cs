using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BattleServer
{
    public class LogHelper
    {
        private static readonly LogHelper Instance = new LogHelper();
        public static LogHelper GetLogHelper()
        {
            return Instance;
        }
        public static void Log(string msg)
        {
            GetLogHelper().CreateLog(msg);
        }
        #region 公共属性
        public string StrStartupPath
        {
            get { return Environment.CurrentDirectory; }
        }
        public string FileName
        {
            get
            {
                return StrStartupPath + @"\Logs" + @"\SyncLog" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            }
        }

        #endregion

        #region 写日志

        public void CreateLog(string strMsg)
        {

            //1. 判断目录是否存在  
            var fileLocation = StrStartupPath + @"\Logs";
            if (!Directory.Exists(fileLocation))
            {
                Directory.CreateDirectory(fileLocation);
            }

            //2. 日志写入  
            using (StreamWriter myWriter = new StreamWriter(FileName, true))
            {
                try
                {
                    myWriter.WriteLine(DateTime.Now.ToString("yy/MM/dd HH:mm:ss:fff") + "," + strMsg);
                    myWriter.WriteLine("");
                }
                finally
                {
                    myWriter.Close();
                }
            }

        }
        #endregion

    }  
}
