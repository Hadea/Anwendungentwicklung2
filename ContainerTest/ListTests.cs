using Microsoft.VisualStudio.TestTools.UnitTesting;
using Container;
using System;

namespace ContainerTest
{
    [TestClass]
    public class ListTests
    {
        [TestMethod]
        public void Createable()
        {
            Assert.IsNotNull(new List());
        }

        [TestMethod]
        public void IsEmptyAfterNew()
        {
            List testList = new();
            Assert.IsTrue(testList.IsEmpty());
        }

        [TestMethod]
        public void IsEmptyAfterAddAndRemove()
        {
            List testList = new();
            testList.Add(1);
            testList.Remove(0);
            Assert.IsTrue(testList.Count == 0);
        }
        [TestMethod]
        public void IsNotEmptyAfterAdd()
        {
            List testList = new();
            testList.Add(1);
            Assert.IsTrue(testList.Count == 1);
        }

        [TestMethod]
        public void IsNotEmptyAfterDoubleAddAndRemove()
        {
            // testet ob nach mehrfachen einfügen und einigen entfernen das der füllstandszähler korrekt arbeitet
            List testList = new();
            testList.Add(1);
            testList.Add(1);
            testList.Add(1);
            testList.Remove(0);
            testList.Remove(0);
            Assert.IsTrue(testList.Count == 1);
        }
        [TestMethod]
        public void IsNotEmptyAfterMultiAddAndRemove()
        {
            // testet count ob es nach mehreren add und remove jederzeit korrekt arbeitet
            List testList = new();
            testList.Add(1);
            testList.Add(1);
            testList.Remove(0);
            Assert.IsTrue(testList.Count == 1);
            testList.Remove(0);
            Assert.IsTrue(testList.Count == 0);
            testList.Add(1);
            testList.Add(1);
            Assert.IsTrue(testList.Count == 2);
        }

        [TestMethod]
        public void EqualAfterAddAndRemove()
        {
            // testet ob das eingefügte element auch wieder ausgelesen werden kann
            List testList = new();
            int testValue = 20;
            testList.Add(testValue);
            int returnValue = testList.At(0);
            Assert.IsTrue(testValue == returnValue);
        }
        [TestMethod]
        public void EqualAfterMultiAddAndRemove()
        {
            // testet das mehrfache einfügen und auslesen und überprüft die werte
            List testList = new();
            int testValueA = 20;
            int testValueB = 5;
            int testValueC = 9000;
            testList.Add(testValueA);
            testList.Add(testValueB);
            testList.Add(testValueC);
            int returnValueA = testList.At(0);
            int returnValueB = testList.At(1);
            int returnValueC = testList.At(2);
            Assert.IsTrue(testValueA == returnValueA);
            Assert.IsTrue(testValueB == returnValueB);
            Assert.IsTrue(testValueC == returnValueC);
        }
        [TestMethod]
        public void EqualAfterLoopAddAndRemove()
        {
            // testet das vergrössern der Liste und das alle elemente auch korrekt gespeichert wurden
            List testList = new();

            for (int i = 10; i <= 90000; i++)
            {
                testList.Add(i);
            }

            for (int counter = 10; counter <= 90000; counter++)
            {
                Assert.IsTrue(testList.At(counter-10) == counter);
            }

            // Pos :  00 01 02 03 04 05 ..
            // Wert:  10 11 12 13 14 15 ..

        }
        [TestMethod]
        public void EqualAfterLoopAddAndPopWithParameter()
        {
            // testet ob der überladene construktor funkitoniert
            List testList = new(90000);

            Assert.IsTrue(testList.Capacity == 90000);
            for (int i = 10; i <= 90000; i++)
            {
                testList.Add(i);
            }

            for (int i = 10; i <= 90000; i++)
            {
                Assert.IsTrue(testList.At(i-10) == i);
            }
        }

        [TestMethod]
        public void RemoveOnEmpty()
        {
            // testet ob das entfernen aus einer bereits leeren liste einen fehler erzeugt
            List testList = new();
            int testValue = 20;
            testList.Add(testValue);
            testList.Add(testValue);
            testList.Remove(0);
            testList.Remove(0);
            Assert.ThrowsException<IndexOutOfRangeException>(() => testList.Remove(0));
        }

