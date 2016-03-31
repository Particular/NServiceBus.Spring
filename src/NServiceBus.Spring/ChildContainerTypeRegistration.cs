namespace NServiceBus.ObjectBuilder.Spring
{
    using System;
    using global::Spring.Objects.Factory.Config;
    using global::Spring.Objects.Factory.Support;

    class ChildContainerTypeRegistration : TypeRegistration
    {
        public ChildContainerTypeRegistration(Type componentType, DependencyLifecycle dependencyLifecycle, IObjectDefinitionFactory definitionFactory)
            : base(componentType, dependencyLifecycle, definitionFactory)
        {
        }

        protected override ObjectDefinitionBuilder CreateBuilder()
        {
            return ObjectDefinitionBuilder.RootObjectDefinition(definitionFactory, componentType)
                .SetAutowireMode(AutoWiringMode.AutoDetect)
                .SetSingleton(true);
        }
    }
}