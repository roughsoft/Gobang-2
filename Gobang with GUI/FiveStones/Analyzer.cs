using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiveStones
{
    public static class Analyzer                                                    //分析估值静态类
    {
        public static Tag GetChess(Tag[,] Board, int x, int y)                      //获取当前位置棋子（若越界返回禁止）
        {
            if (x < 0 || x >= Constants.SIZE || y < 0 || y >= Constants.SIZE)
                return Tag.Forbidden;
            return Board[x, y];
        }

        private static void AnalyzeLine(Tag[] List, double[] Live, double[] Sleep)  //分析行列
        {
            int i1 = 4, i2 = 4, j1, j2, nLength;
            Tag tag = List[4];                                                      //最中间的位置为选取颜色
            bool bIsSleep = false;                                                  //眠X为否
            
            for (; i1 > -1 && List[i1] == tag; i1--) ;
            for (; i2 < List.Length  && List[i2] == tag && i2 - i1 < 6; i2++) ;     //连子数
            j1 = i1; j2 = i2;
            i1++; i2--;
            nLength = i2 - i1 + 1;                                                  //连子长度
            
            if (nLength == 5)                                                       //连五为胜
            {
                Live[5]++;
                return;
            }
            for (; j1 > -1 && List[j1] != Tag.Forbidden && List[j1] != Game.Divert(tag); j1--) ;
            for (; j2 < List.Length && List[j2] != Tag.Forbidden && List[j2] != Game.Divert(tag); j2++) ;
            if (j2 - j1 < 6) return;
            if (j2 - j1 == 6 || i1 - j1 == 1 || j2 - i2 == 1) bIsSleep = true;      //看看边界是否有敌人的子或出棋盘
            
            switch (nLength)
            {
                case 4:                                                             //四连
                    {
                        if (bIsSleep) Sleep[4] += Constants.CONTINOUS_MODIFICATION; //冲四
                        else Live[4]++;                                             //活四
                        break;
                    }
                case 3:
                    {
                        if (i1 - j1 > 2 && List[i1 - 2] != Tag.No)
                            if (j2 - i2 > 2 && List[i2 + 2] != Tag.No) Live[4]++;   //双冲四OXOOOXO
                            else Sleep[4]++;                                        //跳冲四
                        else if (j2 - i2 > 2 && List[i2 + 2] != Tag.No) Sleep[4]++; //冲四
                        else if (bIsSleep) Sleep[3] += Constants.CONTINOUS_MODIFICATION;
                        else Live[3] += Constants.CONTINOUS_MODIFICATION;           //眠三，活三
                        break;
                    }
                case 2:                                                             //连二的情况
                    {
                        if (i1 - j1 > 3 && List[i1 - 2] != Tag.No && List[i1 - 3] != Tag.No)
                            if (j2 - i2 > 3 && List[i2 + 2] != Tag.No && List[i2 + 3] != Tag.No) Live[4]++;
                            else Sleep[4]++;
                        else if (j2 - i2 > 3 && List[i2 + 2] != Tag.No && List[i2 + 3] != Tag.No) Sleep[4]++;
                        else if (i1 - j1 > 2 && List[i1 - 2] != Tag.No)
                            if (i1 - j1 == 3 || bIsSleep)
                                if (!bIsSleep && j2 - i2 > 3 && List[i2 + 2] != Tag.No) Live[3]++;
                                else Sleep[3]++;
                            else Live[3]++;
                        else if (j2 - i2 > 2 && List[i2 + 2] != Tag.No)
                            if (j2 - i2 == 3 || bIsSleep) Sleep[3]++;
                            else Live[3] += Constants.CONTINOUS_MODIFICATION;
                        else if (i1 - j1 > 3 && List[i1 - 3] != Tag.No || j2 - i2 > 3 && List[i2 + 3] != Tag.No) Sleep[3]++;
                        else if (bIsSleep) Sleep[2] += Constants.CONTINOUS_MODIFICATION;
                        else Live[2] += Constants.CONTINOUS_MODIFICATION;

                        break;
                    }
                case 1:                                                             //只有一个的情况，仍然有可能必胜，如OOOXOXOOO
                    {
                        if (i1 - j1 > 4 && List[i1 - 2] != Tag.No && List[i1 - 3] != Tag.No && List[i1 - 4] != Tag.No)
                            if (j2 - i2 > 4 && List[i2 + 2] != Tag.No && List[i2 + 3] != Tag.No && List[i2 + 4] != Tag.No) Live[4]++;
                            else Sleep[4]++;
                        else if (j2 - i2 > 4 && List[i2 + 2] != Tag.No && List[i2 + 3] != Tag.No && List[i2 + 4] != Tag.No) Sleep[4]++;
                        else if (i1 - j1 > 3 && List[i1 - 2] != Tag.No && List[i1 - 3] != Tag.No)
                            if (i1 - j1 == 4 || bIsSleep)
                                if (!bIsSleep && j2 - i2 > 4 && List[i2 + 2] != Tag.No && List[i2 + 3] != Tag.No) Live[3]++;
                                else Sleep[3]++;
                            else Live[3]++;
                        else if (j2 - i2 > 3 && List[i2 + 2] != Tag.No && List[i2 + 3] != Tag.No)
                            if (j2 - i2 == 4 || bIsSleep) Sleep[3]++;
                            else Live[3]++;
                        else if (i1 - j1 > 4 && List[i1 - 3] != Tag.No && List[i1 - 4] != Tag.No ||
                            j2 - i2 > 4 && List[i2 + 3] != Tag.No && List[i2 + 4] != Tag.No) Sleep[3]++;
                        else if (i1 - j1 > 2 && List[i1 - 2] != Tag.No || i1 - j1 > 3 && List[i1 - 3] != Tag.No ||
                            i1 - j1 > 4 && List[i1 - 4] != Tag.No || j2 - i2 > 2 && List[i2 + 2] != Tag.No ||
                            j2 - i2 > 3 && List[i2 + 3] != Tag.No || j2 - i2 > 4 && List[i2 + 4] != Tag.No) Sleep[2]++;
                        break;
                    }
            }
        }

        public static double GetValue(Tag[,] Board, AbstractStone stone)            //估值
        {
            return GetValue(Board, stone.X, stone.Y);
        }

        public static double GetValue(Tag[,] Board, int x, int y)
        {
            double dValue = 0;
            double[] Live = new double[6];
            double[] Sleep = new double[6];
            int i;
            Tag[] List = new Tag[9];                                                //从四个不同方向得到棋盘的数组
            
            for (i = 0; i < 9; i++) List[i] = GetChess(Board, x - 4 + i, y);
            AnalyzeLine(List, Live, Sleep);
            if (Live[5] > 0) return Constants.WIN;                                  //获胜直接返回
            
            for (i = 0; i < 9; i++) List[i] = GetChess(Board, x, y - 4 + i);
            AnalyzeLine(List, Live, Sleep);
            if (Live[5] > 0) return Constants.WIN;
            
            for (i = 0; i < 9; i++) List[i] = GetChess(Board, x - 4 + i, y - 4 + i);
            AnalyzeLine(List, Live, Sleep);
            if (Live[5] > 0) return Constants.WIN;
            
            for (i = 0; i < 9; i++) List[i] = GetChess(Board, x - 4 + i, y + 4 - i);
            AnalyzeLine(List, Live, Sleep);
            if (Live[5] > 0) return Constants.WIN;
            
            if (Sleep[4] > 2 - Constants.DELTA || Sleep[4] > 0 && Live[3] > 0) Live[4]++;//多于一个冲四，或一冲四一活三
            if (Live[4] > 0) return Constants.LIVE_4;
            if (Live[3] > 2 - Constants.DELTA) return Constants.DOUBLE_3;           //双三
            dValue = Sleep[4] * Constants.SLEEP_4 + Live[3] * Constants.LIVE_3 + Sleep[3] * Constants.SLEEP_3
                + Live[2] * Constants.LIVE_2 + Sleep[2] * Constants.SLEEP_2;        //正常估值
            return dValue;
        }
    }
}

