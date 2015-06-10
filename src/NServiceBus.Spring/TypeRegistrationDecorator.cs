namespace NServiceBus.ObjectBuilder.Spring
{
    using System;
    using global::Spring.Context.Support;
    using global::Spring.Objects.Factory.Support;

    class TypeRegistrationDecorator : RegisterAction
    {
        readonly DependencyLifecycle dependencyLifecycle;
        readonly Type componentType;
        readonly ComponentConfig componentConfig;
        readonly IObjectDefinitionFactory definitionFactory;

        public TypeRegistrationDecorator(Type componentType, ComponentConfig componentConfig, DependencyLifecycle dependencyLifecycle, IObjectDefinitionFactory definitionFactory)
        {
            this.definitionFactory = definitionFactory;
            this.componentConfig = componentConfig;
            this.componentType = componentType;
            this.dependencyLifecycle = dependencyLifecycle;
        }

        public override bool ApplicableForChildContainer
        {
            get { return dependencyLifecycle == DependencyLifecycle.InstancePerUnitOfWork; }
        }

        public override void Register(GenericApplicationContext context)
        {
            var registerAction = context.Name.StartsWith("child_of_") ?
                (RegisterAction)new ChildContainerTypeRegistration(componentType, componentConfig, dependencyLifecycle, definitionFactory) :
                new RootContainerTypeRegistration(componentType, componentConfig, dependencyLifecycle, definitionFactory);

            registerAction.Register(context);
        }
    }
}