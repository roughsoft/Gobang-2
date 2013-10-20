using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Net.Sockets;

namespace FiveStones
{
    public sealed class Game                                                                    //游戏类
    {
        public static Tag Divert(Tag tag) { return tag == Tag.Black ? Tag.White : Tag.Black; }  //翻转棋色

        public delegate void NetInput(int x, int y);                                            //网络输出委托
        public event PaintEventHandler BoardPainter;                                            //窗体重绘事件

        private AbstractPlayer[] player = new AbstractPlayer[4];                                //玩家数组
        private bool bIsVideo;                                                                  //是否是放录像

        #region 属性与索引

        public frmMain MainForm { set; get; }                                                   //主窗体
        public Tag CurrentTag { set; get; }                                                     //当前棋手颜色
        public DateTime CurrentStartTime { set; get; }                                          //当前棋手开始思考世界
        public int Count { get { return MainForm.lstHistory.Items.Count; } }                    //已下棋子数
        public Tag[,] Board { set; get; }                                                       //棋盘
        public NetInput NetInputer { set; get; }                                                //网络输出
        
        public AbstractPlayer this[Tag tag]                                                     //建立选手索引
        {
            set { player[(int)tag] = value; }
            get { return player[(int)tag]; }
        }
        public AbstractPlayer Winner { set; get; }                                              //获胜者
        public AbstractPlayer CurrentPlayer                                                     //获取当前玩家
        {
            get { return player[(int)CurrentTag]; }
        }
        public AbstractPlayer CurrentAnotherPlayer                                              //获取当前玩家对手
        {
            get { return player[Constants.SUM_STONE - (int)CurrentTag]; }
        }
        
        public AbstractStone Last                                                               //最后的棋子
        {
            get
            {
                if (MainForm.lstHistory.Items.Count == 0) return null;                          //没有下过子，返回空
                return (RealStone)(MainForm.lstHistory.Items[MainForm.lstHistory.Items.Count - 1]);
            }
        }

        public ArrayList SocketArray { set; get; }                                              //套接字数组

        #endregion

        #region 构造函数及游戏主过程相关

        public Game(frmMain MainForm)                                                           //构造函数
        {
            this.MainForm = MainForm;                                                           //主窗体
            MainForm.timerVideo.Tick += VideoTicker;                                            //注册主窗体timerVideo的Tick事件
            
            Board = new Tag[Constants.SIZE, Constants.SIZE];                                    //创建棋盘
            BoardPainter += MainForm.picBoard_Paint;                                            //注册窗体重绘事件
            
            CurrentStartTime = DateTime.Now;                                                    //初始时间
            SocketArray = new ArrayList();                                                      //初始化套接字数组
        }

        public void Clear()                                                                     //清空游戏
        {
            bIsVideo = false;                                                                   //非播放录像
            NetInputer = null;                                                                  //无网络输出事件
            
            foreach (RealStone stone in MainForm.lstHistory.Items)
            {
                stone.Up();                                                                     //删除已下各子
            }
            MainForm.lstHistory.Items.Clear();                                                  //清空历史记录
            Winner = null;                                                                      //胜者置空
            
            MainForm.picBoard.CreateGraphics().DrawImage(MainForm.picBoard.Image, 0, 0);        //棋盘重绘
        }

        public void Start()                                                                     //启动游戏
        {
            if (MainForm.lstHistory.Items.Count > 0)                                            //若已读取了存档的落子记录
            {
                CurrentTag = Divert(((RealStone)(MainForm.lstHistory.Items[MainForm.lstHistory.Items.Count - 1])).tag);
                MainForm.lstHistory.SelectedIndex = MainForm.lstHistory.Items.Count - 1;        //设置最后一个子的对手是当前玩家
            }
            else CurrentTag = Tag.Black;                                                        //否则黑子先手
            if (!bIsVideo) CurrentPlayer.Play();                                                //如果不是播放录像，开始第一个玩家
            else VideoTicker(this, new EventArgs());                                            //播放录像
        }

        public void GiveUp()                                                                    //放弃
        {
            Winner = player[(int)Divert(CurrentTag)];                                           //胜者为对方
            if ((NetInputer != null) && (CurrentAnotherPlayer is NetPlayer))                    //如果敌对玩家为网络玩家
            {
                NetInputer(Constants.NET_GIVE_UP, Constants.NET_GIVE_UP);                       //输出弃权
            }
            MainForm.StopGame();                                                                //停止游戏的窗体处理
        }

