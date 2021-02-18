using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChatMessages;

namespace ChatTests
{
    [TestClass]
    public class ExtensionTests
    {
        [TestMethod]
        public void ConvertToArray()
        {
            // vorbereitung
            string source = "Hallo Welt!";
            byte[] expected = new byte[] { 0x48, 0x61, 0x6c, 0x6c, 0x6f, 0x20, 0x57, 0x65, 0x6c, 0x74, 0x21 };
            
            // durchführung
            byte[] arr = source.ConvertToArray();

            // auswertung
            for (int counter = 0; counter < expected.Length; counter++)
                Assert.AreEqual(expected[counter], arr[counter]);
        }
        [TestMethod]
        public void ConvertToString()
        {
            byte[] source = new byte[] { 0x48, 0x61, 0x6c, 0x6c, 0x6f, 0x20, 0x57, 0x65, 0x6c, 0x74, 0x21 };
            string expected = "Hallo Welt!";
            string result = source.ConvertToString();
            for (int counter = 0; counter < expected.Length; counter++)
                Assert.AreEqual(expected[counter], result[counter]);
        }
    }
}
