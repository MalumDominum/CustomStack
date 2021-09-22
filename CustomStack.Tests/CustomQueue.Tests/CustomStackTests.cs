using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;


namespace CustomStack.Tests
{   
    [TestFixture]
    public class CustomStackTests
    {
        [Test]
        public void Pop_AddNotifyHandlerAndPop_EventTriggered()
        {
            var stack = new CustomStack<int>(new List<int> { 2, 0 });
            const int expectedPopResult = 0;
            const int expectedCount = 1;
            var actualCount = 0;

            stack.Notify += (sender, args) =>
            {
                if (sender is CustomStack<int> && args.Message != null)
                    actualCount++;
            };
            var actualPopResult = stack.Pop();

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedPopResult, actualPopResult,
                        "Pop returns last element");

                Assert.AreEqual(expectedCount, actualCount, "Event triggered");
            });
        }

        [Test]
        public void Pop_AddNotifyHandlerAndPopLastElement_EventTriggeredTwice()
        {
            var stack = new CustomStack<int>(new List<int> { 0 });
            const int expectedCount = 2;
            var actualCount = 0;

            stack.Notify += (sender, args) =>
            {
                if (sender is CustomStack<int> && args.Message != null)
                    actualCount++;
            };
            stack.Pop();
            
            Assert.AreEqual(expectedCount, actualCount, "Event triggered twice (when element poped and when stack is empty)");
        }

        [Test]
        public void GetEnumerator_SumStackElements235_Result10()
        {
            var stack = new CustomStack<int>(new List<int> { 2, 3, 5 });
            const int expectedResult = 10;

            var actualResult = stack.Sum();

            Assert.AreEqual(expectedResult, actualResult,
                "GetEnumerator work correctly");
        }

        [Test]
        public void Clear_ClearNotEmptyStack_StackIsEmpty()
        {
            var stack = new CustomStack<int>(new List<int> { 0, 1, 2, 3, 4, 5 });
            var expectedStack = new CustomStack<int>(new List<int>());

            stack.Clear();

            Assert.AreEqual(stack, expectedStack,
                "Stack is empted by Clear method");
        }

        [TestCase(new int[0])]
        [TestCase(new [] {2, 5, 8})]
        public void Contains_CheckStackForElement3_False(int[] initial)
        {
            var stack = new CustomStack<int>(initial.ToArray());
            const bool expectedResult = false;

            var actualResult = stack.Contains(3);

            Assert.AreEqual(expectedResult, actualResult,
                "Stack does not contain element 3");
        }

        [Test]
        public void Contains_CheckStack258ForElement2_True()
        {
            var stack = new CustomStack<int>(new List<int> { 2, 5, 8 });
            const bool expectedResult = true;

            var actualResult = stack.Contains(2);

            Assert.AreEqual(expectedResult, actualResult,
                "Stack {2, 5, 8} contains element 2");
        }

        [TestCase(new int[0])]
        [TestCase(new[] { 2, 5, 8 })]
        public void ToArray_CastStackToArray_SameResultWithDefaultStackToArray(int[] initial)
        {
            var stack = new CustomStack<int>(initial);
            var expectedArray = new Stack<int>(initial).ToArray();

            var array = stack.ToArray();

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedArray, array,
                    "Custom stack and default stack have same logic and results");
            });
        }

        [Test]
        public void Pop_PopFromEmptyStack_ThrowInvalidOperationException()
        {
            var stack = new CustomStack<int>(new List<int>());
            var expectedException = new InvalidOperationException("The stack is empty");

            var actualException = Assert.Catch(() => stack.Pop());

            Assert.AreEqual(expectedException.GetType(), actualException?.GetType(),
                "Got invalid operation exception, because trying Pop empty stack");
        }

        [Test]
        public void StackConstructor_PassIntoConstructorNull_ArgumentNullException()
        {
            var expectedException = new ArgumentNullException();

            var actualException = Assert.Catch(() => new CustomStack<int>(null));

            Assert.AreEqual(expectedException.GetType(), actualException?.GetType(),
                "Got invalid operation exception, because trying Pop empty stack");
        }
    }
}