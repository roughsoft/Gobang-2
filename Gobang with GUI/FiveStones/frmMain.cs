using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;

namespace FiveStones
{
    public enum Tag { No, Black, White, Forbidden = -1 };           //枚举棋子颜色、无棋子、棋盘外

    public partial class frmMain : Form
    {
        #region 字段及属性

        private string[] szBtnGame;                                 //第一个按钮文本
        private string[] szBtnFile;                                 //第二个按钮文本
        private Game game;                                          //游戏
        private int nTimeLimit;                                     //时间限制
        private bool bSaveGame;                                     //是否要保存游戏了（使用timer防止多线程无法打开保存对话框）

        public bool InGame { set; get; }                            //是否在游戏属性

        #endregion

        #region 窗体构造函数及关闭事件

        public frmMain()                                            //构造函数
        {
            InitializeComponent();                                  //初始化各控件
            Control.CheckForIllegalCrossThreadCalls = false;        //控件可以多线程调用修改
            
            szBtnGame = new string[2];                              //按钮文本数组
            szBtnFile = new string[2];
            szBtnGame[0] = "开始新局";
            szBtnGame[1] = "投子认输";
            szBtnFile[0] = "读取游戏";
            szBtnFile[1] = "存储游戏";
            
            InGame = false;                                         //不在游戏
            RealStone.Init(picBlack.Image, picWhite.Image);         //设置棋子图片
            game = new Game(this);                                  //创建游戏
            SetControls();                                          //设置各控件属性
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {                                                           //窗体关闭事件
            if (InGame && game.CurrentAnotherPlayer != null && game.CurrentAnotherPlayer is NetPlayer)
            {                                                       //游戏中，对方玩家非空且为网络玩家
                game.GiveUp();                                      //我方弃权
            }
        }

        #endregion

        #region 棋盘及棋子列表控件事件

        private void picBoard_MouseDown(object sender, MouseEventArgs e)
        {
            game.MouseDown(e.X, e.Y);                               //鼠标点击棋盘
        }

        public void picBoard_Paint(object sender, PaintEventArgs e)
        {
            game.Draw(e.Graphics);                                  //重绘棋盘
        }

        private void lstHistory_SelectedIndexChanged(object sender, EventArgs e)//棋子列表更改选择项
        {
            picBoard_Paint(sender, new PaintEventArgs(picBoard.CreateGraphics(), picBoard.ClientRectangle));
        }                                                           //重绘棋盘

        #endregion

        #region 各单选复选框控件事件及函数

        private void RadioChecks_CheckedChanged(object sender, EventArgs e)//单选复选框选中状态改变
        {
            txtBlackHuman.Enabled = radBlackHuman.Checked;          //玩家名输入框只有选中之前玩家单选框才能使用
            txtWhiteHuman.Enabled = radWhiteHuman.Checked;

            txtTimeLimit.Enabled = chkTimeLimit.Checked;            //限制时间框只有之前限制时间复选框选中才能使用
            if (!chkTimeLimit.Checked) nTimeLimit = 0;

            radWhiteNet.Enabled = !radBlackNet.Checked;             //网络IP框只有之前网络单选框选中才能使用，且不能两边都为网络玩家
            txtBlackNet.Enabled = radBlackNet.Checked;

            radBlackNet.Enabled = !radWhiteNet.Checked;
            txtWhiteNet.Enabled = radWhiteNet.Checked;
        }

        public void SetControls()                                   //根据游戏状态设置控件属性
        {
            radBlackAI.Enabled =                                    //游戏外启用控件
            radBlackHuman.Enabled =
            radWhiteAI.Enabled =
            radWhiteHuman.Enabled =
            chkTimeLimit.Enabled =
            !InGame;

            picBoard.Enabled = InGame;                              //游戏内控件

            txtBlackHuman.Enabled = !InGame && radBlackHuman.Checked;//同时由游戏状态和其他控件属性决定的空间
            txtWhiteHuman.Enabled = !InGame && radWhiteHuman.Checked;
            txtBlackNet.Enabled = !InGame && radBlackNet.Checked;
            txtWhiteNet.Enabled = !InGame && radWhiteNet.Checked;
            txtTimeLimit.Enabled = !InGame && chkTimeLimit.Checked;
            timerLimit.Enabled = InGame;

            radWhiteNet.Enabled = !InGame && !radBlackNet.Checked;
            radBlackNet.Enabled = !InGame && !radWhiteNet.Checked;
            btnFile.Enabled = !InGame || (!radBlackNet.Checked && !radWhiteNet.Checked);
            txtBlackNet.Enabled = !InGame && radBlackNet.Checked;
            txtWhiteNet.Enabled = !InGame && radWhiteNet.Checked;

            lblBlackTime.Enabled = lblWhiteTime.Enabled =
            timerVideo.Enabled = trkVideo.Enabled = false;          //时间标签禁用掉待后来启用，禁用播放录像相关控件

            btnGame.Enabled = true;                                 //启用Game按钮
            btnGame.Text = szBtnGame[Convert.ToInt32(InGame)];      //设置按钮文本
            btnFile.Text = szBtnFile[Convert.ToInt32(InGame)];
        }

        private void SetControlsFromFile(FiveStones.Tag tag, string szName, int nFlag)
        {                                                           //根据读入存档信息设置各控件
            if (tag == FiveStones.Tag.Black)                        //黑色玩家
            {
                if ((nFlag & Constants.AI_PLAYER) > 0)              //AI玩家，启用AI相关控件
                {
                    radBlackHuman.Checked = txtBlackHuman.Enabled = radBlackNet.Checked = false;
                    radBlackAI.Checked = true;
                    return;
                }
                if ((nFlag & Constants.HUMAN_PLAYER) > 0)           //人类玩家，启用人类相关控件
                {
                    radBlackAI.Checked = radBlackNet.Checked = false;
                    radBlackHuman.Checked = txtBlackHuman.Enabled = true;
                    txtBlackHuman.Text = szName;                    //人名
                    return;
                }
            }
            else
            {                                                       //白色玩家同上
                if ((nFlag & Constants.AI_PLAYER) > 0)
                {
                    radWhiteHuman.Checked = txtWhiteHuman.Enabled = radWhiteNet.Checked = false;
                    radWhiteAI.Checked = true;
                    return;
                }
                if ((nFlag & Constants.HUMAN_PLAYER) > 0)
                {
                    radWhiteAI.Checked = radWhiteNet.Checked = false;
                    radWhiteHuman.Checked = txtWhiteHuman.Enabled = true;
                    txtWhiteHuman.Text = szName;
                    return;
                }
            }
        }

        private void SetTimeLimit(int nTimeLimit)                   //设置限制时间
        {
            this.nTimeLimit = nTimeLimit;                           //设置限制时间
            chkTimeLimit.Checked = txtTimeLimit.Enabled = nTimeLimit > 0;//如果限时大于0，选中限制时间复选框，启用限制时间文本框
            txtTimeLimit.Text = nTimeLimit.ToString();              //显示限制时间
        }

        #endregion

        #region 计时器相关
        
        public void timerLimit_Tick(object sender, EventArgs e)     //游戏时间限制计时器
        {
            if (btnGame.Text != szBtnGame[1]) return;               //当前不是思考落子时间
            TimeSpan span = DateTime.Now - game.CurrentStartTime;   //计算当前玩家思考事件
            string szLblTime = span.Minutes.ToString("D2") + ":" + span.Seconds.ToString("D2");
            if (game.CurrentTag == FiveStones.Tag.Black)            //生成显示的时间，将其显示在对应玩家的面板上
            {
                lblBlackTime.Enabled = true;                        //启用当前时间标签
                lblBlackTime.Text = szLblTime;                      //显示已用时间
                lblWhiteTime.Enabled = false;                       //对方玩家时间标签禁用
            }
            else
            {                                                       //同理
                lblWhiteTime.Enabled = true;
                lblWhiteTime.Text = szLblTime;
                lblBlackTime.Enabled = false;
            }
            if (chkTimeLimit.Checked && span.Minutes >= nTimeLimit) game.GiveUp();//设置了限制时间，而且超时，则弃权
        }

        private void timerSynchronize_Tick(object sender, EventArgs e)//同步计时器，防止多线程启动不了保存对话框
        {
            if (bSaveGame)                                          //需要保存
            {
                bSaveGame = false;                                  //保存标签设空
                dlgSaveGame.ShowDialog();                           //显示保存对话框
            }
        }

        #endregion

        private void trkVideo_Scroll(object sender, EventArgs e)    //录像播放滑杆
        {
            if (trkVideo.Value > 0)                                 //滑杆数值大于0
            {
                timerVideo.Interval = trkVideo.Value * Constants.TIME_RATE;//录像播放计时器间隔时间为秒数乘以1000毫秒/秒
                timerVideo.Enabled = true;                          //启用录像播放计时器
                lblVideo.Text = "录像播放速度：" + trkVideo.Value.ToString() + "秒/步";
            }                                                       //显示提示
            else
            {
                timerVideo.Enabled = false;                         //暂停播放
                lblVideo.Text = "录像播放暂停";
            }
        }

        #region 开始/结束游戏

        private void btnGame_Click(object sender, EventArgs e)      //点击启动/结束游戏按钮
        {
            if (!InGame) StartGame();                               //不在游戏中，则启动游戏
            else
                if (btnGame.Text == szBtnGame[1])                   //如果是正常游戏状态
                {
                    if (game.CurrentPlayer is HumanPlayer && !(game.CurrentPlayer is NetPlayer))
                    {
                        game.GiveUp();                              //如果当前玩家是非网络玩家的人类玩家，弃权
                    }
                    else MessageBox.Show("您不能帮别人弃权！");     //防止为他人弃权恶意作弊
                }
                else StopGame();                                    //直接结束游戏
        }

        private void StartGame()                                    //开始游戏
        {
            if (chkTimeLimit.Checked)
            {
                try
                {
                    nTimeLimit = Convert.ToInt32(txtTimeLimit.Text);//转换设置好的限制时间标签
                }
                catch (Exception) { nTimeLimit = 0; }
                if (nTimeLimit == 0)
                {
                    MessageBox.Show("请输入有效限制时间！");        //设置时间有问题
                    return;
                }
            }
            else nTimeLimit = 0;
            game.Clear();                                           //清空游戏
            
            if (!radBlackNet.Checked && !radWhiteNet.Checked)       //非网络游戏
            {
                if (radBlackHuman.Checked)                          //根据单选框设置两边是否为电脑或人类
                {
                    game[FiveStones.Tag.Black] = new HumanPlayer(game, FiveStones.Tag.Black, txtBlackHuman.Text);
                }
                else
                {
                    game[FiveStones.Tag.Black] = new AIPlayer(game, FiveStones.Tag.Black);
                }

                if (radWhiteHuman.Checked)
                {
                    game[FiveStones.Tag.White] = new HumanPlayer(game, FiveStones.Tag.White, txtWhiteHuman.Text);
                }
                else
                {
                    game[FiveStones.Tag.White] = new AIPlayer(game, FiveStones.Tag.White);
                }
                
                InGame = true;                                      //游戏中
                SetControls();                                      //设置各控件
                game.Start();                                       //开始游戏
            }
            else
            {
                SetTimeLimit(Constants.NET_TIME_LIMIT);             //调整为网络默认限时
                IPAddress IP;
                if (radBlackNet.Checked)
                {
                    MessageBox.Show("本地为白棋方将建立服务端，等待指定IP客户端信息，每人每步限" + Constants.NET_TIME_LIMIT.ToString() + "分钟。");
                    if (radWhiteHuman.Checked)                      //本地端建立玩家对象
                    {
                        game[FiveStones.Tag.White] = new HumanPlayer(game, FiveStones.Tag.White, txtWhiteHuman.Text);
                    }
                    else
                    {
                        game[FiveStones.Tag.White] = new AIPlayer(game, FiveStones.Tag.White);
                    }
                    try
                    {
                        IP = IPAddress.Parse(txtBlackNet.Text);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("请输入有效IP地址！");
                        return;
                    }
                    
                    InGame = true;                                  //设置为游戏中，重置各控件，显示第一个按钮为“断开连接”
                    SetControls();
                    btnGame.Text = "断开连接";
                    
                    game[FiveStones.Tag.Black] = new NetPlayer(game, FiveStones.Tag.Black, IP);
                }                                                   //创建网络玩家
                else
                {
                    MessageBox.Show("本地为黑棋方将建立客户端，请确保指定IP已建立服务端，每人每步限" + Constants.NET_TIME_LIMIT.ToString() + "分钟。");
                    if (radBlackHuman.Checked)                      //本地端建立玩家对象
                    {
                        game[FiveStones.Tag.Black] = new HumanPlayer(game, FiveStones.Tag.Black, txtBlackHuman.Text);
                    }
                    else
                    {
                        game[FiveStones.Tag.Black] = new AIPlayer(game, FiveStones.Tag.Black);
                    }

                    try
                    {
                        IP = IPAddress.Parse(txtWhiteNet.Text);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("请输入有效IP地址！");
                        return;
                    }

                    InGame = true;                                  //设置为游戏中，重置各控件，显示第一个按钮为“断开连接”
                    SetControls();
                    btnGame.Text = "断开连接";
                    
                    game[FiveStones.Tag.White] = new NetPlayer(game, FiveStones.Tag.White, IP);
                }                                                   //创建网络玩家
            }
            
        }

        public void StopGame()
        {
            InGame = false;                                         //不再游戏中了
            string szTemp = btnGame.Text;                           //保存之前第一个按钮文本
            SetControls();                                          //重置各控件
            
            if (szTemp == szBtnGame[1])                             //第一个按钮之前正常显示“投子认输”
            {
                string szTip = (game.Winner == null ? "平局！" : (game.Winner + "获胜！")) + "\n是否保存录像？";
                
                DialogResult dlgRes = MessageBox.Show(szTip, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dlgRes == DialogResult.Yes) bSaveGame = true;   //显示提示信息，询问保存录像
            }
            game.Close();                                           //关闭游戏
        }

        #endregion

        #region 文件读取与保存

        private void btnFile_Click(object sender, EventArgs e)      //点击读取/保存文件按钮
        {
            if (!InGame) dlgOpenGame.ShowDialog();                  //文件外为读取
            else
            {
                if (game.CurrentPlayer is HumanPlayer && !(game.CurrentPlayer is NetPlayer))
                {                                                   //当前玩家是在本地的人类
                    bSaveGame = true;                               //设置可保存字段为真
                }
                else MessageBox.Show("只有在人类玩家下棋时可以保存！");
            }
        }

        private void dlgOpenGame_FileOk(object sender, CancelEventArgs e)
        {                                                           //打开对话框点击确定后
            try
            {
                FileStream stream = new FileStream(dlgOpenGame.FileName, FileMode.Open);
                BinaryReader reader = new BinaryReader(stream);     //建立文件流
                
                LoadGame(reader);                                   //读取窗体需要的空间信息
                game.Load(reader);                                  //读取游戏需要的内部信息
                
                reader.Close();                                     //关闭reader
                stream.Close();                                     //关闭文件流
                
                game.Start();                                       //开始游戏
            }
            catch (Exception) { MessageBox.Show("读取游戏失败！"); }
        }

        private void LoadGame(BinaryReader reader)                  //读取空间信息
        {
            int nFlag, nWinner;                                     //玩家标记，是否获胜
            string szName;                                          //玩家名
            AbstractPlayer player;                                  //玩家临时变量

            nTimeLimit = reader.ReadInt32();                        //读取限制时间
            SetTimeLimit(nTimeLimit);                               //设定限制时间
            
            nWinner = reader.ReadInt32();                           //读取胜利者
            if (nWinner > 0)                                        //胜利者已产生
            {
                MessageBox.Show("存档中游戏已结束，切换至播放录像模式。");
                chkTimeLimit.Checked = txtTimeLimit.Enabled = false;//禁用限时对话框
            }

            for (FiveStones.Tag i = FiveStones.Tag.Black; i <= FiveStones.Tag.White; ++i)
            {                                                       //读取各玩家信息
                szName = reader.ReadString();                       //玩家名
                nFlag = reader.ReadInt32();                         //玩家标记
                
                if (nWinner == 0)                                   //并非录像
                {
                    if ((nFlag & Constants.HUMAN_PLAYER) > 0)       //人类
                    {
                        player = new HumanPlayer(game, i, szName);  //创建人类玩家
                    }
                    else
                    {
                        player = new AIPlayer(game, i);             //创建机器人玩家
                    }
                }
                else player = new VideoPlayer(game, i, szName);     //创建录像播放器
                
                game[i] = player;                                   //设置游戏的玩家
                SetControlsFromFile(player.tag, szName, nFlag);     //设置各控件属性
            }

            game.Clear();                                           //清空之前游戏落子等信息
            InGame = true;                                          //已在游戏中
            SetControls();                                          //设置控件
            
            if (nWinner > 0)                                        //录像模式
            {
                btnGame.Text = "停止录像";
                btnFile.Enabled = false;                            //不可以保存
                game.Winner = game[(FiveStones.Tag)nWinner];        //游戏已有胜利者
                trkVideo.Enabled = timerVideo.Enabled = true;       //启用录像相关控件
            }
        }

        private void dlgSaveGame_FileOk(object sender, CancelEventArgs e)
        {                                                           //保存对话框点击确定后
            try
            {
                FileStream stream = new FileStream(dlgSaveGame.FileName, FileMode.Create);
                BinaryWriter writer = new BinaryWriter(stream);     //建立文件流
                
                writer.Write(nTimeLimit);                           //保存限制时间
                game.Save(writer);                                  //保存游戏内部信息
                
                writer.Close();                                     //关闭文件流
                stream.Close();
            }
            catch (Exception) { MessageBox.Show("保存游戏失败！"); }
        }

        #endregion
    }
}
