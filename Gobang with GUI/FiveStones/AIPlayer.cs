using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;

namespace FiveStones
{
    public sealed class AIPlayer : AbstractPlayer                                               //电脑玩家类
    {
        private class VirtualStone : AbstractStone, IComparable                                 //虚拟棋子，用于搜索，可比较
        {
            public double Value { set; get; }                                                   //纯进攻值
            public double ValueWithDefence { set; get; }                                        //攻防综合值
            public int Distance { set; get; }                                                   //与中心距离

            public VirtualStone(Tag[,] Board, Tag tag, int x, int y)                            //构造函数
                : base(Board, tag, x, y)
            {
                Tag temp = Board[X, Y];                                                         //备份之前的子
                
                Down();                                                                         //落本方子
                Value = Analyzer.GetValue(Board, x, y);                                         //估值
                
                this.tag = Game.Divert(tag);                                                    //翻转颜色，落对方子
                Down();
                ValueWithDefence = Value + Analyzer.GetValue(Board, this) * Constants.AD_RATE;  //估值，计算综合
                
                this.tag = tag;
                Board[X, Y] = temp;                                                             //恢复
                
                Distance = Math.Abs(x - 7) + Math.Abs(y - 7);                                   //与中心距离
            }

            #region IComparable Members

            public int CompareTo(object obj)                                                    //默认比较函数，仅比较进攻
            {
                double dXValue, dYValue;
                
                dXValue = Value; dYValue = ((VirtualStone)obj).Value;                           //若进攻值不等，返回
                if (dXValue < dYValue) return 1;
                if (dXValue > dYValue) return -1;
                
                dXValue = ValueWithDefence; dYValue = ((VirtualStone)obj).ValueWithDefence;     //进攻值相等，比较综合值
                if (dXValue < dYValue) return 1;
                if (dXValue > dYValue) return -1;
                
                return Distance.CompareTo(((VirtualStone)obj).Distance);                        //综合值相等，距离中心近者为优
            }

            #endregion

            #region 比较器

            public class StoneComparer : IComparer                                              //按进攻值比较，直接调用比较函数
            {
                #region IComparer Members

                public int Compare(object x, object y)
                {
                    return ((VirtualStone)x).CompareTo(y);
                }

                #endregion
            }

            public class StoneComparerWithDefence : IComparer                                   //按综合值比较，相反，先比较综合值，再比较进攻
            {
                #region IComparer Members

                public int Compare(object x, object y)
                {
                    double dXValue, dYValue;
                    
                    dXValue = ((VirtualStone)x).ValueWithDefence; dYValue = ((VirtualStone)y).ValueWithDefence;
                    if (dXValue < dYValue) return 1;
                    if (dXValue > dYValue) return -1;
                    
                    dXValue = ((VirtualStone)x).Value; dYValue = ((VirtualStone)y).Value;
                    if (dXValue < dYValue) return 1;
                    if (dXValue > dYValue) return -1;
                    
                    return ((VirtualStone)x).Distance.CompareTo(((VirtualStone)y).Distance);
                }

                #endregion
            }

            public static StoneComparer stoneComparer = new StoneComparer();
            public static StoneComparerWithDefence stoneComparerWithDefence = new StoneComparerWithDefence();

            #endregion
        }

        private Tag[,] Board;                                                                   //棋盘
        private Thread AIThread;                                                                //AI线程

        public AIPlayer(Game game, Tag tag)
            : base(game, tag, "Cheat") { }
        
        private bool Available(int x, int y)                                                    //在设定的距离内有别的子，且可下
        {
            if (Analyzer.GetChess(Board, x, y) == Tag.No)
            {
                for (int i = x - Constants.DISTANCE; i <= x + Constants.DISTANCE; ++i)
                {
                    for (int j = y - Constants.DISTANCE; j <= y + Constants.DISTANCE; ++j)
                    {
                        if (Analyzer.GetChess(Board, i, j) > Tag.No)
                            return true;
                    }
                }
            }
            return false;
        }

        #region AI算法核心

