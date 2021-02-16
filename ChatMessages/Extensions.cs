using System;
using System.Text;

namespace ChatMessages
{
    public static class Extensions
    {
        /* Konvertierungs-Erweiterungen damit der Algorhithmus zum Nachrichten konvertieren 
         * an nur einem einzigen punkt im Code ausgetauscht werden muss.
         */
        public static byte[] ConvertToArray(this String str)
        {
            return Encoding.ASCII.GetBytes(str);
        }

        public static string ConvertToString(this byte[] arr)
        {
            return Encoding.ASCII.GetString(arr);
        }
    }
}
