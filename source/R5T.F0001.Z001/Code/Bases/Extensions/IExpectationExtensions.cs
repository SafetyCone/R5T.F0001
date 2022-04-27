using System;
using System.Additions;
using System.Collections.Generic;

using R5T.T0120;
using R5T.T0129;

using IExpectation = R5T.T0119.IExpectation;


namespace R5T.F0001.Z001
{
    public static class IExpectationExtensions
    {
        public static Expectation<
            (string typeNameFragment,
            string containingNamespaceName,
            IEnumerable<string> availableNamespacedTypeNames,
            IEnumerable<string> availableNamespaceNames,
            IEnumerable<NameAlias> availableTypeNameAliases),
            string> BasicServiceDefinitionNamespacedTypeNameGuess(this IExpectation _)
        {
            var output = new Expectation<(string, string, IEnumerable<string>, IEnumerable<string>, IEnumerable<NameAlias>), string>
            {
                Input = (
                    Instances.TypeNameFragment.IServiceRepository(),
                    Instances.NamespaceName.R5T_S0030_Repositories(),
                    Instances.NamespacedTypeName.AvailableServiceDefinitions(),
                    Instances.NamespaceName.OtherAvailableNamespaceNames(),
                    Instances.TypeNameAlias.AvailableNameAliases()),
                Output = Instances.NamespacedTypeName.R5T_S0030_Repositories_IServiceRepository(),
                OutputEqualityComparer = ComparisonBasedEqualityComparer.From<string>((x, y) => x == y),
            };

            return output;
        }
    }
}
