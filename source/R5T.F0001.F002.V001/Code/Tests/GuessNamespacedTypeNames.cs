using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using R5T.F0001.Z001;


namespace R5T.F0001.F002.V001
{
    [TestClass]
    public class GuessNamespacedTypeNames
    {
        [TestMethod]
        public async Task Basic()
        {
            // Get the reference code file path.
            var referenceFileOutputDirectoryRelativePath = @"/Code Files/Basic Service Definition.cs";

            var referenceFilePath = Instances.PathOperator.GetTestingOutputDirectoryFilePath_WithExistenceCheck(
                referenceFileOutputDirectoryRelativePath);

            var originalCompilationUnit = await Instances.CompilationUnitOperator.Load(referenceFilePath);

            var compilationUnit = originalCompilationUnit;

            var firstBaseType = compilationUnit.GetNamespaces().First().GetClasses().First().BaseList.Types.First().Type;

            compilationUnit = compilationUnit.AnnotateNode_Typed(
                firstBaseType,
                out var firstBaseTypeSyntaxAnnoation);

            var namespacedTypeNameWasFound = Instances.Operation.TryGuessNamespacedTypeName(
                Instances.NamespacedTypeName.AvailableServiceDefinitions(),
                compilationUnit,
                firstBaseTypeSyntaxAnnoation);
        }
    }
}
