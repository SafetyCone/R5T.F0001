using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using R5T.Magyar;
using R5T.Magyar.Results;

using R5T.T0126;

using R5T.F0001.F001;

using IOperation = R5T.T0098.IOperation;
using NameAlias = R5T.T0129.NameAlias;


namespace R5T.F0001.F002
{
    public static class IOperationExtensions
    {
        public static WasFound<AttributeSyntax> HasAttributeOfType<T>(this IOperation _,
            ParameterSyntax parameter,
            CompilationUnitSyntax compilationUnit)
            where T : Attribute
        {
            var attributeNamespacedTypeName = Instances.NamespacedTypeNameOperator.GetNamespacedTypeName<T>();

            var output = _.HasAttributeOfType(
                parameter,
                compilationUnit,
                attributeNamespacedTypeName);

            return output;
        }

        public static WasFound<AttributeSyntax> HasAttributeOfType(this IOperation _,
            ParameterSyntax parameter,
            CompilationUnitSyntax compilationUnit,
            string attributeNamespacedTypeName)
        {
            var attributesOfType = _.HasAttributesOfType(
                parameter,
                compilationUnit,
                attributeNamespacedTypeName);

            var output = attributesOfType.Result?.SingleOrDefault();

            return WasFound.From(output);
        }

        public static WasFound<AttributeSyntax[]> HasAttributesOfType(this IOperation _,
            ParameterSyntax parameter,
            CompilationUnitSyntax compilationUnit,
            string attributeNamespacedTypeName)
        {
            var hasAttributes = parameter.HasAttributes();
            if (!hasAttributes)
            {
                return WasFound.NotFound<AttributeSyntax[]>();
            }

            var output = _.HasAttributesOfType(
                hasAttributes.Result,
                compilationUnit,
                attributeNamespacedTypeName);

            return output;
        }

        public static WasFound<AttributeSyntax> HasAttributeOfType<T>(this IOperation _,
            MemberDeclarationSyntax memberDeclaration,
            CompilationUnitSyntax compilationUnit)
            where T : Attribute
        {
            var attributeNamespacedTypeName = Instances.NamespacedTypeNameOperator.GetNamespacedTypeName<T>();

            var output = _.HasAttributeOfType(
                memberDeclaration,
                compilationUnit,
                attributeNamespacedTypeName);

            return output;
        }

        public static WasFound<AttributeSyntax> HasAttributeOfType(this IOperation _,
            MemberDeclarationSyntax memberDeclaration,
            CompilationUnitSyntax compilationUnit,
            string attributeNamespacedTypeName)
        {
            var attributesOfType = _.HasAttributesOfType(
                memberDeclaration,
                compilationUnit,
                attributeNamespacedTypeName);

            var output = attributesOfType.Result?.SingleOrDefault();

            return WasFound.From(output);
        }

        public static WasFound<AttributeSyntax[]> HasAttributesOfType<T>(this IOperation _,
            MemberDeclarationSyntax memberDeclaration,
            CompilationUnitSyntax compilationUnit)
        {
            var attributeNamespacedTypeName = Instances.NamespacedTypeNameOperator.GetNamespacedTypeName<T>();

            var output = _.HasAttributesOfType(
                memberDeclaration,
                compilationUnit,
                attributeNamespacedTypeName);

            return output;
        }

        public static WasFound<AttributeSyntax[]> HasAttributesOfType(this IOperation _,
            MemberDeclarationSyntax memberDeclaration,
            CompilationUnitSyntax compilationUnit,
            string attributeNamespacedTypeName)
        {
            var hasAttributes = memberDeclaration.HasAttributes();
            if (!hasAttributes)
            {
                return WasFound.NotFound<AttributeSyntax[]>();
            }

            var output = _.HasAttributesOfType(
                hasAttributes.Result,
                compilationUnit,
                attributeNamespacedTypeName);

            return output;
        }

