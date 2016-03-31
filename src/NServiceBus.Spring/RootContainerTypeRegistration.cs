namespace NServiceBus.ObjectBuilder.Spring
{
    using System;
    using global::Spring.Objects.Factory.Config;
    using global::Spring.Objects.Factory.Support;

    class RootContainerTypeRegistration : TypeRegistration
    {
        public RootContainerTypeRegistration(Type componentType, DependencyLifecycle dependencyLifecycle, IObjectDefinitionFactory definitionFactory)
            : base(componentType, dependencyLifecycle, definitionFactory)
        {
        }

        protected override ObjectDefinitionBuilder CreateBuilder()
        {
            return ObjectDefinitionBuilder.RootObjectDefinition(definitionFactory, componentType)
                .SetAutowireMode(AutoWiringMode.AutoDetect)
                .SetSingleton(dependencyLifecycle == DependencyLifecycle.SingleInstance || dependencyLifecycle == DependencyLifecycle.InstancePerUnitOfWork);
        }
    }
}