namespace NServiceBus
{
    using Container;
    using ObjectBuilder.Common;
    using ObjectBuilder.Spring;
    using Settings;
    using Spring.Context.Support;

    /// <summary>
    /// Spring Container
    /// </summary>
    [ObsoleteEx(
        Message = "Support for external dependency injection containers is no longer provided by NServiceBus adapters for each container library. Instead, the NServiceBus.Extensions.DependencyInjection library provides the ability to use any container that conforms to the Microsoft.Extensions.DependencyInjection container abstraction.",
        RemoveInVersion = "10.0.0",
        TreatAsErrorFromVersion = "9.0.0")]
    public class SpringBuilder : ContainerDefinition
    {
        /// <summary>
        /// Implementers need to new up a new container.
        /// </summary>
        /// <param name="settings">The settings to check if an existing container exists.</param>
        /// <returns>The new container wrapper.</returns>
        public override IContainer CreateContainer(ReadOnlySettings settings)
        {
            if (settings.TryGet(out ContextHolder contextHolder))
            {
                settings.AddStartupDiagnosticsSection("NServiceBus.Spring", new
                {
                    UsingExistingApplicationContext = true
                });

                return new SpringObjectBuilder(contextHolder.ExistingContext);
            }

            settings.AddStartupDiagnosticsSection("NServiceBus.Spring", new
            {
                UsingExistingApplicationContext = false
            });

            return new SpringObjectBuilder();
        }

        internal class ContextHolder
        {
            public ContextHolder(GenericApplicationContext context)
            {
                ExistingContext = context;
            }

            public GenericApplicationContext ExistingContext { get; }
        }
    }
}