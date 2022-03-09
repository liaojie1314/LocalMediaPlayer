using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 简单播放器
{

    public partial class Form1 : Form
    {
        private ControlResizer Resizer; //定义缩放类
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            musicPlayer.Ctlcontrols.play();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            musicPlayer.Ctlcontrols.pause();
        }
        /// <summary>
        /// 停止播放
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count == 0)
            {
                tkbProgress.Enabled = false;
                return;
            }
            tkbProgress.Value = 0;
            musicPlayer.Ctlcontrols.stop();
            btnPlayorPause.Text = "播放";
            lbltotalprogress.Text = "00:00";
        }
        /// <summary>
        /// 窗体加载时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void Form1_Load(object sender, EventArgs e)
        {
            //绑定
            Resizer = new ControlResizer(this);

            tkbVol.Minimum = 0;
            tkbVol.Maximum = 100;
            tkbProgress.Enabled = false;
            tkbVol.Value = musicPlayer.settings.volume;
            tkbProgress.Value = 0;
            lbltotalprogress.Text = "00:00";
            lblNow.Text = "00:00";
            lblVolume.Text = tkbVol.Value.ToString();
            //在程序加载时取消自动播放音乐
            musicPlayer.settings.autoStart = false;
            //musicPlayer.URL = @"";
            label1.Image = Image.FromFile(@"F:\voice.png");
        }
        bool b = true;
        private void btnPlayorPause_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count == 0)
            {
                tkbProgress.Enabled = false;
                return;
            }
            if (btnPlayorPause.Text=="播放")
            {
                //获得选中的歌曲
                if(b)
                {
                    musicPlayer.URL = listPath[listBox1.SelectedIndex];
                }
                musicPlayer.Ctlcontrols.play();
                btnPlayorPause.Text = "暂停";
            }else if(btnPlayorPause.Text=="暂停")
            {
                musicPlayer.Ctlcontrols.pause();
                btnPlayorPause.Text = "播放";
                b = false;
            }
        }
        List<String> listPath = new List<string>();
        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = @"F:\音乐";
            ofd.Filter = "MP3文件|*.mp3|音乐文件|*.wav|所有文件|*.*";
            ofd.Title = "请选择你想听的音乐o(^v^)o";
            ofd.Multiselect = true;
            ofd.ShowDialog();
            //获得选择文件的全路径
            string[] path =  ofd.FileNames;
            bool isContains = false;
            bool isAdd = true;
            for (int i = 0; i < path.Length; i++)
            {
                if (listPath.Contains(path[i]))
                {
                    isContains = true;

                }

            }
            if(isContains)
            {
                if (MessageBox.Show("是否添加?", "发现重复文件", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                {
                    isAdd = false;
                }
            }
            if (isAdd)
            {
                for(int i=0;i<path.Length;i++)
                {
                    listPath.Add(path[i]);
                    //将文件名存储到listbox中
                    listBox1.Items.Add(Path.GetFileName(path[i]));
                }
            }
        }
        /// <summary>
        /// 双击播放音乐
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if(listBox1.Items.Count==0)
            {
                MessageBox.Show("请选择音乐文件");
                tkbProgress.Enabled = false;
                return;
            }
            try{
                musicPlayer.URL = listPath[listBox1.SelectedIndex];
                musicPlayer.Ctlcontrols.play();
                btnPlayorPause.Text = "暂停";
                //lblInformation.Text = musicPlayer.Ctlcontrols.currentPosition.ToString();
                IsExistLrc(listPath[listBox1.SelectedIndex]);
            }
            catch { }
        }
        /// <summary>
        /// 下一曲
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void button6_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count == 0)
            {
                tkbProgress.Enabled = false;
                MessageBox.Show("请选择音乐文件");
                return;
            }
            //首先获得当前选中歌曲索引
            int index = listBox1.SelectedIndex;
            //清空所有选中项索引
            listBox1.SelectedIndices.Clear();
            index++;
            if(index==listBox1.Items.Count)
            {
                index = 0;
            }
            //修改当前选中的歌曲项
            listBox1.SelectedIndex = index;
            musicPlayer.URL = listPath[index];
            musicPlayer.Ctlcontrols.play();
            btnPlayorPause.Text = "暂停";
        }
        /// <summary>
        /// 上一曲
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count == 0)
            {
                MessageBox.Show("请选择音乐文件");
                tkbProgress.Enabled = false;
                return;
            }
            //首先获得当前选中歌曲索引
            int index = listBox1.SelectedIndex;
            //清空所有选中项索引
            listBox1.SelectedIndices.Clear();
            index--;
            if (index <0)
            {
                index = listBox1.Items.Count-1;
            }
            //修改当前选中的歌曲项
            listBox1.SelectedIndex = index;
            musicPlayer.URL = listPath[index];
            musicPlayer.Ctlcontrols.play();
            btnPlayorPause.Text = "暂停";
        }
        /// <summary>
        /// 点击删除选中项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int count = listBox1.SelectedItems.Count;
            for (int i = 0; i < count; i++)
            {
                //先删集合
                listPath.RemoveAt(listBox1.SelectedIndex);
                //删除列表选中项
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            }

        }
        /// <summary>
        /// 点击静音或放音
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label1_Click(object sender, EventArgs e)
        {
            if(label1.Tag.ToString()=="1")
            {
                //切换为静音
                musicPlayer.settings.mute = true;
                //显示静音图片
                label1.Image = Image.FromFile(@"F:\voice_off.png");
                label1.Tag = "2";
            }else if (label1.Tag.ToString() == "2")
            {
                //切换为放音
                musicPlayer.settings.mute = false;
                //显示放音图片
                label1.Image = Image.FromFile(@"F:\voice.png");
                label1.Tag = "1";
            }
        }

        /*
        private void timer1_Tick(object sender, EventArgs e)
        {
            //如果播放器状态正在播放中
            if (musicPlayer.playState == WMPLib.WMPPlayState.wmppsPlaying) 
            {
                lblInformation.Text = musicPlayer.currentMedia.duration.ToString()+"\r\n"
                    +musicPlayer.currentMedia.durationString+"\r\n"
                    +musicPlayer.Ctlcontrols.currentPosition.ToString()+"\r\n"
                    +musicPlayer.Ctlcontrols.currentPositionString;
                /*
                //如果歌曲当前播放时间等于总时间播放下一曲
                //double d1 = double.Parse(musicPlayer.currentMedia.durationString);//格式不对
                double d1 = double.Parse(musicPlayer.currentMedia.duration.ToString());
                double d2 = double.Parse(musicPlayer.Ctlcontrols.currentPosition.ToString()+2);
                //if (musicPlayer.currentMedia.durationString == musicPlayer.Ctlcontrols.currentPositionString)
                if (d1 <= d2)
                {
                    //首先获得当前选中歌曲索引
                    int index = listBox1.SelectedIndex;
                    //清空所有选中项索引
                    listBox1.SelectedIndices.Clear();
                    index++;
                    if (index == listBox1.Items.Count)
                    {
                        index = 0;
                    }
                    //修改当前选中的歌曲项
                    listBox1.SelectedIndex = index;
                    musicPlayer.URL = listPath[index];
                    musicPlayer.Ctlcontrols.play();
                    btnPlayorPause.Text = "暂停";
                }           
            }
        }*/
        //制作歌词
        void IsExistLrc(string songPath)
        {
            listTime.Clear();
            listLrcText.Clear();
            songPath = songPath.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[0] + ".lrc";
            if (File.Exists(songPath))
            {
                //读取歌词文件
                string[] lrcText = File.ReadAllLines(songPath,Encoding.UTF8);
                //格式化歌词
                FormatLrc(lrcText);
            }
            else//不存在歌词
            {
                label2.Text = "---未找到歌词文件---";
            }

        }
        //存储时间
        List<double> listTime = new List<double>();
        //存储歌词
        List<string> listLrcText = new List<string>();


        void FormatLrc(string[] lrcText)
        {
            
            for (int i = 0; i < lrcText.Length; i++)
            {
                string[] lrcTemp = lrcText[i].Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
                string[] lrcNewTemp = lrcTemp[0].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                double time = double.Parse(lrcNewTemp[0]) * 60 + double.Parse(lrcNewTemp[1]);
                if(lrcTemp.Length==1)
                {
                    continue;
                }
                listTime.Add(time);
                listLrcText.Add(lrcTemp[1]);
            }
        }


        private void label2_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 播放歌词
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer2_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < listTime.Count; i++)
            {
                try
                {
                    if (musicPlayer.Ctlcontrols.currentPosition >= listTime[i] && musicPlayer.Ctlcontrols.currentPosition < listTime[i + 1])
                    {
                        label2.Text = listLrcText[i];
                    }
                }catch
                {

                }
            }
        }

        private void musicPlayer_StatusChange(object sender, EventArgs e)
        {
            if (musicPlayer.playState == WMPLib.WMPPlayState.wmppsStopped)
            {
                if(rbtnList.Checked == true)
                {
                    //提供的方法
                    //musicPlayer.settings.setMode("shuffle",false);
                    //自己写的
                    ListPlayer();
                }else if(rbtnRandom.Checked==true)
                {
                    //提供的方法
                    //musicPlayer.settings.setMode("shuffle", true);
                    RandomPlayer();
                }else if(rbtnLoop.Checked==true)
                {
                    musicPlayer.settings.setMode("loop", true);
                }
                
            }
        }

        public void LoopPlayer()
        {
            int i = listBox1.SelectedIndex;
            musicPlayer.URL = listPath[i];
            listBox1.SelectedIndices.Clear();
            listBox1.SelectedIndex = i;
            IsExistLrc(listPath[listBox1.SelectedIndex]);
        }
        /// <summary>
        /// 增加音量+5
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click(object sender, EventArgs e)
        {
            if(musicPlayer.settings.volume<100)
            {
                musicPlayer.settings.volume+=5;
                tkbVol.Value = musicPlayer.settings.volume;
                lblVolume.Text = tkbVol.Value.ToString();
            }
        }
        /// <summary>
        /// 降低音量-5
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button8_Click(object sender, EventArgs e)
        {
            if (musicPlayer.settings.volume > 0)
            {
                musicPlayer.settings.volume-=5;
                tkbVol.Value = musicPlayer.settings.volume;
                lblVolume.Text = tkbVol.Value.ToString();
            }
        }
        
        private void timer3_Tick(object sender, EventArgs e)
        {
            if(musicPlayer.playState== WMPLib.WMPPlayState.wmppsReady)
            {
                try
                {
                    musicPlayer.Ctlcontrols.play();
                }
                catch
                {

                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if(MessageBox.Show("确定退出吗?","",MessageBoxButtons.OKCancel)==DialogResult.Cancel)
            //{
            //    e.Cancel = true;
            //}
            e.Cancel = true;
            this.Hide();
            this.notifyIcon1.Visible = true;
            this.ShowInTaskbar = false;//图标显示在任务栏
            contextMenuStrip2.Items[0].Text = "显示";

        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            //当窗体最小化状态时
            if(this.WindowState==FormWindowState.Minimized)
            {
                this.notifyIcon1.Visible = true;
                this.ShowInTaskbar = true;//图标显示在任务栏
                contextMenuStrip2.Items[0].Text = "显示";
            }
        }
        /// <summary>
        /// 双击显示窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ShowWindows();
        }
        /// <summary>
        /// 托盘右键显示窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 显示窗体ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowWindows();
        }
        
        /// <summary>
        /// 托盘暂停
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 暂停ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(musicPlayer.playState== WMPLib.WMPPlayState.wmppsPlaying)
            {
                musicPlayer.Ctlcontrols.pause();
                contextMenuStrip2.Items[1].Text = "继续";
                btnPlayorPause.Text = "播放";
            }
            else
            {
                musicPlayer.Ctlcontrols.play();
                contextMenuStrip2.Items[1].Text = "暂停";
                btnPlayorPause.Text = "暂停";
            }
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定退出吗?", "question", MessageBoxButtons.OKCancel);
            if(dr==DialogResult.OK)
            {
                Application.Exit();
            }
            else
            {

            }
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            ShowWindows();
        }
        /// <summary>
        /// 顺序播放
        /// </summary>
        private void ListPlayer()
        {
            try
            {
                int index = listBox1.SelectedIndex;
                //判断是否为最后一项
                if (index < listBox1.Items.Count - 1)
                {
                    musicPlayer.URL = listPath[index + 1];
                    listBox1.SelectedIndices.Clear();
                    listBox1.SelectedIndex = index + 1;
                }
                else
                {
                    listBox1.SelectedIndices.Clear();
                    listBox1.SelectedIndex = 0;
                    musicPlayer.URL = listPath[0];
                }
                IsExistLrc(listPath[listBox1.SelectedIndex]);
            }
            catch
            {
                
            }
        }
        /// <summary>
        /// 随机播放
        /// </summary>
        public void RandomPlayer()
        {
            Random r = new Random();
            int i = r.Next(0, listPath.Count);
            musicPlayer.URL = listPath[i];
            listBox1.SelectedIndices.Clear();
            listBox1.SelectedIndex = i;
            IsExistLrc(listPath[listBox1.SelectedIndex]);
        }
        /// <summary>
        /// 窗口显示
        /// </summary>
        void ShowWindows()
        {
            if (contextMenuStrip2.Items[0].Text == "显示")
            {
                this.Show();
                //还原窗体
                WindowState = FormWindowState.Normal;
                //激活窗体并给他焦点
                this.Activate();
                //任务栏显示图标
                this.ShowInTaskbar = true;
                this.notifyIcon1.Visible = false;
                contextMenuStrip2.Items[0].Text = "隐藏";
            }
            else
            {
                contextMenuStrip2.Items[0].Text = "显示";
                this.notifyIcon1.Visible = true;
                this.Hide();
                this.ShowInTaskbar = false;
            }
        }

        private void button9_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                musicPlayer.Ctlcontrols.fastForward();
            }catch
            {

            }
        }

        private void button9_MouseUp(object sender, MouseEventArgs e)
        {
            musicPlayer.Ctlcontrols.play();
        }
        private void musicPlayer_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if(musicPlayer.playState== WMPLib.WMPPlayState.wmppsReady)
            {
                IsExistLrc(listPath[listBox1.SelectedIndex]);
                tkbProgress.Value = 0;
                tkbProgress.Enabled = true;
            }
            else if(musicPlayer.playState== WMPLib.WMPPlayState.wmppsPlaying){
                tkbProgress.Maximum = (int)musicPlayer.currentMedia.duration;
                lbltotalprogress.Text = musicPlayer.currentMedia.durationString;
                tkbProgress.Enabled = true;
            }
        }

        private void button10_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                musicPlayer.Ctlcontrols.fastReverse();
            }
            catch
            {

            }
        }

        private void button10_MouseUp(object sender, MouseEventArgs e)
        {
            musicPlayer.Ctlcontrols.play();
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.notifyIcon1.Visible = false;
                this.ShowInTaskbar = true;//图标显示在任务栏
                contextMenuStrip2.Items[0].Text = "隐藏";
            }
        }
        //下面为窗体缩放
        /// <summary>
        /// 同步缩放窗体上控件的大小和字体
        /// </summary>
        public class ControlResizer
        {
            class ControlPosAndSize
            {
                public float FrmWidth { get; set; }
                public float FrmHeight { get; set; }
                public int Left { get; set; }
                public int Top { get; set; }
                public int Width { get; set; }
                public int Height { get; set; }
                public float FontSize { get; set; }

            }

            private Form _form;

            //句柄,大小信息
            private Dictionary<int, ControlPosAndSize> _dic = new Dictionary<int, ControlPosAndSize>();
            public ControlResizer(Form form)
            {
                _form = form;
                _form.Resize += _form_Resize;//绑定窗体大小改变事件

                _form.ControlAdded += form_ControlAdded;  //窗体上新增控件的处理
                _form.ControlRemoved += form_ControlRemoved;

                SnapControlSize(_form);//记录控件和窗体大小
            }

            void form_ControlRemoved(object sender, ControlEventArgs e)
            {
                var key = e.Control.Handle.ToInt32();
                _dic.Remove(key);
            }

            //绑定控件添加事件
            private void form_ControlAdded(object sender, ControlEventArgs e)
            {
                var ctl = e.Control;
                var ps = new ControlPosAndSize
                {
                    FrmHeight = _form.Height,
                    FrmWidth = _form.Width,
                    Width = ctl.Width,
                    Height = ctl.Height,
                    Left = ctl.Left,
                    Top = ctl.Top,
                    FontSize = ctl.Font.Size
                };
                var key = ctl.Handle.ToInt32();
                _dic[key] = ps;
            }

            void _form_Resize(object sender, EventArgs e)
            {
                ResizeControl(_form);
            }

            private void ResizeControl(Control control)
            {
                foreach (Control ctl in control.Controls)
                {
                    var key = ctl.Handle.ToInt32();
                    if (_dic.ContainsKey(key))
                    {
                        var ps = _dic[key];
                        var newx = _form.Width / ps.FrmWidth;
                        var newy = _form.Height / ps.FrmHeight;

                        ctl.Top = (int)(ps.Top * newy);
                        ctl.Height = (int)(ps.Height * newy);

                        ctl.Left = (int)(ps.Left * newx);
                        ctl.Width = (int)(ps.Width * newx);

                        ctl.Font = new Font(ctl.Font.Name, ps.FontSize * newy, ctl.Font.Style, ctl.Font.Unit);

                        if (ctl.Controls.Count > 0)
                        {
                            ResizeControl(ctl);
                        }

                    }

                }
            }

            /// <summary>
            /// 创建控件的大小快照,参数为需要记录大小控件的 容器
            /// </summary>
            private void SnapControlSize(Control control)
            {
                foreach (Control ctl in control.Controls)
                {
                    var ps = new ControlPosAndSize
                    {
                        FrmHeight = _form.Height,
                        FrmWidth = _form.Width,
                        Width = ctl.Width,
                        Height = ctl.Height,
                        Left = ctl.Left,
                        Top = ctl.Top,
                        FontSize = ctl.Font.Size
                    };

                    var key = ctl.Handle.ToInt32();

                    _dic[key] = ps;

                    //绑定添加事件
                    ctl.ControlAdded += form_ControlAdded;
                    ctl.ControlRemoved += form_ControlRemoved;

                    if (ctl.Controls.Count > 0)
                    {
                        SnapControlSize(ctl);
                    }

                }

            }

        }

        private void tkbVol_Scroll(object sender, EventArgs e)
        {
            
            musicPlayer.settings.volume = tkbVol.Value;
            lblVolume.Text = tkbVol.Value.ToString();
        }

        private void tkbProgress_Scroll(object sender, EventArgs e)
        {
            musicPlayer.Ctlcontrols.currentPosition = (double)this.tkbProgress.Value;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            tkbProgress.Value = (int)musicPlayer.Ctlcontrols.currentPosition;
            lblNow.Text = musicPlayer.Ctlcontrols.currentPositionString;
        }
    }
    
}
