using NServiceBus.ContainerTests;
using NServiceBus.ObjectBuilder.Spring;
using NUnit.Framework;

[SetUpFixture]
public class SetUpFixture
{
    public SetUpFixture()
    {
        TestContainerBuilder.ConstructBuilder = () => new SpringObjectBuilder();
    }
}