using System;

using R5T.B0001;
using R5T.T0129;


namespace R5T.F0001.Z001
{
    public static class ITypeNameAliasExtensions
    {
        public static NameAlias FileContextIsMainFileContext(this ITypeNameAlias _)
        {
            var output = NameAlias.From("FileContext", "R5T.S0030.FileContexts.MainFileContext");
            return output;
        }

        public static NameAlias[] AvailableNameAliases(this ITypeNameAlias _)
        {
            var output = new[]
            {
                _.FileContextIsMainFileContext(),
            };

            return output;
        }
    }
}
