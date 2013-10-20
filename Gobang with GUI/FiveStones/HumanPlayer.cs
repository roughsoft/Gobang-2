using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiveStones
{
    public class HumanPlayer : AbstractPlayer                   //人类玩家
    {
        public HumanPlayer(Game game, Tag tag, string szName)
            : base(game, tag, szName) { }

        public override void MouseDown(int X, int Y)            //人类通过鼠标点击落子
        {
            int x, y;
            x = (int)(X / Constants.LENGTH_OF_SQUARE);          //将像素坐标转化为棋盘坐标
            y = (int)(Y / Constants.LENGTH_OF_SQUARE);
            
            if (Math.Abs(X - x * Constants.LENGTH_OF_SQUARE - Constants.HALF_LENGTH_OF_SQUARE) > Constants.QUARTER_LENGTH_OF_SQUARE 
                || Math.Abs(Y - y * Constants.LENGTH_OF_SQUARE - Constants.HALF_LENGTH_OF_SQUARE) > Constants.QUARTER_LENGTH_OF_SQUARE)
                return;                                         //点击位置必须在以纵横线相交点为中心的10*10的正方形内
            
            game.Down(x, y);                                    //落子
        }
    }
}