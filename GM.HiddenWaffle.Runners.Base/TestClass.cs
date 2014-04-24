using System;
using System.Collections.Generic;

namespace GM.HiddenWaffle.Runners.Base
{
    public class TestClass
    {
        /// <summary>
        /// 
        /// </summary>
        public TestClass()
        {
            this.TestCases = new List<TestCase>();
        }

        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SpotifyUri { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Skip { get; set; }

        /// <summary>
        /// 
        /// </summary>
        internal Type ClassType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<TestCase> TestCases { get; set; }
    }
}