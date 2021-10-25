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