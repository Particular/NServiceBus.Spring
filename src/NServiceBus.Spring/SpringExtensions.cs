namespace NServiceBus
{
    using System;
    using Container;
    using Spring.Context.Support;

    /// <summary>
    /// Spring extension to pass an existing Spring container instance.
    /// </summary>
    public static class SpringExtensions
    {
        /// <summary>
        /// Use a pre-configured Spring application context
        /// </summary>
        /// <param name="customizations"></param>
        /// <param name="applicationContext">The existing application context.</param>
        [CLSCompliant(false)]
        [ObsoleteEx(Message = obsolete.Message,
            TreatAsErrorFromVersion = "9.0")]
        public static void ExistingApplicationContext(this ContainerCustomizations customizations, GenericApplicationContext applicationContext)
        {
            customizations.Settings.Set<SpringBuilder.ContextHolder>(new SpringBuilder.ContextHolder(applicationContext));
        }
    }
}