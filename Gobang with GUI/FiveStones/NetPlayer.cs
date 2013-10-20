using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Threading;

namespace FiveStones
{
    public sealed class NetPlayer : HumanPlayer             //网络玩家
    {
        private IPAddress IP;                               //IPAddress对象
        private IPEndPoint IPe;                             //IPEndPoint对象
        private Socket sock, other;                         //套接字对象
        private Thread ConnectionThread;                    //网络初始化、输出、输入线程
        private int nErrorHappened;                         //出现错误

        #region 选手初始化

        public NetPlayer(Game game, Tag tag, IPAddress IP)  //构造函数，传入IP
            : base(game, tag, "")
        {
            this.IP = IP;
            if (tag == Tag.Black)                           //网络端为黑，建立服务端
            {
                IPe = new IPEndPoint(IPAddress.Any, Constants.PORT);
                other = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                game.SocketArray.Add(other);                 //加入游戏网络连接列表
            }
            else
            {                                               //网络端为白，建立客户端
                IPe = new IPEndPoint(IP, Constants.PORT);
                sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                game.SocketArray.Add(sock);                 //加入游戏网络连接列表
            }

            ConnectionThread = new Thread(Init);
            ConnectionThread.IsBackground = true;       //创建初始化选手的线程，设置为背景线程，启动之
            ConnectionThread.Start();
        }
        
        public void Init()                                  //初始化函数
        {
            game.NetInputer += Input;                       //传入网络输入事件，本地落子后发送到网络端
            try
            {
                int nNameLength;                            //用户名长度
                if (tag == Tag.Black)                       //服务端
                {
                    other.Bind(IPe);                        //绑定本机ip
                    other.Listen(0);                        //监听

                    while (sock == null)
                    {
                        sock = other.Accept();              //接受连接
                        if (((IPEndPoint)(sock.RemoteEndPoint)).Address.ToString() != IP.ToString())
                        {
                            sock.Close();                   //如果IP不是事先设好的，关闭连接
                            sock = null;
                        }
                        game.SocketArray.Add(sock);         //加入游戏的网络连接列表
                    }

                    byte[] byteName = new byte[Constants.MAX_NAME_LENGTH];

                    nNameLength = sock.Receive(byteName);   //获取网络端用户名，这里在用户名前都有3个字符附加字符，防止传输出错
                    szName = Encoding.ASCII.GetString(byteName, 0, nNameLength);
                    szName = szName.Substring(3);

                    byteName = Encoding.ASCII.GetBytes("sdd" + game[Tag.White].Name);
                    sock.Send(byteName);                    //发送本地端用户名
                }
                else
                {
                    sock.Connect(IPe);                      //连接对方IP

                    byte[] byteName = Encoding.ASCII.GetBytes("sdd" + game[Tag.Black].Name);
                    sock.Send(byteName);                    //发送本地端用户名

                    byteName = new byte[Constants.MAX_NAME_LENGTH];
                    nNameLength = sock.Receive(byteName);
                    szName = Encoding.ASCII.GetString(byteName, 0, nNameLength);
                    szName = szName.Substring(3);           //获取网络端用户名
                }

                game.MainForm.SetControls();                //重置各控件，设置开始时间
                game.CurrentStartTime = DateTime.Now;
                game.Start();                               //开始游戏
            }
            catch (ThreadAbortException) { }
            catch (Exception) { Error((tag == Tag.Black ? "客户端" : "服务器") + "连接失败！"); }
        }

        #endregion

        #region 发送本地端的落子情况

        public void Input(int x, int y)                     //输入本地端落子坐标
        {
            ConnectionThread = new Thread(() => Put(x, y));
            ConnectionThread.IsBackground = true;           //创建新线程，启动线程
            ConnectionThread.Start();
        }

        private void Put(int x, int y)                      //发送本地端落子
        {
            try
            {
                byte[] bytes = new byte[2];                 //发送棋子坐标
                bytes[0] = (byte)x; bytes[1] = (byte)y;
                sock.Send(bytes);
            }
            catch (ThreadAbortException) { }
            catch (Exception) { Error("与" + (tag == Tag.Black ? "客户端" : "服务器") + "连接断开"); }
        }

        #endregion

        #region 接收网络端的落子情况

        public override void Play()                         //启动获取网络端落子的线程
        {
            base.Play();
            ConnectionThread = new Thread(Get);
            ConnectionThread.IsBackground = true;
            ConnectionThread.Start();
        }

        private void Get()                                  //获取网络端落子
        {
            try
            {
                byte[] bytes = new byte[2];                 //接受棋子坐标
                sock.Receive(bytes);
                game.Down((int)bytes[0], (int)bytes[1]);    //落子
            }
            catch (ThreadAbortException) { }
            catch (Exception) { Error("与" + (tag == Tag.Black ? "客户端" : "服务器") + "连接断开"); }
        }

        #endregion

        private void Error(string szTip)                    //根据提示处理错误
        {
            game.MainForm.InGame = false;                   //设置已不在游戏状态
            game.MainForm.SetControls();                    //重置各窗体控件属性
            if (++nErrorHappened == 1) MessageBox.Show(szTip);//弹出提示框
            game.Close();                                   //关闭游戏
        }

        public override void Close()                        //关闭各线程
        {
            if (ConnectionThread != null) ConnectionThread.Abort();
        }

        public override void MouseDown(int X, int Y) { }    //本类继承自人类玩家类，覆盖掉鼠标点击事件以免出错
    }
}
