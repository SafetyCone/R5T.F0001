using System;
using System.Collections.Generic;
using System.Linq;

using R5T.Magyar;

using R5T.T0098;
using R5T.T0129;


namespace R5T.F0001.F001
{
    public static class IOperationExtensions
    {
        private static string FullyUnaliasDestinationName_Internal(this IOperation _,
            string destinationName,
            Dictionary<string, NameAlias> nameAliasesByDestinationName,
            int depth)
        {
            // If we have descended through the same number of elements as in the set of name aliases, then there is a cycle.
            // This should never happen in C# since name aliases in a namespace cannot use other name aliases in the name space, only name aliases from parent namespaces, which prevents cycles.
            if (depth == nameAliasesByDestinationName.Count)
            {
                throw new Exception("Name alias cycle detected.");
            }

            var nameAlias = nameAliasesByDestinationName[destinationName];

            var sourceName = nameAlias.SourceName;

            // If the source name is itself aliased, then unalias it.
            var sourceNameIsAliased = nameAliasesByDestinationName.ContainsKey(sourceName);
            if (sourceNameIsAliased)
            {
                var output = _.FullyUnaliasDestinationName_Internal(
                    sourceName,
                    nameAliasesByDestinationName,
                    depth + 1);

                return output;
            }

            // If there are multiple tokens in the source name, see if the first token is aliased.
            var multipleTokensWereFound = Instances.NamespaceNameOperator.MultipleTokensWereFound(
                sourceName);

            if(multipleTokensWereFound)
            {
                var firstToken = multipleTokensWereFound.Result.First();

                // If the first token is aliased, then unalias it, and then append the rest of the tokens.
                var firstTokenIsAliased = nameAliasesByDestinationName.ContainsKey(firstToken);
                if (firstTokenIsAliased)
                {
                    var unaliasedFirstToken = _.FullyUnaliasDestinationName_Internal(
                        firstToken,
                        nameAliasesByDestinationName,
                        depth + 1);

                    // Append the rest of the tokens.
                    var output = Instances.NamespaceNameOperator.CombineTokens(
                        EnumerableHelper.From(unaliasedFirstToken)
                            .Concat(multipleTokensWereFound.Result.SkipFirst()));

                    return output;
                }
            }

            // Otherwise, the source name is the fully unalised name for the destination name.
            return sourceName;
        }

        /// <summary>
        /// C# allows each nested namespace to use the name aliases of the namespace above it. Thus, we could have x = R5T.F0001, then y = x, and finally z = y.F001.Class1.
        /// This could will fully unalias z to R5T.F0001.F001.Class1.
        /// </summary>
        public static string FullyUnaliasDestinationName(this IOperation _,
            string destinationName,
            Dictionary<string, NameAlias> nameAliasesByDestinationName)
        {
            var output = _.FullyUnaliasDestinationName_Internal(
                destinationName,
                nameAliasesByDestinationName,
                // Start at 0 so that cycle is detected on the first step of the cycle.
                0);

            return output;
        }