        [TestMethod]
        public void ContinuousAddAndPopOnMaxCapacity()
        {
            // testet ob nach mehrfachen einfügen und löschen das vergrössern gestartet wird und die werte trotzdem
            // korrekt hintereinander stehen
            List testList = new(4);
            testList.Add(1);
            testList.Add(2);
            testList.Add(3);
            testList.Add(4);
            testList.Remove(1);
            testList.Remove(0);
            Assert.IsTrue(testList.At(0) == 3);
            Assert.IsTrue(testList.At(1) == 4);
            testList.Add(5);
            testList.Add(6);
            Assert.IsTrue(testList.Remove(0) == 3);
            testList.Add(7);
            testList.Add(8);
            Assert.IsTrue(testList.Remove(0) == 4);
        }

        [TestMethod]
        public void ContinuousAddTiming()
        {
            List testList = new(5);
            DateTime testStart = DateTime.Now;
            for (int counter = 0; counter < 100000; counter++)
                testList.Add(counter);
            for (int counter = 0; counter < 100000; counter++)
                testList.Remove(0);
            DateTime testEnd = DateTime.Now;
            Assert.IsTrue((testEnd - testStart).TotalMilliseconds < 100);
        }

        [TestMethod]
        public void ZBonusResizeAutomated()
        {
            List testList = new(5);
            for (int counter = 0; counter < 20; counter++)
                testList.Add(counter);
            for (int counter = 0; counter < 10; counter++)
                Assert.IsTrue(testList.Remove(0) == counter);
            for (int counter = 20; counter < 40; counter++)
                testList.Add(counter);
            for (int counter = 10; counter < 30; counter++)
                Assert.IsTrue(testList.Remove(0) == counter);
            Assert.IsTrue(testList.Capacity <= 20);
        }

        [TestMethod]
        public void ZBonusResizeManualMiddle()
        {
            List testList = new(10);
            for (int counter = 0; counter < 8; counter++)
                testList.Add(counter);
            for (int counter = 0; counter < 4; counter++)
                Assert.IsTrue(testList.Remove(0) == counter);
            testList.Capacity = 5;
            for (int counter = 4; counter < 8; counter++)
                Assert.IsTrue(testList.Remove(0) == counter);
        }

        [TestMethod]
        public void ZBonusResizeManualEnd()
        {
            List testList = new(10);
            for (int counter = 0; counter < 10; counter++)
                testList.Add(counter);
            for (int counter = 0; counter < 8; counter++)
                Assert.IsTrue(testList.Remove(0) == counter);
            for (int counter = 10; counter < 12; counter++)
                testList.Add(counter);
            testList.Capacity = 5;
            for (int counter = 8; counter < 12; counter++)
                Assert.IsTrue(testList.Remove(0) == counter);
        }

        [TestMethod]
        public void ZBonusResizeTooSmall()
        {
            List testList = new(10);
            for (int counter = 0; counter < 8; counter++)
                testList.Add(counter);
            Assert.ThrowsException<InsufficientMemoryException>(() => testList.Capacity = 5);
        }

        [TestMethod]
        public void ZBonusCapacityOutOfRange()
        {
            List testList = new();
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => testList.Capacity = -1);
        }

        [TestMethod]
        public void ZBonusCapacityNeverBelowInitialization()
        {
            List testList = new(10);
            for (int counter = 0; counter < 30; counter++)
                testList.Add(counter);
            for (int counter = 0; counter < 29; counter++)
                testList.Remove(0);
            Assert.IsTrue(testList.Capacity == 10);
        }

        [TestMethod]
        public void ZBonusCapacityMinimumOnUserDefinition()
        {
            List testList = new(10);
            for (int counter = 0; counter < 30; counter++)
                testList.Add(counter);
            for (int counter = 0; counter < 29; counter++)
                testList.Remove(0);
            testList.Capacity = 5;
            for (int counter = 0; counter < 29; counter++)
                testList.Add(counter);
            for (int counter = 0; counter < 29; counter++)
                testList.Remove(0);
            Assert.IsTrue(testList.Capacity == 5);
        }
        [TestMethod]
        public void ZBonusDirectResize()
        {
            List testList = new(15);
            testList.Capacity = 7;
            Assert.IsTrue(testList.Capacity == 7);
        }

        [TestMethod]
        public void ZBonusForEachDelegate()
        {
            List testList = new(10);
            testList.Add(1);
            testList.Add(2);
            testList.Add(3);
            testList.Add(4);
            int Q = 0;
            testList.ForEach((i) => Q += i);
            Assert.IsTrue(Q == 10);
        }
    }
}
