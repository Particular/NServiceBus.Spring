namespace NServiceBus.ObjectBuilder.Spring
{
    using System;
    using global::Spring.Context.Support;

    abstract class RegisterAction
    {
        public abstract bool ApplicableForChildContainer { get; }

        public abstract void Register(GenericApplicationContext context);
        public abstract bool MatchesComponent(Type type);
    }
}