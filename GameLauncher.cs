using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Media;

namespace Study08
{
    public struct Position
    {
        public int row;
        public int col;
        public Position(int xPos, int yPos)
        {
            this.row = xPos;
            this.col = yPos;
        }
    }
    //게임을 구동하는 클래스
    public class GameLauncher
    {
        private int[,] arrPlate;              //판
        private int turn;
        private int score;
        public GameLauncher()
        {

        }

        //초기화
        public void Init()
        {
            Console.Clear();
            score = 0;
            turn = 0;

            int menu = ViewTitle();
            if (menu == 1)
            {
                CreatePlate();
                StartGame();
            }
            else
            {
                EndGame();
            }
        }

        //새로운 판을 생성
        public void CreatePlate()
        {
            this.arrPlate = new int[5, 5];          //5x5 판 생성
            CreateBlock();
            Print();
        }

        //빈 자리 중 한 칸에 랜덤하게 2 또는 4가 나온다
        public void CreateBlock()
        {
            Random r = new Random();
            int row;
            int col;
            do
            {
                if (!CheckZero())
                {
                    Print();
                    Console.WriteLine("\t\t\t\t\t\t빈\t칸\t 없\t음");
                    EndGame();
                }
                row = r.Next(0, 4);
                col = r.Next(0, 4);
            } while (arrPlate[row, col] != 0); ;
            arrPlate[row, col] = (r.Next(1, 101) <= 50 ? 2 : 4);
        }