        public static WasFound<AttributeSyntax[]> HasAttributesOfType(this IOperation _,
            AttributeSyntax[] attributes,
            CompilationUnitSyntax compilationUnit,
            string attributeNamespacedTypeName)
        {
            var attributesByAttributeTypes = attributes
                .ToDictionary(
                    x => x.Name);

            var withAttributeSuffix = Instances.AttributeTypeName.GetEnsuredAttributeSuffixedTypeName(attributeNamespacedTypeName);
            var withoutAttributeSuffix = Instances.AttributeTypeName.GetEnsuredNonAttributeSuffixedTypeName(attributeNamespacedTypeName);

            var isAttributeTypeByAttributeType = _.TryGuessNamespacedTypeNames(
                EnumerableHelper.From(
                    withAttributeSuffix,
                    withoutAttributeSuffix),
                compilationUnit,
                attributesByAttributeTypes.Keys);

            var attributeTypesFound = isAttributeTypeByAttributeType
                .Where(x => x.Value.Exists)
                .Select(x => x.Key)
                .Now_OLD();

            var output = attributeTypesFound
                .Select(xAttributeType => attributesByAttributeTypes[xAttributeType])
                .Now_OLD();

            return WasFound.FromArray(output);
        }

        public static WasFound<BaseTypeSyntax> HasBaseTypeOfType<T>(this IOperation _,
            ClassDeclarationSyntax classDeclaration,
            CompilationUnitSyntax compilationUnit)
        {
            var namespacedTypeName = Instances.NamespacedTypeNameOperator.GetNamespacedTypeName<T>();

            var output = _.HasBaseTypeOfType(
                classDeclaration,
                compilationUnit,
                namespacedTypeName);

            return output;
        }

        public static WasFound<BaseTypeSyntax> HasBaseTypeOfType(this IOperation _,
            ClassDeclarationSyntax classDeclaration,
            CompilationUnitSyntax compilationUnit,
            string namespacedTypeName)
        {
            var hasBaseTypes = classDeclaration.HasBaseTypes();
            if (!hasBaseTypes)
            {
                return WasFound.NotFound<BaseTypeSyntax>();
            }

            var baseTypesByBaseTypeType = hasBaseTypes.Result
                .ToDictionary(
                    x => x.Type);

            var isBaseTypeByBaseTypeType = _.TryGuessNamespacedTypeNames(
                EnumerableHelper.From(namespacedTypeName),
                compilationUnit,
                baseTypesByBaseTypeType.Keys);

            var baseTypesFound = isBaseTypeByBaseTypeType
                .Where(x => x.Value.Exists)
                .Select(x => x.Key)
                .Now_OLD();

            var output = baseTypesFound
                .Select(xAttributeType => baseTypesByBaseTypeType[xAttributeType])
                // Even though it's not syntactically correct ('X' is already listed in interface list, or cannot have multiple base classes), there might be incorrect syntax. So use First() not Single().
                .FirstOrDefault();

            return WasFound.From(output);
        }

        public static FunctionResult<string> HasBaseType(this IOperation _,
            ClassDeclarationSyntax classDeclaration,
            CompilationUnitSyntax compilationUnit,
            string availableNamespacedTypeName)
        {
            var output = _.HasBaseType(
                classDeclaration,
                compilationUnit,
                ArrayHelper.From(availableNamespacedTypeName));

            return output;
        }

        public static Dictionary<BaseTypeSyntax, WasFound<string>> HasBaseTypesOfTypes(this IOperation _,
            ClassDeclarationSyntax classDeclaration,
            CompilationUnitSyntax compilationUnit,
            IList<string> availableNamespacedTypeNames)
        {
            var hasBaseTypes = classDeclaration.HasBaseTypes();
            if (!hasBaseTypes)
            {
                return new Dictionary<BaseTypeSyntax, WasFound<string>>();
            }

            var baseTypesByBaseTypeType = hasBaseTypes.Result
                .ToDictionary(
                    x => x.Type);

            var isAvailableTypeByBaseTypeType = _.TryGuessNamespacedTypeNames(
                availableNamespacedTypeNames,
                compilationUnit,
                baseTypesByBaseTypeType.Keys);

            var output = isAvailableTypeByBaseTypeType
                .ToDictionary(
                    x => baseTypesByBaseTypeType[x.Key],
                    x => x.Value);

            return output;
        }

