using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using R5T.T0129;

using R5T.F0001.Z001;


namespace R5T.F0001.F001.V001
{
    [TestClass]
    public class GuessNamespacedTypeNames
    {
        [TestMethod]
        public void Basic()
        {
            var expectation = Instances.Expectation.BasicServiceDefinitionNamespacedTypeNameGuess();

            var actualValue = Instances.Operation.TryGuessNamespacedTypeName(
                expectation.Input.availableNamespacedTypeNames,
                expectation.Input.typeNameFragment,
                expectation.Input.containingNamespaceName,
                expectation.Input.availableNamespaceNames,
                expectation.Input.availableTypeNameAliases);

            Instances.Assertion.AreEqual(expectation, actualValue);
        }

        [TestMethod]
        public void NestedAliasesForType()
        {
            var expectation = Instances.Expectation.BasicServiceDefinitionNamespacedTypeNameGuess();

            var actualValue = Instances.Operation.TryGuessNamespacedTypeName(
                expectation.Input.availableNamespacedTypeNames,
                "z",
                Instances.NamespaceName.R5T_S0030(),
                expectation.Input.availableNamespaceNames,
                new[]
                {
                    NameAlias.From("x", "R5T.S0030"),
                    NameAlias.From("y", "x"),
                    NameAlias.From("z", "y.Repositories.IServiceRepository"),
                });

            Instances.Assertion.AreEqual(expectation, actualValue);
        }

        [TestMethod]
        public void NestedAliasesForNamespace()
        {
            var expectation = Instances.Expectation.BasicServiceDefinitionNamespacedTypeNameGuess();

            var actualValue = Instances.Operation.TryGuessNamespacedTypeName(
                expectation.Input.availableNamespacedTypeNames,
                "z.IServiceRepository",
                Instances.NamespaceName.R5T_S0030(),
                expectation.Input.availableNamespaceNames,
                new[]
                {
                    NameAlias.From("x", "R5T.S0030"),
                    NameAlias.From("y", "x"),
                    NameAlias.From("z", "y.Repositories"),
                });

            Instances.Assertion.AreEqual(expectation, actualValue);
        }
    }
}
