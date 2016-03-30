namespace NServiceBus.ObjectBuilder.Spring
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::Spring.Context.Support;

    class SingletonRegistration : RegisterAction
    {
        public SingletonRegistration(Type componentType, object instance)
        {
            Instance = instance;
            componentTypes.Add(componentType);
        }

        public override bool ApplicableForChildContainer => false;

        public void AppendAlias(Type componentType)
        {
            componentTypes.Add(componentType);
        }

        public override void Register(GenericApplicationContext context)
        {
            var first = componentTypes.First();
            context.ObjectFactory.RegisterSingleton(first.FullName, Instance);
            foreach (var componentType in componentTypes.Skip(1))
            {
                context.ObjectFactory.RegisterAlias(first.FullName, componentType.FullName);
            }
        }

        public override bool MatchesComponent(Type type)
        {
            return componentTypes.Any(type.IsAssignableFrom);
        }

        public readonly object Instance;
        List<Type> componentTypes = new List<Type>();
    }
}