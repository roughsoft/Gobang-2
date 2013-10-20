using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace FiveStones
{
    public sealed class RealStone : AbstractStone                           //真实棋子类
    {
        private static Image[] imgStone = new Image[3];                     //棋子图片
        
        public static void Init(Image imgBlack, Image imgWhite)             //获取黑白子图片
        {
            imgStone[1] = imgBlack;
            imgStone[2] = imgWhite;
        }

        private AbstractPlayer player;                                      //棋子玩家

        public RealStone(Tag[,] Board, AbstractPlayer player, int x, int y) //构造函数，传入玩家
            : base(Board, player.tag, x, y)
        {
            this.player = player;
        }

        public void Draw(Graphics g)                                        //画出棋子
        {
            g.DrawImage(imgStone[(int)tag], x * Constants.LENGTH_OF_SQUARE, y * Constants.LENGTH_OF_SQUARE);
        }

        public void DrawHighlight(Graphics g)                               //在棋子上画一个希腊式十字，表示选中
        {
            int X, Y;                                                       //棋盘坐标转为像素坐标
            X = x * Constants.LENGTH_OF_SQUARE + Constants.HALF_LENGTH_OF_SQUARE;
            Y = y * Constants.LENGTH_OF_SQUARE + Constants.HALF_LENGTH_OF_SQUARE;
            
            Pen pen = new Pen(Color.Red, Constants.STAR_WIDTH);             //指定粗细的画笔
            g.DrawLine(pen, new Point(X - 5, Y), new Point(X + 5, Y));      //画出一横一竖
            g.DrawLine(pen, new Point(X, Y - 5), new Point(X, Y + 5));
        }
         
        public override string ToString()                                   //用来在ListBox表示
        {
            return player + ((char)(x + 'A')).ToString() + (y + 1).ToString();
        }

        public void Save(System.IO.BinaryWriter writer)                     //保存
        {
            writer.Write((int)tag);                                         //保存颜色，坐标
            writer.Write(x);
            writer.Write(y);
        }
    }
}
