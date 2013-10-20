using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace FiveStones
{
    public sealed class VideoPlayer : AbstractPlayer                    //录像玩家
    {
        private Queue lstStone = new Queue();                           //保存要下的棋子
        
        public VideoPlayer(Game game, Tag tag, string szName)
            : base(game, tag, szName) { }

        public void Add(RealStone stone) { lstStone.Enqueue(stone); }   //添加棋子
        
        public override void Play()
        {
            base.Play();
            if (lstStone.Count == 0) game.GiveUp();                     //队列空
            else
            {
                RealStone stone = (RealStone)(lstStone.Dequeue());      //队列有子，落子
                game.Down(stone);
            }
        }
    }
}
