using System;
using System.Linq;

namespace GM.HiddenWaffle.Tests.Base.Decorations
{
    [AttributeUsage(System.AttributeTargets.Class,
                       AllowMultiple = false)
    ]
    public class GMTestClassAttribute : Attribute
    {
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
        public TimeSpan MaxExecutionDuration { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Skip { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        public GMTestClassAttribute(string title, bool active = true)
        {
            this.Title = title;
        }
    }
}
