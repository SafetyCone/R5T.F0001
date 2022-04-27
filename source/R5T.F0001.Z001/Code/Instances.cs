using System;

using R5T.B0001;
using R5T.B0002;
using R5T.B0003;
using R5T.T0119;


namespace R5T.F0001.Z001
{
    public static class Instances
    {
        public static IAssertion Assertion { get; } = T0119.Assertion.Instance;
        public static IExpectation Expectation { get; } = T0119.Expectation.Instance;
        public static INamespacedTypeName NamespacedTypeName { get; } = B0003.NamespacedTypeName.Instance;
        public static INamespaceName NamespaceName { get; } = B0002.NamespaceName.Instance;
        public static ITypeNameAlias TypeNameAlias { get; } = B0001.TypeNameAlias.Instance;
        public static ITypeNameFragment TypeNameFragment { get; } = B0001.TypeNameFragment.Instance;
    }
}
