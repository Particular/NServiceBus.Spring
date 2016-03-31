namespace NServiceBus.ObjectBuilder.Spring
{
    using System;
    using global::Spring.Context.Support;
    using global::Spring.Objects.Factory.Support;

    class TypeRegistrationProxy : RegisterAction
    {
        public TypeRegistrationProxy(Type componentType, DependencyLifecycle dependencyLifecycle, IObjectDefinitionFactory definitionFactory)
        {
            this.definitionFactory = definitionFactory;
            this.componentType = componentType;
            this.dependencyLifecycle = dependencyLifecycle;
        }

        public override bool ApplicableForChildContainer => dependencyLifecycle == DependencyLifecycle.InstancePerUnitOfWork;

        public override void Register(GenericApplicationContext context)
        {
            RegisterAction registerAction;
            if (context.Name.StartsWith("child_of_"))
            {
                registerAction = new ChildContainerTypeRegistration(componentType, dependencyLifecycle, definitionFactory);
            }
            else
            {
                registerAction = new RootContainerTypeRegistration(componentType, dependencyLifecycle, definitionFactory);
            }

            registerAction.Register(context);
        }

        public override bool MatchesComponent(Type type)
        {
            return type.IsAssignableFrom(componentType);
        }

        DependencyLifecycle dependencyLifecycle;
        Type componentType;
        IObjectDefinitionFactory definitionFactory;
    }
}