using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using GM.HiddenWaffle.Runners.Base.Events;
using GM.HiddenWaffle.Tests.Base;
using GM.HiddenWaffle.Tests.Base.Decorations;

namespace GM.HiddenWaffle.Runners.Base
{
    public class Jelly
    {
        private string AssemblyPath { get; set; }
        private uint NumberOfThreads { get; set; }
        private List<TestClass> testClasses = null;

        #region "Events for outside world"

        public event TestsLoadedEvent OnTestsLoaded = null;

        public event BeginExecutionEvent OnBeginExecution = null;
        public event ExecutionCompletedEvent OnExecutionCompleted = null;

        public event BeginTestClassExecutionEvent OnBeginTestClassExecution = null;
        public event TestClassExecutionCompletedEvent OnTestClassExecutionCompleted = null;

        public event BeginTestCaseExecutionEvent OnBeginTestCaseExecution = null;
        public event TestCaseExecutionCompletedEvent OnTestCaseExecutionCompleted = null;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public List<TestClass> TestClasses
        {
            get
            {
                return testClasses;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblyPath"></param>
        /// <param name="numberOfThreads"></param>
        public Jelly()
        {
        }

        public void Prepare(string assemblyPath, uint numberOfThreads = 1)
        {
            this.AssemblyPath = assemblyPath;
            this.NumberOfThreads = numberOfThreads;
            this.testClasses = new List<TestClass>();

            //1: Load the assembly
            Assembly assembly = Assembly.LoadFrom(assemblyPath);

            //2: Find all classes
            var assemblyTypes = assembly.GetTypes();
            foreach (Type t in assemblyTypes)
            {
                //1: see if it is a test-case
                object[] testClassAttribute = t.GetCustomAttributes(typeof(GMTestClassAttribute), false);
                if (testClassAttribute != null && testClassAttribute.Count() != 0)
                {
                    GMTestClassAttribute obj = testClassAttribute[0] as GMTestClassAttribute;
                    TestClass _testClass = null;
                    _testClass = new TestClass
                    {
                        Id = t.ToString(),
                        Skip = obj.Skip,
                        ClassType = t,
                        Description = obj.Description,
                        Title = obj.Title,
                        Url = obj.Url
                    };

                    //2: Find methods in this class
                    var testCaseMethods = t.GetMethods();
                    foreach (MethodInfo m in testCaseMethods)
                    {
                        object[] testCaseAttribute = m.GetCustomAttributes(typeof(GMTestCaseAttribute), false);
                        if (testCaseAttribute != null && testCaseAttribute.Count() != 0)
                        {
                            GMTestCaseAttribute objcaseattr = testCaseAttribute[0] as GMTestCaseAttribute;

                            _testClass.TestCases.Add(
                                new TestCase
                                {
                                    Skip = objcaseattr.Skip,
                                    Details = objcaseattr.Details,
                                    MustPass = objcaseattr.MustPass,
                                    Order = objcaseattr.Order,
                                    TestCaseResult = TestCaseResults.Pending,
                                    Id = _testClass.Id + ":" + m.Name,
                                    methodInfo = m
                                });
                        }
                    }

                    this.testClasses.Add(_testClass);
                }
            }

            //3: raise the event that we have data
            if (this.OnTestsLoaded != null)
                this.OnTestsLoaded(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tc"></param>
        public void Start()
        {
            if (this.OnBeginExecution != null)
                this.OnBeginExecution(this);

            Task[] tasks = new Task[NumberOfThreads];

            // All the crap
            int taskIndex = 0;
            bool firstIteration = true;
            foreach (TestClass tc in this.testClasses)
            {
                if (!tc.Skip)
                {
                    if (taskIndex % NumberOfThreads == 0 && !firstIteration)
                    {
                        taskIndex = Task.WaitAny(tasks);
                    }

                    tasks[taskIndex++] = Task.Factory.StartNew(() => { this.ExecuteClass(tc); });

                    if (taskIndex > NumberOfThreads - 1)
                    {
                        taskIndex = taskIndex % (int)NumberOfThreads;
                        firstIteration = false;
                    }


                }
            }

            for (int i = 0; i < tasks.Count(); ++i)
            {
                if (tasks[i] == null)
                    tasks[i] = Task.Factory.StartNew(() => { });
            }

            //
            Task.WaitAll(tasks);

            if (this.OnExecutionCompleted != null)
                this.OnExecutionCompleted(this);
        }

        private void ExecuteClass(TestClass tc)
        {
            if (this.OnBeginTestClassExecution != null)
                this.OnBeginTestClassExecution(this, tc);

            //
            if (!tc.Skip)
            {
                List<TestCase> methods = (from x in tc.TestCases where !x.Skip orderby x.Order ascending select x).ToList();

                //
                GMTestCase instance = (GMTestCase)Activator.CreateInstance(tc.ClassType);
                instance.Start();

                foreach (TestCase method in methods)
                {
                    TestCaseResults tcr = this.ExecuteCase(instance, tc, method);
                    if (tcr != TestCaseResults.Success)
                        break;
                }

                instance.End();
                //
            }

            if (this.OnTestClassExecutionCompleted != null)
                this.OnTestClassExecutionCompleted(this, tc);
        }

        private TestCaseResults ExecuteCase(GMTestCase testClassObject, TestClass tc, TestCase testCase)
        {
            testCase.ExecutionStartedOn = System.DateTime.UtcNow;
            testCase.TestCaseResult = TestCaseResults.InProgress;

            if (this.OnBeginTestCaseExecution != null)
                this.OnBeginTestCaseExecution(this, tc, testCase);

            //
            try
            {
                //

                bool result = (bool)testCase.methodInfo.Invoke(testClassObject, null);

                //

                if (result)
                    testCase.TestCaseResult = TestCaseResults.Success;
                else
                    testCase.TestCaseResult = TestCaseResults.Fail;
            }
            catch (TargetInvocationException ex)
            {
                testCase.TestCaseResult = TestCaseResults.Error;
                testCase.TestCaseError = ex;
            }
            catch (Exception ex)
            {
                testCase.TestCaseResult = TestCaseResults.Error;
                testCase.TestCaseError = ex;
            }
            finally
            {
                testCase.ExecutionCompletedOn = System.DateTime.UtcNow;
            }
            //

            if (this.OnBeginTestCaseExecution != null)
                this.OnBeginTestCaseExecution(this, tc, testCase);

            return testCase.TestCaseResult;
        }
    }
}
