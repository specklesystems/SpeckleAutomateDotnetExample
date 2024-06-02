# nullable enable
namespace TestAutomateFunction;

using Speckle.Automate.Sdk;
using Speckle.Core.Api;
using Speckle.Core.Credentials;
using Utils = TestAutomateUtils;
using Env = TestAutomateEnvironment;

[TestFixture]
public sealed class AutomationContextTest : IDisposable
{

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
    var inputs = new FunctionInputs
    {
      SpeckleTypeToCount = "Base",
      SpeckleTypeTargetCount = 1
    };

    var automationRunData = await Utils.CreateTestRun(client);
    var automationContext = await AutomationRunner.RunFunction(
      AutomateFunction.Run,
      automationRunData,
      account.token,
      inputs
    );

    Assert.That(automationContext.RunStatus, Is.EqualTo("SUCCEEDED"));
  }

  public void Dispose()
  {
    client.Dispose();
    Env.Clear();
  }
}
