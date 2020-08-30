using System;
using System.Diagnostics;

namespace CareBoo.Blinq
{
    /// <summary>
    /// Use this attribute on methods that are intended to be sourced for codegen,
    /// then retargeted to <see cref="T:CareBoo.Blinq.CodeGenTargetApiAttribute">.
    /// </summary>
    [Conditional("UNITY_EDITOR")]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    internal class CodeGenSourceApiAttribute : Attribute
    {
        public Guid PairId { get; }

        /// <summary>
        /// Create a <see cref="T:CareBoo.Blinq.CodeGenSourceApiAttribute">.
        /// </summary>
        /// <param name="pairId">The pairId is used to reference the corresponding <see cref="T:CareBoo.Blinq.CodeGenTargetApiAttribute">.</param>
        public CodeGenSourceApiAttribute(string pairId)
        {
            PairId = Guid.Parse(pairId);
        }
    }
}
