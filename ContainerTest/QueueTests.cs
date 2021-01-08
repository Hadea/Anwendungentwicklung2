using Microsoft.VisualStudio.TestTools.UnitTesting;
using Container;
using System;

namespace ContainerTest
{
    [TestClass]
    public class QueueTests
    {
        [TestMethod]
        public void Createable()
        {
            Assert.IsNotNull(new Queue());
        }
        
        [TestMethod]
        public void IsEmptyAfterNew()
        {
            Queue testQueue = new();
            Assert.IsTrue(testQueue.IsEmpty());
        }

        [TestMethod]
        public void IsEmptyAfterPushAndPop()
        {
            Queue testQueue = new();
            testQueue.Push(1);
            _ = testQueue.Pop();
            Assert.IsTrue(testQueue.IsEmpty());
        }
        [TestMethod]
        public void IsNotEmptyAfterPush()
        {
            Queue testQueue = new();
            testQueue.Push(1);
            Assert.IsFalse(testQueue.IsEmpty());
        }

        [TestMethod]
        public void IsNotEmptyAfterDoublePushAndPop()
        {
            Queue testQueue = new();
            testQueue.Push(1);
            testQueue.Push(1);
            testQueue.Pop();
            Assert.IsFalse(testQueue.IsEmpty());
        }
        [TestMethod]
        public void IsNotEmptyAfterMultiPushAndPop()
        {
            Queue testQueue = new();
            testQueue.Push(1);
            testQueue.Push(1);
            testQueue.Pop();
            Assert.IsFalse(testQueue.IsEmpty());
            testQueue.Pop();
            Assert.IsTrue(testQueue.IsEmpty());
            testQueue.Push(1);
            testQueue.Push(1);
            Assert.IsFalse(testQueue.IsEmpty());
        }

        [TestMethod]
        public void EqualAfterPushAndPop()
        {
            Queue testQueue = new();
            int testValue = 20;
            testQueue.Push(testValue);
            int returnValue = testQueue.Pop();
            Assert.IsTrue(testValue == returnValue);
        }
        [TestMethod]
        public void EqualAfterMultiPushAndPop()
        {
            Queue testQueue = new();
            int testValueA = 20;
            int testValueB = 5;
            int testValueC = 9000;
            testQueue.Push(testValueA);
            testQueue.Push(testValueB);
            testQueue.Push(testValueC);
            int returnValueA = testQueue.Pop();
            int returnValueB = testQueue.Pop();
            int returnValueC = testQueue.Pop();
            Assert.IsTrue(testValueA == returnValueA);
            Assert.IsTrue(testValueB == returnValueB);
            Assert.IsTrue(testValueC == returnValueC);
        }
        [TestMethod]
        public void EqualAfterLoopPushAndPop()
        {
            Queue testQueue = new();

            for (int i = 10; i <= 90000; i++)
            {
                testQueue.Push(i);
            }

            for (int i = 10; i <= 90000; i++)
            {
                Assert.IsTrue(testQueue.Pop() == i);
            }
        }
        [TestMethod]
        public void EqualAfterLoopPushAndPopWithParameter()
        {
            Queue testQueue = new(90000);

            for (int i = 10; i <= 90000; i++)
            {
                testQueue.Push(i);
            }

            for (int i = 10; i <= 90000; i++)
            {
                Assert.IsTrue(testQueue.Pop() == i);
            }
        }

        [TestMethod]
        public void PopOnEmpty()
        {
            Queue testQueue = new();
            int testValue = 20;
            testQueue.Push(testValue);
            testQueue.Push(testValue);
            testQueue.Pop();
            testQueue.Pop();
            Assert.ThrowsException<IndexOutOfRangeException>(() => testQueue.Pop());
        }

        [TestMethod]
        public void ContinuousPushAndPop()
        {
            Queue testQueue = new(5);
            bool testResult = true;
            for (int i = 0; i < 50; i++)
            {
                testQueue.Push(i);
                int result = testQueue.Pop();
                if (i != result)
                    testResult = false;
            }
            Assert.IsTrue(testResult);
        }
    }
}
