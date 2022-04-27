using System;

using R5T.B0003;
using R5T.T0034;


namespace R5T.F0001.F002
{
    public static class Instances
    {
        public static IAttributeTypeName AttributeTypeName { get; } = T0034.AttributeTypeName.Instance;
        public static INamespacedTypeNameOperator NamespacedTypeNameOperator { get; } = B0003.NamespacedTypeNameOperator.Instance;
    }
}
