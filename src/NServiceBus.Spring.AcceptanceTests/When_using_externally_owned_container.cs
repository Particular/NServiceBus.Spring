namespace NServiceBus.Spring.AcceptanceTests
{
    using System.Threading.Tasks;
    using AcceptanceTesting;
    using NServiceBus.AcceptanceTests;
    using NServiceBus.AcceptanceTests.EndpointTemplates;
    using NUnit.Framework;

    public class When_using_externally_owned_container : NServiceBusAcceptanceTest
    {
        [Test]
        public async Task Should_shutdown_properly()
        {
            var context = await Scenario.Define<Context>()
                .WithEndpoint<Endpoint>()
                .Done(c => c.EndpointsStarted)
                .Run();

            Assert.IsFalse(context.ApplicationContext.Disposed);
            Assert.DoesNotThrow(() => context.ApplicationContext.Dispose());
        }

        class Context : ScenarioContext
        {
            public SpecialGenericApplicationContext ApplicationContext { get; set; }
        }

        class Endpoint : EndpointConfigurationBuilder
        {
            public Endpoint()
            {
                EndpointSetup<DefaultServer>((config, desc) =>
                {
                    var genericContext = new SpecialGenericApplicationContext();

                    config.SendFailedMessagesTo("error");
#pragma warning disable 618
                    config.UseContainer<SpringBuilder>(c => c.ExistingApplicationContext(genericContext));
#pragma warning restore 618
                    var context = (Context)desc.ScenarioContext;
                    context.ApplicationContext = genericContext;
                });
            }
        }
    }
}