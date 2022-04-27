using System;

using R5T.B0003;
using R5T.T0041;
using R5T.T0045;
using R5T.T0098;


namespace R5T.F0001.F002.V001
{
    public static class Instances
    {
        public static ICompilationUnitOperator CompilationUnitOperator { get; } = T0045.CompilationUnitOperator.Instance;
        public static INamespacedTypeName NamespacedTypeName { get; }
        public static IOperation Operation { get; } = T0098.Operation.Instance;
        public static IPathOperator PathOperator { get; } = T0041.PathOperator.Instance;
    }
}
