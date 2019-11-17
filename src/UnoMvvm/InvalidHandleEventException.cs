using System;
using System.Reflection;

namespace UnoMvvm
{
    /// <summary>
    /// Represents errors that occur during WeakEventManager.HandleEvent execution.
    /// </summary>
    public class InvalidHandleEventException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:UnoMvvm.InvalidHandleEventException"/> class.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="targetParameterCountException">Target parameter count exception.</param>
        public InvalidHandleEventException(string message, TargetParameterCountException targetParameterCountException) : base(message, targetParameterCountException)
        {

        }
    }
}
