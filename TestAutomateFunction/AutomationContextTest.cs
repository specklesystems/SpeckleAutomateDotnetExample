# nullable enable
namespace TestAutomateFunction;

using Speckle.Automate.Sdk.Schema;
using Speckle.Automate.Sdk;
using Speckle.Core.Api;
using Speckle.Core.Credentials;
using Speckle.Core.Models;
using Speckle.Core.Transports;
using Utils = TestAutomateUtils;
using Env = TestAutomateEnvironment;

[TestFixture]
public sealed class AutomationContextTest : IDisposable
{
//   private async Task<AutomationRunData> AutomationRunData(Base testObject)
//   {
//     string projectId = await client.StreamCreate(new() { name = "Automate function e2e test" });
//     const string branchName = "main";

//     Branch model = await client.BranchGet(projectId, branchName, 1);
//     string modelId = model.id;

//     string rootObjId = await Operations.Send(
//       testObject,
//       new List<ITransport> { new ServerTransport(client.Account, projectId) }
//     );

//     string versionId = await client.CommitCreate(
//       new()
//       {
//         streamId = projectId,
//         objectId = rootObjId,
//         branchName = model.name
//       }
//     );

//     var automationName = TestAutomateUtils.RandomString(10);
//     var automationId = TestAutomateUtils.RandomString(10);
//     var automationRevisionId = TestAutomateUtils.RandomString(10);

//     await TestAutomateUtils.RegisterNewAutomation(projectId, modelId, client, automationId, automationName, automationRevisionId);

//     var automationRunId = TestAutomateUtils.RandomString(10);
//     var functionId = TestAutomateUtils.RandomString(10);
//     var functionName = "Automation name " + TestAutomateUtils.RandomString(10);
//     var functionRelease = TestAutomateUtils.RandomString(10);

//     return new AutomationRunData
//     {
//       ProjectId = projectId,
//       ModelId = modelId,
//       BranchName = branchName,
//       VersionId = versionId,
//       SpeckleServerUrl = client.ServerUrl,
//       AutomationId = automationId,
//       AutomationRevisionId = automationRevisionId,
//       AutomationRunId = automationRunId,
//       FunctionId = functionId,
//       FunctionName = functionName,
//       FunctionRelease = functionRelease,
//     };
//   }

  private Client client;
  private Account account;

  [OneTimeSetUp]
  public void Setup()
  {
    account = new Account
    {
      token = Env.GetSpeckleToken(),
      serverInfo = new ServerInfo { url = Env.GetSpeckleServerUrl()}
    };
    client = new Client(account);
  }

  [Test]
  public async Task TestFunctionRun()
  {
    var automationRunData = await AutomationRunData(TestAutomateUtils.TestObject());
    var automationContext = await AutomationRunner.RunFunction(
      AutomateFunction.Run,
      automationRunData,
      account.token,
      new FunctionInputs { SpeckleTypeToCount = "Base" }
    );

    Assert.That(automationContext.RunStatus, Is.EqualTo("SUCCEEDED"));
  }

  public void Dispose()
  {
    client.Dispose();
  }
}
