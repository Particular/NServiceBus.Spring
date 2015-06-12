namespace NServiceBus.ObjectBuilder.Spring
{
    using System;
    using global::Spring.Context.Support;

    class SingletonRegistration : RegisterAction
    {
        readonly Type componentType;
        readonly object instance;

        public SingletonRegistration(Type componentType, object instance)
        {
            this.instance = instance;
            this.componentType = componentType;
        }

        public override bool ApplicableForChildContainer
        {
            get { return false; }
        }

        public override void Register(GenericApplicationContext context)
        {
            context.ObjectFactory.RegisterSingleton(componentType.FullName, instance);
        }
    }
}