using System;
using System.Diagnostics;

namespace CareBoo.Blinq
{
    /// <summary>
    /// Use this attribute on methods that are intended to be targeted by <see cref="T:CareBoo.Blinq.CodeGenSourceApiAttribute">.
    /// </summary>
    [Conditional("UNITY_EDITOR")]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    internal class CodeGenTargetApiAttribute : Attribute
    {
        public Guid PairId { get; }

        /// <summary>
        /// Create a <see cref="T:CareBoo.Blinq.CodeGenTargetApiAttribute">.
        /// </summary>
        /// <param name="pairId">The pairId is used to reference the corresponding <see cref="T:CareBoo.Blinq.CodeGenSourceApiAttribute">.</param>
        public CodeGenTargetApiAttribute(string pairId)
        {
            PairId = Guid.Parse(pairId);
        }
    }
}
