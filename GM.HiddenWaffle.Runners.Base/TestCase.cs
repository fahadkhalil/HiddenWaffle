using System;
using System.Reflection;

namespace GM.HiddenWaffle.Runners.Base
{
    public class TestCase
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool MustPass { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Skip { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime ExecutionStartedOn { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime ExecutionCompletedOn { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TestCaseResults TestCaseResult { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Exception TestCaseError { get; set; }

        /// <summary>
        /// 
        /// </summary>
        internal MethodInfo methodInfo { get; set; }
    }
}