        public void Close()                                                                     //结束游戏的后续处理
        {
            foreach (Socket sock in SocketArray)
            {
                if (sock != null) sock.Close();                                                 //关闭所有连接
            }
            SocketArray.Clear();                                                                //清空连接

            if (player[1] != null) player[1].Close();                                           //如果玩家非空，调用结束处理函数
            if (player[2] != null) player[2].Close();
        }

        #endregion

        #region 落子

        public void Down(int x, int y)                                                          //按坐标落子
        {
            if (x == Constants.NET_GIVE_UP) { GiveUp(); return; }                               //放弃
            if (Board[x, y] != Tag.No) return;                                                  //落子失败，位置已有子
            RealStone stone = new RealStone(Board, CurrentPlayer, x, y);                        //创建真实棋子
            Down(stone);                                                                        //落子
        }

        public void Down(AbstractStone stone)                                                   //按棋子落子
        {
            stone.Down();                                                                       //落子
            
            MainForm.lstHistory.Items.Add(stone);
            MainForm.lstHistory.SelectedIndex = MainForm.lstHistory.Items.Count - 1;            //添加记录，列表中选中最后一个棋子
            Draw(MainForm.picBoard.CreateGraphics());                                           //窗体重绘
            
            if ((NetInputer != null) && !(CurrentPlayer is NetPlayer)) NetInputer(stone.X, stone.Y);//网络发送棋子
            CurrentTag = Divert(CurrentTag);                                                    //玩家换手
            
            if (!bIsVideo)                                                                      //如果非录像
            {
                double nValue = Analyzer.GetValue(Board, stone);                                //估值是否获胜
                if (nValue > Constants.WIN - Constants.DELTA) { GiveUp(); return; }             //对方获胜，则己方弃权
                if (MainForm.lstHistory.Items.Count == Constants.MAX_STONE) MainForm.StopGame();//双方下满格子，平局
                else CurrentPlayer.Play();                                                      //对方开始下棋
            }
        }

        public void MouseDown(int x, int y)                                                     //鼠标点击
        {
            if (CurrentPlayer != null) CurrentPlayer.MouseDown(x, y);                           //如果当前玩家存在，触发其鼠标点击事件
        }

        #endregion

        #region 读写存档

        public void Load(BinaryReader reader)                                                   //读档    
        {
            int nTag, nCount, x, y;
            
            nCount = reader.ReadInt32();                                                        //棋子个数
            for (int i = 0; i < nCount; ++i)
            {
                nTag = reader.ReadInt32();                                                      //棋子颜色
                x = reader.ReadInt32();                                                         //棋子坐标
                y = reader.ReadInt32();
                
                if (Winner == null)                                                             //不是录像
                {
                    AbstractStone stone = new RealStone(Board, player[nTag], x, y);             //添加棋子
                    MainForm.lstHistory.Items.Add(stone);
                    stone.Down();                                                               //落在棋盘上
                }
                else
                {
                    ((VideoPlayer)player[nTag]).Add(new RealStone(Board, player[nTag], x, y));  //棋子加入录像玩家播放队列
                    bIsVideo = true;                                                            //本次游戏是录像播放
                }
            }
        }

        public void Save(BinaryWriter writer)                                                   //存档
        {
            int nWinner, nFlag;

            if (Winner == null) nWinner = 0;                                                    //如果为空，保存赢家为0，否则保存其颜色编号
            else nWinner = (int)(Winner.tag);
            writer.Write(nWinner);

            for (int i = 1; i <= 2; ++i)                                                        //写入玩家
            {
                writer.Write(player[i].Name);                                                   //写入玩家名
                nFlag = (Convert.ToInt32(player[i] is AIPlayer) << 1) + Convert.ToInt32(player[i] is HumanPlayer);
                writer.Write(nFlag);                                                            //按照玩家对象的派生类型保存其标识
            }
            
            writer.Write(MainForm.lstHistory.Items.Count);                                      //写入已下棋子数
            foreach (RealStone stone in MainForm.lstHistory.Items) stone.Save(writer);          //保存每一个棋子
        }

        #endregion

        public void Draw(Graphics g)                                                            //窗体重绘
        {
            foreach (RealStone stone in MainForm.lstHistory.Items)                              //列表框所有棋子重绘
            {
                stone.Draw(g);
            }
            foreach (RealStone stone in MainForm.lstHistory.SelectedItems)                      //列表框所有选中棋子重绘高亮部分
            {
                stone.DrawHighlight(g);
            }
        }

        public void VideoTicker(object sender, EventArgs e)                                     //播放录像时计时器事件
        {
            ((VideoPlayer)CurrentPlayer).Play();                                                //让录像播放器播放下一个棋子
        }
    }
}