        //게임 시작
        public void StartGame()
        {
            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                Console.WriteLine();
                Console.Clear();
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Red;
                SystemSounds.Exclamation.Play();
                if (keyInfo.Key == ConsoleKey.LeftArrow)
                {
                    for (int i = 0; i < arrPlate.GetLength(0); i++)
                    {
                        for (int j = 1; j < arrPlate.GetLength(1); j++)
                        {
                            //첫번째 가로줄부터 왼쪽부터 오른쪽으로 탐색.
                            //맨 왼쪽에 있는것은 판별할 이유가 없기에 인덱스 1번부터 시작
                            int pos = 0;
                            if (arrPlate[i, j] != 0)
                            {
                                pos = j;
                                while (pos > 0 && arrPlate[i, pos - 1] == 0)
                                {
                                    //index가 1 이상이고 왼쪽이 0(빈칸)이면 서로 교체
                                    //왼쪽이 0이 아닌 숫자가 있거나 pos가 0이 될때까지 이동
                                    Swap(new Position(i, pos--), new Position(i, pos));
                                }
                            }

                            if (pos > 0 && pos < arrPlate.Length && arrPlate[i, pos - 1] == arrPlate[i, pos])
                            {
                                //교체후 왼쪽에 같은 숫자가 있으면 왼쪽 칸은 x2 , 오른쪽 칸은 0으로 채우고 스코어 + 1
                                arrPlate[i, pos - 1] *= 2;
                                arrPlate[i, pos] = 0;
                                score++;
                            }
                        }
                    }
                }
                else if (keyInfo.Key == ConsoleKey.RightArrow)
                {
                    for (int i = 0; i < arrPlate.GetLength(0); i++)
                    {
                        for (int j = arrPlate.GetLength(1) - 2; j >= 0; j--)
                        {
                            //첫번째 가로줄부터 오른쪽에서 왼쪽으로 탐색
                            //맨 오른쪽에 있는것은 판별할 필요 없음. 맨 오른쪽 인덱스-1 부터 탐색 시작
                            int pos = 0;
                            if (arrPlate[i, j] != 0)
                            {
                                pos = j;
                                while (pos < arrPlate.GetLength(1) - 1 && arrPlate[i, pos + 1] == 0)
                                {
                                    //index가 맨 오른쪽보다 작고 오른쪽이 0(빈칸)이면 서로 교체
                                    //오른쪽이 0이 아닌 숫자가 있거나 pos가 마지막 인덱스가 될때까지 이동
                                    Swap(new Position(i, pos++), new Position(i, pos));
                                }
                            }

                            if (pos >= 0 && pos < arrPlate.GetLength(1) - 1 && arrPlate[i, pos + 1] == arrPlate[i, pos])
                            {
                                //오른쪽값과 비교해서 오른쪽과 같으면 오른쪽에 있는 칸을 x2, 본인 칸을 0으로 만듬
                                arrPlate[i, pos + 1] *= 2;
                                arrPlate[i, pos] = 0;
                                score++;

                            }
                        }
                    }
                }
                else if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    //위쪽 화살표와 오른쪽 화살표는 왼쪽 오른쪽 할때를 90도 뒤집었다 생각하면 된다. 즉 i,j값이 뒤바뀐다
                    //나머지는 동일
                    for (int i = 0; i < arrPlate.GetLength(1); i++)
                    {
                        int pos = 0;
                        for (int j = 0; j < arrPlate.GetLength(0); j++)
                        {
                            if (arrPlate[j, i] != 0)
                            {
                                pos = j;
                                while (pos > 0 && arrPlate[pos - 1, i] == 0)
                                {
                                    Swap(new Position(pos--, i), new Position(pos, i));
                                }
                            }
                            if (pos > 0 && arrPlate[pos, i] == arrPlate[pos - 1, i])
                            {
                                arrPlate[pos - 1, i] *= 2;
                                arrPlate[pos, i] = 0;
                                score++;
                            }
                        }
                    }
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    for (int i = 0; i < arrPlate.GetLength(1); i++)
                    {
                        for (int j = arrPlate.GetLength(0) - 2; j >= 0; j--)
                        {
                            int pos = 0;
                            if (arrPlate[j, i] != 0)
                            {
                                pos = j;
                                while (pos < arrPlate.GetLength(0) - 1 && arrPlate[pos + 1, i] == 0)
                                {
                                    Swap(new Position(pos++, i), new Position(pos, i));
                                }
                                if (pos < arrPlate.GetLength(0) - 1 && arrPlate[pos, i] == arrPlate[pos + 1, i])
                                {
                                    arrPlate[pos, i] = 0;
                                    arrPlate[pos + 1, i] *= 2;
                                    score++;
                                }
                            }
                        }
                    }
                }
                else if (keyInfo.Key == ConsoleKey.Escape)
                {
                    Console.Clear();
                    this.EndGame();
                }
                CreateBlock();
                turn++;
                Print();
            }
        }

        //게임 종료
        public void EndGame()
        {
            Console.WriteLine("\n\n\n\n\n\t\t\t\t\t\t게\t임\t종\t료");
            System.Diagnostics.Process.GetCurrentProcess().Kill();          //프로세스 강제종료
        }

        private void Swap(Position a, Position b)
        {
            int temp = arrPlate[a.row, a.col];
            arrPlate[a.row, a.col] = arrPlate[b.row, b.col];
            arrPlate[b.row, b.col] = temp;
        }

        private void Print()
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n\n\t\t\t\t\t\t턴 : {0}, 점수 : {1}", turn, turn + score * 3);
            Console.WriteLine("\n\n\n\n");
            Console.WriteLine("\t\t\t\t==============================================");
            for (int i = 0; i < arrPlate.GetLength(0); i++)
            {
                Console.Write("\t\t\t\t");
                for (int j = 0; j < arrPlate.GetLength(1); j++)
                {
                    if (j == 0)
                    {
                        Console.Write("||");
                    }
                    Console.Write("{0, 5}", arrPlate[i, j]);
                    if (j == arrPlate.GetLength(1) - 1)
                    {
                        Console.Write(" ||");
                    }
                    else
                    {
                        Console.Write("  ||");
                    }
                }
                Console.WriteLine();
                Console.WriteLine("\t\t\t\t==============================================");
            }
        }

        //판에 0이 적어도 하나정도는 있는가?, 굳이 0이 몇개있는지와 어디에 0이 있는지는 알 필요가 없다.
        private bool CheckZero()
        {
            for (int i = 0; i < arrPlate.GetLength(0); i++)
            {
                for (int j = 0; j < arrPlate.GetLength(1); j++)
                {
                    if (arrPlate[i, j] == 0)
                        return true;
                }
            }
            return false;
        }

        private int ViewTitle()
        {
            string title = @"                                kkkkkkO000KKKKKKKKKKKKK000KKKKKKKKKKKKKKKK0Okkkkkk
                                kkkO0KXNNNNNNNNNNNNNNNNXKKNWWWWWWWWWWWWWWWWNX0Okkk
                                kkOKNNNNNNNNXXNNNNNNNNNXKKNWWWWWWWWWWNWWWWWWWWX0kk
                                k0XNNNNNX0xdddxOKNNNNNNXKKNWWWWWWNKkdddx0NWWWWWN0k
                                OKNNNNNNXOkOOxlcxKNNNNNXKKNWWWWWNOllk0xllOWWWWWWXO
                                OXNNNNNNNNNNN0ocxXNNNNNXKKNWWWWWXdcdXWKdcxXWWWWWN0
                                0XNNNNNNNNNKkolxKNNNNNNXKKNWWWWWKocxNWXdcdXWWWWWN0
                                OXNNNNNNNKkoox0XNNNNNNNXKKNWWWWWXdcdXWKocxNWWWWWN0
                                OXNNNNNN0occoxxxOXNNNNNXKKNWWWWWW0oldkdldKWWWWWWN0
                                OXNNNNNN0kxxxxxxOXNNNNNXKKNWWWWWWWXOkkO0NWWWWWWWN0
                                OXNNNNNNNNNNNNNNNNNNNNNXKKNWWWWWWWWWWWWWWWWWWWWWN0
                                OKXXXXXXXXXXXXXXXXXXXXXK0KXNNNNNNNNNNNNNNNNNNNNNX0
                                O0KKKKKKKKKKKKKKKKKKKKK0OOO000000000000000000000OO
                                OXXXXXXXXXXXXXXXXXNXXXXX0OOOOOOOOOOOOOOOOOOOOOOOOk
                                OXXXXXXXXXXXXXKXXXXXXXXK0OOOOOOOOOO00000OOOOOOOOOO
                                OXNXXXXXXXXXOdoxKNXXXXXK0OOOOOOOO0XNNNNNX0OOOOOOOO
                                OXXXXXXNNXKxolco0XXXXXNX0OOOOOOO0XWWX0XWWXOOOOOOOO
                                OXXXXXXNX0dokxco0NXXXXXX0OOOOOOOOKNWNXNWX0OOOOOOOO
                                OXXXXXXXOook0xcoOXXXXXXX0OOOOOOO0KNWNNWWXK0OOOOOOk
                                OKXXXNXKxloddlcldOXXXXXK0OOOOOOO0NWNK0KNMX0OOOOOOk
                                O0XXXXXXKKKK0xcoOXXXXXXK0OOOOOOO0XWWNXNWNX0OOOOOOk
                                kOKXXXXXXXXXXKO0XXXXXXXK0OOOOOOOO00KXXXK00OOOOOOkk
                                kkk0KXXXXXXXXXXNNXXXXXXK0OOOOOOOOOOOOOOOOOOOOOOkkk
                                kkkkOO0KKXXXXXXXXXXXXXXKOOOOOOOOOOOOOOOOOOOOkkkkkk
                                kkkkkkkkOOOOOOOOOOOOOOOOOOOOOOOOOkkkkkkkkkkkkkkkkk";
            Console.WriteLine(title);
            int menu = 0;
            Console.WriteLine("\n                                       1: START GAME\t2:SHUT DOWN");
            menu = Convert.ToInt32(Console.ReadLine());

            return menu;

        }
    }
}
