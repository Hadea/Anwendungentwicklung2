using System;
using System.Threading.Tasks;

namespace ThreadingConsole
{
    class Program
    {
        static Object sync = new Object();
        static void Main()
        {
            Console.WriteLine($"Willkommen zum Threading-Spielplatz. Sie haben {Environment.ProcessorCount} Prozessoren. Loslogen mit Enter");
            Console.ReadLine();

            var t = new Task<int>[4];

            //t[0] = new Task<int>(() => { return countUpV2("T0", 0); });
            //t[1] = new Task<int>(() => { return countUpV2("T1", 0); });
            //t[2] = new Task<int>(() => { return countUpV2("T2", 0); });
            //t[3] = new Task<int>(() => { return countUpV2("T3", 0); });
            t[0] = new Task<int>(() => { return countUp("T0"); });
            t[1] = new Task<int>(() => { return countUp("T1"); });
            t[2] = new Task<int>(() => { return countUp("T2"); });
            t[3] = new Task<int>(() => { return countUp("T3"); });

            foreach (var item in t) item.Start();

            DateTime startTime = DateTime.Now;
            while ((DateTime.Now - startTime).TotalSeconds < 6)
            {
                Task.Delay(250).Wait();
                Console.WriteLine("Aktueller Zählerstand: {0}", counter);
            }

            Task.WaitAll(t);

            int summeThreads = 0;
            foreach (var item in t) summeThreads += item.Result;

            Console.WriteLine("letzter Zählerstand: {0}, summe der Threads: {1}", counter, summeThreads);
            Console.WriteLine("Fertig");
            Console.ReadLine();
        }

        static int counter = 0;

        static int countUp(string name)
        {
            int eigenerCounter = 0;
            DateTime startTime = DateTime.Now;
            while ((DateTime.Now - startTime).TotalSeconds < 5)
            {
                lock (sync)
                {
                    eigenerCounter += 1;
                    counter += 1;
                }
            }
            Console.WriteLine($"Thread: {name}, intern: {eigenerCounter}, vergangene Zeit: {(DateTime.Now - startTime).TotalSeconds}");
            return eigenerCounter;
        }

        static int countUpV2(string name, int refreshspeed)
        {
            int eigenerCounter = 0;
            DateTime startTime = DateTime.Now;
            int zwischenCounter = 0;
            while ((DateTime.Now - startTime).TotalSeconds < 5)
            {
                eigenerCounter += 1;
                zwischenCounter += 1;
                if (zwischenCounter > refreshspeed) lock (sync)
                    {
                        counter += zwischenCounter;
                        zwischenCounter = 0;
                    }
            }
            counter += zwischenCounter;
            Console.WriteLine($"Thread: {name}, intern: {eigenerCounter}, vergangene Zeit: {(DateTime.Now - startTime).TotalSeconds}");
            return eigenerCounter;
        }
    }
}
