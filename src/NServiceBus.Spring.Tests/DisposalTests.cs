namespace NServiceBus.Spring.Tests
{
    using global::Spring.Context.Support;
    using NUnit.Framework;
    using ObjectBuilder.Spring;

    [TestFixture]
    public class DisposalTests
    {
        [Test]
        public void Owned_container_should_be_disposed()
        {
            var fakeContainer = new SpecialGenericApplicationContext();

            var container = new SpringObjectBuilder(fakeContainer, true);
            container.Dispose();

            Assert.True(fakeContainer.Disposed);
        }

        [Test]
        public void Externally_owned_container_should_not_be_disposed()
        {
            var fakeContainer = new SpecialGenericApplicationContext();

            var container = new SpringObjectBuilder(fakeContainer, false);
            container.Dispose();

            Assert.False(fakeContainer.Disposed);
        }

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
}