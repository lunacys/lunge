using System;

namespace lunge.Library.Scheduling
{
    public class TaskCompletedEventArgs<T, TResult> : EventArgs
    {
        /// <summary>
        ///     Input given to task
        /// </summary>
        public T Input { get; }

        /// <summary>
        ///     Result of the task
        /// </summary>
        public TResult Result { get; }

        public TaskCompletedEventArgs(T input, TResult result)
        {
            Input = input;
            Result = result;
        }
    }
}