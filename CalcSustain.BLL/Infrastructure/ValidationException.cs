using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcSustain.BLL.Infrastructure
{
    /// <summary>
    /// BLL level validation - as transfer exceptions to WEB level
    /// </summary>
    public class ValidationException : Exception
    {
        /// <summary>
        ///  model name - that not correct and not passed through validation
        /// </summary>
        public string Property { get; protected set; }

        /// <summary>
        /// Validation exception at BLL level
        /// </summary>
        /// <param name="message">message appeared for this property - if it's incorrect</param>
        /// <param name="prop">model name - that not correct and not passed through validation</param>
        public ValidationException(string message, string prop)
            : base(message)
        {
            Property = prop;
        }
    }
}