        public static FunctionResult<string> HasBaseType(this IOperation _,
            ClassDeclarationSyntax classDeclaration,
            CompilationUnitSyntax compilationUnit,
            IList<string> availableNamespacedTypeNames)
        {
            var hasBaseTypes = classDeclaration.HasBaseTypes();
            if (!hasBaseTypes)
            {
                return FunctionResult.Failure<string>("Class did not have any base types.");
            }

            var baseTypeTypes = hasBaseTypes.Result
                .Select(x => x.Type)
                .Now_OLD();

            compilationUnit = compilationUnit.AnnotateNodes_Typed(
                baseTypeTypes,
                out var typedAnnotationsByBaseTypeType);

            var baseTypeTypesByTypedAnnotation = typedAnnotationsByBaseTypeType.Invert();

            var baseTypeNamespacedTypeNamesByTypedAnnotation = _.TryGuessNamespacedTypeNames(
                availableNamespacedTypeNames,
                compilationUnit,
                baseTypeTypesByTypedAnnotation.Keys);

            var baseTypeNamespacedTypeNamesFound = baseTypeNamespacedTypeNamesByTypedAnnotation.Values
                .Where(x => x.Exists)
                .Select(x => x.Result)
                .Now_OLD();

            var noBaseTypeNamespacedTypeNameWasFound = baseTypeNamespacedTypeNamesFound.None_OLD();
            if (noBaseTypeNamespacedTypeNameWasFound)
            {
                return FunctionResult.Failure<string>("No base type was recognized among the available namespaced type names.");
            }

            var multipleBaseTypeNamespacedTypeNameWereFound = baseTypeNamespacedTypeNamesFound.Multiple();
            if (multipleBaseTypeNamespacedTypeNameWereFound)
            {
                // Use the first base type found that is a service definition.
                foreach (var baseTypeType in baseTypeTypes)
                {
                    var baseTypeTypedAnnotation = typedAnnotationsByBaseTypeType[baseTypeType];

                    var namespacedTypeNameWasFound = baseTypeNamespacedTypeNamesByTypedAnnotation[baseTypeTypedAnnotation];
                    if (namespacedTypeNameWasFound)
                    {
                        // Warning, not failure.
                        return FunctionResult.Warning(
                            namespacedTypeNameWasFound.Result,
                            "Multiple available namespaced types names found in base types list for class, using first found.");
                    }
                }

                // Note, the above for-loop will always succeed, but check.
                throw new Exception("Namespaced type name should have been found, but was not.");
            }

            // At this point, there is exactly one base type, namespaced type name found.
            var namespacedTypeName = baseTypeNamespacedTypeNamesFound.Single();

            return FunctionResult.Success(namespacedTypeName);
        }

        public static WasFound<string> TryGuessNamespacedTypeName<TTypeSyntax>(this IOperation _,
            IEnumerable<string> availableNamespacedTypeNames,
            CompilationUnitSyntax compilationUnit,
            TTypeSyntax type)
            where TTypeSyntax : TypeSyntax
        {
            var types = EnumerableHelper.From(type);

            var wasFoundByType = _.TryGuessNamespacedTypeNames(
                availableNamespacedTypeNames,
                compilationUnit,
                types);

            var output = wasFoundByType.Values.Single();
            return output;
        }

