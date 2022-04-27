using System;

using R5T.B0002;
using R5T.B0003;


namespace R5T.F0001.F001
{
    public static class Instances
    {
        public static INamespacedTypeNameOperator NamespacedTypeNameOperator { get; } = B0003.NamespacedTypeNameOperator.Instance;
        public static INamespaceNameOperator NamespaceNameOperator { get; } = B0002.NamespaceNameOperator.Instance;
    }
}
