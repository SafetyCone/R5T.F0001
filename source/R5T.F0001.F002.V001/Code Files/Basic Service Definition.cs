using System;
using System.Linq;
using System.Threading.Tasks;

using R5T.T0064;


namespace R5T.S0030
{
    [ServiceImplementationMarker]
    public class ServiceDefinitionTypeIdentifier : IServiceDefinitionTypeIdentifier, IServiceImplementation
    {
        public async Task<ITypeNamedCodeFilePathed[]> GetServiceDefinitionTypes(string serviceDefinitionCodeFilePath)
        {
            var output = await Instances.Operation.GetTypeNamedCodeFilePatheds(
                serviceDefinitionCodeFilePath,
                compilationUnit =>
                {
                    var interfaces = compilationUnit.GetInterfaces();

                    var serviceDefinitionInterfaces = interfaces
                        .Where(Instances.InterfaceOperator.IsServiceDefinition)
                        .Now();

                    var serviceDefinitionInterfaceTypeNames = serviceDefinitionInterfaces.GetNamespacedTypeParameterizedWithConstraintsTypeNames().Now();

                    return Task.FromResult(serviceDefinitionInterfaceTypeNames);
                });

            return output;
        }
    }
}
