namespace NServiceBus.Features
{
    /// <summary>
    /// Adds Diagnostics information
    /// </summary>
    public class SpringDiagnostics : Feature
    {
        /// <summary>
        /// Constructor for diagnostics feature
        /// </summary>
        public SpringDiagnostics()
        {
            EnableByDefault();
        }

        /// <summary>
        /// Sets up diagnostics
        /// </summary>
        protected override void Setup(FeatureConfigurationContext context)
        {
            context.S
        }
    }
}
