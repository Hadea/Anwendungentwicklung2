using Microsoft.VisualStudio.TestTools.UnitTesting;
using Container;

namespace ContainerTest
{
    [TestClass]
    public class StackTests
    {
        [TestMethod]
        public void IsEmptyAfterNew()
        {
            Stack testStack = new();
            Assert.IsTrue(testStack.IsEmpty());
        }

        [TestMethod]
        public void IsEmptyAfterPushAndPop()
        {
            Stack testStack = new();
            testStack.Push(1);
            _ = testStack.Pop();
            Assert.IsTrue(testStack.IsEmpty());
        }
        [TestMethod]
        public void IsNotEmptyAfterPush()
        {
            Stack testStack = new();
            testStack.Push(1);
            Assert.IsFalse(testStack.IsEmpty());
        }

        [TestMethod]
        public void IsNotEmptyAfterDoublePushAndPop()
        {
            Stack testStack = new();
            testStack.Push(1);
            testStack.Push(1);
            testStack.Pop();
            Assert.IsFalse(testStack.IsEmpty());
        }
        [TestMethod]
        public void IsNotEmptyAfterMultiPushAndPop()
        {
            Stack testStack = new();
            testStack.Push(1);
            testStack.Push(1);
            testStack.Pop();
            Assert.IsFalse(testStack.IsEmpty());
            testStack.Pop();
            Assert.IsTrue(testStack.IsEmpty());
            testStack.Push(1);
            testStack.Push(1);
            Assert.IsFalse(testStack.IsEmpty());
        }

        [TestMethod]
        public void EqualAfterPushAndPop()
        {
            Stack testStack = new();
            int testValue = 20;
            testStack.Push(testValue);
            int returnValue = testStack.Pop();
            Assert.IsTrue(testValue == returnValue);
        }
        [TestMethod]
        public void EqualAfterMultiPushAndPop()
        {
            Stack testStack = new();
            int testValueA = 20;
            int testValueB = 5;
            int testValueC = 9000;
            testStack.Push(testValueA);
            testStack.Push(testValueB);
            testStack.Push(testValueC);
            int returnValueC = testStack.Pop();
            int returnValueB = testStack.Pop();
            int returnValueA = testStack.Pop();
            Assert.IsTrue(testValueA == returnValueA);
            Assert.IsTrue(testValueB == returnValueB);
            Assert.IsTrue(testValueC == returnValueC);
        }
        [TestMethod]
        public void EqualAfterLoopPushAndPop()
        {
            Stack testStack = new();

            for (int i = 10; i <= 90000; i++)
            {
                testStack.Push(i);
            }

            for (int i = 90000; i >= 10 ; --i)
            {
                Assert.IsTrue(testStack.Pop() == i);
            }
        }
        [TestMethod]
        public void EqualAfterLoopPushAndPopWithParameter()
        {
            Stack testStack = new(90000);

            for (int i = 10; i <= 90000; i++)
            {
                testStack.Push(i);
            }

            for (int i = 90000; i >= 10 ; --i)
            {
                Assert.IsTrue(testStack.Pop() == i);
            }
        }
    }
}
