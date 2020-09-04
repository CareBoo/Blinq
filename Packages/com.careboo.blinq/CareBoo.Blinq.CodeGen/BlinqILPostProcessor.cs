using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.CompilationPipeline.Common.Diagnostics;
using Unity.CompilationPipeline.Common.ILPostProcessing;

namespace CareBoo.Blinq.CodeGen
{
    public class BlinqILPostProcessor : ILPostProcessor
    {
        public override ILPostProcessor GetInstance()
        {
            return this;
        }

        public override ILPostProcessResult Process(ICompiledAssembly compiledAssembly)
        {
            var diagnostics = new List<DiagnosticMessage>();
            diagnostics.Add(new DiagnosticMessage { MessageData = $"[{GetType().Name}]: {compiledAssembly.Name}", DiagnosticType = DiagnosticType.Warning });
            return new ILPostProcessResult(null, diagnostics);
        }

        public override bool WillProcess(ICompiledAssembly compiledAssembly)
        {
            if (compiledAssembly.Name == "CareBoo.Blinq")
                return true;
            return compiledAssembly.References.Any(f => Path.GetFileName(f) == "CareBoo.Blinq.dll") &&
                !compiledAssembly.Name.Contains("CodeGen.Tests");
        }
    }
}
