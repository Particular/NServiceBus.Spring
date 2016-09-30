namespace NServiceBus.AcceptanceTests
{
    using NServiceBus.AcceptanceTesting;
    using NServiceBus.AcceptanceTests.EndpointTemplates;
    using NUnit.Framework;

    public class When_using_externally_owned_container : NServiceBusAcceptanceTest
    {
        [Test]
        public void Should_shutdown_properly()
        {
            Scenario.Define<Context>()
                .WithEndpoint<Endpoint>()
                .Done(c => c.EndpointsStarted)
                .Run();

            Assert.IsFalse(Endpoint.Context.ApplicationContext.Disposed);
            Assert.DoesNotThrow(() => Endpoint.Context.ApplicationContext.Dispose());
        }

        class Context : ScenarioContext
        {
            public SpecialGenericApplicationContext ApplicationContext { get; set; }
        }

        class Endpoint : EndpointConfigurationBuilder
        {
            public static Context Context { get; set; }
            public Endpoint()
            {
                EndpointSetup<DefaultServer>(config =>
                {
                    Context = new Context();

                    var genericContext = new SpecialGenericApplicationContext();

                    Context.ApplicationContext = genericContext;

                    config.UseContainer<SpringBuilder>(c => c.ExistingApplicationContext(genericContext));
                });
            }
        }
    }
}