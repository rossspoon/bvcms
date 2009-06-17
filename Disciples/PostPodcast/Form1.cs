using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using PostPodcast.Properties;

namespace PostPodcast
{
    public partial class Form1 : Form
    {
        WebSvc.WebService1 ws = new WebSvc.WebService1();
        private string key, code;

        public Form1()
        {
            InitializeComponent();
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
            canPostFor.SelectedValueChanged += new EventHandler(canPostFor_SelectedValueChanged);
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            FileName.LostFocus += new EventHandler(FileName_LostFocus);
        }

        void FileName_LostFocus(object sender, EventArgs e)
        {
            posted.Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            getGuid();
            if (!IsDev)
                ws.Url = "http://disciples.bellevue.org/Webservice1.asmx";
                //ws.Url = "http://localhost:53135/BellevueTeachers/Webservice1.asmx";

            time[] ta = new time[] { 
                new time(8,0), new time(9, 30),new time(11, 0),
                new time(16, 0),new time(18, 0),new time(18, 30), new time(20, 0)};
            Time.Items.AddRange(ta);

            string[] a = ws.GetAuthorization("acts412", guid, out key, out code);
            if (a.Length == 0)
                return;
            for (int i = 0; i < a.Length; i += 2)
                canPostFor.Items.Add(new poster(a[i], a[i + 1]));
            canPostFor.SelectedIndex = 0;
            canPostFor.Focus();

            Description.Text = Settings.Default.Description;
            Title.Text = Settings.Default.Title;
            pubDate = Settings.Default.Date;
            openFileDialog1.InitialDirectory = Settings.Default.Directory;
        }

        void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.Description = Description.Text;
            Settings.Default.Title = Title.Text;
            Settings.Default.Date = pubDate;
            Settings.Default.Directory = Path.GetDirectoryName(openFileDialog1.FileName);
            Settings.Default.Save();
        }

        void canPostFor_SelectedValueChanged(object sender, EventArgs e)
        {
            poster p = (poster)canPostFor.Items[canPostFor.SelectedIndex];
            Author.Text = p.username;
        }

        private class time
        {
            public DateTime t;
            public time(int h, int m)
            {
                this.t = new DateTime(1, 1, 1, h, m, 0);
            }
            public override string ToString()
            {
                return t.ToShortTimeString();
            }
        }
        private class poster
        {
            public string userid;
            public string username;
            public poster(string userid, string username)
            {
                this.userid = userid;
                this.username = username;
            }
            override public string ToString()
            {
                return string.Format("{0}({1})", userid, username);
            }
        }

        void Cancel_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }

        void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
                MessageBox.Show("Canceled");
            else
                posted.Visible = true;
            EnableAll(true);
            Cursor = Cursors.Default;
            progressBar1.Value = 0;
            FileName.Focus();
        }
        private void EnableAll(bool truefalse)
        {
            FileName.Enabled = truefalse;
            Author.Enabled = truefalse;
            BrowseFile.Enabled = truefalse;
            Title.Enabled = truefalse;
            dateTimePicker1.Enabled = truefalse;
            Time.Enabled = truefalse;
            Part.Enabled = truefalse;
            Description.Enabled = truefalse;
            Post.Enabled = truefalse;
            Cancel.Enabled = !truefalse;
        }

        void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void Post_Click(object sender, EventArgs e)
        {
            EnableAll(false);
            progressBar1.Value = 0;
            Cursor = Cursors.WaitCursor;
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.RunWorkerAsync();
        }

        private void BrowseFile_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            FileName.Text = openFileDialog1.FileName;
        }

        public string TitleAndPart
        {
            get { return GetTitleAndPart(); }
        }
        delegate time GetTimeCallback();
        private time GetTime()
        {
            if (this.Time.InvokeRequired)
            {
                GetTimeCallback d = new GetTimeCallback(GetTime);
                return (time)this.Invoke(d);
            }
            else
            {
                if (Time.SelectedIndex < 0)
                    return (time)Time.Items[1];
                return (time)Time.Items[Time.SelectedIndex];
            }
        }
        delegate string GetTitleAndPartCallback();
        private string GetTitleAndPart()
        {
            if (this.Part.InvokeRequired)
            {
                GetTitleAndPartCallback d = new GetTitleAndPartCallback(GetTitleAndPart);
                return (string)this.Invoke(d);
            }
            else
                if (Part.Value == 0)
                    return Title.Text;
                else
                    return Title.Text + ", part " + Part.Value.ToString();
        }


        public DateTime pubDate
        {
            get
            {
                time time = GetTime();
                return new DateTime(
                    dateTimePicker1.Value.Year,
                    dateTimePicker1.Value.Month,
                    dateTimePicker1.Value.Day,
                    time.t.Hour,
                    time.t.Minute, 0);
            }
            set
            {
                int i = Time.FindString(value.ToShortTimeString());
                Time.SelectedIndex = i;
                if (value.Year == 1980)
                    dateTimePicker1.Value = DateTime.Now;
                else
                    dateTimePicker1.Value = value.Date;
            }
        }
        public string User
        {
            get
            {
                poster p = (poster)canPostFor.Items[canPostFor.SelectedIndex];
                return p.userid;
            }
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;
            worker.WorkerReportsProgress = true;

            string s3name = string.Format("{0}-{1:yyMMddhhmm}-{2}.mp3",
                User, pubDate, Part.Value);
            s3name = s3name.Replace(' ', '_');

            AmazonS3REST.AWSAuthConnection s3 = new AmazonS3REST.AWSAuthConnection(key, code);
            string sContentType = "audio/mp3";
            SortedList sList = new SortedList();
            sList.Add("Content-Type", sContentType);
            sList.Add("x-amz-acl", "public-read");  // Set access control list to "publicly readable"
            s3.PutObjectAsStream("http://podcast.bellevueteachers.com.s3.amazonaws.com", s3name, FileName.Text, sList, worker, e);
            if (worker.CancellationPending)//http://podcast.bellevueteachers.com.s3.amazonaws.com/davcar-0705060930-1.mp3
                return;

            FileInfo fi = new FileInfo(FileName.Text);
            //string id = PostBlog(TitleAndPart, Description.Text, Author.Text, s3name, pubDate);

            ws.PostPodcast(key, User, TitleAndPart, Description.Text, pubDate, s3name, (int)fi.Length);
        }
        private NameValueCollection GetQueryStringParameters()
        {
            string[] ad = AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData;
            if (ad == null)
                return new NameValueCollection();
            string fullUrl = ad[0];
            string[] a = fullUrl.Split(new char[] { '?' }, 2);
            if (a.Length < 2)
                return new NameValueCollection();
            return HttpUtility.ParseQueryString(a[1]);
        }
        private Guid guid;
        private void getGuid()
        {
            guid = new Guid();
            string g = GetQueryStringParameters()["id"];
            if (g != null)
                guid = new Guid(g);
        }
        private bool IsDev
        {
            get { return guid.ToString() == new Guid().ToString(); }
        }
    }
}