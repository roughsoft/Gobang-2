using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiveStones
{
    public static class Constants
    {
        public const int
            LENGTH_OF_SQUARE = 40,              //棋盘每格子边长像素
            HALF_LENGTH_OF_SQUARE = 20,         //半边长
            QUARTER_LENGTH_OF_SQUARE = 10,      //四分之一边长
            
            SUM_STONE = 3,                      //黑白两个枚举常量之和
            MAX_STONE = 225,                    //棋盘可容纳棋子数
            TIME_RATE = 1000,                   //一秒的毫秒数
            STAR_WIDTH = 2,                     //画星星的宽度
            
            AI_PLAYER = 2,                      //电脑玩家的Flag
            HUMAN_PLAYER = 1,                   //人类玩家的Flag
            
            PORT = 2255,                        //端口
            MAX_NAME_LENGTH = 1024,             //名字最大长度
            NET_TIME_LIMIT = 1,                 //网络时间限制
            NET_GIVE_UP = 255,                  //网络发送放弃的棋子坐标
            
            SIZE = 15,                          //棋盘大小
            DISTANCE = 3,                       //AI考虑落子距离最近有棋子的最远距离
            MAXIMUM = 100000000,                //最大与最小
            MINIMUM = -100000000,            
            
            MAX_DOT = 7,                        //每层搜索结点数上限
            MAX_DEPTH = 6,                      //搜索层数
            MAX_EXTRA_DEPTH = 9;                //额外推算层数
        
        public const double
            DELTA = 0.5,                        //浮点误差
            CONTINOUS_MODIFICATION = 1.05,      //连子连续性修正
            
            WIN = 100000,                       //获胜，活四，双三，冲四，活三，眠三，活二，眠二的取值
            LIVE_4 = 10000,
            DOUBLE_3 = 1000,
            
            SLEEP_4 = 500,
            LIVE_3 = 300,
            SLEEP_3 = 50,
            LIVE_2 = 4,
            SLEEP_2 = 1,
            
            AD_RATE = 0.25,                     //攻防比
            DEC_RATE = 0.9;                     //每层搜索结果衰变比率
    }
}
