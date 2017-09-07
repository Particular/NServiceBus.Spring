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
    [ObsoleteEx(Message = obsolete.Message,
        TreatAsErrorFromVersion = "9.0")]
    public class SpringBuilder : ContainerDefinition
    {
        /// <summary>
        /// Implementers need to new up a new container.
        /// </summary>
        /// <param name="settings">The settings to check if an existing container exists.</param>
        /// <returns>The new container wrapper.</returns>
        [ObsoleteEx(Message = obsolete.Message,
            TreatAsErrorFromVersion = "9.0")]
        public override IContainer CreateContainer(ReadOnlySettings settings)
        {
            ContextHolder contextHolder;

            if (settings.TryGet(out contextHolder))
            {
                return new SpringObjectBuilder(contextHolder.ExistingContext);
            }

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