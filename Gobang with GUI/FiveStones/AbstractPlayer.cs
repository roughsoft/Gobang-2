using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiveStones
{
    public abstract class AbstractPlayer                        //抽象选手类
    {
        protected Game game;                                    //选手参加的游戏
        protected string szName;                                //用户名

        public string Name { get { return szName; } }           //用户名属性
        public Tag tag { set; get; }                            //选手棋子颜色

        public AbstractPlayer(Game game, Tag tag, string szName)//默认构造函数，传入游戏、棋子颜色、用户名
        {
            this.game = game;
            this.tag = tag;
            this.szName = szName;
        }
        public virtual void MouseDown(int x, int y) { }         //鼠标点击不做任何处理
        public virtual void Play()                              //轮到玩家
        {
            game.CurrentStartTime = DateTime.Now;               //设置初始时间
        }
        public override string ToString()                       //转换字符串
        {
            return szName + "(" + tag.ToString() + ")";
        }
        public virtual void Close() { }                         //玩家结束游戏时后续处理
    }
}
