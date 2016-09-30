namespace NServiceBus.AcceptanceTests
{
    using global::Spring.Context.Support;

    class SpecialGenericApplicationContext : GenericApplicationContext
    {
        public bool Disposed { get; private set; }

        public override void Dispose()
        {
            base.Dispose();
            Disposed = true;
        }
    }
}