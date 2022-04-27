using System;

using R5T.B0003;


namespace R5T.F0001.Z001
{
    public static class INamespacedTypeNameExtensions
    {
        public static string R5T_S0030_IServiceDefinitionTypeIdentifier(this INamespacedTypeName _)
        {
            return "R5T.S0030.IServiceDefinitionTypeIdentifier";
        }

        public static string R5T_S0030_Repositories_IServiceRepository(this INamespacedTypeName _)
        {
            return "R5T.S0030.Repositories.IServiceRepository";
        }

        public static string[] AvailableServiceDefinitions(this INamespacedTypeName _)
        {
            var output = new[]
            {
                _.R5T_S0030_IServiceDefinitionTypeIdentifier(),
                _.R5T_S0030_Repositories_IServiceRepository(),
            };

            return output;
        }
    }
}
