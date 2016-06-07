namespace NServiceBus.ObjectBuilder.Spring
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Common;
    using global::Spring.Context.Support;
    using global::Spring.Objects.Factory.Support;

    class SpringObjectBuilder : IContainer
    {
        public SpringObjectBuilder() : this(new GenericApplicationContext())
        {
        }

        public SpringObjectBuilder(GenericApplicationContext context)
        {
            this.context = context;
        }

        public void Dispose()
        {
            //Injected at compile time
        }

        public IContainer BuildChildContainer()
        {
            Init();

            var childContext = new GenericApplicationContext(context)
            {
                Name = $"child_of_{context.Name}"
            };

            return new SpringObjectBuilder(childContext)
            {
                isChildContainer = true,
                registrations = registrations,
                factory = factory
            };
        }

        public object Build(Type typeToBuild)
        {
            Init();
            var dict = context.GetObjectsOfType(typeToBuild, true, false);

            var de = dict.GetEnumerator();

            if (de.MoveNext())
            {
                return de.Current.Value;
            }

            var parentContext = context.ParentContext;
            if (parentContext != null)
            {
                dict = parentContext.GetObjectsOfType(typeToBuild, true, false);

                de = dict.GetEnumerator();

                if (de.MoveNext())
                {
                    return de.Current.Value;
                }
            }

            throw new ArgumentException($"{typeToBuild} has not been configured. In order to avoid this exception, check the return value of the 'HasComponent' method for this type.");
        }

        public IEnumerable<object> BuildAll(Type typeToBuild)
        {
            Init();
            var dict = context.GetObjectsOfType(typeToBuild, true, false);

            var de = dict.GetEnumerator();
            while (de.MoveNext())
            {
                yield return de.Current.Value;
            }
        }

        public void Configure(Type concreteComponent, DependencyLifecycle dependencyLifecycle)
        {
            ThrowIfAlreadyInitialized();

            if (registrations.ContainsKey(concreteComponent))
            {
                return;
            }

            registrations[concreteComponent] = new TypeRegistrationProxy(concreteComponent, dependencyLifecycle, factory);
        }

        public void Configure<T>(Func<T> componentFactory, DependencyLifecycle dependencyLifecycle)
        {
            ThrowIfAlreadyInitialized();

            var componentType = typeof(T);

            if (registrations.ContainsKey(componentType))
            {
                return;
            }
            registrations[componentType] = new ComponentFactoryRegistration<T>(componentFactory, dependencyLifecycle);
        }

        public void RegisterSingleton(Type lookupType, object instance)
        {
            ThrowIfAlreadyInitialized();
            var existing = registrations.Values.OfType<SingletonRegistration>()
                .SingleOrDefault(x => ReferenceEquals(x.Instance, instance));
            if (existing == null)
            {
                registrations[lookupType] = new SingletonRegistration(lookupType, instance);
            }
            else
            {
                existing.AppendAlias(lookupType);
            }
        }

        public bool HasComponent(Type componentType)
        {
            return registrations.Values.Any(_ => _.MatchesComponent(componentType));
        }

        public void Release(object instance)
        {
        }

        bool IsRootContainer => !isChildContainer;

        void DisposeManaged()
        {
            context?.Dispose();
        }

        void Init()
        {
            if (Interlocked.Exchange(ref initializeSignaled, 1) != 0)
            {
                return;
            }

            foreach (var action in new List<RegisterAction>(registrations.Values.Where(r => IsRootContainer || r.ApplicableForChildContainer)))
            {
                action.Register(context);
            }

            initialized = true;
            context.Refresh();
        }

        void ThrowIfAlreadyInitialized()
        {
            if (initialized)
            {
                throw new InvalidOperationException("Altering the registrations, after the container components has been resolved from the container, is not supported.");
            }
        }

        int initializeSignaled;
        GenericApplicationContext context;
        bool isChildContainer;
        Dictionary<Type, RegisterAction> registrations = new Dictionary<Type, RegisterAction>();
        bool initialized;
        DefaultObjectDefinitionFactory factory = new DefaultObjectDefinitionFactory();
    }
}