namespace NServiceBus.Spring.Tests
{
    using System;
    using System.Threading.Tasks;
    using ContainerTests;
    using NUnit.Framework;

    [TestFixture]
    public class When_using_nested_containers_including_reconfiguration
    {
        [Test]
        public void it_just_works()
        {
            ConventionalBusDependency.DisposeCalled = 0;
            ConventionalBusDependency.InstanceCounter = 0;

            using (var builder = TestContainerBuilder.ConstructBuilder())
            {
                builder.Configure(typeof(RealBus), DependencyLifecycle.SingleInstance);
                var outerBus = builder.Build(typeof(IRealBus));

                Assert.IsInstanceOf<RealBus>(outerBus);

                var t1 = Task.Run(() =>
                {
                    using (var nestedContainer = builder.BuildChildContainer())
                    {
                        nestedContainer.Configure(typeof(ConventionalBusDependency), DependencyLifecycle.InstancePerUnitOfWork);
                        nestedContainer.Configure(typeof(ConventionalBus), DependencyLifecycle.InstancePerUnitOfWork);

                        var bus1 = nestedContainer.Build(typeof(IRealBus));
                        var bus2 = nestedContainer.Build(typeof(IRealBus));

                        Assert.IsInstanceOf<ConventionalBus>(bus1);
                        Assert.IsInstanceOf<ConventionalBus>(bus2);

                        Assert.AreSame(bus1, bus2);
                    }
                });

                var t2 = Task.Run(() =>
                {
                    using (var nestedContainer = builder.BuildChildContainer())
                    {
                        nestedContainer.Configure(typeof(ConventionalBusDependency), DependencyLifecycle.InstancePerUnitOfWork);
                        nestedContainer.Configure(typeof(ConventionalBus), DependencyLifecycle.InstancePerUnitOfWork);

                        var bus1 = nestedContainer.Build(typeof(IRealBus));
                        var bus2 = nestedContainer.Build(typeof(IRealBus));

                        Assert.IsInstanceOf<ConventionalBus>(bus1);
                        Assert.IsInstanceOf<ConventionalBus>(bus2);

                        Assert.AreSame(bus1, bus2);
                    }
                });

                Task.WaitAll(t1, t2);

                Assert.AreEqual(2, ConventionalBusDependency.DisposeCalled);
                Assert.AreEqual(2, ConventionalBusDependency.InstanceCounter);
            }
        }

        interface IRealBus
        {
        }

        class ConventionalBusDependency : IDisposable
        {
            public ConventionalBusDependency()
            {
                InstanceCounter++;
            }

            public void Dispose()
            {
                DisposeCalled++;
            }

            public static int InstanceCounter;

            public static int DisposeCalled;
        }

        class RealBus : IRealBus
        {
        }

        class ConventionalBus : IRealBus
        {
            public ConventionalBus(ConventionalBusDependency dependency)
            {
                conventionalBusDependency = dependency;
            }

            // ReSharper disable once NotAccessedField.Local
            readonly ConventionalBusDependency conventionalBusDependency;
        }
    }
}