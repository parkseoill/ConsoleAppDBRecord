using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAppDBRecord
{
    internal class Program
    {
        static int  Px=0, Py=0, Ps=0,AllData=0, StartCount=0;

        static void Main(string[] args)
        {
            Random Rand = new Random();
            Thread DBThread1 = new Thread(DBFunc1);         
            Thread DBThread2 = new Thread(DBFunc1);
            Thread DBThread3 = new Thread(DBFunc1);           
            Thread DBThread4 = new Thread(DBFunc1);

            int RandThread1 = Rand.Next(100,500);
            int RandThread2 = Rand.Next(100,2000);
            DBThread1.Start(3000);
            DBThread2.Start(4000);
            DBThread3.Start(RandThread1);
            Thread.Sleep(500);
            DBThread4.Start(RandThread2);
        }

        private static string CallDB()
        {
            string connStr = "Server = 192.168.245.1;";//Server =172.168.0.47;";//192.168.0.82
            connStr += "Uid=root;";
            connStr += "Database=agv_db;";
            connStr += "Pwd=1234;";

            /*string connStr = "Server = 192.168.245.1;";//Server =172.168.0.47;";//192.168.0.82
            connStr += "Uid=root;";
            connStr += "Database=agv_db;";
            connStr += "Pwd=tjdlf926;";*/
            return connStr;

        }
        private static string Rand()
        {

            Random Rand = new Random();
            StartCount++;
            if(StartCount == 1)
            {
                //x 24 y 12 360
                Px = Rand.Next(-2400, 2400);
                Py = Rand.Next(-1200, 1200);
                Ps = Rand.Next(0, 360);
            }
            int RPx = Rand.Next(0, 20);
            int RPy = Rand.Next(0, 20);
            int RPs = Rand.Next(0, 10);    
            int PlusorMinus = Rand.Next(0, 2);

            if (PlusorMinus == 0  && Px  >-2400)
            {
                 Px -= RPx;
            }
            if (PlusorMinus == 0  && Py >- 1200)
            {
                 Py -= RPy;
            }
            if (PlusorMinus == 0 && Ps > 0)
            {
                 Ps -= RPs;
            }
             //if()
             else
             {
                  Px += RPx; Py += RPy; Ps += RPs;
             }

                int Bf = Rand.Next(0, 2);
                int Bb = Rand.Next(0, 2);
                int Bl = Rand.Next(0, 2);
                int Br = Rand.Next(0, 2);
                int EMR = Rand.Next(0, 2);
                int Start = Rand.Next(0, 2);
                int Stop = Rand.Next(0, 2); 
            
                string qurry = "INSERT INTO agv_table (Position_X, Position_Y, Position_S, Bumper_Front, Bumper_Back, Bumper_Left, Bumper_Right," +
                " EMR, Start, Stop, D_Time) VALUES (" + Px + "," + Py + "," + Ps + "," + Bf + "," + Bb + "," + Bl + "," + Br + "," + EMR + "," + Start + "," + Stop + ",NOW(3))";

            return qurry;
        }
        private static void DBFunc1(Object obj)
        {
            MySqlConnection conn = new MySqlConnection(CallDB());         
            bool bA = true;
            int num = (int)obj;
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                Console.WriteLine("Connected to MySQL.");
                while (bA)
                {
                   
                    if(AllData == 200)
                    {
                        Environment.Exit(0);
                    }
                    AllData++;

                    Random Rand1 = new Random();
                    int RandSleep = Rand1.Next(500, 10000);
                    string qurry = Rand();//랜덤 함수 받아오기 
                    MySqlCommand cmd = new MySqlCommand(qurry, conn);
                    cmd.ExecuteNonQuery();
                    if (num == 3000)
                    {
                        Thread.Sleep(num);
                        Console.WriteLine("3초 정주기 보내는중..!" + num + "m/s");
                    
                    }
                    else if (num == 4000)
                    {
                        Thread.Sleep(num);
                        Console.WriteLine("4초 정주기 보내는중..!" + num + "m/s");
                    }
                    else
                    {
                        Thread.Sleep(RandSleep);
                        Console.WriteLine("랜덤 주기 보내는중..!" + RandSleep + "m/s");
                    }
                }
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                conn.Close();   
            }
            Console.ReadLine();
        }
    }
}
