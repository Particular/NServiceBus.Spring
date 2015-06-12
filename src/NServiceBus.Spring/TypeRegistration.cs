namespace NServiceBus.ObjectBuilder.Spring
{
    using System;
    using global::Spring.Context.Support;
    using global::Spring.Objects.Factory.Config;
    using global::Spring.Objects.Factory.Support;

    abstract class TypeRegistration : RegisterAction
    {
        protected readonly DependencyLifecycle dependencyLifecycle;
        protected readonly Type componentType;
        protected readonly IObjectDefinitionFactory definitionFactory;
        readonly ComponentConfig componentConfig;

        protected TypeRegistration(Type componentType, ComponentConfig componentConfig, DependencyLifecycle dependencyLifecycle, IObjectDefinitionFactory definitionFactory)
        {
            this.definitionFactory = definitionFactory;
            this.componentConfig = componentConfig;
            this.componentType = componentType;
            this.dependencyLifecycle = dependencyLifecycle;
        }

        public override bool ApplicableForChildContainer
        {
            get { throw new InvalidOperationException("TypeRegistration ApplicableForChildContainer should never be called. The decision should is done in the TypeRegistrationDecorator"); }
        }

        public override void Register(GenericApplicationContext context)
        {
            var builder = CreateBuilder();

            componentConfig.Configure(builder);

            IObjectDefinition def = builder.ObjectDefinition;
            context.RegisterObjectDefinition(componentType.FullName, def);
        }

        protected abstract ObjectDefinitionBuilder CreateBuilder();
    }
}