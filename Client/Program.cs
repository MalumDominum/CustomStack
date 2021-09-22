using System;
using System.Collections.Generic;
using CustomStack;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var customStack = new CustomStack<int>(new List<int> {3});
            customStack.Notify += DisplayMessage<int>;
            customStack.Push(10, 302, 8, 10);
            customStack.Push(10);
            Console.WriteLine(customStack.Peek() + "\n");
            Console.WriteLine(customStack.Pop() + "\n");

            var arrayFromCustomStack = customStack.ToArray();
            Console.WriteLine("\nArray from stack type: " + arrayFromCustomStack);
            foreach (var element in arrayFromCustomStack)
                Console.WriteLine(element);

            Console.WriteLine("\nElements in array:");
            foreach (var element in customStack)
                Console.WriteLine(element);

            Console.WriteLine("\nIs stack contains 10: " + customStack.Contains(10));

            Console.WriteLine("\nElements from cleared stack:");
            customStack.Clear();
            foreach (var element in customStack)
                Console.WriteLine(element);

            Console.WriteLine("\nNew stack with new Hash Code:");
            var customStack2 = new CustomStack<int>();
            customStack2.Notify += DisplayMessage<int>;
            customStack2.Push(10);
            customStack2.Pop();
        }
        private static void DisplayMessage<T>(object sender, StackEventArgs e)
        {
            var hashCode = "";
            if (sender is CustomStack<T> stack)
                hashCode += "\tStack hash code: " + stack.GetHashCode();
            Console.WriteLine(e.Message + hashCode);
        }
    }
}
