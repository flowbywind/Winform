using System;
using System.Windows.Forms;
using DownloadVideo.Common;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using DownloadVideo.Model;
using System.ComponentModel;
using System.Threading;
using System.Linq;
using System.Security.Policy;
namespace DownloadVideo {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
            InitData();
           
        }
        
        private List<string> _ListUrl = new List<string>();
        /// <summary>
        /// 下载列表
        /// </summary>
        private Dictionary<string, DownloadViewModel> _DownloadDic;
        private Dictionary<VideoSourceType, IList<UrlCategoryViewModel>> _DownloadSoureDic;
        private Dictionary<VideoSourceType, IList<Regex>> _RegexDic;
        private List<string> _DownloadingUrlList; ///正在下载的视频
        private Dictionary<string,BackgroundWorker> _BgWorkerDict; //后台线程列表
        private Dictionary<string,ManualResetEvent> _ManualRestDict; //
        private string _DownloadPath=string.Empty;
        #region 轮询程序 
        /// <summary>
        /// 初始化数据
        /// </summary>
        public void InitData() { 
          //
            _DownloadDic = new Dictionary<string, DownloadViewModel>();
            _DownloadSoureDic = new Dictionary<VideoSourceType, IList<UrlCategoryViewModel>>();
            //添加优酷视频下载地址
            _DownloadSoureDic.Add(VideoSourceType.Youku, new List<UrlCategoryViewModel> {
                new UrlCategoryViewModel{ Category=CategoryType.fun, UrlCategory="http://fun.youku.com/" },
                new UrlCategoryViewModel{ Category=CategoryType.fun, UrlCategory="http://dv.youku.com/" }
            });
            _RegexDic = new Dictionary<VideoSourceType, IList<Regex>>();
            _RegexDic.Add(VideoSourceType.Youku, new List<Regex>() {
               new Regex(@"<a[^>]*href=[""']http://v.youku.com(?<url>[^""']*?)[""'][^>]*></a>"),//获取优酷页面中视频链接 1
               new Regex(@"<a[^>]*href=[""']http://v.youku.com(?<url>[^""']*?)[""'][^>]*>[\w][^<][^\b&nbsp;]+</a>"),//获取优酷页面中视频链接 2
               new Regex(@"http://([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\.,@?^=%&amp;:/~\+#])?"), //a标签中的获取url
               new Regex(@""">[\w\W]+"), //a标签中获取视频名称 1 如：<a>我是一只小小鸟</a>
               new Regex(@"href=""http://k.youku.com([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?"), //获取视频下载链接
            });
            //初始化多任务列表集合
            _BgWorkerDict = new Dictionary<string, BackgroundWorker>();
            _ManualRestDict = new Dictionary<string,ManualResetEvent>();
        }
        
        #endregion
        #region 1、获取视频列表
        public void GetVideoList() {
            if (_DownloadSoureDic == null) return;
            foreach (var source in _DownloadSoureDic) {

                if (source.Value != null && source.Value.Count > 0) {
                    foreach (UrlCategoryViewModel urlCategory in source.Value) {
                        Regex rx = _RegexDic[VideoSourceType.Youku][1];
                        Regex rxTitle = _RegexDic[VideoSourceType.Youku][3];
                        Regex rxUrl = _RegexDic[VideoSourceType.Youku][2];
                        string url = urlCategory.UrlCategory;
                        var response =  HttpWebResponseUtility.CreateGetHttpResponse(url, null, "", null);
                        using (Stream stream = response.GetResponseStream()) {
                            using (StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8)) {
                                string responseStr = reader.ReadToEnd();
                                if (rx.IsMatch(responseStr)) {
                                    MatchCollection mc = rx.Matches(responseStr);
                                    foreach (Match item in mc) {
                                        if (item.Success) {
                                            string a = item.Value;
                                            string title =string.Empty;
                                            string urlSource = string.Empty;
                                            if (string.IsNullOrEmpty(a)) { continue; } //过滤空字符串
                                            if (!rxTitle.IsMatch(a)) {
                                                continue; 
                                            }
                                            title = rxTitle.Match(a).Value;
                                            //过滤视频名称
                                            title = title.Replace(@""">", "").Replace(@"</a>","").Replace("?","").Replace(":","").Replace("*","").Replace("<","").Replace(">","").Trim();
                                            if (!rxUrl.IsMatch(a)) {
                                                continue;
                                            }
                                            urlSource=rxUrl.Match(a).Value.Trim();
                                            if (string.IsNullOrEmpty(title)) { continue; }
                                            if(string.IsNullOrEmpty(urlSource)) {continue;}
                                            if (!_DownloadDic.ContainsKey(title) ) {
                                                DownloadViewModel model = new DownloadViewModel() {
                                                    Guid = Guid.NewGuid().ToString(),
                                                    Title=title,
                                                    Category = urlCategory.Category,
                                                    Status = DownloadStatus.HasCreated,
                                                    UrlSource = urlSource,
                                                    VideoSourceType = source.Key,
                                                    CreatedTime = DateTime.Now,
                                                };
                                                _DownloadDic.Add(title, model);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        #endregion
        #region 2、解析视频地址
        public async Task ConvertToVideoUrl() {
            foreach (var item in _DownloadDic) {
                await  GetVideoUrl(item.Value);
            }
        }
        private async Task GetVideoUrl(DownloadViewModel model) {
            string url = "http://www.flvcd.com/parse.php?format=&kw={0}";
            if (string.IsNullOrEmpty(model.UrlSource)) { return; }
            url = string.Format(url, model.UrlSource);
            var response = await HttpWebResponseUtility.CreateGetHttpResponseAsync(url, null, "", null);
            using (Stream stream = response.GetResponseStream()) {
                using (StreamReader reader = new StreamReader(stream, System.Text.Encoding.GetEncoding("gb2312"))) {
                    string responseStr = reader.ReadToEnd();
                    Regex rx = _RegexDic[VideoSourceType.Youku][4];
                    model.UrlDownloadList=new List<string>();
                    if (rx.IsMatch(responseStr)) {
                        MatchCollection mc = rx.Matches(responseStr);
                        foreach (var item in mc) {
                            string downloadurl = item.ToString().Replace(@"href=""", "");
                            model.UrlDownloadList.Add(downloadurl);
                            model.Status = DownloadStatus.PreDownload;
                        }
                    }

                }
            }
        }
        #endregion
        #region 3、下载视频
        public void DownloadVideo() {
            //清空多线程任务列表数据
            //_BgWorkerDict.Clear();
            //_ManualRestDict.Clear();
            //转为后台多线程任务
            this.Invoke((MethodInvoker)delegate{
                int hasDownloadCount = (from u in _DownloadDic
                                        where u.Value.Status == DownloadStatus.Downloaded
                                        select u).Count();
                this.progressBarDownload.Maximum = _DownloadDic.Count;
                this.progressBarDownload.Value = hasDownloadCount;
                this.lblProgress.Text = hasDownloadCount + "/" + _DownloadDic.Count;
            });
            while (_BgWorkerDict.Count <= 5) {
                var model = (from u in _DownloadDic
                            let down = u.Value.Status
                            where down == DownloadStatus.PreDownload
                            select u.Value).First();
                if (model != null) {
                    if (_BgWorkerDict.ContainsKey(model.Title)) {
                        continue;
                    }
                    //创建多线程下载任务
                    BackgroundWorker bg = new BackgroundWorker();
                    bg.DoWork += bg_DoWork;
                    bg.RunWorkerCompleted += (sender, e) => {
                        bg_RunWorkerCompleted(sender, e, model.Title);
                    };
                    bg.WorkerSupportsCancellation = true;
                    _BgWorkerDict.Add(model.Title, bg);
                    
                    model.Status = DownloadStatus.Downloading;
                    ManualResetEvent mre = new ManualResetEvent(true);
                    _ManualRestDict.Add(model.Title, mre);
                    bg.RunWorkerAsync(model);
                }
                else {
                    break;
                }

            }
        }

        void bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e,string key) {
            var bg = sender as BackgroundWorker;
            if (_BgWorkerDict.ContainsKey(key)) {
                _BgWorkerDict.Remove(key);
            }
            if (_DownloadDic.ContainsKey(key)) {
                var model = _DownloadDic[key];
                model.Status = DownloadStatus.Downloaded;
            }
            int surplusCount = (from u in _DownloadDic
                               where u.Value.Status == DownloadStatus.PreDownload
                               select u.Key).Count();
            if (surplusCount > 0) {
                DownloadVideo();
            }
        }

        void bg_DoWork(object sender, DoWorkEventArgs e) {
            var model=e.Argument as DownloadViewModel;
            if(model==null) return;
            if (model.UrlDownloadList.Count == 1) {
                using (var client = new WebClient()) {
                    string file = string.Format(@"{0}\{1}.flv", _DownloadPath, model.Title);
                    //client.DownloadFileAsync(new Uri(model.UrlDownloadList[0]), file);
                    //client.DownloadDataCompleted += client_DownloadFileCompleted;
                   
                    client.DownloadFile(new Uri(model.UrlDownloadList[0]), file);
                }
            }
            else if (model.UrlDownloadList.Count > 1) {
                string multifilePath = _DownloadPath + "\\" + model.Title;
                if (!Directory.Exists(multifilePath)) {
                    Directory.CreateDirectory(multifilePath);
                }
                for (int i = 0; i < model.UrlDownloadList.Count; i++) {
                    using (var client = new WebClient()) {
                        string file = string.Format(@"{0}\{1}.flv", multifilePath, model.Title + "_" + i);
                        //client.DownloadFileAsync(new Uri(model.UrlDownloadList[0]), file);
                        //client.DownloadDataCompleted += client_DownloadFileCompleted;
                        client.DownloadFile(new Uri(model.UrlDownloadList[0]), file);
                    }
                }

            }
        }

        void client_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e) {
            
            if (e.Cancelled) { 
            
            }
            if (e.Error != null) { 
            
            } 
            
           
            
        }

        
        #endregion


        private void Form1_Load(object sender, EventArgs e) {

        }

        private void BtnAuto_Click(object sender, EventArgs e) {
            CreateDownloadDirectory();
            GetVideoList();
            Task t =  ConvertToVideoUrl();
            t.ContinueWith((task) => {
                if (task.Status == TaskStatus.RanToCompletion) {
                    DownloadVideo();
                }
            });
        }
        /// <summary>
        /// 创建下载路径
        /// </summary>
        private void CreateDownloadDirectory() {
            string today = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
            _DownloadPath = @"E:\VideoDownload\" + today;
            if (!Directory.Exists(_DownloadPath)) {
                Directory.CreateDirectory(_DownloadPath);
            }
        }
       
        /// <summary>
        /// 控制暂停结束
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnControl_Click(object sender, EventArgs e) {
            Button b=sender as Button;
            if (b.Text == "暂停") {
                b.Text = "继续";
                foreach(var item in _ManualRestDict){
                  ManualResetEvent m = item.Value as ManualResetEvent;
                  m.Reset();
                }
            }
            if (b.Text == "继续") {
                b.Text = "暂停";
                foreach (var item in _ManualRestDict) {
                    ManualResetEvent m = item.Value as ManualResetEvent;
                    m.Set();
                }
            }
        }


        #region 单个视频下载

        private async void BtnGetVideoUrl_Click(object sender, EventArgs e) {
            string url = "http://www.flvcd.com/parse.php?format=&kw={0}";
            if (string.IsNullOrEmpty(TxtVideoUrl.Text.Trim())) {
                MessageBox.Show("请输入视频地址");
                return;
            }
            url = string.Format(url, TxtVideoUrl.Text.Trim());
            var response = await HttpWebResponseUtility.CreateGetHttpResponseAsync(url, null, "", null);

            using (Stream stream = response.GetResponseStream()) {
                using (StreamReader reader = new StreamReader(stream, System.Text.Encoding.GetEncoding("gb2312"))) {
                    string responseStr = reader.ReadToEnd();
                    string subStr = "下载地址：<a href=\"";
                    int startIndex = responseStr.IndexOf(subStr) + 14;
                    int endIndex = responseStr.IndexOf("\" target=\"_blank\"", startIndex);
                    //string videoUrl = responseStr.Substring(startIndex, endIndex - startIndex);
                    //TxtResult.Text = videoUrl;

                    //获取视频名称
                    startIndex = responseStr.IndexOf("<title>") + 7;
                    endIndex = responseStr.IndexOf("FLVCD硕鼠官网|FLV下载", startIndex);
                    string title = responseStr.Substring(startIndex, endIndex - startIndex);
                    txtVideoTitle.Text = title;

                    Regex rx = _RegexDic[VideoSourceType.Youku][1];
                    _ListUrl.Clear();
                    TxtResult.Text = "";
                    if (rx.IsMatch(responseStr)) {
                        MatchCollection mc = rx.Matches(responseStr);
                        foreach (var item in mc) {
                            string downloadurl = item.ToString().Replace(@"href=""", "");
                            _ListUrl.Add(downloadurl);
                            TxtResult.Text += downloadurl + ";";
                        }
                    }

                }
            }
        }

        private void BtnDownload_Click(object sender, EventArgs e) {
            int i = 0;
            foreach (string url in _ListUrl) {
                i++;
                using (var client = new WebClient()) {
                    if (!Directory.Exists(@"E:\VideoDownload")) {
                        Directory.CreateDirectory(@"E:\VideoDownload");
                    }
                    string file = string.Format(@"E:\VideoDownload\{0}.flv", "tmp" + i);
                    client.DownloadFileAsync(new Uri(url), file);
                }
            }
        }
        #endregion

    }
}
