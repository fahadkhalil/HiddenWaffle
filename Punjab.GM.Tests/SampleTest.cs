using System;
using System.Collections.ObjectModel;
using System.Linq;
using GM.HiddenWaffle.Tests.Base;
using GM.HiddenWaffle.Tests.Base.Decorations;
using OpenQA.Selenium;

namespace Punjab.GM.Tests
{
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

        //
        [GMTestCase(details: "1 - Confirm homep age is loaded properly", order: 1)]
        public bool LoadHomePage()
        {
            base.Driver.Manage().Window.Maximize();
            IWebElement inputfield = base.FindElement(By.Id("start"));

            if (inputfield == null)
                return false;

            return true;
        }

        [GMTestCase(details: "2 - Enter 2 as start value", order: 2)]
        public bool Screen1()
        {
            base.Driver.Manage().Window.Maximize();
            IWebElement inputfield = base.FindElement(By.Id("start"));

            inputfield.SendKeys("2");

            IWebElement submitbutton = base.FindElement(By.Id("submit"));
            submitbutton.Submit();

            return true;
        }

        [GMTestCase(details: "3 - Enter 10 as start value", order: 3)]
        public bool Screen2()
        {
            IWebElement inputfield = base.FindElement(By.Id("end"));
            if (inputfield == null)
                return false;

            inputfield.SendKeys("10");

            IWebElement submitbutton = base.FindElement(By.Id("submit"));
            submitbutton.Submit();

            return true;
        }

        [GMTestCase(details: "4 - Check results - confirm we have 8 li tags", order: 3)]
        public bool CheckResults()
        {
            IWebElement resultul = base.FindElement(By.Id("result"));
            if (resultul == null)
                return false;

            ReadOnlyCollection<IWebElement> li = resultul.FindElements(By.TagName("li"));
            if (li.Count == 8)
                return true;

            return false;
        }
    }
}
