using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadVideo.Common {
     public  class Log {

        private const string _baseDirectory = @"";
        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteErrorLog(string msg) {
            string path = _baseDirectory+"errorLog.txt";
            File.AppendAllLines(path, new string[]{msg});

        }
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="listStr"></param>
        /// <param name="logName"></param>
        public static void WriteLog(List<string> listStr, string logName) {
            string path = _baseDirectory + logName;
            File.AppendAllLines(path, listStr);
        }
        /// <summary>
        /// 写入日志文件
        /// </summary>
        /// <param name="content">文件内容</param>
        /// <param name="logName">文件名</param>
        public static void WriteLog(string content, string logName) {
            string path = _baseDirectory + logName;
            File.AppendAllLines(path, new string[]{content});

        }
        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="logName">文件名</param>
        /// <returns></returns>
        public static string ReadLog(string logName) {
            string path = _baseDirectory + logName;
            if (!File.Exists(path)) {
                throw new Exception("未找到文件");
            }
            using (StreamReader sr = File.OpenText(path)) {
                return sr.ReadLine();
            }
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="logName">文件名</param>
        public static void DeleteFile(string logName) {
            string filePath = _baseDirectory+logName;
            if (File.Exists(filePath)) {
                File.Delete(filePath);
            }
        }
         /// <summary>
         /// 读取整个文件
         /// </summary>
         /// <param name="logName"></param>
         /// <returns></returns>
        public static string ReadAllFile(string logName) {
            string path = logName;
            if (File.Exists(path)) {
              return  File.ReadAllText(path);
            }
            return "";
        }
    }
}