        private double Search(Tag LevelTag, int nDepth, double dFMax, double dValue)            //搜索
        {
            int i, j, nMaxDot = Constants.MAX_DOT - nDepth;
            double dMax = Constants.MINIMUM;
            ArrayList stones = new ArrayList();
            
            for (i = 0; i < Constants.SIZE; ++i)
            {
                for (j = 0; j < Constants.SIZE; ++j)
                {
                    if (Available(i, j)) stones.Add(new VirtualStone(Board, LevelTag, i, j));   //枚举所有可以下的子
                }
            }
            stones.Sort(VirtualStone.stoneComparerWithDefence);                                 //按综合值排序
            if (((VirtualStone)stones[0]).Value > Constants.WIN - Constants.DELTA) return Constants.WIN;//获胜
            
            if (nDepth < Constants.MAX_DEPTH)                                                   //搜索下一层
            {
                i = 0;
                foreach (VirtualStone stone in stones)                                          //枚举前面几个棋子
                {
                    stone.Down();                                                               //落子，估值
                    stone.Value -= Search(Game.Divert(LevelTag), nDepth + 1, dMax, stone.Value) * Constants.DEC_RATE;
                    stone.Up();
                    if (++i == nMaxDot || dValue - stone.Value < dFMax) break;                  //剪枝
                    if (dMax < stone.Value) dMax = stone.Value;
                }
                stones.Sort(0, i, VirtualStone.stoneComparer);                                  //按纯进攻值比较
            }
            else
            {
                if (nDepth < Constants.MAX_EXTRA_DEPTH)                                         //超过搜索层次，推演最优子
                {
                    ((VirtualStone)stones[0]).Down();
                    ((VirtualStone)stones[0]).Value -= Search(Game.Divert(LevelTag), nDepth + 1, 0, 0) * Constants.DEC_RATE;
                    ((VirtualStone)stones[0]).Up();
                }
            }
            
            return ((VirtualStone)stones[0]).Value;                                             //返回最优子进攻值
        }

        private void Computer()
        {
            base.Play();
            AbstractStone DownStone;                                                            //落子

            if (game.Count == 0)
            {
                DownStone = new RealStone(game.Board, this, 7, 7);                              //占住天元
            }
            else
            {
                Board = (Tag[,])(game.Board.Clone());                                           //拷贝棋盘
                int i, j;
                double dMax = Constants.MINIMUM;
                ArrayList stones = new ArrayList();
                
                for (i = 0; i < Constants.SIZE; ++i)
                {
                    for (j = 0; j < Constants.SIZE; ++j)
                    {
                        if (Available(i, j)) stones.Add(new VirtualStone(Board, tag, i, j));    //枚举所有可下子
                    }
                }
                stones.Sort(VirtualStone.stoneComparerWithDefence);                             //按综合值排序
                
                if (((VirtualStone)stones[0]).Value < Constants.WIN - Constants.DELTA)          //没有立即获胜，搜索
                {
                    i = 0;
                    foreach (VirtualStone stone in stones)                                      //搜索（同上，不过无剪枝）
                    {
                        stone.Down();
                        stone.Value -= Search(Game.Divert(tag), 1, dMax, stone.Value) * Constants.DEC_RATE;
                        stone.Up();
                        if (dMax < stone.Value) dMax = stone.Value;
                        if (++i == Constants.MAX_DOT) break;
                    }
                    stones.Sort(0, i, VirtualStone.stoneComparer);                              //按进攻值排序
                }
                
                DownStone = new RealStone(game.Board, this, ((VirtualStone)stones[0]).X, ((VirtualStone)stones[0]).Y);//创建真实最优子
            }
            
            game.Down(DownStone);                                                               //落子
        }

        #endregion

        public override void Play()
        {
            AIThread = new Thread(Computer);                                                    //创建ai线程
            AIThread.IsBackground = true;                                                       //设置为背景线程
            AIThread.Start();                                                                   //开始线程
        }

        public override void Close()
        {
            if (AIThread != null) AIThread.Abort();                                             //关闭线程
        }
    }
}
