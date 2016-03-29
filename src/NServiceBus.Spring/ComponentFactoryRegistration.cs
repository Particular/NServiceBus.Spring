namespace NServiceBus.ObjectBuilder.Spring
{
    using System;
    using global::Spring.Context.Support;

    class ComponentFactoryRegistration<T> : RegisterAction
    {
        Func<T> componentFactory;
        DependencyLifecycle dependencyLifecycle;

        public ComponentFactoryRegistration(Func<T> componentFactory, DependencyLifecycle dependencyLifecycle)
        {
            this.dependencyLifecycle = dependencyLifecycle;
            this.componentFactory = componentFactory;
        }

        public override bool ApplicableForChildContainer => dependencyLifecycle == DependencyLifecycle.InstancePerUnitOfWork;

        public override void Register(GenericApplicationContext context)
        {
            var componentType = typeof(T);

            var funcFactory = new ArbitraryFuncDelegatingFactoryObject<T>(componentFactory, dependencyLifecycle == DependencyLifecycle.SingleInstance || dependencyLifecycle == DependencyLifecycle.InstancePerUnitOfWork);

            context.ObjectFactory.RegisterSingleton(componentType.FullName, funcFactory);
        }
        public override bool MatchesComponent(Type type)
        {
            return type.IsAssignableFrom(typeof(T));
        }
    }
}