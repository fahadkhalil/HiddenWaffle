HiddenWaffle
============

What is HiddenWaffle
--------------
Hidden Waffle is a small open-source initiative. This is a set of classes which makes writing test-cases with Selenium an extremely pleasant experience. It contains extendable set of base classes containing rich functionality and a runner, which can be used to write any type of test execution scenario and environment.

It is an alternative to using NUnit framework.

Low cost solution for small-sized teams for automated testing.

Concepts:
--------------
A Test Case is a micro level test case and is represented by a C# class function. In case of success, return true and in case of logical error, return false. If function crashes, HiddenWaffle will mark the test failed. A test case is decorated with attribute GM.HiddenWaffle.Tests.Base.Decorations.GMTestCaseAttribute. A sample decorated test case:

```C#
  [GMTestCase(details: "1 - Confirm homep age is loaded properly", order: 1)]
  public bool LoadHomePage()
  {
    base.Driver.Manage().Window.Maximize();
    IWebElement inputfield = base.FindElement(By.Id("start"));

    if (inputfield == null)
        return false;
    return true;
  }
```

A Test Class is a collection of Test Cases. A test class is decorated with attribute GM.HiddenWaffle.Tests.Base.Decorations.GMTestClassAttribute.

```C#
  [GMTestClass(
  title: "Testing Punjab.GrayMath.com/HiddenWaffle-Test",
  Description = "",
  Skip = false,
  Url = "http://punjab.graymath.com/HiddenWaffle-Test/")]
```

All Test Classes inherit from GM.HiddenWaffle.Tests.Base.GMTestCase and are bound to implement few setup related functions.

```C#
  [GMTestClass(
  title: "Testing Punjab.GrayMath.com/HiddenWaffle-Test",
  Description = "",
  Skip = false,
  Url = "http://punjab.graymath.com/HiddenWaffle-Test/")]
  public class SampleTest
      : GMTestCase
  {
    public override string BaseUrl()
    {
      return "http://punjab.graymath.com/HiddenWaffle-Test/";
    }
    public override void Setup()
    {
    }
    public override void Cleanup()
    {
    }
 }
```

All test classes are compiled into an assembly. A set of sample test cases are available under Punjab.GM.Tests.

Jelly (GM.HiddenWaffle.Runners.Base.Jelly) is test case runner. Jelly takes the assembly DLL path as input and executes the tests and raises appropriate events.

Jelly requires a runner implementation. I have included a lame CLI based runner (GM.HiddenWaffle.LameRunner).

```C#
  Jelly j = new Jelly();

  // One of the many juicy events exposed by Jelly. You can monitor test case progress while listening to the events
  j.OnTestsLoaded += j_OnTestsLoaded;

  j.OnBeginExecution += j_OnBeginExecution;
  j.OnExecutionCompleted += j_OnExecutionCompleted;

  j.Prepare(@"D:\open-source\GM.HiddenWaffle\Punjab.GM.Tests\bin\Debug\Punjab.GM.Tests.dll", 1);
  j.Start();

  // Test results are available in collection called: j.TestClasses;

  Console.ReadLine();
```

Recommended Implementation
--------------

A practical implementation can be:

- Setup a private GIT repository & start composing test cases
- Setup a low-cost AWS EC2 Server & configure it to pull latest code from repository, compile it and execute test cases
- Upon completion, send out an email to interested team
- Once completed, shutdown the server to save bill

Need help with any of the above? Contact me at fahad_at_graymath.com

About Fahad Khalil
--------------
A Software Engineering Practitioner with over 10 years of experience in software engineering and training. His passions include – Preaching Agile, Software Development Best Practices, Coding Apps and Frameworks. He can talk numbers with clients and sit with engineers to debug and resolve technical issues at code level. 

License
--------------

<a rel="license" href="http://creativecommons.org/licenses/by/4.0/"><img alt="Creative Commons License" style="border-width:0" src="http://i.creativecommons.org/l/by/4.0/88x31.png" /></a><br /><span xmlns:dct="http://purl.org/dc/terms/" property="dct:title">Hiddle Waffle</span> by <a xmlns:cc="http://creativecommons.org/ns#" href="http://graymath.com" property="cc:attributionName" rel="cc:attributionURL">Fahad Khalil</a> is licensed under a <a rel="license" href="http://creativecommons.org/licenses/by/4.0/">Creative Commons Attribution 4.0 International License</a>.
