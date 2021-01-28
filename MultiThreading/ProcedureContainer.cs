using System.Threading;

namespace MultiThreading
{
    class ProcedureContainer
    {
        public static string Wait5SecForString()
        {
            Thread.Sleep(5000);
            return "done";
        }
    }
}
