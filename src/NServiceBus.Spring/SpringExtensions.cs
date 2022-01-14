namespace NServiceBus
{
    using System;
    using Container;
    using Spring.Context.Support;

    /// <summary>
    /// Spring extension to pass an existing Spring container instance.
    /// </summary>
    [ObsoleteEx(
        Message = "Support for external dependency injection containers is no longer provided by NServiceBus adapters for each container library. Instead, the NServiceBus.Extensions.DependencyInjection library provides the ability to use any container that conforms to the Microsoft.Extensions.DependencyInjection container abstraction.",
        RemoveInVersion = "10.0.0",
        TreatAsErrorFromVersion = "9.0.0")]
    public static class SpringExtensions
    {
        /// <summary>
        /// Use a pre-configured Spring application context
        /// </summary>
        /// <param name="customizations"></param>
        /// <param name="applicationContext">The existing application context.</param>
        [ObsoleteEx(
        Message = "Support for external dependency injection containers is no longer provided by NServiceBus adapters for each container library. Instead, the NServiceBus.Extensions.DependencyInjection library provides the ability to use any container that conforms to the Microsoft.Extensions.DependencyInjection container abstraction.",
        RemoveInVersion = "10.0.0",
        TreatAsErrorFromVersion = "9.0.0")]
        [CLSCompliant(false)]
        public static void ExistingApplicationContext(this ContainerCustomizations customizations, GenericApplicationContext applicationContext)
        {
            customizations.Settings.Set<SpringBuilder.ContextHolder>(new SpringBuilder.ContextHolder(applicationContext));
        }
    }
}