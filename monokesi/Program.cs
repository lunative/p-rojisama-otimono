using System;
using System.Collections.Generic;

namespace monokesi
{
    public struct Point
    {
        public int x, y;
        public Point(int x, int y) { this.x = x; this.y = y; }
    }

    public class Program
    {
        public static void monokesiColor(int hCount, int vCount, string kesiColor, Point kesiPoint, ref string[,] blockList)
        {
            // 消しポイントの上下左右を確認,これ再帰必要か。。
            // 上にブロックある？
            if (kesiPoint.y - 1 >= 0)
            {
                // 色を確認
                if (blockList[kesiPoint.y - 1, kesiPoint.x] == kesiColor)
                {
                    // 消し対象の色ならWを代入
                    blockList[kesiPoint.y - 1, kesiPoint.x] = "W";
                    // 再帰。上下左右の色を確認
                    monokesiColor(hCount, vCount, kesiColor, new Point(kesiPoint.x, kesiPoint.y - 1), ref blockList);
                }
            }
            // 下にブロックある？
            if (kesiPoint.y + 1 < vCount)
            {
                // 色を確認
                if (blockList[kesiPoint.y + 1, kesiPoint.x] == kesiColor)
                {
                    // 消し対象の色ならWを代入
                    blockList[kesiPoint.y + 1, kesiPoint.x] = "W";
                    // 再帰。上下左右の色を確認
                    monokesiColor(hCount, vCount, kesiColor, new Point(kesiPoint.x, kesiPoint.y + 1), ref blockList);
                }
            }
            // 左にブロックある？
            if (kesiPoint.x - 1 >= 0)
            {
                // 色を確認
                if (blockList[kesiPoint.y, kesiPoint.x - 1] == kesiColor)
                {
                    // 消し対象の色ならWを代入
                    blockList[kesiPoint.y, kesiPoint.x - 1] = "W";
                    // 再帰。上下左右の色を確認
                    monokesiColor(hCount, vCount, kesiColor, new Point(kesiPoint.x - 1, kesiPoint.y), ref blockList);
                }
            }
            // 左にブロックある？
            if (kesiPoint.x + 1 < hCount)
            {
                // 色を確認
                if (blockList[kesiPoint.y, kesiPoint.x + 1] == kesiColor)
                {
                    // 消し対象の色ならWを代入
                    blockList[kesiPoint.y, kesiPoint.x + 1] = "W";
                    // 再帰。上下左右の色を確認
                    monokesiColor(hCount, vCount, kesiColor, new Point(kesiPoint.x + 1, kesiPoint.y), ref blockList);
                }
            }
        }

        public static void Main(string[] args)
        {
            var line = Console.ReadLine();
            var data = line.Split(' ');
            // ブロックの集まりの列数(x)
            int hCount = int.Parse(data[0]);
            // ブロックの集まりの行数(y)
            int vCount = int.Parse(data[1]);
            // 「モノケシ」の魔法を使える回数を表す整数
            int spellCount = int.Parse(data[2]);

            string[,] blockList = new string[vCount, hCount];
            // ブロック読み込み
            for (int tate = 0; tate < vCount; tate++)
            {
                var nextLine = Console.ReadLine();
                for (int yoko = 0; yoko < hCount; yoko++)
                {
                    blockList[tate, yoko] = nextLine[yoko].ToString();
                }
            }

            int monokesiCount = 0;
            List<Point> kesiPointList = new List<Point>();
            // 「モノケシ」の回数繰り返し
            for (int monokesi = 0; monokesi < spellCount; monokesi++)
            {
                // どこを削除するか探索する
                /*
                横で連続している色を見つける？
                探索は左上から、一番最初に見つけた最長の連続した色を消す
                ぷよぷよとかだと消した後の形まで考えるけど・・・
                */
                int maxSCount = 0;
                Point kesiPoint = new Point(0, 0);
                for (int tate = 0; tate < vCount; tate++)
                {
                    string lastColor = "";
                    int sameCount = 0;
                    for (int yoko = 0; yoko < hCount; yoko++)
                    {
                        string nColor = blockList[tate, yoko];
                        if (nColor == "W") { continue; }
                        else if (nColor == "R" || nColor == "G" || nColor == "B")
                        {
                            if (lastColor == "" || lastColor != nColor)
                            {
                                lastColor = nColor;
                                sameCount = 1;
                                if (maxSCount < sameCount)
                                {
                                    kesiPoint.x = yoko;
                                    kesiPoint.y = tate;
                                    maxSCount = sameCount;
                                }
                            }
                            else if (lastColor == nColor)
                            {
                                sameCount++;
                                if (maxSCount < sameCount)
                                {
                                    kesiPoint.x = yoko;
                                    kesiPoint.y = tate;
                                    maxSCount = sameCount;
                                }
                            }
                            else { Console.WriteLine("Error:UndefinedError"); }
                        }
                        else { Console.WriteLine("Error:ColorIsError"); }
                    }
                }
                kesiPointList.Add(kesiPoint);
                if (maxSCount == 0) { break; }
                monokesiCount++;

                // 削除処理
                /*
                落ちた所の色はW(白)にする？
                */
                // 消す色を確認
                string kesiColor = blockList[kesiPoint.y, kesiPoint.x];
                // 消しポイントにWを代入
                blockList[kesiPoint.y, kesiPoint.x] = "W";
                // 消しポイントの上下左右を確認,これ再帰必要か。。
                monokesiColor(hCount, vCount, kesiColor, kesiPoint, ref blockList);

                // ブロックが消えた位置の上にブロックがあれば移動させる
                bool kesiFlag = true;
                while (kesiFlag == true)
                {
                    kesiFlag = false;
                    for (int tate = 1; tate < vCount; tate++)
                    {
                        for (int yoko = 0; yoko < hCount; yoko++)
                        {
                            // 色が白(W)
                            if (blockList[tate, yoko] == "W")
                            {
                                // 上にブロックがあるか？
                                if (blockList[tate - 1, yoko] != "W")
                                {
                                    // あるなら落とす
                                    blockList[tate, yoko] = blockList[tate - 1, yoko];
                                    blockList[tate - 1, yoko] = "W";
                                    kesiFlag = true;
                                }
                            }
                        }
                    }
                }

                Console.WriteLine("-----------");
                // ブロックリスト確認
                for (int tate = 0; tate < vCount; tate++)
                {
                    string output = "";
                    for (int yoko = 0; yoko < hCount; yoko++)
                    {
                        output += blockList[tate, yoko];
                    }
                    Console.WriteLine(output);
                }
            }

            // 「モノケシ」の呪文を使った回数
            Console.WriteLine(monokesiCount);
            // 「モノケシ」の呪文で指定したブロックの列番号 xi 及び行番号 yi
            for (int i = 0; i < monokesiCount; i++)
            {
                Console.WriteLine("{0} {1}", kesiPointList[i].x + 1, kesiPointList[i].y + 1);
            }
        }
    }
}
