using System;
using System.Linq;

namespace GM.HiddenWaffle.Tests.Base.Decorations
{
    [AttributeUsage(System.AttributeTargets.Method,
                       AllowMultiple = false)]
    public class GMTestCaseAttribute : Attribute
    {
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
        public TimeSpan MaxExecutionDuration { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="details"></param>
        /// <param name="order"></param>
        /// <param name="skip"></param>
        /// <param name="mustPass"></param>
        public GMTestCaseAttribute(string details, int order, bool skip = false, bool mustPass = false)
        {
            this.Details = details;
            this.Order = order;
            this.MustPass = mustPass;
            this.Skip = skip;
        }
    }
}