        public static WasFound<string> TryGuessNamespacedTypeName(this IOperation _,
            HashSet<string> availableNamespacedTypeNamesHash,
            string typeNameFragment,
            string containingNamespaceName,
            IEnumerable<string> availableNamespaceNames,
            Dictionary<string, NameAlias> availableTypeNameAliasesByDestinationName)
        {
            // Simple check: is the type name fragment already one of the available namespaced type names?
            var isAvailableNamespacedTypeName = availableNamespacedTypeNamesHash.Contains(typeNameFragment);
            if(isAvailableNamespacedTypeName)
            {
                // If so, then we are found!
                return WasFound.Found(typeNameFragment);
            }

            // See if the type name fragment is directly aliased.
            var typeNameAliasExists = availableTypeNameAliasesByDestinationName.ContainsKey(typeNameFragment);
            if (typeNameAliasExists)
            {
                // If so, check if the source name is one of the available namespaced type names.
                var typeNameAlias = availableTypeNameAliasesByDestinationName[typeNameFragment];

                var typeNameAliasSourceName = typeNameAlias.SourceName;

                var sourceIsAvailableNamespacedTypeName = availableNamespacedTypeNamesHash.Contains(typeNameAliasSourceName);

                // If the source name is available, then we are found.
                if(sourceIsAvailableNamespacedTypeName)
                {
                    return WasFound.Found(typeNameAliasSourceName);
                }

                // Else, fully unalias the source name, and see if it is one of the available namespaced type names.
                var fullyUnaliasedSourceName = _.FullyUnaliasDestinationName(
                    typeNameFragment,
                    availableTypeNameAliasesByDestinationName);

                var fullyUnaliasedSourceNameIsAvailableNamespacedTypeName = availableNamespacedTypeNamesHash.Contains(fullyUnaliasedSourceName);
                if(fullyUnaliasedSourceNameIsAvailableNamespacedTypeName)
                {
                    return WasFound.Found(fullyUnaliasedSourceName);
                }

                // Else, not found.
                return WasFound.NotFound<string>();
            }

            // If there are multiple namespace name tokens in the type name fragment, see if the first is aliased.
            var multipleTokensWereFound = Instances.NamespaceNameOperator.MultipleTokensWereFound(typeNameFragment);
            if(multipleTokensWereFound)
            {
                var firstToken = multipleTokensWereFound.Result.First();

                var firstTokenIsAliased = availableTypeNameAliasesByDestinationName.ContainsKey(firstToken);
                if(firstTokenIsAliased)
                {
                    var unaliasedFirstToken = _.FullyUnaliasDestinationName(
                        firstToken,
                        availableTypeNameAliasesByDestinationName);

                    // Append the rest of the tokens.
                    var unaliasedTypeNameFragment = Instances.NamespaceNameOperator.CombineTokens(
                        EnumerableHelper.From(unaliasedFirstToken)
                            .Concat(multipleTokensWereFound.Result.SkipFirst()));

                    var unaliasedTypeNameFragmentIsAvailableNamespacedTypeName = availableNamespacedTypeNamesHash.Contains(
                        unaliasedTypeNameFragment);

                    if(unaliasedTypeNameFragmentIsAvailableNamespacedTypeName)
                    {
                        return WasFound.Found(unaliasedTypeNameFragment);
                    }
                }
            }

            // Now, for the containing namespace, get all sub-namespaces.
            var allSubNamespacesOfContainingNamespace = Instances.NamespaceNameOperator.EnumerateNamespaceAndSubNamespaces(
                containingNamespaceName);

            // Finally, foreach of the available namespace names, prepend the available namespace name.
            var allAvailableNamespaceNames = availableNamespaceNames
                .Concat(allSubNamespacesOfContainingNamespace)
                .Now();

            foreach (var namespaceName in allAvailableNamespaceNames)
            {
                var candidateNamespacedTypeName = Instances.NamespacedTypeNameOperator.GetNamespacedTypeName(
                    namespaceName,
                    typeNameFragment);

                var candidateIsAvailableNamespacedTypeName = availableNamespacedTypeNamesHash.Contains(
                    candidateNamespacedTypeName);

                if(candidateIsAvailableNamespacedTypeName)
                {
                    return WasFound.Found(candidateNamespacedTypeName);
                }
            }

            // Else, no luck.
            return WasFound.NotFound<string>();
        }

        public static Dictionary<string, WasFound<string>> TryGuessNamespacedTypeNames(this IOperation _,
            HashSet<string> availableNamespacedTypeNamesHash,
            IEnumerable<string> typeNameFragments,
            string containingNamespaceName,
            IEnumerable<string> availableNamespaceNames,
            Dictionary<string, NameAlias> availableTypeNameAliasesByDestinationName)
        {
            var output = typeNameFragments
                // Ensure distinct.
                .Distinct()
                .Select(x => (TypeNameFragment: x, NamespacedTypeNameWasFound: _.TryGuessNamespacedTypeName(
                    availableNamespacedTypeNamesHash,
                    x,
                    containingNamespaceName,
                    availableNamespaceNames,
                    availableTypeNameAliasesByDestinationName)))
                .ToDictionary(
                    x => x.TypeNameFragment,
                    x => x.NamespacedTypeNameWasFound);

            return output;
        }

        public static Dictionary<string, WasFound<string>> TryGuessNamespacedTypeNames(this IOperation _,
            IEnumerable<string> availableNamespacedTypeNames,
            IEnumerable<string> typeNameFragments,
            string containingNamespaceName,
            IEnumerable<string> availableNamespaceNames,
            IEnumerable<NameAlias> availableTypeNameAliases)
        {
            var availableNamespacedTypeNamesHash = availableNamespacedTypeNames.ToHashSet();
            var availableTypeNameAliasesByDestinationName = availableTypeNameAliases
                .ToDictionary(x => x.DestinationName);

            var output = _.TryGuessNamespacedTypeNames(
                availableNamespacedTypeNamesHash,
                typeNameFragments,
                containingNamespaceName,
                availableNamespaceNames,
                availableTypeNameAliasesByDestinationName);

            return output;
        }

        public static WasFound<string> TryGuessNamespacedTypeName(this IOperation _,
            IEnumerable<string> availableNamespacedTypeNames,
            string typeNameFragment,
            string containingNamespaceName,
            IEnumerable<string> availableNamespaceNames,
            IEnumerable<NameAlias> availableTypeNameAliases)
        {
            var wasFoundByTypeNameFragment = _.TryGuessNamespacedTypeNames(
                availableNamespacedTypeNames,
                EnumerableHelper.From(typeNameFragment),
                containingNamespaceName,
                availableNamespaceNames,
                availableTypeNameAliases);

            var output = wasFoundByTypeNameFragment.Values.Single();
            return output;
        }
    }
}