        public static Dictionary<TTypeSyntax, WasFound<string>> TryGuessNamespacedTypeNames<TTypeSyntax>(this IOperation _,
            IEnumerable<string> availableNamespacedTypeNames,
            CompilationUnitSyntax compilationUnit,
            IEnumerable<TTypeSyntax> types)
            where TTypeSyntax : TypeSyntax
        {
            compilationUnit = compilationUnit.AnnotateNodes_Typed(
                    types,
                    out var typedAnnotationsByType);

            var namespacedTypeNamesByTypedAnnotation = _.TryGuessNamespacedTypeNames(
                availableNamespacedTypeNames,
                compilationUnit,
                typedAnnotationsByType.Values);

            var output = types
                .Select(x => (Type: x, TypedAnnotation: typedAnnotationsByType[x]))
                .Select(x => (x.Type, NamespacedTypeName: namespacedTypeNamesByTypedAnnotation[x.TypedAnnotation]))
                .ToDictionary(
                    x => x.Type,
                    x => x.NamespacedTypeName);

            return output;
        }

        public static Dictionary<string, WasFound<string>> TryGuessNamespacedTypeNames<TTypeSyntax>(this IOperation _,
            IEnumerable<string> availableNamespacedTypeNames,
            SyntaxNode referenceLocationNode,
            IEnumerable<string> typeNameFragments)
        {
            var (containingNamespaceName, availableNamespaceNames, availableTypeNameAliases) = _.GetGuessNamespacedTypeNameInformation(
                referenceLocationNode);

            // Now perform string operations.
            var output = _.TryGuessNamespacedTypeNames(
                availableNamespacedTypeNames,
                typeNameFragments,
                containingNamespaceName,
                availableNamespaceNames,
                availableTypeNameAliases);

            return output;
        }

        public static Dictionary<ISyntaxNodeAnnotation<TTypeSyntax>, WasFound<string>> TryGuessNamespacedTypeNames<TTypeSyntax>(this IOperation _,
            IEnumerable<string> availableNamespacedTypeNames,
            CompilationUnitSyntax compilationUnit,
            IEnumerable<ISyntaxNodeAnnotation<TTypeSyntax>> typeSyntaxAnnotations)
            where TTypeSyntax : TypeSyntax
        {
            // Get the type name fragments.
            var typeSyntaxesByAnnotation = compilationUnit.GetAnnotatedNodes_Typed(typeSyntaxAnnotations);

            var typeNameFragmentsByAnnotation = typeSyntaxesByAnnotation
                .ToDictionary(
                    x => x.Key,
                    x => x.Value.GetTypeName_HandlingTypeParameters());

            // Get the containing namespace name, ensuring that all exist within the same containing namespace name.
            var containingNamespaceName = typeSyntaxesByAnnotation.Values
                .Select(x => x.GetContainingNamespaceName())
                .Distinct()
                .Single();

            // Get the available namespace names.
            // Since there can be multiple instances of the same namespace name within a namespace, each either their own using directives, at this point we need to iterate over the type syntaxes one-by-one.
            var output = new Dictionary<ISyntaxNodeAnnotation<TTypeSyntax>, WasFound<string>>();

            foreach (var typeNameFragmentByAnnotationPair in typeNameFragmentsByAnnotation)
            {
                var typeSyntax = typeSyntaxesByAnnotation[typeNameFragmentByAnnotationPair.Key];
                var typeNameFragment = typeNameFragmentByAnnotationPair.Value;

                var availableUsingDirectives = typeSyntax.GetAvailableUsingDirectives();

                var availableNamespaceNames = availableUsingDirectives.GetUsingNamespaceNames();
                var availableTypeNameAliases = availableUsingDirectives.GetUsingNameAliases()
                    // Convert between different name alias types.
                    .Select(x => new T0129.NameAlias
                    {
                        DestinationName = x.DestinationName,
                        SourceName = x.SourceNameExpression,
                    })
                    .Now_OLD();

                // Now perform string operations.
                var namespacedTypeNameWasFound = _.TryGuessNamespacedTypeName(
                    availableNamespacedTypeNames,
                    typeNameFragment,
                    containingNamespaceName,
                    availableNamespaceNames,
                    availableTypeNameAliases);

                output.Add(typeNameFragmentByAnnotationPair.Key, namespacedTypeNameWasFound);
            }

            return output;
        }

