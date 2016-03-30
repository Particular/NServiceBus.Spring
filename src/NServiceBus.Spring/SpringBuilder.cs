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
            GenericApplicationContext existingContext;

            if (settings.TryGet("ExistingContext", out existingContext))
            {
                return new SpringObjectBuilder(existingContext);
            }

            return new SpringObjectBuilder();
        }
    }
}