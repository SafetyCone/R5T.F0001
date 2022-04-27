using System;

using R5T.B0002;


namespace R5T.F0001.Z001
{
    public static class INamespaceNameExtensions
    {
        public static string R5T_S0030(this INamespaceName _)
        {
            return "R5T.S0030";
        }

        public static string R5T_S0030_Repositories(this INamespaceName _)
        {
            return "R5T.S0030.Repositories";
        }

        public static string[] OtherAvailableNamespaceNames(this INamespaceName _)
        {
            var output = new[]
            {
                "System",
                "System.Collections.Generic",
                "System.Linq",
                "System.Threading.Tasks",

                "R5T.Magyar",

                "R5T.T0064",
                "R5T.T0094",
                "R5T.T0097",
                "R5T.T0101",
                "R5T.T0128",
                "R5T.T0128.D001",
            };

            return output;
        }
    }
}
