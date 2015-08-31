namespace NServiceBus.ObjectBuilder.Spring
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using global::Spring.Context.Support;
    using global::Spring.Objects.Factory.Support;
    using NServiceBus.ObjectBuilder.Common;

    /// <summary>
    /// Implementation of <see cref="IContainer"/> using the Spring Framework container
    /// </summary>
    class SpringObjectBuilder : IContainer
    {
        int intializedSignaled;
        GenericApplicationContext context;
        bool isChildContainer;
        Dictionary<Type, RegisterAction> registrations = new Dictionary<Type, RegisterAction>();
        Dictionary<Type, ComponentConfig> componentProperties = new Dictionary<Type, ComponentConfig>();
        bool initialized;
        DefaultObjectDefinitionFactory factory = new DefaultObjectDefinitionFactory();

        /// <summary>
        /// Instantiates the builder using a new <see cref="GenericApplicationContext"/>.
        /// </summary>
        public SpringObjectBuilder()
            : this(new GenericApplicationContext())
        {
        }

        /// <summary>
        /// Instantiates the builder using the given container.
        /// </summary>
        public SpringObjectBuilder(GenericApplicationContext context)
        {
            this.context = context;
        }

        public void Dispose()
        {
            //Injected at compile time
        }

        void DisposeManaged()
        {
            if (context != null)
            {
                context.Dispose();
            }
        }

        public IContainer BuildChildContainer()
        {
            Init();

            var childContext = new GenericApplicationContext(context)
            {
                Name = string.Format("child_of_{0}", context.Name)
            };

            return new SpringObjectBuilder(childContext)
                   {
                       isChildContainer = true,
                       componentProperties = componentProperties,
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

            throw new ArgumentException(string.Format("{0} has not been configured. In order to avoid this exception, check the return value of the 'HasComponent' method for this type.", typeToBuild));
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

            if (!componentProperties.ContainsKey(concreteComponent))
            {
                componentProperties[concreteComponent] = new ComponentConfig();
            }

            registrations[concreteComponent] = new TypeRegistrationProxy(concreteComponent, componentProperties[concreteComponent], dependencyLifecycle, factory);
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

        public void ConfigureProperty(Type concreteComponent, string property, object value)
        {
            ThrowIfAlreadyInitialized();

            var componentConfig = new Dictionary<Type, ComponentConfig>(componentProperties);
            ComponentConfig result;
            componentConfig.TryGetValue(concreteComponent, out result);

            if (result == null)
            {
                throw new InvalidOperationException("Cannot configure property before the component has been configured. Please call 'Configure' first.");
            }

            result.ConfigureProperty(property, value);
        }

        public void RegisterSingleton(Type lookupType, object instance)
        {
            ThrowIfAlreadyInitialized();

            registrations[lookupType] = new SingletonRegistration(lookupType, instance);
        }

        public bool HasComponent(Type componentType)
        {
            var registeredTypes = new List<Type>(registrations.Keys);

            return registeredTypes.Any(componentType.IsAssignableFrom);
        }

        public void Release(object instance)
        {
        }

        void Init()
        {
            if (Interlocked.Exchange(ref intializedSignaled, 1) != 0)
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

        bool IsRootContainer
        {
            get { return !isChildContainer; }
        }

        void ThrowIfAlreadyInitialized()
        {
            if (initialized)
            {
                throw new InvalidOperationException("You can't alter the registrations after the container components has been resolved from the container");
            }
        }
    }
}
