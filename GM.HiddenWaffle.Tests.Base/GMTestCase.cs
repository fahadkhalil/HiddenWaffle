//#define F3DEBUG

using System;
using System.Collections.ObjectModel;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace GM.HiddenWaffle.Tests.Base
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class GMTestCase
    {
        IWebDriver driver = null;
        INavigation navigation = null;
        
        /// <summary>
        /// 
        /// </summary>
        protected IWebDriver Driver
        {
            get
            {
                return driver;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected INavigation Navigation
        {
            get
            {
                return navigation;
            }
        }

        #region "Abstract methods"

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract string BaseUrl();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract void Setup();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract void Cleanup();

        #endregion

        #region "Utility Methods"

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void Start()
        {
            // Start the engine
            this.driver = new FirefoxDriver();
            this.navigation = driver.Navigate();

            // Lets see what our master has in mind?
            this.Setup();

            // Go the URL dude
            this.Navigation.GoToUrl(this.BaseUrl());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void End()
        {
            this.Cleanup();

            // Destroy the driver
            try
            {
                if (driver != null)
                {
                    driver.Close();
                    driver.Quit();
                    driver.Dispose();
                }
            }
            catch (Exception ex)
            {
#if F3DEBUG
                throw ex;
#endif
            }
        }

        #endregion

        #region "Helper Methods"

        protected ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            return this.driver.FindElements(by);
        }

        protected IWebElement FindElement(By by)
        {
            return this.driver.FindElement(by);
        }

        protected bool Click(By by)
        {
            var r = this.FindElements(by);
            if (r.Count() == 0)
                return false;
            if (r.Count() != 1)
                return false;

            r[0].Click();

            return true;
        }

        protected bool CheckImageLoaded(String xpath)
        {
            Int64 naturalWidth = 0;
            Int64 naturalHeight = 0;

            Int64.TryParse(this.GetAttribute(xpath, "naturalHeight"), out naturalWidth);
            Int64.TryParse(this.GetAttribute(xpath, "naturalWidth"), out naturalHeight);

            if (naturalWidth > 0 && naturalHeight > 0)
                return true;

            return false;
        }

        protected String GetAttribute(String xpath, String attribute)
        {
            String js = "window.document.evaluate('" + xpath + "', " +
                "window.document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue[" +
                "'" + attribute + "'];";

            return this.ExecuteJS(js);
        }

        protected String ExecuteJS(string js)
        {
            return ((IJavaScriptExecutor)this.driver).ExecuteScript(js).ToString();
        }

        protected IWebElement AjaxWaitUntilElementExists(By by)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(this.driver, new TimeSpan(0, 3, 0));
                return wait.Until(ExpectedConditions.ElementExists(by));
            }
            catch (Exception)
            {
                return null;
            }
        }

        protected IWebElement AjaxWaitUntilElementAppears(By by)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(this.driver, new TimeSpan(0, 3, 0));
                return wait.Until(ExpectedConditions.ElementIsVisible(by));
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion
    }
}