        public static (
            string containingNamespaceName,
            string[] availableNamespaceNames,
            NameAlias[] availableNameAliases)
        GetGuessNamespacedTypeNameInformation(this IOperation _,
            SyntaxNode node)
        {
            // Get the containing namespace name.
            var containingNamespaceName = node.GetContainingNamespaceName();

            // Get the available namespace names.
            var availableUsingDirectives = node.GetAvailableUsingDirectives();

            var availableNamespaceNames = availableUsingDirectives.GetUsingNamespaceNames()
                .Now_OLD();

            var availableTypeNameAliases = availableUsingDirectives.GetUsingNameAliases()
                // Convert between different name alias types.
                .Select(x => new T0129.NameAlias
                {
                    DestinationName = x.DestinationName,
                    SourceName = x.SourceNameExpression,
                })
                .Now_OLD();

            return (containingNamespaceName, availableNamespaceNames, availableTypeNameAliases);
        }

        public static WasFound<string> TryGuessNamespacedTypeName<TTypeSyntax>(this IOperation _,
            IEnumerable<string> availableNamespacedTypeNames,
            CompilationUnitSyntax compilationUnit,
            ISyntaxNodeAnnotation<TTypeSyntax> typeSyntaxAnnotation)
            where TTypeSyntax : TypeSyntax
        {
            // Get the type name fragment.
            var typeSyntax = compilationUnit.GetAnnotatedNode_Typed(typeSyntaxAnnotation);

            var typeNameFragment = typeSyntax.GetTypeName();

            var (containingNamespaceName, availableNamespaceNames, availableTypeNameAliases) = _.GetGuessNamespacedTypeNameInformation(
                typeSyntax);

            // Now perform string operations.
            var output = _.TryGuessNamespacedTypeName(
                availableNamespacedTypeNames,
                typeNameFragment,
                containingNamespaceName,
                availableNamespaceNames,
                availableTypeNameAliases);

            return output;
        }

        public static bool HasAttributeOfNamespacedTypeName(this IOperation _,
            TypeDeclarationSyntax typeDeclaration,
            CompilationUnitSyntax compilationUnit,
            string attributeNamespacedTypeName)
        {
            var attributeSuffixed = Instances.AttributeTypeName.GetEnsuredAttributeSuffixedTypeName(attributeNamespacedTypeName);
            var nonAttributeSuffixed = Instances.AttributeTypeName.GetEnsuredNonAttributeSuffixedTypeName(attributeNamespacedTypeName);

            var hasAttributes = typeDeclaration.HasAttributes();
            if (hasAttributes)
            {
                var attributeTypeNames = hasAttributes.Result
                    .Select(x => x.Name)
                    .Now_OLD();

                var namespacedTypeNamesByType = _.TryGuessNamespacedTypeNames(
                    new[]
                    {
                        attributeSuffixed,
                        nonAttributeSuffixed,
                    },
                    compilationUnit,
                    attributeTypeNames);

                // Use any, even though there should only be one or zero.
                var output = namespacedTypeNamesByType.AnyWereFound();
                return output;
            }
            else
            {
                return false;
            }
        }

        public static bool HasBaseTypeWithNamespacedTypeName(this IOperation _,
            TypeDeclarationSyntax interfaceDeclaration,
            CompilationUnitSyntax compilationUnit,
            string namespacedTypeName)
        {
            var hasBaseTypes = interfaceDeclaration.HasBaseTypes();
            if(hasBaseTypes)
            {
                var baseTypeTypes = hasBaseTypes.Result
                    .Select(x => x.Type)
                    .Now_OLD();

                var namespacedTypeNamesByType = _.TryGuessNamespacedTypeNames(
                    EnumerableHelper.From(namespacedTypeName),
                    compilationUnit,
                    baseTypeTypes);

                // Use any, even though there should only be one or zero.
                var output = namespacedTypeNamesByType.AnyWereFound();
                return output;
            }
            else
            {
                return false;
            }
        }
    }
}