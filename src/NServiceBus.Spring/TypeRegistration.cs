namespace NServiceBus.ObjectBuilder.Spring
{
    using System;
    using global::Spring.Context.Support;
    using global::Spring.Objects.Factory.Config;
    using global::Spring.Objects.Factory.Support;

    abstract class TypeRegistration : RegisterAction
    {
        protected TypeRegistration(Type componentType, DependencyLifecycle dependencyLifecycle, IObjectDefinitionFactory definitionFactory)
        {
            this.definitionFactory = definitionFactory;
            this.componentType = componentType;
            this.dependencyLifecycle = dependencyLifecycle;
        }

        public override bool ApplicableForChildContainer =>
            throw new InvalidOperationException("TypeRegistration ApplicableForChildContainer should never be called. The decision should is done in the TypeRegistrationDecorator");

        public override void Register(GenericApplicationContext context)
        {
            var builder = CreateBuilder();
            IObjectDefinition def = builder.ObjectDefinition;
            context.RegisterObjectDefinition(componentType.FullName, def);
        }

        protected abstract ObjectDefinitionBuilder CreateBuilder();

        public override bool MatchesComponent(Type type)
        {
            return type.IsAssignableFrom(componentType);
        }

        protected DependencyLifecycle dependencyLifecycle;
        protected Type componentType;
        protected IObjectDefinitionFactory definitionFactory;
    }
}