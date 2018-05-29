using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace AsyncTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");


            Task.Run(async () =>
            {
                await Run();
            }).GetAwaiter().GetResult();

            Console.ReadLine();
        }

        static async Task Run()
        {
            {
                // Standard Await
                Console.WriteLine("Standard Await");

                var stopwatch = new Stopwatch();

                using (var client = new HttpClient())
                {
                    stopwatch.Start();
                    var result = await client.GetAsync("http://slowwly.robertomurray.co.uk/delay/10/url/http://www.google.co.uk");
                    stopwatch.Stop();

                    Console.WriteLine(stopwatch.ElapsedMilliseconds);
                }
            }

            Console.WriteLine();

            {
                // Using Task.Result
                Console.WriteLine("Task.Result");

                var stopwatch = new Stopwatch();

                using (var client = new HttpClient())
                {
                    stopwatch.Start();
                    var result = client.GetAsync("http://slowwly.robertomurray.co.uk/delay/10/url/http://www.google.co.uk").Result;
                    stopwatch.Stop();

                    Console.WriteLine(stopwatch.ElapsedMilliseconds);
                }
            }

            Console.WriteLine();

            {
                //Using Task.Run({})   
                Console.WriteLine("Task.Run({}) ");

                var stopwatch = new Stopwatch();
                stopwatch.Start();

                Console.WriteLine($"BEFORE TASK {stopwatch.ElapsedMilliseconds}");

                Task.Run(async () =>
                {
                    using (var client = new HttpClient())
                    {
                        var result = await client.GetAsync("http://slowwly.robertomurray.co.uk/delay/10/url/http://www.google.co.uk");

                        Console.WriteLine($"DURING TASK {stopwatch.ElapsedMilliseconds}");
                    }
                }).GetAwaiter().GetResult();

                stopwatch.Stop();

                Console.WriteLine($"AFTER TASK {stopwatch.ElapsedMilliseconds}");
            }

            Console.WriteLine();

            {
                //Using TaskFactory
                Console.WriteLine("Standard Await");

                var stopwatch = new Stopwatch();
                stopwatch.Start();

                Console.WriteLine($"Thread 1: {stopwatch.ElapsedMilliseconds}");

                await Task.Factory.StartNew(async () =>
                {
                    using (var client = new HttpClient())
                    {
                        var result = await client.GetAsync("http://slowwly.robertomurray.co.uk/delay/10/url/http://www.google.co.uk");

                        stopwatch.Stop();

                        Console.WriteLine($"Thread 2: {stopwatch.ElapsedMilliseconds}");
                    }
                }).ContinueWith((result) =>
                {
                    Console.WriteLine($"Thread 1: {stopwatch.ElapsedMilliseconds}");
                });
            }
        }
    }
}
