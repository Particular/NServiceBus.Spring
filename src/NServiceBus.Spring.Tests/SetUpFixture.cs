using NServiceBus.ContainerTests;
using NServiceBus.ObjectBuilder.Spring;
using NUnit.Framework;

[SetUpFixture]
public class SetUpFixture
{
    [SetUp]
    public void Setup()
    {
        TestContainerBuilder.ConstructBuilder = () => new SpringObjectBuilder();
    }

}