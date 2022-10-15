using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Test
{
    delegate int gameDelegate(int userHp);
    internal class Game
    {
        public static int hp = 1000;
        public static int lastDamage = 0;
        public static float time = 0;
        public static bool IsDie = false;
        public void GameRun()
        {
            //int userHp = 1000;

            gameDelegate game = TestGame;

            IAsyncResult testGame = game.BeginInvoke(hp, new AsyncCallback(TestCallBack), "ㅠ");

            testGame.AsyncWaitHandle.WaitOne();

            Console.WriteLine("종료");
            //int gameResult = game.EndInvoke(testGame);
            //Console.WriteLine(gameResult);
            //game.Invoke(1000);
            Thread.Sleep(1000);
        }

        private static void TestCallBack(IAsyncResult ar)
        {

            gameDelegate temp = ((AsyncResult)ar).AsyncDelegate as gameDelegate;
            int gameResult = temp.EndInvoke(ar);
            //Console.WriteLine(gameResult);
            Console.WriteLine("막타 : {0}, 시간 : {1}초", gameResult, Math.Round(time, 1));
        }

        private static int TestGame(int userHp)
        {
            Thread Thread100 = new Thread(Damage);
            ThreadParam tp1 = new ThreadParam(100, -3);

            Thread Thread500 = new Thread(Damage);
            ThreadParam tp2 = new ThreadParam(500, -5);

            Thread Thread2500 = new Thread(Damage);
            ThreadParam tp3 = new ThreadParam(2500, -105);

            Thread Thread1000 = new Thread(Damage);
            ThreadParam tp4 = new ThreadParam(1000, +3);

            Thread ThreadTimer = new Thread(Timer);

            ThreadTimer.Start();
            Thread100.Start(tp1);
            Thread500.Start(tp2);
            Thread2500.Start(tp3);
            Thread1000.Start(tp4);
            

            while (!IsDie)
            {

            }

            if (IsDie)
            {
                Thread100.Abort();
                Thread500.Abort();
                Thread1000.Abort();
                Thread2500.Abort();
            }

            return lastDamage;
        }

        private static void Timer()
        {
            while (hp > 0)
            {
                Thread.Sleep(100);
                time += 0.1f;
                Console.WriteLine("{0}초", Math.Round(time, 1));
                //Console.WriteLine("{0}초", time);
            }
        }

        private static void Damage(object obj)
        {
            while (hp >= 0)
            {
                ThreadParam tempParam = obj as ThreadParam;
                Thread.Sleep(tempParam.param1);
                hp += tempParam.param2;

                Console.WriteLine("현재 체력 : {0} , {1}", hp, tempParam.param2);

                if (hp <= 0)
                {

                    lastDamage = tempParam.param2;
                    //Console.WriteLine("{0}로 인해 사망", lastDamage);
                    IsDie = true;

                    //Environment.Exit(0);
                }
            }
        }
    }
    class ThreadParam
    {
        public int param1;
        public int param2;
        public ThreadParam(int param1, int param2)
        {
            this.param1 = param1;
            this.param2 = param2;
        }
    }
}