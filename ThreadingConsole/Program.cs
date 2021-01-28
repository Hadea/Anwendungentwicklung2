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
            for (int counter = 0; counter < t.Length; counter++)
                t[counter] = new Task<int>(() => { return countUp("T" + counter.ToString()); });

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
            Console.WriteLine($"Thread: {name}, intern: {eigenerCounter}");
            return eigenerCounter;
        }
    }
}
