using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnitAutomationFramework.WebElements
{
    /// <summary>
    /// The `ActionExpection` class represents custom exceptions that occur during actions in the automation framework.
    /// </summary>
    public class ActionExpection : Exception
    {
        /// <summary>
        /// Initializes a new instance of the `ActionExpection` class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ActionExpection(string message) : base(message)
        {

        }
    }
}
