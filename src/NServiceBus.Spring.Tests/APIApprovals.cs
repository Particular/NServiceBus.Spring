using NServiceBus;
using NUnit.Framework;
using Particular.Approvals;
using PublicApiGenerator;

[TestFixture]
public class APIApprovals
{
    [Test]
    public void Approve()
    {
#pragma warning disable 618
        var publicApi = ApiGenerator.GeneratePublicApi(typeof(SpringBuilder).Assembly, excludeAttributes: new[] { "System.Reflection.AssemblyMetadataAttribute" });
#pragma warning restore 618
        Approver.Verify(publicApi);
    }
}