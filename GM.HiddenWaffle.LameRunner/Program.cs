using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GM.HiddenWaffle.Runners.Base;

namespace GM.HiddenWaffle.LameRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            Jelly j = new Jelly();

            j.OnTestsLoaded += j_OnTestsLoaded;

            j.OnBeginExecution += j_OnBeginExecution;
            j.OnExecutionCompleted += j_OnExecutionCompleted;

            j.Prepare(@"D:\open-source\GM.HiddenWaffle\Punjab.GM.Tests\bin\Debug\Punjab.GM.Tests.dll", 1);
            j.Start();

            Console.ReadLine();
        }

        static void j_OnExecutionCompleted(Jelly obj)
        {
            Console.WriteLine("++ Execution completed, publishing results.");

            foreach (var cl in obj.TestClasses)
            {
                Console.WriteLine(">> Test : " + cl.Title);

                foreach (var cs in cl.TestCases.OrderBy(m => m.Order))
                {
                    Console.Write(">> " + cs.Order + ":[" + cs.Details + "] ");

                    if (cs.TestCaseResult == TestCaseResults.Success)
                        Console.ForegroundColor = ConsoleColor.Green;

                    if (cs.TestCaseResult != TestCaseResults.Success)
                        Console.ForegroundColor = ConsoleColor.Red;

                    Console.WriteLine(cs.TestCaseResult);

                    Console.ResetColor();
                }

                Console.WriteLine("\n\n =========================================== \n\n");
            }
        }

        static void j_OnBeginExecution(Jelly obj)
        {
            Console.WriteLine("++ Starting execution.");
        }

        static void j_OnTestsLoaded(Jelly obj)
        {
            Console.WriteLine("++ Test cases loaded.");
        }
    }
}
