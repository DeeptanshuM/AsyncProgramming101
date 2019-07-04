using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AsyncProgrammingDemo
{
    class Program
    {
        public static void Main(string[] args)
        {
            var p = new Program();

            p.demo();
            
            Console.ReadLine();
        }

        private void demo()
        {
            Console.WriteLine("##### Testing synchronous code #####");
            demoSyncFunction(syncMultiplyBy10AndAdd);
            Console.WriteLine("####################################" + Environment.NewLine);

            Console.WriteLine("##### Testing await immediately #####");
            demoAsyncFunction(asyncMultiplyBy10AndAddAwaitImmediately);
            Console.WriteLine("####################################" + Environment.NewLine);
            Console.WriteLine();

            Console.WriteLine("##### Testing await at end #####");
            demoAsyncFunction(asyncMultiplyBy10AndAddAwaitAtEnd);
            Console.WriteLine("####################################" + Environment.NewLine);
            Console.WriteLine();
        }

        private void demoSyncFunction(Func<int, int, int> asyncDemoFunction)
        {
            int num1 = 15;
            int num2 = 33;
            int result;
            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();
            result = asyncDemoFunction(num1, num2);
            stopWatch.Stop();
            displayRuntime(stopWatch.Elapsed);

        }

        private void demoAsyncFunction(Func<int, int, Task<int>> asyncDemoFunction)
        {
            int num1 = 15;
            int num2 = 33;
            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();

            int result = asyncDemoFunction(num1, num2).Result;

            stopWatch.Stop();

            displayRuntime(stopWatch.Elapsed);
        }

        private void displayRuntime(TimeSpan ts)
        {
            string elapsedTime = String.Format("{0:00}min:{1:00}s.{2:00}ms",
                ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);

            Console.WriteLine("Runtime: " + elapsedTime);
        }

        private int syncMultiplyBy10AndAdd(int num1, int num2)
        {
            Console.WriteLine("Calling long running Op from Thread " + System.Threading.Thread.CurrentThread.ManagedThreadId);
            int a = longRunningMultiplyBy10(num1).Result;

            Console.WriteLine("Calling long running Op from Thread " + System.Threading.Thread.CurrentThread.ManagedThreadId);
            int b = longRunningMultiplyBy10(num2).Result;

            Console.WriteLine("Executing return from Thread " + System.Threading.Thread.CurrentThread.ManagedThreadId);
            return a + b;
        }

        private async Task<int> asyncMultiplyBy10AndAddAwaitImmediately(int num1, int num2)
        {
            Console.WriteLine("Calling long running Op from Thread " + System.Threading.Thread.CurrentThread.ManagedThreadId);
            int a = await longRunningMultiplyBy10(num1);

            Console.WriteLine("Calling long running Op from Thread " + System.Threading.Thread.CurrentThread.ManagedThreadId);
            int b = await longRunningMultiplyBy10(num2);

            Console.WriteLine("Executing return from Thread " + System.Threading.Thread.CurrentThread.ManagedThreadId);
            return a + b;
        }

        private async Task<int> asyncMultiplyBy10AndAddAwaitAtEnd(int num1, int num2)
        {
            Console.WriteLine("Calling long running Op from Thread " + System.Threading.Thread.CurrentThread.ManagedThreadId);
            Task<int> a = longRunningMultiplyBy10(num1);

            Console.WriteLine("Calling long running Op from Thread " + System.Threading.Thread.CurrentThread.ManagedThreadId);
            Task<int> b = longRunningMultiplyBy10(num2);

            Console.WriteLine("Executing return from Thread " + System.Threading.Thread.CurrentThread.ManagedThreadId);
            return await a + await b;
        }

        private async Task<int> longRunningMultiplyBy10(int number)
        {
            Console.WriteLine("# Initiated long running op on for the input number=" + number + " from Thread " + System.Threading.Thread.CurrentThread.ManagedThreadId + " #");

            //100 delays of 100 ms = 10 seconds
            for (int i = 0; i < 100 ; i++)
            {
                await Task.Delay(100);
            }

            Console.WriteLine("# Completed long running op on for the input number=" + number + " from Thread " + System.Threading.Thread.CurrentThread.ManagedThreadId + " #");
            return number * 10;
        }
    }
}