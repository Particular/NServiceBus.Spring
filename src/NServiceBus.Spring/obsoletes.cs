namespace NServiceBus
{
    // An internal type referenced by the API approvals test as it can't reference obsoleted types.
    class SpringInternalType
    {
    }

    /// <summary>
    /// Spring Container
    /// </summary>
    [ObsoleteEx(
        Message = "Spring is no longer supported via the NServiceBus.Spring adapter. NServiceBus directly supports all containers compatible with Microsoft.Extensions.DependencyInjection.Abstractions via the externally managed container mode.",
        RemoveInVersion = "10.0.0",
        TreatAsErrorFromVersion = "9.0.0")]
    public class SpringBuilder
    {
    }

    /// <summary>
    /// Spring extension to pass an existing Spring container instance.
    /// </summary>
    [ObsoleteEx(
        Message = "Spring is no longer supported via the NServiceBus.Spring adapter. NServiceBus directly supports all containers compatible with Microsoft.Extensions.DependencyInjection.Abstractions via the externally managed container mode.",
        RemoveInVersion = "10.0.0",
        TreatAsErrorFromVersion = "9.0.0")]
    public static class SpringExtensions
    {
    }
}