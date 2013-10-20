using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiveStones
{
    public abstract class AbstractStone                             //抽象棋子类
    {
        protected Tag[,] Board;                                     //棋盘
        protected int x, y;                                         //坐标
        public Tag tag { set; get; }                                //棋子颜色
        public int X { get { return x; } }
        public int Y { get { return y; } }

        public AbstractStone(Tag[,] Board, Tag tag, int x, int y)   //设置各初值
        {
            this.Board = Board;
            this.tag = tag;
            this.x = x;
            this.y = y;
        }
        public void Down() { Board[x, y] = tag; }                   //落子
        public void Up() { Board[x, y] = Tag.No; }                  //清除之前颜色
    }
}
