namespace NServiceBus.ObjectBuilder.Spring
{
    using global::Spring.Context.Support;

    abstract class RegisterAction
    {
        public abstract bool ApplicableForChildContainer { get; }

        public abstract void Register(GenericApplicationContext context);
    }
}